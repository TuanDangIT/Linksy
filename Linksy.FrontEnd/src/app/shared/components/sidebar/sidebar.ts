import { Component, inject, output, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faLink,
  faQrcode,
  faBarcode,
  faPager,
  faArrowRightFromBracket,
  IconDefinition,
  faAnglesLeft,
  faAnglesRight,
  faUser,
} from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../../../core/services/auth-service';
import { UserModal } from '../user-modal/user-modal';

interface NavItem {
  label: string;
  icon: IconDefinition;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  imports: [FontAwesomeModule, RouterLink, RouterLinkActive, UserModal],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  isMinimized = signal(false);
  authService = inject(AuthService);
  router = inject(Router);
  isMinimizedChange = output<boolean>();
  activeItem = signal('Shortened Urls');
  faArrowRightFromBracket = faArrowRightFromBracket;
  faAnglesLeft = faAnglesLeft;
  faAnglesRight = faAnglesRight;
  faUser = faUser;
  isUserModalOpen = signal(false);

  navItems: NavItem[] = [
    {
      label: 'Shortened Urls',
      icon: faLink,
      route: '/shortened-urls',
    },
    {
      label: 'QR Codes',
      icon: faQrcode,
      route: '/qrcodes',
    },
    {
      label: 'Barcodes',
      icon: faBarcode,
      route: '/barcodes',
    },
    {
      label: 'Landing Pages',
      icon: faPager,
      route: '/landing-pages',
    },
  ];

  toggleMinimize(): void {
    this.isMinimized.update((value) => !value);
    this.isMinimizedChange.emit(this.isMinimized());
  }
  setActive(itemLabel: string): void {
    this.activeItem.set(itemLabel);
  }
  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
    });
    console.log('Logging out...');
  }

  openUserModal(): void {
    this.isUserModalOpen.set(true);
  }

  closeUserModal(): void {
    this.isUserModalOpen.set(false);
  }
}
