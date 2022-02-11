import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@enviroments/environment';
import { Observable, take } from 'rxjs';
import { Evento } from 'src/models/Evento';

@Injectable()
export class EventoService {

  baseURL = environment.apiURL + '/api/eventos';
  constructor(private http: HttpClient) { }

  public getEventos(): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(this.baseURL)
      .pipe(take(1));
  }

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(`${this.baseURL}/${tema}/tema`)
      .pipe(take(1));
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http
      .get<Evento>(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }

  public postEvento(evento: Evento): Observable<Evento> {
    return this.http
      .post<Evento>(`${this.baseURL}`, evento)
      .pipe(take(1));
  }

  public putEvento(evento: Evento): Observable<Evento> {
    return this.http
      .put<Evento>(`${this.baseURL}/${evento.id}`, evento)
      .pipe(take(1));
  }

  public deleteEvento(id: number): Observable<any> {
    return this.http
      .delete(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }

  public postUpload(eventoId: number, file: any): Observable<Evento> {

    const fileToUpload = file as File;
    const formData = new FormData();
    formData.append('file', fileToUpload)
    return this.http
      .post<Evento>(`${this.baseURL}/upload-image/${eventoId}`,formData )
      .pipe(take(1));
  }

}
