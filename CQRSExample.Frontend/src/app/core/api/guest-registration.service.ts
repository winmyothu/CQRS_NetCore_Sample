import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface MonthlyRegistrationDto {
  month: string;
  count: number;
}

export interface YearlyRegistrationFeeDto {
  year: string;
  totalFee: number;
}

@Injectable({
  providedIn: 'root'
})
export class GuestRegistrationService {
  private guestRegistrationUrl = 'https://localhost:59080/api/GuestRegistration';
  private dashboardUrl = 'https://localhost:59080/api/Dashboard';

  constructor(private http: HttpClient) { }

  createRegistration(formData: FormData): Observable<any> {
    return this.http.post(this.guestRegistrationUrl, formData, { withCredentials: true });
  }

  getMonthlyRegistrations(): Observable<MonthlyRegistrationDto[]> {
    return this.http.get<MonthlyRegistrationDto[]>(`${this.dashboardUrl}/monthly-registrations`, { withCredentials: true });
  }

  getYearlyRegistrationFees(): Observable<YearlyRegistrationFeeDto[]> {
    return this.http.get<YearlyRegistrationFeeDto[]>(`${this.dashboardUrl}/yearly-registration-fees`, { withCredentials: true });
  }
}
