import { Component } from "@angular/core";
import { Site } from "../model/site.model";
import { SiteRepository } from "../model/site.repository";

@Component({
  moduleId: module.id,
  templateUrl: "siteTable.component.html"
})
export class SiteTableComponent {
  public sitesPerPage = 10;
  public selectedPage = 1;

  constructor(private repository: SiteRepository) { }

  get sites(): Site[] {
    let pageIndex = (this.selectedPage - 1) * this.sitesPerPage;
    return this.repository.getItems()
      .slice(pageIndex, pageIndex + this.sitesPerPage);
  }

  deleteSite(id: string) {
    this.repository.deleteItem(id);
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
