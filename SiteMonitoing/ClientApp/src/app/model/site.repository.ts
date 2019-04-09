import { Injectable } from "@angular/core";
import { Site } from "./site.model";
import { RestDataSource } from "./rest.datasource";
import { StaticDataSource } from "./static.datasource";

@Injectable()
export class SiteRepository {
  private sites: Site[] = [];

  constructor(private dataSource: RestDataSource, private staticDataSource: StaticDataSource) {
    dataSource.getSites().subscribe(data => {
      this.sites = data;
    });
  }

  getItems(): Site[] {
    return this.sites;
  }

  getItem(id: string): Site {
    return this.sites.find(p => p.id == id);
  }

  saveItem(site: Site) {
    if (!site.id) {
      this.dataSource.saveSite(site)
        .subscribe(p => this.sites.push(p));
    } else {
      this.dataSource.updateSite(site)
        .subscribe(obj => {
          this.sites.splice(this.sites.
            findIndex(p => p.id == site.id), 1, obj);
        });
    }
  }

  deleteItem(id: string) {
    this.dataSource.deleteSite(id).subscribe(res => {
      if (res) {
        this.sites.splice(this.sites.findIndex(p => p.id == id), 1);
      }
    });
  }
}
