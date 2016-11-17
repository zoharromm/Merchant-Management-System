using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using HtmlAgilityPack;
using System.Net;
using System.Net.Mail;
using Crawler_Without_Sizes_Part2;
using WatiN.Core;
using System.Text.RegularExpressions;
namespace Crawler_WithouSizes_Part2
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        #region DatbaseVariable
        SqlConnection Connection = new SqlConnection(System.Configuration.ConfigurationSettings.
                                               AppSettings["connectionstring"]);
        #endregion DatbaseVariable
        #region Buinesslayervariable
        List<BusinessLayer.Product> Products = new List<BusinessLayer.Product>();
        BusinessLayer.APPConfig Config = new BusinessLayer.APPConfig();
        BusinessLayer.Mail _Mail = new BusinessLayer.Mail();
        #endregion Buinesslayervariable
        StreamWriter _writer = new StreamWriter(Application.StartupPath + "/test.csv");
        #region booltypevariable


        bool _IScanadacomputers = true;
        bool _IsProduct = false;
        bool _IsCategorypaging = false;
        bool _IsSubcat = false;
        bool _Stop = false;
        bool Erorr_401_1 = true;
        bool Erorr_401_2 = true;
        #endregion booltypevariable
        #region intypevariable

        int FindCounter = 0;
        int _Workindex = 0;
        int _WorkIndex1 = 0;
        int _Pages = 0;
        int _TotalRecords = 0;
        int gridindex = 0;
        int time = 0;
        int _401index = 0;

        #endregion intypevariable
        #region stringtypevariable
        string BrandName1 = "";
        string BrandName2 = "";
        string Url1 = "";
        string Url2 = "";
        string _ScrapeUrl = "";
        string _Description1 = "";
        string _Description2 = "";

        #endregion listtypevariable
        #region listtypevariable

        List<string> _Url = new List<string>();
        List<string> _dateofbirth = new List<string>();
        Dictionary<string, string> Url = new Dictionary<string, string>();
        Dictionary<string, string> _ProductUrlthread1 = new Dictionary<string, string>();
        Dictionary<string, string> _ProductUrlthread2 = new Dictionary<string, string>();
        List<string> _Name = new List<string>();
        Dictionary<string, string> subCategoryUrl = new Dictionary<string, string>();
        Dictionary<string, string> CategoryUrl = new Dictionary<string, string>();
        #endregion stringtypevariable
        #region backgroundworker

        BackgroundWorker _Work = new BackgroundWorker();
        BackgroundWorker _Work1 = new BackgroundWorker();


        #endregion backgroundworker
        #region webclient

        WebClient _Client2 = new WebClient();
        WebClient _Client1 = new WebClient();
        WebClient _Client3 = new WebClient();
        WebClient _Client4 = new WebClient();

        #endregion webclient
        #region htmlagility

        HtmlAgilityPack.HtmlDocument _Work1doc = new HtmlAgilityPack.HtmlDocument();
        HtmlAgilityPack.HtmlDocument _Work1doc2 = new HtmlAgilityPack.HtmlDocument();
        HtmlAgilityPack.HtmlDocument _Work1doc3 = new HtmlAgilityPack.HtmlDocument();
        HtmlAgilityPack.HtmlDocument _Work1doc4 = new HtmlAgilityPack.HtmlDocument();

        #endregion htmlagility
        #region IeVariable

        IE _Worker1 = null;
        IE _Worker2 = null;

        #endregion IeVariable

        public Form1()
        {
            InitializeComponent();
            #region backrgoundworketevendeclaration
            _Work.WorkerReportsProgress = true;
            _Work.WorkerSupportsCancellation = true;
            _Work.ProgressChanged += new ProgressChangedEventHandler(Work_ProgressChanged);
            _Work.RunWorkerCompleted += new RunWorkerCompletedEventHandler(work_RunWorkerAsync);

            _Work.DoWork += new DoWorkEventHandler(work_dowork);
            _Work1.WorkerReportsProgress = true;
            _Work1.WorkerSupportsCancellation = true;
            _Work1.ProgressChanged += new ProgressChangedEventHandler(Work1_ProgressChanged);
            _Work1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(work_RunWorkerAsync1);
            _Work1.DoWork += new DoWorkEventHandler(work_dowork1);

            #endregion backrgoundworketevendeclaration
        }

        public void DisplayRecordProcessdetails(string Message, string TotalrecordMessage)
        {
            _lblerror.Visible = true;
            _lblerror.Text = Message;
            totalrecord.Visible = true;
            totalrecord.Text = TotalrecordMessage;

        }
        public void Process()
        {

            _IsProduct = false;
            _Name.Clear();
            CategoryUrl.Clear();
            _ProductUrlthread1.Clear();
            _ProductUrlthread1.Clear();
            Url.Clear();
            _percent.Visible = false;
            _Bar1.Value = 0;
            _Url.Clear();
            _lblerror.Visible = false;
            _Pages = 0;
            _TotalRecords = 0;
            gridindex = 0;
            _Stop = false;
            time = 0;


            #region canadacomputers.com
            _IScanadacomputers = true;
            _ScrapeUrl = "http://www.canadacomputers.com/asus/notebooks.php";

            try
            {
                //_Worker1 = new IE();
                //_Worker2 = new IE();
                _lblerror.Visible = true;
                _lblerror.Text = "We are going to read  category url of " + chkstorelist.Items[0].ToString() + " Website";
                _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));


                HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"hd-nav-prod-dropdn\"]");
                if (_Collection != null)
                {
                    HtmlNodeCollection _Collection11 = _Collection[0].SelectNodes(".//a");
                    if (_Collection11 != null)
                    {
                        foreach (HtmlNode node in _Collection11)
                        {
                            foreach (HtmlAttribute att in node.Attributes)
                            {
                                if (att.Name == "href")
                                    try
                                    {
                                        if (att.Value.Trim() != string.Empty && att.Value.Trim() != "#")
                                            CategoryUrl.Add((att.Value.ToLower().Contains("canadacomputers.com") ? "" : "http://www.canadacomputers.com") + att.Value.Replace("..", ""), "CANCOM" + node.InnerText.Trim());

                                    }
                                    catch
                                    { }
                            }
                        }
                    }
                }

                DisplayRecordProcessdetails("We are going to read product url from category pages for " + chkstorelist.Items[0].ToString() + " Website", "Total  Category :" + CategoryUrl.Count());

                if (File.Exists(Application.StartupPath + "/Files/Url.txt"))
                {
                    FileInfo _Info = new FileInfo(Application.StartupPath + "/Files/Url.txt");
                    int Days = 14;
                    try
                    {
                        Days = Convert.ToInt32(Config.GetAppConfigValue("canadacomputers.com", "FrequencyOfCategoryScrapping"));
                    }
                    catch
                    {
                    }
                    if (_Info.CreationTime < DateTime.Now.AddDays(-Days))
                        _IsCategorypaging = true;
                    else
                        _IsCategorypaging = false;
                }
                else
                    _IsCategorypaging = true;

                if (_IsCategorypaging)
                {
                    int i = 0;

                    foreach (var Caturl in CategoryUrl)
                    {
                        try
                        {
                            while (_Work.IsBusy && _Work1.IsBusy)
                            {
                                Application.DoEvents();

                            }

                            while (_Stop)
                            {
                                Application.DoEvents();
                            }



                            if (!_Work.IsBusy)
                            {
                                Url1 = Caturl.Key;
                                BrandName1 = Caturl.Value;
                                _Work.RunWorkerAsync();
                            }

                            else
                            {
                                Url2 = Caturl.Key;
                                BrandName2 = Caturl.Value;
                                _Work1.RunWorkerAsync();

                            }

                        }
                        catch { }

                    }
                    while (_Work.IsBusy || _Work1.IsBusy)
                    {
                        Application.DoEvents();

                    }
                    DisplayRecordProcessdetails("We are going to read product url from sub-category pages for " + chkstorelist.Items[0].ToString() + " Website", "Total  Category :" + CategoryUrl.Count());

                    _401index = 0;
                    _IsSubcat = true;
                    foreach (var Caturl in subCategoryUrl)
                    {
                        try
                        {
                            while (_Work.IsBusy && _Work1.IsBusy)
                            {
                                Application.DoEvents();

                            }

                            while (_Stop)
                            {
                                Application.DoEvents();
                            }



                            if (!_Work.IsBusy)
                            {
                                Url1 = Caturl.Key;
                                BrandName1 = Caturl.Value;
                                _Work.RunWorkerAsync();
                            }

                            else
                            {
                                Url2 = Caturl.Key;
                                BrandName2 = Caturl.Value;
                                _Work1.RunWorkerAsync();

                            }

                        }
                        catch (Exception exp)
                        {
                        }


                    }
                }
                while (_Work.IsBusy || _Work1.IsBusy)
                {
                    Application.DoEvents();

                }

                System.Threading.Thread.Sleep(1000);
                _Bar1.Value = 0;
                _401index = 0;

                #region Code to get and write urls from File

                if (File.Exists(Application.StartupPath + "/Files/Url.txt"))
                {
                    if (!_IsCategorypaging)
                    {
                        using (StreamReader Reader = new StreamReader(Application.StartupPath + "/Files/Url.txt"))
                        {
                            string line = "";
                            while ((line = Reader.ReadLine()) != null)
                            {
                                try
                                {
                                    Url.Add(line.Split(new[] { "@#$#" }, StringSplitOptions.None)[0], line.Split(new[] { "@#$#" }, StringSplitOptions.None)[1]);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }


                foreach (var url in _ProductUrlthread1)
                {
                    try
                    {
                        if (!Url.Keys.Contains(url.Key.ToLower()))
                            Url.Add(url.Key.ToLower(), url.Value);
                    }
                    catch
                    {
                    }
                }

                foreach (var url in _ProductUrlthread2)
                {
                    try
                    {
                        if (!Url.Keys.Contains(url.Key.ToLower()))
                            Url.Add(url.Key.ToLower(), url.Value);
                    }
                    catch
                    {
                    }
                }

                // Code to write in file
                if (_IsCategorypaging)
                {
                    using (StreamWriter writer = new StreamWriter(Application.StartupPath + "/Files/Url.txt"))
                    {

                        foreach (var PrdUrl in Url)
                        {
                            writer.WriteLine(PrdUrl.Key + "@#$#" + PrdUrl.Value);
                        }
                    }
                }
                #endregion Code to get and write urls from File

                _IsCategorypaging = false;

                DisplayRecordProcessdetails("We are going to read Product information for   " + chkstorelist.Items[0].ToString() + " Website", "Total  products :" + Url.Count());

                _IsProduct = true;

                foreach (var PrdUrl in Url)
                {
                    try
                    {
                        while (_Work.IsBusy && _Work1.IsBusy)
                        {
                            Application.DoEvents();

                        }
                        while (_Stop)
                        {
                            Application.DoEvents();
                        }
                        if (!_Work.IsBusy)
                        {
                            Url1 = PrdUrl.Key;
                            BrandName1 = PrdUrl.Value;
                            _Work.RunWorkerAsync();
                        }
                        else
                        {
                            Url2 = PrdUrl.Key;
                            BrandName2 = PrdUrl.Value;
                            _Work1.RunWorkerAsync();
                        }
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }

                }
                while (_Work.IsBusy || _Work1.IsBusy)
                {
                    Application.DoEvents();

                }




                if (Products.Count() > 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    _lblerror.Visible = true;
                    BusinessLayer.ProductMerge _Prd = new BusinessLayer.ProductMerge();
                    _Prd.ProductDatabaseIntegration(Products, "canadacomputers.com", 1);
                }
                else
                {
                    BusinessLayer.DB _Db = new BusinessLayer.DB();
                    _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='canadacomputers.com'");
                    _Mail.SendMail("OOPS there is no any product scrapped by app for canadacomputers.com Website." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

                }
            }
            catch
            {
                BusinessLayer.DB _Db = new BusinessLayer.DB();
                _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='canadacomputers.com'");
                _lblerror.Visible = true;
                _Mail.SendMail("Oops Some issue Occured in scrapping data canadacomputers.com  Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

            }

            #region closeIEinstance
            try
            {
                _Worker1.Close();
                _Worker2.Close();
            }
            catch
            {
            }
            #endregion closeIEinstance
            #endregion
            _writer.Close();
            this.Close();
        }
        public void work_dowork(object sender, DoWorkEventArgs e)
        {

            Erorr_401_1 = true;
            if (_IScanadacomputers)
            {
                try
                {
                    int CounterError = 0;
                    do
                    {
                        try
                        {
                            _Work1doc.LoadHtml(_Client1.DownloadString(Url1));
                            Erorr_401_1 = false;
                        }
                        catch
                        {
                            CounterError++;
                        }
                    } while (Erorr_401_1 && CounterError < 20);
                }
                catch { }
            }
            int index = 0;

            #region canadacomputers.com

            if (_IsCategorypaging)
            {
                #region canadacomputers.com

                if (!Erorr_401_1)
                {
                    try
                    {
                        #region getChildCat
                        if (!_IsSubcat)
                        {
                            HtmlNodeCollection _SubCat = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"sub_cat1\"]");
                            if (_SubCat != null)
                            {
                                foreach (HtmlNode node in _SubCat)
                                {
                                    foreach (HtmlNode subnode in node.SelectNodes(".//a"))
                                    {
                                        foreach (HtmlAttribute attr in subnode.Attributes)
                                        {
                                            if (attr.Name == "href")
                                            {
                                                try
                                                {
                                                    subCategoryUrl.Add("http://www.canadacomputers.com" + attr.Value.Replace("..", ""), "CANCOM" + node.InnerText.Trim());
                                                }
                                                catch
                                                { }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion getChildCat

                        HtmlNodeCollection coll = _Work1doc.DocumentNode.SelectNodes("//td[@class=\"productListing-data\"]/form");
                        if (coll != null)
                        {
                            int Stock = 0;
                            foreach (HtmlNode node in coll)
                            {
                                Stock = 0;
                                HtmlNodeCollection collStock = node.SelectNodes("..//div[@class=\"availability_text\"]");
                                if (collStock != null)
                                {
                                    if (collStock[0].InnerText.ToLower().Contains("available online") && !collStock[0].InnerText.ToLower().Contains("not available online"))
                                        Stock = 1;
                                }
                                if (Stock == 1)
                                {
                                    HtmlNodeCollection collPrdUrl = node.SelectNodes("..//div[@class=\"item_description\"]/a");
                                    if (collPrdUrl != null)
                                    {
                                        foreach (HtmlAttribute attr in collPrdUrl[0].Attributes)
                                        {
                                            if (attr.Name == "href")
                                            {
                                                try
                                                {
                                                    if (!CheckUrlExist("http://www.canadacomputers.com" + attr.Value.Replace("..", "")))
                                                        _ProductUrlthread1.Add("http://www.canadacomputers.com" + attr.Value.Replace("..", ""), BrandName1);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _writer.WriteLine(Url1 + " " + "workerexp1 " + "Product Url is not found");
                                    }
                                }
                            }
                            try
                            {
                                int TotalRecords = 0;
                                int TotalPages = 0;
                                int CurrentPage = 1;
                                HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//td[@class=\"pageHeading\"]/b");
                                if (_Collection != null)
                                {

                                    int.TryParse(_Collection[0].InnerText.Trim(), out TotalRecords);
                                    if (TotalRecords != 0)
                                    {
                                        if (TotalRecords % 20 == 0)
                                        {
                                            TotalPages = Convert.ToInt32(TotalRecords / 20);
                                        }
                                        else
                                        {
                                            TotalPages = Convert.ToInt32(TotalRecords / 20) + 1;
                                        }
                                    }
                                    else
                                        _writer.WriteLine(Url1 + " " + "workerexp1 " + "Total records Tags Not found");
                                }


                                for (int i = 2; i <= TotalPages; i++)
                                {
                                    Erorr_401_1 = true;
                                    try
                                    {
                                        int CounterError = 0;
                                        do
                                        {
                                            try
                                            {
                                                _Work1doc.LoadHtml(_Client1.DownloadString(Url1.Contains("?") ? Url1 + "&page=" + i : Url1 + "?page=" + i));
                                                Erorr_401_1 = false;
                                            }
                                            catch
                                            {
                                                CounterError++;
                                            }
                                        } while (Erorr_401_1 && CounterError < 20);
                                        if (!Erorr_401_1)
                                        {

                                            HtmlNodeCollection coll1 = _Work1doc.DocumentNode.SelectNodes("//td[@class=\"productListing-data\"]/form");
                                            if (coll != null)
                                            {
                                                foreach (HtmlNode node in coll1)
                                                {
                                                    Stock = 0;
                                                    HtmlNodeCollection collStock = node.SelectNodes("..//div[@class=\"availability_text\"]");
                                                    if (collStock != null)
                                                    {
                                                        if (collStock[0].InnerText.ToLower().Contains("available online") && !collStock[0].InnerText.ToLower().Contains("not available online"))
                                                            Stock = 1;
                                                    }
                                                    if (Stock == 1)
                                                    {
                                                        HtmlNodeCollection collPrdUrl = node.SelectNodes("..//div[@class=\"item_description\"]/a");
                                                        if (collPrdUrl != null)
                                                        {
                                                            foreach (HtmlAttribute attr in collPrdUrl[0].Attributes)
                                                            {
                                                                if (attr.Name == "href")
                                                                {
                                                                    try
                                                                    {
                                                                        if (!CheckUrlExist("http://www.canadacomputers.com" + attr.Value.Replace("..", "")))
                                                                            _ProductUrlthread1.Add("http://www.canadacomputers.com" + attr.Value.Replace("..", ""), BrandName1);
                                                                    }
                                                                    catch
                                                                    {
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            _writer.WriteLine(Url1 + " " + "workerexp1 " + "Product Url is not found");
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    catch { }
                                }

                            }
                            catch
                            {
                                _writer.WriteLine(Url1 + " " + "workerexp1 " + "Total records Tags Not found");
                            }


                        }
                        _401index++;
                        _Work.ReportProgress((_401index * 100 / (_IsSubcat == false ? CategoryUrl.Count() : subCategoryUrl.Count())));

                #endregion canadacomputers.com
                    }
                    catch
                    {
                    }

                }
            }
            else if (_IsProduct)
            {
                if (!Erorr_401_1)
                {
                    try
                    {
                        GetProductInfo(_Work1doc, Url1, BrandName1);
                    }
                    catch (Exception exp)
                    {
                        _writer.WriteLine(Url1 + " " + " product page exception workerexp4 " + exp.Message);
                    }
                }
                _401index++;
                _Work.ReportProgress((_401index * 100 / Url.Count()));
            }


            #endregion canadacomputers.com
        }

        public void work_dowork1(object sender, DoWorkEventArgs e)
        {

            Erorr_401_2 = true;
            if (_IScanadacomputers)
            {
                try
                {
                    int CounterError = 0;
                    do
                    {
                        try
                        {
                            _Work1doc2.LoadHtml(_Client2.DownloadString(Url2));
                            Erorr_401_2 = false;
                        }
                        catch
                        {
                            CounterError++;
                        }
                    } while (Erorr_401_2 && CounterError < 20);
                }
                catch { }
            }
            int index = 0;

            #region canadacomputers.com

            if (_IsCategorypaging)
            {
                #region canadacomputers.com

                if (!Erorr_401_2)
                {
                    try
                    {
                        #region getChildCat
                        if (!_IsSubcat)
                        {
                            HtmlNodeCollection _SubCat = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"sub_cat1\"]");
                            if (_SubCat != null)
                            {
                                foreach (HtmlNode node in _SubCat)
                                {
                                    foreach (HtmlNode subnode in node.SelectNodes(".//a"))
                                    {
                                        foreach (HtmlAttribute attr in subnode.Attributes)
                                        {
                                            if (attr.Name == "href")
                                            {
                                                try
                                                {
                                                    subCategoryUrl.Add("http://www.canadacomputers.com" + attr.Value.Replace("..", ""), "CANCOM" + node.InnerText.Trim());
                                                }
                                                catch
                                                { }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion getChildCat

                        HtmlNodeCollection coll = _Work1doc2.DocumentNode.SelectNodes("//td[@class=\"productListing-data\"]/form");
                        if (coll != null)
                        {
                            int Stock = 0;
                            foreach (HtmlNode node in coll)
                            {
                                Stock = 0;
                                HtmlNodeCollection collStock = node.SelectNodes("..//div[@class=\"availability_text\"]");
                                if (collStock != null)
                                {
                                    if (collStock[0].InnerText.ToLower().Contains("available online") && !collStock[0].InnerText.ToLower().Contains("not available online"))
                                        Stock = 1;
                                }
                                if (Stock == 1)
                                {
                                    HtmlNodeCollection collPrdUrl = node.SelectNodes("..//div[@class=\"item_description\"]/a");
                                    if (collPrdUrl != null)
                                    {
                                        foreach (HtmlAttribute attr in collPrdUrl[0].Attributes)
                                        {
                                            if (attr.Name == "href")
                                            {
                                                try
                                                {
                                                    if (!CheckUrlExist("http://www.canadacomputers.com" + attr.Value.Replace("..", "")))
                                                        _ProductUrlthread1.Add("http://www.canadacomputers.com" + attr.Value.Replace("..", ""), BrandName2);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _writer.WriteLine(Url2 + " " + "workerexp1 " + "Product Url is not found");
                                    }
                                }
                            }
                            try
                            {
                                int TotalRecords = 0;
                                int TotalPages = 0;
                                int CurrentPage = 1;
                                HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//td[@class=\"pageHeading\"]/b");
                                if (_Collection != null)
                                {

                                    int.TryParse(_Collection[0].InnerText.Trim(), out TotalRecords);
                                    if (TotalRecords != 0)
                                    {
                                        if (TotalRecords % 20 == 0)
                                        {
                                            TotalPages = Convert.ToInt32(TotalRecords / 20);
                                        }
                                        else
                                        {
                                            TotalPages = Convert.ToInt32(TotalRecords / 20) + 1;
                                        }
                                    }
                                    else
                                        _writer.WriteLine(Url2 + " " + "workerexp1 " + "Total records Tags Not found");
                                }


                                for (int i = 2; i <= TotalPages; i++)
                                {
                                    Erorr_401_2 = true;
                                    try
                                    {
                                        int CounterError = 0;
                                        do
                                        {
                                            try
                                            {
                                                _Work1doc2.LoadHtml(_Client2.DownloadString(Url2.Contains("?") ? Url2 + "&page=" + i : Url2 + "?page=" + i));
                                                Erorr_401_2 = false;
                                            }
                                            catch
                                            {
                                                CounterError++;
                                            }
                                        } while (Erorr_401_2 && CounterError < 20);
                                        if (!Erorr_401_2)
                                        {

                                            HtmlNodeCollection coll1 = _Work1doc2.DocumentNode.SelectNodes("//td[@class=\"productListing-data\"]/form");
                                            if (coll != null)
                                            {
                                                foreach (HtmlNode node in coll1)
                                                {
                                                    Stock = 0;
                                                    HtmlNodeCollection collStock = node.SelectNodes("..//div[@class=\"availability_text\"]");
                                                    if (collStock != null)
                                                    {
                                                        if (collStock[0].InnerText.ToLower().Contains("available online") && !collStock[0].InnerText.ToLower().Contains("not available online"))
                                                            Stock = 1;
                                                    }
                                                    if (Stock == 1)
                                                    {
                                                        HtmlNodeCollection collPrdUrl = node.SelectNodes("..//div[@class=\"item_description\"]/a");
                                                        if (collPrdUrl != null)
                                                        {
                                                            foreach (HtmlAttribute attr in collPrdUrl[0].Attributes)
                                                            {
                                                                if (attr.Name == "href")
                                                                {
                                                                    try
                                                                    {
                                                                        if (!CheckUrlExist("http://www.canadacomputers.com" + attr.Value.Replace("..", "")))
                                                                            _ProductUrlthread1.Add("http://www.canadacomputers.com" + attr.Value.Replace("..", ""), BrandName2);
                                                                    }
                                                                    catch
                                                                    {
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            _writer.WriteLine(Url2 + " " + "workerexp1 " + "Product Url is not found");
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    catch { }
                                }

                            }
                            catch
                            {
                                _writer.WriteLine(Url2 + " " + "workerexp1 " + "Total records Tags Not found");
                            }


                        }
                        _401index++;
                        _Work1.ReportProgress((_401index * 100 / (_IsSubcat == false ? CategoryUrl.Count() : subCategoryUrl.Count())));

                #endregion canadacomputers.com
                    }
                    catch
                    {
                    }

                }
            }
            else if (_IsProduct)
            {
                if (!Erorr_401_2)
                {
                    try
                    {
                        GetProductInfo(_Work1doc2, Url2, BrandName2);
                    }
                    catch (Exception exp)
                    {
                        _writer.WriteLine(Url2 + " " + " product page exception workerexp4 " + exp.Message);
                    }
                }
                _401index++;
                _Work1.ReportProgress((_401index * 100 / Url.Count()));
            }


            #endregion canadacomputers.com
        }
        public void work_RunWorkerAsync(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        public void work_RunWorkerAsync1(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        public string Removeunsuaalcharcterfromstring(string name)
        {
            return name.Replace("â€“", "-").Replace("Ã±", "ñ").Replace("â€™", "'").Replace("Ã¢â‚¬â„¢", "'").Replace("ÃƒÂ±", "ñ").Replace("Ã¢â‚¬â€œ", "-").Replace("Â ", "").Replace("Â", "").Trim();

        }
        private string StripHTML(string source)
        {
            try
            {
                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                      @"( )+", " ");
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @" ", " ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&bull;", " * ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lsaquo;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&rsaquo;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&trade;", "(tm)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&frasl;", "/",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lt;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&gt;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&copy;", "(c)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&reg;", "(r)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                // That's it.
                return result;
            }
            catch
            {

                return source;
            }
        }
        public void GetProductInfo(HtmlAgilityPack.HtmlDocument _doc, string url, string Category)
        {


            BusinessLayer.Product product = new BusinessLayer.Product();
            try
            {

                string Bullets = "";
                #region title
                HtmlNodeCollection formColl = _doc.DocumentNode.SelectNodes("//div[@class=\"item_title\"]/h1");
                if (formColl != null)
                    product.Name = System.Net.WebUtility.HtmlDecode(formColl[0].InnerText).Trim();

                else if (_doc.DocumentNode.SelectNodes("//title") != null)
                    product.Name = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//title")[0].InnerText).Trim();
                else
                    _writer.WriteLine(url + " " + "title not found");
                #endregion title

                #region Price
                string priceString = "";
                double Price = 0;
                if (_doc.DocumentNode.SelectNodes("//span[@id=\"SalePrice\"]") != null)
                {
                    priceString = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//span[@id=\"SalePrice\"]")[0].InnerText).Replace("$", "").Trim();
                    double.TryParse(priceString, out Price);
                    if (Price != 0)
                        product.Price = Price.ToString(); //System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//span[@class=\"regular-price\"]/span[@class=\"price\"]")[0].InnerText).re;
                    else
                        _writer.WriteLine(url + " " + "Price not found");
                }
                else
                    _writer.WriteLine(url + " " + "Price not found");
                #endregion Price


                #region Brand
                try
                {
                    if (_doc.DocumentNode.SelectNodes("//span[@class=\"icon\"]/img") != null)
                    {
                        if (_doc.DocumentNode.SelectNodes("//span[@class=\"icon\"]/img")[0].Attributes["src"].Value.ToLower().Contains("logos"))
                        {
                            product.Brand = _doc.DocumentNode.SelectNodes("//span[@class=\"icon\"]/img")[0].Attributes["src"].Value.Replace("logos", "").Replace("/", "").Replace(".gif", "").Replace(".png", "").Replace(".jpg", "").Trim();
                            product.Manufacturer = _doc.DocumentNode.SelectNodes("//span[@class=\"icon\"]/img")[0].Attributes["src"].Value.Replace("logos", "").Replace("/", "").Replace(".gif", "").Replace(".png", "").Replace(".jpg", "").Trim();

                        }
                    }
                }
                catch
                {
                }
                if (string.IsNullOrEmpty(product.Brand))
                {
                    product.Brand = "JZ HOLDINGS";
                    product.Manufacturer = "JZ HOLDINGS";
                }
                #endregion Brand

                #region Category
                product.Category = string.IsNullOrEmpty(Category) ? "CANCOMJZ HOLDINGS" : Category;
                #endregion Category

                product.Currency = "CAD";


                int descIndex = 0;
                int specIndex = 0;
                int stockIndex = 0;
                #region description
                string Description = "";

                #region Get Description,stock abnd SpecificationIndex
                HtmlNodeCollection index = _doc.DocumentNode.SelectNodes("//ul[@class=\"TabbedPanelsTabGroup\"]/li");
                if (index != null)
                {
                    int counter = 0;
                    foreach (HtmlNode node in index)
                    {
                        if (node.InnerText.Trim().ToLower().Trim() == "stock level")
                            stockIndex = counter;
                        else if (node.InnerText.Trim().ToLower().Trim() == "overview")
                            descIndex = counter;
                        else if (node.InnerText.Trim().ToLower().Trim() == "specifications")
                            specIndex = counter;
                        counter++;
                    }
                }

                #endregion Get Description,stock abnd SpecificationIndex
                HtmlNodeCollection desCollection = _doc.DocumentNode.SelectNodes("//div[@class=\"TabbedPanelsContentGroup\"]/div");
                if (descIndex != 0)
                {
                    if (desCollection != null)
                    {
                        try
                        {
                            foreach (HtmlNode node in desCollection[descIndex].ChildNodes)
                            {
                                if (node.Name != "script")
                                {

                                    Description = Description + node.InnerHtml;
                                }
                            }

                            Description = Removeunsuaalcharcterfromstring(StripHTML(Description).Trim());
                            try
                            {
                                if (Description.Length > 2000)
                                    Description = Description.Substring(0, 1997) + "...";
                            }
                            catch
                            {
                            }
                            product.Description = System.Net.WebUtility.HtmlDecode(Description.Replace("Â", ""));
                        }
                        catch
                        {
                            _writer.WriteLine(url + " " + "Description not found");
                        }
                    }
                    else
                        _writer.WriteLine(url + " " + "Description not found");
                }
                else
                    _writer.WriteLine(url + " " + "Description not found");

                #endregion description

                #region BulletPoints
                string Feature = "";
                if (specIndex != 0)
                {
                    if (desCollection != null)
                    {
                        string Header = "";
                        string Value = "";
                        int PointCounter = 1;
                        try
                        {
                            if (desCollection[specIndex].SelectNodes(".//td[@class=\"specification\"]") != null)
                            {
                                foreach (HtmlNode node in desCollection[specIndex].SelectNodes(".//td[@class=\"specification\"]"))
                                {
                                    try
                                    {
                                        Header = System.Net.WebUtility.HtmlDecode(node.InnerText.Trim());
                                        Value = System.Net.WebUtility.HtmlDecode(node.NextSibling.InnerText.Trim());
                                        if (Value != "" && Header.Length < 100 && !Header.ToLower().Contains("warranty") && !Header.ToLower().Contains("return"))
                                        {
                                            Feature = "  " + Header + "  " + Value;
                                            if (Feature.Length > 480)
                                                Feature = Feature.Substring(0, 480);
                                            if (Bullets.Length + Feature.Length + 2 <= PointCounter * 480)
                                                Bullets = Bullets + Feature + ". ";
                                            else
                                            {
                                                Bullets = Bullets + "@@" + Feature + ". ";
                                                PointCounter++;
                                            }

                                            if (Header.ToLower() == "brand")
                                            {
                                                product.Brand = Value;
                                                product.Manufacturer = Value;
                                            }
                                        }

                                    }
                                    catch { }
                                }
                            }
                            else
                            {
                                Bullets = Removeunsuaalcharcterfromstring(StripHTML(desCollection[specIndex].InnerText).Trim());
                                if (Bullets.Length > 1000)
                                    Bullets = Bullets.Substring(0, 990) + "...";
                            }
                        }
                        catch { }


                        if (!string.IsNullOrEmpty(Bullets))
                            Bullets = Bullets.Trim();

                    }
                    else
                        _writer.WriteLine(url + " " + "Bullet Points  not found");
                }
                else
                    _writer.WriteLine(url + " " + "Bullet Points  not found");
                if (Bullets.Length > 0)
                {
                    Bullets = Removeunsuaalcharcterfromstring(StripHTML(Bullets).Trim());
                    string[] BulletPoints = Bullets.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < BulletPoints.Length; i++)
                    {
                        if (i == 0)
                            product.Bulletpoints1 = BulletPoints[i].ToString();
                        if (i == 1)
                            product.Bulletpoints2 = BulletPoints[i].ToString();
                        if (i == 2)
                            product.Bulletpoints3 = BulletPoints[i].ToString();
                        if (i == 3)
                            product.Bulletpoints4 = BulletPoints[i].ToString();
                        if (i == 4)
                            product.Bulletpoints5 = BulletPoints[i].ToString();
                        if (i > 4)
                            break;
                    }

                }
                if (string.IsNullOrEmpty(product.Description))
                {
                    product.Description = product.Name;
                    if (string.IsNullOrEmpty(product.Bulletpoints1))
                        product.Bulletpoints1 = product.Name;
                }
                else if (string.IsNullOrEmpty(product.Bulletpoints1))
                {
                    if (product.Description.Length >= 500)
                        product.Bulletpoints1 = product.Description.Substring(0, 497);
                    else
                        product.Bulletpoints1 = product.Description;
                }


                #endregion BulletPoints


                #region Image
                string Images = "";
                HtmlNodeCollection imgCollection = _doc.DocumentNode.SelectNodes("//div[@class=\"prod_thumb\"]");
                if (imgCollection != null)
                {
                    if (imgCollection[0].SelectNodes(".//img") != null)
                    {
                        foreach (HtmlNode node in imgCollection[0].SelectNodes(".//img"))
                        {
                            foreach (HtmlAttribute attr in node.Attributes)
                            {
                                if (attr.Name == "src")
                                {
                                    try
                                    {
                                        if (!node.Attributes["title"].Value.Trim().ToLower().Contains("image not available"))
                                            Images = Images + attr.Value.Trim().Replace("40x40", "450x450") + ",";
                                    }
                                    catch
                                    {
                                        Images = Images + attr.Value.Trim().Replace("40x40", "450x450") + ",";
                                    }
                                    break;
                                }
                            }

                        }
                    }
                }
                else
                    _writer.WriteLine(url + " " + "Main Images  not found");
                if (string.IsNullOrEmpty(Images))
                {
                    HtmlNodeCollection imgCollection1 = _doc.DocumentNode.SelectNodes("//div[@class=\"preview_img\"]/img");
                    if (imgCollection != null)
                    {
                        try
                        {
                            if (!imgCollection1[0].Attributes["title"].Value.Trim().ToLower().Contains("image not available"))
                                Images = imgCollection1[0].Attributes["src"].Value.Trim();
                        }
                        catch
                        { Images = imgCollection1[0].Attributes["src"].Value.Trim(); }
                    }
                }

                if (Images.Length > 0)
                {
                    if (Images.Contains(","))
                        Images = Images.Substring(0, Images.Length - 1);
                }

                product.Image = Images;
                #endregion Image
                product.Isparent = true;
                #region sku
                string sku = "";
                if (_doc.DocumentNode.SelectNodes("//span[@class=\"itdetail\"]/strong") != null)
                {
                    try
                    {
                        foreach (HtmlNode Node in _doc.DocumentNode.SelectNodes("//span[@class=\"itdetail\"]/strong"))
                        {
                            if (Node.InnerText.ToLower().Contains("part number:"))
                                product.ManPartNO = Removeunsuaalcharcterfromstring(StripHTML(Node.NextSibling.InnerText)).Replace("|", "").Trim();
                            else if (Node.InnerText.ToLower().Contains("item code:"))
                            {
                                //  product.SKU =
                                //  product.parentsku = string.IsNullOrEmpty(Removeunsuaalcharcterfromstring(StripHTML(Node.NextSibling.InnerText))) ? "" : "CANCOM" + Removeunsuaalcharcterfromstring(StripHTML(Node.NextSibling.InnerText)).Replace("|", "").Trim();
                            }
                        }
                    }
                    catch
                    { }
                }
                else
                    _writer.WriteLine(url + " " + "Model  not found");

                if (_doc.DocumentNode.SelectNodes("//input[@name=\"item_id\"]") != null)
                {
                    product.SKU = string.IsNullOrEmpty(Removeunsuaalcharcterfromstring(StripHTML(_doc.DocumentNode.SelectNodes("//input[@name=\"item_id\"]")[0].Attributes["value"].Value))) ? "" : "CANCOM" + Removeunsuaalcharcterfromstring(StripHTML(_doc.DocumentNode.SelectNodes("//input[@name=\"item_id\"]")[0].Attributes["value"].Value)).Replace("|", "").Trim();
                    product.parentsku = string.IsNullOrEmpty(Removeunsuaalcharcterfromstring(StripHTML(_doc.DocumentNode.SelectNodes("//input[@name=\"item_id\"]")[0].Attributes["value"].Value))) ? "" : "CANCOM" + Removeunsuaalcharcterfromstring(StripHTML(_doc.DocumentNode.SelectNodes("//input[@name=\"item_id\"]")[0].Attributes["value"].Value)).Replace("|", "").Trim();

                }
                else
                    _writer.WriteLine(url + " " + "Sku  not found");
                #endregion sku




                #region stock
                product.Stock = "0";
                int Stock = 0;

                if (desCollection != null)
                {
                    string Header = "";
                    string Value = "";
                    int PointCounter = 1;
                    try
                    {
                        HtmlNodeCollection stockNodes = desCollection[stockIndex].SelectNodes(".//td");
                        if (stockNodes != null)
                        {

                            for (int i = 0; i < stockNodes.Nodes().Count(); i++)
                            {
                                if (stockNodes[i].InnerText.ToLower().Trim() == "online store")
                                {

                                    int.TryParse(stockNodes[i + 1].InnerText.Trim(), out Stock);
                                    break;

                                }


                            }
                        }
                    }
                    catch
                    { Stock = 2; }
                }

                product.Stock = Stock > 1 ? "1" : "0";
                #endregion stock
                product.URL = url;
                if (!string.IsNullOrEmpty(product.Image))
                    if (!product.Image.ToLower().Contains("ina.jpg"))
                        Products.Add(product);
            }
            catch (Exception exp)
            {
                _writer.WriteLine(url + " " + "Issue accured in reading product info from given product url. exp: " + exp.Message);
            }

        }

        public bool CheckImageExist(string url)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";


            try
            {
                response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }

        }
        public string GenrateSkuFromDatbase(string sku, string Name, string storename, decimal Price, string url)
        {

            string Result = sku;
            try
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    if (Connection.State == ConnectionState.Closed)
                        Connection.Open();
                    Cmd.Connection = Connection;
                    Cmd.Parameters.AddWithValue("@SKU", sku);
                    Cmd.Parameters.AddWithValue("@Name", Name);
                    Cmd.Parameters.AddWithValue("@Storename", storename);
                    Cmd.Parameters.AddWithValue("@URL", url);
                    Cmd.CommandText = "Getsku";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = Cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Result = dr[0].ToString();
                        }
                    }
                    dr.Close();
                }
            }
            catch
            {

            }
            return Result;
        }
        public void Work_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _Bar1.Value = e.ProgressPercentage;
            _percent.Visible = true;
            _percent.Text = e.ProgressPercentage + "% Completed. Record Processed " + _401index;
        }
        public void Work1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _Bar1.Value = e.ProgressPercentage;
            _percent.Visible = true;
            _percent.Text = e.ProgressPercentage + "% Completed. Record Processed " + _401index;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }
        private void Go_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            /***************Grid view************************************/
            totalrecord.Visible = false;
            _lblerror.Visible = false;
            _percent.Visible = false;
        }
        private void btnsubmit_Click(object sender, System.EventArgs e)
        {


            Process();

        }

        public bool CheckUrlExist(string Url)
        {
            bool result = true;
            try
            {
                string[] parts = Url.Split('&');
                foreach (string part in parts)
                {
                    if (part.ToLower().Contains("item_id"))
                    {
                        if ((from dic in _ProductUrlthread1
                             where dic.Key.IndexOf(part) >= 0
                             select dic).Count() == 0)
                            result = false;

                    }
                }
            }
            catch
            {


            }
            return result;
        }
        private void Form1_Shown(object sender, System.EventArgs e)
        {
            base.Show();
            this.btnsubmit_Click(null, null);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
