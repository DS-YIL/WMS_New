import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee, DynamicSearchResult } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { commonComponent } from '../../WmsCommon/CommonCode';
import { categoryValues } from '../../Models/WMS.Model';

@Component({
  selector: 'app-ABCAnalysis',
  templateUrl: './ABCAnalysis.component.html'
})
export class ABCAnalysisComponent implements OnInit {
  constructor(private wmsService: wmsService, private commonComponent: commonComponent, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public employee: Employee;
  public dynamicData: DynamicSearchResult;

  public ABCAnalysisList: Array<any> = [];
  public category: string;
  public showAbcAnalysisList: boolean = true;
  cols: any[];
  exportColumns: any[];

  public ABCAnalysisMateDet: Array<any> = [];
  public matDetails: any;
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
    this.classA.minpricevalue = '20000';
    this.classA.maxpricevalue = '100000';
    this.classB.maxpricevalue = '20000';
    this.classB.minpricevalue = '10000';
    this.classC.maxpricevalue = '10000';
    this.classC.minpricevalue = '0';
    this.classB.categoryname = "B";
    this.classC.categoryname = "C";
    this.classA.createdby = this.classB.createdby = this.classC.createdby = this.classA.updatedby = this.classB.updatedby = this.classC.updatedby = this.employee.employeeno;

    this.cols = [
      { field: 'Category', header: 'Category' },
      { field: 'materialid', header: 'Material' },
      { field: 'materialdescription', header: 'Material Descr' },
      { field: 'availableqty', header: 'Available Qty' },
       { field: 'unitprice', header: 'Unit Price' }
    ];

    this.exportColumns = this.cols.map(col => ({ title: col.header, dataKey: col.field }));

  }

  getABCAnalysisList() {
    this.spinner.show();
    this.catList = [];
    this.catList.push(this.classA);
    this.catList.push(this.classB);
    this.catList.push(this.classC);
    this.ABCAnalysisList = [];
    this.wmsService.updateABCRange(this.catList).subscribe(data => {
      this.ABCAnalysisList = data;
      this.spinner.hide();
    });
  }

  getCategory(details: any) {
    if (details.unitprice >= this.classC.minpricevalue && details.unitprice <= this.classC.maxpricevalue)
      return 'C';
    else if (details.unitprice >= this.classB.minpricevalue && details.unitprice <= this.classB.maxpricevalue)
      return 'B';
    else if (details.unitprice >= this.classA.minpricevalue && details.unitprice <= this.classA.maxpricevalue)
      return 'A';
  }
  showList() {
    this.showAbcAnalysisList = true;
  }
  showMatdetails(details: any) {
    this.showAbcAnalysisList = false;
    this.spinner.show();
    this.matDetails = details;
    this.ABCAnalysisMateDet = [];
    this.dynamicData = new DynamicSearchResult();
    this.dynamicData.query = "select itemid, sec.grnnumber , createddate as receiveddate, totalquantity,availableqty,totalquantity - availableqty AS issuedqty,itemlocation from wms.wms_stock ws inner join wms.wms_securityinward sec on sec.pono = ws.pono  where ws.materialid = '" + details.materialid + "'";
    this.wmsService.GetListItems(this.dynamicData).subscribe(data => {
      this.ABCAnalysisMateDet = data;
      this.spinner.hide();
    });
  }
  calculateTotalQty() {
    var qty = 0;
    this.ABCAnalysisMateDet.forEach(item => {
      if (item.availableqty)
        qty += item.availableqty;
    })
    return qty;
  }

}

