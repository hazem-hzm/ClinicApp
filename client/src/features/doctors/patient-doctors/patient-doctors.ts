import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { lastValueFrom } from 'rxjs';

type Doctor = {
  id: string;
  displayName: string;
  specialty: string;
  yearsOfExperience: number;
};

@Component({
  selector: 'app-patient-doctors',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './patient-doctors.html',
})
export class PatientDoctors implements OnInit {
  private http = inject(HttpClient);

  protected loading = signal(true);
  protected error = signal<string | null>(null);
  protected doctors = signal<Doctor[]>([]);

  async ngOnInit() {
    await this.load();
  }

  private async load() {
    try {
      this.loading.set(true);
      this.error.set(null);
      const res = await lastValueFrom(
        this.http.get<Doctor[]>('https://localhost:5001/api/doctors')
      );
      this.doctors.set(res);
    } catch {
      this.error.set('Failed to load doctors.');
    } finally {
      this.loading.set(false);
    }
  }
}

