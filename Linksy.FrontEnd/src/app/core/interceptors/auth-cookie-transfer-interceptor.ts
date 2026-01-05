import { HttpInterceptorFn } from '@angular/common/http';
import { inject, PLATFORM_ID, REQUEST } from '@angular/core';
import { isPlatformServer } from '@angular/common';

export const authCookieTransferInterceptor: HttpInterceptorFn = (req, next) => {
  const platformId = inject(PLATFORM_ID);

  if (isPlatformServer(platformId)) {
    const serverRequest = inject(REQUEST, { optional: true });

    if (serverRequest) {
      const cookies = serverRequest.headers.get('cookie') || '';

      req = req.clone({
        setHeaders: {
          Cookie: cookies,
          'X-Rendering-Mode': 'SSR',
        },
        withCredentials: true,
      });
    }
  }

  return next(req);
};
