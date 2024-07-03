import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TicketsServicesHttpService {

  private url: string = "https://localhost:7026/api/";

  constructor(private http: HttpClient) { }

  public async connectApiGet(controller: string): Promise<any> {
    return await lastValueFrom(this.http.get(this.url + controller));
  }

  public async connectApiPost(controller: string, body: any): Promise<any> {
    return await lastValueFrom(this.http.post(this.url + controller, body));
  }

  public async connectApiPut(controller: string, body: any): Promise<any> {
    return await lastValueFrom(this.http.put(this.url + controller, body));
  }

}
