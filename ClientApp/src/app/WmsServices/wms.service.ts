import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { constants } from '../Models/WMSConstants'
import { Employee, Login, DynamicSearchResult } from '../Models/Common.Model';
import { PoFilterParams, PoDetails, BarcodeModel, StockModel, materialRequestDetails, inwardModel } from '../Models/WMS.Model';
import { Text } from '@angular/compiler/src/i18n/i18n_ast';

@Injectable({
  providedIn: 'root'
})
export class wmsService {

  public url = this.constants.url;
  public httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
  private currentUserSubject: BehaviorSubject<Employee>;
  public currentUser: Observable<Employee>;
  constructor(private http: HttpClient, private constants: constants) {
    this.currentUserSubject = new BehaviorSubject<Employee>(JSON.parse(localStorage.getItem('Employee')));
    this.currentUser = this.currentUserSubject.asObservable();
  }
  public get currentUserValue(): Employee {
    return this.currentUserSubject.value;
  }



  //Login
  ValidateLoginCredentials(uname: string, pwd: string) {
    var login = new Login();
    login.Username = uname;
    login.Password = pwd;
    return this.http.post<any>(this.url + 'Users/authenticate/', login)
      .pipe(map(data => {
        if (data.employeeno != null) {
          //const object = Object.assign({}, ...data);

          this.currentUserSubject.next(data);
        }
        return data;
      }))
  }

  GetListItems(search: DynamicSearchResult): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post<any>(this.url + 'POData/GetListItems/', search, httpOptions);
  }

  getPoDetails(PoNo: string): Observable<any> {
    return this.http.get<any>(this.url + 'POData/CheckPoNoexists?PONO=' + PoNo + '', this.httpOptions);
  }
  getitemdetailsbygrnno(GrnNo: string): Observable<any> {
    return this.http.get<any>(this.url + 'POData/getitemdetailsbygrnno?grnnumber=' + GrnNo + '', this.httpOptions);
  }

  getPOList(PoFilterParams: PoFilterParams): Observable<any[]> {
    // POData / GetOpenPoList?pono = 2999930 & vendorid=12
    return this.http.get<any[]>(this.url + 'POData/GetOpenPoList?loginid=' + PoFilterParams.loginid + '&pono=' + PoFilterParams.PONo + '&docno=' + PoFilterParams.DocumentNo + '&vendorid=' + PoFilterParams.venderid + '', this.httpOptions);
  }

  insertbarcodeandinvoiceinfo(BarcodeModel: BarcodeModel): Observable<any> {
    return this.http.post<any>(this.url + 'POData/insertbarcodeandinvoiceinfo', BarcodeModel, this.httpOptions);
  }

  Getthreewaymatchingdetails(barcodeId: number, PoNo: string): Observable<inwardModel[]> {
    return this.http.get<inwardModel[]>(this.url + 'POData/Getthreewaymatchingdetails?barcodeid=' + barcodeId + '&PONO=' + PoNo + '', this.httpOptions);
  }

  verifythreewaymatch(invoiceno: string, PoNo: string, quantity: string, projectCode: string, material: string): Observable<any> {
    return this.http.get<any>(this.url + 'POData/verifythreewaymatch?PONO=' + PoNo + '&invoiceno=' + invoiceno + '&quantity=' + quantity + '&projectcode=' + projectCode + '&material=' + material + '', this.httpOptions);
  }

  insertitems(inwardModel: inwardModel[]): Observable<any> {
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }), responseType: 'text' as any };
    return this.http.post<any>(this.url + 'POData/insertitems', inwardModel, httpOptions);
  }

  InsertStock(StockModel: StockModel): Observable<any> {
    return this.http.post<any>(this.url + 'POData/insertitemsToStock', StockModel, this.httpOptions);
  }

  getMaterialRequestlist(loginid: string, pono: string): Observable<any> {
    return this.http.get<any>(this.url + 'POData/getmaterialrequestList?PONO=' + pono + '&loginid=' + loginid + '', this.httpOptions);
  }

  materialRequestUpdate(materialRequestList: any): Observable<any> {
    return this.http.post<any>(this.url + 'POData/updaterequestedqty/', materialRequestList, this.httpOptions);
  }


  getMaterialIssueLlist(loginid: string): Observable<any> {
    return this.http.get<any>(this.url + 'POData/getmaterialIssueListbyapproverid?approverid=' + loginid + '', this.httpOptions);
  }
  getmaterialIssueListbyrequestid(requestid: string): Observable<any> {
    return this.http.get<any>(this.url + 'POData/getmaterialIssueListbyrequestid?requestid=' + requestid + '', this.httpOptions);
  }


  approvematerialrequest(materialIssueList: any): Observable<any> {
    return this.http.post<any>(this.url + 'POData/approvematerialrequest/', materialIssueList, this.httpOptions);
  }

  ackmaterialreceived(materialAckList: any): Observable<any> {
    return this.http.post<any>(this.url + 'POData/ackmaterialreceived/', materialAckList, this.httpOptions);
  }


  logout() {
    //localStorage.removeItem('Employee');
    this.currentUserSubject.next(null);
    //window.location.reload();
  }

}

