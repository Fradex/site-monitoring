import { Component } from "@angular/core";
import { Site } from "../model/site.model";
import { SiteRepository } from "../model/site.repository";

/**
 * Компонент с таблицей сайтов
 */
@Component({
  moduleId: module.id,
  templateUrl: "siteTable.component.html"
})
export class SiteTableComponent {
  public sitesPerPage = 10;
  public selectedPage = 1;

  constructor(private repository: SiteRepository) { }

  /**
   * геттер для списка сайтов
   */
  get sites(): Site[] {
    let pageIndex = (this.selectedPage - 1) * this.sitesPerPage;
    return this.repository.getItems()
      .slice(pageIndex, pageIndex + this.sitesPerPage);
  }

  /**
   * Выполнить удаленик
   */
  deleteSite(id: string) {
    this.repository.deleteItem(id);
  }

  /**
   * изменить страницу пагинатора
   */
  changePage(newPage: number) {
    this.selectedPage = newPage;
  }

  /** 
   * Изменить кол-во элементов списка на странице
  */
  changePageSize(newSize: number) {
    this.sitesPerPage = Number(newSize);
    this.changePage(1);
  }
  
  /**
   * генттер кол-ва страниц
   */
  get pageCount(): number {
    return Math.ceil(this.repository
      .getItems().length /
      this.sitesPerPage);
  }
}
