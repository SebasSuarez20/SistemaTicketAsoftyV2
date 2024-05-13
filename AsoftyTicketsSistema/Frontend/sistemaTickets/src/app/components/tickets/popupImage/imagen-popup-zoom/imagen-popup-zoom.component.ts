import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-imagen-popup-zoom',
  templateUrl: './imagen-popup-zoom.component.html',
  styleUrls: ['./imagen-popup-zoom.component.css']
})
export class ImagenPopupZoomComponent implements OnInit {

  @Input() imgZoom: string;

  constructor() {
  }
  ngOnInit(): void {
    console.log(this.imgZoom);
  }



}
