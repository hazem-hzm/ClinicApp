import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { AccountService } from '../../../core/services/account-service';

type PatientDto = {
  id: string;
  displayName: string;
  dateOfBirth: string;
};

@Component({
  selector: 'app-patient-info',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './patient-info.html',
})
export class PatientInfo implements OnInit {
  private http = inject(HttpClient);
  protected accountService = inject(AccountService);

  protected loading = signal(true);
  protected error = signal<string | null>(null);
  protected patient = signal<PatientDto | null>(null);

  async ngOnInit() {
    await this.load();
  }

  private async load() {
    try {
      this.loading.set(true);
      this.error.set(null);
      const res = await lastValueFrom(
        this.http.get<PatientDto>('https://localhost:5001/api/patients/me')
      );
      this.patient.set(res);
    } catch {
      this.error.set('Failed to load patient info.');
    } finally {
      this.loading.set(false);
    }
  }
}

