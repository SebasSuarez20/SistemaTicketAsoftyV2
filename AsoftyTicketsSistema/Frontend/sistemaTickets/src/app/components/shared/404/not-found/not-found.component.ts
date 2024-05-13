import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent {

  constructor(private router: Router) {
    Swal.fire({
      icon: 'error',
      text: 'Error: no se encontro ruta elegida.',
      timer: 2200,
      timerProgressBar: true,
      toast: true
    }).then(() => {
      this.router.navigateByUrl("/c751439d0db3883ac1c8e816327adcab");
    });

  }

}
