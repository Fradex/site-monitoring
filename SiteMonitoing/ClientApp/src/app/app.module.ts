import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MonitoringModule } from "./monitoring/monitoring.module";
import { MonitoringComponent } from "./monitoring/monitoring.component";
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    MonitoringModule,
    RouterModule.forRoot([
      { path: "monitoring", component: MonitoringComponent },
      {
        path: "admin",
        loadChildren: "app/admin/admin.module#AdminModule",
      },
      { path: "**", redirectTo: "/monitoring" }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
