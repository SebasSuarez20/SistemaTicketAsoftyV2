import { TestBed } from '@angular/core/testing';

import { LibraryMessageService } from './library-message.service';

describe('LibraryMessageService', () => {
  let service: LibraryMessageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LibraryMessageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
