using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using log4net;
using DataVisualizer.Models;

namespace DataVisualizer.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            var csvLocation = ConfigurationManager.AppSettings["csvLocation"];
            var records = new List<DataModel>();

            try
            {
                // try to open the file
                using (TextReader textReader = System.IO.File.OpenText(csvLocation))
                {
                    var csv = new CsvReader(textReader);

                    // need to read past header to get model data
                    csv.Read();
                    csv.ReadHeader();

                    // read each row
                    while (csv.Read())
                    {
                        var record = csv.GetRecord<DataModel>();

                        if (record != null)
                        {
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

            return View(records);
        }
        
    }
}