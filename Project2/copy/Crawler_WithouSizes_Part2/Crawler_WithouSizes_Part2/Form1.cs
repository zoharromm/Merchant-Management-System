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
        List<string> _ProductUrl = new List<string>();
        List<string> _ProductUrlthread1 = new List<string>();
        List<string> _ProductUrlthread2 = new List<string>();
        List<string> _Name = new List<string>();
        List<string> CategoryUrl = new List<string>();
        List<string> SubCategoryUrl = new List<string>();

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
        DataTable _Tbale = new DataTable();
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
            SubCategoryUrl.Clear();
            _ProductUrlthread1.Clear();
            _ProductUrlthread1.Clear();
            _ProductUrl.Clear();
            _percent.Visible = false;
            _Bar1.Value = 0;
            _Url.Clear();
            _Tbale.Rows.Clear();
            _Tbale.Columns.Clear();
            dataGridView1.Rows.Clear();

            DataColumn _Dc = new DataColumn();
            _Dc.ColumnName = "Rowid";
            _Dc.AutoIncrement = true;
            _Dc.DataType = typeof(int);
            _Dc.AutoIncrementSeed = 1;
            _Dc.AutoIncrementStep = 1;
            _Tbale.Columns.Add(_Dc);
            _Tbale.Columns.Add("SKU", typeof(string));
            _Tbale.Columns.Add("Product Name", typeof(string));
            _Tbale.Columns.Add("Product Description", typeof(string));
            _Tbale.Columns.Add("Bullet Points", typeof(string));
            _Tbale.Columns.Add("Manufacturer", typeof(string));
            _Tbale.Columns.Add("Brand Name", typeof(string));
            _Tbale.Columns.Add("Price", typeof(string));
            _Tbale.Columns.Add("Currency", typeof(string));
            _Tbale.Columns.Add("In Stock", typeof(string));
            _Tbale.Columns.Add("Image URL", typeof(string));
            _Tbale.Columns.Add("URL", typeof(string));

            _lblerror.Visible = false;
            _Pages = 0;
            _TotalRecords = 0;
            gridindex = 0;
            _IsCategory = true;
            _Stop = false;
            time = 0;


            #region 402Games
            _IS401games = true;
            _ScrapeUrl = "http://store.401games.ca/";
            try
            {
                _Worker1 = new IE();
               // _Worker2 = new IE();
                _lblerror.Visible = true;
                _lblerror.Text = "We are going to read  category url of " + chkstorelist.Items[0].ToString() + " Website";
                _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));
                HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"col left\"]");
                CategoryUrl = CommanFunction.GetCategoryUrl(_Collection, "ul", "//li/a", "http://store.401games.ca", "#st=&begin=1&nhit=40");
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
                
                if (CategoryUrl.Count() > 0)
                {

                    DisplayRecordProcessdetails("We are going to read product url from category pages for " + chkstorelist.Items[0].ToString() + " Website", "Total  Category :" + CategoryUrl.Count());
                    _IsCategorypaging = true;
                    foreach (string Caturl in CategoryUrl)
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
                            Url1 = Caturl;
                            _Work.RunWorkerAsync();
                        }

                        else
                        {
                            Url2 = Caturl;
                            _Work1.RunWorkerAsync();

                        }
                        break;
                    }

                    while (_Work.IsBusy || _Work1.IsBusy)
                    {
                        Application.DoEvents();

                    }
                    _Bar1.Value = 0;
                    _401index = 0;
                    _IsCategorypaging = false;
                    _ProductUrl = _ProductUrlthread1.Concat(_ProductUrlthread2).ToList();
                    DisplayRecordProcessdetails("We are going to read Product information for   " + chkstorelist.Items[0].ToString() + " Website", "Total  products :" + _ProductUrl.Count());
                   
                    _IsProduct = true;
                    foreach (string PrdUrl in _ProductUrl)
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
                            Url1 = "http://store.401games.ca"+PrdUrl;
                            _Work.RunWorkerAsync();
                        }
                        else
                        {
                            Url2 ="http://store.401games.ca"+ PrdUrl;
                            _Work1.RunWorkerAsync();
                        }

                    }
                }
                while (_Work.IsBusy || _Work1.IsBusy)
                {
                    Application.DoEvents();

                }
                MessageBox.Show("Process Completed.");

            }
            catch
            {
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
        }
        public void work_dowork(object sender, DoWorkEventArgs e)
        {
            bool _Iserror = false;
            if (_IS401games)
            {
                if (_IsCategorypaging)
                {

                    try
                    {
                        Erorr_401_1 = true;
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
                    }
                    catch
                    {
                        _Iserror = true;
                    }
                }
            }
            else
            {
                if (_IsProduct)
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
                                } while ((_Worker1.Html == null || !_Worker1.Html.ToLower().Contains("class=\"pages\"")) && checkcounter < 1000);
                            }

                            checkcounter = 0;

                            #endregion CheckPageLoaded

                            _Work1doc.LoadHtml(_Worker1.Html);

                            if (_IsCategorypaging)
                            {
                                HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"pages\"]/ul/li");
                                int TotalRecords = Convert.ToInt32(_Collection[_Collection.Count - 1].SelectNodes("span")[0].InnerText.Trim());
                                int TotalPages = 0;
                                int CurrentPage = 0;
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
                                            if (!_ProductUrlthread1.Contains(_Attribute.Value))
                                            {
                                                _ProductUrlthread1.Add(_Attribute.Value);
                                            }
                                        }

                                    }
                                }
                                //for (int i = 0; i < TotalPages; i++)
                                //{
                                //    SubCategoryUrl.Add(Url1.Substring(0,Url1.IndexOf("#")) + "#st=&begin=" + ((i * 40) + 1) + "&nhit=40");
                                //}


                                while (CurrentPage < TotalPages)
                                {
                                    DivCollection Div = _Worker1.Divs.Filter(Find.ByClass("pages"));
                                    LinkCollection _Links = Div[0].Links;
                                    foreach (Link _Link in _Links)
                                    {
                                        _Worker1.WaitForComplete();
                                        try
                                        {
                                            int value = 0;
                                            if (int.TryParse(_Link.InnerHtml.Trim(), out value))
                                            {
                                                if (value > CurrentPage)
                                                {
                                                    CurrentPage = value;
                                                    _Link.Click();

                                                    System.Threading.Thread.Sleep(1000);
                                                    try
                                                    {
                                                        if (_Worker1.Html == null || _Worker1.Html.ToLower().Contains(_ProductUrlthread1.ToArray()[_ProductUrlthread1.Count - 1].ToString().ToLower()) || !_Worker1.Html.ToLower().Contains("class=\"pages\""))
                                                        {
                                                            do
                                                            {
                                                                System.Threading.Thread.Sleep(20);
                                                                Application.DoEvents();
                                                                checkcounter++;
                                                            } while ((_Worker1.Html == null || _Worker1.Html.ToLower().Contains(_ProductUrlthread1.ToArray()[_ProductUrlthread1.Count - 1].ToString().ToLower()) || !_Worker1.Html.ToLower().Contains("class=\"pages\"")));
                                                        }

                                                        _Work1doc.LoadHtml(_Worker1.Html);

                                                        HtmlNodeCollection _Collection2 = _Work1doc.DocumentNode.SelectNodes("//a[@class=\"product-img\"]");
                                                        foreach (HtmlNode _Node in _Collection2)
                                                        {
                                                            foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                                            {
                                                                if (_Attribute.Name.ToLower() == "href")
                                                                {
                                                                    if (!_ProductUrlthread1.Contains(_Attribute.Value))
                                                                    {
                                                                        _ProductUrlthread1.Add(_Attribute.Value);
                                                                    }
                                                                    else
                                                                    {
                                                                        string test = _Attribute.Value;
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }

                                }

                            }

                        }
                        catch
                        {
                        }
                    }

                    _401index++;
                    _Work.ReportProgress((_401index * 100 / CategoryUrl.Count()));


                }
                   #endregion 401categorypaging
                else if (_IsProduct)
                {
                    _401index++;
                    _Work.ReportProgress((_401index * 100 / _ProductUrl.Count()));

                }


            }
            #endregion 401games
        }
        public void work_dowork1(object sender, DoWorkEventArgs e)
        {

            bool _Iserror = false;
            if (_IS401games)
            {
                if (_IsCategorypaging)
                {

                    try
                    {
                        Erorr_401_2 = true;
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
                    }
                    catch
                    {
                        _Iserror = true;
                    }
                }
            }
            else
            {
                if (_IsProduct)
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
                                } while ((_Worker2.Html == null || !_Worker2.Html.ToLower().Contains("class=\"pages\"")) && checkcounter < 1000);
                            }

                            checkcounter = 0;

                            #endregion CheckPageLoaded

                            _Work1doc2.LoadHtml(_Worker2.Html);

                            if (_IsCategorypaging)
                            {
                                HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"pages\"]/ul/li");
                                int TotalRecords = Convert.ToInt32(_Collection[_Collection.Count - 1].SelectNodes("span")[0].InnerText.Trim());
                                int TotalPages = 0;
                                int CurrentPage = 0;
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
                                            if (!_ProductUrlthread2.Contains(_Attribute.Value))
                                            {
                                                _ProductUrlthread2.Add(_Attribute.Value);
                                            }
                                        }

                                    }
                                }


                                while (CurrentPage < TotalPages)
                                {
                                    DivCollection Div = _Worker2.Divs.Filter(Find.ByClass("pages"));
                                    LinkCollection _Links = Div[0].Links;
                                    foreach (Link _Link in _Links)
                                    {

                                        _Worker2.WaitForComplete();
                                        try
                                        {
                                            int value = 0;
                                            if (int.TryParse(_Link.InnerHtml.Trim(), out value))
                                            {
                                                if (value > CurrentPage)
                                                {
                                                    CurrentPage = value;
                                                    _Link.Click();

                                                    System.Threading.Thread.Sleep(1000);
                                                    try
                                                    {
                                                        if (_Worker2.Html == null || _Worker2.Html.ToLower().Contains(_ProductUrlthread2.ToArray()[_ProductUrlthread2.Count - 1].ToString().ToLower()) || !_Worker1.Html.ToLower().Contains("class=\"pages\""))
                                                        {
                                                            do
                                                            {
                                                                System.Threading.Thread.Sleep(20);
                                                                Application.DoEvents();
                                                                checkcounter++;
                                                            } while ((_Worker2.Html == null || _Worker2.Html.ToLower().Contains(_ProductUrlthread2.ToArray()[_ProductUrlthread2.Count - 1].ToString().ToLower()) || !_Worker1.Html.ToLower().Contains("class=\"pages\"")));
                                                        }

                                                        _Work1doc2.LoadHtml(_Worker2.Html);

                                                        HtmlNodeCollection _Collection2 = _Work1doc2.DocumentNode.SelectNodes("//a[@class=\"product-img\"]");
                                                        foreach (HtmlNode _Node in _Collection2)
                                                        {
                                                            foreach (HtmlAttribute _Attribute in _Node.Attributes)
                                                            {
                                                                if (_Attribute.Name.ToLower() == "href")
                                                                {
                                                                    if (!_ProductUrlthread2.Contains(_Attribute.Value))
                                                                    {
                                                                        _ProductUrlthread2.Add(_Attribute.Value);
                                                                    }
                                                                    else
                                                                    {
                                                                        string test = _Attribute.Value;
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }

                                }

                            }

                        }
                        catch
                        {
                        }
                    }

                    _401index++;
                    _Work1.ReportProgress((_401index * 100 / CategoryUrl.Count()));


                }
                #endregion 
                #region product
                else if (_IsProduct)
                {
                    _401index++;
                    _Work1.ReportProgress((_401index * 100 / _ProductUrl.Count()));
                }
                #endregion product
            }
            #endregion 401games
        }
        public string GenrateSkuFromDatbase(string sku, string Name, string storename)
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
        public void work_RunWorkerAsync(object sender, RunWorkerCompletedEventArgs e)
        {

            #region 401games
            if (_IS401games)
            {
                if (_IsProduct)
                {
                    int index = 0;

                    index = gridindex;
                    gridindex++;
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = index;
                    dataGridView1.Rows[index].Cells[11].Value = Url1;

                    #region title
                    HtmlNodeCollection _Title = _Work1doc.DocumentNode.SelectNodes("//h1[@class=\"title product\"]");
                    if (_Title != null)
                    {
                        dataGridView1.Rows[index].Cells[2].Value = CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim());
                        dataGridView1.Rows[index].Cells[1].Value = GenrateSkuFromDatbase(CommanFunction.GenerateSku("ST4GAM", CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim())), CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim()), "store.401games");

                    }
                    else
                    {
                        HtmlNodeCollection _Title1 = _Work1doc.DocumentNode.SelectNodes("//h1");
                        if (_Title1 != null)
                        {
                            dataGridView1.Rows[index].Cells[2].Value = CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim());
                            dataGridView1.Rows[index].Cells[1].Value = GenrateSkuFromDatbase(CommanFunction.GenerateSku("ST4GAM", CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim())), CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim()), ".store401games");
                        }
                    }
                    #endregion title

                    #region description
                    _Description1=""
                    HtmlNodeCollection _description = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab_desc\"]");
                    if (_description != null)
                    {
                        _Description1 = CommanFunction.Removeunsuaalcharcterfromstring(CommanFunction.StripHTML(_description[0].InnerHtml.Replace("Product Description","")).Trim());
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
                        {
                            _Description1 = _Description1.Substring(0, 1997) + "...";

                        }
                    }
                    catch
                    {
                    }

                    dataGridView1.Rows[index].Cells[3].Value = _Description1.Replace("Â", "");
                 
                    #endregion description

                    #region manufacturer
                    dataGridView1.Rows[index].Cells[5].Value = "Store 401 games";
                    dataGridView1.Rows[index].Cells[6].Value = "Store 401 games";
                    #endregion manufacturer
                    
                   #region For decsription empty
                    try
                    {
                        if (dataGridView1.Rows[index].Cells[3].Value == null || dataGridView1.Rows[index].Cells[3].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[3].Value.ToString()))
                        {
                            dataGridView1.Rows[index].Cells[3].Value = dataGridView1.Rows[index].Cells[2].Value;
                        }
                    }
                    catch
                    {
                    }

                    #endregion For decsription empty

                    #region currency
                    dataGridView1.Rows[index].Cells[8].Value = "CDN";
                    #endregion currency


                    #region price,stock
                    if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span") != null)
                    {
                        if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span")[0].InnerText.Trim() == "0")
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "0";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span")[0].InnerText.ToLower().Replace("in-stock :","").Trim();
                        }
                    }
                    else if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span") != null)
                    {
                        if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span")[0].InnerText.Trim() == "0")
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "0";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span")[0].InnerText.ToLower().Replace("in-stock :", "").Trim();
                        }
                    }

                    else if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]") != null)
                    {
                        if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]")[0].InnerText.ToLower().Replace("Quantity :", "").Trim() == "0")
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "0";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]")[0].InnerText.ToLower().Replace("Quantity :", "").Replace("in-stock :", "").Trim();
                        }
                    }
                     if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"discounted-price\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[7].Value = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"discounted-price\"]")[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                    }
                    else if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"regular-price\"]") != null)
                    {

                        dataGridView1.Rows[index].Cells[7].Value = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"regular-price\"]")[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                    }
                        

                    #endregion price,stock

                    #region Image
                    if (_Work1doc.DocumentNode.SelectNodes("//img[@id=\"main_image\"]") != null)
                    {
                        foreach(HtmlAttribute _Attribute in _Work1doc.DocumentNode.SelectNodes("//img[@id=\"main_image\"]")[0].Attributes)
                        {

                            if(_Attribute.Name=="src")
                            {
                                dataGridView1.Rows[index].Cells[10].Value = "http://store.401games.ca/" + _Attribute.Value;
                            }
                        }


                    }
                    else if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"product-img-wrapper\"]/img") != null)
                    {
                        foreach (HtmlAttribute _Attribute in _Work1doc.DocumentNode.SelectNodes("//div[@class=\"product-img-wrapper\"]/img")[0].Attributes)
                        {

                            if (_Attribute.Name == "src")
                            {
                                dataGridView1.Rows[index].Cells[10].Value = "http://store.401games.ca/" + _Attribute.Value;
                            }
                        }


                    }
                    #endregion  Image
                }
            }
            #endregion 401games
        }
        public void work_RunWorkerAsync1(object sender, RunWorkerCompletedEventArgs e)
        {
            #region 401games
            if (_IS401games)
            {
                if (_IsProduct)
                {
                    int index = 0;

                    index = gridindex;
                    gridindex++;
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = index;
                    dataGridView1.Rows[index].Cells[11].Value = Url2;

                    #region title
                    HtmlNodeCollection _Title = _Work1doc2.DocumentNode.SelectNodes("//h1[@class=\"title product\"]");
                    if (_Title != null)
                    {
                        dataGridView1.Rows[index].Cells[2].Value = CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim());
                        dataGridView1.Rows[index].Cells[1].Value = GenrateSkuFromDatbase(CommanFunction.GenerateSku("ST4GAM", CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim())), CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim()), "store.401games");

                    }
                    else
                    {
                        HtmlNodeCollection _Title1 = _Work1doc2.DocumentNode.SelectNodes("//h1");
                        if (_Title1 != null)
                        {
                            dataGridView1.Rows[index].Cells[2].Value = CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim());
                            dataGridView1.Rows[index].Cells[1].Value = GenrateSkuFromDatbase(CommanFunction.GenerateSku("ST4GAM", CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim())), CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim()), ".store401games");
                        }
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

                    dataGridView1.Rows[index].Cells[3].Value = _Description2.Replace("Â", "");

                    #endregion description

                    #region manufacturer
                    dataGridView1.Rows[index].Cells[5].Value = "Store 401 games";
                    dataGridView1.Rows[index].Cells[6].Value = "Store 401 games";
                    #endregion manufacturer

                    #region For decsription empty
                    try
                    {
                        if (dataGridView1.Rows[index].Cells[3].Value == null || dataGridView1.Rows[index].Cells[3].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[3].Value.ToString()))
                        {
                            dataGridView1.Rows[index].Cells[3].Value = dataGridView1.Rows[index].Cells[2].Value;
                        }
                    }
                    catch
                    {
                    }

                    #endregion For decsription empty

                    #region currency
                    dataGridView1.Rows[index].Cells[8].Value = "CDN";
                    #endregion currency


                    #region price,stock
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span") != null)
                    {
                        if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span")[0].InnerText.Trim() == "0")
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "0";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability \"]/span")[0].InnerText.ToLower().Replace("in-stock :", "").Trim();
                        }
                    }
                    else if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span") != null)
                    {
                        if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span")[0].InnerText.Trim() == "0")
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "0";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability\"]/span")[0].InnerText.ToLower().Replace("in-stock :", "").Trim();
                        }
                    }

                    else if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]") != null)
                    {
                        if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]")[0].InnerText.ToLower().Replace("Quantity :", "").Trim() == "0")
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "0";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"availability in-stock\"]")[0].InnerText.ToLower().Replace("Quantity :", "").Replace("in-stock :", "").Trim();
                        }
                    }
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"discounted-price\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[7].Value = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"discounted-price\"]")[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                    }
                    else if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"regular-price\"]") != null)
                    {

                        dataGridView1.Rows[index].Cells[7].Value = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"regular-price\"]")[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                    }


                    #endregion price,stock

                    #region Image
                    if (_Work1doc2.DocumentNode.SelectNodes("//img[@id=\"main_image\"]") != null)
                    {
                        foreach (HtmlAttribute _Attribute in _Work1doc2.DocumentNode.SelectNodes("//img[@id=\"main_image\"]")[0].Attributes)
                        {

                            if (_Attribute.Name == "src")
                            {
                                dataGridView1.Rows[index].Cells[10].Value = "http://store.401games.ca/" + _Attribute.Value;
                            }
                        }


                    }
                    else if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"product-img-wrapper\"]/img") != null)
                    {
                        foreach (HtmlAttribute _Attribute in _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"product-img-wrapper\"]/img")[0].Attributes)
                        {

                            if (_Attribute.Name == "src")
                            {
                                dataGridView1.Rows[index].Cells[10].Value = "http://store.401games.ca/" + _Attribute.Value;
                            }
                        }


                    }
                    #endregion  Image
                }
            }
            #endregion 401games
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
        private void _percent_Click(object sender, EventArgs e)
        {

        }
        private void createcsvfile_Click(object sender, EventArgs e)
        {
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
        private void Pause_Click(object sender, EventArgs e)
        {
        }
        private void totalrecord_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            /****************Code to select all check boxes*************/
            /************Uncomment durng live**/
            for (int i = 0; i < chkstorelist.Items.Count; i++)
            {
                chkstorelist.SetItemChecked(i, true);
            }
            /********************End*************************************/
            /***************Grid view************************************/
            totalrecord.Visible = false;
            _lblerror.Visible = false;
            _percent.Visible = false;
            DataGridViewColumn _Columns = new DataGridViewColumn();
            _Columns.Name = "RowID";
            _Columns.HeaderText = "RowID";
            _Columns.DataPropertyName = "RowID";
            _Columns.ValueType = Type.GetType("System.float");
            _Columns.SortMode = DataGridViewColumnSortMode.Automatic;
            DataGridViewCell cell = new DataGridViewLinkCell();

            _Columns.CellTemplate = cell;
            dataGridView1.Columns.Add(_Columns);
            dataGridView1.Columns.Add("SKU", "SKU");
            dataGridView1.Columns.Add("Product Name", "Product Name");
            dataGridView1.Columns.Add("Product Description", "Product Description");
            dataGridView1.Columns.Add("Bullet Points", "Bullet Points");
            dataGridView1.Columns.Add("Manufacturer", "Manufacturer");
            dataGridView1.Columns.Add("Brand Name", "Brand Name");
            dataGridView1.Columns.Add("Price", "Price");
            dataGridView1.Columns.Add("Currency", "Currency");
            dataGridView1.Columns.Add("In Stock", "In Stock");
            dataGridView1.Columns.Add("Image URL", "Image URL");
            dataGridView1.Columns.Add("URL", "URL");


            /****************BackGround worker *************************/

        }

        private void btnsubmit_Click(object sender, System.EventArgs e)
        {
            Process();

        }

        private void Form1_Shown(object sender, System.EventArgs e)
        {
            btnsubmit.PerformClick();
        }
    }
}
