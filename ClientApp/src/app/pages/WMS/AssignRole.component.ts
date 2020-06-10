import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee, DynamicSearchResult } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { authUser } from '../../Models/WMS.Model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-AssignRole',
  templateUrl: './AssignRole.component.html'
})
export class AssignRoleComponent implements OnInit {
  constructor(private wmsService: wmsService, private messageService: MessageService, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public employee: Employee;
  public dynamicData = new DynamicSearchResult();
  roleNameModel: Array<any> = [];
  employeeModel: Array<any> = [];
  authUsersList: Array<any> = [];
  authUser: authUser;

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");
    this.authUser = new authUser();
    this.authUser.employeeid = 0;
    this.authUser.roleid = 0;
    this.authUser.createdby = this.employee.employeeno;
    this.getEmployees();
    this.getRoles();
    this.getauthUserList();
  }

  //get EmployeeList
  getEmployees() {
    this.dynamicData = new DynamicSearchResult();
    this.dynamicData.query = "select * from wms.employee e where dol  is null and orgdepartmentid in(1,14,35,9) order by name";
    this.wmsService.GetListItems(this.dynamicData).subscribe(data => {
      this.employeeModel = data;
      //alert();
    })
  }

  //get Role list
  getRoles() {
    this.dynamicData = new DynamicSearchResult();
    this.dynamicData.query = "select * from wms.rolemaster";
    this.wmsService.GetListItems(this.dynamicData).subscribe(data => {
      this.roleNameModel = data;
    })
  }

  getauthUserList() {
    this.dynamicData = new DynamicSearchResult();
    this.dynamicData.query = "select * from wms.auth_users";
    this.wmsService.GetListItems(this.dynamicData).subscribe(data => {
      this.authUsersList = data;
    })
  }

  rolechange() {

  }
  //Assign Roles
  assignRole() {
    if (this.authUser.employeeid && this.authUser.roleid) {
      if (this.authUsersList.length > 0 && (this.authUsersList.filter(li => li.employeeid == this.authUser.employeeid && li.roleid == this.authUser.roleid).length > 0)) {
        this.messageService.add({ severity: 'error', summary: 'Validation', detail: 'Selected Role already added for this Employee' });
        return false;
      }
      this.wmsService.assignRole(this.authUser).subscribe(data => {
        if (data) {
          this.authUser.authid = data;
          var object = { authid: data, employeeid: this.authUser.employeeid, roleid: this.authUser.roleid };
          this.authUsersList.push(object);
          this.messageService.add({ severity: 'success', summary: 'success Message', detail: 'Role added' });
        }
      })
    }
    else {
      if (!this.authUser.employeeid)
        this.messageService.add({ severity: 'error', summary: 'Validation', detail: 'select Employee' });
      if (!this.authUser.roleid)
        this.messageService.add({ severity: 'error', summary: 'Validation', detail: 'select Role' });
    }
  }
}

