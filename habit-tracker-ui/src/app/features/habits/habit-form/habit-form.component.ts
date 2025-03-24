import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HabitService, Habit } from '../habit.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-habit-form',
  templateUrl: './habit-form.component.html',
  styleUrls: ['./habit-form.component.scss']
})
export class HabitFormComponent implements OnInit {
  habitForm!: FormGroup;
  habitId?: number;
  userId = 1; // For demonstration, assume userId = 1

  constructor(
    private fb: FormBuilder,
    private habitService: HabitService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.habitForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      frequency: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: [''],
      reminderTime: ['12:30:00.0000000']
    });

    // Check if editing an existing habit
    this.habitId = this.route.snapshot.params['id'];
    if (this.habitId) {
      this.habitService.getHabitById(this.habitId).subscribe({
        next: (habit) => this.habitForm.patchValue(habit),
        error: (err) => console.error('Error loading habit', err)
      });
    }
  }

  onSubmit(): void {
    if (this.habitForm.valid) {
      const habit: Habit = { ...this.habitForm.value, userId: this.userId };
      if (this.habitId) {
        habit.id = this.habitId;
        this.habitService.updateHabit(habit).subscribe({
          next: () => this.router.navigate(['/habits']),
          error: (err) => console.error('Error updating habit', err)
        });
      } else {
        this.habitService.createHabit(habit).subscribe({
          next: () => this.router.navigate(['/habits']),
          error: (err) => console.error('Error creating habit', err)
        });
      }
    }
  }
}
