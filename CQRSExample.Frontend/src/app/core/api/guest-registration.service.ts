import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GuestRegistrationService {
  private apiUrl = 'https://localhost:7258/api/GuestRegistration'; // Adjust the port if your backend runs on a different one.

  constructor(private http: HttpClient) { }

  createRegistration(formData: FormData): Observable<any> {
    return this.http.post(this.apiUrl, formData);
  }
}