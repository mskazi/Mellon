import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { LoginService } from './login.service';

export const authGuard = (route?: ActivatedRouteSnapshot, state?: RouterStateSnapshot) => {
  const auth = inject(LoginService);
  const router = inject(Router);

  return auth.check() ? true : router.parseUrl('/auth/unauthorized');
};
