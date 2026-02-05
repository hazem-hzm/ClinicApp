import { inject, Injectable, signal } from '@angular/core';
import { AppUser } from '../../types/user';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  http = inject(HttpClient)

  register(creds: AppUser) {
    return this.http.post<AppUser>('https://localhost:5001/api/' + 'account/register', creds)
      }  
  }

