import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html'
})
export class AppComponent {
  products$ = this.http.get('http://localhost:5003/api/catalog/products');

  constructor(private readonly http: HttpClient, private readonly authService: AuthService) {}

  signIn() {
    void this.authService.login();
  }
}
