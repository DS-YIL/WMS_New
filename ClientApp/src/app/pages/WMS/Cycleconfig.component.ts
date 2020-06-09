import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { wmsService } from '../../WmsServices/wms.service';
import { constants } from '../../Models/WMSConstants';
import { Employee, DynamicSearchResult } from '../../Models/Common.Model';
import { NgxSpinnerService } from "ngx-spinner";
import { commonComponent } from '../../WmsCommon/CommonCode';
import { categoryValues, Cyclecountconfig, daylist } from '../../Models/WMS.Model';
import { isNullOrUndefined } from 'util';
import { MessageService } from 'primeng/api';
import { getLocaleExtraDayPeriodRules } from '@angular/common';
import { FormGroup, FormBuilder, FormArray, Validators } from "@angular/forms";
import { DatePipe } from "@angular/common";

@Component({
  selector: 'app-Cycleconfig',
  templateUrl: './Cycleconfig.component.html'
})
export class CycleconfigComponent implements OnInit {
  constructor(private fb: FormBuilder,private wmsService: wmsService, private messageService: MessageService, private route: ActivatedRoute, private router: Router, public constants: constants, private spinner: NgxSpinnerService) { }

  public employee: Employee;
  name: string = "Ramesh";
  configmodel = new Cyclecountconfig();
  showweekly: boolean = false;
  showbiweekly: boolean = false;
  showmonthly: boolean = false;
  showquarterly: boolean = false;
  showyearly: boolean = false;
  public yearlynotifdate: Date;
  DayList: daylist[] = [];
  cols: any[];
  exportColumns: any[];
  weekday1: string = "";
  weekday2: string = "";
  weekday3: string = "";
 

  ngOnInit() {
    if (localStorage.getItem("Employee"))
      this.employee = JSON.parse(localStorage.getItem("Employee"));
    else
      this.router.navigateByUrl("Login");
    this.getCyclecountConfig();
    this.cols = [
      { field: 'description', header: 'description' },
      { field: 'showdate', header: 'Date' },
    ];

    this.exportColumns = this.cols.map(col => ({ title: col.header, dataKey: col.field }));
   
   
  }

  setnotifydate() {
    this.showweekly = false;
    this.showbiweekly = false;
    this.showmonthly = false;
    this.showquarterly = false;
    this.showyearly = false;
    var freq = this.configmodel.frequency;
    if (freq == "Weekly") {
      this.showweekly = true;
    }
    else if (freq == "Twice in a week") {
      this.showbiweekly = true;
    }
    else if (freq == "Monthly") {
      this.showmonthly = true;
    }
    else if (freq == "Quarterly") {
      this.showquarterly = true;

    }
    else if (freq == "Yearly") {
      this.showyearly = true;

    }

  }


  updateCycleCountConfig() {
    var persum = this.configmodel.apercentage + this.configmodel.bpercentage + this.configmodel.cpercentage;

    if (persum > 100) {
      this.messageService.add({ severity: 'error', summary: 'Validation Message', detail: 'Sum of percentage can not exceed 100' });
      return;
    }
    if (isNullOrUndefined(this.configmodel.frequency) || this.configmodel.frequency == "") {
      this.messageService.add({ severity: 'error', summary: 'Validation Message', detail: 'Please select frequency' });
      return;
    }
    if (isNullOrUndefined(this.configmodel.cyclecount) || this.configmodel.cyclecount < 1) {
      this.messageService.add({ severity: 'error', summary: 'Validation Message', detail: 'Please provide cycle count' });
      return;
    }
    if (this.configmodel.frequency == "Weekly" || this.configmodel.frequency == "Twice in a week") {
      this.configmodel.notificationtype = "Day";
      if (this.configmodel.frequency == "Weekly") {
        var data = {
          "description": "Weekly",
          "showday": this.weekday1
        }
        this.configmodel.notificationon.push(data);

      }
      else {
        var data = {
          "description": "Weekly",
          "showday": this.weekday2
        }
        var data1 = {
          "description": "Weekly",
          "showday": this.weekday3
        }
        this.configmodel.notificationon.push(data);
        this.configmodel.notificationon.push(data1);

      }
    }
    else if (this.configmodel.frequency == "Monthly" || this.configmodel.frequency == "Quarterly" || this.configmodel.frequency == "Yearly") {
      this.configmodel.notificationtype = "Date";
      this.DayList.forEach(element => {
        this.configmodel.notificationon.push(element);

      });

    }
    else {
      this.configmodel.notificationtype = "Daily";
      this.configmodel.notificationon = null;

    }
    this.spinner.show();
    this.wmsService.updateCyclecountconfig(this.configmodel).subscribe(data => {
      console.log(data);
      this.messageService.add({ severity: 'success', summary: 'success Message', detail: 'Configuration updated' });
      this.getCyclecountConfig();
      this.spinner.hide();
    });
     
  }

 
  



