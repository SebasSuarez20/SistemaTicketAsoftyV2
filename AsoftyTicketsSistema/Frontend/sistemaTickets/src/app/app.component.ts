import { Component } from '@angular/core';
import { Route, Router } from '@angular/router';
import { LoginService } from './services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  public constructor(private router: Router, public islogged: LoginService) {
    this.islogged.informationInactive.next(false);
  }


}
