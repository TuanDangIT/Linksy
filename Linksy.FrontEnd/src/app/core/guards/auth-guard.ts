import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.checkAuthStatus().pipe(
    map((user) => {
      if (user) {
        return true;
      }
      return router.createUrlTree(['/login'], {
        queryParams: { returnUrl: state.url },
      });
    }),
    catchError((e) => {
      return of(
        router.createUrlTree(['/login'], {
          queryParams: { returnUrl: state.url },
        })
      );
    })
  );
};

export const noAuthGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return router.createUrlTree(['/shortened-urls']);
  }

  return authService.checkAuthStatus().pipe(
    map((user) => {
      if (user) {
        return router.createUrlTree(['/shortened-urls']);
      }
      return true;
    }),
    catchError(() => {
      return of(true);
    })
  );
};
