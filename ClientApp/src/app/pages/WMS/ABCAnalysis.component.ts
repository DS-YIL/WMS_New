import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee, DynamicSearchResult } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { commonComponent } from '../../WmsCommon/CommonCode';

@Component({
  selector: 'app-ABCAnalysis',
  templateUrl: './ABCAnalysis.component.html'
})
export class ABCAnalysisComponent implements OnInit {
  constructor(private wmsService: wmsService, private commonComponent: commonComponent,  private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public employee: Employee;
  public dynamicData: DynamicSearchResult;

  public ABCAnalysisList: Array<any> = [];
  public category: string;
  public showAbcAnalysisList: boolean = true;
  cols: any[];
  exportColumns: any[];

  public ABCAnalysisMateDet: Array<any> = [];
  public matDetails: any;

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");


    this.category = "All";
    this.getABCAnalysisList();

    this.cols = [
      { field: 'Category', header: 'Category' },
      { field: 'materialid', header: 'Material' },
      { field: 'materialdescription', header: 'Material Descr' },
      { field: 'availableqty', header: 'Available Qty' }
    ];

    this.exportColumns = this.cols.map(col => ({ title: col.header, dataKey: col.field }));

  }

  getABCAnalysisList() {
    this.spinner.show();
    this.ABCAnalysisList = [];
    this.dynamicData = new DynamicSearchResult();
    this.dynamicData.query = "select * from wms.ABCAnalysis";
    if (this.category != 'All')
      this.dynamicData.query = "select * from wms.ABCAnalysis where category='" + this.category+"'";
    this.wmsService.GetListItems(this.dynamicData).subscribe(data => {
      this.ABCAnalysisList = data;
      this.spinner.hide();
    });
  }
  showList() {
    this.showAbcAnalysisList = true;
  }
  showMatdetails(details:any) {
    this.showAbcAnalysisList = false;
    this.spinner.show();
    this.matDetails = details;
    this.ABCAnalysisMateDet = [];
    this.dynamicData = new DynamicSearchResult();
    this.dynamicData.query = "select itemid, sec.grnnumber , createddate as receiveddate, totalquantity,availableqty,totalquantity - availableqty AS issuedqty,itemlocation from wms.wms_stock ws inner join wms.wms_securityinward sec on sec.pono = ws.pono  where ws.materialid = '" + details.material + "'  limit 10";
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

