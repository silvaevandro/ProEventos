import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/Helpers/ValidatorsField';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/models/Identity/User';
import { UserService } from 'src/services/User.service';

@Component({
  selector: 'app-cadastro',
  templateUrl: './cadastro.component.html',
  styleUrls: ['./cadastro.component.scss'],
})
export class CadastroComponent implements OnInit {
  form!: FormGroup;
  user = {} as User;
  get f(): any {
    return this.form.controls;
  }

  constructor(private fb: FormBuilder,
              private userService: UserService,
              private router: Router,
              private toaster: ToastrService
  ) { }

  ngOnInit(): void {
    this.validation();
  }

  public resetForm(): void {
    this.form.reset();
  }

  public register(): void{
    this.user = { ... this.form.value };
    this.userService.register(this.user).subscribe({
      next: () => {
        this.router.navigateByUrl('/dashboard')
      },
      error: (err) => {
        this.toaster.error("Erro ao criar o usuário", "Erro")
        console.log(err)
      }
    })
  }

  public validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmacaoPassword'),
    };

    this.form = this.fb.group(
      {
        primeiroNome: ['', Validators.required],
        ultimoNome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        userName: ['', Validators.required],
        password: ['', Validators.required],
        confirmacaoPassword: ['', Validators.required],
      },
      formOptions
    );
  }
}
