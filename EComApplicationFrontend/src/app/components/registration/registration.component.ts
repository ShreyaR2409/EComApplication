import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common'; 
import { AuthService } from '../../services/authServices/auth.service';
import { Router,RouterLink } from '@angular/router'; 
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,RouterLink, MatSnackBarModule, MatProgressSpinnerModule],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})

export class RegistrationComponent implements OnInit{
  countries: any[] = [];
  states: any[] = [];
  roles: any[] = [];
  selectedFile: File | null = null;
  isLoading = false;
  todayDate: string = '';

  constructor(private router: Router, private authService : AuthService, private snackBar: MatSnackBar) {}

  RegistrationForm = new FormGroup({
    firstname : new FormControl("",[Validators.required]),
    lastname : new FormControl("",[Validators.required]),
    email : new FormControl("",[Validators.required, Validators.email]),
    roleid : new FormControl(null,[Validators.required]),
    dob : new FormControl("",[Validators.required]),
    mobilenumber : new FormControl("",[Validators.required,  Validators.pattern(/^[0-9]{10}$/)]),
    profileimage : new FormControl("",[Validators.required]),
    address : new FormControl("",[Validators.required]),
    zipcode : new FormControl("",[Validators.required]),
    countryid : new FormControl(null,[Validators.required]),
    stateid : new FormControl(null,[Validators.required]),
  })

  ngOnInit(): void {
    const today = new Date();
    this.todayDate = today.toISOString().split('T')[0];
    this.fetchCountries();
    this.fetchRoles();
    // Fetch states when country changes
    this.RegistrationForm.get('countryid')?.valueChanges.subscribe((countryId) => {
      if (countryId) {
        this.fetchStatesByCountry(countryId);
      }
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.RegistrationForm.patchValue({ profileimage: file });
      this.RegistrationForm.get('profileimage')?.updateValueAndValidity();
    }
  }

   // Fetch all countries
   private fetchCountries(): void {
    this.authService.getAllCountries().subscribe(
      (data) => {
        this.countries = data;
      },
      (error) => {
        console.error('Error fetching countries:', error);
      }
    );
  }

  private fetchRoles():void{
    this.authService.getAllRoles().subscribe(
      (data) => {
        this.roles = data;
      },
      (error) => {
        console.error('Error fetching countries:', error);
      }
    )
  }

  // Fetch states by country ID
  private fetchStatesByCountry(countryId: number): void {
    this.authService.getStatesByCountryId(countryId).subscribe(
      (data) => {
        this.states = data;
        this.RegistrationForm.get('stateid')?.reset();
      },
      (error) => {
        console.error('Error fetching states:', error);
      }
    );
  }

  RegistrationFormSubmit(): void {
    if (this.RegistrationForm.invalid) {
      this.RegistrationForm.markAllAsTouched();
      console.error('Form is invalid');
    }
    if (this.RegistrationForm.valid && this.selectedFile) {
      this.isLoading = true;
      const formData = new FormData();
      formData.append('firstname', this.RegistrationForm.get('firstname')?.value || '');
      formData.append('lastname', this.RegistrationForm.get('lastname')?.value || '');
      formData.append('email', this.RegistrationForm.get('email')?.value || '');
      formData.append('roleid', this.RegistrationForm.get('roleid')?.value || '');
      formData.append('dob', this.RegistrationForm.get('dob')?.value || '');
      formData.append('mobilenumber', this.RegistrationForm.get('mobilenumber')?.value || '');
      formData.append('address', this.RegistrationForm.get('address')?.value || '');
      formData.append('zipcode', this.RegistrationForm.get('zipcode')?.value || '');
      formData.append('countryid', this.RegistrationForm.get('countryid')?.value || '');
      formData.append('stateid', this.RegistrationForm.get('stateid')?.value || '');
      formData.append('profileimage', this.selectedFile);

      this.authService.registerUser(formData).subscribe({
        next: (res) => {
          this.isLoading = false;
          console.log('Registration Successful', res);
          this.snackBar.open('User registered successfully', 'Close', {
            duration: 3000,
            verticalPosition: 'top', // or 'bottom'
            horizontalPosition: 'right', // or 'right' | 'left'
          });
          this.router.navigate(['/login']); 
        },
        error: (err) => {
          this.isLoading = false;
          console.error('Registration Failed', err);
          this.snackBar.open('Registration failed. Please try again.', 'Close', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'right',
          });
        },
      });
    } else {
      console.error('Form is invalid');
    }
  }

}
