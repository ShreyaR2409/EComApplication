import { Component } from '@angular/core';
import { ProductService } from '../../services/productServices/product.service';
import { CartService } from '../../services/cartServices/cart.service';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, NavbarComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent {
  products: any[] = [];
  userid: any;
  constructor(private productService: ProductService, private cartService: CartService) {
    this.userid = sessionStorage.getItem("id");
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data;
    });
  }

  addToCart(product: any): void {
    if (product.stock > 0) {
      const requestBody = {
        productId: product.id,
        quantity: 1 
      };
      this.cartService.AddToCart(this.userid, requestBody).subscribe(() => {
        alert('Product added to cart!');
      });
    } else {
      alert('Product is out of stock!');
    }
  }
}

