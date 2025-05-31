import { createAction, props } from '@ngrx/store';
import { User } from '../../core/models/user.models';

export const loginSuccess = createAction('[Auth] Login Success', props<{ user: User }>());

export const logout = createAction('[Auth] Logout');