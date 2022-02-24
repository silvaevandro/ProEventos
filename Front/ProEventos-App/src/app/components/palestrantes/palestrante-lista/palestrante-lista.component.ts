import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from '@enviroments/environment';
import { PaginatedResult, Pagination } from 'src/models/Pagination';
import { debounceTime, Subject } from 'rxjs';
import { PalestranteService } from 'src/services/palestrante.service';
import { Palestrante } from 'src/models/Palestrante';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {

  public modalRef?: BsModalRef;
  public message?: string;
  public PalestranteId = 0;
  public palestrantes: Palestrante[] = [];
  public mostrarImagem: boolean = false;
  public pagination = {} as Pagination;

  public larguraImagem: number = 150;

  public termoBuscaChanged: Subject<string> = new Subject<string>();


  constructor(
    private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.pagination = { currentPage: 1, pageSize: 3, totalItems: 3 } as Pagination;
    this.getPalestrantes();
  }

  public getImageURL(imageName: string): string{
    if (imageName != null)
      return environment.apiURLPerfil + imageName
    return "assets/img/perfil.jpg"
  }

  public getPalestrantes(): void {
    this.spinner.show();
    this.palestranteService.getPalestrantes(this.pagination.currentPage, this.pagination.pageSize)
                      .subscribe({
      next: (paginatedResult : PaginatedResult<Palestrante[]>) => {
                          this.palestrantes = paginatedResult.result;
                          this.pagination = paginatedResult.pagination;
                          this.spinner.hide();
      },
      error: error => {
        this.toastr.error(error.message, 'Erro');
        this.spinner.hide();
      }
    }).add(() => this.spinner.hide());
  }

  public filtrarPalestrantes(event: any): void{
    if (this.termoBuscaChanged.observers.length == 0){
      this.termoBuscaChanged.pipe(debounceTime(500)).subscribe(
        filtrarPor => {
          this.spinner.show();
          this.palestranteService.getPalestrantes(this.pagination.currentPage, this.pagination.pageSize, filtrarPor)
                            .subscribe({
            next: (paginatedResult : PaginatedResult<Palestrante[]>) => {
                                this.palestrantes = paginatedResult.result;
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
}
