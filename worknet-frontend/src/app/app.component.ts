import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, Router } from '@angular/router';
import { HeaderComponent } from './features/pages/header/header.component';
import { AuthService } from './core/services/auth.service';
@Component({
    selector: 'app-root',
    imports: [RouterOutlet, HeaderComponent, CommonModule],
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss'
})
export class AppComponent {
  hiddenHeaderRoutes = ['/login', '/register', '/forgot-password']; 

  constructor(private authService: AuthService, public router: Router) {}
      ngOnInit() {
    this.authService.initUserFromToken();
  }
}

