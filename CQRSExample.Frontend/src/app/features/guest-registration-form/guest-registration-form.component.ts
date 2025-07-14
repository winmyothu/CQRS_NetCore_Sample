import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GuestRegistrationService } from '../../core/api/guest-registration.service';

@Component({
  selector: 'app-guest-registration-form',
  templateUrl: './guest-registration-form.component.html',
  styleUrls: ['./guest-registration-form.component.css']
})
export class GuestRegistrationFormComponent implements OnInit {
  registrationForm: FormGroup;
  selectedFiles: File[] = [];

  constructor(
    private fb: FormBuilder,
    private guestRegistrationService: GuestRegistrationService
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
          console.log('Registration successful', response);
          this.registrationForm.reset();
          this.selectedFiles = [];
        },
        error: (error) => {
          console.error('Registration failed', error);
        }
      });
    }
  }
}