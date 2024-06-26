import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { LoginService } from 'src/app/services/login.service';
import { AccessRolUserDirective } from 'src/app/Directive/access-rol-user.directive';
import { Router } from '@angular/router';
import { DEFAULT_INTERRUPTSOURCES, Idle } from '@ng-idle/core';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';
import { LibraryMessageService } from 'src/app/services/ToastServices/library-message.service';
import { HubConnection } from '@microsoft/signalr';
import { HubConnectionService } from 'src/app/services/hub/hub-connection.service';
import { DataEncryptionService } from 'src/app/services/Encryption/data-encryption.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {

  public nameUser: string = '';
  public photoProfile: string = '';
  public role: number;
  public rol: string = ""
  public MONTH: string[] = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio",
    "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
  public date: Date;


  constructor(private breakpointObserver: BreakpointObserver, private data_Service: LoginService, private router: Router, private idle: Idle, private cd: ChangeDetectorRef,
    public dialog: MatDialog, private toast: LibraryMessageService, private hubconnection: HubConnectionService, public encryptService: DataEncryptionService) {
    this.date = new Date();
    this.nameUser = `${this.data_Service.dataLogged()?.nameUser ?? ""} ${this.data_Service.dataLogged()?.surName ?? ""}`;
    this.photoProfile = this.data_Service.dataLogged()?.photo ?? "";



    this.role = this.data_Service.dataLogged().rolCode;

    if (this.role == 1) this.rol = "Administrador";
    else if (this.role == 2) this.rol = "Soporte";
    else if (this.role == 3) this.rol = "Empresa";

    this.idle.setIdle(600);
    this.idle.setTimeout(5);
    this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);


    this.idle.onIdleEnd.subscribe(() => {
      cd.detectChanges();
      Swal.close();
    });


    this.idle.onTimeout.subscribe(() => {
      this.dialog.closeAll();
      this.toast.InfoMessagge("La sesión se está cerrando", "Adios!!").then(() => {
      }).then(() => {
        this.signIn();
      })
    });

    this.idle.onIdleStart.subscribe(() => {
      Swal.fire({
        icon: 'warning',
        title: 'Atencion!!!',
        text: 'Has estado inactivo por un tiempo. La sesión se cerrará en 5 segundos. Para continuar, realiza alguna acción.',
        timer: 5000,
        toast: true,
        timerProgressBar: true,
        showCancelButton: false,
        showConfirmButton: false
      })
    });

    setTimeout(() => {
      this.hubconnection.connectionStart(this.data_Service.dataLogged().idControl);
    }, 1000);
  }

  public validatorActive?: number;

  ngOnInit(): void {
    this.idle.watch();
    this.effectDashboard();
    this.validatorActive = 2;
    document.body.classList.toggle(this.validatorActive % 2 != 0 ? 'white-theme-variables' : 'dark-theme-variables');
  }


  ngOnDestroy(): void {
    this.hubconnection.closeConnection();
    this.idle.stop();
  }

  public effectDashboard() {
    const sideMenu = document.querySelector("aside")
    const menuBtn = document.querySelector("#menu-btn")
    const closeBtn = document.querySelector("#close-btn")
    const themeThoggler = document.querySelector(".theme-toggler")

    //show sidebar
    menuBtn.addEventListener('click', () => {
      sideMenu.style.display = 'block'
    })

    //Close sidebar
    closeBtn.addEventListener('click', () => {
      sideMenu.style.display = 'none'
    })

    //change theme
    themeThoggler.addEventListener('click', () => {
      document.body.classList.toggle('white-theme-variables');

      themeThoggler.querySelector('span:nth-child(1)').classList.toggle('active');
      themeThoggler.querySelector('span:nth-child(2)').classList.toggle('active');
    })

  }

  public signIn() {

    Swal.fire({
      title: '¿Esta seguro de cerrar sesion?',
      icon: 'info',
      toast: true,
      showCancelButton: true,
      showConfirmButton: true
    }).then((result) => {
      if (result.isConfirmed) {
        this.data_Service.closeSession();
        let routstrl = `/${this.encryptService.getEncryption('login')}`;
        this.router.navigateByUrl(routstrl);
      }
    })


  }






}
