import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@enviroments/environment';
import { map, Observable, ReplaySubject, take } from 'rxjs';
import { User } from 'src/models/Identity/User';
import { UserUpdate } from 'src/models/Identity/UserUpdate';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  baseUrl = environment.apiURL + '/api/user/';

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void> {
    return this.http.post<User>(this.baseUrl + 'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user)
        }
      })
    );
  }

  public register(model: any): Observable<void>{
    return this.http.post<User>(this.baseUrl + 'register', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user)
        }
      })
    );
  }

  public getUser(): Observable<UserUpdate>{
    return this.http.get<UserUpdate>(`${this.baseUrl}getuser`).pipe(take(1));
  }

  public updateUser(model: any): Observable<void>{
    return this.http.put<UserUpdate>(`${this.baseUrl}update`, model).pipe(
      take(1),
      map((user: UserUpdate) => {
          this.setCurrentUser(user)
      })
    );
  }

  public logout(): void{
    localStorage.removeItem('user');
    this.currentUserSource.next(null!);
    this.currentUserSource.complete();
    window.location.reload();
  }

  public setCurrentUser(user: User): void {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}
