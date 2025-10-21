import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IDashboard, IDepartmentDashboard } from '../types/IDashboard';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
   http = inject(HttpClient);

   getDashboardData(){
      return this.http.get<IDashboard>(`${environment.apiUrl}/api/Dashboard`);
    }
     getDepartmentData(){
      return this.http.get<IDepartmentDashboard[]>(`${environment.apiUrl}/api/Dashboard/department-data`);
    }
}
