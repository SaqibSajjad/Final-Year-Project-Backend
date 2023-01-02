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
    public class FypGroupController : ApiController
    {
        Mybpms db = new Mybpms();



        // ***********************************  Project Committee  ************************************
        // ********************************************************************************************

            // remove student from requested_group

        // Group Approval by Project Committee
        [HttpGet]
        public HttpResponseMessage aproveFypGroup(int group_id)
        {
            try
            {

                var group = db.fyp_groups.Where(u => u.id == group_id).First();

                if (group == null) {

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found !");
                }

                fyp_groups updated_group = new fyp_groups();

                updated_group.id = group.id;
                updated_group.name = group.name;
                updated_group.description = group.description;
                updated_group.created_by = group.created_by;
                updated_group.isApproved = 1;
                updated_group.is_fyp_1 = 1;


                db.Entry(group).CurrentValues.SetValues(updated_group);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Group Rejection by Project Committee
        [HttpGet]
        public HttpResponseMessage rejectFypGroup(int group_id)
        {
            try
            {

                var group = db.fyp_groups.Where(u => u.id == group_id).First();

                if (group == null)
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found !");
                }

                fyp_groups updated_group = new fyp_groups();

                updated_group.id = group.id;
                updated_group.name = group.name;
                updated_group.description = group.description;
                updated_group.created_by = group.created_by;
                updated_group.isApproved = -1;


                db.Entry(group).CurrentValues.SetValues(updated_group);
                db.SaveChanges();

                var users_list = db.users.Where(u => u.group_id == group_id).ToList();

                foreach (user user_ in users_list)
                {
                    user updated_user = new user();
                    updated_user.id = user_.id;
                    updated_user.name = user_.name;
                    updated_user.username = user_.username;
                    updated_user.password = user_.password;
                    updated_user.email = user_.email;
                    updated_user.phone = user_.phone;
                    updated_user.address = user_.address;
                    updated_user.platform = user_.platform;
                    updated_user.cgpa = user_.cgpa;
                    updated_user.semester = user_.semester;
                    updated_user.group_id = null;
                    updated_user.role_id = user_.role_id;

                    db.Entry(user_).CurrentValues.SetValues(updated_user);
                    db.SaveChanges();

                }

                return Request.CreateResponse(HttpStatusCode.OK, "Rejected Successfully");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Get FinalGroup Requests at Project Committee End
        [HttpGet]
        public HttpResponseMessage getFinalGroupRequests()
        {
            try
            {
                var groupRequests = db.fyp_groups.Where(u => u.isApproved == 0 || u.isApproved == null).ToList();
                
                List<FypGroup> fyp_groups_list = new List<FypGroup>();
                foreach (fyp_groups group in groupRequests) {
                    FypGroup f_group = new FypGroup();
                    f_group.id = group.id;
                    f_group.name = group.name;
                    f_group.description = group.description;
                    f_group.created_by = group.created_by+"";
                    f_group.isApproved = ""+group.isApproved;
                    fyp_groups_list.Add(f_group);

                }

                return Request.CreateResponse(HttpStatusCode.OK, fyp_groups_list);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Get Groups at Project Commitee end
        [HttpGet]
        public HttpResponseMessage getFinalGroups(int is_fyp_1)
        {
            try
            {

                var group_ids = db.project_allocation.Select(s => new { s.group_id});
                var groupList = db.fyp_groups.Where(d => d.is_fyp_1 == is_fyp_1).Join(group_ids, us => us.id, r => r.group_id, (us, r) =>
            new
                FypGroup
            {

                id = us.id,
                name = us.name,
                description = us.description,
                created_by = us.created_by,
                isApproved = "" + us.isApproved,


            });


            List<FypGroup> fyp_group_list = new List<FypGroup>();

            foreach (FypGroup gp in groupList)
            {
                                        
                var user = db.project_allocation.SingleOrDefault(g => g.group_id == gp.id).user;
                var project = db.project_allocation.SingleOrDefault(p => p.group_id == gp.id).project;
                if (user != null)
                {
                    gp.supervisor_id = user.id;
                    gp.supervisor = user.name;
                }
                if (project != null) {

                        gp.project = project.title;
                }

                    try { gp.group_progress = getGroupProgress(gp.id); }
                    catch (Exception ex) { }

                fyp_group_list.Add(gp);
            }


                return Request.CreateResponse(HttpStatusCode.OK, fyp_group_list);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        // ********************************************  Supervisor  **********************************
        // ********************************************************************************************


        // Get Suprevisor assigned Groups 
        [HttpGet]
        public HttpResponseMessage getGroups(int user_id,int is_fyp_1)
        {
            try
            {
                
                var supervisor = db.users.FirstOrDefault(u => u.id == user_id);

                if (supervisor == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found !");
                }

                var groudp_ids = db.project_allocation.Where(u => u.supervisor_id == user_id && u.isActive==1).Select(s => new {s.group_id });

                var groupList = db.fyp_groups.Where(d => d.is_fyp_1 == is_fyp_1).Join(groudp_ids, u => u.id, r => r.group_id, (u, r) =>
                  new
                      FypGroup
                  {
                      id = u.id,
                      name = u.name,
                      description = u.description,
                      created_by = u.created_by,
                      isApproved = "" + u.isApproved,
                      supervisor = supervisor.name,
                      supervisor_id = supervisor.id,
                      is_fyp_1 = (byte) u.is_fyp_1,
                });

                List<FypGroup> fyp_group_list = new List<FypGroup>();

                foreach (FypGroup gp in groupList)
                {

                    var project = db.project_allocation.SingleOrDefault(g => g.group_id == gp.id).project;
                    if (project != null)
                    {
                        gp.project = project.title;
                    }


                    try { gp.group_progress = getGroupProgress(gp.id); }
                    catch (Exception ex) { }

                    fyp_group_list.Add(gp);
                }

                return Request.CreateResponse(HttpStatusCode.OK, fyp_group_list);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        // ********************************************  Student  **********************************
        // ********************************************************************************************

        // Get Group Requests sent by your fellows => *uid is loginUserId
        [HttpGet]
        public HttpResponseMessage getGroupRequests(int uid)
        {
            try
            {

                var requestedUsers = db.fyp_groups_requests.Where(u => u.requested_to == uid).Select(s => new { id = s.requested_by });

                var users = db.users.Join(requestedUsers, u => u.id, r => r.id, (u, r) =>
                   new
                       User
                   {
                       id = u.id,
                       name = u.name,
                       username = u.username,
                       email = u.email,
                       cgpa = "" + u.cgpa,
                       phone = u.phone,
                       address = u.address,
                       platform = u.platform,
                       group_id = (int) u.group_id,
                   });

                List<User> userList = new List<User>();
                foreach (var u in users)
                {
                    var group = db.fyp_groups.SingleOrDefault(x => x.id == u.group_id && x.isApproved!=1);
                    if (group != null) {
                        userList.Add(
                        new
                      User
                        {
                            id = u.id,
                            name = u.name,
                            username = u.username,
                            email = u.email,
                            cgpa = "" + u.cgpa,
                            phone = u.phone,
                            address = u.address,
                            platform = u.platform,
                            group_id = group.id,
                            group_name = group.name,
                        }
                       );
                    }
                   
                }


                return Request.CreateResponse(HttpStatusCode.OK, userList);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public HttpResponseMessage acceptGroupRequest(int uid,int group_id,int requested_by)
        {
            try
            {

                var original = db.fyp_groups_requests.SingleOrDefault(gp => gp.group_id == group_id && gp.requested_by == requested_by && gp.requested_to == uid);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }

                var user_ = db.users.FirstOrDefault(u => u.id == uid);

                if (user_==null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found !");
                }

                user updated_user = new user();
                updated_user.id = user_.id;
                updated_user.name = user_.name;
                updated_user.username = user_.username;
                updated_user.password = user_.password;
                updated_user.email = user_.email;
                updated_user.phone = user_.phone;
                updated_user.address = user_.address;
                updated_user.platform = user_.platform;
                updated_user.cgpa = user_.cgpa;
                updated_user.semester = user_.semester;
                updated_user.group_id = group_id;
                updated_user.role_id = user_.role_id;



                db.Entry(user_).CurrentValues.SetValues(updated_user);
                db.SaveChanges();

               

                db.Entry(original).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Group Added Successfully !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage rejectGroupRequest(int group_id, int requested_by,int requested_to)
        {
            try
            {

                var original = db.fyp_groups_requests.Find(group_id,requested_by,requested_to);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }

                db.Entry(original).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Request Rejected !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage sendGroupRequest(fyp_groups_requests request)
        {
            try
            {


                db.fyp_groups_requests.Add(request);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,"Request Sent Successfully !" );
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage sendGroupRequestToSupervisor(fyp_groups_requests_supervisor request)
        {
            try
            {
                

                db.fyp_groups_requests_supervisor.Add(request);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Request Sent Successfully !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage createGroup(FypGroup group)
        {
            try
            {
                var user_ = db.users.FirstOrDefault(u => u.id == group.creator_id);


                if (user_ == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not Found !");
                }

                fyp_groups gp = new fyp_groups();

                gp.name = group.name;
                gp.description = group.description;
                gp.created_by = user_.name;
                gp.is_fyp_1 =1;
                gp.created_by_id = group.creator_id;
                if (group.isApproved == "1")
                {
                    gp.isApproved = 1;
                }
                else {
                    gp.isApproved = 0;
                }

                db.fyp_groups.Add(gp);
                db.SaveChanges();

                user updated_user = new user();
                updated_user = user_;
                //updated_user.id = user_.id;
                //updated_user.name = user_.name;
                //updated_user.username = user_.username;
                //updated_user.password = user_.password;
                //updated_user.email = user_.email;
                //updated_user.phone = user_.phone;
                //updated_user.address = user_.address;
                //updated_user.platform = user_.platform;
                //updated_user.cgpa = user_.cgpa;
                //updated_user.semester = user_.semester;
                updated_user.group_id = gp.id;
                //updated_user.role_id = user_.role_id;



                db.Entry(user_).CurrentValues.SetValues(updated_user);
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK,gp.id);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getGroup(int group_id)
        {
            try
            {


                var groupFound = db.fyp_groups.FirstOrDefault(g=>g.id==group_id);

                if (groupFound == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }

                //var creator_id = db.fyp_groups_requests.FirstOrDefault(g => g.group_id == group_id).user.id;
                var allocated_project = db.project_allocation.SingleOrDefault(g => g.group_id == group_id);
                FypGroup group = new FypGroup();

                if (allocated_project == null)
                {

                    group.id = groupFound.id;
                    group.name = groupFound.name;
                    group.description = groupFound.description;
                    group.created_by = groupFound.created_by;
                    group.isApproved = "" + groupFound.isApproved;
                    group.supervisor = "";
                    group.creator_id = (int) groupFound.created_by_id;
        
                    group.is_fyp_1 = (byte) groupFound.is_fyp_1;
                    group.supervisor_id = -1;


                }
                else {

                    var project = allocated_project.project;

                    var supervisor = allocated_project.user;


                    group.id = groupFound.id;
                    group.name = groupFound.name;
                    group.description = groupFound.description;
                    group.created_by = groupFound.created_by;
                    group.isApproved = "" + groupFound.isApproved;
                    group.supervisor = supervisor.name;
                    group.supervisor_id = supervisor.id;
                    group.is_fyp_1 = (byte)groupFound.is_fyp_1;
                    group.project = project.title;
                    group.project_id = project.id;

                }

               

           

                return Request.CreateResponse(HttpStatusCode.OK, group);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage removeFypGroupMembers(int group_id)
        {
            try
            {

                var group = db.fyp_groups.Where(u => u.id == group_id).First();

                if (group == null)
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found !");
                }


                var users_list = db.users.Where(u => u.group_id == group_id).ToList();

                foreach (user user_ in users_list)
                {
                    user updated_user = new user();
                    updated_user.id = user_.id;
                    updated_user.name = user_.name;
                    updated_user.username = user_.username;
                    updated_user.password = user_.password;
                    updated_user.email = user_.email;
                    updated_user.phone = user_.phone;
                    updated_user.address = user_.address;
                    updated_user.platform = user_.platform;
                    updated_user.cgpa = user_.cgpa;
                    updated_user.semester = user_.semester;
                    updated_user.group_id = null;
                    updated_user.role_id = user_.role_id;

                    db.Entry(user_).CurrentValues.SetValues(updated_user);
                    db.SaveChanges();

                }

                return Request.CreateResponse(HttpStatusCode.OK, 1);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        private Double getGroupProgress(int group_id)
        {
            
                Double StudentProgress = 0.0;

                List<User> std_List = new List<User>();
                var student = db.users.Where(s => s.group_id == group_id).ToList();

                if (student != null)
                {               
                    foreach (user u in student)
                    {
                        StudentProgress += getStudentAverage(u);
                    }

                    return ((StudentProgress / (student.Count() * 100)) * 100);
                }

            return StudentProgress;
           
        }

        [HttpGet]
        public HttpResponseMessage getGroupOverallProgress(int group_id)
        {
            try
            {
                Double StudentProgress = 0.0;

                List<User> std_List = new List<User>();
                var student = db.users.Where(s => s.group_id == group_id).ToList();

                if (student == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found !");
                }

                foreach (user u in student)
                {
                    StudentProgress += getStudentAverage(u);

                }
                return Request.CreateResponse(HttpStatusCode.OK, ((StudentProgress / (student.Count() * 100)) * 100));
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private Double getStudentAverage(user u)
        {
            int obtMarks = 0;
            var tasks = db.assigned_tasks.Where(s => s.assigned_to == u.id && (s.status_id == 1 || s.status_id == 2)).ToList();
            if (tasks.Count > 0) {
                foreach (assigned_tasks t in tasks)
                {
                    obtMarks += (int)t.rating;
                }

                return ((obtMarks / (tasks.Count * 5)) * 100);
            }

            return 100; // if task not assigned progress will be 100%

        }

    }

   
}
