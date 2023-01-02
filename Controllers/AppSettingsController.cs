using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BiitProjectProgessSystemApi.Models;
using BiitProjectProgessSystemApi.Models.CustomModels;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace BiitProjectProgessSystemApi.Controllers
{
    public class AppSettingsController : ApiController
    {
        Mybpms db = new Mybpms();

        OleDbConnection Econ;
        string constr, Query, sqlconn;
        SqlConnection con;

        private void connection()
        {
            sqlconn = "Data Source=DESKTOP-803NEAL\\SQLEXPRESS;Initial Catalog=bppms;Persist Security Info=True;User ID=sa;Password=gohar12345";
            con = new SqlConnection(sqlconn);
        }

        private void ExcelConn(string FilePath)
        {
            constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", FilePath);
            Econ = new OleDbConnection(constr);
        }

        [HttpGet]
        public HttpResponseMessage getAppSettings()
        {
            try
            {

                var appSetting = db.app_settings.OrderByDescending(c=>c.id).First();
                

                return Request.CreateResponse(HttpStatusCode.OK, appSetting);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage updateAppSettings(int no_of_members)
        {
            try
            {
                var appSetting = db.app_settings.OrderByDescending(c => c.id).First();
              

                if (appSetting != null)
                {
                    app_settings updated_setting = new app_settings();

                    updated_setting.id = appSetting.id;
                    updated_setting.no_of_members = no_of_members;


                    db.Entry(appSetting).CurrentValues.SetValues(updated_setting);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "Settings Updated Successfully ");
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Settings not Updated !");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage insertProjects()
        {
            try
            {
                var request = HttpContext.Current.Request;

                if (request.Files.Count > 0)
                {
                    string file_path_ = "";
                    for (int i = 0; i < request.Files.Count; i++)
                    {


                        var photo = request.Files[i];
                        photo.SaveAs(HttpContext.Current.Server.MapPath("~/Content/uploaded_tasks/" + photo.FileName));
                        System.IO.FileInfo fi = new System.IO.FileInfo(photo.FileName);
                        file_path_ = HttpContext.Current.Server.MapPath("~/Content/uploaded_tasks/" + photo.FileName);
                      

                    }

                    //db.uploaded_tasks.Add(t);
                    //db.SaveChanges();

                    insertProjectRecordsFromExcel(file_path_);
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Project Inserted Successfully!");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage insertUsers()
        {
            try
            {
                var request = HttpContext.Current.Request;

                if (request.Files.Count > 0)
                {
                    string file_path_ = "";
                    for (int i = 0; i < request.Files.Count; i++)
                    {

                        var photo = request.Files[i];
                        photo.SaveAs(HttpContext.Current.Server.MapPath("~/Content/uploaded_tasks/" + photo.FileName));
                        System.IO.FileInfo fi = new System.IO.FileInfo(photo.FileName);

                        file_path_ = HttpContext.Current.Server.MapPath("~/Content/uploaded_tasks/" + photo.FileName);
                        
                        // Check if file is there  
                        //String ext = fi.Extension;
                        //String newname = photo.FileName;
                        //t.file_path = "Content/uploaded_tasks/" + newname;
                       
                    }

                    //db.uploaded_tasks.Add(t);
                    //db.SaveChanges();
                    
                    insertUsersRecordsFromExcel(file_path_);
                }




                return Request.CreateResponse(HttpStatusCode.OK, "Users Inserted Successfully!");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        private void insertProjectRecordsFromExcel(string FilePath)
        {
            ExcelConn(FilePath);
            Query = string.Format("Select [title],[description] FROM [{0}]", "Sheet1$");
            OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            Econ.Close();
            oda.Fill(ds);
            System.Data.DataTable Exceldt = ds.Tables[0];
            connection();
            //creating object of SqlBulkCopy      
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name      
            objbulk.DestinationTableName = "projects";
            //Mapping Table column      
            objbulk.ColumnMappings.Add("title", "title");
            objbulk.ColumnMappings.Add("description", "description");
            //inserting Datatable Records to DataBase      
            con.Open();
            objbulk.WriteToServer(Exceldt);
            con.Close();
        }


        private void insertUsersRecordsFromExcel(string FilePath)
        {
            ExcelConn(FilePath);
            Query = string.Format("Select [name],[username],[password],[email],[phone],[address],[platform],[cgpa],[semester],[role_id] FROM [{0}]", "Sheet1$");
            OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            //Query = string.Format("Select [name],[username],[password],[email],[phone],[address],[platform],[cgpa],[semester],[group_id],[role_id],[is_allowed_in_meeting],[fyp_1_final_grade],[fyp_2_final_grade],[profile_pic] FROM [{0}]", "Sheet1$");

            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            Econ.Close();
            oda.Fill(ds);
            System.Data.DataTable Exceldt = ds.Tables[0];
            connection();
            //creating object of SqlBulkCopy      
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name      
            objbulk.DestinationTableName = "users";
            //Mapping Table column      
            objbulk.ColumnMappings.Add("name", "name");
            objbulk.ColumnMappings.Add("username", "username");
            objbulk.ColumnMappings.Add("password", "password");
            objbulk.ColumnMappings.Add("email", "email");
            objbulk.ColumnMappings.Add("phone", "phone");
            objbulk.ColumnMappings.Add("address", "address");
            objbulk.ColumnMappings.Add("platform", "platform");
            objbulk.ColumnMappings.Add("cgpa", "cgpa");
            objbulk.ColumnMappings.Add("semester", "semester");
            objbulk.ColumnMappings.Add("role_id", "role_id");
            //objbulk.ColumnMappings.Add("group_id", "group_id");
            //objbulk.ColumnMappings.Add("is_allowed_in_meeting", "is_allowed_in_meeting");
            //objbulk.ColumnMappings.Add("fyp_1_final_grade", "fyp_1_final_grade");
            //objbulk.ColumnMappings.Add("fyp_2_final_grade", "fyp_2_final_grade");
            //objbulk.ColumnMappings.Add("profile_pic", "profile_pic");

            //inserting Datatable Records to DataBase      
            con.Open();
            objbulk.WriteToServer(Exceldt);
            con.Close();
        }

    }


}
