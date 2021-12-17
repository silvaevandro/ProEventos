import { Component, OnInit } from '@angular/core';
import {
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ValidatorField } from '@app/Helpers/ValidatorsField';

@Component({
  selector: 'app-cadastro',
  templateUrl: './cadastro.component.html',
  styleUrls: ['./cadastro.component.scss'],
})
export class CadastroComponent implements OnInit {
  form!: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.validation();
  }

  public resetForm(): void {
    this.form.reset();
  }

  public validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('senha', 'confirmacaoSenha'),
    };

    this.form = this.fb.group(
      {
        primeiroNome: ['', Validators.required],
        ultimoNome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        usuario: ['', Validators.required],
        senha: ['', Validators.required],
        confirmacaoSenha: ['', Validators.required],
      },
      formOptions
    );
  }
}
