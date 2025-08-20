// auth.service.ts
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';


@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:7060/api/Users'; // cambia si usas otro puerto

  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, { email, password });
  }

  register(name: string, email: string, password: string) {
    return this.http.post(`${this.apiUrl}/register`, { name , email, password });
  }

  saveToken(token: string) {
    if (typeof window !== 'undefined') {
      localStorage.setItem('jwt', token);
    }
  }

  getToken(): string | null {
    if (typeof window !== 'undefined') {  // ✅ Solo en navegador
      return localStorage.getItem('jwt');
    }
    return null;
  }

    getDecodedToken(): any {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }

  getUserName(): string | null {
    const decoded = this.getDecodedToken();
    return decoded?.name || null;
  }

  getUserEmail(): string | null {
    const decoded = this.getDecodedToken();
    return decoded?.email || null;
  }

  getUserId(): string{
    const decoded = this.getDecodedToken();
    return decoded?.sub;
  }
}
