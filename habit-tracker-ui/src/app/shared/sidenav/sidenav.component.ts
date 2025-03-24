import { AuthService } from '../../features/auth/auth.service';
import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit {
  isDesktop = true;
  isMobile = false;

  constructor(public authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.checkScreenSize();
  }

  @HostListener('window:resize', [])
  checkScreenSize(): void {
    this.isDesktop = window.innerWidth > 768;
    this.isMobile = !this.isDesktop;
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/login'], { replaceUrl: true });
  }
}

