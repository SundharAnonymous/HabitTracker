import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  userId: number = 1; // For demonstration, assume the user ID is 1
  errorMessage = '';
  successMessage = '';

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit(): void {
    // Initialize the form with disabled email field (assuming email should not be changed)
    this.profileForm = this.fb.group({
      email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
      fullName: ['', Validators.required]
    });
    this.loadProfile();
  }

  // Load the user profile from the API
  loadProfile(): void {
    this.http.get<any>(`${environment.apiUrl}/Users/profile/${this.userId}`)
      .subscribe({
        next: data => {
          this.profileForm.patchValue({
            email: data.email,
            fullName: data.fullName
          });
        },
        error: err => this.errorMessage = 'Error loading profile. Please try again.'
      });
  }

  // Submit updated profile data to the API
  onSubmit(): void {
    if (this.profileForm.valid) {
      const updateData = {
        fullName: this.profileForm.get('fullName')?.value
      };
      this.http.put(`${environment.apiUrl}/Users/update-profile/${this.userId}`, updateData)
        .subscribe({
          next: () => this.successMessage = 'Profile updated successfully.',
          error: () => this.errorMessage = 'Error updating profile. Please try again.'
        });
    }
  }
}
