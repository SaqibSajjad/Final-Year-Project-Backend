using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiitProjectProgessSystemApi.Models;
using BiitProjectProgessSystemApi.Models.CustomModels;

namespace BiitProjectProgessSystemApi.Controllers
{
    public class RemarksController : ApiController
    {
        Mybpms db = new Mybpms();


        // ********************************************  Supervisor  **********************************
        // ********************************************************************************************

        [HttpGet]
        public HttpResponseMessage getStudentRemarks(int given_by,int given_to)
        {

            try
            {
                var remarks = db.remarks.Where(r =>  r.given_to == given_to).OrderByDescending(r=>r.id).ToList();

                List<Notificatoins> remarks_list = new List<Notificatoins>();
                foreach (remark r in remarks) {
                    Notificatoins obj = new Notificatoins();

                    obj.id = r.id;
                    obj.title = r.title;
                    obj.description = r.description;
                    obj.date = ""+r.created_at;
                    obj.remarks_from = r.user.name;

                    if (r.given_by == given_by || r.is_public==1)
                    {
                        remarks_list.Add(obj);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, remarks_list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getGroupRemarks(int given_by, int group_id)
        {

            try
            {
                var remarks = db.remarks.Where(r => r.given_to==null && r.group_id == group_id ).OrderByDescending(r => r.id).ToList();

                List<Notificatoins> remarks_list = new List<Notificatoins>();
                foreach (remark r in remarks)
                {
                    Notificatoins obj = new Notificatoins();

                    obj.id = r.id;
                    obj.title = r.title;
                    obj.description = r.description;
                    obj.date = "" + r.created_at;
                    obj.remarks_from = r.user.name;

                    if (r.given_by == given_by || r.is_public == 1)
                    {
                        remarks_list.Add(obj);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, remarks_list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage addRemarks(remark obj)
        {
            
            try
            {
                obj.created_at = DateTime.Now;
                db.remarks.Add(obj);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Remarks Added Successfully !");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



    }

    
}
