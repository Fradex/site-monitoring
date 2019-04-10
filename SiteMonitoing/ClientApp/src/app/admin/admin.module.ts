import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { AuthComponent } from "./auth.component";
import { AdminComponent } from "./admin.component";
import { SiteTableComponent } from "./siteTable.component";
import { ModelModule } from "../model/model.module";
import { SiteEditorComponent } from "./siteEditor.component";
import { AuthGuard } from "./auth.guard";
import { Ng4LoadingSpinnerModule } from 'ng4-loading-spinner';

let routing = RouterModule.forChild([
  { path: "auth", component: AuthComponent },
  {
    path: "main", component: AdminComponent, canActivate: [AuthGuard],
    children: [
      { path: "site/:mode/:id", component: SiteEditorComponent },
      { path: "site/:mode", component: SiteEditorComponent },
      { path: "site", component: SiteTableComponent },
      { path: "**", redirectTo: "site" }
    ]
  },
  { path: "**", redirectTo: "auth" }
]);

@NgModule({
  imports: [ModelModule, Ng4LoadingSpinnerModule.forRoot(), CommonModule, FormsModule, ReactiveFormsModule, routing],
  providers: [AuthGuard],
  declarations: [AuthComponent, AdminComponent, SiteTableComponent, SiteEditorComponent]
})
export class AdminModule { }
