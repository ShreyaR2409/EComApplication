import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient, private router: Router) { }
   public url = "https://localhost:7066/api/Product";

   addProduct(productData: FormData): Observable<any> {
    return this.http.post(`${this.url}/Add-Product`, productData);
  }

  getProductById(productId: number): Observable<any> {
    return this.http.get<any>(`${this.url}/${productId}`);
  }

   public DeleteProduct(id : any): Observable<any>{
    return this.http.delete<any>(`${this.url}/Delete-Product?id=${id}`)
   }

   public getAllProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.url}/GetAll-Product`);
  }

  //  public updateProduct(product: any): Observable<any> {
  //   return this.http.put<any>(`${this.url}/Update-Product/${product.id}`, product);
  // }

  updateProduct(productId: number, formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.url}/Update-Product/${productId}`, formData);
  }
}
