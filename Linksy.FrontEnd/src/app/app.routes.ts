import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { MainLayout } from './core/layouts/main-layout/main-layout';
import { ShortenedUrlList } from './features/shortened-urls/shortened-url-list/shortened-url-list';
import { LandingPageList } from './features/landing-pages/list/landing-page-list';
import { QrcodeList } from './features/qrcodes/list/qrcode-list';
import { BarcodeList } from './features/barcodes/list/barcode-list';
import { authGuard, noAuthGuard } from './core/guards/auth-guard';
import { ShortenedUrlDetails } from './features/shortened-urls/shortened-url-details/shortened-url-details';

export const routes: Routes = [
  { path: 'login', component: Login, title: 'Login', canActivate: [noAuthGuard] },
  { path: 'register', component: Register, title: 'Register', canActivate: [noAuthGuard] },
  {
    path: '',
    component: MainLayout,
    children: [
      { path: '', redirectTo: 'shortened-urls', pathMatch: 'full' },
      {
        path: 'shortened-urls/:id',
        component: ShortenedUrlDetails,
        title: 'Shortened URL Details',
        canActivate: [authGuard],
      },
      {
        path: 'shortened-urls',
        component: ShortenedUrlList,
        title: 'Shortened URLs',
        canActivate: [authGuard],
      },
      {
        path: 'landing-pages',
        component: LandingPageList,
        title: 'Landing Pages',
        canActivate: [authGuard],
      },
      { path: 'qrcodes', component: QrcodeList, title: 'QR Codes', canActivate: [authGuard] },
      { path: 'barcodes', component: BarcodeList, title: 'Barcodes', canActivate: [authGuard] },
    ],
  },
  { path: '**', redirectTo: '/login' },
];
