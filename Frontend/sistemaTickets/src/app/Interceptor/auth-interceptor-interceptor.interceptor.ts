import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptorInterceptor implements HttpInterceptor {

  constructor() { }

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let getToken = sessionStorage.getItem('token');
    let isLoginrequest = request.url.includes('login');

    if (isLoginrequest || !getToken) {
      return next.handle(request);
    } else {
      const authRequest = request.clone({
        setHeaders: {
          Authorization: `Bearer ${getToken}`
        }
      });
      return next.handle(authRequest);
    }
  }
}

