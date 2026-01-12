import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, map, Observable, tap, throwError } from 'rxjs';
import { User } from '../models/user';
import { ApiResponse } from '../types/apiResponse';
import { LoginRequest, RegisterRequest } from '../types/auth'

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = environment.apiBaseUrl + '/users';
  private readonly httpClient = inject(HttpClient);

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  readonly currentUser$ = this.currentUserSubject.asObservable();

  constructor() {}

  checkAuthStatus(): Observable<User | null> {
    return this.httpClient
      .get<ApiResponse<User>>(`${this.apiUrl}`, {
        withCredentials: true,
        transferCache: false,
      })
      .pipe(
        map((response) => response.data),
        tap((user) => this.currentUserSubject.next(user)),
        catchError((e) => {
          this.currentUserSubject.next(null);
          return throwError(() => null);
        })
      );
  }

  login(loginDto: LoginRequest): Observable<User> {
    return this.httpClient
      .post<ApiResponse<User>>(`${this.apiUrl}/login`, loginDto, { withCredentials: true })
      .pipe(
        map((response) => response.data),
        tap((user) => this.currentUserSubject.next(user))
      );
  }

  register(registerDto: RegisterRequest): Observable<void> {
    return this.httpClient.post<void>(`${this.apiUrl}/register`, registerDto, {
      withCredentials: true,
    });
  }

  logout(): Observable<void> {
    return this.httpClient
      .post<void>(
        `${this.apiUrl}/logout`,
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
}
