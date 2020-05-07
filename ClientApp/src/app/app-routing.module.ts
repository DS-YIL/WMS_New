import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';


const routes: Routes = [
  {
    path: 'WMS',
    loadChildren: () => import('./pages/pages.module')
      .then(m => m.PagesModule),
  },
  {
    path: '', redirectTo: 'WMS', pathMatch: 'full'
  },
  { path: '**', redirectTo: 'WMS' },
];
const config: ExtraOptions = {
  useHash: false,
  onSameUrlNavigation: 'reload'
};


@NgModule({
  imports: [RouterModule.forRoot(routes, config)],
  exports: [RouterModule]
})
export class AppRoutingModule { } //export const
 // RoutingComponent = [];
