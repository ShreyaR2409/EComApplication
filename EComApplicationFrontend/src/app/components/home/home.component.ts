import { Component, OnInit  } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/authServices/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent{
  userRole: string | null = null;
  userName : string ;
  isAdmin : boolean = true;
  profile: any;
  profileimg: string = '';

  constructor(private authService: AuthService) {
    this.userRole = sessionStorage.getItem("role");
    this.userName = sessionStorage.getItem("username") ?? '';
    this.getProfile();
  }

  isAdminFn(): boolean {
    if(this.userRole ==  "1"){
      this.isAdmin = true;
      return true;
    } 
    else
    this.isAdmin = false;
    return false;
  }

  getProfile(){
    this.authService.getUserByUsername(this.userName).subscribe((data) => {
      this.profile = data;
      console.log("User Profile",this.profile);
      
      this.profileimg = this.profile.profileimage;
      console.log("profileimg", this.profileimg);
    })
  }


}
