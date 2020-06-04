using Dapper;
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



        public async Task<IEnumerable<OpenPoModel>> getOpenPoList(string loginid, string pono = null, string docno = null, string vendorid = null)
        {
            using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
            {

                try
                {
                    string query = WMSResource.openpolist.Replace("#projectmanager", loginid);
                    if (pono != null)
                    {
                        query = query + " and pono='" + pono + "'";
                    }
                    if (docno != null)
                    {
                        query = query + " and documentno='" + docno + "'";
                    }
                    if (vendorid != null)
                    {
                        query = query + " and  vendorid=" + vendorid;
                    }

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

        public int InsertBarcodeInfo(BarcodeModel dataobj)
        {
            try
            {
                dataobj.createddate = System.DateTime.Now;
                string insertquery = WMSResource.insertbarcodedata;
                using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
                {
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
            catch (Exception Ex)
            {
                log.ErrorMessage("PODataProvider", "InsertBarcodeInfo", Ex.StackTrace.ToString());
                return 0;
            }

        }

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

        public bool VerifythreeWay(string pono, string invoiceno, int quantity, string projectcode, string material)
        {
            Boolean verify = false;
            sequencModel obj = new sequencModel();
            using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
            {

                try
                {
                    pgsql.Open();
                    string lastinsertedgrn = WMSResource.lastinsertedgrn;

                    string query = WMSResource.Verifythreewaymatch.Replace("#pono", pono).Replace("#invoiceno", invoiceno).Replace("#quantity", quantity.ToString()).Replace("#projectcode", projectcode).Replace("#material", material);
                    var info = pgsql.QuerySingle(
                       query, null, commandType: CommandType.Text);
                    if (info != null)
                    {
                        verify = true;
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
                    }
                    else
                    {
                        verify = false;
                    }
                    return verify;
                }
                catch (Exception Ex)
                {
                    log.ErrorMessage("PODataProvider", "VerifythreeWay", Ex.StackTrace.ToString());
                    return false;
                }
                finally
                {
                    pgsql.Close();
                }
                //throw new NotImplementedException();
            }
        }



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

                    string query = WMSResource.getinwmasterid.Replace("#pono", datamodel[0].pono);
                    obj = pgsql.QuerySingle<inwardModel>(
                       query, null, commandType: CommandType.Text);
                    string getGRNno = WMSResource.getGRNNo.Replace("#pono", datamodel[0].pono);
                    getgrnnoforpo = pgsql.QuerySingle<inwardModel>(
                       getGRNno, null, commandType: CommandType.Text);

                    if (obj.inwmasterid != 0)
                    {

                        foreach (var item in datamodel)
                        {
                            string insertforinvoicequery = WMSResource.insertforinvoicequery;
                            using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
                            {
                                var results = DB.ExecuteScalar(insertforinvoicequery, new
                                {
                                    obj.inwmasterid,
                                    //item.poitemid,
                                    item.receiveddate,
                                    item.receivedby,
                                    item.returnqty,
                                    item.confirmqty,
                                    item.deleteflag,

                                });
                                int inwardid = Convert.ToInt32(results);
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
                    return (getgrnnoforpo.grnnumber);
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
                    objs = pgsql.QuerySingle<StockModel>(
                       query, null, commandType: CommandType.Text);
                    inwmasterid = objs.inwmasterid;
                }
                //foreach (var item in data) { 
                data.createddate = System.DateTime.Now;
                string insertquery = WMSResource.insertstock;
                int itemid = 0;
                if (data.itemid == 0)
                {
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
                            data.stockstatus
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

        public async Task<IEnumerable<IssueRequestModel>> MaterialRequest(string pono, string approverid)
        {
            using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
            {
                string materialrequestquery = WMSResource.materialrequestquery;
                if (pono != null)
                {
                    materialrequestquery = materialrequestquery + " and openpo.pono = '" + pono + "'";
                }
                if (approverid != null)
                {
                    materialrequestquery = materialrequestquery + " and openpo.projectmanager = '" + approverid + "' limit 50";
                }
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

        public int acknowledgeMaterialReceived(List<IssueRequestModel> dataobj)
        {

            try
            {
                var result = 0;
                //data.createddate = System.DateTime.Now;
                foreach (var item in dataobj)
                {
                    string ackstatus = string.Empty;
                    if (item.status != true)
                    {
                        ackstatus = "received";
                    }
                    else if (item.status == false)
                    {
                        ackstatus = "not received";
                    }
                    DateTime approveddate = System.DateTime.Now;

                    int requestforissueid = item.requestforissueid;
                    string materialid = item.Material;
                    string ackremarks = item.ackremarks;
                    int issuedquantity = item.issuedquantity;
                    string updateackstatus = WMSResource.updateackstatus;

                    using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
                    {

                        result = DB.Execute(updateackstatus, new
                        {
                            ackstatus,
                            ackremarks,
                            requestforissueid,
                            materialid,

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

        public int updaterequestedqty(List<IssueRequestModel> dataobj)
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
                foreach (var item in dataobj)
                {

                    item.requesteddate = System.DateTime.Now;
                    string insertquery = WMSResource.materialquest;
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
                            item.materialid,
                            item.requesterid,
                            requestid,
                            item.requestedquantity
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

        public int ApproveMaterialissue(List<IssueRequestModel> dataobj)
        {
            try
            {
                var result = 0;
                //data.createddate = System.DateTime.Now;
                foreach (var item in dataobj)
                {
                    string approverstatus = string.Empty;
                    if (item.issuedquantity != 0)
                    {
                        approverstatus = "approved";
                    }
                    else
                    {
                        approverstatus = "rejected";
                    }
                    DateTime approvedon = System.DateTime.Now;
                    int itemid = 0;
                    using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
                    {
                        IssueRequestModel obj = new IssueRequestModel();
                        IssueRequestModel objs = new IssueRequestModel();
                        pgsql.Open();
                        string getitemidqry = WMSResource.getitemid.Replace("#materialid",item.materialid).Replace("#pono",item.pono) ;
                       // string getmaterialidqry = WMSResource.getrequestforissueid.Replace("#materialid", item.materialid).Replace("#pono", item.pono);

                        obj = pgsql.QuerySingle<IssueRequestModel>(
                           getitemidqry, null, commandType: CommandType.Text);
                        itemid = obj.itemid;
                        //objs = pgsql.QuerySingle<IssueRequestModel>(
                        //getmaterialidqry, null, commandType: CommandType.Text);
                        
                        //requestforissueid = objs.requestforissueid;
                    }



                    int requestforissueid = item.requestforissueid;
                    string materialid = item.Material;
                    int issuedqty = item.issuedquantity;
                    DateTime itemissueddate = System.DateTime.Now;
                    string updateapproverstatus = WMSResource.updateapproverstatus;

                    using (IDbConnection DB = new NpgsqlConnection(config.PostgresConnectionString))
                    {

                        result = DB.Execute(updateapproverstatus, new
                        {
                            approverstatus,
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
                      var  gatepassid = DB.ExecuteScalar(insertquery, new
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

        public string checkmaterialandqty(string material = null, int qty = 0)
        {
            using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
            {
                materialistModel obj = new materialistModel();
                string returnvalue = string.Empty;
                try
                {
                    pgsql.Open();
                    if (material != null && qty==0)
                    {
                        string query = WMSResource.checkmaterial.Replace("#materialid", material);
                        obj = pgsql.QueryFirstOrDefault<materialistModel>(
                           query, null, commandType: CommandType.Text);
                        if(obj==null)
                        {
                             returnvalue = "material does not exists";
                        }
                        else
                        {
                            returnvalue = "true";
                        }
                    }
                   else if(qty!=0 && material==null)
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
                    else if(material!=null && qty!=0)
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
                    if(model.inwmasterid!=null)
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
                string updatequery = WMSResource.updatereprintcount.Replace("#reprinthistoryid",Convert.ToString(data));
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
                            item.enddate= item.enddate.AddDays(1);
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

        public async Task<IEnumerable<FIFOModel>> GetFIFOList()
        {
            using (var pgsql = new NpgsqlConnection(config.PostgresConnectionString))
            {

                try
                {
                    string query = WMSResource.getFIFOList;

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
    }
}
