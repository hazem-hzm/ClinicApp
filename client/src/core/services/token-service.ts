import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private readonly storageKey = 'access_token';
  tokenChanged = signal<string | null>(this.getToken());

  setToken(token: string | null) {
    if (token) {
      localStorage.setItem(this.storageKey, token);
    } else {
      localStorage.removeItem(this.storageKey);
    }
    this.tokenChanged.set(token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.storageKey);
  }

}

