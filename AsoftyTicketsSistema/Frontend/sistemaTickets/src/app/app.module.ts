import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { AuthInterceptorInterceptor } from './Interceptor/auth-interceptor-interceptor.interceptor';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HeaderComponent } from './components/header/header/header.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { InicioComponent } from './components/inicio/inicio/inicio.component';
import { TicketsComponent } from './components/tickets/tickets/tickets.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { DashboardInicioComponent } from './components/dashboard/dashboard-inicio/dashboard-inicio.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatTableModule } from '@angular/material/table';
import { FormsModule } from '@angular/forms';

import { MatStepperModule } from '@angular/material/stepper';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatDialogModule } from '@angular/material/dialog';
import { ImagenPopupZoomComponent } from './components/tickets/popupImage/imagen-popup-zoom/imagen-popup-zoom.component';
import { AccessRolUserDirective } from './Directive/access-rol-user.directive';
import { ToastrModule } from "ngx-toastr";
import { ProfileInformationComponent } from './components/profile-information/profile-information.component';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { NotFoundComponent } from './components/shared/404/not-found/not-found.component';
import { MatTooltipModule } from '@angular/material/tooltip';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HeaderComponent,
    InicioComponent,
    TicketsComponent,
    DashboardInicioComponent,
    ImagenPopupZoomComponent,
    AccessRolUserDirective,
    NotFoundComponent,
    ProfileInformationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatExpansionModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    MatTableModule,
    FormsModule,
    MatStepperModule,
    MatInputModule,
    MatRadioModule,
    MatDialogModule,
    ToastrModule.forRoot({
      timeOut: 1800,
      progressBar: true,
      progressAnimation: 'increasing',
      preventDuplicates: true,
      positionClass: 'toast-top-center' // Esta línea establece la posición en la parte inferior derecha
    }),
    NgIdleKeepaliveModule,
    MatTooltipModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorInterceptor, multi: true },
    DashboardInicioComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
