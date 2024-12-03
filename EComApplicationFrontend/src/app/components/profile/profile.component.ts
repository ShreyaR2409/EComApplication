import { Component } from '@angular/core';
import { AuthService } from '../../services/authServices/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import{Modal} from 'bootstrap';
import {NavbarComponent} from '../navbar/navbar.component'; 

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule,FormsModule, NavbarComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  username: string;
  user: any = {
    firstname: '',
    lastname: '',
    email: '',
    roleid: 0,
    dob: '',
    mobilenumber: '',
    address: '',
    zipcode: '',
    countryid: 0,
    stateid: 0,
    profileimage: null,
  };

  constructor(private authService: AuthService) {
    this.username = sessionStorage.getItem('username') ?? '';
    this.getUser();
  }

  // Fetch user details
  getUser() {
    this.authService.getUserByUsername(this.username).subscribe((data) => {
      this.user = data;
    });
  }

  // Toggle edit mode
  toggleEditMode() {
    const modalElement = document.getElementById('editProfileModal');
    if (modalElement) { // Check if the element exists
      const modal = new Modal(modalElement); // Create a new Modal instance
      modal.show();
    }
  }

  // Handle file input for profile image
  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.user.profileimage = file;
    }
  }

  // Update user profile
  updateProfile() {
    const formData = new FormData();
    formData.append('firstname', this.user.firstname);
    formData.append('lastname', this.user.lastname);
    formData.append('email', this.user.email);
    formData.append('roleid', this.user.roleid.toString());
    formData.append('dob', this.user.dob);
    formData.append('mobilenumber', this.user.mobilenumber);
    formData.append('address', this.user.address);
    formData.append('zipcode', this.user.zipcode);
    formData.append('countryid', this.user.countryid.toString());
    formData.append('stateid', this.user.stateid.toString());

    if (this.user.profileimage) {
      formData.append('profileimage', this.user.profileimage, this.user.profileimage.name);
    }

    this.authService.updateUser(this.user.id, formData).subscribe(
      (response) => {
        console.log('Profile updated successfully', response);

        // Close the modal after a successful update
        const modalElement = document.getElementById('editProfileModal');
        if (modalElement) {
          const modalInstance = Modal.getInstance(modalElement);
          if (modalInstance) {
            modalInstance.hide();
          }
        }
      },
      (error) => {
        console.error('Error updating profile', error);
      }
    );
  }

  // Logout user
  Logout() {
    this.authService.logoutUser();
  }
}