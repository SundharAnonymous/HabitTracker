import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { AnalyticsService, AnalyticsData } from '../analytics.service';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})
export class ReportComponent implements OnInit {
  analyticsData: AnalyticsData[] = [];
  @ViewChild('analyticsChart', { static: true }) analyticsChart!: ElementRef;
  chart!: Chart;

  constructor(private analyticsService: AnalyticsService) {
    Chart.register(...registerables); // ✅ Fix for Chart.js v3+
  }

  ngOnInit(): void {
    this.loadAnalytics();
  }

  loadAnalytics(): void {
    this.analyticsService.getUserAnalytics().subscribe({
      next: (data) => {
        this.analyticsData = data;
        this.initializeChart(); // ✅ Update chart after data load
      },
      error: err => console.error('Error loading analytics data', err)
    });
  }

  parseProgressData(data: string): { completedDays: number; missedDays: number } {
    try {
      return JSON.parse(data) || { completedDays: 0, missedDays: 0 };
    } catch (error) {
      console.error("Error parsing progressData", error);
      return { completedDays: 0, missedDays: 0 };
    }
  }

  initializeChart(): void {
    if (this.analyticsData.length === 0) return;

    // ✅ Use `habitName` instead of `habitId`
    const labels = this.analyticsData.map(d => d.habitName || `Habit ${d.habitId}`);
    const completedDays = this.analyticsData.map(d => this.parseProgressData(d.progressData).completedDays);
    const missedDays = this.analyticsData.map(d => this.parseProgressData(d.progressData).missedDays);

    // ✅ Destroy existing chart instance if already created
    if (this.chart) {
      this.chart.destroy();
    }

    this.chart = new Chart(this.analyticsChart.nativeElement, {
      type: 'bar',
      data: {
        labels: labels, // ✅ Now showing habit names
        datasets: [
          {
            label: 'Completed Days',
            data: completedDays,
            backgroundColor: '#4CAF50'
          },
          {
            label: 'Missed Days',
            data: missedDays,
            backgroundColor: '#F44336'
          }
        ]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            display: true
          }
        },
        scales: {
          x: {
            ticks: { color: "#fff" }
          },
          y: {
            ticks: { color: "#fff" }
          }
        }
      }
    });
  }
}
