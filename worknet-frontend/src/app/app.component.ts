import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HomeComponent } from './features/home/home/home.component';
import { HeaderComponent } from './features/header/header/header.component';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, HomeComponent, HeaderComponent],
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'worknet-frontend';
}
