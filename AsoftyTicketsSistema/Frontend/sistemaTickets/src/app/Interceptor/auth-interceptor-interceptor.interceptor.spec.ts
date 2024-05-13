import { TestBed } from '@angular/core/testing';

import { AuthInterceptorInterceptorInterceptor } from './auth-interceptor-interceptor.interceptor';

describe('AuthInterceptorInterceptorInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      AuthInterceptorInterceptorInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: AuthInterceptorInterceptorInterceptor = TestBed.inject(AuthInterceptorInterceptorInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
