import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { catchError, map, exhaustMap, withLatestFrom, filter, tap } from 'rxjs/operators'; 

import * as ProfileActions from '../actions/profile.actions'
import { selectProfileData } from '../selectors/profile.selectors';
import { ApiService } from '../../core/services/api.service';
import { Profile } from '../../core/models/profile.models';

@Injectable()
export class ProfileEffects {
    loadProfileIfNeeded$;

  constructor(
    private actions$: Actions,
    private apiService: ApiService,
    private store: Store 
  ) {
   this.loadProfileIfNeeded$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProfileActions.loadProfile),
      withLatestFrom(this.store.select(selectProfileData)),
      filter(([action, existingProfile]) => {
        if (existingProfile === null) {
          console.log('ProfileEffects: Profile is null in store, proceeding to fetch from API.');
          return true; // Proceed with the effect chain
        } else {
          console.log('ProfileEffects: Profile already exists in store, API call skipped.');
          return false; // Stop the effect chain here, no API call
        }
      }),

      exhaustMap(() =>
        this.apiService.get<Profile>('profile/info') // Replace with your actual API endpoint
          .pipe(
            map(profile => ProfileActions.loadProfileSuccess({ profile })),
            catchError(error => {
              console.error('ProfileEffects: API error while loading profile', error);
              return of(ProfileActions.loadProfileFailure({ error }));
            })
          )
      )
    )
  );
  } 
}