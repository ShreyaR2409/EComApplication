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
export class HomeComponent implements OnInit{
  userRole: string | null = null;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.checkUserRole();
  }

  checkUserRole(): void {
    this.userRole = this.authService.getUserRole();
  }

  isAdmin(): boolean {
    return this.userRole === '1'; // Dynamically check if the user is admin
  }

}
