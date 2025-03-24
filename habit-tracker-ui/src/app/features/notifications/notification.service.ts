import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface Notification {
  id?: number;
  message: string;
  scheduledTime: string;
  isRead: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = environment.apiUrl + '/Notifications';

  constructor(private http: HttpClient) {}

  // Retrieve all notifications for a given user
  getUserNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/user/`);
  }

  // Create a new notification
  createNotification(notification: Notification): Observable<Notification> {
    return this.http.post<Notification>(`${this.apiUrl}/create`, notification);
  }

  // Update an existing notification
  updateNotification(notification: Notification): Observable<Notification> {
    return this.http.put<Notification>(`${this.apiUrl}/update`, notification);
  }

  // Delete a notification by ID
  deleteNotification(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}
