import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface UserGamification {
  id?: number;
  userId: number;
  xp: number;
  level: number;
  userName: String
}

export interface UserBadge {
  badgeId: number;
  name: string;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class GamificationService {
  private apiUrl = environment.apiUrl + '/Gamification';

  constructor(private http: HttpClient) {}

  // Get the gamification data (XP and level) for a given user
  getUserGamification(): Observable<UserGamification> {
    return this.http.get<UserGamification>(`${this.apiUrl}/user`);
  }

  // Get leaderboard data (Top users by XP)
getLeaderboard(): Observable<UserGamification[]> {
  return this.http.get<UserGamification[]>(`${this.apiUrl}/leaderboard`);
}

  // Get all available badges
  getBadges(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/badges`);
  }

  // Get earned badges for the user
  getUserBadges(): Observable<UserBadge[]> {
    return this.http.get<UserBadge[]>(`${this.apiUrl}/user-badges`);
  }

}
