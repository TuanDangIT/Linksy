import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-user-modal',
  standalone: true,
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './user-modal.html',
  styleUrl: './user-modal.css',
})
export class UserModal {
  @Input() isOpen = false;
  @Output() cancel = new EventEmitter<void>();

  private readonly authService = inject(AuthService);

  faUser = faUser;
  user$ = this.authService.currentUser$;

  onBackdropClick(): void {
    this.cancel.emit();
  }
}
