import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Departments } from './pages/departments/departments';

export const routes: Routes = [
    {
        path: '',
        component:Home
    },
    {
        path:'departments',
        component:Departments

    }
];
