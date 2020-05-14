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
  selector: 'app-GatePass',
  templateUrl: './GatePass.component.html'
})
export class GatePassComponent implements OnInit {
  constructor(private formBuilder: FormBuilder, private messageService: MessageService, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public formName: string;
  public txtName: string;
  public dynamicData = new DynamicSearchResult();
  public showList: boolean = false;
  public searchItems: Array<searchList> = [];
  public selectedlist: Array<searchList> = [];
  public searchresult: Array<object> = [];

  public gatepasslist: Array<any> = [];
  public gatepassModelList: Array<gatepassModel> = [];
  public employee: Employee;
  public gatepassdialog: boolean = false;
  public gatepassModel: gatepassModel;
  public materialistModel: materialistModel;
  public material: any;

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");
    this.gatepassModel = new gatepassModel();
    this.materialistModel = new materialistModel();
    this.getGatePassList();

  }

  public bindSearchListData(event: any, name?: string) {
    var searchTxt = event.query;
    if (searchTxt == undefined)
      searchTxt = "";
    searchTxt = searchTxt.replace('*', '%');
    this.dynamicData = new DynamicSearchResult();
    this.dynamicData.tableName = this.constants[name].tableName;
    this.dynamicData.searchCondition = "" + this.constants[name].condition + this.constants[name].fieldName + " like '" + searchTxt + "%'";
    this.dynamicData.searchCondition += " OR materialid" + " like '" + searchTxt + "%'" + "";

    this.wmsService.GetListItems(this.dynamicData).subscribe(data => {
      this.searchresult = data;
      this.searchItems = [];
      var fName = "";
      this.searchresult.forEach(item => {
        fName = item[this.constants[name].fieldName];
        var value = { listName: name, name: fName, code: item[this.constants[name].fieldId] };
        this.searchItems.push(value);
      });

    });
  }

  getGatePassList() {
    this.wmsService.getGatePassList().subscribe(data => {
      this.gatepasslist = data;
      this.prepareGatepassList();
    });
  }

  prepareGatepassList() {
    this.gatepasslist.forEach(item => {
      var res = this.gatepassModelList.filter(li => li.gatepassid == item.gatepassid);
      if (res.length == 0) {
        item.materialList = this.gatepasslist.filter(li => li.gatepassid == item.gatepassid);
        this.gatepassModelList.push(item);
      }
    });
  }
  openGatepassDialog(gatepassobject: gatepassModel) {
    this.gatepassdialog = true;
    if (gatepassobject) {
      this.gatepassModel = gatepassobject;
    } else {
      this.gatepassModel.gatepasstype = "0";
    }
  }
  addMaterial() {
    this.materialistModel.materialid = this.material.code;
    this.materialistModel.materialdescription = this.material.name;
    if (this.materialistModel.materialid && this.materialistModel.quantity) {
      this.wmsService.checkMaterialandQty(this.material.code, this.materialistModel.quantity).subscribe(data => {
        if (data == "true") {
          this.gatepassModel.materialList.push(this.materialistModel);
          this.materialistModel = new materialistModel();
          this.material = "";
        }
        else
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: data});
      });
    }
    else {
      if (this.materialistModel.materialid)
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'select material' });
      else if (this.materialistModel.quantity)
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Enter Quantity' });
    }

  }

  onSubmitgatepassDetails() {
    if (this.gatepassModel.gatepasstype != "0") {
      this.gatepassModel.creatorid = this.employee.employeeno;
      this.wmsService.saveoreditgatepassmaterial(this.gatepassModel).subscribe(data => {
        if (data)
          this.messageService.add({ severity: 'success', summary: 'success Message', detail: 'Data saved' });
      })
    }
    else
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Select Type' });
  }
}
