import { Injectable, Inject } from "@angular/core";
import { Http, Request, RequestMethod } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { Site } from "./site.model";
import "rxjs/add/operator/map";
import "rxjs/add/observable/of";

/**
 * Сервис для выполения запросов к АПИ
 */
@Injectable()
export class RestDataSource {
  baseUrl: string;
  auth_token: string;

  constructor(private http: Http, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  /**
   * Выполнить аутентификацию
   * @param user логин
   * @param pass пароль
   */
  authenticate(user: string, pass: string): Observable<boolean> {
    return this.http.request(new Request({
      method: RequestMethod.Post,
      url: this.baseUrl + "api/Auth/Login",
      body: { login: user, password: pass }
    })).map(response => {
      let r = response.json();
      this.auth_token = r.success ? r.token : null;
      return r.success;
    },
      err => { return false; });
    // return Observable.of(true);
  }

  /**
   * Получить объекты сайта
   */
  getSites(): Observable<Site[]> {
    return this.sendRequest(RequestMethod.Get, "api/Site") as Observable<Site[]>;
  }

  /**
   * Сохранить объект сайта
   * @param site  объект сайта
   */
  saveSite(site: Site): Observable<Site> {
    return this.sendRequest(RequestMethod.Post, "api/Site/Save",
      site, true) as Observable<Site>;
  }

  /**
   * Обновить объект. !!! Метод нафиг не нужен, saveSite - сначала был на put, но метод апишнки 1 и на post
   * Переделать/удалить лишний
   * @param site объект сайта
   */
  updateSite(site): Observable<Site> {
    return this.sendRequest(RequestMethod.Post,
      `api/Site/Save`, site, true) as Observable<Site>;
  }

  /**
   * Удалить сайт
   * @param id Ид объекта
   */
  deleteSite(id: string): Observable<boolean> {
    return this.sendRequest(RequestMethod.Delete,
      `api/Site/${id}`, null, true) as Observable<boolean>;
  }

  /**
   * Выполнить запрос к серверу
   * @param verb метод
   * @param url урл
   * @param body тело запроса
   * @param auth выполнить ли запрос с аутентификацией
   */
  private sendRequest(verb: RequestMethod,
    url: string, body?: Site, auth: boolean = false)
    : Observable<Site | Site[] | boolean> {

    let request = new Request({
      method: verb,
      url: this.baseUrl + url,
      body: body
    });
    if (auth && this.auth_token != null) {
      request.headers.set("Authorization", `Bearer ${this.auth_token}`);
    }
    return this.http.request(request).map(response => {
      if (response && verb !== RequestMethod.Delete) {
        return response.json();
      } else {
        return response.ok;
      }
    }, err => console.log(err));
  }
}
