import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserLogin } from 'src/models/Identity/UserLogin';
import { UserService } from 'src/services/User.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  model = {} as UserLogin;
  constructor(private userService: UserService,
              private router: Router,
              private toaster: ToastrService) { }


  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {
  }

  public login(): void {
    this.userService.login(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/dashboard')
      },
      error: (err) => {
        if (err.status == 401)
          this.toaster.error("Usuário ou senha inválidos", "Erro");
        else {
          this.toaster.error("Erro ao efetuar o login", "Erro");
          console.error(err)
        }
      }
    })
  }
}
