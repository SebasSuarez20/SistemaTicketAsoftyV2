import { TestBed } from '@angular/core/testing';

import { AuthGeneralGuard } from './auth-general.guard';

describe('AuthGeneralGuard', () => {
  let guard: AuthGeneralGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AuthGeneralGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
