import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { RegisterUserPayload, LoginUserPayload } from '../models/auth.models';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:7047/api/auth';

  register(data: RegisterUserPayload): Observable<any> {
    return this.http.post<{ token: string }>(`${this.baseUrl}/register`, data).pipe(
      tap((res) => {
        localStorage.setItem('jwt', res.token); // Save token
      })
    );
  }

  login(data: LoginUserPayload): Observable<any> {
    return this.http.post<{ token: string }>(`${this.baseUrl}/login`, data).pipe(
      tap((res) => {
        localStorage.setItem('jwt', res.token);
      })
    );
  }

  logout() {
    localStorage.removeItem('jwt');
  }

  getToken(): string | null {
    return localStorage.getItem('jwt');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
