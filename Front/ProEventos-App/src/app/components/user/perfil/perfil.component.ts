import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/Helpers/ValidatorsField';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { UserUpdate } from 'src/models/Identity/UserUpdate';
import { UserService } from 'src/services/User.service';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss'],
})
export class PerfilComponent implements OnInit {
  userUpdate = {} as UserUpdate;
  form!: FormGroup;
  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    public userService: UserService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService,
  ) { }

  ngOnInit(): void {
    this.validation();
    this.carregarUsuario();
  }

  public resetForm(): void {
    this.form.reset();
  }

  public carregarUsuario():void {
    this.spinner.show();
    this.userService.getUser().subscribe({
      next: (resp) => {
        this.userUpdate = resp;
        this.form.patchValue(this.userUpdate);
        this.toaster.success('Usuário carregado com sucesso.')
      },
      error: (err) => {
        this.toaster.error("Erro ao Carregar o Usuário", "Erro")
        console.log(err)
      }
    }).add(() => {this.spinner.hide()});
  }

  public onSubmit(): void{
    this.atualizarUsuario();
  }

  public atualizarUsuario(): void {
    this.userUpdate = { ...this.form.value }
    this.spinner.show();
    this.userService.updateUser(this.userUpdate).subscribe({
      next: () => {
        this.toaster.success('Usuário atualizado com sucesso.')
      },
      error: (err) => {
        this.toaster.error("Erro ao Atualizar o Usuário", "Erro")
        console.log(err)
      }
    }).add(() => {this.spinner.hide()});
  }

  public validation(): void {
    // const formOptions: AbstractControlOptions = {
    //   validators: ValidatorField.MustMatch('senha', 'confirmacaoSenha'),
    // };

    this.form = this.fb.group({
      userName:[''],
      titulo: ['', [Validators.required]],
      primeiroNome: ['', [Validators.required]],
      ultimoNome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      funcao: ['', [Validators.required]],
      descricao: [''],
      password: [''],
      confirmacaoPassword: [''],
    });
  }
}
