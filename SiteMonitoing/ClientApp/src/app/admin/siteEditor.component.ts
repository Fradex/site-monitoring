import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { NgForm } from "@angular/forms";
import { Site } from "../model/site.model";
import { SiteRepository } from "../model/site.repository";

@Component({
  moduleId: module.id,
  templateUrl: "siteEditor.component.html"
})
export class SiteEditorComponent {
  editing: boolean = false;
  site: Site = new Site();

  constructor(private repository: SiteRepository,
    private router: Router,
    activeRoute: ActivatedRoute) {

    this.editing = activeRoute.snapshot.params["mode"] == "edit";
    if (this.editing) {
      Object.assign(this.site,
        repository.getItem(activeRoute.snapshot.params["id"]));
    }
  }

  save(form: NgForm) {
    this.repository.saveItem(this.site);
    this.router.navigateByUrl("/admin/main/site");
  }
}
