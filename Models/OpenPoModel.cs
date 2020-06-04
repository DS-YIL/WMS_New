using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace WMS.Models
{
	public class OpenPoModel
	{
		public int rfqsplititemid { get; set; }
		public string departmentname { get; set; }
		public int departmentid { get; set; }
		public string documentno { get; set; }
		public int paid { get; set; }
		public string pono { get; set; }
		public string vendorname { get; set; }
		public string JobName { get; set; }
		public string SaleOrderNo { get; set; }
		public float quotationqty { get; set; }
		public string invoiceno { get; set; }
		public string projectcode { get; set; }
		public int paitemid { get; set; }
		public string Material { get; set; }
		public string Materialdescription { get; set; }
		public string status { get; set; }
		public int returnqty { get; set; }
        public int vendorid { get; set; }
    }
	public class BarcodeModel
	{
		public int departmentid { get; set; }
		public int barcodeid { get; set; }
		public int paitemid { get; set; }
		public string barcode { get; set; }
		public DateTime createddate { get; set; }
		public string createdby { get; set; }
		public bool deleteflag { get; set; }
		public int inwmasterid { get; set; }
		public string pono { get; set; }
		public string invoiceno { get; set; }
		public DateTime invoicedate { get; set; }
		public string receivedby { get; set; }
		public DateTime receiveddate { get; set; }
		public string Material { get; set; }
		public string Materialdescription { get; set; }

	}
	public class iwardmasterModel
	{
		public int inwmasterid { get; set; }
		public string pono { get; set; }
		public string invoiceno { get; set; }
		public DateTime invoicedate { get; set; }
		public string receivedby { get; set; }
		public DateTime receiveddate { get; set; }
		public int barcodeid { get; set; }
		public bool deleteflag { get; set; }
        public string grnnumber { get; set; }
	}
	public class inwardModel
	{
		public int itemid { get; set; }

		public int vendorid { get; set; }
        public int inwardid { get; set; }
        public string projectname { get; set; }
        public int inwmasterid { get; set; }
		public string quotationqty { get; set; }
		public int receivedqty { get; set; }
		public DateTime receiveddate { get; set; }
		public string receivedby { get; set; }
		public int returnqty { get; set; }
		public int confirmqty { get; set; }
		public bool deleteflag { get; set; }
		public string pono { get; set; }
		public string quality { get; set; }
		public string qtype { get; set; }
		public DateTime qcdate { get; set; }
		public string qcby { get; set; }
		public string remarks { get; set; }
		public string Material { get; set; }
        public string grnnumber { get; set; }
        public string Materialdescription { get; set; }
        public string itemlocation { get; set; }
    }
	public class StockModel
	{
		public string stockstatus { get; set; }
		public int itemid { get; set; }
		public string grnnumber { get; set; }
		public int inwmasterid { get; set; }
		public int paitemid { get; set; }
		public string pono { get; set; }
		public int binid { get; set; }
		public int rackid { get; set; }
		public int vendorid { get; set; }
		public int totalquantity { get; set; }
		public DateTime shelflife { get; set; }
		public int availableqty { get; set; }
		public bool deleteflag { get; set; }
		public DateTime itemreceivedfrom { get; set; }
		public string itemlocation { get; set; }
		public DateTime createddate { get; set; }
		public string createdby { get; set; }
        public string binnumber { get; set; }
        public string racknumber { get; set; }

    }
	public class trackstatusModel
	{
		public int trackid { get; set; }
		public int paitemid { get; set; }
		public string pono { get; set; }
		public string status { get; set; }
	}
	public class DynamicSearchResult
	{
		public string tableName { get; set; }
		public string searchCondition { get; set; }
		public  string query { get; set; }

	}
   
    public class IssueRequestModel
    {
        public int requestforissueid { get; set; }
        public int itemid { get; set; }
		public Boolean itemreturnable { get;set; }

		public int requestid { get; set; }
        public int inwardid { get; set; }
        public int quantity { get; set; }
        public int quotationqty { get; set; }
        public int requestedquantity { get; set; }
        public int issuedquantity { get; set; }
        public string pono { get; set; }
        public DateTime requesteddate { get; set; }
        public string approveremailid { get; set; }
        public string approverid { get; set; }
		public string approvedby { get; set; }
		public string itemreceiverid { get; set; }
		public DateTime approvedon { get; set; }
        public string approvedstatus { get; set; }
        public bool status { get; set; }
        public bool deleteflag { get; set; }
        public string materialid { get; set; }
        public string requesterid { get; set; }
        public string projectname { get; set; }
        public string name { get; set; }
        public string Material { get; set; }
        public string ackremarks { get; set; }
        public string Materialdescription { get; set; }
    }
    public class sequencModel
    {
        public int id { get; set; }
        public string sequencename { get; set; }
        public int sequenceid { get; set; }
        public int year { get; set; }
        public int sequencenumber { get; set; }
    }
    public class gatepassModel
    {
		public string reprintedby { get; set; }

		public int gatepassid { get; set; }
        public string gatepasstype { get; set; }
        public string status { get; set; }
        public string referenceno { get; set; }
        public string vehicleno { get; set; }
        public string requestedby{ get; set; }
        public DateTime requestedon { get; set; }
        public int gatepassmaterialid { get; set; }
        public string    materialid { get; set; }
		public string materialdescription { get; set; }
		public int quantity { get; set; }
		public string vendorname { get; set; }
		public string name { get; set; }
		public bool deleteflag { get; set; }
		public string reasonforgatepass { get; set; }
		public string approverstatus { get; set; }
		
		public string approverremarks { get; set; }
		public Boolean print { get; set; }
		public int reprintcount { get; set; }
		public DateTime approvedon { get; set; }
		public List<materialistModel> materialList { get; set; }
		public string printedby { get; set; }
		public DateTime printedon { get; set; }
		public string remarks { get; set; }
		
		public int materialcost { get; set; }
		public DateTime expecteddate { get; set; }
		public DateTime returneddate { get; set; }
	}
	public class materialistModel
	{
		public string materialid { get; set; }
		public string remarks { get; set; }
		public int quantity { get; set; }
		public int gatepassmaterialid { get; set; }
		public int materialcost { get; set; }
		public DateTime expecteddate { get; set; }
		public DateTime returneddate { get; set; }
		
	}

		public class reprintModel
		{
		public int reprinthistoryid { get; set; }
		public int? gatepassid { get; set; }
		public int? inwmasterid { get; set; }
		public DateTime reprintedon { get; set; }
		public string reprintedby { get; set; }
		public int reprintcount { get; set; }
	}
	public class ReportModel
	{
		public int itemid { get; set; }
		public string materialid { get; set; }
		public string materialdescription { get; set; }
		public string departmentname { get; set; }
		public string itemlocation { get; set; }
		public DateTime receiveddate { get; set; }
		public int receivedqty { get; set; }
		public int issuedqty { get; set; }
		public int availableqty { get; set; }
		public double unitprice { get; set; }
		public int daysinstock { get; set; }
		public DateTime reportdate { get; set; }
		public string projectname { get; set; }
		public string vendorname { get; set; }
		public string ponumber { get; set; }
		public string category { get; set; }
		public int totalcost { get; set; }
	}
	public class ABCCategoryModel
	{
		public int categoryid { get; set; }
		public string categoryname { get; set; }
		public int? minpricevalue { get; set; }
		public int? maxpricevalue { get; set; }
		public string createdby { get; set; }
		public DateTime createdon { get; set; }
		public string updatedby { get; set; }
		public DateTime updatedon { get; set; }
		public Boolean deleteflag { get; set; }
		public DateTime enddate { get; set; }
		public DateTime startdate { get; set; }
	}
	public class FIFOModel
	{
		public string materialid { get; set; }
		public string Materialdescription { get; set; }
		public string itemlocation { get; set; }
		public DateTime shelflife { get; set; }
		public DateTime createddate { get; set; }
	}
}
