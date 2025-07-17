import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TranslateModule } from '@ngx-translate/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css'],
  standalone: true,
  imports: [CommonModule, TranslateModule, FontAwesomeModule]
})
export class UserInfoComponent implements OnInit {
  username: string | null = null;
  email: string | null = null;
  faUserCircle = faUserCircle;

  constructor(private jwtHelper: JwtHelperService) { }

  ngOnInit(): void {
    const token = localStorage.getItem('accessToken');
    if (token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      this.username = decodedToken?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || decodedToken?.unique_name || decodedToken?.name || null;
      this.email = decodedToken?.email || null; // Assuming 'email' claim exists
    }
  }

  onChangePassword(): void {
    // Placeholder for change password logic
    console.log('Change password clicked');
    alert('Change password functionality not yet implemented.');
  }
}