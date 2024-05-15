import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { elementAt, map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { CodeGenService } from 'src/app/services/code-gen.service';
import { ICodeGen } from 'src/app/Model/ICodeGen';
import { MatDialog } from '@angular/material/dialog';
import { TicketsComponent } from '../../tickets/tickets/tickets.component';
import { LoginService } from 'src/app/services/login.service';
import { Idle } from '@ng-idle/core';
import { TicketsServicesHttpService } from 'src/app/services/ticketsServicesHttp/tickets-services-http.service';
import { MatTableDataSource } from '@angular/material/table';
import { ITicketMapAndSup } from 'src/app/Model/ITicketMapAndSup';
import { HubConnectionService } from 'src/app/services/hub/hub-connection.service';
import { ObserverService } from 'src/app/services/observer.service';

@Component({
  selector: 'app-dashboard-inicio',
  templateUrl: './dashboard-inicio.component.html',
  styleUrls: ['./dashboard-inicio.component.css']
})
export class DashboardInicioComponent implements OnInit {
  /** Based on the screen size, switch from standard to one column per row */
  cards = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(({ matches }) => {
      if (matches) {
        return [
          { title: 'Card 1', cols: 1, rows: 1 },
          { title: 'Card 2', cols: 1, rows: 1 },
          { title: 'Card 3', cols: 1, rows: 1 },
          { title: 'Card 4', cols: 1, rows: 1 }
        ];
      }

      return [
        { title: 'Card 1', cols: 2, rows: 1 },
        { title: 'Card 2', cols: 1, rows: 1 },
        { title: 'Card 3', cols: 1, rows: 2 },
        { title: 'Card 4', cols: 1, rows: 1 }
      ];
    })
  );

  public isVisible: boolean = false;
  public displayedColumns: string[] = [];
  public dataSource: MatTableDataSource<Partial<ITicketMapAndSup>>;
  public codeGenStatus!: ICodeGen[];
  public isValid: boolean = true;
  public isValidRol: boolean = false;
  public resultUsername: number[] = [];

  constructor(private breakpointObserver: BreakpointObserver, private codeGenericService: CodeGenService, private data_Service: LoginService,
    public dialog: MatDialog, private idle: Idle, private cd: ChangeDetectorRef, private serviceHttp: TicketsServicesHttpService,
    private hubConnection: HubConnectionService, private serviceObserver: ObserverService) {
    this.dataSource = new MatTableDataSource();
    if (this.data_Service.dataLogged().rolCode === 1) this.isValidRol = true;
    this.serviceObserver.getUniqueObservable().subscribe(() => {
      this.GetAllMapAndSup();
    });
  }


  ngOnInit(): void {
    this.codeGenStatus = this.codeGenericService.loadCode('Status');
    this.getStatusApp();
    this.GetAllMapAndSup();
  }


  public OpenCreateTicket() {
    this.dialog.open(TicketsComponent)
  }


  private getStatusApp() {
    this.idle.watch();
  }

  public UpdateItemTicket(event: any, data: ITicketMapAndSup, index: number) {

    this.hubConnection.invokeSendMessageToClient(parseInt(event.target.value), this.resultUsername[index], data.no);

  }

  public GetAllMapAndSup() {

    this.serviceHttp.connectApiGet(`ticketssupport/GetAllMapAndSup`).then((res: Partial<ITicketMapAndSup[]>) => {

      let infoData: Partial<ITicketMapAndSup>[] = [];

      let respTable: Partial<ITicketMapAndSup> = {
        no: null,
        nombre: '',
        descripcion: '',
        estado: '',
        asignacion: null,
        username: -1
      };

      res.forEach(e => {
        let element = { ...respTable };
        element.no = e.no;
        element.nombre = e.nombre;
        element.estado = e.estado;
        element.descripcion = e.descripcion;
        element.asignacion = e.asignacion;
        if (element.asignacion == -1) delete element.asignacion;
        if (this.isValid) this.resultUsername.push(e.username);
        delete element.username;
        infoData.push(element);
      });

      this.dataSource.data = infoData;

      this.displayedColumns = this.dataSource.data.length != 0 ? Object.keys(this.dataSource.data[0]) :
        Object.keys(respTable).filter(c => this.isValidRol != true ? c != 'asignacion' : c);
      if (!this.isValidRol) this.displayedColumns.push("Acciones")
    })
  }

}
