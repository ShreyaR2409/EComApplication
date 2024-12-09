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

  // Create a BehaviorSubject to hold the cart data
  private cartSubject: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);
  public cart$: Observable<any[]> = this.cartSubject.asObservable(); // Observable to be used in components

  private cartCountSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  cartCount$: Observable<number> = this.cartCountSubject.asObservable();
  
  constructor(private http: HttpClient, private router: Router) { }

  // public AddToCart(id: number, data: any): Observable<any> {
  //   return this.http.post<any>(`${this.url}/add?id=${id}`, data, {
  //     responseType: 'text' as 'json'
  //   }).pipe(
  //     tap(() => this.updateCart(id)) // After adding, update the cart
  //   );
  // }

   // Add a product to the cart
   public AddToCart(id: number, data: any): Observable<any> {
    return this.http.post<any>(`${this.url}/add?id=${id}`, data, {
      responseType: 'text' as 'json'
    }).pipe(
      tap(() => this.updateCart(id)) // After adding, update the cart
    );
  }


  // public GetCartDetail(id : number) : Observable<any>{
  //   return this.http.get<any>(`${this.url}/${id}`);
  // }

    // Get cart details and update the BehaviorSubject
    public GetCartDetail(id: number): Observable<any> {
      return this.http.get<any>(`${this.url}/${id}`).pipe(
        tap(cartData => this.cartSubject.next(cartData)) // Update BehaviorSubject with the new cart data
      );
    }

  // public removeCartItem(id: number, productId: number): Observable<any> {
  //   console.log("Services", id , productId);    
  //   return this.http.delete<any>(`${this.url}?CartId=${id}&ProductId=${productId}`);
  // }

    // Remove a product from the cart
    public removeCartItem(id: number, productId: number): Observable<any> {
      return this.http.delete<any>(`${this.url}?CartId=${id}&ProductId=${productId}`, {
        responseType : 'text' as 'json'
      }).pipe(
        tap(() => this.updateCart(id)) // After removing, update the cart
      );
    }

    public cardDetails(data : any ) : Observable<any>{
      return this.http.post<any>(`${this.url}/Card-Details`, data, {
        responseType: 'text' as 'json'
      })
    }

    
  private updateCart(id: number): void {
    this.GetCartDetail(id).subscribe(); // Refresh the cart data
  }
  
}
