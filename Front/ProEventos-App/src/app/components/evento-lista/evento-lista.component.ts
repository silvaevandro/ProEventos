import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Evento } from 'src/models/Evento';
import { EventoService } from 'src/services/evento.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';


type NewType = ToastrService;

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {



  public modalRef?: BsModalRef;
  public message?: string;
  public eventoId = 0;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public mostrarImagem: boolean = false;
  private _filtroLista: string = "";

  public larguraImagem: number = 150;

  public get filtroLista() : string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this._filtroLista) : this.eventos
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  }

  public filtrarEventos(filtrar: string): Evento[]{
    filtrar = filtrar.toLocaleLowerCase();
    return this.eventosFiltrados.filter(
      evento => evento.tema!.toLocaleLowerCase().indexOf(filtrar) !== -1
    );
  }

  public getEventos(): void {
    this.eventoService.getEventos().subscribe({
      next: (evento : Evento[]) => {
        this.eventos = evento
        this.eventosFiltrados = evento
        this.spinner.hide();
      },
      error: error => {
        this.toastr.error(error.message, 'Erro');
        this.spinner.hide();
      },
      complete: () => {
        this.spinner.hide();
      }
    });
  }

  //Confirm
  openModal(event: any, template: TemplateRef<any>, eventoId: number) {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService.deleteEvento(this.eventoId).subscribe({
      next: (result: any) => {
        if (result.message == "Deletado") {
          console.log(result);
          this.toastr.success("Evento deletado com sucesso.", 'Deletado!')
          this.spinner.hide();
          this.getEventos();
        }
      },
      error: (err) => {
        this.toastr.error(err.message, 'Erro');
        this.spinner.hide();
      },
      complete: () => {this.spinner.hide},
    })
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`])
  }
}
