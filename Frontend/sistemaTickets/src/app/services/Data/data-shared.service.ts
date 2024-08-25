import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataSharedService {

  public dataShared = new BehaviorSubject("");
  public tokenShared = new BehaviorSubject("");
  public suscription = new Subscription;

  constructor() { }

  public release() {
    this.tokenShared.next("");
  }

  public setToken(t: string): void {
    this.tokenShared.next(t);
  }

  public getToken(): string {
    return this.tokenShared.value;
  }


}
