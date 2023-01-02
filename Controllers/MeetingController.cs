using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiitProjectProgessSystemApi.Models;
using BiitProjectProgessSystemApi.Models.CustomModels;
using System.Data;
//using System.Data.SqlClient;
using System.Web;

namespace BiitProjectProgessSystemApi.Controllers
{
    public class MeetingController : ApiController
    {
        
        Mybpms db = new Mybpms();

        public Mybpms Db { get => db; set => db = value; }

        [HttpGet]
        public HttpResponseMessage getMeetingDetail(int meeting_id)
        {

            try
            {

                var meeting = db.meetings.FirstOrDefault(s => s.id == meeting_id);

                if (meeting == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }

                Meeting meeting_item = new Meeting();

                meeting_item.id = meeting.id;
                meeting_item.title = meeting.title;
                meeting_item.location = meeting.location;
                meeting_item.meeting_notes = meeting.meeting_notes;
                meeting_item.meeting_timing = meeting.meeting_timing;
                meeting_item.isRecurring = (byte)meeting.isRecurring;


                return Request.CreateResponse(HttpStatusCode.OK, meeting_item);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage markAttendance(List<User> users,int meeting_id)
        {


            try
            {

                foreach (User u in users) {
                    var meeting = db.scheduled_meetings.Where(m => m.scheduled_for == u.id && m.meeting_id == meeting_id).First();
                   

                    scheduled_meetings updated_meeting = new scheduled_meetings();
                    updated_meeting = meeting;
                    updated_meeting.is_attended = u.is_present;

                    db.Entry(meeting).CurrentValues.SetValues(updated_meeting);
                    db.SaveChanges();
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, "Attendance Marked Successfully !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage createMeeting(Meeting meeting)
        {
            

            try
            {

                meeting t = new meeting();
                t.title = meeting.title;
                t.meeting_notes = meeting.meeting_notes;
                t.meeting_timing = meeting.meeting_timing;
                t.location = meeting.location;
                t.isRecurring = meeting.isRecurring;
                t.is_fyp_1 = meeting.is_fyp_1;
                t.status = 6;

                db.meetings.Add(t);
                db.SaveChanges();

               
                return Request.CreateResponse(HttpStatusCode.OK, t.id);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // ***************************************  Project Committee  ******************************************
        // ********************************************************************************************


        [HttpGet]
        public HttpResponseMessage getDateTime() {

            String now =  DateTime.Now.ToString("MM/dd/yyyy hh:mm ");

            String time = DateTime.Now.ToString("07/05/2022 02:20");

            if (DateTime.Parse(now) > DateTime.Parse(time)) {
                return Request.CreateResponse(HttpStatusCode.OK, Convert.ToDateTime(time).AddDays(1));

            }

            return Request.CreateResponse(HttpStatusCode.OK, "failed");


        }
        [HttpPost]
        public HttpResponseMessage scheduleMeetingWithCommittee(Meeting meeting)
        {
            try
            {



                var meeting_exist = db.meetings.FirstOrDefault(u => u.id == meeting.id);

                if (meeting_exist == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotImplemented, "Record Not Found");
                }


                var fyp_groups = db.fyp_groups.Where(s=>s.is_fyp_1== meeting.is_fyp_1);

                //var students = db.users.Where(u=>u.role_id==4).Join(fyp_groups, u => u.group_id, r => r.id, (u, r) =>
                //new
                //    User
                //{
                //    id = u.id,
                //    name = u.name,
                //    username = u.username,
                //    email = u.email,
                //    cgpa = "" + u.cgpa,
                //    phone = u.phone,
                //    address = u.address,
                //    platform = u.platform,
                //});

               

                if (fyp_groups.Count() > 0)
                {
                    double minutes = 0.0;
                    var meeting_time = meeting_exist.meeting_timing;
                    String time = DateTime.Now.ToString("07/05/2022 10:00");
                    foreach (fyp_groups group in fyp_groups)
                    {
                        
                        
                        DateTime date_time = Convert.ToDateTime(meeting_time).AddMinutes(minutes);                        
                        string date_str = date_time.ToString("dd MMM yyyy hh:mm");


                        string specific_sql_format_after_adding_minutes = date_time.ToString("MM/dd/yyyy hh:mm");
                        DateTime checking_date_time = DateTime.Parse(meeting_time);
                        string specific_sql_format_for_checking = checking_date_time.ToString("MM/dd/yyyy hh:mm");
                        

                        if (DateTime.Parse(specific_sql_format_after_adding_minutes) > DateTime.Parse(time)) {
                            DateTime date_time1 = Convert.ToDateTime(meeting_time).AddDays(1);
                            time = Convert.ToDateTime(time).AddDays(1).ToString("MM/dd/yyyy hh:mm");
                            meeting_time = date_time1.ToString("dd MMM yyyy hh:mm");
                            minutes = 0;
                        }

                        //String now = DateTime.Now.ToString("MM/dd/yyyy hh:mm ");

                        //String time = DateTime.Now.ToString("07/05/2022 02:20");

                        //if (DateTime.Parse(specific_sql_format) > DateTime.Parse(time))
                        //{
                        //    return Request.CreateResponse(HttpStatusCode.OK, Convert.ToDateTime(time).AddDays(1));

                        //}

                        var students = db.users.Where(u => u.role_id == 4 && u.group_id == group.id);
                        foreach (user std in students)
                        {
                            scheduled_meetings t1 = new scheduled_meetings();

                            t1.meeting_id = meeting.id;
                            t1.scheduled_by = int.Parse(meeting.scheduled_by);
                            t1.scheduled_for = std.id;
                            t1.is_attended = 0;
                            t1.role_id = meeting.role_id;
                            t1.meeting_timing = date_str;


                            db.scheduled_meetings.Add(t1);
                        }
                        
                        minutes = minutes + 15.0;

                    }
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Meeting Arranged Successfully !");
                }
                


                //if (students.Count() > 0) {
                //    foreach (User std in students)
                //    {
                //        scheduled_meetings t1 = new scheduled_meetings();

                //        t1.meeting_id = meeting.id;
                //        t1.scheduled_by = int.Parse(meeting.scheduled_by);
                //        t1.scheduled_for = std.id;
                //        t1.is_attended = 0;                        


                //        db.scheduled_meetings.Add(t1);

                //    }
                //    db.SaveChanges();

                //    return Request.CreateResponse(HttpStatusCode.OK, "Meeting Arranged Successfully !");
                //}

                var original = db.meetings.Find(meeting.id);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }

                db.Entry(original).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK, "No student belongs to this FYP group !");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // ***************************************  Supervisor  ******************************************
        // ********************************************************************************************


        [HttpGet]
        public HttpResponseMessage getMeetingSubmittedFiles(int group_id, int meeting_id)
        {
            try
            {

                var meeting_files = db.uploaded_meeting_files.Where(s => s.group_id == group_id  && s.meeting_id == meeting_id).ToList();

                if (meeting_files == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }



                List<String> meeting_files_list = new List<String>();
                foreach (uploaded_meeting_files file in meeting_files)
                {
                    meeting_files_list.Add(file.file_path.Trim('.'));
                }


                return Request.CreateResponse(HttpStatusCode.OK, meeting_files_list);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getCommitteeMeetingsList(int user_id,int role_id,int is_fyp_1)
        {
            try
            {

                var user = db.users.FirstOrDefault(s => s.id == user_id);

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }


                var scheduled_meetings_ids = db.scheduled_meetings.Where(t => t.role_id == role_id).OrderByDescending(d => d.meeting_id).Select(s => new { id = s.meeting_id }).Distinct();

                IQueryable<Meeting> meetings = null;
                if (is_fyp_1 == -1)   // -1 means both fyp-1 & fyp-2
                {
                    meetings = db.meetings.Join(scheduled_meetings_ids, t => t.id, r => r.id, (t, r) =>
                    new
                        Meeting
                    {

                        id = t.id,
                        title = t.title,
                        location = t.location,
                        meeting_notes = t.meeting_notes,
                        meeting_timing = t.meeting_timing,
                        isRecurring = (byte)t.isRecurring,
                        //scheduled_by = scheduled_by.name,
                        authority_role = "Supervisor",
                        status = t.status == null ? 6 : (int)t.status,

                        //scheduled_for = meeting.scheduled_for,
                    });

                }
                else {
                     meetings = db.meetings.Where(m => m.is_fyp_1 == is_fyp_1).Join(scheduled_meetings_ids, t => t.id, r => r.id, (t, r) =>
                      new
                          Meeting
                      {

                          id = t.id,
                          title = t.title,
                          location = t.location,
                          meeting_notes = t.meeting_notes,
                          meeting_timing = t.meeting_timing,
                          isRecurring = (byte)t.isRecurring,
                        //scheduled_by = scheduled_by.name,
                        authority_role = "Supervisor",
                          status = t.status == null ? 6 : (int)t.status,

                        //scheduled_for = meeting.scheduled_for,
                    });
                }

                if (meetings == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");

                }



                List<Meeting> meetingList = new List<Meeting>();
                List<int> addedGroupIds = new List<int>();


                foreach (Meeting meeting in meetings)
                {
                   
                        
                        var s_meetings = db.scheduled_meetings.Where(m => m.meeting_id == meeting.id).ToList();

                        foreach (scheduled_meetings obj in s_meetings)
                        {
                            if (obj.user1.group_id != null) {

                                if (isMeetingAlreadyAdded(addedGroupIds, (int)obj.user1.group_id))
                                {
                                    continue;
                                }

                                addedGroupIds.Add((int)obj.user1.group_id);
                            }
                            Meeting m = new Meeting();
                            m.id = meeting.id;
                            m.title = meeting.title;
                            m.location = meeting.location;
                            m.meeting_notes = meeting.meeting_notes;
                            m.isRecurring = (byte)meeting.isRecurring;
                            //scheduled_by = scheduled_by.name;
                            m.authority_role = "Supervisor";
                            m.status = meeting.status == null ? 6 : (int)meeting.status;

                            if (obj.user1.group_id != null)
                            {
                                int group_id = (int) obj.user1.group_id;
                                m.group_id = group_id;
                                m.is_fyp_1 = (byte) obj.user1.fyp_groups.is_fyp_1;
                                m.meeting_timing = obj.meeting_timing;
                                try {
                                    var project = db.project_allocation.FirstOrDefault(a => a.group_id == group_id);
                                    m.group_name = project.project.title;
                                }
                                catch (Exception ex) { }

                                if (m.group_name != null)
                                { meetingList.Add(m); }
                         }

                        }

                        addedGroupIds.Clear();

                   
                }

                return Request.CreateResponse(HttpStatusCode.OK, meetingList);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool isMeetingAlreadyAdded(List<int> addedGroupIds,int group_id)
        {
            foreach (int already_present_id in addedGroupIds) {
                if (group_id == already_present_id) {
                    return true;
                }
            }

            return false;
        }

        [HttpGet]
        public HttpResponseMessage getSupervisorMeetings(int user_id)
        {
            try
            {

                var supervisor = db.users.FirstOrDefault(s => s.id == user_id);

                if (supervisor == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }


                var scheduled_meetings_ids = db.scheduled_meetings.Where(t => t.scheduled_by == user_id).OrderByDescending(d=>d.meeting_id).Select(s => new { id = s.meeting_id }).Distinct();


                var meetings = db.meetings.Join(scheduled_meetings_ids, t => t.id, r => r.id, (t, r) =>
                new
                    Meeting
                {
                   
                    id = t.id,
                    title = t.title,
                    location = t.location,
                    meeting_notes = t.meeting_notes,
                    meeting_timing = t.meeting_timing,
                    isRecurring = (byte)t.isRecurring,
                    //scheduled_by = scheduled_by.name,
                    authority_role = "Supervisor",
                    status = t.status==null?6:(int)t.status,

                    //scheduled_for = meeting.scheduled_for,
                });


                List<Meeting> meetingList = new List<Meeting>();
                foreach (Meeting meeting in meetings)
                {
                    try {
                        var s_meetings = db.scheduled_meetings.Where(m => m.meeting_id == meeting.id).ToList();

                        foreach (scheduled_meetings obj in s_meetings) {
                            if (obj.user1.group_id != null) {
                                int group_id = obj.user1.fyp_groups.id;
                                meeting.group_id = group_id;
                                meeting.group_name = db.project_allocation.Where(a => a.group_id == group_id).First().project.title;
                                meetingList.Add(meeting);
                                break;
                            }
                            
                        }
                       
                    } catch (Exception ex) { }                   
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, meetingList);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage scheduleMeeting(scheduled_meetings meeting)
        {
            try
            {

                var meeting_exist = db.meetings.FirstOrDefault(u => u.id == meeting.id);

                if (meeting_exist == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotImplemented, "Record Not Found");
                }

             
                scheduled_meetings t1 = new scheduled_meetings();

                t1.meeting_id = meeting.id;
                t1.scheduled_by = meeting.scheduled_by;
                t1.scheduled_for = meeting.scheduled_for;
                t1.is_attended = 0;
                t1.meeting_timing = meeting_exist.meeting_timing;

                db.scheduled_meetings.Add(t1);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Meeting Arranged Successfully !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // ***************************************  Student  ******************************************
        // ********************************************************************************************


        [HttpPost]
        public HttpResponseMessage uploadMeetingFiles(int group_id, int meeting_id)
        {
            try
            {
                var meeting_found = db.meetings.FirstOrDefault(m => m.id == meeting_id );

                if (meeting_found == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotImplemented, "Record Not Saved");
                }

                var request = HttpContext.Current.Request;

                if (request.Files.Count > 0)
                {
                    for (int i = 0; i < request.Files.Count; i++)
                    {

                        uploaded_meeting_files t = new uploaded_meeting_files();

                        var photo = request.Files[i];
                        photo.SaveAs(HttpContext.Current.Server.MapPath("~/Content/uploaded_tasks/" + photo.FileName));
                        System.IO.FileInfo fi = new System.IO.FileInfo(photo.FileName);
                        // Check if file is there  
                        String ext = fi.Extension;
                        String newname = photo.FileName;
                        t.file_path = "Content/uploaded_tasks/" + newname;
                        t.group_id = group_id;
                        t.meeting_id = meeting_id;
                        t.uploaded_at = DateTime.Now.ToString();
                        db.uploaded_meeting_files.Add(t);
                        db.SaveChanges();
                    }

                    //db.uploaded_tasks.Add(t);
                    //db.SaveChanges();



                    return Request.CreateResponse(HttpStatusCode.OK, "Files Successfully Uploaded !");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Task File Missing !");

                }
            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getStudentMeeting(int user_id)
        {
            try
            {

                var student = db.users.FirstOrDefault(s => s.id == user_id && s.role_id == 4);

                if (student == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }


                var scheduled_meetings_list = db.scheduled_meetings.Where(t => t.scheduled_for == user_id && student.group_id!=null);

                List<Meeting> meeting_list = new List<Meeting>();
                foreach (scheduled_meetings meeting in scheduled_meetings_list)
                {

                    var t = db.meetings.FirstOrDefault(tk => tk.id == meeting.meeting_id);
                    var scheduled_by = meeting.user;
                    var authority_role = meeting.user.role;

                    if (t != null)
                    {

                        Meeting meeting_item = new Meeting();

                        meeting_item.id = t.id;
                        meeting_item.title = t.title;
                        meeting_item.location = t.location;
                        meeting_item.meeting_notes = t.meeting_notes;
                        meeting_item.group_id = student.fyp_groups.id;
                        meeting_item.meeting_timing = meeting.meeting_timing;
                        meeting_item.isRecurring = (byte)t.isRecurring;
                        meeting_item.scheduled_by = scheduled_by.name;
                        meeting_item.authority_role = authority_role.title;
                        meeting_item.scheduled_for = meeting.scheduled_for;
                        meeting_item.status = (int) t.status;                     
                        meeting_item.is_attended = (byte) meeting.is_attended;

                        meeting_list.Add(meeting_item);
                    }

                }


                return Request.CreateResponse(HttpStatusCode.OK, meeting_list);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]    
        public HttpResponseMessage getMeetingStudents(int meeting_id,int group_id)
        {

            try
            {

                var students_ids = db.scheduled_meetings.Where(u => u.meeting_id == meeting_id).Select(s=>new { id = s.scheduled_for,s.is_attended });

                if (students_ids == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found !");
                }

                var group_members = db.users.Where(u => u.group_id == group_id).ToList();

                if (group_members == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found !");
                }

                var students = group_members.Where(u=>u.group_id!=null).Join(students_ids, u => u.id, r => r.id, (u, r) =>
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
                       is_present = (byte) r.is_attended,
                   });



                return Request.CreateResponse(HttpStatusCode.OK, students);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }


        }

      

        [HttpGet]
        public HttpResponseMessage deleteMeeting(int meeting_id,int group_id)
        {

            try
            {
                var original = db.meetings.Find(meeting_id);
                var group = db.fyp_groups.Find(group_id);
                if (original == null || group==null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
                }

                var scheduled_meetings_list = db.scheduled_meetings.Where(m=>m.meeting_id==meeting_id).ToList();

                var remarks_list = db.remarks.Where(m => m.group_id == group_id).ToList();

                // Deleteing Remarks

                if (remarks_list != null)
                {

                    foreach (remark rm in remarks_list)
                    {
                        db.Entry(rm).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }


                }


                // Deleteing Scheduled Meeting

                if (scheduled_meetings_list != null) {

                    foreach (scheduled_meetings sm in scheduled_meetings_list) {
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

