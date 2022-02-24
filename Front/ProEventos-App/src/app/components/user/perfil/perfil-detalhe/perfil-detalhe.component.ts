import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { UserUpdate } from 'src/models/Identity/UserUpdate';
import { Palestrante } from 'src/models/Palestrante';
import { PalestranteService } from 'src/services/palestrante.service';
import { UserService } from 'src/services/User.service';


@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {

  @Output() changeFormValue = new EventEmitter();

  form!: FormGroup;
  userUpdate = {} as UserUpdate;

  constructor(
    private fb: FormBuilder,
    public userService: UserService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService,
    private palestranteService: PalestranteService,
  ) { }

  get f(): any {
    return this.form.controls;
  }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  public resetForm(): void {
    this.form.reset();
  }

  public onSubmit(): void{
    this.atualizarUsuario();
  }

  private verificaForm(): void{
    this.form.valueChanges.subscribe({
      next: () => this.changeFormValue.emit({... this.form.value})
    })
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

  public validation(): void {
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
      imagemURL: [''],
    });
  }

  public atualizarUsuario(): void {
    this.userUpdate = { ...this.form.value }
    this.spinner.show();
    console.log(this.f.funcao.value);

    if (this.f.funcao.value == 'Palestrante') {
      this.palestranteService.postPalestrante({} as Palestrante).subscribe({
        next: () => {
          this.toaster.success('Função Palestrante ativada', 'Sucesso')
        },
        error: (err) => {
          this.toaster.error("A função palestrante não pode ser Ativada", 'Erro')
          console.error(err)
        }
      })
    }

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

}
