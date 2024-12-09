import { Component, OnInit  } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/authServices/auth.service';
import { RouterLink,Router } from '@angular/router';
import { CartService } from '../../services/cartServices/cart.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})

export class NavbarComponent implements OnInit{
  userRole: string | null = null;
  userName : string ;
  isAdmin : boolean = true;
  profile: any;
  profileimg: string = '';
  cartCount: number = 0;

  constructor(private authService: AuthService, private router: Router,private cartService: CartService) {
    this.userRole = sessionStorage.getItem("role");
    this.userName = sessionStorage.getItem("username") ?? '';
    this.getProfile();
    this.isAdminFn();
  } 

    ngOnInit(): void {
    // Subscribe to cart count observable to get the updated count
    this.cartService.cartCount$.subscribe((count) => {
      this.cartCount = count;
    });
  }

  // ngOnInit(): void {
  //   if (this.userRole === "Admin") {
  //     this.router.navigate(['/product-master']);

  //   } else {
  //     this.router.navigate(['/product-list']);

  //   }
  // }

  isAdminFn(): boolean {
    console.log("Role",this.userRole )
    if (this.userRole === "Admin") {
      this.isAdmin = true;
      return true;
    } else {
      this.isAdmin = false;
      return false;
    }
  }

  getProfile(){
    this.authService.getUserByUsername(this.userName).subscribe((data) => {
      this.profile = data;
      console.log("User Profile",this.profile);
      
      this.profileimg = this.profile.profileimage;
      console.log("profileimg", this.profileimg);
    })
  }

  Logout() {
    this.authService.logoutUser();
  }
}
