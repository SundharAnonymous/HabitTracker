import { Component, OnInit } from '@angular/core';
import { AiService, AIRecommendation } from '../ai.service';

@Component({
  selector: 'app-recommendation',
  templateUrl: './recommendation.component.html',
  styleUrls: ['./recommendation.component.scss']
})
export class RecommendationComponent implements OnInit {
  recommendation: string = '';

  constructor(private aiService: AiService) { }

  ngOnInit(): void {
    this.fetchRecommendation();
  }

  fetchRecommendation(): void {
    this.aiService.getRecommendations().subscribe({
      next: (data: AIRecommendation) => {
        console.log(data.recommendation)
        // Assume data.recommendation is received in markdown format.
        // Parse the markdown into HTML.
        this.recommendation = this.markdownToHtml(data.recommendation);
      },
      error: (err) => {
        console.error('Error fetching recommendation:', err);
        this.recommendation = '<p>No recommendation available at this time.</p>';
      }
    });
  }

  // Simple markdown-to-HTML parser
  markdownToHtml(markdown: string): string {
    // Convert headings (###, ##, #)
    markdown = markdown.replace(/^### (.*$)/gim, '<h3>$1</h3>');
    markdown = markdown.replace(/^## (.*$)/gim, '<h2>$1</h2>');
    markdown = markdown.replace(/^# (.*$)/gim, '<h1>$1</h1>');
    // Convert bold text
    markdown = markdown.replace(/\*\*(.*?)\*\*/gim, '<strong>$1</strong>');
    // Convert list items (assumes each list item starts with a hyphen)
    markdown = markdown.replace(/^- (.*$)/gim, '<li>$1</li>');
    // Optionally wrap list items in <ul> tags if the markdown contains them.
    // For simplicity, this example assumes the markdown output already separates list sections.
    // Replace line breaks with <br> if needed.
    markdown = markdown.replace(/\n$/gim, '<br>');
    return markdown.trim();
  }
}
