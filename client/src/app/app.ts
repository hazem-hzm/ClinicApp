import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Register } from '../features/account/register/register';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected title = 'Users';
  protected users = signal<any>([])
  protected router = inject(Router);

  async ngOnInit() {
    this.users.set(await this.getUsers())
  }

  async getUsers() {
    return lastValueFrom(this.http.get('https://localhost:5001/api/doctors'));
  }
}