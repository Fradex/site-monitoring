import { Component } from "@angular/core";
import { Site } from "../model/site.model";
import { SiteRepository } from "../model/site.repository";
import { Router } from "@angular/router";

@Component({
    selector: "monitoring",
    moduleId: module.id,
    templateUrl: "monitoring.component.html"
})
export class MonitoringComponent {
    public sitesPerPage = 3;
    public selectedPage = 1;

  constructor(private repository: SiteRepository,
        private router: Router) { }

    get sites(): Site[] {
      let pageIndex = (this.selectedPage - 1) * this.sitesPerPage;
        return this.repository.getItems()
          .slice(pageIndex, pageIndex + this.sitesPerPage);
    }

    changePage(newPage: number) {
        this.selectedPage = newPage;
    }

    changePageSize(newSize: number) {
      this.sitesPerPage = Number(newSize);
        this.changePage(1);
    }

    get pageCount(): number {
      return Math.ceil(this.repository
        .getItems().length /
        this.sitesPerPage);
    }
}
