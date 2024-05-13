import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component';
import { SearchComponent } from './components/search/search.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { DetayComponent } from './components/detay/detay.component';

const routes: Routes = [
  {path:"",component:LayoutComponent,children:[
    {path:"",redirectTo:"/search", pathMatch:"full"},
    {path:"search",component:SearchComponent},
    {path:"detay/:id", component:DetayComponent},
  ]},
  {path:"**",component:NotfoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
