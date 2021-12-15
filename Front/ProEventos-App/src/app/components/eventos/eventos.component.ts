import { HttpClient } from '@angular/common/http';
import { Component, TemplateRef } from '@angular/core';
import { Evento } from 'src/models/Evento';
import { EventoService } from 'src/services/evento.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent  {

  constructor() { }

  
}
