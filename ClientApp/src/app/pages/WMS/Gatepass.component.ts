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
  public gatepassdialog; updateReturnedDateDialog: boolean = false;
  public gatepassModel: gatepassModel;
  public materialistModel: materialistModel;
  public material: any;
  public gpIndx: number;

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");
    this.gatepassModel = new gatepassModel();
    this.materialistModel = new materialistModel();
    this.getGatePassList();

  }

  //bind materials based search
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
           if (name == "ItemId") 
          fName = item[this.constants[name].fieldName] + " - " + item[this.constants[name].fieldId];
       
        var value = { listName: name, name: fName, code: item[this.constants[name].fieldId] };
        this.searchItems.push(value);
      });
    });
  }

  //get gatepass list
  getGatePassList() {
    this.wmsService.getGatePassList().subscribe(data => {
      this.gatepasslist = data;
      this.gatepassModelList = [];
      this.prepareGatepassList();
    });
  }

  //prepare list based on gate pass id
  prepareGatepassList() {
    this.gatepasslist.forEach(item => {
      var res = this.gatepassModelList.filter(li => li.gatepassid == item.gatepassid);
      if (res.length == 0) {
        item.materialList = [];
        var result = this.gatepasslist.filter(li => li.gatepassid == item.gatepassid && li.gatepassmaterialid != "0");
        for (var i = 0; i < result.length; i++) {
          var material = new materialistModel();
          material.gatepassmaterialid = result[i].gatepassmaterialid;
          material.materialid = result[i].materialid;
          material.materialdescription = result[i].materialdescription;
          material.quantity = result[i].quantity;
          material.remarks = result[i].remarks;
          material.returneddate = new Date(result[i].returneddate);
          item.materialList.push(material);
        }

        this.gatepassModelList.push(item);
      }
    });
  }

  //open gate pass dialog
  openGatepassDialog(gatepassobject: any, gpIndx: any,dialog) {
    this[dialog] = true;
    this.gatepassModel = new gatepassModel();
    if (gatepassobject) {
      this.gpIndx = gpIndx;
      this.gatepassModel = gatepassobject;
    } else {
      this.gatepassModel.gatepasstype = "0";
      this.gatepassModel.reasonforgatepass = "0";
    }
  }

  //add materials for gate pass
  addMaterial() {
    if (!this.material) {
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Add Material' });
      return false;
    }
    if (this.gatepassModel.materialList.filter(li => li.materialid == this.material.code).length > 0) {
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Material already exist' });
      return false;
    }
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
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: data });
      });
    }
    else {
      if (this.materialistModel.materialid)
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'select material' });
      else if (this.materialistModel.quantity)
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Enter Quantity' });
    }

  }

  //check material and quntity availablity in stock
  checkMaterialandQty(material: any, matIndex: number) {
    if (material.quantity) {
      this.wmsService.checkMaterialandQty(material.code, material.quantity).subscribe(data => {
        if (data == "true") {
        }
        else {
          this.gatepassModelList[this.gpIndx].materialList[matIndex].quantity = 1;
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Quantity Exceeded' });
        }
      });
    }
  }

  //Delete material for gatepass
  removematerial(id: number, matIndex: number) {
    this.wmsService.deleteGatepassmaterial(id).subscribe(data => {
      this.gatepassModelList[this.gpIndx].materialList.splice(matIndex, 1);
      this.messageService.add({ severity: 'success', summary: 'success Message', detail: 'Material Deleted' });
    });

  }

  //update return dated based on role
  updateReturnedDate(gatepassobject: any) {
    this.gatepassModel = new gatepassModel();
    if (gatepassobject) {
      this.gatepassModel = gatepassobject;
      this.onSubmitgatepassDetails();
    }
  }


  //saving gatepass details
  onSubmitgatepassDetails() {
    if (this.gatepassModel.gatepasstype != "0") {
      this.gatepassModel.requestedby = this.employee.employeeno;
      this.wmsService.saveoreditgatepassmaterial(this.gatepassModel).subscribe(data => {
        this.gatepassdialog = false;
        this.updateReturnedDateDialog = false;
        this.getGatePassList();
        if (data)
          this.messageService.add({ severity: 'success', summary: 'success Message', detail: 'Data saved' });
      })
    }
    else
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Select Type' });
  }

  //showing print page
  showprint(gatepassobject: gatepassModel) {
    this.router.navigate(['/WMS/GatePassPrint', gatepassobject.gatepassid]);
  }


}
