import { Component, OnInit} from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { CartService } from '../../services/cartServices/cart.service';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/productServices/product.service';
import * as bootstrap from 'bootstrap';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [NavbarComponent,CommonModule, FormsModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit  {
  userid : any;
  cart : any[] = [];
  products: any[] = [];

  paymentDetails = {
    cardNumber: '',
    expiryDate: '',
    cvv: ''
  };

constructor(private cartservice : CartService,private productService: ProductService){
  this.userid = sessionStorage.getItem("id") ?? '';
  this.GetCart();
  this.loadProducts();
  this.addToCart(this.products);
}

ngOnInit() {
  // Subscribe to the cart updates
  this.cartservice.cart$.subscribe(cartData => {
    this.cart = cartData;
  });
  this.loadProducts();
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

// removeProduct(product: any) {
//   console.log("sdsad", product.productid, product.cartid);
  
//   if (product.quantity > 1) {
//     this.cartservice.removeCartItem(product.cartid, product.productid).subscribe((data) => {
//       this.GetCart();
//     });
//   } else {
//     this.cartservice.removeCartItem(product.cartid, product.productid).subscribe((data) => {
//       this.cart = this.cart.filter(item => item.cartid !== product.productid);
//       this.GetCart();
//     });
//   }
// }

  // Remove product from cart
  removeProduct(product: any) {
    console.log("Removing product:", product.productid, product.cartid);
   
  if (product.quantity > 1) {
    this.cartservice.removeCartItem(product.cartid, product.productid).subscribe((data) => {
      this.GetCart();
    });
  } else {
    this.cartservice.removeCartItem(product.cartid, product.productid).subscribe((data) => {
      this.cart = this.cart.filter(item => item.cartid !== product.productid);
      this.GetCart();
    });
  }
  }

loadProducts(): void {
  this.productService.getAllProducts().subscribe((data) => {
    this.products = data;
  });
}

// addToCart(product: any): void {
//     const requestBody = {
//       productId: product.productid,
//       quantity: 1 
//     };
//     this.cartservice.AddToCart(this.userid, requestBody).subscribe(() => {
//       alert('Product added to cart!');
//     });
// }

 // Add product to cart
 addToCart(product: any) {
  const requestBody = {
    productId: product.productid,
    quantity: 1
  };
  this.cartservice.AddToCart(this.userid, requestBody).subscribe();
}

// Add a computed property to calculate the total price
get totalPrice(): number {
  return this.cart.reduce((sum, product) => sum + (product.sellingprice * product.quantity), 0);
}


processPayment(): void {
  // Logic to process the payment
  console.log('Payment Details:', this.paymentDetails);
  
  // Close the modal
  const modalElement = document.getElementById('checkoutModal');
  if (modalElement) {
    const checkoutModal = bootstrap.Modal.getInstance(modalElement);
    checkoutModal?.hide();
  } else {
    console.error("Checkout modal element not found");
  }
  
  // Add further logic here, like calling an API to process the payment
}

openCheckoutModal(): void {
  // Use Bootstrap's modal JavaScript to show the modal
  const checkoutModal = new bootstrap.Modal(document.getElementById('checkoutModal')!);
  checkoutModal.show();
}
// Add your methods like addToCart, removeProduct, etc.
}
