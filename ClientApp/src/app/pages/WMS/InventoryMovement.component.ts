import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-inventory',
  templateUrl: './InventoryMovement.component.html'
})
export class InventoryMovementComponent implements OnInit {
  constructor(private formBuilder: FormBuilder, private messageService: MessageService, private wmsService: wmsService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public inventoryList: Array<any> = [];
  public employee: Employee;


  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");
    this.getinventoryList();

  }
  getinventoryList() {
    this.wmsService.getInventoryList().subscribe(data => {
      this.inventoryList = data;
    });
  }
}
