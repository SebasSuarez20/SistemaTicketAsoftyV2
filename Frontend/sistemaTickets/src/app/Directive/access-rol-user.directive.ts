import { Directive, ElementRef, Input, OnInit, Renderer2 } from '@angular/core';
import { LoginService } from '../services/login.service';

@Directive({
  selector: '[appAccessRolUser]'
})
export class AccessRolUserDirective implements OnInit {

  public RolCode: number = -1;

  constructor(private element: ElementRef, private render: Renderer2, private data_Service: LoginService) { }

  ngOnInit(): void {
    this.RolCode = this.data_Service.dataLogged().rolCode;
    if (this.RolCode != 3) this.render.setStyle(this.element.nativeElement, 'display', 'none');
  }


}
