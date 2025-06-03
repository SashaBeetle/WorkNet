import { createFeatureSelector, createSelector } from '@ngrx/store';
import { UserState, ProfileState } from '../reducers/user.reducer';

export const selectUserState = createFeatureSelector<UserState>('user');
export const selectProfileState = createFeatureSelector<ProfileState>('profile');

export const selectCurrentUser = createSelector(
  selectUserState,
  (state) => state.user
);

export const isLoggedIn = createSelector(
  selectUserState,
  (state) => !!state.user
);
