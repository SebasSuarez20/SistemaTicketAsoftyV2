import { TestBed } from '@angular/core/testing';

import { TicketsServicesHttpService } from './tickets-services-http.service';

describe('TicketsServicesHttpService', () => {
  let service: TicketsServicesHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TicketsServicesHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
