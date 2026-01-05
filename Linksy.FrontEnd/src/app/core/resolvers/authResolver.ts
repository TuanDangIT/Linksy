import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { User } from '../models/user';

export const authResolver: ResolveFn<User | null> = () => {
  const authService = inject(AuthService);
  console.log('AuthResolver: Resolving user authentication status');
  return authService.checkAuthStatus();
};
