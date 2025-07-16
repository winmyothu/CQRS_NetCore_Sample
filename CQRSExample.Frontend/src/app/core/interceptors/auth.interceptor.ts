import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, filter, take, switchMap } from 'rxjs/operators';
import { AuthService } from '../api/auth.service';
import { Router } from '@angular/router';
import { inject } from '@angular/core';

let isRefreshing = false;
let refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

export const AuthInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const addToken = (request: HttpRequest<unknown>): HttpRequest<unknown> => {
    const accessToken = authService.getAccessToken();
    if (accessToken) {
      return request.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`
        }
      });
    }
    return request;
  };

  const handle401Error = (request: HttpRequest<unknown>, next: HttpHandlerFn) => {
    if (!isRefreshing) {
      isRefreshing = true;
      refreshTokenSubject.next(null);

      return authService.refreshToken().pipe(
        switchMap((response: any) => {
          isRefreshing = false;
          refreshTokenSubject.next(response.accessToken); // Update subject with new access token
          return next(addToken(request));
        }),
        catchError((err: any) => {
          isRefreshing = false;
          authService.logout().subscribe(); // Clear tokens on frontend and backend
          router.navigate(['/login']); // Redirect to login page
          return throwError(err);
        })
      );
    } else {
      return refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(token => next(addToken(request)))
      );
    }
  };

  // Add access token to request if available in localStorage
  req = addToken(req);

  return next(req).pipe(
    catchError(error => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        return handle401Error(req, next);
      }
      return throwError(error);
    })
  );
};
