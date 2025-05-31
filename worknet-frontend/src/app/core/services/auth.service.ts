import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, switchMap } from 'rxjs';
import { User } from '../models/user.models';
import { RegisterUserPayload, LoginUserPayload } from '../models/auth.models';
import { Store } from '@ngrx/store';
import { loginSuccess } from '../../ngrx/actions/user.actions';
import { logout } from '../../ngrx/actions/user.actions';
import { jwtDecode } from 'jwt-decode';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private store = inject(Store);

  private baseUrl = 'https://localhost:7047/api';

  register(data: RegisterUserPayload): Observable<any> {
     return this.http.post<{ token: string }>(`${this.baseUrl}/auth/register`, data).pipe(
    tap((res) => {
      localStorage.setItem('jwt', res.token);
    }),
    switchMap(() => this.http.get<User>(`${this.baseUrl}/user/me`)),
    tap((user) => {
      this.store.dispatch(loginSuccess({ user }));
    })
  );
  }

  login(data: LoginUserPayload): Observable<any> {
  return this.http.post<{ token: string }>(`${this.baseUrl}/auth/login`, data).pipe(
    tap((res) => {
      localStorage.setItem('jwt', res.token);
    }),
    switchMap((res) => {
      const token = res.token;
      return this.http.get<User>(`${this.baseUrl}/user/me`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
    }),
    tap((user) => {
      console.log('User loaded from /user/me:', user);
      this.store.dispatch(loginSuccess({ user }));
    })
  );
}

  logout() {
    if (typeof window !== 'undefined') {
        localStorage.removeItem('jwt');
      }
    
    this.store.dispatch(logout()); 
  }

  getToken(): string | null {
     if (typeof window === 'undefined') return null;
    return localStorage.getItem('jwt');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const { exp } = jwtDecode<{ exp: number }>(token);
      return Date.now() < exp * 1000;
    } catch {
      return false;
    }
  }

  initUserFromToken() {
  const token = this.getToken();
  if (token && this.isLoggedIn()) {
    this.http.get<User>(`${this.baseUrl}/user/me`).subscribe({
      next: (user) => this.store.dispatch(loginSuccess({ user })),
      error: () => console.log("User not autorized")
    });
  } else {
    this.logout();
  }
}

}
