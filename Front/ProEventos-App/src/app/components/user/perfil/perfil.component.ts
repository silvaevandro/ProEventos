import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/Helpers/ValidatorsField';
import { environment } from '@enviroments/environment';
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

  //form!: FormGroup;
  usuario = {} as UserUpdate;
  public imagemURL: string = "";
  public file!: File;

  get ehPalestrante(): boolean{
    return this.usuario.funcao == "Palestrante"
  }

  constructor(
    private spinner: NgxSpinnerService,
    private userService: UserService,
    private toastr: ToastrService,
  ) { }

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {
  }

  public getFormValue(usuario: UserUpdate): void{
    this.usuario = usuario
    if (usuario.imagemURL)
      this.imagemURL = `${environment.apiURLPerfil}${usuario.imagemURL}`
    else
      this.imagemURL = "assets/img/perfil.jpg"
  }

  public onFileChange(ev: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;


    this.file = ev.target.files[0];
    reader.readAsDataURL(this.file);
    this.uploadImagem();
  }

  public uploadImagem(): void {
    this.spinner.show();
    this.userService.postUpload(this.file).subscribe({
      next:() => {
        this.toastr.success('Imagem atualizada com Sucesso', 'Sucesso!');
      },
      error:(err: any) => {
        this.toastr.error('Erro ao fazer upload de imagem', 'Erro!');
        console.error(err);
      }
    }).add(() => this.spinner.hide());
  }
}
