import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Evento } from 'src/models/Evento';
import { EventoService } from 'src/services/evento.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from '@enviroments/environment';
import { PaginatedResult, Pagination } from 'src/models/Pagination';
import { debounceTime, Subject } from 'rxjs';


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
  //public eventosFiltrados: Evento[] = [];
  public mostrarImagem: boolean = false;
  public pagination = {} as Pagination;
  // private _filtroLista: string = "";

  public larguraImagem: number = 150;

  public termoBuscaChanged: Subject<string> = new Subject<string>();

  // public get filtroLista() : string {
  //   return this._filtroLista;
  // }

  // public set filtroLista(value: string) {
  //   this._filtroLista = value;
  //   this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this._filtroLista) : this.eventos
  // }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.pagination = { currentPage: 1, pageSize: 3, totalItems: 3 } as Pagination;
    this.getEventos();
  }

  public getEventos(): void {
    this.spinner.show();
    this.eventoService.getEventos(this.pagination.currentPage, this.pagination.pageSize)
                      .subscribe({
      next: (paginatedResult : PaginatedResult<Evento[]>) => {
                          this.eventos = paginatedResult.result;
                          this.pagination = paginatedResult.pagination;
                          this.spinner.hide();
      },
      error: error => {
        this.toastr.error(error.message, 'Erro');
        this.spinner.hide();
      }
    }).add(() => this.spinner.hide());
  }

  public filtrarEventos(event: any): void{
    if (this.termoBuscaChanged.observers.length == 0){
      this.termoBuscaChanged.pipe(debounceTime(500)).subscribe(
        filtrarPor => {
          this.spinner.show();
          this.eventoService.getEventos(this.pagination.currentPage, this.pagination.pageSize, filtrarPor)
                            .subscribe({
            next: (paginatedResult : PaginatedResult<Evento[]>) => {
                                this.eventos = paginatedResult.result;
                                this.pagination = paginatedResult.pagination;
                                this.spinner.hide();
            },
            error: error => {
              this.toastr.error(error.message, 'Erro');
              this.spinner.hide();
            }
          }).add(() => this.spinner.hide());
        }
      )
    }
    this.termoBuscaChanged.next(event.value);
  }

  //Confirm
  public openModal(event: any, template: TemplateRef<any>, eventoId: number) {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public mostraImage(imagemURL: string): string{
    return imagemURL != ""
      ? `${environment.apiURL}/resources/Images/${imagemURL}`
      : "assets/img/semimagem.jpeg"
  }

  public confirm(): void {
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

  public pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.getEventos();
  }

  public decline(): void {
    this.modalRef?.hide();
  }

  public detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`])
  }
}
