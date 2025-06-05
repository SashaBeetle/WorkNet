import { User } from "./user.models";

export interface Post {
  id: string;
  data: string;
  user: User;
  createdAt: string;
  profilePhotoId?: string | null;
}

export interface PostPayload {
  data: string;
}