import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HabitsRoutingModule } from './habits-routing.module';
import { HabitListComponent } from './habit-list/habit-list.component';
import { HabitFormComponent } from './habit-form/habit-form.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    HabitListComponent,
    HabitFormComponent
  ],
  imports: [
    CommonModule,
    HabitsRoutingModule,
    SharedModule
  ]
})
export class HabitsModule { }
