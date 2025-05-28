import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, Router } from '@angular/router';
import { HeaderComponent } from './features/header/header/header.component';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, HeaderComponent, CommonModule],
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss'
})
export class AppComponent {
  hiddenHeaderRoutes = ['/login', '/register', '/forgot-password']; 

  constructor(public router: Router) {}
}
