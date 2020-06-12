import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { first } from 'rxjs/operators';
import { constants } from '../../Models/WMSConstants';
import { Employee, Login, DynamicSearchResult } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { MENU_ITEMS } from '../pages-menu';
import { MessageService } from 'primeng/api';
import { commonComponent } from '../../WmsCommon/CommonCode'

@Component({
  selector: 'app-Login',
  templateUrl: './Login.component.html'
})
export class LoginComponent implements OnInit {

  constructor(private messageService: MessageService, private commonComponent: commonComponent, private formBuilder: FormBuilder, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public LoginForm: FormGroup;
  public employee: Employee;
  public LoginModel: Login;
  public LoginSubmitted: boolean = false;
  public dataSaved: boolean = false;
  public dynamicData = new DynamicSearchResult();
  public roleNameModel: Array<any> = [];
  public AcessNameList: Array<any> = [];

  ngOnInit() {
    this.LoginModel = new Login();
    this.LoginModel.roleid = "0";
    this.LoginForm = this.formBuilder.group({
      DomainId: ['', [Validators.required]],
      Password: ['', [Validators.required]],
      roleid: ['', [Validators.required]],
    });
    this.commonComponent.animateCSS('login', 'zoomInDown');
    if (localStorage.getItem("Roles") && JSON.parse(localStorage.getItem("Roles")))
      this.roleNameModel = JSON.parse(localStorage.getItem("Roles"));
    else
      this.getRoles();
  }

  //get Role list
  getRoles() {
    this.dynamicData = new DynamicSearchResult();
    this.dynamicData.query = "select * from wms.rolemaster";
    this.wmsService.GetListItems(this.dynamicData).subscribe(data => {
      this.roleNameModel = data;
      localStorage.setItem('Roles', JSON.stringify(this.roleNameModel));
    })
  }

  //Login
  Login() {
    this.LoginSubmitted = true;
    if (this.LoginForm.invalid || this.LoginModel.roleid == '0') {
      return;
    }
    else {
      this.spinner.show();
      this.wmsService.ValidateLoginCredentials(this.LoginForm.value.DomainId, this.LoginForm.value.Password)
        .subscribe(data1 => {
          this.spinner.hide();
          if (data1.message != null) {
            this.messageService.add({ severity: 'error', summary: 'error Message', detail: 'Username or password is incorrect' });
          }
          if (data1.employeeno != null) {
            this.employee = data1;
            this.wmsService.getuserAcessList(this.employee.employeeno, this.LoginForm.value.roleid).subscribe(data => {
              if (data.length > 0) {
                this.AcessNameList = data;
                this.employee.roleid = this.LoginForm.value.roleid;
                localStorage.setItem('Employee', JSON.stringify(this.employee));
                this.bindMenu();
              }
              else {
                this.messageService.add({ severity: 'error', summary: 'error Message', detail: 'Selected Role is not assigned to you, select Your role' });
              }
            })
          }
        }
        );
    }
  }
  //bindMenu
  //1-security operator 2-inventoryenquiry, 3-inventoryclerk, 4-inventory manager, 5-project manager,6-dashboard user,7-admin
  bindMenu() {
    MENU_ITEMS[1].hidden = MENU_ITEMS[2].hidden = MENU_ITEMS[3].hidden = MENU_ITEMS[4].hidden = MENU_ITEMS[5].hidden = MENU_ITEMS[5].hidden = MENU_ITEMS[6].hidden = MENU_ITEMS[7].hidden = MENU_ITEMS[8].hidden = MENU_ITEMS[9].hidden = MENU_ITEMS[10].hidden = true;
    MENU_ITEMS[11].hidden = MENU_ITEMS[12].hidden = MENU_ITEMS[13].hidden = MENU_ITEMS[14].hidden = MENU_ITEMS[15].hidden = true;
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
      this.router.navigateByUrl('/WMS/WarehouseIncharge');
    }
    if (this.employee.roleid == "4") {//inventory manager
      MENU_ITEMS[3].hidden = false;
      MENU_ITEMS[4].hidden = false;
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
      MENU_ITEMS[8].hidden = MENU_ITEMS[9].hidden = MENU_ITEMS[10].hidden = MENU_ITEMS[11].hidden = MENU_ITEMS[12].hidden = MENU_ITEMS[13].hidden = MENU_ITEMS[14].hidden = MENU_ITEMS[15].hidden = false;
      this.router.navigateByUrl('/WMS/Dashboard');
    }
  }

}
