import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { lastValueFrom } from 'rxjs';
import { UserDto } from '../../../types/user';

type CreateDoctorPayload = {
  userId: string;
  specialty: string;
  yearsOfExperience: number;
};

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-users.html',
})
export class AdminUsers implements OnInit {
  private http = inject(HttpClient);

  protected loading = signal(true);
  protected error = signal<string | null>(null);
  protected success = signal<string | null>(null);
  protected users = signal<UserDto[]>([]);

  // For promoting a selected user to doctor
  protected selectedUserId = signal<string | null>(null);
  protected specialty = '';
  protected yearsOfExperience: number | null = null;

  async ngOnInit() {
    await this.loadUsers();
  }

  private async loadUsers() {
    try {
      this.loading.set(true);
      this.error.set(null);
      const res = await lastValueFrom(
        this.http.get<UserDto[]>('https://localhost:5001/api/admin/users')
      );
      this.users.set(res);
    } catch {
      this.error.set('Failed to load users.');
    } finally {
      this.loading.set(false);
    }
  }

  startPromote(userId: string) {
    this.selectedUserId.set(userId);
    this.specialty = '';
    this.yearsOfExperience = null;
    this.success.set(null);
    this.error.set(null);
  }

  cancelPromote() {
    this.selectedUserId.set(null);
  }

  async createDoctor() {
    const userId = this.selectedUserId();
    const specialty = (this.specialty ?? '').trim();
    const years = this.yearsOfExperience;

    if (!userId) {
      this.error.set('No user selected.');
      return;
    }

    if (!specialty || !years || years <= 0) {
      this.error.set('Please enter specialty and a valid years of experience.');
      return;
    }

    const payload: CreateDoctorPayload = {
      userId,
      specialty,
      yearsOfExperience: years as number,
    };

    try {
      this.error.set(null);
      await lastValueFrom(
        this.http.post('https://localhost:5001/api/doctors', payload)
      );
      this.success.set('Doctor profile created successfully.');
      this.selectedUserId.set(null);
    } catch (e: any) {
      const msg =
        (e?.error && (typeof e.error === 'string' ? e.error : null)) ||
        'Failed to create doctor profile.';
      this.error.set(msg);
    }
  }
}

