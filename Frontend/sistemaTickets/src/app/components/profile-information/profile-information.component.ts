import { DataRowOutlet } from '@angular/cdk/table';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IcreateUser } from 'src/app/Model/IcreateUser';
import { IInformationUser } from 'src/app/Model/IInformationUser';
import { IUser } from 'src/app/Model/IUser';
import { DataEncryptionService } from 'src/app/services/Encryption/data-encryption.service';
import { TicketsServicesHttpService } from 'src/app/services/httpService/tickets-services-http.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-profile-information',
  templateUrl: './profile-information.component.html',
  styleUrls: ['./profile-information.component.css'],
})
export class ProfileInformationComponent implements OnInit {
  public formUser = new FormGroup({
    Idcontrol: new FormControl(null),
    NameSupport: new FormControl(''),
    Surname: new FormControl(''),
    gender: new FormControl(0),
    typeIdentification: new FormControl(1),
    identification: new FormControl('', Validators.required),
    bloodType: new FormControl(''),
    country: new FormControl(0),
    city: new FormControl(0),
    address: new FormControl(''),
    phone: new FormControl(0),
    Email: new FormControl('', Validators.required),
    birthDate: new FormControl(),
    emergencyContact: new FormControl(0),
    parentage: new FormControl(''),
    firstName: new FormControl('', Validators.required),
    firstSurname: new FormControl(''),
    Password: new FormControl('', Validators.required),
    department: new FormControl(''),
    enabled: new FormControl(1),

    nameUser: new FormControl(''),
    roleCode: new FormControl(1),
  });

  constructor(
    private serviceHttp: TicketsServicesHttpService,
    private serviceAES: DataEncryptionService
  ) { }

  public resultCode: string = '';

  ngOnInit(): void { }

  public saveUser() {
    if(this.formUser.valid) {

    const header: Partial<IUser> = {
      nameUser: this.formUser.controls.nameUser.value,
      password: this.formUser.controls.Password.value,
      identification: this.formUser.controls.identification.value,
      roleCode: this.formUser.controls.roleCode.value,
      enabled: this.formUser.controls.enabled.value,
      
    };

    const body: Partial<IInformationUser> = {
      NameSupport: this.formUser.controls.NameSupport.value,
      Surname: this.formUser.controls.Surname.value,
      Email: this.formUser.controls.Email.value,
      gender: this.formUser.controls.gender.value,
      typeIdentification: this.formUser.controls.typeIdentification.value,
      identification: this.formUser.controls.identification.value,
      bloodType: this.formUser.controls.bloodType.value,
      country: this.formUser.controls.country.value,
      city: this.formUser.controls.city.value,
      address: this.formUser.controls.address.value,
      phone: this.formUser.controls.phone.value,
      birthDate: this.formUser.controls.birthDate.value,
      emergencyContact: this.formUser.controls.emergencyContact.value,
      parentage: this.formUser.controls.parentage.value,
      firstName: this.formUser.controls.firstName.value,
      firstSurname: this.formUser.controls.firstSurname.value,
      department: this.formUser.controls.department.value,
      enabled: this.formUser.controls.enabled.value,
    };

    if (this.formUser.valid) {
      const userCreate: IcreateUser = {
        header: header,
        body: body,
      };

      this.serviceHttp.connectApiPost(`user/createUser`, userCreate).then((res: any) => {
        if (res.status === 200) {
          Swal.fire({
            icon: 'success',
            title: res.message,
            toast: true,
            timer: 2200,
            timerProgressBar: true,
            showCancelButton: false,
          }).then(() => {
            this.formUser.reset();
          })
        } 
      });
    }
  }
  }

  public createCodeQr() {
    this.serviceHttp.connectApiGet('login/spExample').then((res: any) => {
      this.resultCode = res.base64;
      console.log(this.serviceAES.decryptBase64(res.information));
    });
  }
}
