import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { AuthGeneralGuard } from './Guards/General/auth-general.guard';
import { GuardRoutingGuard } from './Guards/DashRouting/guard-routing.guard';
import { InicioComponent } from './components/inicio/inicio/inicio.component';
import { ProfileInformationComponent } from './components/profile-information/profile-information.component';
import { NotFoundComponent } from './components/shared/404/not-found/not-found.component';
import { EmployeeComponent } from './components/shared/Consult/employee/employee.component';

const routes: Routes = [
  { path: 'd56b699830e77ba53855679cb1d252da', component: LoginComponent, canActivate: [AuthGeneralGuard] },
  { path: 'c751439d0db3883ac1c8e816327adcab', component: InicioComponent, canActivate: [GuardRoutingGuard] },
  { path: '14014e6a570327892015d91391f0756bc8c84d3594c867af4cefb5a7e9fb4eac', component: EmployeeComponent, canActivate: [GuardRoutingGuard] },
  { path: '**', component: NotFoundComponent, canActivate: [GuardRoutingGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
