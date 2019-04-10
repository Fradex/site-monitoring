import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { RestDataSource } from "./rest.datasource";
import "rxjs/add/operator/map";

/**
 * Сервис аутентификации )))))
 */
@Injectable()
export class AuthService {

    constructor(private datasource: RestDataSource) { }

    /**
     * Выполнить аутентификацию
     * @param username логин
     * @param password пароль
     */
    authenticate(username: string, password: string): Observable<boolean> {
        return this.datasource.authenticate(username, password);
    }

    /**
     * Аутентифицирован ли пользователь?
     */
    get authenticated(): boolean {
        return this.datasource.auth_token != null;
    }
    
    /**
     * скинуть аутентфикацию
     */
    clear() {
        this.datasource.auth_token = null;
    }
}
