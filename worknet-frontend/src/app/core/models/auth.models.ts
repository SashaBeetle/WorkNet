export interface RegisterUserPayload {
  userName: string;
  userEmail: string;
  password: string;
}

export interface LoginUserPayload {
  userEmail: string;
  password: string;
}