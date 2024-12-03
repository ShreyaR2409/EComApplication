import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject,Observable } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private url = 'https://localhost:7066/api/User';
  private countryStateUrl = 'https://localhost:7066/api/CountryCity';

  private currentUserSubject = new BehaviorSubject<any>(null); 
  public currentUser = this.currentUserSubject.asObservable(); 

  constructor(private http: HttpClient, private router: Router) {}

  public registerUser(formData: FormData): Observable<any> {
    return this.http.post<any>(`${this.url}`, formData);
  }

  // public loginUser(user: any): Observable<any> {
  //   return this.http.post<any>(`${this.url}/Login`, user);
  // }

  public loginUser(user: any): Observable<any> {
    return this.http.post<any>(`${this.url}/Login`, user).pipe(
      tap((response) => {
        if (response && response.token) {
          sessionStorage.setItem('authToken', response.token);
          this.loadCurrentUser(); // Load current user info and role
          this.redirectUserBasedOnRole(); // Redirect based on role
        }
      })
    );
  }


  public updateUser(userId: number, userData: FormData): Observable<any> {
    return this.http.put(`${this.url}/Update-User/${userId}`, userData);
  }
  

  public verifyOtp(otp: any): Observable<any> {
    return this.http.post<any>(`${this.url}/VerifyOtp`, otp).pipe(
      tap((response) => {
        if (response && response.token) {
          sessionStorage.setItem('authToken', response.token);
          this.loadCurrentUser();
        }
      })
    );
  }

  public getUserByUsername(username: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.url}/UserByUsername?UserName=${username}`);
  }

  public forgotPassword(email:any):Observable<any>{
    return this.http.post<any>(`${this.url}/ForgotPassword`, email)
  }

  public changePassword(requestBody: { userId: string, changePasswordDto: { newPassword: string } }): Observable<any> {
    return this.http.post(`${this.url}/ChangePassword`, requestBody);
  }

  public getAllCountries(): Observable<any[]> {
    return this.http.get<any[]>(`${this.countryStateUrl}/Country`);
  }

  public getStatesByCountryId(countryId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.countryStateUrl}/State?id=${countryId}`);
  }

  public getAllRoles(): Observable<any[]> {
    return this.http.get<any[]>(`${this.url}/Roles`);
  }

  private decodeToken(token: string): any {
    try {
      const payload = token.split('.')[1];
      const decodedPayload = atob(payload);
      return JSON.parse(decodedPayload);
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }

  public loadCurrentUser(): void {
    const token = sessionStorage.getItem('authToken');
    if (token) {
      const decodedToken = this.decodeToken(token);
      const userData = {
        username: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
        role: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
        id: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
      };
      this.currentUserSubject.next(userData);
      sessionStorage.setItem('username', userData.username || '');
      sessionStorage.setItem('role', userData.role || '');
      sessionStorage.setItem('id', userData.id || '');
    }
  }

  private redirectUserBasedOnRole(): void {
    const role = sessionStorage.getItem('role');

    if (role === 'admin') {
      this.router.navigate(['/product-master']);
    } else if (role === 'user') {
      this.router.navigate(['/product-list']);
    } else {
      this.router.navigate(['/unauthorized']); // Optional: handle unauthorized roles
    }
  }

  public isLoggedIn(): boolean {
    const token = sessionStorage.getItem('authToken');
    return !!token && !this.isTokenExpired(token);
  }

  private isTokenExpired(token: string): boolean {
    const decodedToken: any = this.decodeToken(token);
    const currentTime = Math.floor(Date.now() / 1000);
    return decodedToken.exp < currentTime;
  }

  public logoutUser(): void {
    sessionStorage.clear();
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }
}
