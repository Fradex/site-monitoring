import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { NgForm, FormControl, Validators } from "@angular/forms";
import { Site } from "../model/site.model";
import { SiteRepository } from "../model/site.repository";

@Component({
  moduleId: module.id,
  templateUrl: "siteEditor.component.html"
})
export class SiteEditorComponent {
  editing: boolean = false;
  site: Site = new Site();
  checkedIntervalControl: FormControl;

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
    var checkedInterval = form.controls["checkedInterval"];

    if (checkedInterval.value <= 0) {

      return;
    }

    this.repository.saveItem(this.site);
    this.router.navigateByUrl("/admin/main/site");
  }
}
