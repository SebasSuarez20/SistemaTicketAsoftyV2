import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import Swal from 'sweetalert2';
import { ObserverService } from '../observer.service';

@Injectable({
  providedIn: 'root'
})
export class HubConnectionService {


  private connectionHub!: HubConnection;

  constructor(private serviceObserver: ObserverService) {
    this.connectionHub = new HubConnectionBuilder().
      withUrl('https://localhost:7026/realTime').
      build();
  }

  public connectionStart(id: number) {
    this.connectionHub.start().then(() => {
      this.SignalRlistening(1);
      this.invokeUpdate(id);
      this.test();
    }).catch(() => {
      console.error('Connection Error.');
    });
  }

  public closeConnection() {
    this.connectionHub.stop().then(() => {
      console.log('conexion cerrada');
    })
  }

  public SignalRlistening(listening: number) {
    if (listening === 1) this.connectionHub.on("192.168.0.1", (d) => { })
  }

  public test() {
    this.connectionHub.on("192.168.0.2", (d) => {
      if (d != true) {
        Swal.fire({
          icon: 'info',
          text: d
        }).then(() => {
          this.serviceObserver.getObservable();
        })
      }
    })
  }

  public invokeUpdate(idHub?: number) {
    this.connectionHub.invoke("UpdateLogged", idHub).then((r) => { });
  }

  public invokeSendMessageToClient(assigned: number, usernmae: number, consecutive: number) {
    this.connectionHub.invoke("SendMessageToClient", assigned, usernmae, consecutive).then((r) => { })
  }


}
