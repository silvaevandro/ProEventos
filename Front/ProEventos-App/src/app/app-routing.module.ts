import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContatosComponent } from './components/contatos/contatos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EventoDetalheComponent } from './components/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './components/evento-lista/evento-lista.component';
import { EventosComponent } from './components/eventos/eventos.component';
import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';
import { CadastroComponent } from './components/user/cadastro/cadastro.component';
import { LoginComponent } from './components/user/login/login.component';
import { PerfilComponent } from './components/user/perfil/perfil.component';
import { UserComponent } from './components/user/user.component';

const routes: Routes = [
  {
    path: 'eventos',
    redirectTo: 'eventos/lista',
  },

  {
    path: 'user',
    component: UserComponent,
    children: [
      { path: 'cadastro', component: CadastroComponent },
      { path: 'login', component: LoginComponent },
      { path: 'perfil', component: PerfilComponent },
    ],
  },
  {
    path: 'eventos',
    component: EventosComponent,
    children: [
      { path: 'detalhe/:id', component: EventoDetalheComponent },
      { path: 'detalhe', component: EventoDetalheComponent },
      { path: 'lista', component: EventoListaComponent },
    ],
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
  },
  {
    path: 'palestrantes',
    component: PalestrantesComponent,
  },
  {
    path: 'contatos',
    component: ContatosComponent,
  },
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
