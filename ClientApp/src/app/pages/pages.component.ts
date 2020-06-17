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
<p-confirmDialog [style]="{width: '50vw'}"></p-confirmDialog>

  `,
})
export class PagesComponent {
  constructor(private router: Router) { }
  public employee: Employee;
  menu = MENU_ITEMS;
  ngOnInit() {
    if (localStorage.getItem("Employee")) {
      this.employee = JSON.parse(localStorage.getItem("Employee"));
      this.bindMenu();
    }
  }
  //bindMenu
  //1-security operator 2-inventoryenquiry, 3-inventoryclerk, 4-inventory manager, 5-project manager,6-dashboard user,7-admin

  bindMenu() {
    MENU_ITEMS[1].hidden = MENU_ITEMS[2].hidden = MENU_ITEMS[3].hidden = MENU_ITEMS[4].hidden = MENU_ITEMS[5].hidden = MENU_ITEMS[5].hidden = MENU_ITEMS[6].hidden = MENU_ITEMS[7].hidden = MENU_ITEMS[8].hidden = MENU_ITEMS[9].hidden = MENU_ITEMS[10].hidden = true;
    MENU_ITEMS[11].hidden = MENU_ITEMS[12].hidden = MENU_ITEMS[13].hidden = MENU_ITEMS[14].hidden = MENU_ITEMS[15].hidden = MENU_ITEMS[16].hidden = true;
    if (this.employee.roleid == "1") {//security perator
      MENU_ITEMS[1].hidden = false;
      this.router.navigateByUrl('/WMS/Home');
    }
    if (this.employee.roleid == "2") {//inventory enquiry
      MENU_ITEMS[2].hidden = false;
      this.router.navigateByUrl('/WMS/GRNPosting');
    }
    if (this.employee.roleid == "3") {//inventory clerk
      MENU_ITEMS[3].hidden = false;
      MENU_ITEMS[4].hidden = false;
      MENU_ITEMS[6].hidden = false;
      this.router.navigateByUrl('/WMS/WarehouseIncharge');
    }
    if (this.employee.roleid == "4") {//inventory manager
      MENU_ITEMS[3].hidden = false;
      MENU_ITEMS[4].hidden = false;
      MENU_ITEMS[6].hidden = false;
      MENU_ITEMS[13].hidden = false;
      this.router.navigateByUrl('/WMS/MaterialIssueDashboard');
    }
    if (this.employee.roleid == "5") {//project manager
      MENU_ITEMS[5].hidden = false;
      this.router.navigateByUrl('/WMS/Dashboard');
    }
    if (this.employee.roleid == "6") {//dashboard
      MENU_ITEMS[5].hidden = false;
      this.router.navigateByUrl('/WMS/Dashboard');
    }
    if (this.employee.roleid == "7") {//admin
      MENU_ITEMS[6].hidden = MENU_ITEMS[7].hidden = MENU_ITEMS[8].hidden = MENU_ITEMS[9].hidden = MENU_ITEMS[10].hidden = MENU_ITEMS[11].hidden = MENU_ITEMS[12].hidden = MENU_ITEMS[13].hidden = MENU_ITEMS[14].hidden = MENU_ITEMS[15].hidden = MENU_ITEMS[16].hidden = false;
      this.router.navigateByUrl('/WMS/GatePass');
    }
  }
}
