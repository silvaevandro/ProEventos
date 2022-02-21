import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { User } from 'src/models/Identity/User';
import { UserService } from 'src/services/User.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private userService: UserService){}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser = {} as User;
    this.userService.currentUser$.pipe(take(1)).subscribe((user: User) => {
      currentUser = user
      if (currentUser != null) {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.token}`
          }
        });
      }
    });
    return next.handle(request);
  }
}
