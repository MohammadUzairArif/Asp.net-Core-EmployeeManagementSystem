import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IDashboard } from '../types/IDashboard';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
   http = inject(HttpClient);

   getDashboardData(){
      return this.http.get<IDashboard>(`${environment.apiUrl}/api/Dashboard`);
    }
}
