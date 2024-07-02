import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  public informationInactive = new BehaviorSubject<boolean>(false);

  constructor() {
    this.informationInactive.next(false);
  }

  public loggedSystem(): boolean {
    let isLooged = sessionStorage.getItem('token');
    if (isLooged) return true;
    return false;
  }

  public dataLogged() {
    let data_ = sessionStorage.getItem('_data');
    return JSON.parse(data_);
  }

  public closeSession() {
    sessionStorage.removeItem('_data');
    sessionStorage.removeItem('token');
    sessionStorage.removeItem('_theme');
  }
}
