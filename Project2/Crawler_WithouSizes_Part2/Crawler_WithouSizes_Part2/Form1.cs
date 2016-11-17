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

        bool _Isfind = false;
        bool _IS401games = true;
        bool _Isreadywebbrowser1 = false;
        bool _Isreadywebbrowser2 = false;
        bool _IsProduct = false;
        bool _IsCategory = true;
        bool _IsCategorypaging = false;
        bool _Issubcat = false;
        bool _Stop = false;
        bool _Iscompleted = false;
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
        string Bullets = "";
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
            _IsCategory = true;
            _Stop = false;
            time = 0;


            #region 401Games
            _IS401games = true;
            _ScrapeUrl = "http://store.401games.ca/";
            try
            {
                _Worker1 = new IE();
                _Worker2 = new IE();
                //    _Worker1.Visible = false;
                //   _Worker2.Visible = false;
                _lblerror.Visible = true;
                _lblerror.Text = "We are going to read  category url of " + chkstorelist.Items[0].ToString() + " Website";
                _Worker1.GoTo(_ScrapeUrl);
                _Worker1.WaitForComplete();
                System.Threading.Thread.Sleep(10000);
                _Work1doc.LoadHtml(_Worker1.Html);
                //HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"col left\"]");
                //CategoryUrl = CommanFunction.GetCategoryUrl(_Collection, "ul", "//li/a", "http://store.401games.ca", "#st=&begin=1&nhit=40");
                HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"sub-menu\"]");
             
                if (_Collection != null)
                {
                    HtmlNodeCollection menu = _Collection[0].SelectNodes("..//ul[@class=\"submenu\"]//li//a");
                    foreach (HtmlNode node in menu)
                    {
                        foreach (HtmlAttribute att in node.Attributes)
                        {
                            if (att.Name == "href")
                                CategoryUrl.Add(att.Value, node.InnerText.Trim());
                        }
                    }
                }
                CategoryUrl.Remove("http://store.401games.ca/product/sitemap/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.ca/service/shipping/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.ca/service/returns/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.ca/service/terms/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.ca/service/terms/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.ca/service/contact_us/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.ca/service/contact_us/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.ca/product/sitemap/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.cahttp://payd.moneris.com/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.ca/service/privacy/#st=&begin=1&nhit=40");
                CategoryUrl.Remove("http://store.401games.ca/catalog/93370C/pre-orders#st=&begin=1&nhit=40");
                DisplayRecordProcessdetails("We are going to read product url from category pages for " + chkstorelist.Items[0].ToString() + " Website", "Total  Category :" + CategoryUrl.Count());

                if (File.Exists(Application.StartupPath + "/Files/Url.txt"))
                {
                    FileInfo _Info = new FileInfo(Application.StartupPath + "/Files/Url.txt");
                    int Days = 14;
                    try
                    {
                        Days = Convert.ToInt32(Config.GetAppConfigValue("store.401games", "FrequencyOfCategoryScrapping"));
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
                        while (_Work.IsBusy || _Work1.IsBusy)
                        {
                            Application.DoEvents();

                        }
                        while (_Stop)
                        {
                            Application.DoEvents();
                        }
                        if (!_Work.IsBusy)
                        {
                            Url1 = "http://store.401games.ca" + PrdUrl.Key;
                            BrandName1 = PrdUrl.Value;
                            _Work.RunWorkerAsync();
                        }
                        else
                        {
                            Url2 = "http://store.401games.ca" + PrdUrl.Key;
                            BrandName2 = PrdUrl.Value;
                            _Work1.RunWorkerAsync();
                        }
                    }
                    catch
                    {
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
                    _Prd.ProductDatabaseIntegration(Products, "store.401games", 1);
                }
                else
                {
                    BusinessLayer.DB _Db = new BusinessLayer.DB();
                    _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='store.401games'");
                    _Mail.SendMail("OOPS there is no any product scrapped by app for store.401games Website." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

                }
            }
            catch
            {
                BusinessLayer.DB _Db = new BusinessLayer.DB();
                _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='store.401games'");
                _lblerror.Visible = true;
                _Mail.SendMail("Oops Some issue Occured in scrapping data store.401games  Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

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
            bool _Iserror = false;
            Erorr_401_1 = true;
            if (_IS401games)
            {
                if (_IsCategorypaging)
                {

                    try
                    {

                        int CounterError = 0;
                        do
                        {
                            try
                            {
                                _Worker1.GoTo(Url1);
                                Erorr_401_1 = false;
                            }
                            catch
                            {
                                CounterError++;
                            }
                        } while (Erorr_401_1 && CounterError < 20);
                    }
                    catch
                    {
                        _Iserror = true;
                    }
                }
                else if (_IsProduct)
                {
                    try
                    {
                        _Work1doc.LoadHtml(_Client1.DownloadString(Url1));
                        Erorr_401_1 = false;
                    }
                    catch
                    {
                        Erorr_401_1 = true;
                    }
                }
            }
            else
            {
                if (!_IsProduct)
                {
                    try
                    {
                        _Work1doc.LoadHtml(_Client1.DownloadString(Url1));
                    }
                    catch
                    {
                        _Iserror = true;
                    }
                }
            }



            int index = 0;
            #region 401games
            if (_IS401games)
            {
                #region 401categorypaging
                if (_IsCategorypaging)
                {
                    if (!Erorr_401_1)
                    {
                        try
                        {

                            _Worker1.WaitForComplete();
                            #region CheckPageLoaded

                            #region variable
                            int checkcounter = 0;
                            #endregion variable
                            Erorr_401_1 = true;
                            if (_Worker1.Html == null || !_Worker1.Html.ToLower().Contains("class=\"pages\""))
                            {
                                do
                                {
                                    System.Threading.Thread.Sleep(20);
                                    Application.DoEvents();
                                    checkcounter++;
                                } while ((_Worker1.Html == null || !_Worker1.Html.ToLower().Contains("class=\"pages\"")) && checkcounter < 10000);
                            }

                            checkcounter = 0;

                            #endregion CheckPageLoaded

                            _Work1doc.LoadHtml(_Worker1.Html);

                            if (_IsCategorypaging)
                            {
                                HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"pages\"]/ul/li");
                                int TotalRecords = Convert.ToInt32(_Collection[_Collection.Count - 1].SelectNodes("span")[0].InnerText.Trim());
                                int TotalPages = 0;
                                int CurrentPage = 1;
                                if (TotalRecords % 40 == 0)
                                {
                                    TotalPages = Convert.ToInt32(TotalRecords / 40);
                                }
                                else
                                {
                                    TotalPages = Convert.ToInt32(TotalRecords / 40) + 1;
                                }
                                HtmlNodeCollection _Collection1 = _Work1doc.DocumentNode.SelectNodes("//a[@class=\"product-img\"]");
                                foreach (HtmlNode _Node in _Collection1)
                                {
                                    foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                    {
                                        if (_Attribute.Name.ToLower() == "href")
                                        {
                                            if (!_ProductUrlthread1.Keys.Contains(_Attribute.Value))
                                            {
                                                try
                                                {
                                                    _ProductUrlthread1.Add(_Attribute.Value, BrandName1);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }

                                    }
                                }
                                //for (int i = 0; i < TotalPages; i++)
                                //{
                                //    SubCategoryUrl.Add(Url1.Substring(0,Url1.IndexOf("#")) + "#st=&begin=" + ((i * 40) + 1) + "&nhit=40");
                                //}
                                string ClickTest = "Next";

                                bool Isexist = false;
                                if (TotalPages > 1)
                                {
                                    while (!Isexist)
                                    {
                                        Isexist = true;
                                        try
                                        {
                                            DivCollection Div = _Worker1.Divs.Filter(Find.ByClass("pages"));
                                            LinkCollection _Links = Div[0].Links;
                                            foreach (Link _Link in _Links)
                                            {

                                                if (_Link.InnerHtml.Trim() == ClickTest)
                                                {
                                                    Isexist = false;
                                                    _Link.Click();

                                                    _Worker1.WaitForComplete();
                                                    if (ClickTest == "Next")
                                                    {
                                                        checkcounter = 0;
                                                        if (_Worker1.Html == null || _Worker1.Html.ToLower().Contains(_ProductUrlthread1.ToArray()[_ProductUrlthread1.Count - 1].Key.ToString().ToLower()) || !_Worker1.Html.ToLower().Contains("class=\"pages\""))
                                                        {
                                                            do
                                                            {
                                                                System.Threading.Thread.Sleep(20);
                                                                Application.DoEvents();
                                                                checkcounter++;
                                                            } while ((_Worker1.Html == null || _Worker1.Html.ToLower().Contains(_ProductUrlthread1.ToArray()[_ProductUrlthread1.Count - 1].Key.ToString().ToLower()) || !_Worker1.Html.ToLower().Contains("class=\"pages\"")) && checkcounter < 10000);
                                                        }


                                                        _Work1doc.LoadHtml(_Worker1.Html);

                                                        HtmlNodeCollection _Collection2 = _Work1doc.DocumentNode.SelectNodes("//a[@class=\"product-img\"]");
                                                        foreach (HtmlNode _Node in _Collection2)
                                                        {
                                                            foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                                            {
                                                                if (_Attribute.Name.ToLower() == "href")
                                                                {
                                                                    if (!_ProductUrlthread1.Keys.Contains(_Attribute.Value))
                                                                    {
                                                                        try
                                                                        {
                                                                            _ProductUrlthread1.Add(_Attribute.Value, BrandName1);
                                                                        }
                                                                        catch
                                                                        {
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        string test = _Attribute.Value;
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ClickTest = "Next";
                                                    }
                                                    break;

                                                }

                                            }

                                        }
                                        catch (Exception exp)
                                        {
                                            Isexist = false;
                                            if (ClickTest == "Next")
                                            {
                                                if (!WebUtility.UrlDecode(_Worker1.Url).ToLower().Contains("begin=1&"))
                                                {
                                                    ClickTest = "Previous";
                                                }
                                            }
                                            else
                                            {
                                                ClickTest = "Next";
                                            }
                                            _writer.WriteLine("worker1exp3" + exp.Message);
                                        }
                                    }
                                }



                            }

                        }
                        catch (Exception exp)
                        {
                            _writer.WriteLine("workerexp4" + exp.Message);
                        }
                    }

                    _401index++;
                    _Work.ReportProgress((_401index * 100 / CategoryUrl.Count()));


                }
                #endregion 401categorypaging

                else if (_IsProduct)
                {
                    _401index++;
                    _Work.ReportProgress((_401index * 100 / Url.Count()));

                }

            }
            #endregion 401games
        }
        public void work_dowork1(object sender, DoWorkEventArgs e)
        {

            bool _Iserror = false;
            Erorr_401_2 = true;
            if (_IS401games)
            {
                if (_IsCategorypaging)
                {

                    try
                    {

                        int CounterError = 0;
                        do
                        {
                            try
                            {
                                _Worker2.GoTo(Url2);
                                Erorr_401_2 = false;
                            }
                            catch
                            {
                                CounterError++;
                            }
                        } while (Erorr_401_2 && CounterError < 20);
                    }
                    catch
                    {
                        _Iserror = true;
                    }
                }
                else if (_IsProduct)
                {
                    try
                    {
                        _Work1doc2.LoadHtml(_Client2.DownloadString(Url2));
                        Erorr_401_2 = false;
                    }
                    catch
                    {
                        Erorr_401_2 = true;
                    }
                }

            }
            else
            {
                if (!_IsProduct)
                {
                    try
                    {
                        _Work1doc2.LoadHtml(_Client2.DownloadString(Url2));
                    }
                    catch
                    {
                        _Iserror = true;
                    }
                }
            }
            int index = 0;
            #region 401games
            if (_IS401games)
            {
                #region 401categorypaging
                if (_IsCategorypaging)
                {
                    if (!Erorr_401_2)
                    {
                        try
                        {

                            _Worker2.WaitForComplete();
                            #region CheckPageLoaded

                            #region variable
                            int checkcounter = 0;
                            #endregion variable
                            Erorr_401_2 = true;
                            if (_Worker2.Html == null || !_Worker2.Html.ToLower().Contains("class=\"pages\""))
                            {
                                do
                                {
                                    System.Threading.Thread.Sleep(20);
                                    Application.DoEvents();
                                    checkcounter++;
                                } while ((_Worker2.Html == null || !_Worker2.Html.ToLower().Contains("class=\"pages\"")) && checkcounter < 10000);
                            }

                            checkcounter = 0;

                            #endregion CheckPageLoaded

                            _Work1doc2.LoadHtml(_Worker2.Html);

                            if (_IsCategorypaging)
                            {
                                HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"pages\"]/ul/li");
                                int TotalRecords = Convert.ToInt32(_Collection[_Collection.Count - 1].SelectNodes("span")[0].InnerText.Trim());
                                int TotalPages = 0;
                                int CurrentPage = 1;
                                if (TotalRecords % 40 == 0)
                                {
                                    TotalPages = Convert.ToInt32(TotalRecords / 40);
                                }
                                else
                                {
                                    TotalPages = Convert.ToInt32(TotalRecords / 40) + 1;
                                }
                                HtmlNodeCollection _Collection1 = _Work1doc2.DocumentNode.SelectNodes("//a[@class=\"product-img\"]");
                                foreach (HtmlNode _Node in _Collection1)
                                {
                                    foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                    {
                                        if (_Attribute.Name.ToLower() == "href")
                                        {
                                            if (!_ProductUrlthread2.Keys.Contains(_Attribute.Value))
                                            {
                                                try
                                                {
                                                    _ProductUrlthread2.Add(_Attribute.Value, BrandName2);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }

                                    }
                                }

                                string ClickTest = "Next";
                                bool Isexist = false;
                                if (TotalPages > 1)
                                {
                                    while (!Isexist)
                                    {
                                        Isexist = true;
                                        try
                                        {
                                            DivCollection Div = _Worker2.Divs.Filter(Find.ByClass("pages"));
                                            LinkCollection _Links = Div[0].Links;
                                            foreach (Link _Link in _Links)
                                            {

                                                if (_Link.InnerHtml.Trim() == ClickTest)
                                                {
                                                    Isexist = false;
                                                    _Link.Click();
                                                    _Worker2.WaitForComplete();
                                                    if (ClickTest == "Next")
                                                    {
                                                        checkcounter = 0;
                                                        if (_Worker2.Html == null || _Worker2.Html.ToLower().Contains(_ProductUrlthread2.ToArray()[_ProductUrlthread2.Count - 1].Key.ToString().ToLower()) || !_Worker2.Html.ToLower().Contains("class=\"pages\""))
                                                        {
                                                            do
                                                            {
                                                                System.Threading.Thread.Sleep(20);
                                                                Application.DoEvents();
                                                                checkcounter++;
                                                            } while ((_Worker2.Html == null || _Worker2.Html.ToLower().Contains(_ProductUrlthread2.ToArray()[_ProductUrlthread2.Count - 1].Key.ToString().ToLower()) || !_Worker2.Html.ToLower().Contains("class=\"pages\"")) && checkcounter < 10000);
                                                        }


                                                        _Work1doc2.LoadHtml(_Worker2.Html);

                                                        HtmlNodeCollection _Collection2 = _Work1doc2.DocumentNode.SelectNodes("//a[@class=\"product-img\"]");
                                                        foreach (HtmlNode _Node in _Collection2)
                                                        {
                                                            foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                                            {
                                                                if (_Attribute.Name.ToLower() == "href")
                                                                {
                                                                    if (!_ProductUrlthread2.Keys.Contains(_Attribute.Value))
                                                                    {
                                                                        try
                                                                        {
                                                                            _ProductUrlthread2.Add(_Attribute.Value, BrandName2);
                                                                        }
                                                                        catch
                                                                        {
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        string test = _Attribute.Value;
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ClickTest = "Next";
                                                    }
                                                    break;

                                                }

                                            }

                                        }
                                        catch (Exception exp)
                                        {
                                            Isexist = false;
                                            Isexist = false;
                                            if (ClickTest == "Next")
                                            {
                                                if (!WebUtility.UrlDecode(_Worker2.Url).ToLower().Contains("begin=1&"))
                                                {
                                                    ClickTest = "Previous";
                                                }
                                            }
                                            else
                                            {
                                                ClickTest = "Next";
                                            }
                                            _writer.WriteLine("_Worker2xp3" + exp.Message);
                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception exp)
                        {
                            _writer.WriteLine("worker1exp4" + exp.Message);
                        }
                    }


                    _401index++;
                    _Work1.ReportProgress((_401index * 100 / CategoryUrl.Count()));


                }
                #endregion 401categorypaging
                else if (_IsProduct)
                {
                    _401index++;
                    _Work1.ReportProgress((_401index * 100 / Url.Count()));

                }

            }
            #endregion 401games
        }
        public void work_RunWorkerAsync(object sender, RunWorkerCompletedEventArgs e)
        {

            #region 401games
            if (_IS401games)
            {
                if (_IsProduct && !Erorr_401_1)
                {
                    int index = 0;

                    index = gridindex;
                    gridindex++;
                    try
                    {
                        BusinessLayer.Product Product = new BusinessLayer.Product();
                        Product.URL = Url1;
                        Product.Isparent = true;
                        #region title
                        HtmlNodeCollection _Title = _Work1doc.DocumentNode.SelectNodes("//h1[@class=\"title product\"]");
                        if (_Title != null)
                            Product.Name = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim())).Replace(">", "").Replace("<", "");

                        else
                        {
                            HtmlNodeCollection _Title1 = _Work1doc.DocumentNode.SelectNodes("//h1");
                            if (_Title1 != null)
                                Product.Name = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim())).Replace(">", "").Replace("<", "");
                        }
                        #endregion title

                        #region description
                        _Description1 = "";
                        HtmlNodeCollection _description = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab_desc\"]");
                        if (_description != null)
                        {
                            _Description1 = CommanFunction.Removeunsuaalcharcterfromstring(CommanFunction.StripHTML(_description[0].InnerHtml.Replace("Product Description", "")).Trim());
                        }
                        else
                        {
                            HtmlNodeCollection _description1 = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"ldesc\"]");
                            if (_description != null)
                            {
                                _Description1 = CommanFunction.Removeunsuaalcharcterfromstring(CommanFunction.StripHTML(_description1[0].InnerHtml).Trim());
                            }
                        }
                        try
                        {
                            if (_Description1.Length > 2000)
                                _Description1 = _Description1.Substring(0, 1997) + "...";
                        }
                        catch
                        {
                        }

                        Product.Description = System.Net.WebUtility.HtmlDecode(_Description1.Replace("Â", "").Replace(">", "").Replace("<", ""));

                        #endregion description

                        #region manufacturer
                        Product.Manufacturer = BrandName1;
                        Product.Brand = BrandName1;
                        #endregion manufacturer

                        #region For decsription empty
                        try
                        {
                            if (String.IsNullOrEmpty(Product.Description))
                            {
                                Product.Description = Product.Name.ToString().Replace(">", "").Replace("<", "");
                                Product.Bulletpoints1 = Product.Name.ToString().Replace(">", "").Replace("<", "");

                            }
                            else
                            {
                                if (Product.Description.Length > 500)
                                    Product.Bulletpoints1 = Product.Description.Substring(0, 497);
                                else
                                    Product.Bulletpoints1 = Product.Description;
                            }
                        }
                        catch
                        {
                        }

                        #endregion For decsription empty

                        #region currency
                        Product.Currency = "CDN";
                        #endregion currency


                        #region price,stock
                        if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span") != null)
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span")[0].InnerText.Trim() == "0")
                            {
                                Product.Stock = "0";
                            }
                            else
                            {
                                Product.Stock = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span")[0].InnerText.ToLower().Replace("in-stock :", "").Trim();
                            }
                        }
                        else if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span") != null)
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span")[0].InnerText.Trim() == "0")
                            {
                                Product.Stock = "0";
                            }
                            else
                            {
                                Product.Stock = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span")[0].InnerText.ToLower().Replace("in-stock :", "").Trim();
                            }
                        }

                        else if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]") != null)
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]")[0].InnerText.ToLower().Replace("Quantity :", "").Trim() == "0")
                            {
                                Product.Stock = "0";
                            }
                            else
                            {
                                Product.Stock = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]")[0].InnerText.ToLower().Replace("Quantity :", "").Replace("in-stock :", "").Trim();
                            }
                        }
                        try
                        {
                            if ((Convert.ToInt32(Product.Stock) > 30) || String.IsNullOrEmpty(Product.Stock))
                            {
                                Product.Stock = "30";
                            }
                        }
                        catch
                        {
                        }


                        HtmlNode _Node = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"prices\"]")[0];
                        if (_Node != null)
                        {

                            if (_Node.SelectNodes(".//div[@class=\"discounted-price\"]") != null)
                            {
                                Product.Price = _Node.SelectNodes(".//div[@class=\"discounted-price\"]")[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                            }
                            else if (_Node.SelectNodes(".//div[@class=\"regular-price\"]") != null)
                            {

                                Product.Price = _Node.SelectNodes(".//div[@class=\"regular-price\"]")[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                            }
                        }
                       

                        #endregion price,stock

                        #region sku

                        try
                        {

                            if (String.IsNullOrEmpty(Product.Price))
                            {
                                Product.Price = "0";
                            }
                            if (Convert.ToDecimal(Product.Price) > 0)
                            {
                                Product.SKU = GenrateSkuFromDatbase(CommanFunction.GenerateSku("ST4GAM", CommanFunction.Removeunsuaalcharcterfromstring(Product.Name.Trim())), CommanFunction.Removeunsuaalcharcterfromstring(Product.Name.Trim()), "store.401games", Convert.ToDecimal(Product.Price), Url1);
                                Product.parentsku = Product.SKU;
                            }
                        }
                        catch
                        {
                            Product.SKU = "";
                        }
                        #endregion sku

                        #region Image
                        if (_Work1doc.DocumentNode.SelectNodes("//img[@id=\"main_image\"]") != null)
                        {
                            foreach (HtmlAttribute _Attribute in _Work1doc.DocumentNode.SelectNodes("//img[@id=\"main_image\"]")[0].Attributes)
                            {

                                if (_Attribute.Name == "src")
                                {
                                    Product.Image = "http://store.401games.ca/" + _Attribute.Value;
                                }
                            }


                        }
                        else if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"product-img-wrapper\"]/img") != null)
                        {
                            foreach (HtmlAttribute _Attribute in _Work1doc.DocumentNode.SelectNodes("//div[@class=\"product-img-wrapper\"]/img")[0].Attributes)
                            {

                                if (_Attribute.Name == "src")
                                {
                                    Product.Image = "http://store.401games.ca/" + _Attribute.Value;
                                }
                            }


                        }
                        #endregion  Image

                        #region category
                        if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"breadcrumbs\"]//ul//li[@class=\"category_path\"]//a") != null)
                        {
                            Product.Category = "ST4GAM" + _Work1doc.DocumentNode.SelectNodes("//div[@class=\"breadcrumbs\"]//ul//li[@class=\"category_path\"]//a")[0].InnerText.Trim();
                        }
                        #endregion category

                        #region setQuantityTo0IfLessthen3
                        if (string.IsNullOrEmpty(Product.Stock))
                            Product.Stock = "0";
                        Product.Stock =Convert.ToInt32( Product.Stock) < 3 ? "0 ": (Convert.ToInt32( Product.Stock)-2).ToString();
                        #endregion setQuantityTo0IfLessthen3

                        Products.Add(Product);
                    }
                    catch (Exception exp)
                    {
                        _writer.WriteLine("worker issue for product url" + Url1 + " " + exp.Message);
                    }
                }

            }
            #endregion 401games
        }
        public void work_RunWorkerAsync1(object sender, RunWorkerCompletedEventArgs e)
        {
            #region 401games
            if (_IS401games)
            {
                if (_IsProduct && !Erorr_401_2)
                {
                    int index = 0;

                    index = gridindex;
                    gridindex++;
                    try
                    {
                        BusinessLayer.Product Product = new BusinessLayer.Product();
                        Product.URL = Url2;
                        Product.Isparent = true;
                        #region title
                        HtmlNodeCollection _Title = _Work1doc2.DocumentNode.SelectNodes("//h1[@class=\"title product\"]");
                        if (_Title != null)
                            Product.Name = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim())).Replace(">", "").Replace("<", "");
                        else
                        {
                            HtmlNodeCollection _Title1 = _Work1doc2.DocumentNode.SelectNodes("//h1");
                            if (_Title1 != null)
                                Product.Name = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim())).Replace(">", "").Replace("<", "");

                        }
                        #endregion title

                        #region description
                        _Description2 = "";
                        HtmlNodeCollection _description = _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab_desc\"]");
                        if (_description != null)
                        {
                            _Description2 = CommanFunction.Removeunsuaalcharcterfromstring(CommanFunction.StripHTML(_description[0].InnerHtml.Replace("Product Description", "")).Trim());
                        }
                        else
                        {
                            HtmlNodeCollection _description1 = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"ldesc\"]");
                            if (_description1 != null)
                            {
                                _Description2 = CommanFunction.Removeunsuaalcharcterfromstring(CommanFunction.StripHTML(_description1[0].InnerHtml).Trim());
                            }
                        }
                        try
                        {
                            if (_Description2.Length > 2000)
                            {
                                _Description2 = _Description2.Substring(0, 1997) + "...";

                            }
                        }
                        catch
                        {
                        }

                        Product.Description = System.Net.WebUtility.HtmlDecode(_Description2.Replace("Â", "").Replace(">", "").Replace("<", ""));

                        #endregion description

                        #region manufacturer
                        Product.Manufacturer = BrandName2;
                        Product.Brand = BrandName2;
                        #endregion manufacturer

                        #region For decsription empty
                        try
                        {
                            if (String.IsNullOrEmpty(Product.Description))
                            {
                                Product.Description = Product.Name.ToString().Replace(">", "").Replace("<", "");
                                Product.Bulletpoints1 = Product.Name.ToString().Replace(">", "").Replace("<", "");

                            }
                            else
                            {
                                if (Product.Description.Length > 500)
                                    Product.Bulletpoints1 = Product.Description.Substring(0, 497);
                                else
                                    Product.Bulletpoints1 = Product.Description;
                            }
                        }
                        catch
                        {
                        }

                        #endregion For decsription empty

                        #region currency
                        Product.Currency = "CDN";
                        #endregion currency


                        #region price,stock
                        if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span") != null)
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span")[0].InnerText.Trim() == "0")
                            {
                                Product.Stock = "0";
                            }
                            else
                            {
                                Product.Stock = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span")[0].InnerText.ToLower().Replace("in-stock :", "").Trim();
                            }
                        }
                        else if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span") != null)
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span")[0].InnerText.Trim() == "0")
                            {
                                Product.Stock = "0";
                            }
                            else
                            {
                                Product.Stock = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span")[0].InnerText.ToLower().Replace("in-stock :", "").Trim();
                            }
                        }

                        else if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]") != null)
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]")[0].InnerText.ToLower().Replace("Quantity :", "").Trim() == "0")
                            {
                                Product.Stock = "0";
                            }
                            else
                            {
                                Product.Stock = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]")[0].InnerText.ToLower().Replace("Quantity :", "").Replace("in-stock :", "").Trim();
                            }
                        }


                        try
                        {

                            if ((Convert.ToInt32(Product.Stock) > 30) || String.IsNullOrEmpty(Product.Stock))
                            {
                                Product.Stock = "30";
                            }
                        }
                        catch
                        {
                        }

                        HtmlNode _Node = _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"prices\"]")[0];
                        if (_Node != null)
                        {

                            if (_Node.SelectNodes(".//div[@class=\"discounted-price\"]") != null)
                            {
                                Product.Price = _Node.SelectNodes(".//div[@class=\"discounted-price\"]")[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                            }
                            else if (_Node.SelectNodes(".//div[@class=\"regular-price\"]") != null)
                            {

                                Product.Price = _Node.SelectNodes(".//div[@class=\"regular-price\"]")[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                            }
                        }

                        #endregion price,stock


                        #region sku

                        try
                        {
                            if (String.IsNullOrEmpty(Product.Price))
                            {
                                Product.Price = "0";
                            }
                            if (Convert.ToDecimal(Product.Price) > 0)
                            {
                                Product.SKU = GenrateSkuFromDatbase(CommanFunction.GenerateSku("ST4GAM", CommanFunction.Removeunsuaalcharcterfromstring(Product.Name.Trim())), CommanFunction.Removeunsuaalcharcterfromstring(Product.Name.ToString().Trim()), "store.401games", Convert.ToDecimal(Product.Price), Url2);
                                Product.parentsku = Product.SKU;
                            }
                        }
                        catch
                        {
                            Product.SKU = "";
                        }
                        #endregion sku

                        #region Image
                        if (_Work1doc2.DocumentNode.SelectNodes("//img[@id=\"main_image\"]") != null)
                        {
                            foreach (HtmlAttribute _Attribute in _Work1doc2.DocumentNode.SelectNodes("//img[@id=\"main_image\"]")[0].Attributes)
                            {

                                if (_Attribute.Name == "src")
                                {
                                    Product.Image = "http://store.401games.ca/" + _Attribute.Value;
                                }
                            }


                        }
                        else if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"product-img-wrapper\"]/img") != null)
                        {
                            foreach (HtmlAttribute _Attribute in _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"product-img-wrapper\"]/img")[0].Attributes)
                            {

                                if (_Attribute.Name == "src")
                                {
                                    Product.Image = "http://store.401games.ca/" + _Attribute.Value;
                                }
                            }


                        }
                        #endregion  Image

                        #region category
                        if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"breadcrumbs\"]//ul//li[@class=\"category_path\"]//a") != null)
                        {
                            Product.Category = "ST4GAM" + _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"breadcrumbs\"]//ul//li[@class=\"category_path\"]//a")[0].InnerText.Trim();
                        }
                        #endregion category

                        #region setQuantityTo0IfLessthen3
                        if (string.IsNullOrEmpty(Product.Stock))
                            Product.Stock = "0";
                        Product.Stock = Convert.ToInt32(Product.Stock) < 3 ? "0 " : (Convert.ToInt32(Product.Stock) - 2).ToString();
                        #endregion setQuantityTo0IfLessthen3
                        Products.Add(Product);
                    }
                    catch (Exception exp)
                    {
                        _writer.WriteLine("worker issue for product url" + Url2 + " " + exp.Message);
                    }
                }
            }
            #endregion 401games
        }
        public string GenrateSkuFromDatbase(string sku, string Name, string storename, decimal Price,string url)
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
            _percent.Text = e.ProgressPercentage + "%  Completed";
        }
        public void Work1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _Bar1.Value = e.ProgressPercentage;
            _percent.Visible = true;
            _percent.Text = e.ProgressPercentage + "% Completed";
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
