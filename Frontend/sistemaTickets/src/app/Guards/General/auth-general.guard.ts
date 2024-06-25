import { HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginService } from 'src/app/services/login.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGeneralGuard implements CanActivate {

  public request: HttpRequest<any>;

  public constructor(private serviceLogged: LoginService, private router: Router) {
    this.serviceLogged.informationInactive.next(false);
  }
  canActivate(): boolean {
    if (this.serviceLogged.loggedSystem()) {
      return false;
    }
    return true;
  }



}
