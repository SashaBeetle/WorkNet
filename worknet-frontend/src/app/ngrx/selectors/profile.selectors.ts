import { createFeatureSelector, createSelector } from "@ngrx/store";
import { ProfileState, profileFeatureKey } from "../reducers/profile.reducer";


export const selectProfileFeatureState =
  createFeatureSelector<ProfileState>(profileFeatureKey);

// Select the profile data
export const selectProfileData = createSelector(
  selectProfileFeatureState,
  (state: ProfileState) => state.profile
);