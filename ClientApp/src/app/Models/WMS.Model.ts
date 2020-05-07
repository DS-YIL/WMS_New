export class PoFilterParams {
  loginid: string = "";
  PONo: string = "";
  venderid: string = "";
  DocumentNo: string = "";
}

export class PoDetails {
  pono: string = "";
  invoiceno: string
  Barcode: string = "";
  quotationqty: string;
  returnqty: string;
  pendingqty: string;
  vendorname: string;
  vendorid: number;
  paitemid: number;
  ProjectName: string;
  projectcode: string;
  putawayQty: string;
  returnQty: string;
  location: string;
  rackNo: string;
  material: string;
  materialdescription: string;
  grnnumber: string;
}

export class BarcodeModel {
  barcodeid: number;
  paitemid: number;
  barcode: string;
  createddate: Date
  createdby: string
  deleteflag: boolean
  inwmasterid: number;
  pono: string;
  invoiceno: string;
  invoicedate: Date;
  receivedby: string;
}



export class inwardModel {
  inwardid: number;
  inwmasterid: number;
  poitemid: number;
  receivedqty: string;
  receiveddate: Date;
  receivedby: string;
  returnqty: number;
  confirmqty: number;
  pendingqty: number;
  pono: string;
  quality: string;
  qtype: string;
  qcdate: Date;
  qcby: string;
  remarks: string;
  grnnumber: string;
  grndate: Date;
  itemlocation: string;
  invoiceno: string;
}


export class StockModel {
  itemid: number;
  paitemid: number;
  pono: string;
  binid: number;
  rackid: number;
  vendorid: number;
  totalquantity: string;
  shelflife: Date;
  availableqty: number;
  itemreceivedfrom: Date;
  itemlocation: string;
  createddate: Date;
  createdby: string;

}

export class materialRequestDetails {
  ItemId: string;
  quantity: string;
  projectcode: string;
  pono: string = "";
}
