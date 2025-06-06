import { createAction, props } from "@ngrx/store";
import { Post } from "../../core/models/post.models";

export const loadPosts = createAction('[Profile Page] Load Post');
export const loadPostsSuccess = createAction('[Profile API] Load Post Success', props<{ posts: Post[] }>());
export const loadPostsFailure = createAction('[Profile API] Load Post Failure', props<{ error: any }>());

export const clearPost = createAction(
  '[Auth/Profile] Clear Post'
);