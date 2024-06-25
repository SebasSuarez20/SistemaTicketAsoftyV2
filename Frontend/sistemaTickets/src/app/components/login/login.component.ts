import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import { IUser } from 'src/app/Model/IUser';
import { DataEncryptionService } from 'src/app/services/Encryption/data-encryption.service';
import { HubConnectionService } from 'src/app/services/hub/hub-connection.service';
import { LoginService } from 'src/app/services/login.service';
import { TicketsServicesHttpService } from 'src/app/services/ticketsServicesHttp/tickets-services-http.service';
import { LibraryMessageService } from 'src/app/services/ToastServices/library-message.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public formlogin: FormGroup;

  constructor(private router: Router, private ticketsService: TicketsServicesHttpService, private toast: LibraryMessageService, private logged: LoginService, private encryptService: DataEncryptionService) {
    this.formlogin = new FormGroup({
      user: new FormControl("", Validators.required),
      password: new FormControl("", Validators.required)
    });
    this.logged.informationInactive.next(false);
    this.router.navigateByUrl('/login');
  }
  ngOnInit(): void {
    
  }

  public async accessLogin() {
    await this.ticketsService.connectApiGet(`login/authService?user=${this.formlogin.get('user').value}&pswd=${this.formlogin.get('password').value}`).then(async (res: IUser) => {
      if (res.status === 200) {
        this.toast.successMessage(res.message, ' Felicidades!!! ').then(() => {
          sessionStorage.setItem('_data', JSON.stringify(res));
          sessionStorage.setItem('token', res.token);
          this.router.navigateByUrl(`/${this.encryptService.getEncryption("Ticket")}`);
        })
      } else {
        Swal.fire({
          icon: 'error',
          text: res.message,
          toast: true,
          timer: 1400,
          timerProgressBar: true,
          showCancelButton: false,
          showConfirmButton: false
        }).then(() => {
          this.formlogin.reset();
        })
      }
    });
  }


}
