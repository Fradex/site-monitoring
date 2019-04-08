import { Injectable } from "@angular/core";
import { Site } from "./site.model";
import { Observable } from "rxjs/Observable";
import "rxjs/add/observable/from";

@Injectable()
export class StaticDataSource {
  private sites: Site[] = [
    new Site("Google", "Google", "https://google.com", "Product 1 (Category 1)", true),
    new Site("Product 2", "Category 1", "Product 2 (Category 1)", "Product 1 (Category 1)", false),
    new Site("Product 3", "Category 1", "Product 3 (Category 1)", "Product 1 (Category 1)", true),
    new Site("Product 4", "Category 1", "Product 4 (Category 1)", "Product 1 (Category 1)", true),
    new Site("Product 5", "Category 1", "Product 5 (Category 1)", "Product 1 (Category 1)",false),
    new Site("Product 6", "Category 2", "Product 6 (Category 2)", "Product 1 (Category 1)", false),
    new Site("Product 7", "Category 2", "Product 7 (Category 2)", "Product 1 (Category 1)",false),
    new Site("Product 8", "Category 2", "Product 8 (Category 2)", "Product 1 (Category 1)", false),
    new Site("Product 9", "Category 2", "Product 9 (Category 2)", "Product 1 (Category 1)", false),
    new Site("Product 10", "Category 2", "Product 10 (Category 2)", "Product 1 (Category 1)", false),
    new Site("Product 11", "Category 3", "Product 11 (Category 3)", "Product 1 (Category 1)",false),
    new Site("Product 12", "Category 3", "Product 12 (Category 3)", "Product 1 (Category 1)",false),
    new Site("Product 13", "Category 3", "Product 13 (Category 3)", "Product 1 (Category 1)",false),
    new Site("Product 14", "Category 3", "Product 14 (Category 3)", "Product 1 (Category 1)",false),
    new Site("Product 15", "Category 3", "Product 15 (Category 3)", "Product 1 (Category 1)",false),
    ];

    getSites(): Observable<Site[]> {
      return Observable.from([this.sites]);
    }    
}
