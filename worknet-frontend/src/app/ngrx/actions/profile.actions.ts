import { createAction, props } from "@ngrx/store";
import { Profile } from "../../core/models/profile.models";

export const loadProfile = createAction('[Profile Page] Load Profile');
export const loadProfileSuccess = createAction('[Profile API] Load Profile Success', props<{ profile: Profile }>());
export const loadProfileFailure = createAction('[Profile API] Load Profile Failure', props<{ error: any }>());

// Clear Profile Action (e.g., on logout)
export const clearProfile = createAction(
  '[Auth/Profile] Clear Profile'
);