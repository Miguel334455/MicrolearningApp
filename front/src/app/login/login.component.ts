import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email = '';
  password = '';
  name = '';


  constructor(private auth: AuthService, private router:Router) {}

  login() {
    this.auth.login(this.email, this.password).subscribe({
      next: res => {
        if (res && res.token) {
          this.auth.saveToken(res.token);
          this.router.navigate(['/']);
          alert('Login success');
        } else {
          alert('Invalid credentials');
        }
      },
      error: err => {
        console.error('Login error', err);
        alert('Login failed');
      }
    });
  }

  register() {
    this.auth.register(this.name, this.email, this.password).subscribe({
      next: res => {
        alert('Registration successful');
      },
      error: err => {
        console.error('Registration error', err);
        alert('Registration failed');
      }
    });
  }
}
