import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/api/auth.service';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, TranslateModule]
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  message: string = '';
  messageType: 'success' | 'error' | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private translate: TranslateService
  ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;

      this.authService.login(username, password).subscribe({
        next: (response) => {
          this.message = 'LOGIN.SUCCESS_MESSAGE';
          this.messageType = 'success';
          console.log('Login successful', response);
          this.router.navigate(['/admin/dashboard']);
        },
        error: (error) => {
          this.message = 'LOGIN.FAILED_MESSAGE';
          this.messageType = 'error';
          console.error('Login failed', error);
        }
      });
    }
  }

  onRegisterUser(): void {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authService.register(username, password).subscribe({
        next: (response) => {
          this.message = 'LOGIN.REGISTER_SUCCESS_MESSAGE';
          this.messageType = 'success';
          console.log('User registration successful', response);
          this.loginForm.reset();
        },
        error: (error) => {
          this.message = 'LOGIN.REGISTER_FAILED_MESSAGE';
          this.messageType = 'error';
          console.error('User registration failed', error);
        }
      });
    }
  }
}
