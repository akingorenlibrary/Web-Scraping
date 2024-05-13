import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutComponent } from './components/layout/layout.component';
import { NavbarComponent } from './components/layout/navbar/navbar.component';
import { SearchComponent } from './components/search/search.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { ResultComponent } from './components/result/result.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { DetayComponent } from './components/detay/detay.component';

@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    NavbarComponent,
    SearchComponent,
    NotfoundComponent,
    ResultComponent,
    DetayComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
