import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authUrl = 'https://localhost:59080/api/Auth'; // Adjust the port if your backend runs on a different one.

  constructor(private http: HttpClient) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.authUrl}/login`, { email, password }, { withCredentials: true }).pipe(
      tap(response => {
        if (response && response.accessToken) {
          localStorage.setItem('accessToken', response.accessToken);
        }
      })
    );
  }

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  refreshToken(): Observable<any> {
    return this.http.post<any>(`${this.authUrl}/refresh-token`, {}, { withCredentials: true }).pipe(
      tap(response => {
        if (response && response.accessToken) {
          localStorage.setItem('accessToken', response.accessToken);
        }
      })
    );
  }

  logout(): Observable<any> {
    return this.http.post(`${this.authUrl}/logout`, {}, { withCredentials: true }).pipe(
      tap(() => {
        localStorage.removeItem('accessToken');
      })
    );
  }

  isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }
}
