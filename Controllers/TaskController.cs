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
    public class TaskController : ApiController
    {

        Mybpms db = new Mybpms();

        // *************************************** Supervisor ******************************************
        // ********************************************************************************************

        [HttpGet]
        public HttpResponseMessage getSubmittedTaskFiles(int student_id, int supervisor_id, int task_id)
        {
            try
            {

                var task_files = db.uploaded_tasks.Where(s => s.student_id==student_id && s.supervisor_id==supervisor_id && s.task_id==task_id).ToList();

                if (task_files == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }



                List<String> task_files_list = new List<String>();
                foreach (uploaded_tasks task in task_files)
                {
                    task_files_list.Add(task.file_path.Trim('.'));
                }


                return Request.CreateResponse(HttpStatusCode.OK, task_files_list);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        public HttpResponseMessage checktaskCounter(int user_id, int is_fyp_1)
        {
            try
            {

                var tasks_id = db.tasks.Where(s => s.is_fyp_1 == is_fyp_1).Select(s => new { s.id });

                var tasks = db.assigned_tasks.Where(s => s.assigned_to == user_id).Join(tasks_id, u => u.task_id, r => r.id, (u, r) =>
                    new
                        TaskModel
                    {
                        id = u.id,
                        status_id = (int)u.status_id,
                    });

                int completed= 0;
                int not_completed = 0;
                int pending = 0;

                completed = tasks.Where(t => t.status_id == 1).Count();
                not_completed = tasks.Where(t => t.status_id == 2).Count();
                pending = tasks.Where(t => t.status_id == 3).Count();


                if (completed  == 0 && not_completed==0 && pending==0) {
                    return Request.CreateResponse(HttpStatusCode.OK, 0);

                }


                return Request.CreateResponse(HttpStatusCode.OK,1);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getTasksCounter(int user_id,int is_fyp_1)
        {
            try
            {

                var tasks_id = db.tasks.Where(s => s.is_fyp_1 == is_fyp_1).Select(s => new { s.id });

                var tasks = db.assigned_tasks.Where(s=>s.assigned_to==user_id).Join(tasks_id, u => u.task_id, r => r.id, (u, r) =>
                new
                    TaskModel
                {
                    id = u.id,
                    status_id =(int) u.status_id,                  
                });


                var t_couters_data = new List<TaskCounter>();



                t_couters_data.Add(getTaskCounter("Completed", "#51a855", "#7F7F7F", 12, tasks.Where(t => t.status_id == 1).Count()));
                t_couters_data.Add(getTaskCounter("Not Completed", "#e85e6d", "#7F7F7F", 12, tasks.Where(t => t.status_id == 2).Count()));
                t_couters_data.Add(getTaskCounter("Pending", "#efA757", "#7F7F7F", 12, tasks.Where(t => t.status_id == 3).Count()));


                return Request.CreateResponse(HttpStatusCode.OK, t_couters_data);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private TaskCounter getTaskCounter(String name,String color, String legendColor, int font_size, int population) {
            TaskCounter t_counter = new TaskCounter();

            t_counter.name = name;
            t_counter.color = color;
            t_counter.legendFontColor = legendColor;
            t_counter.legendFontSize = font_size;
            t_counter.population = population;

            return t_counter;
        }

        [HttpPost]
        public HttpResponseMessage markTaskDone(TaskModel task)
        {

            try
            {
                var original = db.assigned_tasks.FirstOrDefault(t=>t.task_id==task.id && t.assigned_by == task.assigned_by_id && t.assigned_to== task.assigned_to);
                //return Request.CreateResponse(HttpStatusCode.NotFound,original.user1.name);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }

                assigned_tasks tk = new assigned_tasks();
                tk = original;
                //tk.task_id = original.task_id;
                //tk.assigned_by = original.assigned_by;
                //tk.assigned_to = original.assigned_to;
                tk.rating = task.rating;
                tk.status_id = 1; // completed
                if (task.rating < 2) {
                    tk.status_id = 2; //not completed
                }
               

                db.Entry(original).CurrentValues.SetValues(tk);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getSuperviorTasks(int user_id,int student_id,int is_fyp_1)
        {
            try
            {

                var user = db.users.FirstOrDefault(s => s.id == user_id);

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }


                var assigned_tasks = db.assigned_tasks.Where(t =>t.assigned_by== user_id && t.assigned_to == student_id).OrderByDescending(f=>f.id);

                List<TaskModel> tasks_list = new List<TaskModel>();
                foreach (assigned_tasks a_task in assigned_tasks)
                {

                    var t = db.tasks.FirstOrDefault(tk => tk.id == a_task.task_id && tk.is_fyp_1==is_fyp_1);             
                    var assigned_by = a_task.user;
                    var authority_role = a_task.user.role;

                    if (t != null)
                    {

                        TaskModel task_item = new TaskModel();

                        task_item.id = t.id;
                        task_item.title = t.title;
                        task_item.description = t.description;
                        task_item.task_deadline = t.task_deadline;
                        task_item.isFinalTask = (byte)t.isFinalTask;
                        task_item.assigned_by = assigned_by.name;
                        task_item.authority_role = authority_role.title;
                        task_item.assigned_to = a_task.assigned_to;
                        task_item.rating = (int)a_task.rating;
                        task_item.status = a_task.status.name;
                        task_item.assigned_by_id = a_task.assigned_by;

                        tasks_list.Add(task_item);
                    }

                }


                return Request.CreateResponse(HttpStatusCode.OK, tasks_list);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        [HttpPost]
        public HttpResponseMessage createTask(AssignTask task) {
            try
            {
                var fyp_group = db.fyp_groups.Where(f => f.id == task.group_id ).First();

                if (fyp_group == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotImplemented, "Record Not Saved");
                }

                var is_fyp_1 = 0;
                if (fyp_group.is_fyp_1 == 1 || fyp_group.is_fyp_1 == null) {
                    is_fyp_1 = 1;
                }
                task t = new task();
                t.title = task.title;
                t.description = task.description;
                t.task_deadline = task.task_deadline;
                t.isFinalTask = task.isFinalTask;
                t.is_fyp_1 = (byte) is_fyp_1;

                db.tasks.Add(t);
                db.SaveChanges();

                

                return Request.CreateResponse(HttpStatusCode.OK, t.id);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage uploadTask(int task_id,int student_id,int supervisor_id)
        {
            try
            {
                var task_found = db.assigned_tasks.Where(f => f.task_id ==task_id && f.assigned_to== student_id && f.status_id==3).First(); // penidng task

                if (task_found == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotImplemented, "Record Not Saved");
                }                

                var request = HttpContext.Current.Request;

                if (request.Files.Count > 0) {
                    for (int i = 0; i < request.Files.Count; i++)
                    {

                        uploaded_tasks t = new uploaded_tasks();

                        var photo = request.Files[i];
                        photo.SaveAs(HttpContext.Current.Server.MapPath("~/Content/uploaded_tasks/" + photo.FileName));
                        System.IO.FileInfo fi = new System.IO.FileInfo(photo.FileName);
                        // Check if file is there  
                        String ext = fi.Extension;
                        String newname = photo.FileName;
                        t.file_path = "Content/uploaded_tasks/"+newname;
                        t.task_id = task_id;
                        t.student_id = student_id;
                        t.supervisor_id = supervisor_id;
                        db.uploaded_tasks.Add(t);
                        db.SaveChanges();
                    }

                    //db.uploaded_tasks.Add(t);
                    //db.SaveChanges();



                    return Request.CreateResponse(HttpStatusCode.OK, "Task Successfully Uploaded !");
                }else {
                    return Request.CreateResponse(HttpStatusCode.OK, "Task File Missing !");

                }
            }
               
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage assignTask(AssignTask task) {
            try
            {

                task task_exist = db.tasks.FirstOrDefault(u => u.id == task.id);

                if (task_exist == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotImplemented, "Task Not Saved");
                }

        
                assigned_tasks t1 = new assigned_tasks();

                t1.task_id = task.id;
                t1.assigned_by = task.assigned_by;
                t1.assigned_to = task.assigned_to;
                t1.rating = 0;
                t1.status_id = 3;

                db.assigned_tasks.Add(t1);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Task Assigned Successfully !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        // ***************************************  Student  ******************************************
        // ********************************************************************************************

        [HttpGet]
        public HttpResponseMessage getStudentTasks(int user_id, int is_fyp_1)
        {
            try
            {

                var student = db.users.FirstOrDefault(s => s.id == user_id && s.role_id==4);

                if (student == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }


                var assigned_tasks = db.assigned_tasks.Where(t=>t.assigned_to==user_id);

                List<TaskModel> tasks_list = new List<TaskModel>();
                foreach(assigned_tasks a_task in assigned_tasks) {

                    var t = db.tasks.FirstOrDefault(tk => tk.id == a_task.task_id && tk.is_fyp_1 == is_fyp_1);                   
                    var assigned_by = a_task.user;
                    var authority_role = a_task.user.role;

                    if (t != null) {

                        TaskModel task_item =new TaskModel();

                        task_item.id = t.id;
                        task_item.title = t.title;
                        task_item.description = t.description;
                        task_item.task_deadline = t.task_deadline;
                        task_item.isFinalTask = (byte) t.isFinalTask;
                        task_item.assigned_by = assigned_by.name;
                        task_item.assigned_by_id = a_task.assigned_by;
                        task_item.authority_role = authority_role.title;
                        task_item.assigned_to = a_task.assigned_to;
                        task_item.rating = (int) a_task.rating;
                        task_item.status = a_task.status.name;
                        task_item.isFinalTask = (byte)(t.isFinalTask==null ? 0: t.isFinalTask);

                        tasks_list.Add(task_item);
                    }

                }
                

                return Request.CreateResponse(HttpStatusCode.OK, tasks_list);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage deleteTask(int task_id)
        {

            try
            {
                var original = db.tasks.Find(task_id);
                
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }

                var assigned_tasks_list = db.assigned_tasks.Where(m => m.task_id == task_id).ToList();

               

                // Deleteing Scheduled Meeting

                if (assigned_tasks_list != null)
                {

                    foreach (assigned_tasks sm in assigned_tasks_list)
                    {
                        db.Entry(sm).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }


                }




                // Deleteing Parent Meeting
                db.Entry(original).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfully !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}
