import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { UserService } from 'src/services/User.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent{
  isCollapsed = true;

  constructor(private router: Router,
              public userService: UserService) {}

 public logout() {
    this.userService.currentUser$
    this.userService.logout();
    this.router.navigateByUrl('/user/login')
  }

  showMenu(): boolean {
    return this.router.url != '/user/login';
  }
}
