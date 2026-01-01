import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-confirm-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './confirm-modal.html',
  styleUrl: './confirm-modal.css',
})
export class ConfirmModal {
  @Input() isOpen = false;
  @Input() title = 'Confirm';
  @Input() message = '';
  @Input() confirmText = 'Confirm';
  @Input() cancelText = 'Cancel';

  @Output() cancel = new EventEmitter<void>();
  @Output() confirm = new EventEmitter<void>();

  onBackdropClick(): void {
    this.cancel.emit();
  }
}
