import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { DataEncryptionService } from 'src/app/services/Encryption/data-encryption.service';
import { LoginService } from 'src/app/services/login.service';

@Injectable({
  providedIn: 'root'
})
export class GuardRoutingGuard implements CanActivate {

  public constructor(private logged: LoginService, private router: Router, private encryptService: DataEncryptionService) { }

  canActivate(): boolean {
    if (this.logged.loggedSystem()) return true;
    let routstrl = `/${this.encryptService.Getencryption('login')}`;
    this.router.navigateByUrl(routstrl);
    return false;
  }

}
