import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { NgForm, FormControl, Validators } from "@angular/forms";
import { Site } from "../model/site.model";
import { SiteRepository } from "../model/site.repository";
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';

/**
 * Компонент формы редактиррования/добавления сайтов
 */
@Component({
  moduleId: module.id,
  templateUrl: "siteEditor.component.html"
})
export class SiteEditorComponent {

  /**
   * Признак редактируемости формы
   */
  editing: boolean = false;

  /**
   * Редактируемый объект
   */
  site: Site = new Site();

  /**
   * .ctor
   * @param repository репозиторий сайтов
   * @param router роутер
   * @param spinnerService сервис для спиннера
   * @param activeRoute сервис для получения параметров маршрутизации
   */
  constructor(private repository: SiteRepository,
    private router: Router,
    private spinnerService: Ng4LoadingSpinnerService,
    activeRoute: ActivatedRoute) {

    this.editing = activeRoute.snapshot.params["mode"] == "edit";
    if (this.editing) {
      Object.assign(this.site,
        repository.getItem(activeRoute.snapshot.params["id"]));
    }
  }

  /**
   * Выполнить сохранение
   * @param form форма
   */
  save(form: NgForm) {
    var checkedInterval = form.controls["checkedInterval"];

    if (checkedInterval.value <= 0) {

      return;
    }
    let spinnerService = this.spinnerService;
    let router = this.router;
    spinnerService.show();
    let successCallback = () => {
      spinnerService.hide();
      router.navigateByUrl("/admin/main/site");
    }

    this.repository.saveItem(this.site, successCallback);
  }
}
