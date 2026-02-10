import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { lastValueFrom } from 'rxjs';

type AppointmentDto = {
  id: number;
  date: string;
  status: string;
  patientName: string;
  doctorName: string;
};

@Component({
  selector: 'app-patient-appointments',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './patient-appointments.html',
})
export class PatientAppointments implements OnInit {
  private http = inject(HttpClient);

  protected loading = signal(true);
  protected error = signal<string | null>(null);
  protected appointments = signal<AppointmentDto[]>([]);

  async ngOnInit() {
    await this.load();
  }

  private async load() {
    try {
      this.loading.set(true);
      this.error.set(null);
      const res = await lastValueFrom(
        this.http.get<AppointmentDto[]>('https://localhost:5001/api/patients/me/appointments')
      );
      const sorted = [...res].sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
      this.appointments.set(sorted);
    } catch {
      this.error.set('Failed to load appointments.');
    } finally {
      this.loading.set(false);
    }
  }

  async cancel(id: number) {
    try {
      this.error.set(null);
      await lastValueFrom(
        this.http.delete(`https://localhost:5001/api/appointments/${id}`)
      );
      this.appointments.update(items =>
        items.map(a => (a.id === id ? { ...a, status: 'Cancelled' } : a))
      );
    } catch {
      this.error.set('Failed to cancel appointment.');
    }
  }
}

