import { Component, OnInit } from '@angular/core';
import { HabitService, Habit } from '../habit.service';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialog } from 'src/app/shared/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-habit-list',
  templateUrl: './habit-list.component.html',
  styleUrls: ['./habit-list.component.scss']
})
export class HabitListComponent implements OnInit {
  habits: Habit[] = [];
  completionForm: FormGroup;

  constructor(private habitService: HabitService, private fb: FormBuilder, private dialog: MatDialog) {
    this.completionForm = this.fb.group({});
  }

  ngOnInit(): void {
    this.loadHabits();
  }

  loadHabits(): void {
    this.habitService.getUserHabits().subscribe({
      next: (data) => {
        this.habits = data;
        this.habits.forEach(habit => {
          this.completionForm.addControl(habit.id.toString(), new FormControl(false));

          this.habitService.getTodayCompletion(habit.id).subscribe({
            next: (status: any) => {
              this.completionForm.controls[habit.id.toString()].setValue(status.isCompleted);
              if (status.isCompleted) {
                this.completionForm.controls[habit.id.toString()].disable(); // Disable checkbox if already completed
              }
            },
            error: err => console.error("Error fetching completion status", habit.id, err)
          });
        });
      },
      error: (err) => console.error('Error loading habits', err)
    });
  }

  onDeleteHabit(id?: number): void {
    if (id) {
      this.habitService.deleteHabit(id).subscribe({
        next: () => {
          this.habits = this.habits.filter(h => h.id !== id);
          this.completionForm.removeControl(id.toString());
        },
        error: (err) => console.error('Error deleting habit', err)
      });
    }
  }

  markCompletion(habitId: number): void {
    const isCompleted = this.completionForm.controls[habitId.toString()].value;

    const dialogRef = this.dialog.open(ConfirmationDialog, {
      width: '300px',
      data: { message: `Are you sure you want to mark this habit as completed?` }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const payload = {
          habitId: habitId,
          isCompleted: isCompleted
        };

        this.habitService.markHabitCompletion(payload).subscribe({
          next: () => {
            this.completionForm.controls[habitId.toString()].disable(); // Disable after marking as completed
            console.log("Habit completion updated");
          },
          error: err => console.error("Error marking habit completion", err)
        });
      } else {
        this.completionForm.controls[habitId.toString()].setValue(!isCompleted); // Revert checkbox if canceled
      }
    });
  }
}
