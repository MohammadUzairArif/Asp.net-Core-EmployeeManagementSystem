import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApplyLeave } from '../types/ILeave';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class Leave {
   http = inject(HttpClient);
  applyLeave(leave: ApplyLeave) {
    return this.http.post(`${environment.apiUrl}/api/Leave/apply`, leave);
  }
}
