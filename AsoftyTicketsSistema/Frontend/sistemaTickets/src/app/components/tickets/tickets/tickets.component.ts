import { Component, HostListener } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ICodeGen } from 'src/app/Model/ICodeGen';
import { ITicketSupport } from 'src/app/Model/ITicketSupport';
import { CodeGenService } from 'src/app/services/code-gen.service';
import { TicketsServicesHttpService } from 'src/app/services/ticketsServicesHttp/tickets-services-http.service';



@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent {

  public isLinear = false;
  public firstFormGroup: FormGroup;
  public secondFormGroup: FormGroup;
  public panelOpenState: boolean = false;
  public codeGenAerea!: ICodeGen[];
  public codeGenPriority!: ICodeGen[];
  public imageFirst: string = '';
  public imageSecond: string = '';
  public imageThree: string = '';
  public isVisible: boolean = false;
  public imageZoom: string = '';
  public imageDescription: File[] = [];

  public formTicket = new FormGroup({
    Idcontrol: new FormControl(null),
    Consecutive: new FormControl(null),
    Aerea: new FormControl(''),
    Title: new FormControl(''),
    Description: new FormControl(''),
    Priority: new FormControl(''),
    PhotoDescription: new FormControl(''),
    Assigned_To: new FormControl(''),
    Enabled: new FormControl(true)
  });

  constructor(private serviceHttp: TicketsServicesHttpService, private codeGenericService: CodeGenService) { }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if ((event.key === 'e' || event.key === 'E') && (this.isVisible)) {
      this.isVisible = false;
    }
  }

  ngOnInit(): void {
    this.codeGenAerea = this.codeGenericService.loadCode('Aerea');
    this.codeGenPriority = this.codeGenericService.loadCode('Priority');
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
    this.isVisible = true;
  }

  public async saveTickets() {

    let arrayImg: string[] = [];

    if (this.imageFirst != "") arrayImg.push(this.imageDescription[0].name);
    if (this.imageSecond != "") arrayImg.push(this.imageDescription[1].name);
    if (this.imageThree != "") arrayImg.push(this.imageDescription[2].name);

    const ModelTickets: Partial<ITicketSupport> = {
      idControl: null,
      consecutive: 7,
      title: this.formTicket.get('Title').value,
      aerea: this.formTicket.get('Aerea').value,
      description: this.formTicket.get('Description').value,
      priority: this.formTicket.get('Priority').value,
      photoDescription: arrayImg.length != 0 ? arrayImg.join(',') : null,
      assigned_To: parseInt(this.formTicket.get('Assigned_To').value),
      enabled: true
    }


    let form = new FormData();
    form.append('header', JSON.stringify(ModelTickets));


    this.imageDescription.forEach((e: File) => {
      form.append('files', e);
    })

    await this.serviceHttp.connectApiPost("ticketssupport/CreateTickets", form).then((res: any) => {
      console.log(res);
    })
  }

}
