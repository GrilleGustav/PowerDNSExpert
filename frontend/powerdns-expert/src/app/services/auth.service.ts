import { Injectable } from '@angular/core';
import { UserManager } from 'oidc-client-ts';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly userManager = new UserManager({
    authority: 'https://localhost:5001',
    client_id: 'angular-client',
    redirect_uri: 'http://localhost:4200/auth/callback',
    response_type: 'code',
    scope: 'openid profile email powerdns.api',
    post_logout_redirect_uri: 'http://localhost:4200'
  });

  login() {
    return this.userManager.signinRedirect();
  }
}
