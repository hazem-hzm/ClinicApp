import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { lastValueFrom } from 'rxjs';
import { AppUser, UserDto } from '../../../types/user';

type CreateDoctorPayload = {
  userId: string;
  specialty: string;
  yearsOfExperience: number;
};

@Component({
  selector: 'app-admin-create-doctor-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-create-doctor-user.html',
})
export class AdminCreateDoctorUser {
  private http = inject(HttpClient);

  protected creds = {} as AppUser;
  protected specialty = '';
  protected yearsOfExperience: number | null = null;

  protected loading = false;
  protected error: string | null = null;
  protected success: string | null = null;

  async submit() {
    this.error = null;
    this.success = null;

    const { email, displayName, password } = this.creds;

    if (!email || !displayName || !password) {
      this.error = 'Please fill in email, display name and password.';
      return;
    }

    if (!this.specialty.trim() || !this.yearsOfExperience || this.yearsOfExperience <= 0) {
      this.error = 'Please enter specialty and a valid years of experience.';
      return;
    }

    this.loading = true;

    try {
      // 1) Register the user
      const registeredUser = await lastValueFrom(
        this.http.post<UserDto>('https://localhost:5001/api/account/register', this.creds)
      );

      // 2) Create the doctor profile for that user
      const payload: CreateDoctorPayload = {
        userId: registeredUser.id,
        specialty: this.specialty.trim(),
        yearsOfExperience: this.yearsOfExperience as number,
      };

      await lastValueFrom(
        this.http.post('https://localhost:5001/api/doctors', payload)
      );

      this.success = 'Doctor user created successfully.';

      // Reset form
      this.creds = {} as AppUser;
      this.specialty = '';
      this.yearsOfExperience = null;
    } catch (e: any) {
      const msg =
        (e?.error && (typeof e.error === 'string' ? e.error : null)) ||
        'Failed to create doctor user.';
      this.error = msg;
    } finally {
      this.loading = false;
    }
  }
}

