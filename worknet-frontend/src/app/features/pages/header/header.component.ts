import { Component, inject } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AsyncPipe } from '@angular/common';
import { Store } from '@ngrx/store';
import { isLoggedIn, selectCurrentUser } from '../../../ngrx/selectors/user.selectors';
import { User } from '../../../core/models/user.models';
import { Observable, timer  } from 'rxjs';
import { takeUntil , startWith, tap } from 'rxjs/operators';



@Component({
  selector: 'app-header',
  imports: [CommonModule, AsyncPipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  user$!: Observable<User | null>;
  isLoaded: Observable<boolean>;

  constructor(private authService: AuthService,private store: Store, private router: Router) {
        this.user$ = this.store.select(selectCurrentUser);
        this.isLoaded = this.store.select(isLoggedIn);

  }

  ngOnInit(): void {
    // debugger; // Ваш debugger breakpoint

    // const loadingTimeout$ = timer(3000).pipe(
    //   tap(() => {
    //     // Цей tap спрацює через 3 секунди
    //     // Якщо this.isLoading все ще true, він встановить його в false.
    //     // Це може бути запасним механізмом, якщо визначення стану користувача займає занадто багато часу.
    //     if (this.isLoading) {
    //         console.log('Loading timeout triggered. Setting isLoading to false.');
    //         this.isLoading = false;
    //     }
    //   })
    // );

    // this.user$ = this.store.select(selectCurrentUser).pipe(
    //   tap(userFromStore => { // Для дебагу, що саме повертає селектор
    //     console.log('Raw user from store selector:', userFromStore);
    //   }),
    //   catchError(error => {
    //     // Якщо селектор selectCurrentUser видає помилку (наприклад, стан не ініціалізовано
    //     // або логіка селектора дає збій для стану "немає користувача"),
    //     // ми перехоплюємо її тут.
    //     console.error('Error in selectCurrentUser stream, defaulting to null:', error);
    //     // Повертаємо Observable з `null`, щоб потік продовжився.
    //     // Це дозволить `startWith(null)` (якщо він після) або наступним операторам працювати коректно.
    //     return of(null);
    //   }),
    //   startWith(null), // Гарантує, що user$ | async отримає початкове значення (null).
    //                    // Це має змусити шаблон #loading (спінер) зникнути швидко.
    //   tap(user => { // Цей tap тепер спрацює для значення від startWith(null),
    //                 // потім для значення від selectCurrentUser (або of(null) з catchError).
    //     console.log('User stream emitted (after startWith/catchError):', user);
    //     // Логіка для вашого прапорця this.isLoading.
    //     // Якщо спінер, яким керує this.isLoading, окремий від спінера в #loading,
    //     // то ця логіка керує ним.
    //     if (this.isLoading) {
    //       this.isLoading = false;
    //       console.log('User stream tap: isLoading set to false');
    //     }
    //   }),
    //   takeUntil(loadingTimeout$) // Завершує Observable user$ після того, як loadingTimeout$ спрацює.
    //                              // Це також гарантує, що tap в loadingTimeout$ виконається.
    // );
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
