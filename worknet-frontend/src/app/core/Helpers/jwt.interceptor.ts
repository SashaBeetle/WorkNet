import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';          // Import isPlatformBrowser
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
   // Inject PLATFORM_ID to determine the platform
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log('JWT_INTERCEPTOR: Intercepting request to URL ->', req.url);
    let token: string | null = null;

    // Only try to access localStorage if running in a browser environment
    if (isPlatformBrowser(this.platformId)) {
      token = localStorage.getItem('jwt');
      console.log(`JWT_INTERCEPTOR (Browser): Token from localStorage ('jwt') for ${req.url} ->`, token);
    } else {
      console.log(`JWT_INTERCEPTOR (Server): Not in browser, localStorage not accessed for ${req.url}.`);

    }

    if (token) {
      const cloned = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
      console.log('JWT_INTERCEPTOR: Authorization header attached for URL ->', req.url);
      return next.handle(cloned);
    }

    console.log('JWT_INTERCEPTOR: No token available to attach (or not in browser). Original request passed for URL ->', req.url);
    return next.handle(req);
  }
}