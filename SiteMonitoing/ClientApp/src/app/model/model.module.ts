import { NgModule } from "@angular/core";
import { SiteRepository } from "./site.repository";
import { StaticDataSource } from "./static.datasource";
import { RestDataSource } from "./rest.datasource";
import { HttpModule } from "@angular/http";
import { AuthService } from "./auth.service";
import { CounterDirective } from "./counter.directive";

@NgModule({
  declarations: [CounterDirective],
  imports: [HttpModule],
    providers: [SiteRepository,
       StaticDataSource,
    RestDataSource, AuthService],
  exports: [CounterDirective]
})
export class ModelModule { }
