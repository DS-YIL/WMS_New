import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee, DynamicSearchResult, searchList } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { MessageService } from 'primeng/api';
import { gatepassModel, materialistModel } from '../../Models/WMS.Model';

@Component({
  selector: 'app-GatePassApprover',
  templateUrl: './GatePassApproverForm.component.html'
})
export class GatePassApproverComponent implements OnInit {
  constructor(private formBuilder: FormBuilder, private messageService: MessageService, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }


  public employee: Employee;
  public materialList: Array<any> = [];
  public gatepassModel: gatepassModel;
  public btnDisable: boolean= false;

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");

    this.route.params.subscribe(params => {
      if (params["gatepassid"]) {
        var gatepassId = params["gatepassid"]
        this.bindMaterilaDetails(gatepassId);
      }
    });
    this.gatepassModel = new gatepassModel();
    this.gatepassModel.approverstatus="Approved"

  }

 
  //get gatepass list
  bindMaterilaDetails(gatepassId:any) {
    this.wmsService.gatepassmaterialdetail(gatepassId).subscribe(data => {
      this.materialList = data;
      this.gatepassModel = this.materialList[0];
      if (this.gatepassModel.approverstatus == 'Approved')
        this.btnDisable = true;
    });
  }

  updategatepassapproverstatus() {
    this.gatepassModel.gatepassid = this.materialList[0].gatepassid;
    this.wmsService.updategatepassapproverstatus(this.gatepassModel).subscribe(data => {
      //this.materialList = data;
      this.messageService.add({ severity: 'success', summary: 'success Message', detail: 'Status updated' });
    });
  }
}
