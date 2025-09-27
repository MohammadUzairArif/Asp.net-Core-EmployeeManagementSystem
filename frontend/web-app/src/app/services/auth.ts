import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IAuthCredentials } from '../types/IAuthCredentials';
import { IAuthToken } from '../types/IAuthToken';
import { environment } from '../../environments/environment.development';


@Injectable({
  providedIn: 'root'
})
export class Auth {
  http = inject(HttpClient);

  login(credentials: IAuthCredentials){
   return this.http.post<IAuthToken>(`${environment.apiUrl}/api/Auth/login`, credentials);
  }

  saveToken(authToken: IAuthToken) {
  localStorage.setItem("auth", JSON.stringify(authToken));
  localStorage.setItem("token", authToken.token);
}
logOut(){
  localStorage.removeItem("auth");
  localStorage.removeItem("token");
}
get isLoggedIn() {
  return localStorage.getItem('token') ? true : false;
}
get isEmployee() {
  return JSON.parse(localStorage.getItem('auth')!)?.role === 'Employee';
}
}
