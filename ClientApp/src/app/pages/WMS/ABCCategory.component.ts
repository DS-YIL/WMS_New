import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee, DynamicSearchResult } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { commonComponent } from '../../WmsCommon/CommonCode';
import { categoryValues } from '../../Models/WMS.Model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-ABCCategory',
  templateUrl: './ABCCategory.component.html'
})
export class ABCCategoryComponent implements OnInit {
  constructor(private wmsService: wmsService, private messageService: MessageService,  private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public employee: Employee;
  public dynamicData: DynamicSearchResult;
  public category: string;
  public catList: Array<any> = [];
  public classA: categoryValues;
  public classB: categoryValues;
  public classC: categoryValues;

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");


    this.classA = new categoryValues();
    this.classB = new categoryValues();
    this.classC = new categoryValues();
    this.classA.categoryname = "A";
    this.classB.categoryname = "B";
    this.classC.categoryname = "C";
    this.getcategorydata();
  }
  getcategorydata() {
    this.wmsService.getcategorymasterdata().subscribe(data => {
      this.classA.startdate = new Date(data[0].startdate);
      this.classA.enddate = new Date(data[0].enddate);
      this.classA.minpricevalue = data[0].minpricevalue;
      this.classA.maxpricevalue = data[0].maxpricevalue
      this.classB.maxpricevalue = data[1].maxpricevalue
      this.classB.minpricevalue = data[1].minpricevalue
      this.classC.maxpricevalue = data[2].maxpricevalue
      this.classC.minpricevalue = data[2].minpricevalue
    });
  }

  updateABCRange() {
    this.spinner.show();
    this.catList = [];
    this.catList.push(this.classA);
    this.classB.startdate = this.classC.startdate = this.classA.startdate;
    this.classB.enddate = this.classC.enddate = this.classA.enddate;
    this.catList.push(this.classB);
    this.catList.push(this.classC);
    this.classA.createdby = this.classB.createdby = this.classC.createdby = this.employee.employeeno;
     this.classA.updatedby = this.classB.updatedby = this.classC.updatedby = this.employee.employeeno;
    this.wmsService.updateABCRange(this.catList).subscribe(data => {
      this.spinner.hide();
      this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Saved Sucessfully' });
    });
  }
}

