import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private http = inject(HttpClient);
  protected title = 'Users';
  protected users = signal<any>([])

  async ngOnInit() {
    this.users.set(await this.getUsers())
  }

  async getUsers() {
    return lastValueFrom(this.http.get('https://localhost:5001/api/admin/users'));
  }
}