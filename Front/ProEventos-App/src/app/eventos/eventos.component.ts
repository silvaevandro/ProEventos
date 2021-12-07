import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = [];
  public eventosFiltrados: any = [];
  mostrarImagem: boolean = false;
  private _filtroLista: string = "";

  public get filtroLista() : string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this._filtroLista) : this.eventos
  }

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getEventos();
  }

  filtrarEventos(filtrar: string): any{
    filtrar = filtrar.toLocaleLowerCase();
    return this.eventosFiltrados.filter(
      (evento: { tema: string; }) => evento.tema.toLocaleLowerCase().indexOf(filtrar) !== -1
    );
  }


  public getEventos(): void{
    this.http.get('https://localhost:7272/api/eventos').subscribe(
      res => {
        this.eventos = res
        this.eventosFiltrados = res
      },
      error => console.log(error)
    );
  }
}
