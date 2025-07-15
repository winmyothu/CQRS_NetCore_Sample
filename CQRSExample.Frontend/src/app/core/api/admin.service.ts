import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private adminUrl = 'https://localhost:59080/api/Admin';

  constructor(private http: HttpClient) { }

  getRegistrations(): Observable<any[]> {
    return this.http.get<any[]>(`${this.adminUrl}/registrations`, { withCredentials: true });
  }

  getPayments(): Observable<any[]> {
    return this.http.get<any[]>(`${this.adminUrl}/payments`, { withCredentials: true });
  }
}
