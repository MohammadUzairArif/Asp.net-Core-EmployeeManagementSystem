import { inject, Injectable } from '@angular/core';
import { IPagedData } from '../types/IPagedData';
import { IAttendance } from '../types/IAttendance';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {

  http = inject(HttpClient);
  markAttendance() {
  return this.http.post(`${environment.apiUrl}/api/Attendence/mark-present`,{});

}
  getAttendanceHistory(filter:any) {
    let params = new HttpParams({ fromObject: filter });
    return this.http.get<IPagedData<IAttendance>>(`${environment.apiUrl}/api/Attendence`, { params });
  }
  
}