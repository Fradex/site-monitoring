import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Site } from "../model/site.model";
import { SiteRepository } from "../model/site.repository";
import { AuthService } from "../model/auth.service";

/**
 * Компонент для админки
 */
@Component({
  moduleId: module.id,
  templateUrl: "admin.component.html"
})
export class AdminComponent {

  constructor(private auth: AuthService,
    private router: Router) { }

  /**
   * Разлогиниться
   */
  logout() {
    this.auth.clear();
    this.router.navigateByUrl("/");
  }
}
