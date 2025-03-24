import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface Habit {
  id: number;
  userId: number;
  title: string;
  description: string;
  frequency: string; // e.g., 'Daily', 'Weekly'
  startDate: Date;
  endDate?: Date;
  reminderTime?: string; // Format "HH:mm:ss"
  isCompletedToday?: boolean; // Added for tracking today's completion status
}

export interface HabitCompletion {
  habitId: number;
  isCompleted: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class HabitService {
  private apiUrl = environment.apiUrl + '/Habits';

  constructor(private http: HttpClient) {}

  getUserHabits(): Observable<Habit[]> {
    return this.http.get<Habit[]>(`${this.apiUrl}/user/`);
  }

  getHabitById(id: number): Observable<Habit> {
    return this.http.get<Habit>(`${this.apiUrl}/${id}`);
  }

  createHabit(habit: Habit): Observable<Habit> {
    return this.http.post<Habit>(`${this.apiUrl}/create`, habit);
  }

  updateHabit(habit: Habit): Observable<Habit> {
    return this.http.put<Habit>(`${this.apiUrl}/update`, habit);
  }

  deleteHabit(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }

  // New method: Mark habit as completed/uncompleted for today
  markHabitCompletion(completion: HabitCompletion): Observable<any> {
    return this.http.post(`${this.apiUrl}/mark-completion`, completion);
  }

  // New method: Get today's completion status for a habit.
  getTodayCompletion(habitId: number): Observable<{ isCompleted: boolean }> {
    return this.http.get<{ isCompleted: boolean }>(`${this.apiUrl}/today-completion/${habitId}`);
  }
}
