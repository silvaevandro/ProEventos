import { Component , OnInit} from '@angular/core';
import { User } from 'src/models/Identity/User';
import { UserService } from 'src/services/User.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(public userService: UserService) { }

  ngOnInit() {
    this.setCurrentUser();
  }

  public setCurrentUser(): void {
    let user = {} as User;
    if (localStorage.getItem('user')) {
      user = JSON.parse(localStorage.getItem('user')!)
      this.userService.setCurrentUser(user);
    }
  }
}
