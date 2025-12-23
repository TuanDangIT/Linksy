import { Component, inject, signal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-login',
  imports: [FontAwesomeModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private authService = inject(AuthService);
  private router = inject(Router);
  faLock = faLock;
  error = signal('');

  onLogin(form: NgForm) {
    if (form.invalid) {
      return;
    }

    const { email, password } = form.value;

    this.authService.login(email, password).subscribe({
      next: () => {
        // this.isLoading.set(false);
        this.router.navigate(['/shortened-urls']);
      },
      error: (error) => {
        // this.isLoading.set(false);
        this.error.set(error.message || 'Login failed. Please check your credentials and try again.');
      }
    });
  }
}
