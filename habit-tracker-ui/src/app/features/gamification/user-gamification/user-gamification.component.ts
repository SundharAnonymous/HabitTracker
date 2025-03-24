import { Component, OnInit } from '@angular/core';
import { GamificationService, UserGamification, UserBadge } from '../gamification.service';

@Component({
  selector: 'app-user-gamification',
  templateUrl: './user-gamification.component.html',
  styleUrls: ['./user-gamification.component.scss']
})
export class UserGamificationComponent implements OnInit {
  gamificationData!: UserGamification;
  badges: UserBadge[] = [];

  constructor(private gamificationService: GamificationService) {}

  ngOnInit(): void {
    this.loadGamificationData();
  }

  loadGamificationData(): void {
    this.gamificationService.getUserGamification().subscribe({
      next: data => this.gamificationData = data,
      error: err => console.error('Error fetching gamification data', err)
    });

    this.gamificationService.getUserBadges().subscribe({
      next: data => this.badges = data,
      error: err => console.error('Error fetching user badges', err)
    });
  }
}
