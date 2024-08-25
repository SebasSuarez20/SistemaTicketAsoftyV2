import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { DataSharedService } from '../services/Data/data-shared.service';

@Injectable()
export class AuthInterceptorInterceptor implements HttpInterceptor {

  constructor(private data: DataSharedService) { }

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let getToken = sessionStorage.getItem('token');

    const authRequest = request.clone({
      setHeaders: {
        Authorization: `Bearer ${getToken ?? this.data.getToken()}`
      }
    });

    if (!getToken) {
      if (this.data.getToken().length != 0) {
        return next.handle(authRequest);
      }
      return next.handle(request);
    } else {
      return next.handle(authRequest);
    }
  }
}

