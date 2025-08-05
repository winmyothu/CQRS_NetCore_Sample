import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './core/api/auth.service';
import { CommonModule } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { ThemeService } from './core/services/theme.service';
import { NetworkStatusService } from './core/services/network-status.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  standalone: true
})
export class AppComponent {
  title = 'CQRSExample.Frontend';
  isOnline: boolean = true;

  constructor(
    public authService: AuthService,
    private router: Router,
    private translate: TranslateService,
    private themeService: ThemeService,
    private networkStatusService: NetworkStatusService
  ) {
    this.translate.setDefaultLang('en');
    this.translate.use('en');
    this.themeService.isDarkMode();

    this.networkStatusService.isOnline.subscribe(online => {
      this.isOnline = online;
    });
  }

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Logout failed', err);
        // Even if logout fails on the server, we clear the local token and redirect
        localStorage.removeItem('accessToken');
        this.router.navigate(['/login']);
      }
    });
  }
}
