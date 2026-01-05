import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-error-box',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './error-box.html',
  styleUrl: './error-box.css',
})
export class ErrorBox {
  @Input({ required: true }) errors: string[] = [];
}
