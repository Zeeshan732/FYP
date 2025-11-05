import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Get token from localStorage
    const token = localStorage.getItem('token');

    // Clone the request and add the Authorization header if token exists
    let authRequest = request;
    if (token) {
      authRequest = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    // Handle the request and catch errors
    return next.handle(authRequest).pipe(
      catchError((error: HttpErrorResponse) => {
        // Handle 401 Unauthorized - token expired or invalid
        if (error.status === 401) {
          // Clear token and user data
          localStorage.removeItem('token');
          localStorage.removeItem('user');
          
          // Redirect to login if not already there
          if (this.router.url !== '/login') {
            this.router.navigate(['/login']);
          }
        }

        // Handle 403 Forbidden - insufficient permissions
        if (error.status === 403) {
          console.error('Access denied: Insufficient permissions');
        }

        return throwError(() => error);
      })
    );
  }
}


