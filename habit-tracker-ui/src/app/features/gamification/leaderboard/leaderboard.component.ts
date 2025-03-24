import { Component, OnInit } from '@angular/core';
import { GamificationService, UserGamification } from '../gamification.service';

@Component({
  selector: 'app-leaderboard',
  templateUrl: './leaderboard.component.html',
  styleUrls: ['./leaderboard.component.scss']
})
export class LeaderboardComponent implements OnInit {
  leaderboard: UserGamification[] = [];

  constructor(private gamificationService: GamificationService) {}

  ngOnInit(): void {
    this.gamificationService.getLeaderboard().subscribe({
      next: data => this.leaderboard = data,
      error: err => console.error('Error fetching leaderboard data', err)
    });
  }
}
