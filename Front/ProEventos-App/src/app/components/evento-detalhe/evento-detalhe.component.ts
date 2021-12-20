import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/models/Evento';
import { EventoService } from 'src/services/evento.service';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  form!: FormGroup;
  locale = 'pt-BR';
  evento!: Evento;

  constructor( private fb: FormBuilder,
               private localeService: BsLocaleService,
               private router: ActivatedRoute,
               private eventoService: EventoService,
               private spinner: NgxSpinnerService,
               private toastr: ToastrService,
               ) {
    this.localeService.use('pt-br');
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return { isAnimated: true,   adaptivePosition: true,  dateInputFormat: 'DD/MM/YYYY h:mm:ss a', containerClass: 'theme-default', showWeekNumbers: false };
  }

  public carregarEvento(): void{
    this.spinner.show();
    const eventoIdParm = this.router.snapshot.paramMap.get('id');
    if (eventoIdParm != null){
      this.eventoService.getEventoById(+eventoIdParm).subscribe({
        next: (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
        },
        error: (err: any) => {
          this.toastr.error(err, 'Erro!')
        },
        complete: () => {
          this.spinner.hide();
        },
      });
    }
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();
  }

  public resetForm(): void{
    this.form.reset();
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', Validators.required],
      //lote: ['', Validators.required],
      imagemURL: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  }
}
