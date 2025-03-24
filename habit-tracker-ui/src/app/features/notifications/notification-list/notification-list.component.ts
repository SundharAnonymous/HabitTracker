import { Component, OnInit } from '@angular/core';
import { NotificationService, Notification } from '../notification.service';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.scss']
})
export class NotificationListComponent implements OnInit {
  notifications: Notification[] = [];
  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.loadNotifications();
  }

  loadNotifications(): void {
    this.notificationService.getUserNotifications().subscribe({
      next: (data) => this.notifications = data,
      error: (err) => console.error('Error loading notifications', err)
    });
  }

  deleteNotification(id?: number): void {
    if (id) {
      this.notificationService.deleteNotification(id).subscribe({
        next: () => this.notifications = this.notifications.filter(n => n.id !== id),
        error: (err) => console.error('Error deleting notification', err)
      });
    }
  }
}
