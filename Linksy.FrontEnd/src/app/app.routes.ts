import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { MainLayout } from './core/layouts/main-layout/main-layout';
import { ShortenedUrlList } from './features/shortened-urls/list/shortened-url-list';
import { LandingPageList } from './features/landing-pages/list/landing-page-list';
import { QrcodeList } from './features/qrcodes/list/qrcode-list';
import { BarcodeList } from './features/barcodes/list/barcode-list';

export const routes: Routes = [
    { path: 'login', component: Login },   
    { path: 'register', component: Register },
    {
        path: '',
        component: MainLayout,
        children: [
            { path: '', redirectTo: 'shortened-urls', pathMatch: 'full'},
            { path: 'shortened-urls', component: ShortenedUrlList, title: 'Shortened URLs' },
            { path: 'landing-pages', component: LandingPageList, title: 'Landing Pages' },
            { path: 'qrcodes', component: QrcodeList, title: 'QR Codes' },
            { path: 'barcodes', component: BarcodeList, title: 'Barcodes' },
        ]
    },
    { path: '**', redirectTo: 'login' },
];
