import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { catchError, map, exhaustMap, withLatestFrom, filter } from 'rxjs/operators'; 
import * as PostActions from '../actions/post.actions'
import { ApiService } from '../../core/services/api.service';
import { selectPostsData } from '../selectors/post.selectors';
import { Post } from '../../core/models/post.models';

@Injectable()
export class PostEffects {
    loadPostsIfNeeded$;

  constructor(
    private actions$: Actions,
    private apiService: ApiService,
    private store: Store 
  ) {
   this.loadPostsIfNeeded$ = createEffect(() =>
    this.actions$.pipe(
      ofType(PostActions.loadPosts),
      withLatestFrom(this.store.select(selectPostsData)),
      filter(([action, existingPost]) => {
        if (existingPost === null) {
          console.log('ProfileEffects: Profile is null in store, proceeding to fetch from API.');
          return true; // Proceed with the effect chain
        } else {
          console.log('ProfileEffects: Profile already exists in store, API call skipped.');
          return false; // Stop the effect chain here, no API call
        }
      }),

      exhaustMap(() =>
        this.apiService.get<Post[]>('post') 
          .pipe(
            map(posts => PostActions.loadPostsSuccess({ posts })),
            catchError(error => {
              console.error('ProfileEffects: API error while loading profile', error);
              return of(PostActions.loadPostsFailure({ error }));
            })
          )
      )
    )
  );
  } 
}