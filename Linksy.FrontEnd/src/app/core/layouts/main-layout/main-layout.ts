import { Component, signal } from '@angular/core';
import { Sidebar } from '../../../shared/components/sidebar/sidebar';
import { RouterOutlet } from '@angular/router';
import { sign } from 'crypto';

@Component({
  selector: 'app-main-layout',
  imports: [Sidebar, RouterOutlet],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css',
})
export class MainLayout {
  isMinimized = signal(false);
  onMinimized(isMinimized: boolean) {
    this.isMinimized.update((value) => !value);
  }
}
