import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AdminService } from '../api/admin.service'; // Import AdminService
import { AuthService } from '../api/auth.service'; // Import AuthService for logout

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private router: Router,
    private adminService: AdminService, // Inject AdminService
    private authService: AuthService // Inject AuthService
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    console.log('AuthGuard: canActivate triggered for route:', route.url.join('/'));

    if (!this.authService.isLoggedIn()) {
      console.log('AuthGuard: User not logged in (no access token in localStorage). Redirecting to login.');
      return of(this.router.createUrlTree(['/login']));
    }

    return this.adminService.getRegistrations(1, 1).pipe(
      map(() => {
        console.log('AuthGuard: Authentication check successful. Allowing access.');
        return true;
      }), // If getRegistrations succeeds, return true
      catchError((error) => {
        // If getRegistrations fails (e.g., 401 Unauthorized), redirect to login
        console.error('AuthGuard: Authentication check failed. Redirecting to login.', error);
        // Clear tokens on frontend and trigger backend logout
        this.authService.logout().subscribe({
          next: () => console.log('AuthGuard: Backend logout successful'),
          error: (logoutError) => console.error('AuthGuard: Backend logout failed', logoutError)
        });
        return of(this.router.createUrlTree(['/login']));
      })
    );
  }
}