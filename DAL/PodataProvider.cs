﻿using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WMS.Common;
using WMS.Interfaces;
using WMS.Models;

namespace WMS.DAL
{
	public class PodataProvider : IPodataService<OpenPoModel>
	{
		Configurations config = new Configurations();
		ErrorLogTrace log = new ErrorLogTrace();
		/// <summary>
		/// check pono exists or not 
		/// </summary>
		/// <param name="PONO"></param>
		/// <returns></returns>
		public OpenPoModel CheckPoexists(string PONO)
		{

			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					pgsql.Open();
					string query = WMSResource.checkponoexists.Replace("#pono", PONO);
					return pgsql.QueryFirstOrDefault<OpenPoModel>(
					   query, null, commandType: CommandType.Text);
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "CheckPoexists", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}
			//throw new NotImplementedException();
		}


		/// <summary>
		/// get lst of open pono list
		/// </summary>
		/// <param name="loginid"></param>
		/// <param name="pono"></param>
		/// <param name="docno"></param>
		/// <param name="vendorid"></param>
		/// <returns></returns>
		public async Task<IEnumerable<OpenPoModel>> getOpenPoList(string loginid, string pono = null, string docno = null, string vendorid = null)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.openpolist.Replace("#projectmanager", loginid);
					if (pono != null)
					{
						query = query + " and op.pono='" + pono + "'";
					}
					if (docno != null)
					{
						query = query + " and op.documentno='" + docno + "'";
					}
					if (vendorid != null)
					{
						query = query + " and  op.vendorid=" + vendorid;
					}
					query = query + " group by track.pono,op.pono order by max(track.enteredon) desc ";
					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<OpenPoModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getOpenPoList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}
		}

		public async Task<IEnumerable<POList>> getPOList()
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getpolist;


					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<POList>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getPOList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}
		}

		/// <summary>
		/// inserting barcode info
		/// </summary>
		/// <param name="dataobj"></param>
		/// <returns></returns>
		public int InsertBarcodeInfo(BarcodeModel dataobj)
		{
			try
			{
				using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
				{
					var q1 = "select count(*) from wms.wms_securityinward  where pono ='" + dataobj.pono + "'  and invoiceno ='" + dataobj.invoiceno + "'";
					int count = int.Parse(DB.ExecuteScalar(q1, null).ToString());

					if (count >= 1)
					{
						return 2; //for onvoice already exist
					}
					else
					{
						dataobj.createddate = System.DateTime.Now;
						string insertquery = WMSResource.insertbarcodedata;

						var result = 0;
						//var result = DB.ExecuteScalar(insertquery, new
						//{

						//    dataobj.paitemid,
						//    dataobj.barcode,
						//    dataobj.createddate,
						//    dataobj.createdby,
						//    dataobj.deleteflag,
						//});
						string insertqueryforinvoice = WMSResource.insertinvoicedata;
						//int barcodeid = Convert.ToInt32(result);
						dataobj.receiveddate = System.DateTime.Now;
						//if (barcodeid != 0)
						//{
						dataobj.invoicedate = System.DateTime.Now;
						var results = DB.Execute(insertqueryforinvoice, new
						{

							dataobj.invoicedate,
							dataobj.departmentid,
							dataobj.invoiceno,
							dataobj.receiveddate,
							dataobj.receivedby,
							dataobj.pono,
							dataobj.deleteflag,
							//barcodeid,
						});
						//}
						return (Convert.ToInt32(results));
					}

				}
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "InsertBarcodeInfo", Ex.StackTrace.ToString());
				return 0;
			}

		}

		//Get Invoice details based on PONO.
		public async Task<IEnumerable<InvoiceDetails>> getinvoiveforpo(string PONO)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getInvoiceDetails.Replace("#pono", PONO);
					return await pgsql.QueryAsync<InvoiceDetails>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getinvoiveforpo", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}
		}

		//get Material details based on grn number
		public async Task<IEnumerable<MaterialDetails>> getMaterialDetails(string grnNo)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getMaterialDetails.Replace("#grn", grnNo);// + pono+"'";//li
					return await pgsql.QueryAsync<MaterialDetails>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getMaterialDetails", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}
		}


		//Get location details based on material id
		public async Task<IEnumerable<LocationDetails>> getlocationdetails(string materialid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getLocationDetails.Replace("#materialid", materialid);
					return await pgsql.QueryAsync<LocationDetails>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getlocationdetails", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}
		}


		//Get material requested, acknowledged and issued details
		public async Task<IEnumerable<ReqMatDetails>> getReqMatdetails(string materialid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getMaterialRequestDetails.Replace("#materialid", materialid);
					return await pgsql.QueryAsync<ReqMatDetails>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getlocationdetails", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}
		}

		/// <summary>
		/// get list of info for three way matching
		/// </summary>
		/// <param name="invoiceno"></param>
		/// <param name="pono"></param>
		/// <returns></returns>
		public async Task<IEnumerable<OpenPoModel>> GetDeatilsForthreeWaymatching(string invoiceno, string pono)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					await pgsql.OpenAsync();
					string query = WMSResource.Getdetailsforthreewaymatching.Replace("#pono", pono).Replace("#invoiceno", invoiceno);// + pono+"'";//li
					return await pgsql.QueryAsync<OpenPoModel>(
					   query, null, commandType: CommandType.Text);
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetDeatilsForthreeWaymatching", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}
		}
		/// <summary>
		/// to verify three way match and generate GRN No
		/// </summary>
		/// <param name="pono"></param>
		/// <param name="invoiceno"></param>
		/// <returns></returns>
		public async Task<OpenPoModel> VerifythreeWay(string pono, string invoiceno)
		{
			OpenPoModel verify = new OpenPoModel();
			sequencModel obj = new sequencModel();
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					await pgsql.OpenAsync();
					string lastinsertedgrn = WMSResource.lastinsertedgrn;
					iwardmasterModel info = new iwardmasterModel();
					string query = WMSResource.Verifythreewaymatch.Replace("#pono", pono).Replace("#invoiceno", invoiceno);
					info = pgsql.QuerySingle<iwardmasterModel>(
					  query, null, commandType: CommandType.Text);
					if (info != null && info.grnnumber == null)
					{
						//iwardmasterModel infos = new iwardmasterModel();
						//string queryforgrn = WMSResource.verifyGRNgenerated.Replace("#pono", pono).Replace("#invoiceno", invoiceno);
						// infos = pgsql.QuerySingle<iwardmasterModel>(
						//   queryforgrn, null, commandType: CommandType.Text);
						//if (infos.grnnumber == null)
						//{
						//verify = s;
						int grnnextsequence = 0;
						string grnnumber = string.Empty;
						obj = pgsql.QuerySingle<sequencModel>(
					   lastinsertedgrn, null, commandType: CommandType.Text);
						if (obj.id != 0)
						{
							grnnextsequence = (Convert.ToInt32(obj.sequencenumber) + 1);
							grnnumber = obj.sequenceid + "-" + obj.year + "-" + grnnextsequence.ToString().PadLeft(6, '0');
							string updategrnnumber = WMSResource.updategrnnumber.Replace("#invoiceno", invoiceno).Replace("#pono", pono);
							using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
							{
								var results = DB.ExecuteScalar(updategrnnumber, new
								{
									grnnumber,

								});
							}
							verify.grnnumber = grnnumber;
							int id = obj.id;
							string updateseqnumber = WMSResource.updateseqnumber;
							using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
							{
								var results = DB.ExecuteScalar(updateseqnumber, new
								{
									grnnextsequence,
									id,

								});
							}
						}




						else
						{

						}
						string insertqueryforstatus = WMSResource.statusupdatebySecurity;
						using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
						{
							var results = DB.ExecuteScalar(insertqueryforstatus, new
							{
								pono,

							});
						}
						//}
					}

					else
					{
						verify.grnnumber = info.grnnumber;
					}
					return verify;
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "VerifythreeWay", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="datamodel"></param>
		/// <returns></returns>
		public async Task<string> insertquantity(List<inwardModel> datamodel)
		{

			inwardModel obj = new inwardModel();
			inwardModel getgrnnoforpo = new inwardModel();
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					datamodel[0].receiveddate = System.DateTime.Now;
					await pgsql.OpenAsync();

					string query = WMSResource.getinwmasterid.Replace("#pono", datamodel[0].pono).Replace("#invoiceno", datamodel[0].invoiceno);
					obj = pgsql.QuerySingle<inwardModel>(
					   query, null, commandType: CommandType.Text);
					string getGRNno = WMSResource.getGRNNo.Replace("#pono", datamodel[0].pono);
					getgrnnoforpo = pgsql.QueryFirstOrDefault<inwardModel>(
					   getGRNno, null, commandType: CommandType.Text);
					int inwardid = 0;
					if (obj.inwmasterid != 0)
					{

						foreach (var item in datamodel)
						{
							string insertforinvoicequery = WMSResource.insertforinvoicequery;
							item.deleteflag = false;
							string materialid = item.Material;
							using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
							{
								var results = DB.ExecuteScalar(insertforinvoicequery, new
								{
									obj.inwmasterid,
									//item.poitemid,
									item.receiveddate,
									item.receivedby,
									item.receivedqty,
									item.returnqty,
									item.confirmqty,
									materialid,
									item.deleteflag,

								});
								inwardid = Convert.ToInt32(results);
								//if (inwardid != 0)
								//{
								//    string insertqueryforqualitycheck =WMSResource.insertqueryforqualitycheck;

								//    var data = DB.ExecuteScalar(insertqueryforqualitycheck, new
								//    {
								//        inwardid,
								//        datamodel.quality,
								//        datamodel.qtype,
								//        datamodel.qcdate,
								//        datamodel.qcby,
								//        datamodel.remarks,
								//        datamodel.deleteflag,

								//    });
								string insertqueryforstatusforqty = WMSResource.insertqueryforstatusforqty;

								var data1 = DB.ExecuteScalar(insertqueryforstatusforqty, new
								{
									item.pono,
									item.returnqty

								});

							}
						}
					}

					//}
					return (Convert.ToString(inwardid));
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "insertquantity", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
			}
		}
		/// <summary>
		/// inserting material details to warehouse
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public string InsertStock(StockModel data)
		{
			try
			{
				StockModel obj = new StockModel();
				string loactiontext = string.Empty;
				var result = 0;
				int inwmasterid = 0;
				using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
				{
					StockModel objs = new StockModel();
					pgsql.Open();
					string query = WMSResource.getinwardmasterid.Replace("#grnnumber", data.grnnumber);
					objs = pgsql.QueryFirstOrDefault<StockModel>(
					   query, null, commandType: CommandType.Text);
					if (objs != null)
						inwmasterid = objs.inwmasterid;
				}
				//foreach (var item in data) { 
				data.createddate = System.DateTime.Now;
				string insertquery = WMSResource.insertstock;
				int itemid = 0;
				if (data.itemid == 0)
				{
					string materialid = data.Material;
					data.availableqty = data.confirmqty;
					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{
						result = Convert.ToInt32(DB.ExecuteScalar(insertquery, new
						{
							inwmasterid,
							data.pono,
							data.binid,
							data.vendorid,
							data.totalquantity,
							data.shelflife,
							data.availableqty,
							data.deleteflag,
							//data.itemreceivedfrom,
							data.itemlocation,
							data.createddate,
							data.createdby,
							data.stockstatus,
							materialid
						}));
						if (result != 0)
						{
							itemid = Convert.ToInt32(result);
							string insertqueryforlocationhistory = WMSResource.insertqueryforlocationhistory;
							var results = DB.ExecuteScalar(insertqueryforlocationhistory, new
							{
								data.itemlocation,
								itemid,
								data.createddate,
								data.createdby,

							});
							string insertqueryforstatuswarehouse = WMSResource.insertqueryforstatuswarehouse;

							var data1 = DB.ExecuteScalar(insertqueryforstatuswarehouse, new
							{
								data.pono,

							});


						}
					}
				}

				else
				{
					itemid = data.itemid;
					string updatequery = WMSResource.updatelocation.Replace("#itemlocation", data.itemlocation).Replace("#itemid", Convert.ToString(itemid));

					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{
						result = DB.Execute(updatequery, new
						{


						});
					}
				}
				using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
				{


					pgsql.Open();

					string selectqueryforloaction = WMSResource.getlocationasresponse.Replace("#itemid", itemid.ToString());
					obj = pgsql.QuerySingle<StockModel>(
						   selectqueryforloaction, null, commandType: CommandType.Text);
					if (obj.binnumber != null)
					{
						loactiontext = obj.binnumber;
					}
					else if (obj.racknumber != null)
					{
						loactiontext = obj.racknumber;
					}
					else
					{
						loactiontext = "no data";
					}
				}
				return (loactiontext);
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "InsertStock", Ex.StackTrace.ToString());
				return null;
			}

		}
		/// <summary>
		/// to get search data and pass  query dynamically
		/// </summary>
		/// <param name="Result"></param>
		/// <returns></returns>
		public DataTable GetListItems(DynamicSearchResult Result)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					pgsql.OpenAsync();
					DataTable dataTable = new DataTable();
					IDbCommand selectCommand = pgsql.CreateCommand();
					string query = "";
					query = "select * from " + Result.tableName + Result.searchCondition + "";
					if (!string.IsNullOrEmpty(Result.query))
						query = Result.query;
					selectCommand.CommandText = query;
					IDbDataAdapter dbDataAdapter = new NpgsqlDataAdapter();
					dbDataAdapter.SelectCommand = selectCommand;

					DataSet dataSet = new DataSet();

					dbDataAdapter.Fill(dataSet);
					return dataTable = dataSet.Tables[0];
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetListItems", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}

		}
		/// <summary>
		/// material request by Project manager
		/// </summary>
		/// <param name="reqdata"></param>
		/// <returns></returns>
		public int IssueRequest(List<IssueRequestModel> reqdata)
		{
			int requestid = 0;
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{
				IssueRequestModel obj = new IssueRequestModel();
				pgsql.Open();
				string query = WMSResource.getnextrequestid;
				obj = pgsql.QuerySingle<IssueRequestModel>(
				   query, null, commandType: CommandType.Text);
				requestid = obj.requestid + 1;
			}
			try
			{

				var result = 0;
				foreach (var item in reqdata)
				{

					item.requesteddate = System.DateTime.Now;
					string insertquery = WMSResource.materialquest;
					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{
						result = DB.Execute(insertquery, new
						{
							// item.paitemid,
							item.quantity,
							item.requesteddate,
							item.approveremailid,
							item.approverid,
							item.pono,
							item.materialid,
							item.requesterid,
							item.requestedquantity,
							requestid,
						});
					}



				}
				return (Convert.ToInt32(result));
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "IssueRequest", Ex.StackTrace.ToString());
				return 0;
			}
		}
		/// <summary>
		/// based on grnnumber will get lst of items
		/// </summary>
		/// <param name="grnnumber"></param>
		/// <returns></returns>
		public async Task<IEnumerable<inwardModel>> getitemdeatils(string grnnumber)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					await pgsql.OpenAsync();
					string queryforitemdetails = WMSResource.queryforitemdetails.Replace("#grnnumber", grnnumber);
					return await pgsql.QueryAsync<inwardModel>(
					   queryforitemdetails, null, commandType: CommandType.Text);
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getitemdeatils", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}
				//throw new NotImplementedException();
			}

		}
		/// <summary>
		/// requesting for material
		/// </summary>
		/// <param name="pono"></param>
		/// <param name="approverid"></param>
		/// <returns></returns>
		public async Task<IEnumerable<IssueRequestModel>> MaterialRequest(string pono, string approverid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{
				string materialrequestquery = WMSResource.materialrequestquery;
				//if (pono != null)
				//{
				//	materialrequestquery = materialrequestquery + " where openpo.pono = '" + pono + "'";
				//}
				if (approverid != null)
				{
					materialrequestquery = materialrequestquery + " where  req.requesterid = '" + approverid + "' ";
				}
				materialrequestquery = materialrequestquery + " group by req.requestid limit 50";
				try
				{
					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<IssueRequestModel>(
					   materialrequestquery, null, commandType: CommandType.Text);

				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "MaterialIssue", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// acknowledge fro received item from Project manager
		/// </summary>
		/// <param name="dataobj"></param>
		/// <returns></returns>
		public int acknowledgeMaterialReceived(List<IssueRequestModel> dataobj)
		{

			try
			{
				var result = 0;
				//data.createddate = System.DateTime.Now;
				foreach (var item in dataobj)
				{
					string ackstatus = string.Empty;
					if (item.status == true)
					{
						ackstatus = "received";
					}
					else if (item.status == false)
					{
						ackstatus = "not received";
					}
					DateTime approveddate = System.DateTime.Now;

					int requestforissueid = item.requestforissueid;
					int requestid = item.requestid;
					string ackremarks = item.ackremarks;
					int issuedquantity = item.issuedquantity;
					string updateackstatus = WMSResource.updateackstatus;

					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{

						result = DB.Execute(updateackstatus, new
						{
							ackstatus,
							ackremarks,
							requestid,

						});
					}
				}
				return (Convert.ToInt32(result));
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "updaterequestedqty", Ex.StackTrace.ToString());
				return 0;
			}
			//try
			//{
			//    //data.createddate = System.DateTime.Now;
			//    string insertquery = "update  wms.wms_inward set ackstatus='item received',ackremarks=@remarks where inwardid=@inwardid and materialid=@material";
			//    using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
			//    {
			//        var result = DB.ExecuteScalar(insertquery, new
			//        {
			//            remarks,
			//            inwardid,
			//            material,
			//        });

			//        return (Convert.ToInt32(result));


			//    }
			//}
			//catch (Exception Ex)
			//{
			//    log.ErrorMessage("PODataProvider", "acknowledgeMaterialReceived", Ex.StackTrace.ToString());
			//    return 0;
			//}

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="requesterid"></param>
		/// <returns></returns>

		public async Task<IEnumerable<IssueRequestModel>> GetMaterialissueList(string requesterid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.GetListForMaterialRequestByrequesterid.Replace("#requesterid", requesterid);

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<IssueRequestModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetMaterialList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// get list of material details based on approver id
		/// </summary>
		/// <param name="approverid"></param>
		/// <returns></returns>
		public async Task<IEnumerable<IssueRequestModel>> GetMaterialissueListforapprover(string approverid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.GetListForMaterialRequestByapproverid.Replace("#approverid", approverid);

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<IssueRequestModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetRequestList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// get list of materail details based on particlular requestid
		/// </summary>
		/// <param name="requestid"></param>
		/// <returns></returns>
		public async Task<IEnumerable<IssueRequestModel>> GetmaterialdetailsByrequestid(string requestid)
		{

			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.GetdetailsByrequestid.Replace("#requestid", requestid);

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<IssueRequestModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetRequestList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// get list of pono data
		/// </summary>
		/// <param name="pono"></param>
		/// <returns></returns>
		public async Task<IEnumerable<IssueRequestModel>> GetPonodetails(string pono)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.Getponodetailsformaterialissue.Replace("#pono", pono);

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<IssueRequestModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetPonodetails", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// inserting or updating requested qty by PM
		/// </summary>
		/// <param name="dataobj"></param>
		/// <returns></returns>
		public int updaterequestedqty(List<IssueRequestModel> dataobj)
		{

			int requestid = 1;
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{
				IssueRequestModel obj = new IssueRequestModel();
				pgsql.Open();
				string query = WMSResource.getnextrequestid;
				obj = pgsql.QueryFirstOrDefault<IssueRequestModel>(
				   query, null, commandType: CommandType.Text);
				if (obj != null)
					requestid = obj.requestid + 1;
			}
			try
			{

				var result = 0;
				foreach (var item in dataobj)
				{
					if (item.requestedquantity > 0) { 
						
						item.requesteddate = System.DateTime.Now;
					string insertquery = WMSResource.materialquest;
					string materialid = item.Material;
					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{
						result = DB.Execute(insertquery, new
						{
							//item.paitemid,
							item.quantity,
							item.requesteddate,
							item.approveremailid,
							item.approverid,
							item.pono,
							materialid,
							item.requesterid,
							requestid,
							item.requestedquantity
						});
					}
					if (result != 0)
					{
						int availableqty = item.availableqty - item.requestedquantity;
						string updatequery = WMSResource.updatestock.Replace("#availableqty", Convert.ToString(availableqty)).Replace("#itemid", Convert.ToString(item.itemid));
						using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
						{
							result = DB.Execute(updatequery, new
							{
							});
						}
					}

				}

				}
				return (Convert.ToInt32(result));
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "IssueRequest", Ex.StackTrace.ToString());
				return 0;
			}

			//try
			//{
			//    var result = 0;
			//    //data.createddate = System.DateTime.Now;
			//    foreach (var item in dataobj)
			//    {
			//        int requestedquantity = item.requestedquantity;
			//        int inwardid = item.inwardid;
			//        string materialid = item.Material;
			//        string insertquery = "update  wms.wms_inward set requestedquantity=@requestedquantity where inwardid="+inwardid+ " and materialid='"+materialid+"'";

			//        using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
			//        {

			//            result = DB.Execute(insertquery, new
			//            {
			//                requestedquantity
			//            });
			//        }
			//    }
			//    return (Convert.ToInt32(result));
			//}
			//catch (Exception Ex)
			//{
			//    log.ErrorMessage("PODataProvider", "updaterequestedqty", Ex.StackTrace.ToString());
			//    return 0;
			//}
		}
		/// <summary>
		/// issued matreial list 
		/// </summary>
		/// <param name="dataobj"></param>
		/// <returns></returns>
		public int ApproveMaterialissue(List<IssueRequestModel> dataobj)
		{
			try
			{
				var result = 0;
				//data.createddate = System.DateTime.Now;
				foreach (var item in dataobj)
				{
					string approvedstatus = string.Empty;
					if (item.issuedqty != 0)
					{
						approvedstatus = "approved";
					}
					//else
					//{
					//	approvedstatus = "rejected";
					//}
					DateTime approvedon = System.DateTime.Now;
					int itemid = 0;
					using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
					{
						IssueRequestModel obj = new IssueRequestModel();
						IssueRequestModel objs = new IssueRequestModel();
						pgsql.Open();
						string getitemidqry = WMSResource.getitemid.Replace("#materialid", item.materialid).Replace("#pono", item.pono);
						// string getmaterialidqry = WMSResource.getrequestforissueid.Replace("#materialid", item.materialid).Replace("#pono", item.pono);

						obj = pgsql.QueryFirstOrDefault<IssueRequestModel>(
						   getitemidqry, null, commandType: CommandType.Text);
						itemid = obj.itemid;
						//objs = pgsql.QuerySingle<IssueRequestModel>(
						//getmaterialidqry, null, commandType: CommandType.Text);

						//requestforissueid = objs.requestforissueid;
					}



					int requestforissueid = item.requestforissueid;
					string materialid = item.materialid;
					int issuedqty = item.issuedqty;
					DateTime itemissueddate = System.DateTime.Now;

					string updateapproverstatus = WMSResource.updateapproverstatus;

					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{

						result = DB.Execute(updateapproverstatus, new
						{
							approvedstatus,
							requestforissueid,
							approvedon,
							issuedqty,
							materialid,
							item.pono,
							itemid,
							item.itemreturnable,
							item.approvedby,
							itemissueddate,
							item.itemreceiverid,

						});
					}
				}
				return (Convert.ToInt32(result));
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "updaterequestedqty", Ex.StackTrace.ToString());
				return 0;
			}
		}
		/// <summary>
		/// get list of gatepass data
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<gatepassModel>> GetgatepassList()
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getgatepasslist;

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<gatepassModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetgatepassList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// insert or update gatepass info
		/// </summary>
		/// <param name="dataobj"></param>
		/// <returns></returns>
		public int SaveOrUpdateGatepassDetails(gatepassModel dataobj)
		{
			try
			{
				//foreach(var item in dataobj._list)
				//{

				if (dataobj.gatepassid == 0)
				{
					dataobj.requestedon = System.DateTime.Now;
					string insertquery = WMSResource.insertgatepassdata;
					dataobj.deleteflag = false;
					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{
						var gatepassid = DB.ExecuteScalar(insertquery, new
						{

							dataobj.gatepasstype,
							dataobj.status,
							dataobj.requestedon,
							dataobj.referenceno,
							dataobj.vehicleno,
							dataobj.requestedby,
							dataobj.deleteflag,
							dataobj.vendorname,
							dataobj.reasonforgatepass,
						});
						if (dataobj.gatepassid == 0)
							dataobj.gatepassid = Convert.ToInt32(gatepassid);
					}
				}
				else
				{
					dataobj.requestedon = System.DateTime.Now;
					string insertquery = WMSResource.updategatepass.Replace("#gatepassid", Convert.ToString(dataobj.gatepassid));

					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{
						var result = DB.ExecuteScalar(insertquery, new
						{

							dataobj.gatepasstype,
							dataobj.status,
							dataobj.requestedon,
							dataobj.referenceno,
							dataobj.vehicleno,
							dataobj.requestedby,
							dataobj.vendorname,
							dataobj.reasonforgatepass,
						});
					}
				}
				foreach (var item in dataobj.materialList)
				{

					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{
						if (item.gatepassmaterialid == 0)
						{
							string insertquerymaterial = WMSResource.insertgatepassmaterial;

							var results = DB.ExecuteScalar(insertquerymaterial, new
							{

								dataobj.gatepassid,
								item.materialid,
								item.quantity,
								dataobj.deleteflag,
								item.remarks,
								item.materialcost,
								item.expecteddate,
								item.returneddate,
							});
						}
						else
						{
							string updatequery = WMSResource.updategatepassmaterial.Replace("#gatepassmaterialid", Convert.ToString(item.gatepassmaterialid));

							var result = DB.ExecuteScalar(updatequery, new
							{

								dataobj.gatepassid,
								item.materialid,
								item.quantity,
								item.remarks,
								item.materialcost,
								item.expecteddate,
								item.returneddate,

							});
						}
					}
				}
				return (1);
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "SaveOrUpdateGatepassDetails", Ex.StackTrace.ToString());
				return 0;
			}
		}
		/// <summary>
		/// check material in stock
		/// </summary>
		/// <param name="material"></param>
		/// <param name="qty"></param>
		/// <returns></returns>
		public string checkmaterialandqty(string material = null, int qty = 0)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{
				materialistModel obj = new materialistModel();
				string returnvalue = string.Empty;
				try
				{
					pgsql.Open();
					if (material != null && qty == 0)
					{
						string query = WMSResource.checkmaterial.Replace("#materialid", material);
						obj = pgsql.QueryFirstOrDefault<materialistModel>(
						   query, null, commandType: CommandType.Text);
						if (obj == null)
						{
							returnvalue = "material does not exists";
						}
						else
						{
							returnvalue = "true";
						}
					}
					else if (qty != 0 && material == null)
					{
						string query = WMSResource.checkqty.Replace("#availableqty", Convert.ToString(qty));
						obj = pgsql.QueryFirstOrDefault<materialistModel>(
					query, null, commandType: CommandType.Text);
						if (obj == null)
						{
							returnvalue = "qty not available";
						}
						else
						{
							returnvalue = "true";
						}
					}
					else if (material != null && qty != 0)
					{
						string query = WMSResource.checkmaterialandqty.Replace("#availableqty", Convert.ToString(qty)).Replace("#materialid", material);
						obj = pgsql.QueryFirstOrDefault<materialistModel>(
					query, null, commandType: CommandType.Text);
						if (obj == null)
						{
							returnvalue = "material and quantity does not exists";
						}
						else
						{
							returnvalue = "true";
						}
					}
					return returnvalue;


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "checkmaterialandqty", Ex.StackTrace.ToString());
					return "false";
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// delete gatepass
		/// </summary>
		/// <param name="gatepassmaterialid"></param>
		/// <returns></returns>
		public int deletegatepassmaterial(int gatepassmaterialid)
		{
			int returndata = 0;
			try
			{
				string insertquery = WMSResource.deletegatepassmaterial.Replace("#gatepassmaterialid", Convert.ToString(gatepassmaterialid));
				using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
				{
					var data = DB.Execute(insertquery, new

					{


					});
					returndata = Convert.ToInt32(data);
				}
				return returndata;

			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "deletegatepassmaterial", Ex.StackTrace.ToString());
				return 0;
			}
		}
		/// <summary>
		/// update gatepass approver info
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int updategatepassapproverstatus(gatepassModel model)
		{
			int returndata = 0;
			try
			{
				model.approvedon = System.DateTime.Now;
				string insertquery = WMSResource.updategatepassapproverstatus.Replace("#gatepassid", Convert.ToString(model.gatepassid));
				using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
				{
					var data = DB.Execute(insertquery, new

					{
						model.approverremarks,
						model.approverstatus,
						model.approvedon,

					});
					returndata = Convert.ToInt32(data);
				}
				return returndata;

			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "updategatepassapproverstatus", Ex.StackTrace.ToString());
				return 0;
			}
		}
		/// <summary>
		/// get list of material based on gatepassid
		/// </summary>
		/// <param name="gatepassid"></param>
		/// <returns></returns>
		public async Task<IEnumerable<gatepassModel>> GetmaterialList(int gatepassid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getgatepassmaterialdetailList.Replace("#gatepassid", Convert.ToString(gatepassid));

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<gatepassModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetmaterialList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// updating print status
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int updateprintstatus(gatepassModel model)
		{
			{
				int returndata = 0;
				int data = 0;
				try
				{
					gatepassModel obj = new gatepassModel();

					using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
					{
						string query = WMSResource.getprintdetails.Replace("#gatepassid", Convert.ToString(model.gatepassid));

						pgsql.Open();
						obj = pgsql.QueryFirstOrDefault<gatepassModel>(
						   query, null, commandType: CommandType.Text);
					}
					if (obj.print == true)
					{
						string insertquery = WMSResource.printstatusupdate.Replace("#gatepassid", Convert.ToString(model.gatepassid));
						using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
						{
							var data1 = DB.Execute(insertquery, new

							{
								model.printedby,
							});
							returndata = Convert.ToInt32(data);
						}
					}
					else
					{
						using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
						{
							string query = WMSResource.checkreprintalreadydone;

							query = query + " gatepassid=" + model.gatepassid + " order by reprintcount desc limit 1";

							pgsql.Open();
							obj = pgsql.QueryFirstOrDefault<gatepassModel>(
							   query, null, commandType: CommandType.Text);


						}
						string insertquery = WMSResource.insertreprintcount;
						using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
						{

							if (obj == null)
							{ model.reprintcount = 1; }
							else
							{
								model.reprintcount = model.reprintcount + 1;
							}
							data = Convert.ToInt32(DB.ExecuteScalar(insertquery, new

							{

								model.gatepassid,
								model.reprintedby,
								model.reprintcount,

							}));
							returndata = Convert.ToInt32(data);
						}


						string updatequery = WMSResource.updatereprintcount.Replace("#reprinthistoryid", Convert.ToString(data));

						model.reprintcount = model.reprintcount + 1;
						using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
						{
							var data1 = DB.Execute(updatequery, new
							{
								model.reprintcount,
							});
							returndata = Convert.ToInt32(data);
						}
					}
					return returndata;
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "updateprintstatus", Ex.StackTrace.ToString());
					return 0;
				}
			}
		}
		//int returndata = 0;
		//try
		//{

		//    string insertquery = WMSResource.printstatusupdate.Replace("#gatepassid", Convert.ToString(model.gatepassid));
		//    using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
		//    {
		//        var data = DB.Execute(insertquery, new

		//        {  
		//            model.printedby,
		//        });
		//        returndata = Convert.ToInt32(data);
		//    }
		//    return returndata;

		//}
		//catch (Exception Ex)
		//{
		//    log.ErrorMessage("PODataProvider", "updateprintstatus", Ex.StackTrace.ToString());
		//    return 0;
		//}
		//}
		/// <summary>
		/// updating reprint status 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int updatereprintstatus(reprintModel model)
		{
			reprintModel obj = new reprintModel();
			reprintModel secondobj = new reprintModel();
			int returndata = 0;
			try
			{
				int data = 0;
				model.inwmasterid = (model.inwmasterid == null) ? null : model.inwmasterid;
				model.gatepassid = (model.gatepassid == null) ? null : model.gatepassid;
				string insertquery = WMSResource.insertreprintcount;
				using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
				{
					data = Convert.ToInt32(DB.ExecuteScalar(insertquery, new

					{
						model.inwmasterid,
						model.gatepassid,
						model.reprintcount,
						model.reprintedby,


					}));
					returndata = Convert.ToInt32(data);
				}
				using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
				{
					string query = WMSResource.checkreprintalreadydone;
					if (model.inwmasterid != null)
					{
						query = query + " inwmasterid=" + model.inwmasterid + " order by reprintcount desc limit 1";
					}
					else if (model.gatepassid != null)
					{
						query = query + " gatepassid=" + model.gatepassid + " order by reprintcount desc limit 1";
					}
					pgsql.Open();
					obj = pgsql.QuerySingle<reprintModel>(
					   query, null, commandType: CommandType.Text);
				}
				string updatequery = WMSResource.updatereprintcount.Replace("#reprinthistoryid", Convert.ToString(data));
				int reprintcount = obj.reprintcount + 1;
				using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
				{
					var data1 = DB.Execute(updatequery, new
					{
						reprintcount
					});
					returndata = Convert.ToInt32(data);
				}
				return returndata;

			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "updategatepassapproverstatus", Ex.StackTrace.ToString());
				return 0;
			}
		}
		/// <summary>
		/// get list based on ABC category
		/// </summary>
		/// <param name="categoryid"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ReportModel>> GetreportBasedCategory(int categoryid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					// string query = WMSResource.getcategorylist.Replace("#categoryid", Convert.ToString(categoryid));
					string query = WMSResource.getcategorylist;

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<ReportModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetreportBasedCategory", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}

		}
		/// <summary>
		/// get list based on materail in category
		/// </summary>
		/// <param name="materailid"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ReportModel>> GetreportBasedMaterial(string materailid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					// string query = WMSResource.getcategorylist.Replace("#categoryid", Convert.ToString(categoryid));
					string query = WMSResource.getcategorylistbymaterailid.Replace("#materialid", Convert.ToString(materailid));

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<ReportModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetreportBasedMaterial", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// updating abccategorydata
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int updateABCcategorydata(List<ABCCategoryModel> model)
		{
			int returndata = 0;
			try
			{

				int data = 0;
				if (model != null)
				{
					foreach (var item in model)
					{
						string updatequery = WMSResource.updateABCrange;
						item.updatedon = System.DateTime.Now;
						using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
						{
							data = Convert.ToInt32(DB.Execute(updatequery, new

							{
								item.updatedby,
								item.updatedon,

							}));
							returndata = Convert.ToInt32(data);

						}
						break;
					}
					if (returndata != 0)
					{
						foreach (var item in model)
						{
							item.startdate = item.startdate.AddDays(1);
							item.enddate = item.enddate.AddDays(1);
							string insertquery = WMSResource.insertABCrange;
							//item.createdon = System.DateTime.Now;
							using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
							{
								data = Convert.ToInt32(DB.ExecuteScalar(insertquery, new

								{
									item.categoryname,
									item.minpricevalue,
									item.maxpricevalue,
									item.createdby,
									item.startdate,
									item.enddate
								}));
								returndata = Convert.ToInt32(data);
							}
						}
					}


				}
				List<ReportModel> obj = new List<ReportModel>();
				//using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
				//{


				//        // string query = WMSResource.getcategorylist.Replace("#categoryid", Convert.ToString(categoryid));
				//        string query = WMSResource.getcategorylist;

				//    await pgsql.OpenAsync();
				//    return await pgsql.QueryAsync<ReportModel>(
				//       query, null, commandType: CommandType.Text);



				//}
				return returndata;
			}

			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "updateABCcategorydata", Ex.StackTrace.ToString());
				return 0;
			}
		}
