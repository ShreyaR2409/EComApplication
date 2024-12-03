import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common'; 
import { AuthService } from '../../services/authServices/auth.service';
import { Router,RouterLink } from '@angular/router'; 

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule,RouterLink],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})

export class RegistrationComponent implements OnInit{
  countries: any[] = [];
  states: any[] = [];
  roles: any[] = [];
  selectedFile: File | null = null;
  constructor(private router: Router, private authService : AuthService) {}

  RegistrationForm = new FormGroup({
    firstname : new FormControl("",[Validators.required]),
    lastname : new FormControl("",[Validators.required]),
    email : new FormControl("",[Validators.required]),
    roleid : new FormControl(null,[Validators.required]),
    dob : new FormControl("",[Validators.required]),
    mobilenumber : new FormControl("",[Validators.required]),
    profileimage : new FormControl("",[Validators.required]),
    address : new FormControl("",[Validators.required]),
    zipcode : new FormControl("",[Validators.required]),
    countryid : new FormControl(null,[Validators.required]),
    stateid : new FormControl(null,[Validators.required]),
  })

  ngOnInit(): void {
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

  // RegistrationFormSubmit(): void {
  //   if (this.RegistrationForm.valid) {
  //     const newUser = this.RegistrationForm.value;
  //     this.authService.registerUser(newUser).subscribe({
  //       next: (res) => {
  //         console.log('Registration Successful', res);
  //       },
  //       error: (err) => {
  //         console.error('Registration Failed', err);
  //       },
  //     });
  //   } else {
  //     console.error('Form is invalid');
  //   }
  // }

  RegistrationFormSubmit(): void {
    if (this.RegistrationForm.valid && this.selectedFile) {
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
          console.log('Registration Successful', res);
          this.router.navigate(['/login']); 
        },
        error: (err) => {
          console.error('Registration Failed', err);
        },
      });
    } else {
      console.error('Form is invalid');
    }
  }

}
