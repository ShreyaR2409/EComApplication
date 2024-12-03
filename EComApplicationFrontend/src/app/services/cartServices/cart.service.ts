import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject,Observable } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private url = 'https://localhost:7066/api/Cart';

  constructor(private http: HttpClient, private router: Router) { }

  public AddToCart(id: number, data: any): Observable<any> {
    return this.http.post<any>(`${this.url}/add?id=${id}`, data);
  }

  public GetCartDetail(id : number) : Observable<any>{
    return this.http.get<any>(`${this.url}/${id}`);
  }
}
