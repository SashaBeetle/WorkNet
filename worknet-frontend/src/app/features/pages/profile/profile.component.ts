import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})

export class ProfileComponent {
  gender = '';
  userType = ''; // HR or JobSeeker
  location = '';
  title = '';
  name = '';
  surname = '';
  about = '';
  skills = '';
  experience = '';
  education = '';

  successMessage = '';
  errorMessage = '';
  onSubmit() {
    const profileData = {
      gender: this.gender,
      userType: this.userType,
      location: this.location,
      title: this.title,
      about: this.about,
      skills: this.skills?.split(',').map(s => s.trim()),
      experience: this.experience,
      education: this.education,
      name: this.name,
      surname: this.surname
    };

    this.apiService.put('/api/profile', profileData).subscribe({
      next: () => {
        this.successMessage = 'Profile saved successfully!';
        this.errorMessage = '';
      },
      error: err => {
        this.errorMessage = err?.error?.message || 'Failed to save profile.';
        this.successMessage = '';
      }
    });;
  }

    constructor(private apiService: ApiService) {}

}
