import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private currentUser: User = {
    username: 'Admin User',
    avatarUrl: 'https://i.pravatar.cc/150?u=a042581f4e29026704d'
  };

  constructor() { }

  getCurrentUser(): Observable<User> {
    return of(this.currentUser);
  }
}