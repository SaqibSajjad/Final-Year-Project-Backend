using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiitProjectProgessSystemApi.Models;
using System.Data;
using BiitProjectProgessSystemApi.Models.CustomModels;
using System.Web;
using System.Web.Http.Cors;


namespace BiitProjectProgessSystemApi.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class UserController : ApiController
    {

        Mybpms db = new Mybpms();
        [HttpPost]
        public HttpResponseMessage LoginStudentLinq(string username, string password)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var user = db.users.FirstOrDefault(u => u.username == username && u.password == password);
                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Username and Password Does not Matched");

                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        // ***********************************  Project Committee  ************************************
        // ********************************************************************************************

        [HttpGet]
        public HttpResponseMessage UserLogin(user us)
        {
            try
            {
                var userFound = db.users.FirstOrDefault(u => u.username == us.username && u.password == us.password);
                if (userFound == null)
                {
                    //return Request.CreateResponse(HttpStatusCode.NotFound, getLoginjsonResponse(0, "Invalid username or password !", 0, 0));
                    return Request.CreateResponse(HttpStatusCode.NotFound,  "Invalid username or password !");

                }

                return Request.CreateResponse(HttpStatusCode.OK, userFound );
                //return Request.CreateResponse(HttpStatusCode.OK, getLoginjsonResponse(1, "Login Successfully !", userFound.id, userFound.role_id.Value));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        //[HttpPost]
        //public HttpResponseMessage updateUser(user u)
        //{
        //    try
        //    {
        //        var original = db.users.FirstOrDefault(s=>s.id==u.id);
        //        if (original == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
        //        }

        //        user updated_user = original;
        //        if (u.name != "")
        //        {
        //            updated_user.name = u.name;
        //        }
        //        if (u.username != "")
        //        {
        //            updated_user.username = u.username;
        //        }
        //        if (u.platform != "")
        //        {
        //            updated_user.platform = u.platform;
        //        }
        //        if (u.email != "")
        //        {
        //            updated_user.email = u.email;
        //        }
        //        if (u.password != "")
        //        {
        //            updated_user.password = u.password;
        //        }
        //        if (u.phone != "")
        //        {
        //            updated_user.phone = u.phone;
        //        }
        //        if (u.address != "")
        //        {
        //            updated_user.address = u.address;
        //        }

        //        db.Entry(original).CurrentValues.SetValues(updated_user);
        //        db.SaveChanges();

        //        return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully !");
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}



        //[HttpGet]
        ////Comment by SaqibSajjad
        //public HttpResponseMessage getCommitteeNotifications(int user_id)
        //{
        //    try
        //    {

        //        var user = db.users.FirstOrDefault(s => s.id == user_id);

        //        if (user == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
        //        }

        //        var meetingsList = db.meetings.Where(m => m.location == "Conference Room").ToList();

        //        List<Notificatoins> notifications = new List<Notificatoins>();

        //        foreach (meeting m in meetingsList)
        //        {
        //            try
        //            {
        //                Notificatoins obj = new Notificatoins();

        //                obj.title = m.title;
        //                obj.meeting_id = m.id;
        //                obj.description = m.meeting_notes;
        //                obj.date = m.meeting_timing;
        //                obj.location = m.location;
        //                if (m.is_fyp_1 == 1)
        //                {
        //                    obj.fyp_group = "FYP-1";
        //                }
        //                else if (m.is_fyp_1 == 0) { obj.fyp_group = "FYP-2"; }

        //                obj.notification_type = "Meeting";

        //                notifications.Add(obj);
        //            }
        //            catch (Exception ex) { }

        //        }

        //        var remarks = db.remarks.Where(m => m.is_public == 1 && m.given_by != user_id).ToList();

        //        foreach (remark m in remarks)
        //        {
        //            try
        //            {
        //                Notificatoins obj = new Notificatoins();

        //                obj.title = m.title;
        //                obj.description = m.description;
        //                obj.date = "" + m.created_at;
        //                obj.remarks_rating = (int)m.rating;
        //                obj.remarks_from = m.user.name;

        //                try
        //                {
        //                    if (m.user1 != null)
        //                    {
        //                        obj.remarks_for = m.user1.name;
        //                        var project_allocation = db.project_allocation.Where(p => p.group_id == m.user1.fyp_groups.id).First();
        //                        if (project_allocation != null)
        //                        {
        //                            obj.project_name = project_allocation.project.title;
        //                        }

        //                        if (m.user1.fyp_groups.is_fyp_1 == 1)
        //                        {
        //                            obj.fyp_group = "FYP-1";
        //                        }
        //                        else if (m.user1.fyp_groups.is_fyp_1 == 0) { obj.fyp_group = "FYP-2"; }


        //                    }



        //                    if (m.given_to == null && m.group_id != null)
        //                    {
        //                        obj.remarks_for = m.fyp_groups.name;
        //                        var project_allocation = db.project_allocation.Where(p => p.group_id == m.group_id).First();
        //                        if (project_allocation != null)
        //                        {
        //                            obj.project_name = project_allocation.project.title;
        //                        }
        //                        if (m.fyp_groups.is_fyp_1 == 1)
        //                        {
        //                            obj.fyp_group = "FYP-1";
        //                        }
        //                        else if (m.fyp_groups.is_fyp_1 == 0) { obj.fyp_group = "FYP-2"; }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {

        //                }

        //                obj.notification_type = "Comment";

        //                if (obj.project_name != null)
        //                    notifications.Add(obj);

        //            }
        //            catch (Exception ex) { }

        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, notifications);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //public HttpResponseMessage getNotAllowedStudents()
        //{
        //    try
        //    {

        //        var students = db.users.Where(u => u.is_allowed_in_meeting == 0).ToList();


        //        List<User> studentList = new List<User>();
        //        foreach (user u in students)
        //        {
        //            User us = new User();
        //            us.id = u.id;
        //            us.name = u.name;
        //            us.username = u.username;
        //            us.email = u.email;
        //            us.phone = u.phone;
        //            us.cgpa = "" + u.cgpa;
        //            us.address = u.address;
        //            us.platform = u.platform;
        //            us.semester = u.semester + "";
        //            us.is_allowed_in_meeting = (u.is_allowed_in_meeting==null) ? (byte) 1 :(byte) u.is_allowed_in_meeting;
        //            if (u.group_id != null) {
        //                us.is_fyp_1 = (byte) u.fyp_groups.is_fyp_1;
        //                us.group_name = u.fyp_groups.name;
        //                us.group_id = (int) u.group_id;

        //                var allocated_project = db.project_allocation.FirstOrDefault(p => p.group_id == u.group_id);
        //                us.project = allocated_project.project.title;

        //                us.supervisor = allocated_project.user.name;
        //            }


        //            studentList.Add(us);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, studentList);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //public HttpResponseMessage allocateProject(user request)
        //{
        //    try
        //    {

        //        var user_ = db.users.FirstOrDefault(u => u.id == request.id);


        //        if (user_ == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "User not found !");
        //        }

        //        user updated_user = new user();
        //        updated_user = user_;
        //        //updated_user.id = user_.id;
        //        //updated_user.name = user_.name;
        //        //updated_user.username = user_.username;
        //        //updated_user.password = user_.password;
        //        //updated_user.email = user_.email;
        //        //updated_user.phone = user_.phone;
        //        //updated_user.address = user_.address;
        //        updated_user.platform = request.platform;
        //        //updated_user.cgpa = user_.cgpa;
        //        //updated_user.semester = user_.semester;
        //        updated_user.group_id = request.group_id;
        //        //updated_user.role_id = user_.role_id;
        //        //updated_user.role_id = user_.role_id;


        //        db.Entry(user_).CurrentValues.SetValues(updated_user);
        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK,1);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}




        //[HttpGet]
        //public HttpResponseMessage getSupervisorList()
        //{
        //    try
        //    {

        //        var supervisors = db.users.Where(u =>u.role_id == 2 || u.role_id==3 || u.role_id == 7 || u.role_id == 8).ToList();


        //        List<User> supervisorList = new List<User>();
        //        foreach (user u in supervisors)
        //        {
        //            User us = new User();
        //            us.id = u.id;
        //            us.name = u.name;
        //            us.username = u.username;
        //            us.email = u.email;
        //            us.phone = u.phone;
        //            us.cgpa = "" + u.cgpa;
        //            us.address = u.address;
        //            us.platform = u.platform;
        //            us.semester = u.semester + "";

        //            supervisorList.Add(us);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, supervisorList);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        [HttpGet]
        public HttpResponseMessage getUnGroupedStudents()
        {
            try
            {

                var students = db.users.Where(u => u.group_id == null && u.role_id == 4).OrderBy(c => c.name).ToList();


                List<User> studentList = new List<User>();
                foreach (user u in students)
                {
                    User us = new User();
                    us.id = u.id;
                    us.name = u.name;
                    us.username = u.username;
                    us.email = u.email;
                    us.phone = u.phone;
                    us.cgpa = "" + u.cgpa;
                    us.address = u.address;
                    us.platform = u.platform;
                    us.semester = u.semester + "";
                   // us.profile_pic = u.profile_pic;

                    studentList.Add(us);
                }

                return Request.CreateResponse(HttpStatusCode.OK, studentList);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        //// Allow Students InMeeting

        //[HttpGet]
        //public HttpResponseMessage allowStudentInMeeting(int user_id,Boolean isAllowed)
        //{
        //    try
        //    {

        //        var student = db.users.FirstOrDefault(u => u.id == user_id);


        //        if (student == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
        //        }

        //        var updated_user = student;
        //        if (isAllowed == true) {
        //            updated_user.is_allowed_in_meeting = 1;
        //        } else {
        //            updated_user.is_allowed_in_meeting = 0;
        //        }


        //        db.Entry(student).CurrentValues.SetValues(updated_user);
        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK, "success");

        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //// Get Members of same group

        //[HttpGet]
        //public HttpResponseMessage getContacts(int user_id,int role_id)
        //{
        //    try
        //    {

        //        var user = db.users.FirstOrDefault(u => u.id == user_id);

        //        if (user == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
        //        }

        //        if (role_id == 1) // if Project Commitee
        //        {


        //            var contacts_list = new List<Contact>();

        //            var contacts_users = db.users.Where(u => (u.role_id == 4 && u.group_id!=null) || u.role_id!=4).ToList();
        //            if (contacts_users.Count() > 0)
        //            {

        //                foreach (user uu in contacts_users)
        //                {

        //                    if (uu.id != user_id)
        //                    {
        //                        Contact cc = new Contact();
        //                        cc.id = uu.id;
        //                        cc.name = uu.name;
        //                        cc.username = uu.username;
        //                        cc.email = uu.email;

        //                        try {
        //                            cc.project = db.project_allocation.Where(p => p.group_id == uu.group_id).First().project.title;
        //                            cc.group_id = (int)uu.group_id;
        //                            cc.group_name = uu.fyp_groups.name;
        //                            cc.is_fyp_1 = (byte)uu.fyp_groups.is_fyp_1;
        //                            cc.profile_pic = uu.profile_pic;
        //                            cc.is_allowed_in_meeting = (uu.is_allowed_in_meeting == null) ? (byte)1 : (byte)uu.is_allowed_in_meeting;
        //                        }
        //                        catch (Exception e) { }
        //                        cc.profile_pic = uu.profile_pic;
        //                        contacts_list.Add(cc);
        //                    }

        //                }
        //            }

        //            foreach (Contact c in contacts_list)
        //            {
        //                var msg = db.messages.Where(m => (m.msg_from == user_id && m.msg_to == c.id) || (m.msg_from == c.id && m.msg_to == user_id)).OrderByDescending(m => m.id).ToList();
        //                if (msg.Count() > 0)
        //                {

        //                    c.message = msg[0].description;
        //                    c.message_time = msg[0].created_at + "";

        //                    if (msg[0].description == null && msg[0].file_path != null)
        //                    {
        //                        c.message = "@attachment";
        //                    }

        //                }

        //            }

        //            return Request.CreateResponse(HttpStatusCode.OK, contacts_list.OrderBy(c => c.name));

        //        }
        //        else if (role_id == 5) // if Documentation Committee
        //        {


        //            var contacts_list = new List<Contact>();

        //            var contacts_users = db.users.Where(u => (u.role_id == 4 && u.group_id != null) || u.role_id != 4).ToList();
        //            if (contacts_users.Count() > 0)
        //            {

        //                foreach (user uu in contacts_users)
        //                {
        //                    int is_fyp_1 = 1;
        //                    if (uu.role_id == 4)
        //                    {
        //                        is_fyp_1 = (int) db.fyp_groups.FirstOrDefault(g => g.id == uu.group_id).is_fyp_1;
        //                    }
        //                    if (uu.id != user_id && is_fyp_1==1)
        //                    {
        //                        Contact cc = new Contact();
        //                        cc.id = uu.id;
        //                        cc.name = uu.name;
        //                        cc.username = uu.username;
        //                        cc.email = uu.email;
        //                        cc.profile_pic = uu.profile_pic;


        //                        if (uu.role_id == 4)
        //                        {
        //                            try
        //                            {
        //                                cc.project = db.project_allocation.Where(p => p.group_id == uu.group_id).First().project.title;
        //                                cc.group_id = (int)uu.group_id;
        //                                cc.group_name = uu.fyp_groups.name;
        //                                cc.is_fyp_1 = (byte)uu.fyp_groups.is_fyp_1;
        //                                cc.is_allowed_in_meeting = (uu.is_allowed_in_meeting == null) ? (byte)1 : (byte)uu.is_allowed_in_meeting;
        //                            }
        //                            catch (Exception e) { }
        //                        }
        //                        else {
        //                            cc.project = uu.role.title;
        //                            cc.group_name = uu.role.title;
        //                        }
        //                        cc.profile_pic = uu.profile_pic;
        //                        contacts_list.Add(cc);
        //                    }

        //                }
        //            }

        //            foreach (Contact c in contacts_list)
        //            {
        //                var msg = db.messages.Where(m => (m.msg_from == user_id && m.msg_to == c.id) || (m.msg_from == c.id && m.msg_to == user_id)).OrderByDescending(m => m.id).ToList();
        //                if (msg.Count() > 0)
        //                {

        //                    c.message = msg[0].description;
        //                    c.message_time = msg[0].created_at + "";

        //                    if (msg[0].description == null && msg[0].file_path != null)
        //                    {
        //                        c.message = "@attachment";
        //                    }

        //                }

        //            }

        //            return Request.CreateResponse(HttpStatusCode.OK, contacts_list.OrderBy(c => c.name));

        //        }
        //        else if (role_id == 2) // if supervisor
        //        { 
        //            var group_ids = db.project_allocation.Where(s => s.supervisor_id == user_id).Select(s => new { s.group_id });

        //            if (group_ids == null) {
        //                return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
        //            }
        //            var contacts = db.users.Join(group_ids, u => u.group_id, r => r.group_id, (u, r) =>
        //            new
        //                Contact
        //            {
        //                id = u.id,
        //                name = u.name,
        //                username = u.username,
        //                email = u.email,
        //                platform = u.platform,
        //                group_id = (int) u.group_id,
        //                group_name = u.fyp_groups.name,
        //                is_fyp_1 = (byte) u.fyp_groups.is_fyp_1,
        //                is_allowed_in_meeting = (u.is_allowed_in_meeting == null) ? (byte)1 : (byte)u.is_allowed_in_meeting,


        //        });


        //            var contacts_list = new List<Contact>();

        //            var project_committee_members = db.users.Where(u => u.role_id != 4).ToList();
        //            if (project_committee_members.Count() > 0) {

        //                foreach (user uu in project_committee_members) {

        //                    if (uu.id != user_id)
        //                    {
        //                        Contact cc = new Contact();
        //                        cc.id = uu.id;
        //                        cc.name = uu.name;
        //                        cc.username = uu.username;
        //                        cc.email = uu.email;
        //                        cc.project = uu.role.title;
        //                        cc.group_name = uu.role.title;
        //                        cc.profile_pic = uu.profile_pic;
        //                        contacts_list.Add(cc);
        //                    }

        //                }
        //            }

        //            foreach (Contact c in contacts) {
        //                var msg = db.messages.Where(m => (m.msg_from == user_id && m.msg_to == c.id) || (m.msg_from == c.id && m.msg_to == user_id)).OrderByDescending(m=>m.id).ToList();
        //                if (msg.Count()>0)
        //                {

        //                    c.message = msg[0].description;
        //                    c.message_time = msg[0].created_at + "";

        //                    if (msg[0].description == null && msg[0].file_path!=null) {
        //                        c.message = "@attachment";
        //                    }                    

        //                }

        //                c.project = db.project_allocation.Where(p => p.group_id == c.group_id).First().project.title;

        //                contacts_list.Add(c);
        //            }

        //            return Request.CreateResponse(HttpStatusCode.OK, contacts_list.OrderBy(c => c.name));

        //        }
        //        else if (role_id == 4) // if student
        //        {

        //            var group = db.fyp_groups.FirstOrDefault(u => u.id==user.group_id);

        //            if (group == null)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
        //            }


        //            if (group.isApproved == 1)
        //            {

        //                var users = db.users.Where(u => u.group_id == group.id).ToList();

        //                var contacts = new List<Contact>();
        //                foreach (user u in users)
        //                {
        //                    if (u.id != user_id)  // except current user
        //                    {
        //                        Contact c = new Contact();
        //                        c.id = u.id;
        //                        c.name = u.name;
        //                        c.username = u.username;
        //                        c.email = u.email;
        //                        c.platform = u.platform;
        //                        c.group_id = (int)u.group_id;
        //                        try {
        //                            c.project = db.project_allocation.FirstOrDefault(p => p.group_id == u.group_id).project.title;
        //                        }
        //                        catch (Exception e) { }
        //                        c.group_name = u.fyp_groups.name;
        //                        c.is_fyp_1 = (byte)u.fyp_groups.is_fyp_1;
        //                        c.profile_pic = u.profile_pic;
        //                        c.is_allowed_in_meeting = (byte)(u.is_allowed_in_meeting == null ? 0 : u.is_allowed_in_meeting);

        //                        contacts.Add(c);
        //                    }

        //                }

        //                var supervisor = db.project_allocation.FirstOrDefault(p => p.group_id == group.id).user;
        //                if (supervisor != null) {
        //                    Contact cc = new Contact();
        //                    cc.id = supervisor.id;
        //                    cc.name = supervisor.name;
        //                    cc.username = supervisor.username;
        //                    cc.email = supervisor.email;
        //                    cc.group_id = (int)user.group_id;
        //                    try
        //                    {
        //                        cc.project = db.project_allocation.FirstOrDefault(p => p.group_id == user.group_id).project.title;
        //                    }
        //                    catch (Exception e) { }
        //                    cc.group_name = user.fyp_groups.name;
        //                    cc.is_fyp_1 = (byte)user.fyp_groups.is_fyp_1;
        //                    cc.profile_pic = supervisor.profile_pic;
        //                    contacts.Add(cc);
        //                }

        //                var faculty_memnbers = db.users.Where(u => u.role_id != 4 && u.role_id != 2).ToList();
        //                if (faculty_memnbers.Count() > 0)
        //                {

        //                    foreach (user uu in faculty_memnbers)
        //                    {

        //                        if (uu.id != user_id)
        //                        {
        //                            Contact cc = new Contact();
        //                            cc.id = uu.id;
        //                            cc.name = uu.name;
        //                            cc.username = uu.username;
        //                            cc.email = uu.email;
        //                            cc.project = uu.role.title;
        //                            cc.group_name = uu.role.title;
        //                            cc.profile_pic = uu.profile_pic;
        //                            contacts.Add(cc);
        //                        }

        //                    }
        //                }


        //                var contacts_list = new List<Contact>();

        //                foreach (Contact c in contacts)
        //                {
        //                    var msg = db.messages.Where(m => (m.msg_from == user_id && m.msg_to == c.id) || (m.msg_from == c.id && m.msg_to == user_id)).OrderByDescending(m => m.id).ToList();
        //                    if (msg.Count() > 0)
        //                    {

        //                        c.message = msg[0].description;
        //                        c.message_time = msg[0].created_at + "";

        //                        if (msg[0].description == null && msg[0].file_path != null)
        //                        {
        //                            c.message = "@attachment";
        //                        }

        //                    }
        //                    try {
        //                        c.project = db.project_allocation.Where(p => p.group_id == c.group_id).First().project.title;
        //                    }
        //                    catch (Exception e) {}

        //                    contacts_list.Add(c);
        //                }

        //                return Request.CreateResponse(HttpStatusCode.OK, contacts_list.OrderBy(c => c.name));

        //                //return Request.CreateResponse(HttpStatusCode.OK, contacts);

        //            }
        //            else {
        //                return Request.CreateResponse(HttpStatusCode.OK, "Record Not Found");}
        //        }

        //        return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found");


        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //// Get Members of same group

        [HttpGet]
        public HttpResponseMessage getGroupMembers(int group_id)
        {
            try
            {

                var students = db.users.Where(u => u.group_id == group_id && u.role_id == 4).ToList();


                List<User> studentList = new List<User>();
                foreach (user u in students)
                {
                    User us = new User();
                    us.id = u.id;
                    us.name = u.name;
                    us.username = u.username;
                    us.email = u.email;
                    us.phone = u.phone;
                    us.cgpa = "" + u.cgpa;
                    us.address = u.address;
                    us.platform = u.platform;
                    us.group_id = (int)u.group_id;
                    us.profile_pic = u.Pic;
                    us.fyp_1_final_grade = u.fyp_1_final_grade;
                    us.fyp_2_final_grade = u.fyp_2_final_grade;
                    if (u.is_allowed_in_meeting == null)
                    {
                        us.is_allowed_in_meeting = 1;
                    }
                    else
                    {
                        us.is_allowed_in_meeting = (byte)u.is_allowed_in_meeting;
                    }


                    studentList.Add(us);
                }

                return Request.CreateResponse(HttpStatusCode.OK, studentList);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //// Get those Members to whom request can be sent

        //[HttpGet]
        //public HttpResponseMessage getMembersForGroup(int loginUserID)
        //{
        //    try
        //    {
        //       var totalusers = db.users.Where(s=>s.role_id==4 && s.id!=loginUserID && s.group_id==null).Select(s => new { s.id});
        //       var requestedusers=  db.fyp_groups_requests.Where(s=>s.requested_by== loginUserID).Select(s=>new { id = s.requested_to });
        //       var remainusers= totalusers.Except(requestedusers);


        //        var students = db.users.OrderBy(u => u.name).Join(remainusers, u => u.id, r => r.id, (u, r) =>
        //        new
        //            User{
        //            id=u.id,
        //            name = u.name,
        //            username = u.username,
        //            email = u.email,
        //            cgpa = ""+u.cgpa,
        //            phone = u.phone,
        //            address = u.address,
        //            platform = u.platform,
        //        });

        //        //List<User> studentList = new List<User>();
        //        //foreach(user u in students) {
        //        //    User us = new User();
        //        //    us.id = u.id;
        //        //    us.name = u.name;
        //        //    us.username = u.username;       
        //        //    us.email = u.email;
        //        //    us.phone = u.phone;
        //        //    us.cgpa = (float) u.cgpa;
        //        //    us.address = u.address;
        //        //    us.platform = u.platform;

        //        //    studentList.Add(us);
        //        //}

        //        return Request.CreateResponse(HttpStatusCode.OK, students);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //public HttpResponseMessage uploadProfilePic(int user_id)
        //{
        //    try
        //    {
        //        var user = db.users.FirstOrDefault(u=>u.id==user_id);

        //        if (user == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotImplemented, "Record Not Saved");
        //        }

        //        var request = HttpContext.Current.Request;

        //        if (request.Files.Count > 0)
        //        {
        //            for (int i = 0; i < request.Files.Count; i++)
        //            {

        //                user updated_user = user;

        //                var photo = request.Files[i];
        //                photo.SaveAs(HttpContext.Current.Server.MapPath("~/Content/uploaded_tasks/" + photo.FileName));
        //                System.IO.FileInfo fi = new System.IO.FileInfo(photo.FileName);
        //                // Check if file is there  
        //                String ext = fi.Extension;
        //                String newname = photo.FileName;
        //                updated_user.profile_pic = "Content/uploaded_tasks/" + newname;
        //                db.Entry(user).CurrentValues.SetValues(updated_user);
        //                db.SaveChanges();
        //            }

        //            //db.uploaded_tasks.Add(t);
        //            //db.SaveChanges();



        //            return Request.CreateResponse(HttpStatusCode.OK, "Profile Photo Updated Successfully!");
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, "Task File Missing !");

        //        }
        //    }

        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //public HttpResponseMessage getUser(user us)
        //{
        //    try
        //    {
        //        var userFound = db.users.FirstOrDefault(u => u.id == us.id);
        //        if (userFound == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "");
        //        }


        //        return Request.CreateResponse(HttpStatusCode.OK, getUserjsonResponse(1,"User Found !", userFound));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //public HttpResponseMessage getUser(int user_id)
        //{
        //    try
        //    {

        //        var u = db.users.FirstOrDefault(use => use.id == user_id);
        //        if (u == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "");
        //        }

        //        User us = new User();
        //        us.id = u.id;
        //        us.name = u.name;
        //        us.username = u.username;
        //        us.email = u.email;
        //        us.phone = u.phone;
        //        us.cgpa = "" + u.cgpa;
        //        us.address = u.address;
        //        us.platform = u.platform;
        //        us.semester = u.semester + "";
        //        us.is_allowed_in_meeting =  (u.is_allowed_in_meeting==null)? (byte) 1: (byte) u.is_allowed_in_meeting;
        //        us.profile_pic = u.profile_pic;


        //        return Request.CreateResponse(HttpStatusCode.OK, us);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}




        private DataTable getLoginjsonResponse(int status,String message,int student_id,int role_id) {

            DataTable dt = new DataTable();

            dt.Columns.Add("status");
            dt.Columns.Add("message");
            dt.Columns.Add("role_id");
            dt.Columns.Add("user_id");


            dt.Rows.Add(status, message,role_id, student_id);

            return dt;
        }



        private DataTable getUserjsonResponse(int status, String message, user obj)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("status");
            dt.Columns.Add("message");
            dt.Columns.Add("user", typeof(DataTable));

            DataTable dt_1 = new DataTable();
            dt_1.Columns.Add("id");
            dt_1.Columns.Add("name");
            dt_1.Columns.Add("email");
            dt_1.Columns.Add("username");   
            dt_1.Columns.Add("platform");
            dt_1.Columns.Add("group_id");
            dt_1.Columns.Add("role_id");
            dt_1.Columns.Add("phone");
            dt_1.Columns.Add("address");
            dt_1.Columns.Add("is_allowed_in_meeting");
            dt_1.Columns.Add("Pic");

            dt_1.Rows.Add(obj.id,obj.name,obj.email,obj.username,obj.platform,obj.group_id,obj.role_id,obj.phone,obj.address,obj.is_allowed_in_meeting,obj.Pic);
            dt.Rows.Add(status, message, dt_1);

            return dt;
        }

      //  [HttpGet]
        //public HttpResponseMessage getStudentGrading(int s_id)
        //{
        //    try
        //    {

        //        FYP_1_Grading std_grading;

        //        std_grading = db.FYP_1_Grading.FirstOrDefault(u => u.std_id == s_id);
        //        if (std_grading == null) {
        //            std_grading = new FYP_1_Grading();
        //            std_grading.std_id = s_id;
        //            std_grading.final_grade = "A";

        //            db.FYP_1_Grading.Add(std_grading);
        //            db.SaveChanges();

        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, std_grading.final_grade);

        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //public HttpResponseMessage updateStudentGrade(int user_id,int role_id,int student_id,String grade)
        //{
        //    try
        //    {

        //        var student = db.users.FirstOrDefault(s => s.id == student_id);

        //        if (student == null) {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
        //        }

        //        var is_fyp_1 = student.fyp_groups.is_fyp_1;

        //        if (is_fyp_1 == 0) // student is in fyp_2
        //        {                    
        //            user updated_u = student;
        //            if (grade == "F")
        //            {

        //                updated_u.group_id = null;
        //                updated_u.fyp_1_final_grade = null;
        //                updated_u.fyp_2_final_grade = null;
        //                db.Entry(student).CurrentValues.SetValues(updated_u);
        //                db.SaveChanges();

        //            }
        //            else {
        //                updated_u.fyp_2_final_grade = grade;
        //                db.Entry(student).CurrentValues.SetValues(updated_u);
        //                db.SaveChanges();
        //            }
                    

        //            return Request.CreateResponse(HttpStatusCode.OK, "success");
        //        }
                

        //        var original = db.FYP_1_Grading.FirstOrDefault(g=> g.std_id==student_id);                
        //        var fyp_grading = original;
        //        if (original == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
        //        }

                

        //        if (role_id == 1) //Project Committee
        //        {
        //            fyp_grading.proj_comm_grade = grade;
        //        }
        //        else if (role_id == 2) //Supervisor
        //        {
        //            fyp_grading.supervisor_grade = grade;
        //        }
        //        else if (role_id == 5) //Documentation Committee
        //        {
        //            fyp_grading.document_comm_grade = grade;
        //        }



        //        String f_grade = getFinalGrade(original);
        //        fyp_grading.final_grade = f_grade;

        //        db.Entry(original).CurrentValues.SetValues(fyp_grading);
        //        db.SaveChanges();

        //        if (fyp_grading.proj_comm_grade != null && fyp_grading.supervisor_grade != null && fyp_grading.document_comm_grade != null) {

        //            var user = db.users.FirstOrDefault(s => s.id == student_id);
        //            if (user != null) {

        //                user updated_u = user;

        //                if (f_grade == "F")
        //                {
        //                    updated_u = user;
        //                    updated_u.group_id = null;
        //                    db.Entry(user).CurrentValues.SetValues(updated_u);
        //                    db.SaveChanges();
        //                }
        //                else {
        //                    updated_u = user;
        //                    updated_u.fyp_1_final_grade = f_grade;
        //                    db.Entry(user).CurrentValues.SetValues(updated_u);
        //                    db.SaveChanges();
        //                }

                        

        //                var group = db.fyp_groups.FirstOrDefault(g => g.id == user.group_id);

        //                if (group != null) {

        //                    if (shouldPromoteGroupToFYP2(group.id))
        //                    {
        //                        fyp_groups u_group = group;
        //                        u_group.is_fyp_1 = 0;
        //                        db.Entry(group).CurrentValues.SetValues(u_group);
        //                        db.SaveChanges();
        //                    }
                           

        //                }


        //            }
        //        }


        //        return Request.CreateResponse(HttpStatusCode.OK, "success");

        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        private Boolean shouldPromoteGroupToFYP2(int group_id) {

            var students = db.users.Where(u => u.group_id == group_id && u.role_id == 4).ToList();

            Boolean shouldPromote = true;
            foreach (user u in students)
            {
                if (u.fyp_1_final_grade == null) {
                    shouldPromote = false;
                }
            }

            return shouldPromote;

        }

        private String getFinalGrade(FYP_1_Grading grade) {

            String pc_grade_ = "A";
            String sup_grade_ = "A";
            String dc_grade_ = "A";
           
            pc_grade_ = grade.proj_comm_grade;  
            sup_grade_ = grade.supervisor_grade; 
            dc_grade_ = grade.document_comm_grade;

            

            int final_marks = (getGradeMarks(pc_grade_) + getGradeMarks(sup_grade_) + getGradeMarks(dc_grade_))/3;

            return calculateGrade(final_marks);
        }

        private String calculateGrade(int marks)
        {

            String grade = "A";
            if (marks>=0 && marks < 25)
            {
                grade = "F";
            }
            else if (marks >=25 && marks < 50)
            {
                grade = "D";
            }
            else if (marks >= 50 && marks < 75)
            {
                grade = "C";
            }
            else if (marks >= 75 && marks < 100)
            {
                grade = "B";
            }
            else if (marks >= 100)
            {
                grade = "A";
            }

            return grade;
        }

        private int getGradeMarks(String grade) {

            int marks = 100;
            if (grade == "A")
            {
                marks = 100;
            }
            else if (grade == "B")
            {
                marks = 75;
            }
            else if (grade == "C")
            {
                marks = 50;
            }
            else if (grade == "D")
            {
                marks = 25;
            }
            else if (grade == "F")
            {
                marks = 0;
            }

            return marks;
        }


    }
}
