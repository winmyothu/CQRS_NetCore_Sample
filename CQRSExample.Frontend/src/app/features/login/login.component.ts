import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/api/auth.service';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  message: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
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
          this.message = 'Login successful!';
          console.log('Login successful', response);
          this.router.navigate(['/admin/dashboard']);
        },
        error: (error) => {
          this.message = 'Login failed: ' + (error.error?.message || error.message);
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
          this.message = 'User registration successful!';
          console.log('User registration successful', response);
          this.loginForm.reset();
        },
        error: (error) => {
          this.message = 'User registration failed: ' + (error.error?.message || error.message);
          console.error('User registration failed', error);
        }
      });
    }
  }
}