  setCycleCount() {
    this.showweekly= false;
    this.showbiweekly= false;
    this.showmonthly= false;
    this.showquarterly = false;
    this.showyearly= false;
    var stdate = this.configmodel.startdate;
    var edate = this.configmodel.enddate;
    var freq = this.configmodel.frequency;
    var count = this.configmodel.countall;
    
    if (isNullOrUndefined(this.configmodel.frequency) || this.configmodel.frequency == "") {
      return;
    }
    if (!isNullOrUndefined(stdate) && !isNullOrUndefined(edate)) {
      debugger;
      if (freq == "Daily") {
        var date2 = new Date(edate);
        var date1 = new Date(stdate);

        // To calculate the time difference of two dates 
        var Difference_In_Time = date2.getTime() - date1.getTime();

        // To calculate the no. of days between two dates 
        var Difference_In_Days = Difference_In_Time / (1000 * 3600 * 24);
        if (count > 0) {
          var cyclecount = Math.ceil(count / Difference_In_Days);
          this.configmodel.cyclecount = cyclecount;
        }
        else {
          this.configmodel.cyclecount = 0;
        }

      }
      else if (freq == "Weekly") {
        this.showweekly = true;
        var date2 = new Date(edate);
        var date1 = new Date(stdate);
        var diff = (date2.getTime() - date1.getTime()) / 1000;
        diff /= (60 * 60 * 24 * 7);
        var weeks = Math.abs(Math.round(diff));
        if (count > 0) {
          var cyclecount = Math.ceil(count / weeks);
          this.configmodel.cyclecount = cyclecount;
        }
        else {
          this.configmodel.cyclecount = 0;
        }

      }
      else if (freq == "Twice in a week") {
        this.showbiweekly = true;
        var date2 = new Date(edate);
        var date1 = new Date(stdate);
        var diff = (date2.getTime() - date1.getTime()) / 1000;
        diff /= (60 * 60 * 24 * 7);
        var weeks = Math.abs(Math.round(diff));
        if (count > 0) {
          var cyclecount = Math.ceil(count / weeks);
          this.configmodel.cyclecount = Math.ceil(cyclecount / 2);
        }
        else {
          this.configmodel.cyclecount = 0;
        }

      }
      else if (freq == "Monthly") {
        this.showmonthly = true;
        var date2 = new Date(edate);
        var date1 = new Date(stdate);
        this.setDaylist(date1, date2, freq);
        var months = 0;
        months = (date2.getFullYear() - date1.getFullYear()) * 12;
        months -= date1.getMonth();
        months += date2.getMonth();
        var monthcount = months <= 0 ? 0 : months;
        if (count > 0) {
          var cyclecount = Math.ceil(count / monthcount);
          this.configmodel.cyclecount = cyclecount;
        }
        else {
          this.configmodel.cyclecount = 0;
        }

      }
      else if (freq == "Quarterly") {
        this.showquarterly = true;
        var date2 = new Date(edate);
        var date1 = new Date(stdate);
        this.setDaylist(date1, date2, freq);
        var months = 0;
        months = (date2.getFullYear() - date1.getFullYear()) * 12;
        months -= date1.getMonth();
        months += date2.getMonth();
        var monthcount = months <= 0 ? 0 : months;
        if (count > 0) {
          var cyclecount = Math.ceil(count / monthcount);
          this.configmodel.cyclecount = Math.ceil(cyclecount / 4);
        }
        else {
          this.configmodel.cyclecount = 0;
        }

      }
      else if (freq == "Yearly") {
        this.showyearly = true;
        if (count > 0) {
          this.configmodel.cyclecount = count;
        }
        else {
          this.configmodel.cyclecount = 0;
        }
      }
    }
  }

  setDaylist(stdate: Date, edate: Date, freq: string) {
    this.DayList = [];
    debugger;
    var months = ["January","February","March","April","May","June","July","August","September","October","November","December"]
    if (freq == "Monthly") {
      var date2year = edate.getFullYear();
      var date1year = stdate.getFullYear();
      var startmonth = stdate.getMonth();
      var endmonth = edate.getMonth();
      var loopend = endmonth;
      if (endmonth < startmonth) {
        loopend = 11 + endmonth;
      }
      for (var i = startmonth; i <= loopend; i++) {
        let day = new daylist();
        var year = date1year;
        var month = months[i];
        if (i > 11) {
          year = date2year;
          month = months[i-12]
        }
        day.description = month + "(" + year + ")";
        this.DayList.push(day);
      }

     

    }
    if (freq == "Quarterly") {
      var startmonth = stdate.getMonth();
      var endmonth = edate.getMonth();
      var loopend = endmonth;
      if (endmonth < startmonth) {
        loopend = 11 + endmonth;
      }
      var quarters = Math.ceil(loopend / 4);
      for (var i = 1; i <= quarters; i++) {
        let day = new daylist();
        day.description = "Quarter" + i;
        this.DayList.push(day);

      }

    }

   

  }

  getCyclecountConfig() {
    this.spinner.show();
    this.wmsService.getCyclecountConfig().subscribe(data => {
      this.configmodel = data;
      this.setnotifydate();
      this.spinner.hide();
    });

  }
 

}

