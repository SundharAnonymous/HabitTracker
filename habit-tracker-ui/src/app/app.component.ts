import { Component, OnInit } from '@angular/core';
import { AuthService } from './features/auth/auth.service';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'habit-tracker-ui';
  showSidenav = true;
  constructor(public authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    // Subscribe to router events to check the current URL
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        // Hide sidenav if URL is login or register
        if (event.urlAfterRedirects.startsWith('/auth/login') ||
            event.urlAfterRedirects.startsWith('/auth/register')) {
          this.showSidenav = false;
        } else {
          this.showSidenav = true;
        }
      }
    });
  }
}
