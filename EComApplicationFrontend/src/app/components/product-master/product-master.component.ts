import { Component } from '@angular/core';
import { ProductService } from '../../services/productServices/product.service';
import { CommonModule } from '@angular/common';
import {NgxPaginationModule} from 'ngx-pagination';
import { HomeComponent } from '../home/home.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-product-master',
  standalone: true,
  imports: [CommonModule, NgxPaginationModule, HomeComponent,RouterModule],
  templateUrl: './product-master.component.html',
  styleUrl: './product-master.component.css'
})

export class ProductMasterComponent {
  products : any[] = [];
  row : any;
  constructor(private productservice : ProductService){
    this.getProducts();
  }

  getProducts() {
   this.productservice.getAllProducts().subscribe((data) =>{
    this.products = data;
   })
  }

}
