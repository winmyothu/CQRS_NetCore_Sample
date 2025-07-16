import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../shared/models/paginated-result.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private adminUrl = 'https://localhost:59080/api/Admin';

  constructor(private http: HttpClient) { }

  getRegistrations(pageNumber: number, pageSize: number): Observable<PaginatedResult<any>> {
    return this.http.get<PaginatedResult<any>>(`${this.adminUrl}/registrations?pageNumber=${pageNumber}&pageSize=${pageSize}`, { withCredentials: true });
  }

  getPayments(pageNumber: number, pageSize: number): Observable<PaginatedResult<any>> {
    return this.http.get<PaginatedResult<any>>(`${this.adminUrl}/payments?pageNumber=${pageNumber}&pageSize=${pageSize}`, { withCredentials: true });
  }
}