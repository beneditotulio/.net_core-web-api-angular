using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebAppAngular.Models;

namespace WebAppAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentAPIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"Select * From Department";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader dataReader;
            using(SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand(query,con))
                {
                    dataReader = cmd.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    con.Close();
                }
                return new JsonResult(table);
            }
        }
    
        [HttpPost]
        public JsonResult Save(Department department)
        {
            string message = "";
            if (department.DepartmentName != null)
            {
                string connectionString = _configuration.GetConnectionString("EmployeeAppCon");
                string SQL = "Insert into Department Values('" + department.DepartmentName + "')";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(SQL, con))
                    {
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        { message = "Gravado com sucesso"; }
                        else
                        { message = "Falha"; }
                    }
                }

            }

            return new JsonResult(message);
        }

        [HttpPut]
        //public JsonResult Update(Department department)
        public JsonResult Update(string DepartmentName, string Id)
        {
            string message = "";
            string name = Request.Form["DepartmentName"];
            string key = Request.Form["DepartmentId"];
            Department department = new Department { DepartmentName = DepartmentName, DepartmentId = int.Parse(Id) };
            if(department !=null)
            {
                string connectinString = _configuration.GetConnectionString("EmployeeAppCon");
                string SQL = "Update Department Set DepartmentName = '"+department.DepartmentName+"'"
                    +"Where DepartmentId = '"+department.DepartmentId+"'";
                using(SqlConnection connection = new SqlConnection(connectinString))
                {
                    connection.Open();
                    using(SqlCommand command = new SqlCommand(SQL, connection))
                    {

                       int i = command.ExecuteNonQuery();
                        connection.Close();
                        if(i>0)
                        { message = "Actualizado com sucesso"; }
                        else
                        { message = "Falha"; }
                    }
                }
            }
            return new JsonResult(message);
        }

    }
}
