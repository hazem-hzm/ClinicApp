import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoginUser } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';
import { Router } from '@angular/router';
import { getRolesFromToken } from '../../../core/utils/jwt';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  standalone: true,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private accountService = inject(AccountService);
  private router = inject(Router);

  protected creds: LoginUser = {
    email: '',
    password: '',
  };

  login() {
    this.accountService.login(this.creds).subscribe({
      next: response => {
        console.log('Logged in user', response);
        const roles = getRolesFromToken(response.token);
        if (roles.includes('ADMIN')) {
          this.router.navigateByUrl('/admin');
        } else if (roles.includes('DOCTOR')) {
          this.router.navigateByUrl('/doctor');
        } else if (roles.includes('PATIENT')) {
          this.router.navigateByUrl('/patient');
        } else {
          this.router.navigateByUrl('/');
        }
      },
      error: error => {
        console.error('Login error', error);
      },
    });
  }
}

