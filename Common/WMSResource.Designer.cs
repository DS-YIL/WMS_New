﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMS.Common {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class WMSResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal WMSResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WMS.Common.WMSResource", typeof(WMSResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_stock where materialid=&apos;#materialid&apos; limit 1.
        /// </summary>
        public static string checkmaterial {
            get {
                return ResourceManager.GetString("checkmaterial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_stock where materialid=&apos;#materialid&apos; and availableqty=#availableqty limit 1.
        /// </summary>
        public static string checkmaterialandqty {
            get {
                return ResourceManager.GetString("checkmaterialandqty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_stock where materialid=&apos;#materialid&apos; and createddate&lt;&apos;#createddate&apos;  order by createddate asc limit 1.
        /// </summary>
        public static string checkoldestmaterial {
            get {
                return ResourceManager.GetString("checkoldestmaterial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select distinct* from wms.openpolistview openpo inner join wms.wms_securityinward inw on openpo.pono=inw.pono where openpo.pono=&apos;#pono&apos;  order by receiveddate desc limit 1.
        /// </summary>
        public static string checkponoexists {
            get {
                return ResourceManager.GetString("checkponoexists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_stock where availableqty &gt;=#availableqty limit 1.
        /// </summary>
        public static string checkqty {
            get {
                return ResourceManager.GetString("checkqty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_reprinthistory where .
        /// </summary>
        public static string checkreprintalreadydone {
            get {
                return ResourceManager.GetString("checkreprintalreadydone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_gatepassmaterial set deleteflag=&apos;true&apos; where gatepassmaterialid=#gatepassmaterialid.
        /// </summary>
        public static string deletegatepassmaterial {
            get {
                return ResourceManager.GetString("deletegatepassmaterial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;&lt;meta charset=\&quot;ISO-8859-1\&quot;&gt;&lt;head&gt;&lt;link rel =&apos;stylesheet&apos; href =&apos;https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css&apos;&gt;&lt;/head&gt;
        ///&lt;body&gt;&lt;div class=&apos;container&apos;&gt;
        ///&lt;p&gt;Dear #user,&lt;/p&gt;
        ///&lt;p&gt;#subbody&lt;/p&gt;
        ///&lt;p&gt;
        ///LINK:&lt;/p&gt;
        ///&lt;a href=&quot;http://10.29.15.183:100/WMS/Login&quot;&gt;http://10.29.15.183:100/WMS/Login&lt;/a&gt;
        ///&lt;p style=&apos;margin-bottom:0px;&apos;&gt;Regards,&lt;/p&gt;&lt;p&gt; #sender.&lt;/p&gt;&lt;/div&gt;&lt;/body&gt;&lt;/html&gt;.
        /// </summary>
        public static string emailbody {
            get {
                return ResourceManager.GetString("emailbody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_rd_category where deleteflag=false.
        /// </summary>
        public static string getabccategorydata {
            get {
                return ResourceManager.GetString("getabccategorydata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.v_getdeailsbycategory  where category like &apos;%abcname&apos;.
        /// </summary>
        public static string GetABCdetailsBycategory {
            get {
                return ResourceManager.GetString("GetABCdetailsBycategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Sum(ws.unitprice*ws.availableqty) as totalcost,
        ///    sum(availableqty) as availableqty,( SELECT wrc.categoryname
        ///           FROM wms.wms_rd_category wrc
        ///          WHERE ws.unitprice ::numeric &gt;= wrc.minpricevalue::numeric and
        ///          (ws.unitprice ::numeric &lt;= wrc.maxpricevalue::numeric or ws.unitprice ::numeric = wrc.maxpricevalue::numeric is null) and wrc.deleteflag=false
        ///         LIMIT 1) AS category from wms.wms_stock ws
        /// inner join wms.&quot;MaterialMasterYGS&quot; op on  ws.materialid =op.material [rest of string was truncated]&quot;;.
        /// </summary>
        public static string GetallavlqtyABCList {
            get {
                return ResourceManager.GetString("GetallavlqtyABCList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select pono,vendorname,quotationqty ,deliverydate  from wms.openpolistview.
        /// </summary>
        public static string getASNList {
            get {
                return ResourceManager.GetString("getASNList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select ws.unitprice,ws.materialid,op.materialdescription,
        ///    sum(availableqty) as availableqty,( SELECT wrc.categoryname
        ///           FROM wms.wms_rd_category wrc
        ///          WHERE ws.unitprice ::numeric &gt;= wrc.minpricevalue::numeric and 
        ///          ws.unitprice ::numeric &lt;= wrc.maxpricevalue::numeric  and wrc.deleteflag=false 
        ///         LIMIT 1) AS category from wms.wms_stock ws
        /// inner join wms.&quot;MaterialMasterYGS&quot; op on  ws.materialid =op.material 
        /// WHERE ws.materialid IS NOT null and ws.unitprice is not [rest of string was truncated]&quot;;.
        /// </summary>
        public static string getcategorylist {
            get {
                return ResourceManager.GetString("getcategorylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select itemid, sec.grnnumber , totalquantity,availableqty,totalquantity - availableqty AS issuedqty,itemlocation from
        ///wms.wms_stock ws inner join wms.wms_securityinward sec on sec.pono =ws.pono where ws.materialid=&apos;#materialid&apos;.
        /// </summary>
        public static string getcategorylistbymaterailid {
            get {
                return ResourceManager.GetString("getcategorylistbymaterailid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select sum(mi.issuedqty) as issuedqty,op.&quot;JobName&quot;,req.requestforissueid,emp.&quot;name&quot;,req.requesteddate,sk.materialid,sk.pono,req.requestedquantity,sk.availableqty,req.requestid 
        /// from wms.wms_materialrequest req 
        /// left join wms.wms_materialissue mi on mi.requestforissueid=req.requestforissueid
        /// inner join wms.wms_stock sk on req.requestid=sk.itemid 
        /// inner join wms.employee emp on emp.employeeno=req.requesterid
        /// inner join wms.openpolistview op on op.pono=sk.pono
        /// where requestid=#requestid and req.del [rest of string was truncated]&quot;;.
        /// </summary>
        public static string GetdetailsByrequestid {
            get {
                return ResourceManager.GetString("GetdetailsByrequestid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select distinct inwa.invoiceno,inwa.grnnumber,inwa.pono,inw.materialid, (inw.confirmqty+inw.returnqty) as totalrecivedqty,openpo.materialdescription, openpo.quotationqty,inw.receivedqty,inw.confirmqty,inw.returnqty from wms.wms_securityinward inwa  
        /// left join wms.wms_storeinward inw on inw.inwmasterid=inwa.inwmasterid
        /// inner join wms.openpolistview openpo on openpo.pono=inwa.pono
        ///  where  inwa.pono=&apos;#pono&apos;  and inwa.invoiceno ilike &apos;%#invoiceno&apos; order by inwa.grnnumber desc.
        /// </summary>
        public static string Getdetailsforthreewaymatching {
            get {
                return ResourceManager.GetString("Getdetailsforthreewaymatching", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select itemid,createddate::date,materialid,mat.materialdescription,itemlocation,shelflife,sk.availableqty ,pono from wms.wms_stock sk inner join  wms.&quot;MaterialMasterYGS&quot; mat on mat.material=sk.materialid
        ///where stockstatus=&apos;active&apos; and sk.deleteflag=false and availableqty!=0.
        /// </summary>
        public static string getFIFOList {
            get {
                return ResourceManager.GetString("getFIFOList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select ygs.materialdescription,emp.name,* from wms.wms_gatepass gate
        ///   left join wms.wms_gatepassmaterial mat on gate.gatepassid=mat.gatepassid 
        ///   left join wms.employee emp on emp.employeeno=gate.requestedby
        ///   left join wms.&quot;MaterialMasterYGS&quot; ygs on ygs.material=mat.materialid and mat.deleteflag=false 
        ///   where gate.deleteflag=false  order by gate.gatepassid desc.
        /// </summary>
        public static string getgatepasslist {
            get {
                return ResourceManager.GetString("getgatepasslist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_gatepassmaterial material
        ///      inner join wms.wms_gatepass pass on pass.gatepassid=material.gatepassid 
        ///      inner join wms.employee emp on pass.requestedby=emp.employeeno
        ///      inner join wms.&quot;MaterialMasterYGS&quot; ygs on ygs.material=material.materialid
        ///           where pass.gatepassid=#gatepassid and pass.deleteflag=false --and material.deleteflag=false.
        /// </summary>
        public static string getgatepassmaterialdetailList {
            get {
                return ResourceManager.GetString("getgatepassmaterialdetailList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select grnnumber from wms.wms_securityinward   where pono=&apos;#pono&apos; and grnnumber is not null 
        ///and deleteflag=false order by grndate desc limit 1.
        /// </summary>
        public static string getGRNNo {
            get {
                return ResourceManager.GetString("getGRNNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select inwmasterid from wms.wms_securityinward where grnnumber=&apos;#grnnumber&apos; order by grndate desc limit 1.
        /// </summary>
        public static string getinwardmasterid {
            get {
                return ResourceManager.GetString("getinwardmasterid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select inwmasterid from wms.wms_securityinward where pono=&apos;#pono&apos; and invoiceno=&apos;#invoiceno&apos; limit 1.
        /// </summary>
        public static string getinwmasterid {
            get {
                return ResourceManager.GetString("getinwmasterid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select itemid from wms.wms_stock where materialid=&apos;#materialid&apos; and pono=&apos;#pono&apos; limit 1.
        /// </summary>
        public static string getitemid {
            get {
                return ResourceManager.GetString("getitemid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select distinct ygs.materialdescription,ygs.material,itemlocation,createddate,sk.itemid,availableqty from wms.wms_stock sk inner join wms.&quot;MaterialMasterYGS&quot; ygs on ygs.material=sk.materialid where materialid=&apos;#materialid&apos; and availableqty!=0 and sk.deleteflag=false.
        /// </summary>
        public static string getitemlocationList {
            get {
                return ResourceManager.GetString("getitemlocationList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select req.requestid,req.requesteddate,req.requesterid,po.projectname,emp.&quot;name&quot; from wms.wms_materialrequest req inner join wms.openpolistview po on po.pono=req.pono 
        ///left join wms.employee emp on req.requesterid=emp.employeeno  where req.approverid=&apos;#approverid&apos; group by req.requestid,req.requesteddate,req.requesterid,po.projectname,emp.&quot;name&quot; order by req.requestid desc.
        /// </summary>
        public static string GetListForMaterialRequestByapproverid {
            get {
                return ResourceManager.GetString("GetListForMaterialRequestByapproverid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_materialrequest where requesterid=&apos;#requesterid&apos;.
        /// </summary>
        public static string GetListForMaterialRequestByrequesterid {
            get {
                return ResourceManager.GetString("GetListForMaterialRequestByrequesterid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_stock  stock 
        ///left join wms.wms_rd_bin bins on bins.binid=stock.binid
        ///left join wms.wms_rd_rack rack on rack.rackid=stock.rackid
        ///where itemid=#itemid.
        /// </summary>
        public static string getlocationasresponse {
            get {
                return ResourceManager.GetString("getlocationasresponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select requestid from wms.wms_materialrequest order by requestid desc limit 1.
        /// </summary>
        public static string getnextrequestid {
            get {
                return ResourceManager.GetString("getnextrequestid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select distinct* from wms.wms_storeinward inw inner join wms.wms_securityinward inwmaster on inwmaster.inwmasterid=inw.inwmasterid  
        ///where  inwmaster.pono=&apos;#pono&apos;.
        /// </summary>
        public static string Getponodetailsformaterialissue {
            get {
                return ResourceManager.GetString("Getponodetailsformaterialissue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.wms_gatepass where gatepassid=#gatepassid and deleteflag=false.
        /// </summary>
        public static string getprintdetails {
            get {
                return ResourceManager.GetString("getprintdetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string getreportforcategory {
            get {
                return ResourceManager.GetString("getreportforcategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select requestforissueid  from wms.wms_materialrequest where materialid=&apos;materialid&apos; and pono=&apos;#pono&apos; limit 1.
        /// </summary>
        public static string getrequestforissueid {
            get {
                return ResourceManager.GetString("getrequestforissueid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.v_getAccessList where employeeid=&apos;#employeeid&apos; and roleid =#roleid.
        /// </summary>
        public static string getUserAcessNames {
            get {
                return ResourceManager.GetString("getUserAcessNames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to insert into wms.wms_rd_category(categoryid,categoryname,minpricevalue,maxpricevalue,createdby,createdon,deleteflag,startdate,enddate)values(default,@categoryname,@minpricevalue,@maxpricevalue,@createdby,current_date,false,@startdate,@enddate).
        /// </summary>
        public static string insertABCrange {
            get {
                return ResourceManager.GetString("insertABCrange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to insert into wms.auth_users(authid,employeeid,roleid,createddate,createdby,deleteflag)values(default,@employeeid,@roleid,@createddate,@createdby,@deleteflag)returning authid.
        /// </summary>
        public static string insertAuthUserData {
            get {
                return ResourceManager.GetString("insertAuthUserData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_barcode(barcodeid,barcode,createddate,createdby,deleteflag)VALUES(DEFAULT,@barcode,@createddate,@createdby,@deleteflag)returning barcodeid.
        /// </summary>
        public static string insertbarcodedata {
            get {
                return ResourceManager.GetString("insertbarcodedata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to insert into wms.wms_fifoitemdistribution(fifoid,itemid,materialid,enteredon,pono)values(default,@itemid,@materialid,current_date,@pono).
        /// </summary>
        public static string insertFIFOdata {
            get {
                return ResourceManager.GetString("insertFIFOdata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_storeinward(inwmasterid,receiveddate,receivedby,returnqty,confirmqty,materialid,deleteflag)
        ///VALUES(@inwmasterid,@receiveddate,@receivedby,@returnqty,@confirmqty,@materialid,@deleteflag)returning inwardid.
        /// </summary>
        public static string insertforinvoicequery {
            get {
                return ResourceManager.GetString("insertforinvoicequery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to insert into wms.wms_gatepass(gatepassid, gatepasstype, status, referenceno, vehicleno, requestedby, requestedon,deleteflag,vendorname,print,reasonforgatepass)values(default,@gatepasstype,@status,@referenceno,@vehicleno, @requestedby,@requestedon,@deleteflag,@vendorname,&apos;true&apos;,@reasonforgatepass)returning gatepassid.
        /// </summary>
        public static string insertgatepassdata {
            get {
                return ResourceManager.GetString("insertgatepassdata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to insert into wms.wms_gatepassmaterial(gatepassmaterialid,gatepassid,materialid,quantity,deleteflag,remarks,materialcost,expecteddate,returneddate)values(default,@gatepassid,@materialid,@quantity,@deleteflag,@remarks,@materialcost,@expecteddate,@returneddate).
        /// </summary>
        public static string insertgatepassmaterial {
            get {
                return ResourceManager.GetString("insertgatepassmaterial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_securityinward(inwmasterid,pono,invoiceno,invoicedate,receivedby,receiveddate,deleteflag,departmentid)VALUES(default,@pono,@invoiceno,@invoicedate,@receivedby,@receiveddate,@deleteflag,@departmentid).
        /// </summary>
        public static string insertinvoicedata {
            get {
                return ResourceManager.GetString("insertinvoicedata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_storageallowcationhistorymaster(itemlocation,itemid,createddate,createdby)values(@itemlocation,@itemid,@createddate,@createdby).
        /// </summary>
        public static string insertqueryforlocationhistory {
            get {
                return ResourceManager.GetString("insertqueryforlocationhistory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_qualitycheck(inwardid,quality,qtype,qcdate,qcby,remarks,deleteflag)VALUES(@inwardid,@quality,@qtype,@qcdate,@qcby,@remarks,@deleteflag).
        /// </summary>
        public static string insertqueryforqualitycheck {
            get {
                return ResourceManager.GetString("insertqueryforqualitycheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_trackstatus(pono,status,enteredon,returnqty)VALUES(@pono,&apos;Store Checked&apos;,current_date,@returnqty).
        /// </summary>
        public static string insertqueryforstatusforqty {
            get {
                return ResourceManager.GetString("insertqueryforstatusforqty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_trackstatus(pono,status,enteredon)VALUES(@pono,&apos;In Store&apos;,current_date).
        /// </summary>
        public static string insertqueryforstatuswarehouse {
            get {
                return ResourceManager.GetString("insertqueryforstatuswarehouse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to insert into wms.wms_reprinthistory(reprinthistoryid,gatepassid,reprintedon,reprintedby,reprintcount)values(default,@gatepassid,current_date,@reprintedby,@reprintcount)returning reprinthistoryid.
        /// </summary>
        public static string insertreprintcount {
            get {
                return ResourceManager.GetString("insertreprintcount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_stock(inwmasterid,stockstatus,pono,binid,vendorid,totalquantity,shelflife,availableqty,deleteflag,itemlocation,createddate,createdby)VALUES(@inwmasterid,@stockstatus,@pono,@binid,@vendorid,@totalquantity,@shelflife,@availableqty,@deleteflag,@itemlocation,@createddate,@createdby)returning itemid.
        /// </summary>
        public static string insertstock {
            get {
                return ResourceManager.GetString("insertstock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from  wms.wms_sequencemaster  where  enddate&gt;=current_date and id=1.
        /// </summary>
        public static string lastinsertedgrn {
            get {
                return ResourceManager.GetString("lastinsertedgrn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_materialrequest(requestforissueid,quantity,approveremailid,approverid,pono,materialid,requesterid,requestid,requestedquantity,requesteddate,deleteflag)VALUES(default,@quantity,@approveremailid,@approverid,@pono,@materialid,@requesterid,@requestid,@requestedquantity,current_date,false).
        /// </summary>
        public static string materialquest {
            get {
                return ResourceManager.GetString("materialquest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select distinct * from   wms.wms_stock  sk inner join wms.openpolistview openpo on openpo.material = sk.materialid left join wms.wms_materialrequest req on req.pono = openpo.pono where req.ackstatus is not null and req.deleteflag=false.
        /// </summary>
        public static string materialrequestquery {
            get {
                return ResourceManager.GetString("materialrequestquery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from wms.openpolistview op
        ///      left join wms.wms_trackstatus track on track.pono=op.pono
        ///      where projectmanager=&apos;#projectmanager&apos; and track.enteredon is not null .
        /// </summary>
        public static string openpolist {
            get {
                return ResourceManager.GetString("openpolist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_gatepass set print=false,printedon=current_date,printedby=@printedby where gatepassid=#gatepassid.
        /// </summary>
        public static string printstatusupdate {
            get {
                return ResourceManager.GetString("printstatusupdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select distinct * from wms.wms_securityinward inw
        ///left join wms.wms_storeinward inwa on inw.inwmasterid=inwa.inwmasterid
        ///left join wms.wms_stock stocks on  stocks.inwmasterid=inwa.inwmasterid
        ///inner join wms.openpolistview openpo on openpo.pono=inw.pono
        ///where inw.grnnumber=&apos;#grnnumber&apos;.
        /// </summary>
        public static string queryforitemdetails {
            get {
                return ResourceManager.GetString("queryforitemdetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_gatepass set reprintedon=current_date,reprintedby=@reprintedby,reprintcount=@reprintcount where gatepassid=#gatepassid.
        /// </summary>
        public static string reprintcountupdate {
            get {
                return ResourceManager.GetString("reprintcountupdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO wms.wms_trackstatus(pono,status,enteredon)VALUES(@pono,&apos;Security Checked&apos;,current_date).
        /// </summary>
        public static string statusupdatebySecurity {
            get {
                return ResourceManager.GetString("statusupdatebySecurity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_rd_category set deleteflag=true,updatedby=@updatedby,updatedon=@updatedon.
        /// </summary>
        public static string updateABCrange {
            get {
                return ResourceManager.GetString("updateABCrange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update  wms.wms_materialrequest set ackstatus=@ackstatus,ackremarks=@ackremarks where requestforissueid=@requestforissueid and materialid=@materialid.
        /// </summary>
        public static string updateackstatus {
            get {
                return ResourceManager.GetString("updateackstatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to insert into wms.wms_materialissue(materialissueid,pono,itemid,requestforissueid,itemissueddate,itemreceiverid,deleteflag,itemreturnable,approvedby,approvedon,issuedqty)
        ///values(default,@pono,@itemid,@requestforissueid,@itemissueddate,@itemreceiverid,false,@itemreturnable,@approvedby,@approvedon,@issuedqty).
        /// </summary>
        public static string updateapproverstatus {
            get {
                return ResourceManager.GetString("updateapproverstatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.cyclecountconfig set apercentage=@apercentage,bpercentage=@bpercentage,cpercentage=@cpercentage,cyclecount=@cyclecount,frequency=@frequency where id = #cid.
        /// </summary>
        public static string updatecyclecountconfig {
            get {
                return ResourceManager.GetString("updatecyclecountconfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_gatepass set gatepasstype=@gatepasstype,status=@status,referenceno=@referenceno,vehicleno=@vehicleno,requestedby=@requestedby,vendorname=@vendorname,reasonforgatepass=@reasonforgatepass where gatepassid=#gatepassid.
        /// </summary>
        public static string updategatepass {
            get {
                return ResourceManager.GetString("updategatepass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_gatepass set approverstatus=@approverstatus,approverremarks=@approverremarks,approvedon=@approvedon where gatepassid=#gatepassid.
        /// </summary>
        public static string updategatepassapproverstatus {
            get {
                return ResourceManager.GetString("updategatepassapproverstatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_gatepassmaterial set gatepassid=@gatepassid,materialid=@materialid,quantity=@quantity,remarks=@remarks,materialcost=@materialcost,expecteddate=@expecteddate,returneddate=@returneddate where gatepassmaterialid=#gatepassmaterialid.
        /// </summary>
        public static string updategatepassmaterial {
            get {
                return ResourceManager.GetString("updategatepassmaterial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_securityinward set grnnumber=@grnnumber,grndate=current_date where invoiceno=&apos;#invoiceno&apos; and pono=&apos;#pono&apos;.
        /// </summary>
        public static string updategrnnumber {
            get {
                return ResourceManager.GetString("updategrnnumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_stock set itemlocation=&apos;#itemlocation&apos; where itemid=#itemid.
        /// </summary>
        public static string updatelocation {
            get {
                return ResourceManager.GetString("updatelocation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_stock set availableqty=#availableqty where itemid=#itemid.
        /// </summary>
        public static string updateqtyafterissue {
            get {
                return ResourceManager.GetString("updateqtyafterissue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_reprinthistory
        ///          set reprintcount=@reprintcount where reprinthistoryid=#reprinthistoryid.
        /// </summary>
        public static string updatereprintcount {
            get {
                return ResourceManager.GetString("updatereprintcount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update wms.wms_sequencemaster set sequencenumber=@grnnextsequence where id=@id.
        /// </summary>
        public static string updateseqnumber {
            get {
                return ResourceManager.GetString("updateseqnumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select grnnumber from wms.wms_securityinward inw 
        ///inner join wms.openpolistview openpo on inw.pono=openpo.pono 
        ///where  inw.invoiceno=&apos;#invoiceno&apos; and openpo.pono=&apos;#pono&apos;.
        /// </summary>
        public static string verifyGRNgenerated {
            get {
                return ResourceManager.GetString("verifyGRNgenerated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Count(*),grnnumber from wms.wms_securityinward inw 
        ///inner join wms.openpolistview openpo on inw.pono=openpo.pono 
        ///where  inw.invoiceno=&apos;#invoiceno&apos; and openpo.pono=&apos;#pono&apos; group by grnnumber.
        /// </summary>
        public static string Verifythreewaymatch {
            get {
                return ResourceManager.GetString("Verifythreewaymatch", resourceCulture);
            }
        }
    }
}
