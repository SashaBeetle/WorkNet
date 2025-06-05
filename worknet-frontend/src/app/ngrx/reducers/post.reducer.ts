import { createReducer, on } from "@ngrx/store";
import * as PostActions from '../actions/post.actions'
import { Post } from "../../core/models/post.models";

export const postFeatureKey = 'post'; 

export interface PostState {
    posts: Post[] | null;
    loading: boolean;  
    updating: boolean;
    error: any | null; 
}

export const initialState: PostState = {
    posts: null,
    loading: false,
    updating: false,
    error: null,
};

export const postReducer = createReducer(
  initialState,

  on(PostActions.loadPosts, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(PostActions.loadPostsSuccess, (state, { posts }) => ({
    ...state,
    posts: posts,
    loading: false,
    error: null,
  })),
  on(PostActions.loadPostsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),
  on(PostActions.clearPost, () => ({
    ...initialState,
  }))
);