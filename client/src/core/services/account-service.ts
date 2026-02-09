import { inject, Injectable, signal } from '@angular/core';
import { AppUser, LoginUser, UserDto } from '../../types/user';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { TokenService } from './token-service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  http = inject(HttpClient);
  private tokenService = inject(TokenService);
  private baseUrl = 'https://localhost:5001/api/account';

  currentUser = signal<UserDto | null>(null);

  register(creds: AppUser) {
    return this.http.post<UserDto>(`${this.baseUrl}/register`, creds).pipe(
      tap(user => {
        this.currentUser.set(user);
        this.tokenService.setToken(user.token);
      })
    );
  }

  login(creds: LoginUser) {
    return this.http.post<UserDto>(`${this.baseUrl}/login`, creds).pipe(
      tap(user => {
        this.currentUser.set(user);
        this.tokenService.setToken(user.token);
      })
    );
  }

  logout() {
    this.currentUser.set(null);
    this.tokenService.setToken(null);
    return this.http.post(`${this.baseUrl}/logout`, {});
  }
}

