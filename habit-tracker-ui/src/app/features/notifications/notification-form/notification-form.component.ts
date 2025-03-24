import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificationService, Notification } from '../notification.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-notification-form',
  templateUrl: './notification-form.component.html',
  styleUrls: ['./notification-form.component.scss']
})
export class NotificationFormComponent implements OnInit {
  notificationForm!: FormGroup;
  notificationId?: number;

  constructor(
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.notificationForm = this.fb.group({
  message: ['', Validators.required],
  scheduledDate: ['', Validators.required],
  scheduledTime: ['', Validators.required]
});


    // Check if editing an existing notification (via route parameter)
    this.notificationId = this.route.snapshot.params['id'];
    if (this.notificationId) {
      // Optionally, load the existing notification data here
      // This example assumes that you'll implement an API endpoint to fetch by ID
    }
  }

  onSubmit(): void {
    if (this.notificationForm.valid) {
      const dateValue = this.notificationForm.get('scheduledDate')?.value;
      const timeValue = this.notificationForm.get('scheduledTime')?.value;

      if (dateValue && timeValue) {
        // Convert dateValue to a Date object.
        const scheduledDate = new Date(dateValue);
        
        // Split the time string into hours, minutes, and optional seconds.
        const timeParts = timeValue.split(':');
        const hours = parseInt(timeParts[0], 10);
        const minutes = parseInt(timeParts[1], 10);
        const seconds = timeParts.length > 2 ? parseInt(timeParts[2], 10) : 0;

        // Set the hours, minutes, and seconds on the date.
        scheduledDate.setHours(hours, minutes, seconds);

        const notification: Notification = {
          message: this.notificationForm.get('message')?.value,
          scheduledTime: scheduledDate.toISOString(),  // Converts to a standard ISO string.
          isRead: true
        };
      if (this.notificationId) {
        // If updating, assign the ID and call update API
        notification.id = this.notificationId;
        this.notificationService.updateNotification(notification).subscribe({
          next: () => this.router.navigate(['/notifications']),
          error: err => console.error('Error updating notification', err)
        });
      } else {
        // Creating a new notification
        this.notificationService.createNotification(notification).subscribe({
          next: () => this.router.navigate(['/notifications']),
          error: err => console.error('Error creating notification', err)
        });
      }
    }
  }
}
}
