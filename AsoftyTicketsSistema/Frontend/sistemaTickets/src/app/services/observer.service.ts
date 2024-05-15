import { Injectable } from '@angular/core';
import { Subject } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ObserverService {

  public data = new BehaviorSubject<void>(null);

  public getObservable(): void {
    this.data.next();
  }

  public getUniqueObservable(): Observable<void> {
    return this.data.asObservable();
  }
}
