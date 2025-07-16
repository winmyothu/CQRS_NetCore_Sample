import { Component } from '@angular/core';
import { RouterOutlet, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/api/auth.service';
import { UserMenuComponent } from './user-menu/user-menu.component';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
  standalone: true,
  imports: [RouterOutlet, CommonModule, RouterModule, UserMenuComponent]
})
export class AdminComponent {
  constructor(public authService: AuthService, private router: Router) { }

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        console.log('Logout successful')},
      error: (error) => {
        console.error('Logout failed', error);}
      });
    this.router.navigate(['/login']);
  }
}
