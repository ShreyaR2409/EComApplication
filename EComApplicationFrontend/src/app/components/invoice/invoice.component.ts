import { Component } from '@angular/core';
import { AuthService } from '../../services/authServices/auth.service';

@Component({
  selector: 'app-invoice',
  standalone: true,
  imports: [],
  templateUrl: './invoice.component.html',
  styleUrl: './invoice.component.css'
})
export class InvoiceComponent {
  todayDate: Date;
  user : any;
  username : string = ''
  constructor(private authservice : AuthService) {
    this.todayDate = new Date();
    this.username = sessionStorage.getItem('username')!;
    this.getUserDetail();
  }

  getUserDetail(){
    this.authservice.getUserByUsername(this.username).subscribe((data)=>{
      this.user = data;
    })
  }
}
