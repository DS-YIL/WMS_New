using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Models;
namespace WMS.Interfaces
{
   public interface IPodataService<T>
    {
        Task<IEnumerable<T>> getOpenPoList(string loginid,string pono = null, string docno = null, string vendorid = null);
        OpenPoModel CheckPoexists(string PONO);
        int InsertBarcodeInfo(BarcodeModel dataobj);
        //int insertInvoicedetails(iwardmasterModel obj);
        Task<IEnumerable<T>> GetDeatilsForthreeWaymatching(string pono);
        bool VerifythreeWay(string pono,string invoiceno,int quantity,string projectcode, string material);
        Task<string> insertquantity(List<inwardModel> datamodel);
       string InsertStock(StockModel data);
		System.Data.DataTable GetListItems(DynamicSearchResult result);
        int IssueRequest(List<IssueRequestModel> reqdata);
        Task<IEnumerable<inwardModel>> getitemdeatils(string grnnumber);
        Task<IEnumerable<IssueRequestModel>> MaterialRequest(string pono,string material);
        int acknowledgeMaterialReceived(List<IssueRequestModel> dataobj);
        Task<IEnumerable<IssueRequestModel>> GetMaterialissueList(string requesterid);
        Task<IEnumerable<IssueRequestModel>> GetMaterialissueListforapprover(string approverid);
        Task<IEnumerable<IssueRequestModel>> GetmaterialdetailsByrequestid(string requestid);
        Task<IEnumerable<IssueRequestModel>> GetPonodetails(string pono);
        int updaterequestedqty(List<IssueRequestModel> dataobj);
        int ApproveMaterialissue(List<IssueRequestModel> dataobj);
        Task<IEnumerable<gatepassModel>> GetgatepassList();
        int SaveOrUpdateGatepassDetails(gatepassModel dataobj);
        string checkmaterialandqty(string material=null,int qty=0);
        int deletegatepassmaterial(int gatepassmaterialid);
        int updategatepassapproverstatus(gatepassModel model);
        Task<IEnumerable<gatepassModel>> GetmaterialList(int gatepassid);
        int updateprintstatus(gatepassModel model);
    }
}
