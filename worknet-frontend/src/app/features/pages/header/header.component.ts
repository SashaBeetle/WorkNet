import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { isLoggedIn, selectCurrentUser } from '../../../ngrx/selectors/user.selectors';
import { User } from '../../../core/models/user.models';
import { Observable, timer, combineLatest, of } from 'rxjs';
import { map, startWith, tap,take, switchMap, distinctUntilChanged } from 'rxjs/operators';
import { CommonModule, AsyncPipe } from '@angular/common';


@Component({
  selector: 'app-header',
  imports: [CommonModule, AsyncPipe], 
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  user$: Observable<User | null>;
  isLoaded$: Observable<boolean>;
  displayState$: Observable<'initialLoading' | 'guest' | 'user'>;

  constructor(
    private authService: AuthService,
    private store: Store,
    private router: Router
  ) {
    this.user$ = this.store.select(selectCurrentUser);
    this.isLoaded$ = this.store.select(isLoggedIn);

    const mandatoryLoadTimeComplete$ = timer(1000).pipe(
      map(() => true),
      startWith(false)
    );
    
    this.displayState$ = mandatoryLoadTimeComplete$.pipe(
      switchMap(mandatoryTimeHasElapsed => {
        if (!mandatoryTimeHasElapsed) {
          return of('initialLoading' as const);
        } else {
          return combineLatest([this.isLoaded$, this.user$]).pipe(
            map(([dataIsActuallyLoaded, currentUser]) => {
              if (!dataIsActuallyLoaded && authService.isLoggedIn()) {
                return 'initialLoading' as const;
              } else if (!dataIsActuallyLoaded){
                return 'guest' as const;
              }else{
                return currentUser ? ('user' as const) : ('guest' as const);

              }
            })
          );
        }
      }),
      startWith('initialLoading' as const),
      tap(currentState => {
        console.log('Current displayState$:', currentState);
      }),
      distinctUntilChanged()
    );
  }

  logout() {
    this.authService.logout();
      this.router.navigate(['/']).then(() => {
      window.location.reload();
    })  }
}