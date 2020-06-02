import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { PoDetails, BarcodeModel, inwardModel } from 'src/app/Models/WMS.Model';
import { MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-StoreClerk',
  templateUrl: './StoreClerk.component.html'
})
export class StoreClerkComponent implements OnInit {

  constructor(private messageService: MessageService, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public PoDetails: PoDetails;
  public podetailsList: Array<inwardModel> = [];
  public employee: Employee;
  public showDetails; showQtyUpdateDialog: boolean = false;
  public BarcodeModel: BarcodeModel;
  public inwardModel: inwardModel;
  public grnnumber: string = "";

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");

    this.PoDetails = new PoDetails();
    this.inwardModel = new inwardModel();
    this.inwardModel.quality = "0";
    this.inwardModel.receiveddate = new Date();
    this.inwardModel.qcdate = new Date();
  }

  scanBarcode() {
    var barcodeId = 3;
    var pono = "228738234";
    this.PoDetails.pono;
    this.PoDetails.invoiceno;
    this.spinner.show();
    this.wmsService.Getthreewaymatchingdetails( this.PoDetails.pono).subscribe(data => {
      this.spinner.hide();
      if (data) {
        // this.PoDetails = data[0];
        this.podetailsList = data;
        this.grnnumber = this.podetailsList[0].grnnumber;
        this.showDetails = true;
      }
      else
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'No data' });
    })

  }

  onVerifyDetails(details: any) {
    this.spinner.show();
    this.PoDetails = details;
    this.inwardModel.grndate = new Date();
    this.wmsService.verifythreewaymatch(this.PoDetails.invoiceno, this.PoDetails.pono, this.PoDetails.quotationqty, this.PoDetails.projectcode, this.PoDetails.material).subscribe(data => {
      //this.wmsService.verifythreewaymatch("123", "228738234", "1", "SK19VASP8781").subscribe(data => {
      this.spinner.hide();
      if (data == true) {
        this.showQtyUpdateDialog = true;
      }
      else
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Verification failed' });
    })
  }

  quanityChange() {
    this.inwardModel.pendingqty = parseInt(this.PoDetails.quotationqty) - this.inwardModel.confirmqty;
  }
  onsubmitGRN() {
    //if (this.inwardModel.quality != '0') {
    this.spinner.show();
    this.inwardModel.pono = this.PoDetails.pono;
    this.inwardModel.receivedqty = this.PoDetails.quotationqty;
    this.inwardModel.receivedby = this.inwardModel.qcby = this.employee.employeeno;
    this.podetailsList.forEach(item => {   
      item.receivedby = this.employee.employeeno;
    });
    this.wmsService.insertitems(this.podetailsList).subscribe(data => {
        this.spinner.hide();
        this.grnnumber = data;
        //  if (data) {
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Saved Sucessfully' });
        this.showQtyUpdateDialog = false;
        //}
      });
    //}
    //else
    //  this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'select quality type' });
  }


}
