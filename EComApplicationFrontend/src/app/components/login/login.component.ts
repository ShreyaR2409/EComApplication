import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/authServices/auth.service';
import * as bootstrap from 'bootstrap';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  constructor(private router: Router, private authService : AuthService) {}

  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  otpForm = new FormGroup({
    otp: new FormControl('', [Validators.required]), // Corrected
  });

  forgotPasswordForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
  });

  private storedUsername: string | null = null; // Variable to store username


  // Email getter
  get Email(): FormControl {
    return this.loginForm.get('username') as FormControl;
  }

  // Login Submitted
  loginSubmitted() {
    if (this.loginForm.valid) {
      const user = this.loginForm.value;
  
      this.authService.loginUser(user).subscribe({
        next: (res) => {
          console.log('Login form submitted', res);
  
          if (res.status === 'Failure') {
            alert("Invalid Username or Password");
          } else {
            // Show OTP modal if login is successful
            this.storedUsername = user.username || null;
            const otpModalElement = document.getElementById('otpModal');
            if (otpModalElement) {
              const otpModal = new bootstrap.Modal(otpModalElement);
              otpModal.show();
            }
          }
        },
        error: (err) => {
          console.error('Error during login', err);
          alert("An error occurred during login. Please try again later.");
        }
      });
    }
  }
  

  submitOtp() {
    if (this.otpForm.valid && this.storedUsername) {
      const otpData = {
        otp: this.otpForm.value.otp,
        username: this.storedUsername, // Include username with OTP
      };

      this.authService.verifyOtp(otpData).subscribe({
        next: (res) => {
          alert('OTP Verified Successfully!');
          this.router.navigateByUrl('home');
          this.closeOtpModal();
        },
        error: (err) => {
          console.error('Error during OTP verification', err);
          alert('OTP verification failed. Please try again.');
        },
      });
    } else {
      alert('Username is missing or OTP is invalid');
    }
  }

  openForgotPasswordModal() {
    const modalElement = document.getElementById('forgotPasswordModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }


   // Submit Forgot Password
   submitForgotPassword() {
    if (this.forgotPasswordForm.valid) {
      const email = this.forgotPasswordForm.value;
      console.log('Forgot Password Email:', email);

      this.authService.forgotPassword(email).subscribe({
        next: (res) => {
          alert('Password reset link has been sent to your email.');
          this.closeForgotPasswordModal();
        },
        error: (err) => {
          console.error('Error during password reset', err);
          alert('An error occurred. Please try again later.');
        },
      });
    }
  }

  // Close Forgot Password Modal
  closeForgotPasswordModal() {
    const forgotPasswordModalElement = document.getElementById('forgotPasswordModal');
    if (forgotPasswordModalElement) {
      const forgotPasswordModalInstance = bootstrap.Modal.getInstance(forgotPasswordModalElement);
      forgotPasswordModalInstance?.hide();
    }
  }
  // Close the OTP Modal
  closeOtpModal() {
    const otpModalElement = document.getElementById('otpModal');
    if (otpModalElement) {
      const otpModalInstance = bootstrap.Modal.getInstance(otpModalElement);
      otpModalInstance?.hide();
    }
  }
  
}
