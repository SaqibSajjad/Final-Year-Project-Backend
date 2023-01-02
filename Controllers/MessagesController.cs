using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiitProjectProgessSystemApi.Models;
using BiitProjectProgessSystemApi.Models.CustomModels;
using System.Web;

namespace BiitProjectProgessSystemApi.Controllers
{
    public class MessagesController : ApiController
    {
        Mybpms db = new Mybpms();


        [HttpGet]
        public HttpResponseMessage getMessages(int sender_id, int receiver_id)
        {

            try
            {

                var messages = db.messages.Where(r => (r.msg_from == sender_id && r.msg_to == receiver_id) || (r.msg_from == receiver_id && r.msg_to == sender_id)).OrderByDescending(m=>m.id).ToList();

                if (messages == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Message Found !");
                }

                List<MessageModel> chat = new List<MessageModel>();
                foreach (message msg in messages) {
                    MessageModel m = new MessageModel();

                    m.id = msg.id;
                    m.sender_id = (int)msg.msg_from;
                    m.sender = msg.user.name;
                    m.receiver_id = (int)msg.msg_to;
                    m.receiver = msg.user1.name;
                    m.description = msg.description;
                    m.time = msg.created_at+"";
                    m.file_path = msg.file_path;

                    chat.Add(m);
                }

                return Request.CreateResponse(HttpStatusCode.OK, chat);



            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage sendMessage(message obj)
        {

            try
            {
                obj.created_at = DateTime.Now;
                db.messages.Add(obj);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "sent");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage sendAttachment(int msg_from, int msg_to, string description)
        {
            try
            {
                var request = HttpContext.Current.Request;

                if (request.Files.Count > 0)
                {
                    for (int i = 0; i < request.Files.Count; i++)
                    {

                        message t = new message();

                        var photo = request.Files[i];
                        photo.SaveAs(HttpContext.Current.Server.MapPath("~/Content/uploaded_tasks/" + photo.FileName));
                        System.IO.FileInfo fi = new System.IO.FileInfo(photo.FileName);
                        // Check if file is there  
                        String ext = fi.Extension;
                        String newname = photo.FileName;
                        t.file_path = "Content/uploaded_tasks/" + newname;
                        t.msg_from = msg_from;
                        t.msg_to = msg_to;
                        t.description = description;
                        t.created_at = DateTime.Now;
                        db.messages.Add(t);
                        db.SaveChanges();
                    }

                    //db.uploaded_tasks.Add(t);
                    //db.SaveChanges();



                    return Request.CreateResponse(HttpStatusCode.OK, "sent");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "File Missing !");

                }
            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}
