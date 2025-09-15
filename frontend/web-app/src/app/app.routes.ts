import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Departments } from './pages/departments/departments';
import { Employee } from './pages/employee/employee';

export const routes: Routes = [
    {
        path: '',
        component:Home
    },
    {
        path:'departments',
        component:Departments

    },
    {
        path:'employees',
        component:Employee
    }
];
