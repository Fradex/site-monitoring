import { Component } from "@angular/core";
import { Site } from "../model/site.model";
import { SiteRepository } from "../model/site.repository";
import { Router } from "@angular/router";

/**
 * Общедоступный компонент для просмотра мониторинга сайтов
 */
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

  /**
   * Геттер с сайтами
   */
  get sites(): Site[] {
    let pageIndex = (this.selectedPage - 1) * this.sitesPerPage;
    return this.repository.getItems()
      .slice(pageIndex, pageIndex + this.sitesPerPage);
  }

  /**
   * Изменить страницу пагинатора
   * @param newPage  номер страницы
   */
  changePage(newPage: number) {
    this.selectedPage = newPage;
  }

  /**
   * Изменить кол-во записей на странице
   * @param newSize 
   */
  changePageSize(newSize: number) {
    this.sitesPerPage = Number(newSize);
    this.changePage(1);
  }

  /**
   * Кол-во записей на странице
   */
  get pageCount(): number {
    return Math.ceil(this.repository
      .getItems().length /
      this.sitesPerPage);
  }
}
