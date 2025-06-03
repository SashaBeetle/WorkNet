import { createReducer, on } from '@ngrx/store';
import { loginSuccess, logout } from '../actions/user.actions';
import { User } from '../../core/models/user.models';

export interface ProfileState {
    profile: 
}

export interface UserState {
  user: User | null;
}

const initialState: UserState = {
  user: null,
};

export const userReducer = createReducer(
  initialState,
  on(loginSuccess, (state, { user }) => ({ ...state, user })),
  on(logout, state => ({ ...state, user: null }))
);
