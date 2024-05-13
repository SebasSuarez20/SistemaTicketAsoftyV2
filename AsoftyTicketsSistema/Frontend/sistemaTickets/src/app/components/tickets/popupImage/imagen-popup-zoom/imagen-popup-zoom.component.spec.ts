import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImagenPopupZoomComponent } from './imagen-popup-zoom.component';

describe('ImagenPopupZoomComponent', () => {
  let component: ImagenPopupZoomComponent;
  let fixture: ComponentFixture<ImagenPopupZoomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImagenPopupZoomComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ImagenPopupZoomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
