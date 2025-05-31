import { jwtDecode } from 'jwt-decode';

export interface JwtPayload {
  nameid: string;        // ClaimTypes.NameIdentifier
  unique_name: string;   // ClaimTypes.Name
  exp: number;
  [key: string]: any;
}

export function getCurrentUser(): JwtPayload | null {
  const token = localStorage.getItem('jwt');
  if (!token) return null;

  try {
    return jwtDecode<JwtPayload>(token);
  } catch (err) {
    console.error('JWT decode error:', err);
    return null;
  }
}
