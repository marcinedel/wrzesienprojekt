using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using wrzesienprojekt.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace wrzesienprojekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotatkiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public NotatkiController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select NotatkaID, NotatkaNazwa, Przedmiot, 
                            convert(varchar(10),DataDodania,120) as DataDodania, NazwaZdjecia 
                            from dbo.Notatki
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WrzesienAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }
            return new JsonResult(table);

        }
        [HttpPost]
        public JsonResult Post(Notatka n)
        {
            string query = @"
                            insert into dbo.Notatki
                            (NotatkaNazwa, Przedmiot, DataDodania, NazwaZdjecia)
                            values (@NotatkaNazwa, @Przedmiot, @DataDodania, @NazwaZdjecia)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WrzesienAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@NotatkaNazwa", n.NotatkaNazwa);
                    myCommand.Parameters.AddWithValue("@Przedmiot", n.Przedmiot);
                    myCommand.Parameters.AddWithValue("@DataDodania", n.DataDodania);
                    myCommand.Parameters.AddWithValue("@NazwaZdjecia", n.NazwaZdjecia);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Dodano nową notatkę");
        }
        [HttpPut]
        public JsonResult Put(Notatka n)
        {
            string query = @"
                            update dbo.Notatki
                            set NotatkaNazwa= @NotatkaNazwa,
                                Przedmiot= @Przedmiot,
                                DataDodania = @DataDodania,
                                NazwaZdjecia = @NazwaZdjecia
                               where NotatkaID= @NotatkaID
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WrzesienAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@NotatkaID", n.NotatkaID);
                    myCommand.Parameters.AddWithValue("@NotatkaNazwa", n.NotatkaNazwa);
                    myCommand.Parameters.AddWithValue("@Przedmiot", n.Przedmiot);
                    myCommand.Parameters.AddWithValue("@DataDodania", n.DataDodania);
                    myCommand.Parameters.AddWithValue("@NazwaZdjecia", n.NazwaZdjecia);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Aktualizacja notatki powiodła się");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Notatki
                            where NotatkaID= @NotatkaID
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WrzesienAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@NotatkaID", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Usunięcie notatki powiodło się");
        }
        [Route("saveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Zdjecia/" + filename; 
                using(var stream=new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);

                }
                return new JsonResult(filename);

            }
            catch (Exception)
            {
                return new JsonResult("puste.png");
            }
        }
    }
}
