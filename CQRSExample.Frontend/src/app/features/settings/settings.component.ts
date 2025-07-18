import { Component, OnInit } from '@angular/core';
import { TranslateService, TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { ThemeService } from '../../core/services/theme.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  standalone: true,
  imports: [TranslateModule, CommonModule]
})
export class SettingsComponent implements OnInit {
  isDarkMode: boolean = false;

  constructor(public translate: TranslateService, private themeService: ThemeService) { }

  ngOnInit(): void {
    this.isDarkMode = this.themeService.isDarkMode();
  }

  switchLanguage(language: string) { // Change parameter back to string
    this.translate.use(language);
  }

  toggleDarkMode() {
    this.themeService.toggleTheme();
    this.isDarkMode = this.themeService.isDarkMode();
  }
}