import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GuestRegistrationService {
  private guestRegistrationUrl = 'https://localhost:59080/api/GuestRegistration';

  constructor(private http: HttpClient) { }

  createRegistration(formData: FormData): Observable<any> {
    return this.http.post(this.guestRegistrationUrl, formData, { withCredentials: true });
  }
}
