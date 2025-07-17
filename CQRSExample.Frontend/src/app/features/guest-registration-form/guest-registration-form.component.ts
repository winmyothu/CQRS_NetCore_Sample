import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { GuestRegistrationService } from '../../core/api/guest-registration.service';
import { AuthService } from '../../core/api/auth.service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-guest-registration-form',
  templateUrl: './guest-registration-form.component.html',
  styleUrls: ['./guest-registration-form.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, TranslateModule],
})
export class GuestRegistrationFormComponent implements OnInit {
  registrationForm!: FormGroup;
  loginForm!: FormGroup;
  selectedFiles: File[] = [];
  message: string = '';
  messageType: 'success' | 'error' | null = null;
  isLoggedIn: boolean = false;

  constructor(
    private fb: FormBuilder,
    private guestRegistrationService: GuestRegistrationService,
    private authService: AuthService,
    private http: HttpClient,
    private translate: TranslateService
  ) { }

  ngOnInit(): void {
    this.registrationForm = this.fb.group({
      name: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      passportNumber: ['', Validators.required],
      nationality: ['', Validators.required],
      nrc: [''],
      currentAddress: ['', Validators.required],
      permanentAddress: [''],
      attachedFiles: [null]
    });

    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onFileChange(event: any): void {
    if (event.target.files.length > 0) {
      this.selectedFiles = Array.from(event.target.files);
    }
  }

  onSubmit(): void {
    if (this.registrationForm.valid) {
      const formData = new FormData();
      Object.keys(this.registrationForm.value).forEach(key => {
        if (key !== 'attachedFiles') {
          formData.append(key, this.registrationForm.value[key]);
        }
      });

      if (this.selectedFiles.length > 0) {
        this.selectedFiles.forEach(file => {
          formData.append('attachedFiles', file, file.name);
        });
      }

      this.guestRegistrationService.createRegistration(formData).subscribe({
        next: (response) => {
          this.message = 'GUEST_REGISTRATION.SUCCESS_MESSAGE';
          this.messageType = 'success';
          console.log('Registration successful', response);
          this.registrationForm.reset();
          this.selectedFiles = [];
        },
        error: (error) => {
          this.message = 'GUEST_REGISTRATION.FAILED_MESSAGE';
          this.messageType = 'error';
          console.error('Registration failed', error);
        }
      });
    }
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authService.login(username, password).subscribe({
        next: (response) => {
          this.message = 'GUEST_REGISTRATION.LOGIN_SUCCESS_MESSAGE';
          this.messageType = 'success';
          this.isLoggedIn = true;
          console.log('Login successful', response);
          this.loginForm.reset();
        },
        error: (error) => {
          this.message = 'GUEST_REGISTRATION.LOGIN_FAILED_MESSAGE';
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
          this.message = 'GUEST_REGISTRATION.REGISTER_SUCCESS_MESSAGE';
          this.messageType = 'success';
          console.log('User registration successful', response);
          this.loginForm.reset();
        },
        error: (error) => {
          this.message = 'GUEST_REGISTRATION.REGISTER_FAILED_MESSAGE';
          this.messageType = 'error';
          console.error('User registration failed', error);
        }
      });
    }
  }

  onLogout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.message = 'GUEST_REGISTRATION.LOGOUT_SUCCESS_MESSAGE';
        this.messageType = 'success';
        this.isLoggedIn = false;
        console.log('Logout successful');
      },
      error: (error) => {
        this.message = 'GUEST_REGISTRATION.LOGOUT_FAILED_MESSAGE';
        this.messageType = 'error';
        console.error('Logout failed', error);
      }
    });
  }
}
