import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IDepartment } from '../types/IDepartment';

@Injectable({
  providedIn: 'root'
})

export class HttpService {
  http = inject(HttpClient)


  getDepartments() {
    return this.http.get<IDepartment[]>('https://localhost:7272/api/Department');
  }
}
