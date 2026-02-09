import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoginUser } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';
import { Router } from '@angular/router';

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
        this.router.navigateByUrl('/doctors');
      },
      error: error => {
        console.error('Login error', error);
      },
    });
  }
}

