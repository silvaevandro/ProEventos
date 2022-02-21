import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  AbstractControl,
  Form,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '@enviroments/environment';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/models/Evento';
import { Lote } from 'src/models/Lote';
import { EventoService } from 'src/services/evento.service';
import { LoteService } from 'src/services/lote.service';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  form!: FormGroup;
  locale = 'pt-BR';
  evento = {} as Evento;
  trnMode = "post";
  eventoId: number = 0;
  loteAtual = { id: 0, nome: '', index: 0 };
  modalRef!: BsModalRef;
  imagemURL = 'assets/img/semimagem.jpeg';
  file!: File;

  constructor( private fb: FormBuilder,
               private localeService: BsLocaleService,
               private activeRoute: ActivatedRoute,
               private router: Router,
               private eventoService: EventoService,
               private loteService: LoteService,
               private spinner: NgxSpinnerService,
               private toastr: ToastrService,
               private modalService: BsModalService,
               ) {
    this.localeService.use('pt-br');
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return { isAnimated: true,   adaptivePosition: true,  dateInputFormat: 'DD/MM/YYYY h:mm:ss a', containerClass: 'theme-default', showWeekNumbers: false };
  }

  get bsConfigLote(): any {
    return { isAnimated: true,   adaptivePosition: true,  dateInputFormat: 'DD/MM/YYYY', containerClass: 'theme-default', showWeekNumbers: false };
  }

  get lotes(): FormArray {
    return (this.form.get('lotes') as FormArray)
  }

  get editando(): boolean{
    return this.trnMode == 'put'
  }

  public carregarEvento(id: number): void{
    this.eventoId = id == 0 ? +this.activeRoute.snapshot.paramMap.get('id')! : id;
    if (this.eventoId != null && this.eventoId != 0) {
      this.spinner.show()
      this.trnMode = 'put'
      if (this.eventoId != 0) {
        this.eventoService.getEventoById(this.eventoId).subscribe({
          next: (evento: Evento) => {
            this.evento = { ...evento };
            this.form.patchValue(this.evento);

            if (this.evento.imagemURL != null && this.evento.imagemURL != '')
              this.imagemURL = environment.apiURLImages + this.evento.imagemURL


            this.evento.lotes?.forEach(lote => {
              this.lotes.push(this.criarLote(lote))
            });
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
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEvento(0);
  }

  public resetForm(): void{
    this.form.reset();
  }

  public cssValidator(campo: FormControl | AbstractControl): any {
    return {'is-invalid': campo.errors && campo.touched}
  }

  public salvarEvento(): void {
    // this.spinner.show();
    // if (this.form.valid) {
      if (this.trnMode == 'post') {
        this.evento = { ... this.form.value }
        this.eventoService.postEvento(this.evento).subscribe({
          next: (evento: Evento) => {
            this.toastr.success(`Evento salvo com ID ${evento.id}!`, "Sucesso")
            this.router.navigate([`eventos/detalhe/${evento.id}`])
          },
          error: (err: any) => {
            console.error(err)
            this.toastr.error("Falha ao salvar o evento" + err?.error, "Erro")
          },
          complete: () => {}
        }).add(() => { this.spinner.hide() });
      }

      if (this.trnMode == 'put') {
        this.evento = {id: this.evento.id, ... this.form.value }
        this.eventoService.putEvento(this.evento,).subscribe({
          next: () => {
            this.toastr.success("Evento salvo com sucesso!", "Sucesso")
          },
          error: (err: any) => {
            console.error(err)
            this.toastr.error("Falha ao salvar o evento", "Erro")
          },
          complete: () => {}
        }).add(() => { this.spinner.hide() });
      }
    // }
  }


  public carregarLotes(): void{
    this.loteService.getELotesByEventoId(this.eventoId).subscribe({
      next: (lotes: Lote[]) => { this.evento.lotes?.push(...lotes) },
      error: () => { },
    }).add(() => {})
  }

  public salvarLote(): void {
    if (this.lotes.valid) {

      this.spinner.show();
      this.loteService.saveLote(this.evento.id, this.form.value.lotes).subscribe({
        next: () => {
          this.toastr.success("Lotes salvos com sucesso!", "Sucesso")

        },
        error: (err: any) => {
            console.error(err)
            this.toastr.error("Falha ao salvar o lote", "Erro")
        },
        complete: () => {

        }
      }).add(() => { this.spinner.hide() });
      this.spinner.hide();
    }
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  criarLote(lote: Lote): FormGroup{
    return  this.fb.group({
        id: [lote.id],
        nome: [lote.nome, Validators.required],
        preco: [lote.preco, Validators.required],
        dataInicio: [lote.dataInicio],
        dataFim: [lote.dataFim],
        quantidade: [lote.quantidade, Validators.required],
      })
  }

  public mudarValorData(value: Date, index: number, campo: string): void{
    this.lotes.value[index][campo] = value;
  }

  public removerLote(template: TemplateRef<any>, index: number): void{
    this.loteAtual.id = this.lotes.get(index + '.id')?.value
    this.loteAtual.nome = this.lotes.get(index + '.nome')?.value
    this.loteAtual.index = index
    this.modalRef = this.modalService.show(template, {
    class: 'modal-sm'
    })
  }

  public confirmDeleteLote() {
    this.modalRef.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe({
      next: () => {
        this.toastr.success("Lotes excluído com sucesso!", "Sucesso")
        this.lotes.removeAt(this.loteAtual.index)
      },
      error: (err: any) => {
          console.error(err)
          this.toastr.error("Falha ao excluir o lote", "Erro")
      },
    }).add(() => {this.spinner.hide()})
  }

  public declineDeleteLote() {
    this.modalRef.hide();
  }

  public retornaTituloLote(nome: string): string {
    return nome === null || nome === "" ? 'Nome do lote' : nome
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
    this.eventoService.postUpload(this.eventoId, this.file).subscribe({
      next:() => {
        this.carregarEvento(0);
        this.toastr.success('Imagem atualizada com Sucesso', 'Sucesso!');
      },
      error:(error: any) => {
        this.toastr.error('Erro ao fazer upload de imagem', 'Erro!');
        console.error(error);
      }
    }).add(() => this.spinner.hide());
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
      //imagemURL: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([])
    });
  }

}
