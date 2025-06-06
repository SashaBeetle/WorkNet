import { createFeatureSelector, createSelector } from "@ngrx/store";
import { postFeatureKey, PostState } from "../reducers/post.reducer";

export const selectPostFeatureState =
  createFeatureSelector<PostState>(postFeatureKey);

export const selectPostsData = createSelector(
  selectPostFeatureState,
  (state: PostState) => state.posts
);