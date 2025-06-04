import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtInterceptor } from './core/Helpers/jwt.interceptor';
import { provideState, provideStore } from '@ngrx/store';
import { profileReducer, profileFeatureKey } from './ngrx/reducers/profile.reducer';
import { userReducer } from './ngrx/reducers/user.reducer';

import {provideStoreDevtools } from'@ngrx/store-devtools';
import { environment } from '../environments/environment';
import { provideEffects } from '@ngrx/effects';
import { ProfileEffects } from './ngrx/effects/profile.effects';


export const appConfig: ApplicationConfig = {
  providers:[
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    provideHttpClient(withInterceptorsFromDi()),
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },

    //NgRx
    provideStore({ user: userReducer }),
    provideState(profileFeatureKey, profileReducer),
    provideEffects([
      ProfileEffects
    ]),
    
    provideStoreDevtools({ maxAge: 25, logOnly: environment.production }),
    importProvidersFrom(BrowserAnimationsModule),
  ],
};
