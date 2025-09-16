import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IDepartment } from '../types/IDepartment';
import { IEmployee } from '../types/IEmployee';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})

export class HttpService {
  http = inject(HttpClient)
  

  // Department API

  getDepartments() {
    return this.http.get<IDepartment[]>(environment.apiUrl+'/api/Department');
  }
  addDepartment(name:string){
    return this.http.post(environment.apiUrl+'/api/Department',{name:name});
  }
  updateDepartment(id: number, name: string) {
  return this.http.put(environment.apiUrl + `/api/Department/${id}`, { id: id, name: name });
}
deleteDepartment(id: number) {
  return this.http.delete(environment.apiUrl + `/api/Department/${id}`); 
}

// Employee API
  getEmployees() {
    return this.http.get<IEmployee[]>(environment.apiUrl+'/api/Employee');
  }

  addEmployee(employee: IEmployee){
    return this.http.post(environment.apiUrl+'/api/Employee',employee);
  }
  updateEmployee(id: number, name: string) {
  return this.http.put(environment.apiUrl + `/api/Employee/${id}`, { id: id, name: name });  
}
deleteEmployee(id: number) {
  return this.http.delete(environment.apiUrl + `/api/Employee/${id}`);  
}
}
