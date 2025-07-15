import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AdminService } from '../../core/api/admin.service';
import { AuthService } from '../../core/api/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class DashboardComponent implements OnInit {
  registrations: any[] = [];
  payments: any[] = [];
  error: string = '';

  constructor(
    private adminService: AdminService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadRegistrations();
    this.loadPayments();
  }

  loadRegistrations(): void {
    this.adminService.getRegistrations().subscribe({
      next: (data) => this.registrations = data,
      error: (err) => this.handleError('Failed to load registrations', err)
    });
  }

  loadPayments(): void {
    this.adminService.getPayments().subscribe({
      next: (data) => this.payments = data,
      error: (err) => this.handleError('Failed to load payments', err)
    });
  }

  onLogout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.handleError('Logout failed', err);
        // Still navigate to login even if logout fails on the server
        this.router.navigate(['/login']);
      }
    });
  }

  private handleError(message: string, error: any): void {
    this.error = `${message}: ${error.error?.message || error.message}`;
    console.error(message, error);
  }
}
