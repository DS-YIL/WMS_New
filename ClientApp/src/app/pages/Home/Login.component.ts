import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { first } from 'rxjs/operators';
import { constants } from '../../Models/WMSConstants';
import { Employee, Login } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { MENU_ITEMS } from '../pages-menu';

@Component({
  selector: 'app-Login',
  templateUrl: './Login.component.html'
})
export class LoginComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public LoginForm: FormGroup;
  public employee: Employee;
  public LoginModel: Login;
  public LoginSubmitted: boolean = false;
  public dataSaved: boolean = false;

  ngOnInit() {
    this.LoginModel = new Login();
    this.LoginModel.RoleId = "0";
    this.LoginForm = this.formBuilder.group({
      DomainId: ['', [Validators.required]],
      Password: ['', [Validators.required]],
      RoleId: ['', [Validators.required]],
    });
    this.constants.animateCSS('login', 'zoomInDown');
  }


  //Login
  Login() {

    this.LoginSubmitted = true;
    if (this.LoginForm.invalid || this.LoginModel.RoleId == '0') {
      return;
    }
    else {
      this.spinner.show();
      this.wmsService.ValidateLoginCredentials(this.LoginForm.value.DomainId, this.LoginForm.value.Password)
        .pipe(first())
        .subscribe(data1 => {
          this.spinner.hide();
          if (data1.employeeno != null) {
            this.employee = data1;
            this.employee.RoleId = this.LoginForm.value.RoleId;
            localStorage.setItem('Employee', JSON.stringify(this.employee));
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
        });
    }
  }


}
