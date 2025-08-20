import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  imports: [] // cualquier otro componente necesario
})
export class SidebarComponent {
  userName: string | null = null;


  constructor(private auth: AuthService) {}

  ngOnInit() {
    this.userName = this.auth.getUserName();
    console.log('User Name:', this.userName);
  }
}
