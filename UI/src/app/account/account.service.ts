import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Register } from '../shared/models/register';
import { environment } from 'src/environments/environment.development';
import { Login } from '../shared/models/login';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  register(model: Register) {
    return this.http.post(`${environment.apiUrl}account/register`, model);
  }

  login(model: Login) {
    return this.http.post(`${environment.apiUrl}account/login`, model);
  }
}
