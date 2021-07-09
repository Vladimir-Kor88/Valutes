using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Xml;

namespace Valuta
{
    class Program
    {
        static void Main()
        {
            //Console.WriteLine(GetCourse(DateTime.Now, "Австралийский доллар"));
        }
        static void FillDictionary(string[] args) //Заполнение справочника
        {
            var query = "http://cbr.ru/scripts/XML_valFull.asp";

            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(query, "valutes.xml");
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("valutes.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            List<Valute> listDC = new List<Valute>();
            foreach (XmlNode xnode in xRoot)
            {
                var item = new Valute
                {
                    Code = xnode.Attributes.GetNamedItem("ID").Value,
                    Name = xnode.ChildNodes[0].InnerText
                };
                listDC.Add(item);
            }

            using (moneyContext db = new moneyContext())
            {
                db.Valutes.AddRange(listDC);
                db.SaveChanges();
            }
        }
        static void EveryDayInsertCourse(string[] args) //ежедневное заполение курса валют
        {
            var query = "http://cbr.ru/scripts/XML_daily.asp?date_req=" + DateTime.Now.ToString("dd.MM.yyyy");

            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(query, "valutes.xml");
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("valutes.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            List<DateCourse> listDC = new List<DateCourse>();
            foreach (XmlNode xnode in xRoot)
            {
                var item = new DateCourse
                {
                    Date = DateTime.Now,
                    Value = decimal.Parse(xnode.ChildNodes[4].InnerText),
                    ValuteId = xnode.Attributes.GetNamedItem("ID").Value
                };
                listDC.Add(item);
            }

            using (moneyContext db = new moneyContext())
            {
                db.DateCourses.AddRange(listDC);
                db.SaveChanges();
            }
        }

        static decimal GetCourse(DateTime date, string valute)
        {
            List<DateCourse> query;
            using (moneyContext db = new moneyContext())
            {
                query = db.DateCourses.Where(d => d.Valute.Name == valute && d.Date == date).ToList();
            }
            if (query.Count() == 0)
                return 0;
            return query[0].Value;
        }
    }
}
