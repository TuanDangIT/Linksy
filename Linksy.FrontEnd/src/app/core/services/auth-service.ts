import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, map, Observable, tap, throwError } from 'rxjs';
import { User } from '../models/user';
import { ApiResponse } from '../types/ApiResponse';
import { LoginRequest } from '../types/LoginRequest';
import { RegisterRequest } from '../types/RegisterRequest';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly API_URL = environment.apiBaseUrl;
  private httpClient = inject(HttpClient);

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    this.checkAuthStatus().subscribe({
      next: (user) => {
        console.log('User authenticated');
        console.log(user);
      },
      error: () => {
        console.log('User not authenticated');
      },
    });
  }

  checkAuthStatus(): Observable<User | null> {
    return this.httpClient
      .get<ApiResponse<User>>(`${this.API_URL}/users`, {
        withCredentials: true,
      })
      .pipe(
        map((response) => response.data),
        tap((user) => this.currentUserSubject.next(user)),
        catchError(() => {
          this.currentUserSubject.next(null);
          return throwError(() => null);
        })
      );
  }

  login(loginDto: LoginRequest): Observable<User> {
    return this.httpClient
      .post<ApiResponse<User>>(`${this.API_URL}/users/login`, loginDto, { withCredentials: true })
      .pipe(
        map((response) => response.data),
        tap((user) => {
          this.currentUserSubject.next(user);
        }),
        catchError(this.handleError)
      );
  }

  register(registerDto: RegisterRequest): Observable<void> {
    return this.httpClient.post<void>(`${this.API_URL}/users/register`, registerDto, {
      withCredentials: true,
    });
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
        }),
        catchError((err) => {
          this.currentUserSubject.next(null);
          return throwError(() => err);
        })
      );
  }

  isAuthenticated(): boolean {
    return this.currentUserSubject.value !== null;
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  hasRole(role: string): boolean {
    const user = this.currentUserSubject.value;
    return user?.roles.includes(role) ?? false;
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      if (error.error?.message) {
        errorMessage = error.error.message;
      } else if (error.error?.errors) {
        const errors = Object.values(error.error.errors).flat();
        errorMessage = errors.join(', ');
      } else {
        errorMessage = `Error ${error.status}: ${error.message}`;
      }
    }

    console.error('Auth Service Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
