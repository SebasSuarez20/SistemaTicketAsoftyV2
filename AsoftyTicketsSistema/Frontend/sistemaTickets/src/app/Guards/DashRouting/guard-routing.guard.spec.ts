import { TestBed } from '@angular/core/testing';

import { GuardRoutingGuard } from './guard-routing.guard';

describe('GuardRoutingGuard', () => {
  let guard: GuardRoutingGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(GuardRoutingGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
