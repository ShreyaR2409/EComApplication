import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/productServices/product.service';
import { CommonModule } from '@angular/common';
import {NgxPaginationModule} from 'ngx-pagination';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterLink, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import * as bootstrap from 'bootstrap';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-product-master',
  standalone: true,
  imports: [CommonModule, NgxPaginationModule, NavbarComponent,RouterModule, FormsModule, MatSnackBarModule, RouterLink],
  templateUrl: './product-master.component.html',
  styleUrl: './product-master.component.css'
})

export class ProductMasterComponent implements OnInit{
  purchasePrice: number = 0;
  sellingPrice: number = 0;
  PriceMismatch: boolean = false;
  priceError: boolean = false;

  products: any[] = [];
  row: number = 1;
  selectedProduct: any;

  newProduct: any = {
    productname: '',
    productcode: '',
    category: '',
    brand: '',
    sellingprice: 0,
    purchaseprice: 0,
    purchasedate: new Date(),
    stock: 0,
    productimg: null, 
  };

  constructor(private productService: ProductService, private snackBar: MatSnackBar) {
    // this.getProducts();
  }

  ngOnInit() {
    this.productService.getAllProducts().subscribe({
      next: (data) => {
        this.products = data;
        console.log(this.products);        
      },
      error: (err) => {
        console.error(err);
      },
    });
  }
  
  // Fetch all products
  getProducts() {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data;
    });
  }

  validatePrices() {
    this.priceError = this.newProduct.sellingprice <= this.newProduct.purchaseprice;
  }
 
  // Show the Add Product modal
  showAddProductModal() {
    const modalElement = document.getElementById('addProductModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  // Show the Edit Product modal
  showEditProductModal(productId: number) {
    this.productService.getProductById(productId).subscribe((product) => {
      this.selectedProduct = { ...product }; 
      this.showEditModal(); // Show the Edit Product modal
    });
  }

  showEditModal() {
    const modalElement = document.getElementById('editProductModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

   // Show the View Product modal and fetch product details by ID
   viewProductDetails(productId: number) {
    this.productService.getProductById(productId).subscribe((product) => {
      this.selectedProduct = product;
      this.showProductModal(); // Show the product details modal
    });
  }

   // Show the Product Details modal
   showProductModal() {
    const modalElement = document.getElementById('viewProductModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

   // Close the modal
   closeModalView() {
    const modalElement = document.getElementById('viewProductModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      if (modal) {
        modal.hide();
      }
    }
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.selectedProduct.productimg = file; // Update the file in the selectedProduct object
    }
  }

  // Add a new product
  addProduct() {
    if (this.priceError) {
      alert('Please correct the errors before adding the product.');
      return;
    }
    const formData = new FormData();
    formData.append('productname', this.newProduct.productname);
    formData.append('productcode', this.newProduct.productcode);
    formData.append('category', this.newProduct.category);
    formData.append('brand', this.newProduct.brand);
    formData.append('sellingprice', this.newProduct.sellingprice.toString());
    formData.append('purchaseprice', this.newProduct.purchaseprice.toString());
    formData.append('purchasedate', this.newProduct.purchasedate.toString());
    formData.append('stock', this.newProduct.stock.toString());
    
    if (this.newProduct.productimg) {
      formData.append('productimg', this.newProduct.productimg, this.newProduct.productimg.name);
    }

    this.productService.addProduct(formData).subscribe(
      (response) => {
        console.log('Product added successfully', response);
        this.snackBar.open('Product added successfully', 'Close', {
          duration: 3000,
          verticalPosition: 'bottom', 
          horizontalPosition: 'right',
        });
        this.getProducts(); 
        this.closeModal();  
      },
      (error) => {
        console.error('Error adding product', error);
      }
    );
  }

  confirmDeleteProduct(productId: any) {
    const confirmation = window.confirm('Are you sure you want to delete this product?');
    if (confirmation) {
      this.deleteProduct(productId);
    }
  }

  deleteProduct(productId: any) {
    this.productService.DeleteProduct(productId).subscribe(
      (response) => {
        console.log('Product deleted successfully', response);
        this.getProducts(); 
      },
      (error) => {
        console.error('Error deleting product', error);
      }
    );
  }

  editProduct() {
    if (!this.selectedProduct) {
      this.snackBar.open('No product selected for editing', 'Close', { duration: 3000 });
      return;
    }
  
    const formData = new FormData();
    formData.append('productname', this.selectedProduct.productname);
    formData.append('productcode', this.selectedProduct.productcode);
    formData.append('category', this.selectedProduct.category);
    formData.append('brand', this.selectedProduct.brand);
    formData.append('sellingprice', this.selectedProduct.sellingprice.toString());
    formData.append('stock', this.selectedProduct.stock.toString());
  
    if (this.selectedProduct.productimg instanceof File) {
      formData.append('productimg', this.selectedProduct.productimg);
    }
  
    this.productService.updateProduct(this.selectedProduct.id, formData).subscribe(
      () => {
        this.snackBar.open('Product updated successfully', 'Close', { duration: 3000 });
        this.closeModalEdit();
        this.getProducts(); 
      },
      (error) => {
        this.snackBar.open('Failed to update product', 'Close', { duration: 3000 });
        console.error('Error updating product:', error);
      }
    );
  }

  // Close the modal
  closeModal() {
    const modalElement = document.getElementById('addProductModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      if (modal) {
        modal.hide();
      }
    }
  }

  closeModalEdit() {
    const modalElement = document.getElementById('editProductModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      if (modal) {
        modal.hide();
      }
    }
  }
}