import { Routes } from '@angular/router';
import { App } from './app';
import { Register } from '../features/account/register/register';
import { Home } from '../features/home/home';
import { Login } from '../features/account/login/login';

export const routes: Routes = [
    { path: 'register', component: Register },
    { path: 'login', component: Login },
    { path: '**', component: Home },
];