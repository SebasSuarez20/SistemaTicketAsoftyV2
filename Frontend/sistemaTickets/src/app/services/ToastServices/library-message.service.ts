import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { lastValueFrom, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LibraryMessageService {

  constructor(public messagetoast: ToastrService) { }


  public successMessage(message: string, title: string): Promise<unknown> {
    return new Promise((resp, reject) => {
      const result = this.messagetoast.success(message, title);
      setTimeout(() => {
        resp(result);
      }, 2000);
    })
  }

  public InfoMessagge(message: string, title: string): Promise<unknown> {
    return new Promise((resp, reject) => {

      const result = this.messagetoast.info(message, title);
      setTimeout(() => {
        resp(result);
      }, 2000);
    })
  }


}
