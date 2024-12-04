import { Component } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { CartService } from '../../services/cartServices/cart.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [NavbarComponent,CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  userid : any;
  cart : any[] = [];
constructor(private cartservice : CartService){
  this.userid = sessionStorage.getItem("id") ?? '';
  this.GetCart();
}

GetCart(){
  console.log("Userid", this.userid);
  this.cartservice.GetCartDetail(this.userid).subscribe((data) =>{
    this.cart = data;
  })
}

get hasProductsWithQuantity(): boolean {
  return this.cart.some(product => product.quantity > 0);
}

removeProduct(product: any) {
  if (product.quantity > 1) {
    this.cartservice.removeCartItem(product.id, product.cartId).subscribe((data) => {
      product.quantity--;
      this.GetCart();
    });
  } else {
    this.cartservice.removeCartItem(product.id, product.cartId).subscribe((data) => {
      this.cart = this.cart.filter(item => item.id !== product.cartId);
      this.GetCart();
    });
  }
}
}
