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


        bool _IStigerdirect = true;
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


            #region tigerdirect.ca
            _IStigerdirect = true;
            _ScrapeUrl = "http://tigerdirect.ca/";
            try
            {
                _Worker1 = new IE();
                _Worker2 = new IE();
                _lblerror.Visible = true;
                _lblerror.Text = "We are going to read  category url of " + chkstorelist.Items[0].ToString() + " Website";
                _Worker1.GoTo(_ScrapeUrl);
                _Worker1.WaitForComplete();
                System.Threading.Thread.Sleep(10);
                _Work1doc.LoadHtml(_Worker1.Html);
                HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//ul[@class=\"mastNav-subCats\"]/li/a");

                if (_Collection != null)
                {
                    foreach (HtmlNode node in _Collection)
                    {
                        foreach (HtmlAttribute att in node.Attributes)
                        {
                            if (att.Name == "href")
                                try
                                {
                                    CategoryUrl.Add("http://www.tigerdirect.ca" + (att.Value.Contains("?") ? att.Value + "&recs=30" : att.Value + "?recs=30"), "TGRDRCT" + node.InnerText.Trim());
                                }
                                catch
                                { }
                        }
                    }
                }
                try
                {
                    CategoryUrl.Add("http://www.tigerdirect.ca/applications/Category/guidedSearch.asp?CatId=21&sel=Detail%3B358_1565_8718_8718&cm_re=Printers-_-Spot%2001-_-Laser%20Printers&pagesize=30", "printer");
                    CategoryUrl.Add("http://www.tigerdirect.ca/applications/Category/guidedSearch.asp?CatId=21&sel=Detail%3B358_1565_84868_84868&cm_re=Printers-_-Spot%2002-_-Inkjet%20Printers&pagesize=30", "printer");
                    CategoryUrl.Add("http://www.tigerdirect.ca/applications/Category/guidedSearch.asp?CatId=25&name=scanners&cm_re=Printers-_-Spot%2003-_-Scanners&pagesize=30", "printer");
                    CategoryUrl.Add("http://www.tigerdirect.ca/applications/category/category_slc.asp?CatId=243&cm_re=Printers-_-Spot%2004-_-Label%20Printers&pagesize=30", "printer");
                    CategoryUrl.Add("http://www.tigerdirect.ca/applications/Category/guidedSearch.asp?CatId=21&sel=Detail%3B358_36_84863_84863&cm_re=Printers-_-Spot%2005-_-Mobile&pagesize=30", "printer");

                }
                catch
                { }
                DisplayRecordProcessdetails("We are going to read product url from category pages for " + chkstorelist.Items[0].ToString() + " Website", "Total  Category :" + CategoryUrl.Count());

                if (File.Exists(Application.StartupPath + "/Files/Url.txt"))
                {
                    FileInfo _Info = new FileInfo(Application.StartupPath + "/Files/Url.txt");
                    int Days = 14;
                    try
                    {
                        Days = Convert.ToInt32(Config.GetAppConfigValue("tigerdirect.ca", "FrequencyOfCategoryScrapping"));
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
                        i++;
                        //if (i == 3)
                        //    break;

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
                    _Prd.ProductDatabaseIntegration(Products, "tigerdirect.ca", 1);
                }
                else
                {
                    BusinessLayer.DB _Db = new BusinessLayer.DB();
                    _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='tigerdirect.ca'");
                    _Mail.SendMail("OOPS there is no any product scrapped by app for tigerdirect.ca Website." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

                }
            }
            catch
            {
                BusinessLayer.DB _Db = new BusinessLayer.DB();
                _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='tigerdirect.ca'");
                _lblerror.Visible = true;
                _Mail.SendMail("Oops Some issue Occured in scrapping data tigerdirect.ca  Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

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
            if (_IStigerdirect && _IsCategorypaging)
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
            int index = 0;

            #region tigerdirect.ca
            if (_IStigerdirect)
            {
                if (_IsCategorypaging)
                {
                    #region 401categorypaging

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
                            if (_Worker1.Html == null || !_Worker1.Html.ToLower().Contains("class=\"breadcrumbs\""))
                            {
                                do
                                {
                                    System.Threading.Thread.Sleep(20);
                                    Application.DoEvents();
                                    checkcounter++;
                                } while ((_Worker1.Html == null || !_Worker1.Html.ToLower().Contains("class=\"breadcrumbs\"")) && checkcounter < 25);
                            }
                            _Work1doc.LoadHtml(_Worker1.Html);
                            checkcounter = 0;
                            #region getChildCat
                            if (!_IsSubcat)
                            {
                                HtmlNodeCollection _SubCat = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"innerWrap\"]/ul[@class=\"filterItem\"]");
                                if (_SubCat != null)
                                {
                                    foreach (HtmlNode node in _SubCat[0].SelectNodes(".//a"))
                                    {
                                        foreach (HtmlAttribute attr in node.Attributes)
                                        {
                                            if (attr.Name == "href")
                                            {
                                                try
                                                {
                                                    subCategoryUrl.Add("http://www.tigerdirect.ca" + (attr.Value.Contains("?") ? attr.Value + "&pagesize=30" : attr.Value + "?pagesize=30"), "TGRDRCT" + node.InnerText.Trim());
                                                }
                                                catch
                                                { }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion getChildCat

                            #endregion CheckPageLoaded

                            int TotalRecords = 0;
                            int TotalPages = 0;
                            int CurrentPage = 1;
                            HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"itemsShowresult\"]/strong");
                            if (_Collection != null)
                            {

                                int.TryParse(_Collection[1].InnerText.Trim(), out TotalRecords);
                                if (TotalRecords != 0)
                                {
                                    if (TotalRecords % 40 == 0)
                                    {
                                        TotalPages = Convert.ToInt32(TotalRecords / 10);
                                    }
                                    else
                                    {
                                        TotalPages = Convert.ToInt32(TotalRecords / 10) + 1;
                                    }
                                }
                                else
                                    _writer.WriteLine(Url1 + " " + "workerexp1 " + "Total records Tags Not found");
                            }
                            HtmlNodeCollection _Collection1 = _Work1doc.DocumentNode.SelectNodes("//a[@class=\"itemImage\"]");
                            if (_Collection1 != null)
                            {
                                foreach (HtmlNode _Node in _Collection1)
                                {
                                    foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                    {
                                        if (_Attribute.Name.ToLower() == "href")
                                        {

                                            try
                                            {
                                                _ProductUrlthread1.Add("http://www.tigerdirect.ca" + _Attribute.Value.Replace("../", "/"), BrandName1);
                                            }
                                            catch
                                            {
                                            }
                                        }

                                    }
                                }
                            }
                            else
                                _writer.WriteLine(Url1 + " " + "workerexp1 " + "product not found for given category");
                            string ClickTest = "Next";
                            bool Isexist = false;
                            if (TotalPages > 1)
                            {
                                while (!Isexist)
                                {
                                    Isexist = true;
                                    try
                                    {
                                        IElementContainer Div = (IElementContainer)_Worker1.Element(Find.ByClass("itemsPagination"));
                                        LinkCollection _Links = Div.Links;
                                        foreach (Link _Link in _Links)
                                        {

                                            if (_Link.InnerHtml.Trim().Contains("Next"))
                                            {
                                                Isexist = false;
                                                _Link.Click();
                                                _Worker1.WaitForComplete();
                                                if (ClickTest == "Next")
                                                {
                                                    checkcounter = 0;
                                                    if (_Worker1.Html == null || _Worker1.Html.ToLower().Contains(_ProductUrlthread1.ToArray()[_ProductUrlthread1.Count - 1].Key.ToString().ToLower()) || !_Worker1.Html.ToLower().Contains("class=\"breadcrumbs\""))
                                                    {
                                                        do
                                                        {
                                                            System.Threading.Thread.Sleep(20);
                                                            Application.DoEvents();
                                                            checkcounter++;
                                                        } while ((_Worker1.Html == null || _Worker1.Html.ToLower().Contains(_ProductUrlthread1.ToArray()[_ProductUrlthread1.Count - 1].Key.ToString().ToLower()) || !_Worker1.Html.ToLower().Contains("class=\"breadcrumbs\"")) && checkcounter < 10);
                                                    }


                                                    _Work1doc.LoadHtml(_Worker1.Html);

                                                    HtmlNodeCollection _Collection2 = _Work1doc.DocumentNode.SelectNodes("//a[@class=\"itemImage\"]");
                                                    if (_Collection2 != null)
                                                    {
                                                        foreach (HtmlNode _Node in _Collection2)
                                                        {
                                                            foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                                            {
                                                                if (_Attribute.Name.ToLower() == "href")
                                                                {
                                                                    try
                                                                    {
                                                                        _ProductUrlthread1.Add("http://www.tigerdirect.ca" + _Attribute.Value.Replace("../", "/"), BrandName1);
                                                                    }
                                                                    catch
                                                                    {
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                    else
                                                        _writer.WriteLine(Url1 + " " + "workerexp1 " + "product not found for given category");
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
                                            if (!WebUtility.UrlDecode(_Worker1.Url).ToLower().Contains("page=1&"))
                                            {
                                                ClickTest = "Previous";
                                            }
                                        }
                                        else
                                        {
                                            ClickTest = "Next";
                                        }
                                        _writer.WriteLine(Url1 + " " + "worker1exp3" + exp.Message);
                                    }
                                }
                            }


                        }
                        catch (Exception exp)
                        {
                            _writer.WriteLine(Url1 + " " + "workerexp4 " + exp.Message);
                        }
                    }

                    _401index++;
                    _Work.ReportProgress((_401index * 100 / (_IsSubcat == false ? CategoryUrl.Count() : subCategoryUrl.Count())));

                    #endregion 401categorypaging
                }


                else if (_IsProduct)
                {
                    try
                    {
                        _Client1.Headers["Accept"] = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                        _Client1.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MDDC)";
                        _Work1doc.LoadHtml(_Client1.DownloadString(Url1));

                        //_Worker1.WaitForComplete();
                        //int checkcounter = 0;
                        //Erorr_401_1 = true;
                        //if (_Worker1.Html == null || !_Worker1.Html.ToLower().Contains("class=\"breadcrumb\""))
                        //{
                        //    do
                        //    {
                        //        System.Threading.Thread.Sleep(20);
                        //        Application.DoEvents();
                        //        checkcounter++;
                        //    } while ((_Worker1.Html == null || !_Worker1.Html.ToLower().Contains("class=\"breadcrumb\"")) && checkcounter < 10);
                        //}
                        //_Work1doc.LoadHtml(_Worker1.Html);


                        GetProductInfo(_Work1doc, Url1, BrandName1);
                    }
                    catch (Exception exp)
                    {
                        _writer.WriteLine(Url1 + " " + " product page exception workerexp4 " + exp.Message);
                    }
                    _401index++;
                    _Work.ReportProgress((_401index * 100 / Url.Count()));
                }

            }
            #endregion tigerdirect.ca
        }
        public void work_dowork1(object sender, DoWorkEventArgs e)
        {
            bool _Iserror = false;
            Erorr_401_2 = true;
            if (_IStigerdirect && _IsCategorypaging)
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

            int index = 0;

            #region tigerdirect.ca
            if (_IStigerdirect)
            {
                if (_IsCategorypaging)
                {
                    #region 401categorypaging

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
                            if (_Worker2.Html == null || !_Worker2.Html.ToLower().Contains("class=\"breadcrumbs\""))
                            {
                                do
                                {
                                    System.Threading.Thread.Sleep(20);
                                    Application.DoEvents();
                                    checkcounter++;
                                } while ((_Worker2.Html == null || !_Worker2.Html.ToLower().Contains("class=\"breadcrumbs\"")) && checkcounter < 25);
                            }
                            _Work1doc2.LoadHtml(_Worker2.Html);
                            checkcounter = 0;
                            #region getChildCat
                            if (!_IsSubcat)
                            {
                                HtmlNodeCollection _SubCat = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"innerWrap\"]/ul[@class=\"filterItem\"]");
                                if (_SubCat != null)
                                {
                                    foreach (HtmlNode node in _SubCat[0].SelectNodes(".//a"))
                                    {
                                        foreach (HtmlAttribute attr in node.Attributes)
                                        {
                                            if (attr.Name == "href")
                                            {
                                                try
                                                {
                                                    subCategoryUrl.Add("http://www.tigerdirect.ca" + (attr.Value.Contains("?") ? attr.Value + "&pagesize=30" : attr.Value + "?pagesize=30"), "TGRDRCT" + node.InnerText.Trim());
                                                }
                                                catch
                                                { }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion getChildCat

                            #endregion CheckPageLoaded

                            int TotalRecords = 0;
                            int TotalPages = 0;
                            int CurrentPage = 1;
                            HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"itemsShowresult\"]/strong");
                            if (_Collection != null)
                            {

                                int.TryParse(_Collection[1].InnerText.Trim(), out TotalRecords);
                                if (TotalRecords != 0)
                                {
                                    if (TotalRecords % 40 == 0)
                                    {
                                        TotalPages = Convert.ToInt32(TotalRecords / 10);
                                    }
                                    else
                                    {
                                        TotalPages = Convert.ToInt32(TotalRecords / 10) + 1;
                                    }
                                }
                                else
                                    _writer.WriteLine(Url2 + " " + "workerexp1 " + "Total records Tags Not found");
                            }
                            HtmlNodeCollection _Collection1 = _Work1doc2.DocumentNode.SelectNodes("//a[@class=\"itemImage\"]");
                            if (_Collection1 != null)
                            {
                                foreach (HtmlNode _Node in _Collection1)
                                {
                                    foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                    {
                                        if (_Attribute.Name.ToLower() == "href")
                                        {

                                            try
                                            {
                                                _ProductUrlthread2.Add("http://www.tigerdirect.ca" + _Attribute.Value.Replace("../", "/"), BrandName2);
                                            }
                                            catch
                                            {
                                            }
                                        }

                                    }
                                }
                            }
                            else
                                _writer.WriteLine(Url2 + " " + "workerexp1 " + "product not found for given category");
                            string ClickTest = "Next";
                            bool Isexist = false;
                            if (TotalPages > 1)
                            {
                                while (!Isexist)
                                {
                                    Isexist = true;
                                    try
                                    {
                                        IElementContainer Div = (IElementContainer)_Worker2.Element(Find.ByClass("itemsPagination"));
                                        LinkCollection _Links = Div.Links;
                                        foreach (Link _Link in _Links)
                                        {

                                            if (_Link.InnerHtml.Trim().Contains("Next"))
                                            {
                                                Isexist = false;
                                                _Link.Click();
                                                _Worker2.WaitForComplete();
                                                if (ClickTest == "Next")
                                                {
                                                    checkcounter = 0;
                                                    if (_Worker2.Html == null || _Worker2.Html.ToLower().Contains(_ProductUrlthread2.ToArray()[_ProductUrlthread2.Count - 1].Key.ToString().ToLower()) || !_Worker2.Html.ToLower().Contains("class=\"breadcrumbs\""))
                                                    {
                                                        do
                                                        {
                                                            System.Threading.Thread.Sleep(20);
                                                            Application.DoEvents();
                                                            checkcounter++;
                                                        } while ((_Worker2.Html == null || _Worker2.Html.ToLower().Contains(_ProductUrlthread2.ToArray()[_ProductUrlthread2.Count - 1].Key.ToString().ToLower()) || !_Worker2.Html.ToLower().Contains("class=\"breadcrumbs\"")) && checkcounter < 10);
                                                    }


                                                    _Work1doc2.LoadHtml(_Worker2.Html);

                                                    HtmlNodeCollection _Collection2 = _Work1doc2.DocumentNode.SelectNodes("//a[@class=\"itemImage\"]");
                                                    if (_Collection2 != null)
                                                    {
                                                        foreach (HtmlNode _Node in _Collection2)
                                                        {
                                                            foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                                            {
                                                                if (_Attribute.Name.ToLower() == "href")
                                                                {
                                                                    try
                                                                    {
                                                                        _ProductUrlthread2.Add("http://www.tigerdirect.ca" + _Attribute.Value.Replace("../", "/"), BrandName2);
                                                                    }
                                                                    catch
                                                                    {
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                    else
                                                        _writer.WriteLine(Url2 + " " + "workerexp1 " + "product not found for given category");
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
                                            if (!WebUtility.UrlDecode(_Worker2.Url).ToLower().Contains("page=1&"))
                                            {
                                                ClickTest = "Previous";
                                            }
                                        }
                                        else
                                        {
                                            ClickTest = "Next";
                                        }
                                        _writer.WriteLine(Url2 + " " + "worker1exp3" + exp.Message);
                                    }
                                }
                            }


                        }
                        catch (Exception exp)
                        {
                            _writer.WriteLine(Url2 + " " + "workerexp4 " + exp.Message);
                        }
                    }

                    _401index++;
                    _Work1.ReportProgress((_401index * 100 / (_IsSubcat == false ? CategoryUrl.Count() : subCategoryUrl.Count())));

                    #endregion 401categorypaging
                }


                else if (_IsProduct)
                {
                    try
                    {

                        _Client2.Headers["Accept"] = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                        _Client2.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; MDDC)";
                        _Work1doc2.LoadHtml(_Client2.DownloadString(Url2));

                        GetProductInfo(_Work1doc2, Url2, BrandName2);
                    }
                    catch (Exception exp)
                    {
                        _writer.WriteLine(Url2 + " " + " product page exception workerexp4 " + exp.Message);
                    }
                    _401index++;
                    _Work1.ReportProgress((_401index * 100 / Url.Count()));
                }

            }
            #endregion tigerdirect.ca
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
                HtmlNodeCollection formColl = _doc.DocumentNode.SelectNodes("//div[@class=\"prodName\"]/h1");
                if (formColl != null)
                    product.Name = System.Net.WebUtility.HtmlDecode(formColl[0].InnerText).Trim();

                else if (_doc.DocumentNode.SelectNodes("//meta[@property=\"og:title\"]") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//meta[@property=\"og:title\"]")[0].Attributes)
                    {
                        if (attr.Name == "content")
                        {
                            product.Name = System.Net.WebUtility.HtmlDecode(attr.Value).Trim();
                            break;
                        }
                    }
                }
                else
                    _writer.WriteLine(url + " " + "title not found");
                #endregion title

                #region Price
                string priceString = "";
                double Price = 0;
                if (_doc.DocumentNode.SelectNodes("//span[@class=\"salePrice\"]") != null)
                {
                    priceString = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//span[@class=\"salePrice\"]")[0].InnerText).Replace("$", "").Trim();
                    double.TryParse(priceString, out Price);
                    if (Price != 0)
                        product.Price = Price.ToString(); //System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//span[@class=\"regular-price\"]/span[@class=\"price\"]")[0].InnerText).re;
                    else
                        _writer.WriteLine(url + " " + "Price not found");
                }
                else if (_doc.DocumentNode.SelectNodes("//meta[@itemprop=\"price\"]") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//meta[@itemprop=\"price\"]")[0].Attributes)
                    {
                        if (attr.Name == "content")
                        {
                            priceString = System.Net.WebUtility.HtmlDecode(attr.Value).Replace("$", "").Trim();
                            break;
                        }
                    }

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
                if (_doc.DocumentNode.SelectNodes("//div[@itemprop=\"brand\"]/meta") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//div[@itemprop=\"brand\"]/meta")[0].Attributes)
                    {
                        if (attr.Name == "content")
                        {
                            product.Brand = attr.Value.Trim();
                            product.Manufacturer = attr.Value.Trim();
                            break;
                        }

                    }
                }
                else
                {
                    product.Brand = "JZ HOLDINGS";
                    product.Manufacturer = "JZ HOLDINGS";
                }
                #endregion Brand

                #region Category
                product.Category = string.IsNullOrEmpty(Category) ? "TGRDRCTJZ HOLDINGS" : Category;
                #endregion Category

                product.Currency = "CAD";

                #region description
                string Description = "";

                HtmlNodeCollection desCollection = _doc.DocumentNode.SelectNodes("//div[@id=\"prodinfo\"]");
                if (desCollection != null)
                {
                    try
                    {
                        Description = Removeunsuaalcharcterfromstring(StripHTML(desCollection[0].InnerText).Trim());
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

                #endregion description

                #region BulletPoints
                string Feature = "";
                HtmlNodeCollection collection = _doc.DocumentNode.SelectNodes("//table[@class=\"prodSpec\"]");
                if (collection != null)
                {
                    string Header = "";
                    string Value = "";
                    int PointCounter = 1;
                    try
                    {
                        foreach (HtmlNode node in collection[0].SelectNodes(".//tr"))
                        {
                            try
                            {
                                Header = System.Net.WebUtility.HtmlDecode(node.SelectNodes(".//th")[0].InnerText.Trim());
                                if (node.SelectNodes(".//td") != null)
                                {
                                    Value = System.Net.WebUtility.HtmlDecode(node.SelectNodes(".//td")[0].InnerText.Trim());
                                    if (Value != "")
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
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }


                    if (!string.IsNullOrEmpty(Bullets))
                        Bullets = Bullets.Trim();

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
                if (_doc.DocumentNode.SelectNodes("//meta[@itemprop=\"image\"]") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//meta[@itemprop=\"image\"]")[0].Attributes)
                    {
                        if (attr.Name == "content")
                        {
                            Images = attr.Value.Trim() + ",";
                            break;
                        }

                    }
                }
                HtmlNodeCollection imgCollection = _doc.DocumentNode.SelectNodes("//ul[@id=\"viewsImg\"]");
                if (imgCollection != null)
                {

                    foreach (HtmlNode node in imgCollection[0].SelectNodes(".//img"))
                    {
                        foreach (HtmlAttribute attr in node.Attributes)
                        {
                            if (attr.Name == "src")
                            {
                                Images = Images + attr.Value.Trim().Replace("/small", "/large") + ",";
                                break;
                            }
                        }

                    }
                }
                else
                    _writer.WriteLine(url + " " + "Main Images  not found");

                if (Images.Length > 0)
                    Images = Images.Substring(0, Images.Length - 1);

                product.Image = Images;
                #endregion Image
                product.Isparent = true;
                #region sku
                string sku = "";
                if (_doc.DocumentNode.SelectNodes("//span[@class=\"sku\"]/strong") != null)
                {
                    try
                    {
                        foreach (HtmlNode Node in _doc.DocumentNode.SelectNodes("//span[@class=\"sku\"]/strong"))
                        {
                            if (Node.InnerText.ToLower().Contains("model#"))
                                product.ManPartNO = Removeunsuaalcharcterfromstring(StripHTML(Node.NextSibling.InnerText)).Replace("|", "").Trim();
                            else if (Node.InnerText.ToLower().Contains("item#"))
                            {
                                product.SKU = "TGRDRCT" + Removeunsuaalcharcterfromstring(StripHTML(Node.NextSibling.InnerText)).Replace("|", "").Trim();
                                product.parentsku = "TGRDRCT" + Removeunsuaalcharcterfromstring(StripHTML(Node.NextSibling.InnerText)).Replace("|", "").Trim();
                            }
                        }
                    }
                    catch
                    { }
                }
                else
                    _writer.WriteLine(url + " " + "Model  and SKU  not found");

                //if (_doc.DocumentNode.SelectNodes("//meta[@itemprop=\"sku\"]") != null)
                //{
                //    foreach (HtmlAttribute node in _doc.DocumentNode.SelectNodes("//meta[@itemprop=\"sku\"]")[0].Attributes)
                //    {
                //        if (node.Name == "content")
                //        {
                //            product.SKU = "TGRDRCT" + node.Value.Trim();
                //            product.parentsku = "TGRDRCT" + node.Value.Trim();
                //        }
                //    }
                //}
                //else
                //    _writer.WriteLine(url + " " + "sku   not found");
                #endregion sku

                //#region upc
                //if (_doc.DocumentNode.SelectNodes("//meta[@itemprop=\"gtin14\"]") != null)
                //{
                //    foreach (HtmlAttribute node in _doc.DocumentNode.SelectNodes("//meta[@itemprop=\"gtin14\"]")[0].Attributes)
                //    {
                //        if (node.Name == "content")
                //        {
                //            product.UPC = node.Value.Trim();
                //            if(product.UPC=="00000000000000")
                //                product.UPC="";
                //            break;
                //        }
                //    }
                //}
                //else
                //    _writer.WriteLine(url + " " + "upc   not found");
                //#endregion upc


                #region stock
                product.Stock = "0";
                if (_doc.DocumentNode.SelectNodes("//meta[@itemprop=\"availability\"]") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//meta[@itemprop=\"availability\"]")[0].Attributes)
                    {
                        if (attr.Name == "content")
                        {
                            if (attr.Value.ToLower() == "instock")
                            {
                                product.Stock = "1";
                                break;
                            }
                        }
                    }
                }
                #endregion stock
                product.URL = url;
                if (!string.IsNullOrEmpty(product.UPC))
                    product.ISGtin = true;
                else
                    product.ISGtin = false;
                Products.Add(product);
            }
            catch (Exception exp)
            {
                _writer.WriteLine(url + " " + "Issue accured in reading product info from given product url. exp: " + exp.Message);
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
