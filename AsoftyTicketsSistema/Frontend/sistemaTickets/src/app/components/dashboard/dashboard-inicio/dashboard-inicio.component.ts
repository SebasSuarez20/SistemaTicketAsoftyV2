import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
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
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { trigger, state, style, transition, animate } from "@angular/animations";
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-dashboard-inicio',
  templateUrl: './dashboard-inicio.component.html',
  styleUrls: ['./dashboard-inicio.component.css'],

  animations: [
    trigger('appearSmoothly', [
      state('NotVisible', style({
        opacity: 0,
        transform: 'scale(0.5)'
      })),
      state('yesVisible', style({
        opacity: 1,
        transform: 'scale(1)'
      })),
      transition('NotVisible <=> yesVisible', [
        animate('0.8s cubic-bezier(0.25, 0.8, 0.25, 1)')
      ])
    ])
  ]
})
export class DashboardInicioComponent implements OnInit, OnDestroy, AfterViewInit {

  public isVisible: boolean = false;
  public displayedColumns: string[] = [];
  public dataSource: MatTableDataSource<Partial<ITicketMapAndSup>>;
  public codeGenStatus!: ICodeGen[];
  public isValid: boolean = true;
  public isValidRol: boolean = false;
  public resultUsername: number[] = [];
  public suscription: Subscription;
  public strlUnique: string[] = [];
  public StatusVisible: string = 'NotVisible';
  public isVisibleLoading: boolean = true;
  public resMessage: string;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private codeGenericService: CodeGenService, private data_Service: LoginService,
    public dialog: MatDialog, private idle: Idle, private cd: ChangeDetectorRef, private serviceHttp: TicketsServicesHttpService,
    private hubConnection: HubConnectionService, private serviceObserver: ObserverService) {
    this.dataSource = new MatTableDataSource();
    if (this.data_Service.dataLogged().rolCode === 1) this.isValidRol = true;
    this.suscription = this.serviceObserver
      .getUniqueObservable()
      .subscribe(() => {
        this.GetAllMapAndSup();
      });
  }

  ngOnInit(): void {
    this.codeGenStatus = this.codeGenericService.loadCode('Status');
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.suscription.unsubscribe();
  }

  public triggerVisiblity() {
    this.StatusVisible = this.StatusVisible === 'NotVisible' ? 'yesVisible' : 'NotVisible';
    console.log(this.StatusVisible);
  }

  public loadingComponent() {

  }

  public OpenCreateTicket() {
    this.dialog.open(TicketsComponent);
  }

  public async UpdateItemTicket(event: any, data: ITicketMapAndSup, index: number) {

    this.hubConnection.invokeSendMessageToClient(parseInt(event.target.value), this.resultUsername[index], data['n.ticket']);

  }

  public navigateUrl(index: number) {
    console.log(this.strlUnique[index]);
  }


  public GetAllMapAndSup() {


    this.serviceHttp
      .connectApiGet(`ticketssupport/GetAllMapAndSup`)
      .then((res: any) => {
        if (res.status == 200) {
          let infoData: Partial<ITicketMapAndSup>[] = [];
          this.strlUnique = [];

          let respTable: Partial<ITicketMapAndSup> = {
            "n.ticket": null,
            area: '',
            prioridad: '',
            estado: '',
            asignacion: null,
            username: -1,
          };

          res.data.forEach((e) => {
            let element = { ...respTable };
            element['n.ticket'] = e.no;
            element.area = e.area;
            element.estado = e.estado;
            element.prioridad = e.prioridad;
            element.asignacion = e.asignacion;
            this.strlUnique.push(e.hasUnique);
            if (element.asignacion == -1) delete element.asignacion;
            if (this.isValid) this.resultUsername.push(e.username);
            delete element.username;
            infoData.push(element);
          });

          this.dataSource.data = infoData;
          this.displayedColumns =
            this.dataSource.data.length != 0
              ? Object.keys(this.dataSource.data[0])
              : Object.keys(respTable).filter((c) =>
                this.isValidRol != true ? c != 'asignacion' : c
              );

          if (!this.isValidRol) this.displayedColumns.push('Chat');

        } else if (res.status == 404) {
          this.resMessage = res.message;
        }

        setTimeout(() => {
          this.isVisibleLoading = false;
        }, 880);

      }).then(() => {
        setTimeout(() => {
          this.triggerVisiblity();
        }, 1000);
      });
  }
}
