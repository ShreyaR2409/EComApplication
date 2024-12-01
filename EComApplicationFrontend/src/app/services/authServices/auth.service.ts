import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
// import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private url = 'https://localhost:7066/api/User';
  private countryUrl = 'https://localhost:7066/api/CountryCity/Country';
  private stateUrl = 'https://localhost:7066/api/CountryCity/State';
  private roleUrl = 'https://localhost:7066/api/User/Roles';
  constructor(private http: HttpClient, private router: Router) {}

  public registerUser(user: any): Observable<any> {
    return this.http.post<any>(`${this.url}`, user);
  }

  public loginUser(user: any): Observable<any> {
    return this.http.post<any>(`${this.url}/login`, user);
  }

  public verifyOtp(otp: any): Observable<any> {
    return this.http.post<any>(`${this.url}/VerifyOtp`, otp).pipe(
      tap((response) => {
        if (response && response.token) {
          // Store token and role in session storage
          sessionStorage.setItem('authToken', response.token);
        }
      })
    );
  }

  public forgotPassword(email:any):Observable<any>{
    return this.http.post<any>(`${this.url}/ForgotPassword`, email)
  }

  // Get all countries
  getAllCountries(): Observable<any[]> {
    return this.http.get<any[]>(this.countryUrl);
  }

  // Get states by country ID
  getStatesByCountryId(countryId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.stateUrl}?id=${countryId}`);
  }

  getAllRoles(): Observable<any[]> {
    return this.http.get<any[]>(this.roleUrl);
  }

  public logoutUser(): void {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }

  private isTokenExpired(token: string): boolean {
    const decodedToken: any = this.getDecodedToken(token);
    const currentTime = Math.floor(Date.now() / 1000);
    return decodedToken.exp < currentTime;
  }

  public getDecodedToken(token: string): any {
    try {
      const payload = token.split('.')[1]; // Get the payload part of the JWT
      const decodedPayload = atob(payload); // Decode from Base64
      return JSON.parse(decodedPayload); // Parse JSON string
    } catch (Error) {
      return null;
    }
  }

  public getUserRole(): string | null {
    const token = sessionStorage.getItem('token'); // Assuming token is stored in localStorage
    if (token) {
      const decodedToken = this.getDecodedToken(token);
      return decodedToken?.role || null;
    }
    return null;
  }

  public isLoggedIn(): boolean {
    const token = sessionStorage.getItem('authToken');
    return !!token && !this.isTokenExpired(token);
  }
}
