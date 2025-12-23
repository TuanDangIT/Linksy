import { Component, inject, signal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-register',
  imports: [FontAwesomeModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  faUserPlus = faUserPlus;
  error = signal('');
  passwordMismatch = signal(false);
  authService = inject(AuthService);
  router = inject(Router);
  onRegister(form: NgForm) {
    if (form.invalid) {
      return;
    }

    const registerData = {
      email: form.value.email,
      firstName: form.value.firstName,
      lastName: form.value.lastName,
      gender: form.value.gender,
      username: form.value.username,
      password: form.value.password,
      confirmPassword: form.value.confirmPassword
    };

    this.authService.register(registerData).subscribe({
      next: () => {
        // this.isLoading.set(false);
        this.router.navigate(['/shortened-urls']);
      },
      error: (error) => {
        // this.isLoading.set(false);
        this.error.set(error.message || 'Registration failed. Please try again.');
      }
    });
  }
}
