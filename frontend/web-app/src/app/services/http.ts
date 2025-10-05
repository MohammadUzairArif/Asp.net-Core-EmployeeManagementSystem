import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IDepartment } from '../types/IDepartment';
import { IEmployee } from '../types/IEmployee';
import { environment } from '../../environments/environment';
import { IProfile, IProfileResponse } from '../types/IProfile';
import { IPagedData } from '../types/IPagedData';


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
  getEmployees(filter: any) {
    let params = new HttpParams({ fromObject: filter });

     return this.http.get<IPagedData<IEmployee>>(environment.apiUrl+'/api/Employee',{params});
  }

  addEmployee(employee: IEmployee){
    return this.http.post(environment.apiUrl+'/api/Employee',employee);
  }
  updateEmployee(id: number, employee: IEmployee) {
  return this.http.put(environment.apiUrl + `/api/Employee/${id}`, employee);
}

deleteEmployee(id: number) {
  return this.http.delete(environment.apiUrl + `/api/Employee/${id}`);  
}

updateProfile(profileDetails: IProfile) {
  return this.http.post(`${environment.apiUrl}/api/Profile`, profileDetails);
}
 getProfile(userId: string) {
  return this.http.get<IProfileResponse>(`${environment.apiUrl}/api/Profile/${userId}`);
}
}
