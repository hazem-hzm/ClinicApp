import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { lastValueFrom } from 'rxjs';

type AppointmentDto = {
  id: number;
  date: string; // ISO string from API
  status: string;
  patientName: string;
  doctorName: string;
};

@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './doctor-appointments.html',
})
export class DoctorAppointments implements OnInit {
  private http = inject(HttpClient);

  protected loading = signal(true);
  protected error = signal<string | null>(null);
  protected appointments = signal<AppointmentDto[]>([]);

  protected readonly statuses = ['Pending', 'Accepted', 'Rejected', 'Cancelled', 'Completed'] as const;

  async ngOnInit() {
    await this.load();
  }

  private async load() {
    try {
      this.loading.set(true);
      this.error.set(null);
      const res = await lastValueFrom(
        this.http.get<AppointmentDto[]>('https://localhost:5001/api/appointments/doctor')
      );
      const sorted = [...res].sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
      this.appointments.set(sorted);
    } catch (e) {
      this.error.set('Failed to load appointments.');
    } finally {
      this.loading.set(false);
    }
  }

  async updateStatus(id: number, status: string) {
    try {
      this.error.set(null);
      await lastValueFrom(
        this.http.patch(
          `https://localhost:5001/api/appointments/${id}/status?status=${encodeURIComponent(status)}`,
          {}
        )
      );
      // update locally
      this.appointments.update(items =>
        items.map(a => (a.id === id ? { ...a, status } : a))
      );
    } catch (e: any) {
      const msg =
        (e?.error && (typeof e.error === 'string' ? e.error : null)) || 'Failed to update status.';
      this.error.set(msg);
    }
  }
}

