import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { lastValueFrom } from 'rxjs';

type PatientOption = {
  id: string;
  name: string;
};

@Component({
  selector: 'app-doctor-medical-records',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './doctor-medical-records.html',
})
export class DoctorMedicalRecords implements OnInit {
  private http = inject(HttpClient);

  protected patients = signal<PatientOption[]>([]);
  protected loadingPatients = signal(true);
  protected submitting = signal(false);
  protected error = signal<string | null>(null);
  protected success = signal<string | null>(null);

  protected form = signal({
    patientId: '',
    diagnosis: '',
    treatment: '',
  });

  async ngOnInit() {
    await this.loadPatients();
  }

  private async loadPatients() {
    try {
      this.loadingPatients.set(true);
      const res = await lastValueFrom(
        this.http.get<any[]>('https://localhost:5001/api/patients')
      );
      this.patients.set(
        res.map(p => ({
          id: p.id as string,
          name: p.displayName as string,
        }))
      );
    } catch (e) {
      this.error.set('Failed to load patients list.');
    } finally {
      this.loadingPatients.set(false);
    }
  }

  async submit() {
    this.error.set(null);
    this.success.set(null);

    const value = this.form();
    if (!value.patientId || !value.diagnosis.trim()) {
      this.error.set('Please select a patient and enter a diagnosis.');
      return;
    }

    const payload = {
      patientId: value.patientId,
      diagnosis: value.diagnosis.trim(),
      treatment: value.treatment.trim(),
    };

    this.submitting.set(true);

    try {
      await lastValueFrom(
        this.http.post('https://localhost:5001/api/medicalrecords', payload)
      );
      this.success.set('Medical record created successfully.');
      this.form.set({
        patientId: '',
        diagnosis: '',
        treatment: '',
      });
    } catch (e: any) {
      const msg =
        (e?.error && (typeof e.error === 'string' ? e.error : null)) ||
        'Failed to create medical record.';
      this.error.set(msg);
    } finally {
      this.submitting.set(false);
    }
  }
}

