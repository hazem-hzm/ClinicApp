import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ToastService } from '../../../core/services/toast-service';

interface Doctor {
  id: string;
  displayName: string;
  specialty: string;
  yearsOfExperience: number;
}

@Component({
  selector: 'app-doctor-appointment',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './doctor-appointment.html',
  styleUrl: './doctor-appointment.css',
})
export class DoctorAppointment implements OnInit {
  private route = inject(ActivatedRoute);
  private http = inject(HttpClient);
  private router = inject(Router);
  private toast = inject(ToastService);

  protected doctor = signal<Doctor | null>(null);
  protected submitting = signal(false);
  protected error = signal<string | null>(null);
  protected success = signal<string | null>(null);
  protected minDateTime = '';
  protected selectedDateTime = '';

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadDoctor(id);
    } else {
      this.error.set('Doctor not found in the URL.');
    }

    this.setMinDateTime();
  }

  private setMinDateTime() {
    const now = new Date();
    // Adjust for timezone so the value works with datetime-local
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    this.minDateTime = now.toISOString().slice(0, 16);
  }

  private loadDoctor(id: string) {
    this.http.get<Doctor>(`https://localhost:5001/api/doctors/${id}`).subscribe({
      next: doctor => this.doctor.set(doctor),
      error: () => this.error.set('Unable to load doctor details.'),
    });
  }

  bookAppointment() {
    this.error.set(null);
    this.success.set(null);

    const doctor = this.doctor();
    if (!doctor) {
      this.error.set('Doctor information is missing.');
      return;
    }

    if (!this.selectedDateTime) {
      this.error.set('Please choose a date and time.');
      return;
    }

    const date = new Date(this.selectedDateTime);

    const payload = {
      doctorId: doctor.id,
      date,
    };

    this.submitting.set(true);

    this.http.post('https://localhost:5001/api/appointments', payload).subscribe({
      next: () => {
        this.success.set('Appointment created successfully.');
        this.toast.success('Appointment created successfully.');
        this.submitting.set(false);
        setTimeout(() => this.router.navigateByUrl('/'), 1200);
      },
      error: err => {
        this.submitting.set(false);
        const msg =
          (err?.error && (typeof err.error === 'string' ? err.error : null)) ||
          'Failed to create appointment.';
        this.error.set(msg);
        this.toast.error(msg);
      },
    });
  }

  backToDoctors() {
    this.router.navigateByUrl('/patient/doctors');
  }
}