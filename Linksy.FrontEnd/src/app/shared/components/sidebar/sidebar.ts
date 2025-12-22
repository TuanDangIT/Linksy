import { Component, inject, output, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faLink, faQrcode, faBarcode, faPager, faArrowRightFromBracket, IconDefinition, faAnglesLeft, faAnglesRight } from '@fortawesome/free-solid-svg-icons';

interface NavItem {
  label: string;
  icon: IconDefinition; // SVG path data
  route: string;
}

@Component({
  selector: 'app-sidebar',
  imports: [FontAwesomeModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  isMinimized = signal(false);
  isMinimizedChange = output<boolean>();
  activeItem = signal('Shortened Urls');
  faArrowRightFromBracket = faArrowRightFromBracket;
  faAnglesLeft = faAnglesLeft;
  faAnglesRight = faAnglesRight;  

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
    console.log('Logging out...');
  }
}
