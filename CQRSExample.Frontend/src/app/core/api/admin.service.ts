import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../shared/models/paginated-result.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private adminUrl = 'https://localhost:59080/api/Admin';

  constructor(private http: HttpClient) { }

  getRegistrations(pageNumber: number, pageSize: number, searchTerm?: string, sortField?: string, sortOrder?: string): Observable<PaginatedResult<any>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }
    if (sortField) {
      params = params.set('sortField', sortField);
    }
    if (sortOrder) {
      params = params.set('sortOrder', sortOrder);
    }
    return this.http.get<PaginatedResult<any>>(`${this.adminUrl}/registrations`, { params, withCredentials: true });
  }

  getPayments(pageNumber: number, pageSize: number, searchTerm?: string, sortField?: string, sortOrder?: string): Observable<PaginatedResult<any>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }
    if (sortField) {
      params = params.set('sortField', sortField);
    }
    if (sortOrder) {
      params = params.set('sortOrder', sortOrder);
    }
    return this.http.get<PaginatedResult<any>>(`${this.adminUrl}/payments`, { params, withCredentials: true });
  }
}
