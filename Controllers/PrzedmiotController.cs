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

namespace wrzesienprojekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrzedmiotController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PrzedmiotController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select PrzedmiotID, PrzedmiotNazwa from
                            dbo.Przedmiot
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WrzesienAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
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
        public JsonResult Post(Przedmiot p)
        {
            string query = @"
                            insert dbo.Przedmiot
                            values (@PrzedmiotNazwa)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WrzesienAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@PrzedmiotNazwa", p.PrzedmiotNazwa);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }
           
            return new JsonResult("Dodano nowy przedmiot");
        }
        [HttpPut]
        public JsonResult Put(Przedmiot p)
        {
            string query = @"
                            update dbo.Przedmiot
                            set PrzedmiotNazwa= @PrzedmiotNazwa
                               where PrzedmiotID= @PrzedmiotID
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WrzesienAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@PrzedmiotID", p.PrzedmiotID);
                    myCommand.Parameters.AddWithValue("@PrzedmiotNazwa", p.PrzedmiotNazwa);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Aktualizacja nazwy przedmiotu powiodła się");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Przedmiot
                            where PrzedmiotID= @PrzedmiotID
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WrzesienAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@PrzedmiotID", id);
                    
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Usunięcie przedmiotu powiodło się");
        }


    }
}
