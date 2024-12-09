import { Component } from '@angular/core';
import { ProductService } from '../../services/productServices/product.service';
import { CartService } from '../../services/cartServices/cart.service';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, NavbarComponent,MatSnackBarModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})

export class ProductListComponent {
  products: any[] = [];
  userid: any;
  constructor(private productService: ProductService, private cartService: CartService, private snackBar: MatSnackBar) {
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
        console.log('Product added to cart!');        
        // alert('Product added to cart!');
        this.snackBar.open('Product added to cart!', 'Close', {
          duration: 3000,
          verticalPosition: 'bottom', 
          horizontalPosition: 'right',
        });
      });
    } else {
      alert('Product is out of stock!');
    }
  }
}

