using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiitProjectProgessSystemApi.Models;
using BiitProjectProgessSystemApi.Models.CustomModels;
using System.Web.Http.Cors;

namespace BiitProjectProgessSystemApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProjectController : ApiController
    {
        Mybpms db = new Mybpms();

        [HttpGet]
        public HttpResponseMessage getProjectList()
        {
            try
            {

                var projects = db.projects.ToList();


                List<Project> projectList = new List<Project>();
                foreach (project p in projects)
                {
                    Project obj = new Project();
                    obj.id = p.id;
                    obj.title = p.title;
                    obj.description = p.description;


                    projectList.Add(obj);
                }

                return Request.CreateResponse(HttpStatusCode.OK, projectList);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getProjectListForAllocation()
        {
            try
            {


                var totalprojects = db.projects.Select(s => new { s.id });
                var allocated_projects = db.project_allocation.Select(s => new { id = s.project_id });
                var remainProjects = totalprojects.Except(allocated_projects);


                var projetcs = db.projects.Join(remainProjects, p => p.id, r => r.id, (p, r) =>
                new
                    Project
                {
                    id = p.id,
                    title = p.title,
                    description = p.description,
                });

                return Request.CreateResponse(HttpStatusCode.OK, projetcs);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage getSupervisorSponsoredProjectList(int user_id)
        {
            try
            {


                //var isAlreadySentRequest = db.fyp_groups_requests_supervisor.Where(r => r.requested_by == user_id).ToList().Count();
                //List<Project> empty_project_List = new List<Project>();
                //if (isAlreadySentRequest > 0)
                //{

                //    return Request.CreateResponse(HttpStatusCode.OK, empty_project_List);

                //}


                var totalprojects = db.projects.Select(s => new { s.id });
                var sponsored_projects = db.project_allocation.Where(p => p.group_id == null).Select(s => new { id = s.project_id, supervisor_name = s.user.name, supervisor_id = s.supervisor_id });
                //var remainProjects = totalprojects.Except(allocated_projects);

                var projetcs = db.projects.Join(sponsored_projects, p => p.id, r => r.id, (p, r) =>
                new
                    Project
                {
                    id = p.id,
                    title = p.title,
                    description = p.description,
                    supervisor_name = r.supervisor_name,
                    supervisor_id = r.supervisor_id,
                });

                return Request.CreateResponse(HttpStatusCode.OK, projetcs);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        //[HttpGet]
        //public HttpResponseMessage getSupervisorProjectsRequests(int user_id)
        //{
        //    try
        //    {

        //        var requests = db.fyp_groups_requests_supervisor.Where(g => g.supervisor_id == user_id).ToList();

        //        List<ProjectRequest> request_lists = new List<ProjectRequest>();
        //        foreach (fyp_groups_requests_supervisor req in requests)
        //        {
        //            ProjectRequest obj = new ProjectRequest();
        //            var allocated_project = db.project_allocation.FirstOrDefault(p => p.project_id == req.project_id && p.group_id != null);
        //            if (allocated_project == null)
        //            {
        //                obj.id = req.id;
        //                obj.group_id = (int)req.group_id;
        //                obj.group_name = req.fyp_groups.name;
        //                obj.supervisor_id = (int)req.supervisor_id;
        //                obj.supervisor_name = req.user.name;
        //                obj.project_id = (int)req.project_id;
        //                obj.project_name = req.project.title;
        //                obj.project_desccription = req.project.description;

        //                request_lists.Add(obj);
        //            }

        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, request_lists.OrderBy(r => r.project_name));
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        [HttpPost]
        public HttpResponseMessage updateProjectAllocation(project_allocation request)
        {
            try
            {

                var project = db.project_allocation.FirstOrDefault(u => u.project_id == request.project_id);


                if (project == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found !");
                }

                var updated_proj_allocation = new project_allocation();
                updated_proj_allocation = project;

                updated_proj_allocation.project_id = request.project_id;
                updated_proj_allocation.supervisor_id = request.supervisor_id;
                updated_proj_allocation.group_id = request.group_id;
                updated_proj_allocation.isSpringSemester = request.isSpringSemester;
                updated_proj_allocation.allocation_year = request.allocation_year;
                updated_proj_allocation.isActive = request.isActive;



                db.Entry(project).CurrentValues.SetValues(updated_proj_allocation);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage allocateProject(project_allocation request)
        {
            try
            {


                db.project_allocation.Add(request);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Project Allocated Successfully !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //[HttpGet]
        //public HttpResponseMessage canRequestForProject(int user_id)
        //{
        //    try
        //    {
        //        var original = db.fyp_groups_requests_supervisor.FirstOrDefault(p => p.requested_by == user_id);

        //        if (original == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, true);

        //        }



        //        return Request.CreateResponse(HttpStatusCode.OK, false);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }

        //}

        //[HttpGet]
        //public HttpResponseMessage acceptProjectRequestBySupervisor(int project_id, int supervisor_id, int group_id)
        //{
        //    try
        //    {
        //        var original = db.project_allocation.FirstOrDefault(p => p.project_id == project_id && p.supervisor_id == supervisor_id && p.group_id == null);

        //        if (original == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found / (Project Already Taken) !");

        //        }

        //        project_allocation updated_p = original;
        //        updated_p.group_id = group_id;

        //        db.Entry(original).CurrentValues.SetValues(updated_p);
        //        db.SaveChanges();

        //        var sup_proj_requests = db.fyp_groups_requests_supervisor.Where(g => g.project_id == project_id && g.group_id != group_id).ToList();



        //        foreach (fyp_groups_requests_supervisor obj in sup_proj_requests)
        //        {

        //            db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
        //            db.SaveChanges();
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, "Accepted Successfully !");
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        //[HttpGet]
        //public HttpResponseMessage rejectProjectRequestBySupervisor(int project_id, int supervisor_id, int group_id)
        //{
        //    try
        //    {
        //        var original = db.fyp_groups_requests_supervisor.FirstOrDefault(p => p.project_id == project_id && p.supervisor_id == supervisor_id && p.group_id == group_id);

        //        if (original == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found");
        //        }



        //        db.Entry(original).State = System.Data.Entity.EntityState.Deleted;
        //        db.SaveChanges();


        //        return Request.CreateResponse(HttpStatusCode.OK, "Rejected Successfully !");
        //    }
        //    catch (Exception ex)
        //    {

        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        [HttpGet]
        public HttpResponseMessage changeSupervisor(int project_id, int supervisor_id)
        {
            try
            {
                var original = db.project_allocation.FirstOrDefault(p => p.project_id == project_id);

                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Record Not Found !");

                }
                project_allocation updated_p = original;
                updated_p.supervisor_id = supervisor_id;

                db.Entry(original).CurrentValues.SetValues(updated_p);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
