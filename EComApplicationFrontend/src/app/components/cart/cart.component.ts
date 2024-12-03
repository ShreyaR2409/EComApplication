import { Component } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { CartService } from '../../services/cartServices/cart.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [NavbarComponent, CommonModule],
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
}
