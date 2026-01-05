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
    catchError(() => {
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

  console.log("noAuthGuard: Checking if user is already authenticated.")
  if (authService.isAuthenticated()) {
    console.log("User is authenticated.")
    return router.createUrlTree(['/shortened-urls']);
  }

  console.log("Checking user authentication status via AuthService.")
  return authService.checkAuthStatus().pipe(
    map((user) => {
      if (user) {
        console.log("User is authenticated.")
        return router.createUrlTree(['/shortened-urls']);
      }
      console.log("User is not authenticated.")
      return true;
    }),
    catchError(() => {
      return of(true);
    })
  );
};
