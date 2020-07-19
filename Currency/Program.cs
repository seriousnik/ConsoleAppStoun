using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Currency
{
    class Program
    {
        private static bool isNeedRefreshCurrencyTable = false;
        private static string urlCbr = "http://www.cbr.ru/scripts/";
        private static DateTime date = DateTime.Today;
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            
            //справочник будем регулярно обновлять или как?!
            if (!isNeedRefreshCurrencyTable)
            {
                string currencyFromRestAPI = CallRest($"{urlCbr}XML_valFull.asp");
                var listCurrency = XMLParse(currencyFromRestAPI, new List<Currency>());
                DataBaseManipulation.AddDataToDbCurrency(connectionString, listCurrency);
            }

            var currencyByDateFromRestApi = CallRest($"{urlCbr}XML_daily.asp?date_req={date:dd.MM.yyyy}");
            var listCurrencyByDate = XMLParse(currencyByDateFromRestApi, new List<CurrencyByDate>());
            DataBaseManipulation.AddDataToDbCurrencyByDate(connectionString, listCurrencyByDate);
        }

        public static string CallRest(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding enc = Encoding.GetEncoding("windows-1251");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            var result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;
        }
        public static List<T> XMLParse<T>(string xmlString, List<T> listCurrency) where T : new()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlString);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                var cur = new T();
                var t = typeof(T);
                var properties = t.GetProperties();
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("ID");
                    if (attr != null)
                        properties.Where(e => e.Name == "ID").FirstOrDefault()?.SetValue(cur, attr.Value);
                }
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (String.IsNullOrEmpty(childnode.InnerText))
                        continue;
                    properties.Where(p => p.Name == childnode.Name).FirstOrDefault()?.SetValue(cur, childnode.InnerText);

                }
                listCurrency.Add(cur);
            }
            return listCurrency;
        }
    }
}






