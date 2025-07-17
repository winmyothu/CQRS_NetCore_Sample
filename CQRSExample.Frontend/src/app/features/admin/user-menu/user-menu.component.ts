import { Component, OnInit, HostListener, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/api/auth.service';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt'; // Import JwtHelperService

@Component({
  selector: 'app-user-menu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class UserMenuComponent implements OnInit {
  username: string | null = null;
  isMenuOpen = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private elementRef: ElementRef,
    private jwtHelper: JwtHelperService // Inject JwtHelperService
  ) { }

  ngOnInit(): void {
    const token = localStorage.getItem('accessToken');
    if (token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      this.username = decodedToken?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || decodedToken?.unique_name || decodedToken?.name || null;
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isMenuOpen = false;
    }
  }

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        console.log('Logout successful');
      },
      error: (error) => {
        console.error('Logout failed', error);
      }
    });

    this.router.navigate(['/login']);
  }
}
