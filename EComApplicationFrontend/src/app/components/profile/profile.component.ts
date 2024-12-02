import { Component } from '@angular/core';
import { AuthService } from '../../services/authServices/auth.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  username : string;
  user: any;
  constructor(private authService : AuthService) {
    this.username = sessionStorage.getItem("email") ?? '';
    this.getUser();
  }

  getUser(){
    this.authService.getUserByUsername(this.username).subscribe((data) => {
      this.user = data;
      console.log(this.user)
    })
  }

  Logout(){
    this.authService.logoutUser();
  }
}
