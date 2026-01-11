import { Component, inject, signal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';
import { RegisterRequest, Gender } from '../../../core/types/registerRequest';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { toErrorList } from '../../../shared/utils/http-utils';

@Component({
  selector: 'app-register',
  imports: [FontAwesomeModule, FormsModule, RouterLink, ErrorBox],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  faUserPlus = faUserPlus;

  errors = signal<string[]>([]);

  authService = inject(AuthService);
  router = inject(Router);

  onRegister(form: NgForm) {
    this.errors.set([]);
    if (form.invalid) return;

    const payload: RegisterRequest = {
      email: form.value.email,
      firstName: form.value.firstName,
      lastName: form.value.lastName,
      gender: (form.value.gender ?? 'Undefined') as Gender,
      username: form.value.username,
      password: form.value.password,
      confirmPassword: form.value.confirmPassword,
    };

    this.authService.register(payload).subscribe({
      next: () => this.router.navigate(['/login']),
      error: (err) => this.errors.set(toErrorList(err)),
    });
  }
}
