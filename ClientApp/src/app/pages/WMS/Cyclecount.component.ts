import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee, DynamicSearchResult } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { commonComponent } from '../../WmsCommon/CommonCode';
import { categoryValues, Cyclecountconfig } from '../../Models/WMS.Model';
import { isNullOrUndefined } from 'util';
import { MessageService } from 'primeng/api';
import { getLocaleExtraDayPeriodRules } from '@angular/common';

@Component({
  selector: 'app-Cyclecount',
  templateUrl: './Cyclecount.component.html'
})
export class CyclecountComponent implements OnInit {
  constructor(private wmsService: wmsService, private messageService: MessageService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public employee: Employee;
  name: string = "Ramesh";
  configmodel = new Cyclecountconfig();
  cols: any[];
  exportColumns: any[];
  public CyclecountMaterialList: Array<any> = [];
  public CyclecountPendingMaterialList: Array<any> = [];
  public allCyclecountMaterialList: Array<any> = [];
  isapprover: boolean = true;
  status: string = "Pending";
  showApprovecolumn: boolean = true;
  showsubmitbutton: boolean = true;
 

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");


    if (this.employee.RoleId == "3") {
      this.isapprover = true;
    }
    else {
      this.isapprover = false;
    }

    this.cols = [
      { field: 'Category', header: 'Category' },
      { field: 'materialid', header: 'Material' },
      { field: 'materialdescription', header: 'Material Descr' },
      { field: 'availableqty', header: 'Available Qty' },
      { field: 'physicalqty', header: 'Physical Qty' },
      { field: 'differnce', header: 'Physical Qty' },
      { field: 'status', header: 'status' },
    ];

    this.exportColumns = this.cols.map(col => ({ title: col.header, dataKey: col.field }));
    this.getCyclecountConfig();
    this.getCyclecountPendingMaterialList();
   
  }

  filterbystatus() {
    var st = this.status;
    if (st == "Approved" || st == "Rejected") {
      this.showApprovecolumn = false;
      this.showsubmitbutton = false;
    }
    else {
      this.showApprovecolumn = true;
      this.showsubmitbutton = true;
    }
    var countlist = this.allCyclecountMaterialList.filter(function (element, index) {
      return (element.status == st);
    });
    this.CyclecountPendingMaterialList = countlist;

  }

  setapproval(e: any, data: any) {
    debugger;
    var data1 = e.target.value;
    if (data1 == 1) {
      data.isapprovalprocess = true;
      data.isapproved = true;

    }
    else if (data1 == 0) {
      data.isapprovalprocess = true;
      data.isapproved = false;
    }
    else {
      data.isapprovalprocess = false;
    }  
  }


  calculatedifference(data: any) {
    debugger;
    data.difference = Math.abs(data.physicalqty - data.availableqty);
    if (!isNullOrUndefined(data.physicalqty) && data.physicalqty > 0) {
      data.iscountprocess = true;
      data.status = "Counted";
    }
  }

  submit() {
    debugger;
    var countlist = this.CyclecountMaterialList.filter(function (element, index) {
      return (element.iscountprocess);
    });
    if (countlist.length > 0) {
      var dt = countlist;
      this.spinner.show();
      this.wmsService.updateinsertCyclecount(countlist).subscribe(data => {
        this.getCyclecountMaterialList();
        this.spinner.hide();
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Saved Sucessfully' });
      });

    }
    else {
      this.approve();
    }


  }
  approve() {
    debugger;
    var countlist = this.CyclecountPendingMaterialList.filter(function (element, index) {
      return (element.isapprovalprocess);
    });
    if (countlist.length > 0) {
      this.spinner.show();
      this.wmsService.updateinsertCyclecount(countlist).subscribe(data => {
        this.getCyclecountPendingMaterialList();
        this.spinner.hide();
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Saved Sucessfully' });
      });
    }

  }

  getCyclecountPendingMaterialList() {
    debugger;
    this.CyclecountPendingMaterialList = [];
    this.spinner.show();
    this.wmsService.getCyclecountPendingList().subscribe(data => {
      this.allCyclecountMaterialList = data;
      this.filterbystatus();
      this.spinner.hide();
    });

  }


  getCyclecountMaterialList() {
    debugger;
    var limita = 0;
    var limitb = 0;
    var limitc = 0;
    if (isNullOrUndefined(this.configmodel.frequency) || this.configmodel.frequency == "") {
      return;
    }

    if (isNullOrUndefined(this.configmodel.cyclecount) || this.configmodel.cyclecount == 0) {
      return;
    }
    if (this.configmodel.cyclecount > 0) {
      if (this.configmodel.apercentage > 0) {
        limita = Math.ceil(this.configmodel.cyclecount * this.configmodel.apercentage / 100);
      }
      if (this.configmodel.bpercentage > 0) {
        limitb = Math.ceil(this.configmodel.cyclecount * this.configmodel.bpercentage / 100);
      }
      if (this.configmodel.cpercentage > 0) {
        limitc = Math.ceil(this.configmodel.cyclecount * this.configmodel.cpercentage / 100);
      }
    }
    var persum = this.configmodel.apercentage + this.configmodel.bpercentage + this.configmodel.cpercentage;

    if (persum > 100) {
     
      return;
    }
   
    this.CyclecountMaterialList = [];
    this.spinner.show();
    this.wmsService.getCyclecountList(limita, limitb, limitc).subscribe(data => {
      this.CyclecountMaterialList = data;
      this.spinner.hide();
    });

  }


 
  getCyclecountConfig() {
    this.spinner.show();
    this.CyclecountMaterialList = [];
    this.wmsService.getCyclecountConfig().subscribe(data => {
      this.configmodel = data;
      this.getCyclecountMaterialList();
      this.spinner.hide();
    });

  }
 

}

