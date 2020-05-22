import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { PoDetails, BarcodeModel } from 'src/app/Models/WMS.Model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-SecurityHome',
  templateUrl: './SecurityHome.component.html'
})
export class SecurityHomeComponent implements OnInit {

  constructor(private messageService: MessageService, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public PoDetails: PoDetails;
  public employee: Employee;
  public showDetails; disSaveBtn: boolean = false;
  public BarcodeModel: BarcodeModel;

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");

    this.PoDetails = new PoDetails();
  }


  SearchPoNo() {
    if (this.PoDetails.pono) {
      this.spinner.show();
      this.wmsService.getPoDetails(this.PoDetails.pono).subscribe(data => {
        this.spinner.hide();
        if (data) {
          this.PoDetails = data;
          this.showDetails = true;
        }
        else
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'No data for this PoNo' });
      })
    }
    else
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Enter PoNo' });
  }
  onsaveSecDetails() {
    //need to generate barcode
    if (this.PoDetails.invoiceno) {
      this.spinner.show();
      this.BarcodeModel = new BarcodeModel();
      this.BarcodeModel.paitemid =1;;
      this.BarcodeModel.barcode = "testbarcodetext";
      this.BarcodeModel.createdby = this.employee.employeeno;
      this.BarcodeModel.pono = this.PoDetails.pono;
      this.BarcodeModel.invoiceno = this.PoDetails.invoiceno;
      this.BarcodeModel.departmentid = this.PoDetails.departmentid;
      this.BarcodeModel.receivedby = this.employee.employeeno;
      this.wmsService.insertbarcodeandinvoiceinfo(this.BarcodeModel).subscribe(data => {
        if (data)
          this.disSaveBtn = true;
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Saved Sucessfully' });
        this.spinner.hide();
      });
    }
    else
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Invoice number mandatory' });
  }
}
