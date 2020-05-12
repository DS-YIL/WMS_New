import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-GatePass',
  templateUrl: './GatePass.component.html'
})
export class GatePassComponent implements OnInit {
  constructor(private formBuilder: FormBuilder, private messageService: MessageService, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public gatepasslist: Array<any> = [];
  public employee: Employee;
  public gatepassdialog: boolean = false;


  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");
    this.getGatePassList();

  }
  getGatePassList() {
    this.wmsService.getGatePassList().subscribe(data => {
      this.gatepasslist = data;
    });
  }
  openGatepassDialog() {
    this.gatepassdialog = true;
  }
}
