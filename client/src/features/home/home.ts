import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-home',
  imports: [RouterModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private http = inject(HttpClient);
  protected accountService = inject(AccountService);
  private router = inject(Router);
  protected title = 'Doctors';
  protected users = signal<any>([]);

  async ngOnInit() {
    // Only load doctors if the user is logged in
    if (this.accountService.currentUser()) {
      this.users.set(await this.getUsers());
    }
  }

  async getUsers() {
    return lastValueFrom(this.http.get('https://localhost:5001/api/doctors'));
  }

  logout() {
    this.accountService.logout().subscribe({
      next: () => {
        this.router.navigateByUrl('/login');
      },
      error: error => {
        console.error('Logout error', error);
        this.router.navigateByUrl('/login');
      },
    });
  }
}