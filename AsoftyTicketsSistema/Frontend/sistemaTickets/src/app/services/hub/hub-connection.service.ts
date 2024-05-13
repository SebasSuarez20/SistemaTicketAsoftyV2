import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class HubConnectionService {


  private connectionHub!: HubConnection;

  constructor() {
    this.connectionHub = new HubConnectionBuilder().
      withUrl('https://localhost:7026/realTime').
      build();
  }

  public connectionStart(id: number) {
    this.connectionHub.start().then(() => {
      this.SignalRlistening(1);
      this.SignalRlistening(2);
      this.invokeUpdate(id);
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
    if (listening === 2) this.connectionHub.on("192.168.0.2", (d) => { })
  }

  public invokeUpdate(idHub?: number) {
    this.connectionHub.invoke("UpdateLogged", idHub).then((r) => { });
  }


}
