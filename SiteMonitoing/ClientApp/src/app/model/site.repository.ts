import { Injectable } from "@angular/core";
import { Site } from "./site.model";
import { RestDataSource } from "./rest.datasource";

/**
 * репозиторий сайта - хранит объекты в памяти после получения, серврная пагинация не предусмотрена.
 * можно конечно было набросать и интерфейс, но зачем?
 */
@Injectable()
export class SiteRepository {
  private sites: Site[] = [];

  constructor(private dataSource: RestDataSource) {
    dataSource.getSites().subscribe(data => {
      this.sites = data;
    });
  }

  /**
   * Получить элементы из локальной коллекции
   */
  getItems(): Site[] {
    return this.sites;
  }

  /**
   * Получить элемент по ИД из локальной коллекции
   */
  getItem(id: string): Site {
    return this.sites.find(p => p.id == id);
  }

  /**
   * Сохранить объект
   * @param site  элемент 
   * @param successCallback функция, выполняемая при успехе
   */
  saveItem(site: Site, successCallback) {
    if (!site.id) {
      this.dataSource.saveSite(site)
        .subscribe(p => {
          this.sites.push(p);
          if (successCallback) successCallback();
        });
    } else {
      this.dataSource.updateSite(site)
        .subscribe(obj => {
          this.sites.splice(this.sites.
            findIndex(p => p.id == site.id), 1, obj);
          if (successCallback) successCallback();
        });
    }
  }

  /**
   * Удалить объект
   */
  deleteItem(id: string) {
    this.dataSource.deleteSite(id).subscribe(res => {
      if (res) {
        this.sites.splice(this.sites.findIndex(p => p.id == id), 1);
      }
    });
  }
}
