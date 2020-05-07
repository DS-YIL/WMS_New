import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MENU_ITEMS } from './pages-menu';
import { Employee } from '../Models/Common.Model';

@Component({
  selector: 'ngx-pages',
  styleUrls: ['pages.component.scss'],
  template: `
    <ngx-one-column-layout>
      <nb-menu [items]="menu"></nb-menu>
      <router-outlet></router-outlet>
    </ngx-one-column-layout>
    <p-toast [style]="{marginTop: '60px'}"  [showTransitionOptions]="'1000ms'" [hideTransitionOptions]="'1000ms'"></p-toast>
  `,
})
export class PagesComponent {
  constructor(private router: Router) { }
  public employee: Employee;
  menu = MENU_ITEMS;
  ngOnInit() {
    if (localStorage.getItem("Employee")) {
      this.employee = JSON.parse(localStorage.getItem("Employee"));
      MENU_ITEMS[1].hidden = true;
      MENU_ITEMS[2].hidden = true;
      MENU_ITEMS[3].hidden = true;
      MENU_ITEMS[4].hidden = true;
      MENU_ITEMS[5].hidden = true;
      if (this.employee.RoleId == "1") {
        MENU_ITEMS[1].hidden = false;
        this.router.navigateByUrl('/WMS/Home');
      }
      else if (this.employee.RoleId == "2") {
        MENU_ITEMS[2].hidden = false;
        MENU_ITEMS[3].hidden = false;
        this.router.navigateByUrl('/WMS/GRNPosting');
      }
      else if (this.employee.RoleId == "3") {
        MENU_ITEMS[4].hidden = false;
        this.router.navigateByUrl('/WMS/WarehouseIncharge');
      }
      else {
        MENU_ITEMS[5].hidden = false;
        this.router.navigateByUrl('/WMS/Dashboard');
      }
    }
  }
}
