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

  constructor(private messageService: MessageService, private commonComponent: commonComponent,private formBuilder: FormBuilder, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

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
    this.getRoles();
  }

  //get Role list
  getRoles() {
    this.dynamicData = new DynamicSearchResult();
    this.dynamicData.query = "select * from wms.rolemaster";
    this.wmsService.GetListItems(this.dynamicData).subscribe(data => {
      this.roleNameModel = data;
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
              if (data.length>0) {
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
  bindMenu() {
    MENU_ITEMS[1].hidden = true;
    MENU_ITEMS[2].hidden = true;
    MENU_ITEMS[3].hidden = true;
    MENU_ITEMS[4].hidden = true;
    MENU_ITEMS[5].hidden = true;
    if (this.employee.roleid == "1") {
      MENU_ITEMS[1].hidden = false;
      this.router.navigateByUrl('/WMS/Home');
    }
    else if (this.employee.roleid == "2") {
      MENU_ITEMS[2].hidden = false;
      MENU_ITEMS[3].hidden = false;
      this.router.navigateByUrl('/WMS/GRNPosting');
    }
    else if (this.employee.roleid == "3") {
      MENU_ITEMS[4].hidden = false;
      this.router.navigateByUrl('/WMS/WarehouseIncharge');
    }
    else {
      MENU_ITEMS[5].hidden = false;
      this.router.navigateByUrl('/WMS/Dashboard');
    }
  }

}
