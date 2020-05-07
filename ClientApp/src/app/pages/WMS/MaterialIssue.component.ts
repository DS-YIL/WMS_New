import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl, ValidatorFn } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee, DynamicSearchResult, searchList } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { materialRequestDetails } from 'src/app/Models/WMS.Model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-MaterialIsuue',
  templateUrl: './MaterialIssue.component.html'
})
export class MaterialIssueComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private messageService: MessageService, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public formName: string;
  public txtName: string;
  public dynamicData = new DynamicSearchResult();
  public showList: boolean = false;
  public searchItems: Array<searchList> = [];
  public selectedlist: Array<searchList> = [];
  public selectedItem: searchList;
  public searchresult: Array<object> = [];

  public MaterialRequestForm: FormGroup
  public materialissueList: Array<any> = [];
  public employee: Employee;
  public displayItemRequestDialog; RequestDetailsSubmitted: boolean = false;
  public materialRequestDetails: materialRequestDetails;
  public requestId: string;

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");

    this.route.params.subscribe(params => {
      if (params["requestid"]) {
        this.requestId = params["requestid"];
      }
    });


    this.getmaterialIssueListbyrequestid();

  }

  getmaterialIssueListbyrequestid() {
    this.wmsService.getmaterialIssueListbyrequestid(this.requestId).subscribe(data => {
      this.materialissueList = data;
      this.materialissueList.forEach(item => {
        if (!item.issuedquantity)
          item.issuedquantity = item.requestedquantity;
      });
    });
  }

  //check validations for issuer quantity
reqQtyChange(data: any) {
  if (data.issuedquantity > data.quantity) {
    this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'issued Quantity should be lessthan or equal to available quantity' });
    data.issuedquantity = data.quantity;
  }
}


  //requested quantity update
  onMaterialIssueDeatilsSubmit() {
    this.spinner.show();
    this.wmsService.approvematerialrequest(this.materialissueList).subscribe(data => {
      this.spinner.hide();
      if (data)
        this.messageService.add({ severity: 'sucess', summary: 'sucee Message', detail: 'Status updated' });
      else
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Update Failed' });

    });

  }
}
