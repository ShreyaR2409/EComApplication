import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/authServices/auth.service';
import {NavbarComponent} from '../navbar/navbar.component';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [CommonModule, FormsModule,NavbarComponent],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {
  changePasswordDto = { newPassword: '' };
  confirmPassword: string = '';
  passwordsMismatch: boolean = false;

  userId: string = sessionStorage.getItem('id') || ''; 

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {

    console.log('newPassword:', this.changePasswordDto.newPassword);  
    if (this.changePasswordDto.newPassword !== this.confirmPassword) {
      this.passwordsMismatch = true;
      return;
    }

    const requestBody = {
      userId: this.userId,
      changePasswordDto: this.changePasswordDto
    };

    this.authService.changePassword(requestBody).subscribe(
      response => {
        alert('Password changed successfully');
        this.router.navigate(['/profile']); 
        
      },
      error => {
        alert('Error: ' + error.message);
      }
    );
  }
}