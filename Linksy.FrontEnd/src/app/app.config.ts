import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {
  provideClientHydration,
  withEventReplay,
  withHttpTransferCacheOptions,
} from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { authCookieTransferInterceptor } from './core/interceptors/auth-cookie-transfer-interceptor';
import { setCookiePropagationInterceptor } from './core/interceptors/set-cookie-propagation-interceptor';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideCharts(withDefaultRegisterables()),
    provideClientHydration(
      withEventReplay(),
      withHttpTransferCacheOptions({
        includePostRequests: false,
        includeHeaders: ['set-cookie'],
      })
    ),
    provideHttpClient(
      withFetch(),
      withInterceptors([authCookieTransferInterceptor, setCookiePropagationInterceptor])
    ),
  ],
};
