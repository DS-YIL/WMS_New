using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WMS.Interfaces;
using WMS.Models;
namespace WMS.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PODataController : ControllerBase
    {
        private IPodataService<OpenPoModel> _poService;

        public PODataController(IPodataService<OpenPoModel> poService)
        {
            _poService = poService;
        }

        [HttpGet("GetOpenPoList")]
        public async Task<IEnumerable<OpenPoModel>> GetPoNodata( string loginid,string pono = null, string docno = null, string vendorid = null)
        {
            return await this._poService.getOpenPoList(loginid,pono, docno, vendorid);
        }
        [HttpGet("CheckPoNoexists")]
        public  OpenPoModel CheckPo(string PONO)
        {
            return  this._poService.CheckPoexists(PONO);
        }
        [HttpPost("insertbarcodeandinvoiceinfo")]
        public int insertbardata(BarcodeModel data)
        {
            return this._poService.InsertBarcodeInfo(data);
        }
        //[HttpPost("insertinvoiceinfo")]
        //public int insertinvoicedata(iwardmasterModel data)
        //{
        //    return this._poService.insertInvoicedetails(data);
        //}
        //need list of items
        [HttpGet("Getthreewaymatchingdetails")]
        public async Task<IEnumerable<OpenPoModel>> Getdetailsforthreewaymatching(string pono)
        {
            return await this._poService.GetDeatilsForthreeWaymatching(pono);
        }
        [HttpGet("verifythreewaymatch")]
        public bool verifythreewaymatching(string pono, string invoiceno, int quantity, string projectcode,string material)
        {
            return  this._poService.VerifythreeWay(pono, invoiceno,quantity,projectcode, material);
        }
        [HttpPost("insertitems")]
        public async Task<string> insertitemdata([FromBody] List<inwardModel> data)
        {
            return await this._poService.insertquantity(data);
        }

        [HttpPost("insertitemsToStock")]
        public string insertitemdataTostock(StockModel data)
        {
            return  this._poService.InsertStock(data);
        }

		[HttpPost("GetListItems")]
		public IActionResult GetListItems([FromBody] DynamicSearchResult Result)
		{
			return Ok(this._poService.GetListItems(Result));
		}
        //not using
        [HttpPost("issuerequest")]
        public IActionResult issuerequest([FromBody] List<IssueRequestModel> model)
        {
            return Ok(this._poService.IssueRequest(model));
        }
        //not usinggetMaterialRequestlist
        //list of items
        [HttpGet("getitemdetailsbygrnno")]
        public async Task<IEnumerable<inwardModel>> getitemdetailsbygrnno(string grnnumber)
        {
            return await this._poService.getitemdeatils(grnnumber);
        }
        [HttpGet("getmaterialrequestList")]
        public async Task<IEnumerable<IssueRequestModel>> materialissue(string pono=null,string loginid=null)
        {
            return await this._poService.MaterialRequest(pono, loginid);
        }
        [HttpPost("ackmaterialreceived")]
        public int ackmaterial([FromBody] List<IssueRequestModel> data)
        {
            return this._poService.acknowledgeMaterialReceived(data);
        }
        [HttpGet("getmaterialIssueListbyreqid")]
        public async Task<IEnumerable<IssueRequestModel>> getmaterial(string requesterid)
        {
            return  await this._poService.GetMaterialissueList(requesterid);
        }
        [HttpGet("getmaterialIssueListbyapproverid")]
        public async Task<IEnumerable<IssueRequestModel>> getmaterialrequestbyapproverid(string approverid)
        {
            return await this._poService.GetMaterialissueListforapprover(approverid);
        }
        [HttpGet("getmaterialIssueListbyrequestid")]
        public async Task<IEnumerable<IssueRequestModel>> getmaterialrequestbyrequestid(string requestid)
        {
            return await this._poService.GetmaterialdetailsByrequestid(requestid);
        }

        [HttpGet("getponodetailsBypono")]
        public async Task<IEnumerable<IssueRequestModel>> getponodetails(string pono)
        {
            return await this._poService.GetPonodetails(pono);
        }
        [HttpPost("updaterequestedqty")]
        public int updaterequestedqty([FromBody] List<IssueRequestModel> dataobj)
        {
            return this._poService.updaterequestedqty(dataobj);
        }
        [HttpPost("approvematerialrequest")]
        public int approvematerial([FromBody] List<IssueRequestModel> data)
        {
            return this._poService.ApproveMaterialissue(data);

        }
        [HttpGet("getgatepasslist")]
        public async Task<IEnumerable<gatepassModel>> getgatepasslist()
        {
            return await this._poService.GetgatepassList();
        }

        [HttpPost("saveoreditgatepassmaterial")]
        public int saveorupdate([FromBody] gatepassModel obj)
        {
            return  this._poService.SaveOrUpdateGatepassDetails(obj);
        }
        [HttpGet("checkmaterialandqty")]
        public string check(string material=null,int qty=0)
        {
            return  this._poService.checkmaterialandqty(material,qty);
        }
        [HttpDelete("deletegatepassmaterial")]
        public int deletematerial(int gatepassmaterialid)
        {
            return this._poService.deletegatepassmaterial(gatepassmaterialid);
        }
        [HttpPost("updategatepassapproverstatus")]
        public int gatepassapproverstatus(gatepassModel model)
        {
            return this._poService.updategatepassapproverstatus(model);
        }
        [HttpGet("getmaterialdetailsbygatepassid")]
        public async Task<IEnumerable<gatepassModel>> gatepassmaterialdetail(int gatepassid)
        {
            return await this._poService.GetmaterialList(gatepassid);
        }
        [HttpGet("updateprintstatus")]
        public int updateprintstatus(gatepassModel model)
        {
            return  this._poService.updateprintstatus(model);
        }
    }
}
