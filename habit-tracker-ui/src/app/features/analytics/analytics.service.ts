import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface AnalyticsData {
  id?: number;
  userId: number;
  habitId: number;
  progressData: string; // e.g., JSON string with details like completedDays, missedDays, etc.
  lastUpdated: Date;
  habitName: string;
}

@Injectable({
  providedIn: 'root'
})
export class AnalyticsService {
  private apiUrl = environment.apiUrl + '/Analytics';

  constructor(private http: HttpClient) {}

  // Retrieve all analytics data for a given user
  getUserAnalytics(): Observable<AnalyticsData[]> {
    return this.http.get<AnalyticsData[]>(`${this.apiUrl}/user/`);
  }

  // Retrieve analytics data for a specific habit
  getHabitAnalytics(habitId: number): Observable<AnalyticsData> {
    return this.http.get<AnalyticsData>(`${this.apiUrl}/habit/${habitId}`);
  }

  // Add or update analytics data
  updateAnalytics(updateData: { userId: number; habitId: number; progressData: string }): Observable<AnalyticsData> {
    return this.http.post<AnalyticsData>(`${this.apiUrl}/update`, updateData);
  }
}
