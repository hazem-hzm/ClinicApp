import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';

type MedicalRecordDto = {
  id: number;
  diagnosis: string;
  treatment: string | null;
  createdAt: string;
  doctorName: string;
};

@Component({
  selector: 'app-patient-medical-records',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './patient-medical-records.html',
})
export class PatientMedicalRecords implements OnInit {
  private http = inject(HttpClient);

  protected loading = signal(true);
  protected error = signal<string | null>(null);
  protected records = signal<MedicalRecordDto[]>([]);

  async ngOnInit() {
    await this.load();
  }

  private async load() {
    try {
      this.loading.set(true);
      this.error.set(null);
      const res = await lastValueFrom(
        this.http.get<MedicalRecordDto[]>('https://localhost:5001/api/patients/me/medical-records')
      );
      const sorted = [...res].sort(
        (a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
      );
      this.records.set(sorted);
    } catch {
      this.error.set('Failed to load medical records.');
    } finally {
      this.loading.set(false);
    }
  }
}

