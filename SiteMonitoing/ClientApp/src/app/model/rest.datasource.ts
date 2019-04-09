import { Injectable, Inject } from "@angular/core";
import { Http, Request, RequestMethod } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { Site } from "./site.model";
import "rxjs/add/operator/map";
import "rxjs/add/observable/of";

@Injectable()
export class RestDataSource {
    baseUrl: string;
    auth_token: string;

  constructor(private http: Http, @Inject('BASE_URL') baseUrl: string) {
       this.baseUrl = baseUrl;
    }

    authenticate(user: string, pass: string): Observable<boolean> {
        return this.http.request(new Request({
            method: RequestMethod.Post,
          url: this.baseUrl + "api/Auth/Login",
            body: { login: user, password: pass }
        })).map(response => {
          debugger;
            let r = response.json();
            this.auth_token = r.success ? r.token : null;
            return r.success;
        },
          err => { return false; });
     // return Observable.of(true);
    }

    getSites(): Observable<Site[]> {
      return this.sendRequest(RequestMethod.Get, "api/Site") as Observable<Site[]>;
    }

    saveSite(site: Site): Observable<Site> {
      return this.sendRequest(RequestMethod.Post, "api/Site/Save",
        site, true) as Observable<Site>;
    }

    updateSite(site): Observable<Site> {
      return this.sendRequest(RequestMethod.Post,
          `api/Site/Save`, site, true) as Observable<Site>;
    }

    deleteSite(id: string): Observable<boolean> {
      return this.sendRequest(RequestMethod.Delete,
        `api/Site/${id}`, null, true) as Observable<boolean>;
    }

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
