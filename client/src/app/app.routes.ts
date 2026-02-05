import { Routes } from '@angular/router';
import { App } from './app';
import { Register } from '../features/account/register/register';
import { Home } from '../features/home/home';

export const routes: Routes = [
    { path: 'register', component: Register},
    { path: '**', component: Home },
];