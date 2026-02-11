import { Routes } from '@angular/router';
import { App } from './app';
import { Register } from '../features/account/register/register';
import { Home } from '../features/home/home';
import { Login } from '../features/account/login/login';
import { DoctorAppointment } from '../features/appointments/doctor-appointment/doctor-appointment';
import { DoctorAppointments } from '../features/appointments/doctor-appointments/doctor-appointments';
import { DoctorDashboard } from '../features/doctor/doctor-dashboard/doctor-dashboard';
import { DoctorMedicalRecords } from '../features/medical-records/doctor-medical-records/doctor-medical-records';
import { PatientDashboard } from '../features/patient/patient-dashboard/patient-dashboard';
import { PatientAppointments } from '../features/appointments/patient-appointments/patient-appointments';
import { PatientMedicalRecords } from '../features/medical-records/patient-medical-records/patient-medical-records';
import { PatientDoctors } from '../features/doctors/patient-doctors/patient-doctors';
import { PatientInfo } from '../features/patient/patient-info/patient-info';
import { AdminDashboard } from '../features/admin/admin-dashboard/admin-dashboard';
import { AdminUsers } from '../features/admin/admin-users/admin-users';
import { AdminCreateDoctorUser } from '../features/admin/admin-create-doctor-user/admin-create-doctor-user';

export const routes: Routes = [
  { path: 'register', component: Register },
  { path: 'login', component: Login },
  {
    path: 'admin',
    component: AdminDashboard,
    children: [
      { path: 'users', component: AdminUsers },
      { path: 'create-doctor-user', component: AdminCreateDoctorUser },
      { path: '', pathMatch: 'full', redirectTo: 'users' },
    ],
  },
  {
    path: 'doctor',
    component: DoctorDashboard,
    children: [
      { path: 'appointments', component: DoctorAppointments },
      { path: 'records', component: DoctorMedicalRecords },
      { path: '', pathMatch: 'full', redirectTo: 'appointments' },
    ],
  },
  {
    path: 'patient',
    component: PatientDashboard,
    children: [
      { path: 'appointments', component: PatientAppointments },
      { path: 'records', component: PatientMedicalRecords },
      { path: 'doctors', component: PatientDoctors },
       { path: 'info', component: PatientInfo },
      { path: '', pathMatch: 'full', redirectTo: 'appointments' },
    ],
  },
  { path: 'doctors/:id', component: DoctorAppointment },
  { path: '**', component: Home },
];