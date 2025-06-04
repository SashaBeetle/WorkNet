import { createReducer, on } from "@ngrx/store";
import { Profile } from "../../core/models/profile.models";
import * as ProfileActions from '../actions/profile.actions'

export const profileFeatureKey = 'profile'; 

export interface ProfileState {
    profile: Profile | null;
    loading: boolean;  // For initial load
    updating: boolean; // For update operations
    error: any | null; 
}

export const initialState: ProfileState = {
    profile: null,
    loading: false,
    updating: false,
    error: null,
};

export const profileReducer = createReducer(
  initialState,

  // Load Profile
  on(ProfileActions.loadProfile, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(ProfileActions.loadProfileSuccess, (state, { profile }) => ({
    ...state,
    profile: profile,
    loading: false,
    error: null,
  })),
  on(ProfileActions.loadProfileFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),
  // Clear Profile
  on(ProfileActions.clearProfile, () => ({
    ...initialState, // Reset to initial state
  }))
);