import { Routes } from '@angular/router';
import { HomeComponent } from './features/pages/home/home.component';
import { LoginComponent } from './features/pages/auth/login/login.component';
import { RegisterComponent } from './features/pages/auth/register/register.component';
import { MessagesComponent } from './features/pages/messages/messages.component';
import { JobsComponent } from './features/pages/jobs/jobs.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'messages', component: MessagesComponent},
    {path: 'jobs', component: JobsComponent}
];
