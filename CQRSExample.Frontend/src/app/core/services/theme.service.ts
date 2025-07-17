import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly themeKey = 'theme';

  constructor() {
    this.loadTheme();
  }

  isDarkMode(): boolean {
    return document.documentElement.classList.contains('dark');
  }

  toggleTheme(): void {
    if (this.isDarkMode()) {
      this.setLightMode();
    } else {
      this.setDarkMode();
    }
  }

  private loadTheme(): void {
    const savedTheme = localStorage.getItem(this.themeKey);
    if (savedTheme === 'dark') {
      this.setDarkMode();
    } else {
      this.setLightMode();
    }
    // Ensure the class is applied immediately on load
    this.applyThemeToDocument();
  }

  private applyThemeToDocument(): void {
    if (this.isDarkMode()) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }

  private setDarkMode(): void {
    document.documentElement.classList.add('dark');
    localStorage.setItem(this.themeKey, 'dark');
  }

  private setLightMode(): void {
    document.documentElement.classList.remove('dark');
    localStorage.setItem(this.themeKey, 'light');
  }
}