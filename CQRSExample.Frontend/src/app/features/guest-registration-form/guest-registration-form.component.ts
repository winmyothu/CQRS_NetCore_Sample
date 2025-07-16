import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { GuestRegistrationService } from '../../core/api/guest-registration.service';
import { AuthService } from '../../core/api/auth.service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http'; // Import HttpClient
import { Observable, of } from 'rxjs'; // Import Observable and of
import { catchError, map } from 'rxjs/operators'; // Import operators

@Component({
  selector: 'app-guest-registration-form',
  templateUrl: './guest-registration-form.component.html',
  styleUrls: ['./guest-registration-form.component.css'],
  standalone: true, // Add standalone: true
  imports: [ReactiveFormsModule, CommonModule],
})
export class GuestRegistrationFormComponent implements OnInit {
  registrationForm!: FormGroup;
  loginForm!: FormGroup; // New login form
  selectedFiles: File[] = [];
  message: string = '';
  isLoggedIn: boolean = false; // To track login status

  constructor(
    private fb: FormBuilder,
    private guestRegistrationService: GuestRegistrationService,
    private authService: AuthService,
    private http: HttpClient // Inject HttpClient
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
          this.message = 'Registration successful!';
          console.log('Registration successful', response);
          this.registrationForm.reset();
          this.selectedFiles = [];
        },
        error: (error) => {
          this.message = 'Registration failed: ' + (error.error || error.message);
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
          this.message = 'Login successful!';
          this.isLoggedIn = true;
          console.log('Login successful', response);
          this.loginForm.reset();
        },
        error: (error) => {
          this.message = 'Login failed: ' + (error.error || error.message);
          console.error('Login failed', error);
        }
      });
    }
  }

  onRegisterUser(): void {
    if (this.loginForm.valid) { // Reusing loginForm for register user for simplicity
      const { username, password } = this.loginForm.value;
      this.authService.register(username, password).subscribe({
        next: (response) => {
          this.message = 'User registration successful!';
          console.log('User registration successful', response);
          this.loginForm.reset();
        },
        error: (error) => {
          this.message = 'User registration failed: ' + (error.error || error.message);
          console.error('User registration failed', error);
        }
      });
    }
  }

  onLogout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.message = 'Logout successful!';
        this.isLoggedIn = false;
        console.log('Logout successful');
      },
      error: (error) => {
        this.message = 'Logout failed: ' + (error.error || error.message);
        console.error('Logout failed', error);
      }
    });
  }
}
