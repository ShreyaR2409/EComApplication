import { Component, OnInit  } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/authServices/auth.service';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent{
  userRole: string | null = null;
  userName : string ;
  isAdmin : boolean = true;
  profile: any;
  profileimg: string = '';

  constructor(private authService: AuthService) {
    this.userRole = sessionStorage.getItem("role");
    this.userName = sessionStorage.getItem("username") ?? '';
    this.getProfile();
    this.isAdminFn();
  }

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
