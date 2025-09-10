import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IDepartment } from '../types/IDepartment';

@Injectable({
  providedIn: 'root'
})

export class HttpService {
  http = inject(HttpClient)
  apiUrl = 'https://localhost:7272'

  getDepartments() {
    return this.http.get<IDepartment[]>(this.apiUrl+'/api/Department');
  }
  addDepartment(name:string){
    return this.http.post(this.apiUrl+'/api/Department',{name:name});
  }
  updateDepartment(id: number, name: string) {
  return this.http.put(this.apiUrl + `/api/Department/${id}`, { id: id, name: name });
}
deleteDepartment(id: number) {
  return this.http.delete(this.apiUrl + `/api/Department/${id}`); }
}
