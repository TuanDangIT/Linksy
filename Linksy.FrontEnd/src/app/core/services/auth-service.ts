import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';

interface AuthTokens {
  accessToken: string;
  refreshToken: string;
}

interface User {
  id: string;
  email: string;
  name: string;
}

interface RegisterRequest {
  email: string;
  firstName: string;
  lastName: string;
  gender: string;
  username: string;
  password: string;
  confirmPassword: string;
}

interface JwtClaims {
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string;
  exp: number;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly API_URL = environment.apiBaseUrl;
  private readonly ACCESS_TOKEN_KEY = environment.authConfig.accessTokenKey;
  private readonly REFRESH_TOKEN_KEY = environment.authConfig.refreshTokenKey;
  private httpClient = inject(HttpClient);
  private router = inject(Router);

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    this.checkAuthStatus().subscribe({
      next: () => {
        console.log('User authenticated');
      },
      error: () => {
        console.log('User not authenticated');
      },
    });
  }

  checkAuthStatus(): Observable<User | null> {
    return this.httpClient
      .get<User>(`${this.API_URL}/users`, {
        withCredentials: true, 
      })
      .pipe(
        tap((user) => this.currentUserSubject.next(user)),
        catchError(() => {
          this.currentUserSubject.next(null);
          return throwError(() => null);
        })
      );
  }

  login(email: string, password: string): Observable<AuthTokens> {
    return this.httpClient
      .post<AuthTokens>(
        `${this.API_URL}/users/login`,
        {
          email,
          password,
        },
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap((tokens) => this.handleAuthSuccess(tokens)),
        catchError(this.handleError)
      );
  }

  register(registerData: RegisterRequest): Observable<AuthTokens> {
    return this.httpClient
      .post<AuthTokens>(`${this.API_URL}/users/register`, registerData)
      .pipe(catchError(this.handleError));
  }

  logout(): Observable<void> {
    return this.httpClient
      .post<void>(
        `${this.API_URL}/users/logout`,
        {},
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap(() => {
          this.currentUserSubject.next(null);
          this.router.navigate(['/login']);
        }),
        catchError((err) => {
          this.currentUserSubject.next(null);
          this.router.navigate(['/login']);
          return throwError(() => err);
        })
      );
  }

  isAuthenticated(): boolean {
    return this.currentUserSubject.value !== null;
  }

  private handleAuthSuccess(tokens: AuthTokens): void {
    this.loadUserFromToken(tokens.accessToken);
  }

  private loadUserFromToken(token: string): void {
    try {
      const payload = this.decodeToken(token);
      const user: User = {
        id: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
        email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
        name: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      };
      this.currentUserSubject.next(user);
    } catch (error) {
      console.error('Error decoding token:', error);
    }
  }

  private decodeToken(token: string): JwtClaims {
    const parts = token.split('.');
    if (parts.length !== 3) {
      throw new Error('Invalid token format');
    }
    const decoded = atob(parts[1]);
    return JSON.parse(decoded);
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }

    return throwError(() => new Error(errorMessage));
  }
}
