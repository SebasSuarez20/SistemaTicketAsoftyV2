import { DialogRef } from '@angular/cdk/dialog';
import { Component, HostListener } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ICodeGen } from 'src/app/Model/ICodeGen';
import { ITicketSupport } from 'src/app/Model/ITicketSupport';
import { CodeGenService } from 'src/app/services/code-gen.service';
import { TicketsServicesHttpService } from 'src/app/services/httpService/tickets-services-http.service';
import Swal from 'sweetalert2';



@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent {

  public firstFormGroup: FormGroup;
  public secondFormGroup: FormGroup;
  public panelOpenState: boolean = false;
  public codeGenArea!: ICodeGen[];
  public codeGenPriority!: ICodeGen[];
  public codeGenStatus!: ICodeGen[];
  public codeGenSoftware!: ICodeGen[];
  public imageFirst: string = '';
  public imageSecond: string = '';
  public imageThree: string = '';
  public isVisible: boolean = false;
  public imageZoom: string = '';
  public imageDescription: File[] = [];
  public isReadOnly: boolean = false;
  private date = new Date();
  public resultMinDate!: string;
  private isValid: boolean = false;

  public formTicket = new FormGroup({
    Idcontrol: new FormControl(null),
    Date: new FormControl(null, Validators.required),
    Consecutive: new FormControl(null),
    Area: new FormControl('', Validators.required),
    SoftwareApplication: new FormControl('', Validators.required),
    Status: new FormControl(''),
    CodeConnectionSoftware: new FormControl(null),
    Title: new FormControl('', Validators.required),
    Description: new FormControl('', Validators.required),
    Priority: new FormControl('', Validators.required),
    PhotoDescription: new FormControl(''),
    Enabled: new FormControl(true)
  });

  constructor(private serviceHttp: TicketsServicesHttpService, private codeGenericService: CodeGenService, private dialog: DialogRef) {
    this.resultMinDate = this.date.toISOString().slice(0, 10);
    this.formTicket.get('Date').setValue(this.resultMinDate);
  }


  ngOnInit(): void {
    this.codeGenArea = this.codeGenericService.loadCode('Area');
    this.codeGenPriority = this.codeGenericService.loadCode('Priority');
    this.codeGenStatus = this.codeGenericService.loadCode('Info_Status');
    this.codeGenSoftware = this.codeGenericService.loadCode('Software');
  }

  public validSoftware(event: any) {
    this.isReadOnly = false;
    if (event.target.value === '3' || event.target.value === '4') {
      this.isReadOnly = true;
      this.formTicket.get("CodeConnectionSoftware").setValue(null);
    }
  }

  public validField(text: string, field?: string): string {
    if (this.isValid) {
      if (field === "idConnection") {
        let { SoftwareApplication } = this.formTicket.value;
        if (SoftwareApplication === "3" || SoftwareApplication === "4") {
          return '';
        } else {
          if (text === "" || text === null) {
            return 'Campo requerido,por favor ingresa informacion.';
          }
        }
      } else {
        if (text === "" || text === null) {
          return 'Campo requerido,por favor ingresa informacion.';
        }
        return '';
      }
    }
    return '';
  }

  public loadImg(event: any, pos: number) {
    const file = event.target.files[0];

    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        if (pos == 0) {

          this.imageDescription[0] = file;
          this.imageFirst = e.target.result.toString();
        }
        if (pos == 1) {

          this.imageSecond = e.target.result.toString();
          this.imageDescription[1] = file;
        }
        if (pos == 2) {
          this.imageThree = e.target.result.toString();
          this.imageDescription[2] = file;
        }
      };
      reader.readAsDataURL(file);
    }
  }

  public showImage(pos: number) {

    this.imageZoom = '';

    if (pos === 1) this.imageZoom = this.imageFirst;
    if (pos === 2) this.imageZoom = this.imageSecond;
    if (pos === 3) this.imageZoom = this.imageThree;
    this.openBase64(this.imageZoom);
  }

  public openBase64(plainText: string) {
    const byteCharacters = atob(plainText.split(',')[1]);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'image/png' });
    // Crear una URL para el Blob y abrirla en una nueva pestaña
    const blobUrl = URL.createObjectURL(blob);
    window.open(blobUrl, '_blank');
    // Para liberar recursos, revocamos la URL del Blob después de un tiempo
    setTimeout(() => {
      URL.revokeObjectURL(blobUrl);
    }, 1000);
  }

  public async saveTickets() {
    if (this.formTicket.valid) {


      Swal.fire({
        title: '¿Esta seguro de crear el ticket?',
        icon: 'info',
        toast: true,
        showCancelButton: true,
        showConfirmButton: true
      }).then(async (result) => {
        if (result.isConfirmed) {


          let arrayImg: string[] = [];

          if (this.imageFirst != "") arrayImg.push(this.imageDescription[0].name);
          if (this.imageSecond != "") arrayImg.push(this.imageDescription[1].name);
          if (this.imageThree != "") arrayImg.push(this.imageDescription[2].name);

          const ModelTickets: Partial<ITicketSupport> = {
            idControl: null,
            consecutive: null,
            date: this.formTicket.get('Date').value,
            title: this.formTicket.get('Title').value,
            description: this.formTicket.get('Description').value,
            aerea: this.formTicket.get('Area').value,
            status: this.formTicket.get('Status').value,
            priority: this.formTicket.get('Priority').value,
            codeConnectionSoftware: this.formTicket.get('CodeConnectionSoftware').value,
            softwareApplication: parseInt(this.formTicket.get('SoftwareApplication').value),
            photoDescription: arrayImg.length != 0 ? arrayImg.join(',') : null,
            enabled: true
          }

          let form = new FormData();
          form.append('header', JSON.stringify(ModelTickets));

          this.imageDescription.forEach((e: File) => {
            form.append('files', e);
          })


          await this.serviceHttp.connectApiPost("ticketssupport/CreateTickets", form).then((res: any) => {

            if (res.status === 200) {
              Swal.fire({
                icon: 'success',
                title: res.message,
                toast: true,
                timer: 2200,
                timerProgressBar: true,
                showCancelButton: false,
                showConfirmButton: false
              }).then(() => {
                this.dialog.close();
              })
            } else {
              Swal.fire({
                icon: 'error',
                title: res.message,
                toast: true,
                timer: 2200,
                timerProgressBar: true,
                showCancelButton: false,
                showConfirmButton: false
              }).then(() => {
                this.dialog.close();
              })
            }
          })
        }
      })

    } else {
      this.isValid = true;
    }


  }

}
