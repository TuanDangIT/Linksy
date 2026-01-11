import { isPlatformServer } from '@angular/common';
import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { inject, PLATFORM_ID, RESPONSE_INIT } from '@angular/core';
import { tap } from 'rxjs';

export const setCookiePropagationInterceptor: HttpInterceptorFn = (req, next) => {
  const platformId = inject(PLATFORM_ID);
  const responseInit = inject(RESPONSE_INIT, { optional: true });

  if (!isPlatformServer(platformId) || !responseInit) {
    return next(req);
  }

  return next(req).pipe(
    tap((event) => {
      if (!(event instanceof HttpResponse)) return;

      const setCookies = event.headers.getAll('set-cookie');
      if (!setCookies || !setCookies.length) return;

      const headers = new Headers(responseInit.headers ?? undefined);
      for (const cookie of setCookies) {
        headers.append('set-cookie', cookie);
      }
      responseInit.headers = headers;
    })
  );
};