/// <summary>
/// getabc categorydata
/// </summary>
/// <returns></returns>
		public async Task<IEnumerable<ABCCategoryModel>> GetABCCategorydata()
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					// string query = WMSResource.getcategorylist.Replace("#categoryid", Convert.ToString(categoryid));
					string query = WMSResource.getabccategorydata;

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<ABCCategoryModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetABCCategorydata", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// availability qty
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<ReportModel>> GetABCavailableqtyList()
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.GetallavlqtyABCList;

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<ReportModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetABCavailableqtyList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		//Ramesh (08/06/2020) returns Enquiry Details
		public async Task<Enquirydata> GetEnquirydata(string materialid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = "select stock.materialid, SUM(stock.availableqty) as availableqty, Max(op.materialdescription) as materialdescription from wms.wms_stock stock left outer join wms.\"MaterialMasterYGS\" op on  stock.materialid =op.material  where stock.materialid='" + materialid + "' group by stock.materialid";

					await pgsql.OpenAsync();
					var data = await pgsql.QueryAsync<Enquirydata>(
					   query, null, commandType: CommandType.Text);
					return data.FirstOrDefault();
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetEnquirydata", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}


		//Ramesh (08/06/2020) returns all  Materials to count
		public async Task<IEnumerable<CycleCountList>> GetCyclecountList(int limita, int limitb, int limitc)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					Cyclecountconfigmodel config = new Cyclecountconfigmodel();
					await pgsql.OpenAsync();

					//Ramesh (08/06/2020) returns category A/B/C configuration

					string QueryABC = "select * from wms.wms_rd_category order by categoryid desc limit 3";
					var abcdata = await pgsql.QueryAsync<ABCCategoryModel>(QueryABC, null, commandType: CommandType.Text);
					foreach (ABCCategoryModel dt in abcdata)
					{
						if (dt.categoryname == "A")
						{
							config.amin = Convert.ToInt32(dt.minpricevalue);
							config.amax = Convert.ToInt32(dt.maxpricevalue);
						}
						else if (dt.categoryname == "B")
						{
							config.bmin = Convert.ToInt32(dt.minpricevalue);
							config.bmax = Convert.ToInt32(dt.maxpricevalue);
						}
						else if (dt.categoryname == "C")
						{
							config.cmin = Convert.ToInt32(dt.minpricevalue);
							config.cmax = Convert.ToInt32(dt.maxpricevalue);
						}
						config.startdate = dt.startdate;
						config.enddate = dt.enddate;

					}

					//Ramesh (08/06/2020) returns today counted list 
					string Querychecktodaycounted = "Select * from wms.cyclecount where counted_on = current_date";
					var todaycountdata = await pgsql.QueryAsync<CycleCountList>(Querychecktodaycounted, null, commandType: CommandType.Text);

					//Ramesh (08/06/2020) returns random A/B/C List
					string QueryA = "select sum(ws.availableqty) as availableqty,'A' AS category,ws.materialid AS materialid from wms.wms_stock ws WHERE ws.materialid IS NOT null and ws.unitprice::numeric >= " + config.amin + " group by ws.materialid order by random() limit " + limita + "";
					string QueryB = "select sum(ws.availableqty) as availableqty,'B' AS category,ws.materialid AS materialid from wms.wms_stock ws WHERE ws.materialid IS NOT null and ws.unitprice::numeric >= " + config.bmin + " and ws.unitprice::numeric <= " + config.bmax + " group by ws.materialid order by random() limit " + limitb + "";
					string QueryC = "select sum(ws.availableqty) as availableqty,'C' AS category,ws.materialid AS materialid from wms.wms_stock ws WHERE ws.materialid IS NOT null and ws.unitprice::numeric <= " + config.cmax + " group by ws.materialid order by random() limit " + limitc + "";
					var adata = await pgsql.QueryAsync<CycleCountList>(QueryA, null, commandType: CommandType.Text);
					var bdata = await pgsql.QueryAsync<CycleCountList>(QueryB, null, commandType: CommandType.Text);
					var cdata = await pgsql.QueryAsync<CycleCountList>(QueryC, null, commandType: CommandType.Text);
					var finaltempdata = adata.Concat(bdata);
					var finaldata = finaltempdata.Concat(cdata);
					foreach (CycleCountList cc in finaldata)
					{
						string Querycheckcounted = "Select * from wms.cyclecount where materialid = '" + cc.materialid + "' and counted_on = current_date";
						var countdata = await pgsql.QueryAsync<CycleCountList>(Querycheckcounted, null, commandType: CommandType.Text);
						if (countdata != null && countdata.Count() > 0)
						{
							var dt = countdata.FirstOrDefault();
							cc.status = dt.status;
							cc.physicalqty = dt.physicalqty;
							cc.difference = dt.difference;
							cc.iscounted = true;
							if (todaycountdata != null && todaycountdata.Count() > 0)
							{
								cc.todayscount = todaycountdata.Count();
							}
							else
							{
								cc.todayscount = 0;

							}
						}
					}

					return finaldata;

					//string query = WMSResource.getCyclecountList;
					//return await pgsql.QueryAsync<CycleCountList>(
					//query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetCyclecountList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		//Ramesh (08/06/2020) returns All counted Material list
		public async Task<IEnumerable<CycleCountList>> GetCyclecountPendingList()
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					Cyclecountconfigmodel config = new Cyclecountconfigmodel();
					await pgsql.OpenAsync();
					string QueryA = "Select * from wms.cyclecount";
					var adata = await pgsql.QueryAsync<CycleCountList>(QueryA, null, commandType: CommandType.Text);
					return adata;
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetCyclecountPendingList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		//Ramesh (08/06/2020) update or insert cycle count
		public int UpdateinsertCycleCount(List<CycleCountList> dataobj)
		{
			using (var DB = new NpgsqlConnection(config.PostgresConnectionString))
			{
				DB.OpenAsync();
				try
				{
					foreach (CycleCountList obj in dataobj)
					{
						//Ramesh (08/06/2020) approver action updation 
						if (obj.isapprovalprocess)
						{
							string status = obj.isapproved ? "Approved" : "Rejected";

							string insertquery = "update wms.cyclecount set status='" + status + "', remarks='" + obj.remarks + "',verified_on = current_date , verified_by = 'Ramesh' where id = '" + obj.id + "' ";
							var result = DB.ExecuteScalar(insertquery);



						}
						else
						{
							//Ramesh (08/06/2020) user count action updation/insertion 
							string selquery = "select * from wms.cyclecount where materialid = '" + obj.materialid + "' and counted_on = current_date ";
							var seldata = DB.QueryAsync<CycleCountList>(selquery, null, commandType: CommandType.Text);
							CycleCountList dt = new CycleCountList();
							if (seldata.Result.Count() > 0)
							{
								//Ramesh (08/06/2020) user count action updation 
								obj.difference = Math.Abs(obj.physicalqty - obj.availableqty);
								string insertquery = "update wms.cyclecount set category='" + obj.category + "', materialid= '" + obj.materialid + "', availableqty= " + obj.availableqty + ", physicalqty=" + obj.physicalqty + ", difference=" + obj.difference + ", status='Pending', counted_on = current_date , counted_by = 'Ramesh', verified_on = null , verified_by = null where materialid = '" + obj.materialid + "' ";
								var result = DB.ExecuteScalar(insertquery);

							}
							else
							{
								//Ramesh (08/06/2020) user count action insertion 
								obj.difference = Math.Abs(obj.physicalqty - obj.availableqty);
								string insertquery = "insert into wms.cyclecount(category, materialid, availableqty, physicalqty, difference, status, counted_on, counted_by, verified_on, verified_by) values('" + obj.category + "', '" + obj.materialid + "', " + obj.availableqty + ", " + obj.physicalqty + ", " + obj.difference + ", 'Pending', current_date , 'Ramesh', null, null)";
								var result = DB.ExecuteScalar(insertquery);
							}

						}


					}
					return 1;
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "UpdateinsertCycleCount", Ex.StackTrace.ToString());
					return 0;
				}
				finally
				{
					DB.Close();
				}
			}
		}

		//Ramesh (08/06/2020) update cycle count configuration 
		public int UpdateCycleCountconfig(Cyclecountconfig dataobj)
		{
			try
			{
				//foreach(var item in dataobj._list)
				//{




				string insertquery = "update wms.cyclecountconfig set apercentage = " + dataobj.apercentage + ",bpercentage = " + dataobj.bpercentage + ",cpercentage = " + dataobj.cpercentage + ",cyclecount = " + dataobj.cyclecount + ",frequency = '" + dataobj.frequency + "',notificationtype='" + dataobj.notificationtype + "',notificationon='" + dataobj.notificationon + "' where id = 1";

				//string insertquery = WMSResource.updatecyclecountconfig.Replace("#cid", "1");

				using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
				{
					var result = DB.ExecuteScalar(insertquery);
					//var result = DB.ExecuteScalar(insertquery, new
					//{

					//    dataobj.apercentage,
					//    dataobj.bpercentage,
					//    dataobj.cpercentage,
					//    dataobj.cyclecount,
					//    dataobj.frequency
					//});

				}
				return 1;

			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "UpdateCycleCountconfig", Ex.StackTrace.ToString());
				return 0;
			}
		}

		//Ramesh (08/06/2020) update cycle count configuration
		public async Task<Cyclecountconfig> GetCyclecountConfig()
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					Cyclecountconfig config = new Cyclecountconfig();
					await pgsql.OpenAsync();
					int count = 0;
					//Ramesh (08/06/2020) get count of all materials on which cycle configuration and randon percentage will be applied
					string QueryALL = "select sum(availableqty) as availableqty,ws.materialid AS materialid from wms.wms_stock ws WHERE ws.materialid IS NOT null and ws.unitprice IS NOT null  group by materialid";
					var alldata = await pgsql.QueryAsync<CycleCountList>(QueryALL, null, commandType: CommandType.Text);

					//Ramesh (08/06/2020) get cycle configuration

					string QueryABC = "select * from wms.cyclecountconfig where id = 1";
					var abcdata = await pgsql.QueryAsync<Cyclecountconfig>(QueryABC, null, commandType: CommandType.Text);
					if (abcdata != null)
					{
						config = abcdata.FirstOrDefault();
					}

					//Ramesh (08/06/2020) get ABC configuration start and end date of cycle
					string QueryABC1 = "select * from wms.wms_rd_category order by categoryid desc limit 3";
					var abcdata1 = await pgsql.QueryAsync<ABCCategoryModel>(QueryABC1, null, commandType: CommandType.Text);
					ABCCategoryModel abcconfig = new ABCCategoryModel();
					if (abcdata1 != null)
					{
						abcconfig = abcdata1.FirstOrDefault();
						config.startdate = abcconfig.startdate;
						config.enddate = abcconfig.enddate;
					}
					if (alldata != null)
					{
						count = alldata.Count();
						config.countall = count;

					}


					return config;



				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetCyclecountConfig", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public async Task<IEnumerable<ReportModel>> GetABCListBycategory(string category)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.GetABCdetailsBycategory.Replace("abcname", category);

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<ReportModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetABCListBycategory", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public async Task<IEnumerable<FIFOModel>> GetFIFOList(string material)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getFIFOList;
					if (material == "null")
					{
						query = query + " order by createddate asc";
					}
					else
					{
						query = query + " and sk.materialid like'%" + material + "' order by createddate asc";
					}


					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<FIFOModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetFIFOList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public ReportModel checkloldestmaterial(string materialid, string createddate)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.checkoldestmaterial.Replace("#materialid", materialid).Replace("#createddate", Convert.ToString(createddate));


					pgsql.Open();
					return pgsql.QuerySingle<ReportModel>(
					   query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "checkloldestmaterial", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public int FIFOitemsupdate(List<FIFOModel> model)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					foreach (var item in model)
					{

						string insertforinvoicequery = WMSResource.insertFIFOdata;
						using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
						{
							var results = DB.ExecuteScalar(insertforinvoicequery, new
							{
								item.itemid,
								item.materialid,
								item.pono

							});
							int availableqty = item.availableqty - item.issueqty;

							string insertqueryforstatusforqty = WMSResource.updateqtyafterissue.Replace("#itemid", Convert.ToString(item.itemid)).Replace("#availableqty", Convert.ToString(availableqty));

							var data1 = DB.ExecuteScalar(insertqueryforstatusforqty, new
							{

							});

						}
					}


					//}
					return (1);
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "FIFOitemsupdate", Ex.StackTrace.ToString());
					return 0;
				}
				finally
				{
					pgsql.Close();
				}
			}
		}

		//get open po list based on current date
		public async Task<IEnumerable<OpenPoModel>> getASNList(string deliverydate)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getASNList;
					query = query + " where deliverydate = '" + deliverydate + "'";
					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<OpenPoModel>(
					   query, null, commandType: CommandType.Text);
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getASNList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public async Task<IEnumerable<IssueRequestModel>> GetItemlocationListBymterial(string material)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getitemlocationList.Replace("#materialid", material);


					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<IssueRequestModel>(
					  query, null, commandType: CommandType.Text);


				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetItemlocationListBymterial", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public int updateissuedmaterial(List<IssueRequestModel> obj)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					foreach (var item in obj)
					{

						string insertforinvoicequery = WMSResource.insertFIFOdata;
						using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
						{
							//var results = DB.ExecuteScalar(insertforinvoicequery, new
							//{
							//	item.itemid,
							//	item.materialid,
							//	item.pono

							//});
							int availableqty = item.availableqty - item.issuedquantity;

							string insertqueryforstatusforqty = WMSResource.updateqtyafterissue.Replace("#itemid", Convert.ToString(item.itemid)).Replace("#availableqty", Convert.ToString(availableqty));

							var data1 = DB.ExecuteScalar(insertqueryforstatusforqty, new
							{

							});

						}
					}


					//}
					return (1);
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "FIFOitemsupdate", Ex.StackTrace.ToString());
					return 0;
				}
				finally
				{
					pgsql.Close();
				}
			}
		}

		//Name of Function : <<AssignRoles>>  Author :<<prasanna>>  
		//Date of Creation <<10-06-2020>>
		//Purpose : <<insert method to Asssign roles for employee >>
		//Review Date :<<>>   Reviewed By :<<>>
		public int assignRole(authUser model)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					model.createddate = System.DateTime.Now;
					string insertquery = WMSResource.insertAuthUserData;
					model.deleteflag = false;
					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{
						var results = DB.ExecuteScalar(insertquery, new
						{
							model.employeeid,
							model.roleid,
							model.createddate,
							model.createdby,
							model.deleteflag
						});
						return (Convert.ToInt32(results));
					}
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "assignRole", Ex.StackTrace.ToString());
					return 0;
				}
				finally
				{
					pgsql.Close();
				}
			}
		}


		//Name of Function : <<getuserAcessList>>  Author :<<prasanna>>  
		//Date of Creation <<11-06-2020>>
		//Purpose : <<function used to get Acessnames list based on employeeid,roleid >>
		//Review Date :<<>>   Reviewed By :<<>>
		public async Task<IEnumerable<userAcessNamesModel>> getuserAcessList(string employeeid, string roleid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{

				try
				{
					string query = WMSResource.getUserAcessNames.Replace("#employeeid", employeeid);
					query = query.Replace("#roleid", roleid);
					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<userAcessNamesModel>(
					  query, null, commandType: CommandType.Text);

				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getuserAcessList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		//Name of Function : <<getdashboarddata>>  Author :<<Ramesh>>  
		//Date of Creation <<17-06-2020>>
		//Purpose : <<function to get dashboard data >>
		//Review Date :<<>>   Reviewed By :<<>>
		public async Task<DashboardModel> getdashboarddata()
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{


				try
				{
					DashboardModel result = new DashboardModel();
					result.graphdata = new List<DashboardGraphModel>();
					DateTime dt = DateTime.Now;
					int i = 1;
					while (i <= 7)
					{
						if (i == 1)
						{
							string date = dt.ToString("yyyy-MM-dd");
							string query1 = "Select Count(*) as todayexpextedcount from wms.openpolistview where deliverydate = current_date";
							string query2 = "select count(*) as todayreceivedcount from wms.wms_securityinward where receiveddate <= '" + date + " 23:59:59' and receiveddate >= '" + date + " 00:00:00' ";
							string query3 = "select count(*) as todaytoissuecount from wms.wms_materialrequest where requesteddate = '" + date + " 23:59:59' and requesteddate >= '" + date + " 00:00:00'";

							await pgsql.OpenAsync();
							var data1 = await pgsql.QueryAsync<DashboardModel>(query1, null, commandType: CommandType.Text);
							var data2 = await pgsql.QueryAsync<DashboardModel>(query2, null, commandType: CommandType.Text);
							var data3 = await pgsql.QueryAsync<DashboardModel>(query3, null, commandType: CommandType.Text);
							DashboardGraphModel md = new DashboardGraphModel();
							md.date = dt.ToString("dd/MM/yyyy");
							if (data1 != null)
							{
								result.todayexpextedcount = data1.FirstOrDefault().todayexpextedcount;
								md.expectedcount = data1.FirstOrDefault().todayexpextedcount;
							}
							if (data2 != null)
							{
								result.todayreceivedcount = data2.FirstOrDefault().todayreceivedcount;
								md.receivedcount = data2.FirstOrDefault().todayreceivedcount;
							}
							if (data3 != null)
							{
								result.todaytoissuecount = data3.FirstOrDefault().todaytoissuecount;
								md.toissuecount = data3.FirstOrDefault().todaytoissuecount;
							}
							result.graphdata.Add(md);
						}
						else
						{
							dt = dt.AddDays(-1);
							string date = dt.ToString("yyyy-MM-dd");
							string date1 = dt.ToString("dd/MM/yyyy");
							string query1 = "Select Count(*) as todayexpextedcount from wms.openpolistview where deliverydate = '" + date + "'";
							string query2 = "select count(*) as todayreceivedcount from wms.wms_securityinward where receiveddate <= '" + date + " 23:59:59' and receiveddate >= '" + date + " 00:00:00' ";
							string query3 = "select count(*) as todaytoissuecount from wms.wms_materialrequest where requesteddate = '" + date + " 23:59:59' and requesteddate >= '" + date + " 00:00:00'";
							var data1 = await pgsql.QueryAsync<DashboardModel>(query1, null, commandType: CommandType.Text);
							var data2 = await pgsql.QueryAsync<DashboardModel>(query2, null, commandType: CommandType.Text);
							var data3 = await pgsql.QueryAsync<DashboardModel>(query3, null, commandType: CommandType.Text);
							DashboardGraphModel md = new DashboardGraphModel();
							md.date = date1;
							if (data1 != null)
							{
								md.expectedcount = data1.FirstOrDefault().todayexpextedcount;
							}
							if (data2 != null)
							{
								md.receivedcount = data2.FirstOrDefault().todayreceivedcount;
							}
							if (data3 != null)
							{
								md.toissuecount = data3.FirstOrDefault().todaytoissuecount;
							}
							result.graphdata.Add(md);
						}

						i++;
					}



					return result;
				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getdashboarddata", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public async Task<IEnumerable<IssueRequestModel>> MaterialRequestdata(string pono, string approverid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{


				try
				{
					string materialrequestquery = WMSResource.getmaterialdetailfprrequest;
					if (pono != null)
					{
						materialrequestquery = materialrequestquery + " and openpo.pono = '" + pono + "'";
					}
					if (approverid != null)
					{
						materialrequestquery = materialrequestquery + " and openpo.projectmanager = '" + approverid + "' limit 50";
					}
					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<IssueRequestModel>(
					  materialrequestquery, null, commandType: CommandType.Text);

				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "MaterialRequestdata", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public async Task<IEnumerable<IssueRequestModel>> getissuematerialdetails(int requestid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{


				try
				{
					string materialrequestquery = WMSResource.issuedqtydetails.Replace("#requestid", Convert.ToString(requestid));

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<IssueRequestModel>(
					  materialrequestquery, null, commandType: CommandType.Text);

				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getissuematerialdetails", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public int insertResevematerial(List<ReserveMaterialModel> datamodel)
		{
			int reserveid = 0;
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{
				ReserveMaterialModel obj = new ReserveMaterialModel();
				pgsql.Open();
				string query = WMSResource.getnextreservetid;
				obj = pgsql.QueryFirstOrDefault<ReserveMaterialModel>(
				   query, null, commandType: CommandType.Text);
				if (obj == null)
					reserveid = 1;
				else
				{
					reserveid = obj.reserveid + 1;
				}

			}
			try
			{

				var result = 0;
				foreach (var item in datamodel)
				{

					if (item.reservedqty > 0)
					{
						item.materialid = item.material;
						string insertquery = WMSResource.insertreservematerial;
						using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
						{
							result = DB.Execute(insertquery, new
							{
								item.materialid,
								item.itemid,
								item.pono,
								item.reservedby,
								item.reservedqty,
								reserveid,
							});
						}

						if (result != 0)
						{
							int availableqty = item.availableqty - item.reservedqty;
							string updatequery = WMSResource.updatestock.Replace("#availableqty", Convert.ToString(availableqty)).Replace("#itemid", Convert.ToString(item.itemid));
							using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
							{
								result = DB.Execute(updatequery, new
								{
								});
							}
						}

					}
				}

				return (Convert.ToInt32(result));
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "insertResevematerial", Ex.StackTrace.ToString());
				return 0;
			}
		}

		public async Task<IEnumerable<ReserveMaterialModel>> GetReservedMaterialList(string reservedby)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{


				try
				{
					string materialrequestquery = WMSResource.getreservedmaterialList.Replace("#reservedby", reservedby);
					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<ReserveMaterialModel>(
					  materialrequestquery, null, commandType: CommandType.Text);

				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetReservedMaterialList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public async Task<IEnumerable<ReserveMaterialModel>> getissuematerialdetailsforreserved(int reservedid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{


				try
				{
					string materialrequestquery = WMSResource.Getreleasedqty.Replace("#reserveid", Convert.ToString(reservedid));

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<ReserveMaterialModel>(
					  materialrequestquery, null, commandType: CommandType.Text);

				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "getissuematerialdetails", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public async Task<IEnumerable<ReserveMaterialModel>> GetReleasedmaterialList()
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{
				try
				{
					string materialrequestquery = WMSResource.GetreleasedmaterialList;

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<ReserveMaterialModel>(
					  materialrequestquery, null, commandType: CommandType.Text);

				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetReleasedmaterialList", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}

		public async Task<IEnumerable<ReserveMaterialModel>> GetmaterialdetailsByreserveid(string reserveid)
		{
			using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
			{
				try
				{
					string materialrequestquery = WMSResource.getmaterialdetailsbyreserveid.Replace("#reserveid", reserveid);

					await pgsql.OpenAsync();
					return await pgsql.QueryAsync<ReserveMaterialModel>(
					  materialrequestquery, null, commandType: CommandType.Text);

				}
				catch (Exception Ex)
				{
					log.ErrorMessage("PODataProvider", "GetmaterialdetailsByreserveid", Ex.StackTrace.ToString());
					return null;
				}
				finally
				{
					pgsql.Close();
				}

			}
		}
		/// <summary>
		/// ApproveMaterialRelease
		/// </summary>
		/// <param name="dataobj"></param>
		/// <returns></returns>
		public int ApproveMaterialRelease(List<ReserveMaterialModel> dataobj)
		{
			try
			{
				var result = 0;
				//data.createddate = System.DateTime.Now;
				foreach (var item in dataobj)
				{
					string approvedstatus = string.Empty;
					if (item.issuedqty != 0)
					{
						approvedstatus = "approved";
					}
					//else
					//{
					//	approvedstatus = "rejected";
					//}
					DateTime approvedon = System.DateTime.Now;
					int itemid = 0;
					using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
					{
						IssueRequestModel obj = new IssueRequestModel();
						IssueRequestModel objs = new IssueRequestModel();
						pgsql.Open();
						string getitemidqry = WMSResource.getitemid.Replace("#materialid", item.materialid).Replace("#pono", item.pono);
						// string getmaterialidqry = WMSResource.getrequestforissueid.Replace("#materialid", item.materialid).Replace("#pono", item.pono);

						obj = pgsql.QueryFirstOrDefault<IssueRequestModel>(
						   getitemidqry, null, commandType: CommandType.Text);
						itemid = obj.itemid;
						//objs = pgsql.QuerySingle<IssueRequestModel>(
						//getmaterialidqry, null, commandType: CommandType.Text);

						//requestforissueid = objs.requestforissueid;
					}



					int reserveformaterialid = item.reserveformaterialid;
					string materialid = item.materialid;
					int issuedqty = item.issuedqty;
					DateTime itemissueddate = System.DateTime.Now;

					string updateapproverstatus = WMSResource.updateapproverstatusforrelease;

					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{

						result = DB.Execute(updateapproverstatus, new
						{
							approvedstatus,
							reserveformaterialid,
							approvedon,
							issuedqty,
							materialid,
							item.pono,
							itemid,
							item.itemreturnable,
							item.approvedby,
							itemissueddate,
							item.itemreceiverid,

						});
					}
				}
				return (Convert.ToInt32(result));
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "updaterequestedqty", Ex.StackTrace.ToString());
				return 0;
			}
		

	}

		public int acknowledgeMaterialReceivedforreserved(List<ReserveMaterialModel> dataobj)
		{
			try
			{
				var result = 0;
				//data.createddate = System.DateTime.Now;
				foreach (var item in dataobj)
				{
					string ackstatus = string.Empty;
					if (item.status == true)
					{
						ackstatus = "received";
					}
					else if (item.status == false)
					{
						ackstatus = "not received";
					}
					DateTime approveddate = System.DateTime.Now;

					int requestforissueid = item.reserveformaterialid;
					int reserveid = item.reserveid;
					string ackremarks = item.ackremarks;
					int issuedquantity = item.issuedqty;
					string updateackstatus = WMSResource.updateackstatusforreserved;

					using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
					{

						result = DB.Execute(updateackstatus, new
						{
							ackstatus,
							ackremarks,
							reserveid,

						});
					}
				}
				return (Convert.ToInt32(result));
			}
			catch (Exception Ex)
			{
				log.ErrorMessage("PODataProvider", "updaterequestedqty", Ex.StackTrace.ToString());
				return 0;
			}

		}
	}
}
