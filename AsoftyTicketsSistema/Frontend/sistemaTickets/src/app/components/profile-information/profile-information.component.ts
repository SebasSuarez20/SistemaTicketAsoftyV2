import { Component, OnInit } from '@angular/core';
import { DataEncryptionService } from 'src/app/services/Encryption/data-encryption.service';
import { TicketsServicesHttpService } from 'src/app/services/ticketsServicesHttp/tickets-services-http.service';

@Component({
  selector: 'app-profile-information',
  templateUrl: './profile-information.component.html',
  styleUrls: ['./profile-information.component.css']
})
export class ProfileInformationComponent implements OnInit {

  constructor(private serviceHttp: TicketsServicesHttpService, private serviceAES: DataEncryptionService) { }

  public resultCode: string = "";

  ngOnInit(): void {
  }


  public createCodeQr() {
    this.serviceHttp.connectApiGet("login/spExample").then((res: any) => {
      this.resultCode = res.base64;
      console.log(this.serviceAES.decryptBase64(res.information));
    })
  }



}
