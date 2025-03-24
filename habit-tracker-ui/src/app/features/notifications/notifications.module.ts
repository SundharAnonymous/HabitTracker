import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NotificationsRoutingModule } from './notifications-routing.module';
import { NotificationListComponent } from './notification-list/notification-list.component';
import { NotificationFormComponent } from './notification-form/notification-form.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    NotificationListComponent,
    NotificationFormComponent
  ],
  imports: [
    CommonModule,
    NotificationsRoutingModule,
    SharedModule
  ]
})
export class NotificationsModule { }
