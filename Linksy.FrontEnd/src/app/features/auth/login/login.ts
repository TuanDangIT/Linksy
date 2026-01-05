import { Component, inject, signal } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service';
import { ErrorBox } from '../../../shared/components/error-box/error-box';
import { toErrorList } from '../../../shared/utils/http-error-utils';

@Component({
  selector: 'app-login',
  imports: [FontAwesomeModule, FormsModule, RouterLink, ErrorBox],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  faLock = faLock;
  errors = signal<string[]>([]);

  onLogin(form: NgForm) {
    this.errors.set([]);
    if (form.invalid) return;

    const { email, password } = form.value;

    this.authService.login({ email, password }).subscribe({
      next: () => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/shortened-urls';
        this.router.navigateByUrl(returnUrl);
      },
      error: (err) => {
        this.errors.set(toErrorList(err));
      },
    });
  }
}
