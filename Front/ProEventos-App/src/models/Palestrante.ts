import { Evento } from "./Evento";
import { User } from "./Identity/User";
import { UserUpdate } from "./Identity/UserUpdate";
import { RedeSocial } from "./RedeSocial";

export interface Palestrante {
  id: number;
  miniCurriculo: string;
  user: UserUpdate;
  redesSociais: RedeSocial[];
  palestrantesEventos: Evento[];
  }
