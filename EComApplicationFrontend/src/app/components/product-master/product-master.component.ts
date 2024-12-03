import { Component } from '@angular/core';
import { ProductService } from '../../services/productServices/product.service';
import { CommonModule } from '@angular/common';
import {NgxPaginationModule} from 'ngx-pagination';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import * as bootstrap from 'bootstrap';
@Component({
  selector: 'app-product-master',
  standalone: true,
  imports: [CommonModule, NgxPaginationModule, NavbarComponent,RouterModule, FormsModule],
  templateUrl: './product-master.component.html',
  styleUrl: './product-master.component.css'
})

export class ProductMasterComponent {
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

  constructor(private productService: ProductService) {
    this.getProducts();
  }

  // Fetch all products
  getProducts() {
    this.productService.getAllProducts().subscribe((data) => {
      this.products = data;
    });
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
  // showEditProductModal(productId: number) {
  //   this.productService.getProductById(productId).subscribe((product) => {
  //     this.selectedProduct = { ...product }; // Copy data to selectedProduct for editing
  //     this.showEditModal(); // Show the Edit Product modal
  //   });
  // }

  // showEditModal() {
  //   const modalElement = document.getElementById('editProductModal');
  //   if (modalElement) {
  //     const modal = new bootstrap.Modal(modalElement);
  //     modal.show();
  //   }
  // }

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

  // Handle file input change
  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.newProduct.productimg = file;
    }
  }

  // Add a new product
  addProduct() {
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
        this.getProducts(); // Refresh product list
        this.closeModal();  // Close the modal after successful submission
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
        this.getProducts(); // Refresh product list after deletion
      },
      (error) => {
        console.error('Error deleting product', error);
      }
    );
  }

  // editProduct() {
  //   const formData = new FormData();
  //   formData.append('productname', this.selectedProduct.productname);
  //   formData.append('productcode', this.selectedProduct.productcode);
  //   formData.append('category', this.selectedProduct.category);
  //   formData.append('brand', this.selectedProduct.brand);
  //   formData.append('sellingprice', this.selectedProduct.sellingprice.toString());
  //   formData.append('stock', this.selectedProduct.stock.toString());
    
  //   if (this.selectedProduct.productimg) {
  //     formData.append('productimg', this.selectedProduct.productimg, this.selectedProduct.productimg.name);
  //   }

  //   this.productService.updateProduct(this.selectedProduct.id, formData).subscribe(
  //     (response) => {
  //       console.log('Product updated successfully', response);
  //       this.getProducts(); // Refresh product list
  //       this.closeModal();  // Close the modal after successful submission
  //     },
  //     (error) => {
  //       console.error('Error updating product', error);
  //     }
  //   );
  // }

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

  // closeModalEdit() {
  //   const modalElement = document.getElementById('editProductModal');
  //   if (modalElement) {
  //     const modal = bootstrap.Modal.getInstance(modalElement);
  //     if (modal) {
  //       modal.hide();
  //     }
  //   }
  // }
}