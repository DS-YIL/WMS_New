using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models
{
	public class OpenPoModel
	{
		public int rfqsplititemid { get; set; }
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
        public int vendorid { get; set; }
        public int inwardid { get; set; }
        public int paitemid { get; set; }
        public int inwmasterid { get; set; }
		public int poitemid { get; set; }
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
		public int itemid { get; set; }
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
        public int paitemid { get; set; }
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
        public DateTime approveddate { get; set; }
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
        public int gatepassid { get; set; }
        public string gatepasstype { get; set; }
        public string status { get; set; }
        public string referenceno { get; set; }
        public string vehicleno { get; set; }
        public string creatorid { get; set; }
        public DateTime createddate { get; set; }
        public int gatepassmaterialid { get; set; }
        public string    materialid { get; set; }
		public string materialdescription { get; set; }
		public int quantity { get; set; }
		public string vendorname { get; set; }
		public string name { get; set; }
		public bool deleteflag { get; set; }
		public List<materialistModel> materialList { get; set; }
	}
	public class materialistModel
	{
		public string materialid { get; set; }
		public string remarks { get; set; }
		public int quantity { get; set; }
		public int gatepassmaterialid { get; set; }
	}


}
