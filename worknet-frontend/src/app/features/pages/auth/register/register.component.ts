import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  name = '';
  email = '';
  password = '';
  confirmPassword = '';
  termsAccepted = false;
  errorMessage = '';
  successMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

    onSubmit() {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    if (!this.termsAccepted) {
      this.errorMessage = 'You must accept the terms.';
      return;
    }

    this.authService
      .register({
        userName: this.name,
        userEmail: this.email,
        password: this.password,
      })
      .subscribe({
        next: () => {
          this.successMessage = 'Account created successfully!';
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Registration failed';
        },
      });
  }
}
