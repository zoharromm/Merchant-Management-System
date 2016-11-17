﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
namespace Crawler_WithouSizes_Part3
{
    public partial class Form1 : Form
    {


        #region DatbaseVariable
        SqlConnection Connection = new SqlConnection(System.Configuration.ConfigurationSettings.
                                               AppSettings["connectionstring"]);
        #endregion DatbaseVariable
        StreamWriter _writer = new StreamWriter(Application.StartupPath + "/test.csv");

        #region booltypevariable

        bool _IScanadadrones = true;
        bool _IsProduct = false;
        bool _IsCategory = true;
        bool _IsCategorypaging = false;
        bool _Stop = false;

        #endregion booltypevariable


        #region intypevariable

        int gridindex = 0;
        int _canadadronesindex = 0;

        #endregion intypevariable

        #region stringtypevariable

        string Url1 = "";
        string Url2 = "";
        string BrandName1 = "";
        string BrandName2 = "";
        string _ScrapeUrl = "";
        string Bullets = "";
        string _Description1 = "";
        string _Description2 = "";

        #endregion listtypevariable

        #region listtypevariable

        List<string> _Url = new List<string>();
        List<string> _dateofbirth = new List<string>();
        Dictionary<string, string> _ProductUrl = new Dictionary<string, string>();
        List<string> _Name = new List<string>();
        Dictionary<string, string> CategoryUrl = new Dictionary<string, string>();
        Dictionary<string, string> SubCategoryUrl = new Dictionary<string, string>();

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
            gridindex = 0;
            _IsCategory = true;
            _Stop = false;



            #region mastermindtoys
            _IScanadadrones = true;
            _ScrapeUrl = "http://www.canadadrones.com/";
            try
            {

                _lblerror.Visible = true;
                _lblerror.Text = "We are going to read  category url for " + chkstorelist.Items[0].ToString() + " Website";
                _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));

                HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"display_menu_1\"]");
                if (_Collection != null)
                {
                    HtmlNodeCollection _AnchorCollection = _Collection[0].SelectNodes(".//a");
                    foreach (HtmlNode _Node in _AnchorCollection)
                    {

                        HtmlAttributeCollection _AttributeCollection = _Node.Attributes;
                        foreach (HtmlAttribute _Attribute in _AttributeCollection)
                        {
                            if (_Attribute.Name.ToLower() == "href")
                            {
                                if (!_Attribute.Value.ToLower().Contains("all-products"))
                                    CategoryUrl.Add(_Attribute.Value, _Node.InnerText);
                            }
                        }
                    }
                }
                try
                  {
                    CategoryUrl.Add("http://www.canadadrones.com/category-s/1861.htm", "Parts for DJI Phantom products");
                    CategoryUrl.Add("http://www.canadadrones.com/category-s/1858.htm", "For Everyone");
                    CategoryUrl.Add("http://www.canadadrones.com/category-s/1860.htm", "For Experts");
                    CategoryUrl.Add("http://www.canadadrones.com/category-s/1862.htm", "Expert parts");
                    CategoryUrl.Add("http://www.canadadrones.com/Canada-Drones-All-Products-s/1818.htm", "CANDRON");
                }
                catch
                {
                }
                if (CategoryUrl.Count() > 0)
                {
                    #region Category
                
                    DisplayRecordProcessdetails("We are going to read paging  from category pages for " + chkstorelist.Items[0].ToString() + " Website", "Total  Category :" + CategoryUrl.Count());
                    _IsCategorypaging = true;
                    foreach (var Caturl in CategoryUrl)
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

                    while (_Work.IsBusy || _Work1.IsBusy)
                    {
                        Application.DoEvents();

                    }
                    #endregion Category

                    #region ProductUrl

                    System.Threading.Thread.Sleep(1000);
                    _Bar1.Value = 0;
                    _canadadronesindex = 0;
                    _IsCategorypaging = false;
                    _IsCategory = true;
                    DisplayRecordProcessdetails("We are going to read Product url for   " + chkstorelist.Items[0].ToString() + " Website", "Total  category url :" + SubCategoryUrl.Count());

                    foreach (var CatUrl in SubCategoryUrl)
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
                            BrandName1 = CatUrl.Value;
                            Url1 = CatUrl.Key;
                            _Work.RunWorkerAsync();
                        }
                        else
                        {
                            BrandName2 = CatUrl.Value;
                            Url2 = CatUrl.Key;
                            _Work1.RunWorkerAsync();
                        }

                    }
                }
                while (_Work.IsBusy || _Work1.IsBusy)
                {
                    Application.DoEvents();

                }

                    #endregion ProductUrl
                #region ProductInformation
                _Bar1.Value = 0;
                System.Threading.Thread.Sleep(1000);
                _canadadronesindex = 0;
                _IsCategory = false;
                _IsProduct = true;
             
                DisplayRecordProcessdetails("We are going to read Product Information for   " + chkstorelist.Items[0].ToString() + " Website", "Total  Products :" + _ProductUrl.Count());
                foreach (var PrdUrl in _ProductUrl)
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


                        BrandName1 = PrdUrl.Value;
                        Url1 = PrdUrl.Key;
                        _Work.RunWorkerAsync();
                    }
                    else
                    {
                        BrandName2 = PrdUrl.Value;
                        Url2 = PrdUrl.Key;
                        _Work1.RunWorkerAsync();
                    }

                }


                while (_Work.IsBusy || _Work1.IsBusy)
                {
                    Application.DoEvents();

                }

                #endregion ProductInformation
                System.Threading.Thread.Sleep(1000);
                _lblerror.Visible = true;
                _lblerror.Text = "Now we going to generate Csv File";
                GenerateCSVFile();
                MessageBox.Show("Process Completed.");

            }
            catch
            {
            }

            #endregion
            _writer.Close();
        }
        public void work_dowork(object sender, DoWorkEventArgs e)
        {
            bool _Iserror = false;
            try
            {
                _Work1doc.LoadHtml(_Client1.DownloadString(Url1));
                _Iserror = false;
            }
            catch
            {
                _Iserror = true;
            }
            #region CanadaDrones
            if (_IScanadadrones)
            {

                if (_IsCategorypaging)
                {
                    if (!_Iserror)
                    {
                        HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//select[@class=\"results_per_page_select\"]");
                        if (_Collection != null)
                        {
                            try
                            {

                                int _TotalPages = Convert.ToInt32(Regex.Replace(_Work1doc.DocumentNode.SelectNodes(_Collection[0].XPath.Substring(0, _Collection[0].XPath.Length - 9) + "/nobr[1]")[0].InnerText.Replace("\r", "").Replace("\n", "").ToLower().Replace("page", "").Replace("of", "").Trim(), "[^0-9+]", string.Empty));
                                for (int Page = 1; Page <= _TotalPages; Page++)
                                {
                                    try
                                    {
                                        SubCategoryUrl.Add(Url1 + "?show=50&page=" + Page, BrandName1);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            catch
                            {
                                try
                                {
                                    SubCategoryUrl.Add(Url1, BrandName1);
                                }
                                catch
                                {
                                }
                            }
                        }
                        else
                         {

                        }

                    }
                    else
                    {
                    }
                    _canadadronesindex++;
                    _Work.ReportProgress((_canadadronesindex * 100 / CategoryUrl.Count()));

                }
                else if (_IsCategory)
                {
                    if (!_Iserror)
                    {
                        HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//table[@class=\"v65-productDisplay\"]");

                        if (_Collection != null)
                        {
                            HtmlNodeCollection _AnchorCollection = _Collection[0].SelectNodes(".//a[@class=\"productnamecolor colors_productname\"]");
                            if (_AnchorCollection != null)
                            {
                                foreach (HtmlNode _Node in _AnchorCollection)
                                {
                                    HtmlAttributeCollection _AttColl = _Node.Attributes;
                                    foreach (HtmlAttribute _Att in _AttColl)
                                    {
                                        if (_Att.Name.ToLower() == "href")
                                        {
                                            try
                                            {
                                                if (!_ProductUrl.Keys.Contains(_Att.Value.ToLower()))
                                                    _ProductUrl.Add(_Att.Value.ToLower(), BrandName1);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }

                        _canadadronesindex++;
                        _Work.ReportProgress((_canadadronesindex * 100 / SubCategoryUrl.Count()));
                    }

                }
                else
                {
                    _canadadronesindex++;
                    _Work.ReportProgress((_canadadronesindex * 100 / _ProductUrl.Count()));
                }
            }

            #endregion CanadaDrones

        }
        public void work_dowork1(object sender, DoWorkEventArgs e)
        {
            bool _Iserror = false;
            try
            {
                _Work1doc2.LoadHtml(_Client2.DownloadString(Url2));
                _Iserror = false;
            }
            catch
            {
                _Iserror = true;
            }
            #region CanadaDrones
            if (_IScanadadrones)
            {

                if (_IsCategorypaging)
                {
                    if (!_Iserror)
                    {
                        HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//select[@class=\"results_per_page_select\"]");
                        if (_Collection != null)
                        {
                            try
                            {

                                int _TotalPages = Convert.ToInt32(Regex.Replace(_Work1doc2.DocumentNode.SelectNodes(_Collection[0].XPath.Substring(0, _Collection[0].XPath.Length - 9) + "/nobr[1]")[0].InnerText.Replace("\r", "").Replace("\n", "").ToLower().Replace("page", "").Replace("of", "").Trim(), "[^0-9+]", string.Empty));
                                for (int Page = 1; Page <= _TotalPages; Page++)
                                {
                                    try
                                    {
                                        SubCategoryUrl.Add(Url2 + "?show=50&page=" + Page, BrandName2);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            catch
                            {
                                try
                                {
                                    SubCategoryUrl.Add(Url2, BrandName2);
                                }
                                catch
                                {
                                }
                            }
                        }
                        else
                        {

                        }

                    }
                    else
                    {
                    }
                    _canadadronesindex++;
                    _Work1.ReportProgress((_canadadronesindex * 100 / CategoryUrl.Count()));

                }
                else if (_IsCategory)
                {
                    if (!_Iserror)
                    {
                        HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//table[@class=\"v65-productDisplay\"]");

                        if (_Collection != null)
                        {
                            HtmlNodeCollection _AnchorCollection = _Collection[0].SelectNodes(".//a[@class=\"productnamecolor colors_productname\"]");
                            if (_AnchorCollection != null)
                            {
                                foreach (HtmlNode _Node in _AnchorCollection)
                                {
                                    HtmlAttributeCollection _AttColl = _Node.Attributes;
                                    foreach (HtmlAttribute _Att in _AttColl)
                                    {
                                        if (_Att.Name.ToLower() == "href")
                                        {
                                            try
                                            {
                                                if (!_ProductUrl.Keys.Contains(_Att.Value.ToLower()))
                                                    _ProductUrl.Add(_Att.Value.ToLower(), BrandName2);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }

                        _canadadronesindex++;
                        _Work1.ReportProgress((_canadadronesindex * 100 / SubCategoryUrl.Count()));
                    }

                }
                else
                {
                    _canadadronesindex++;
                    _Work1.ReportProgress((_canadadronesindex * 100 / _ProductUrl.Count()));
                }
            }

            #endregion CanadaDrones

        }
        public void work_RunWorkerAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            #region CanadaDrones
            if (_IScanadadrones)
            {
                if (_IsProduct)
                 {
                    if (_Work1doc.DocumentNode != null)
                    {
                        if (!_Work1doc.DocumentNode.InnerHtml.ToLower().Contains("product_options_heading"))
                        {
                            int index = 0;

                            index = gridindex;
                            gridindex++;
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[index].Cells[0].Value = index;
                            dataGridView1.Rows[index].Cells[11].Value = Url1;

                            #region title
                            HtmlNodeCollection _Title = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"name\"]");
                            if (_Title != null)
                            {
                                dataGridView1.Rows[index].Cells[2].Value = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim())).Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("Online Only", "").Replace("online only", "");

                            }
                            else
                            {
                                HtmlNodeCollection _Title1 = _Work1doc.DocumentNode.SelectNodes("//meta[@property=\"og:title\"]");
                                if (_Title1 != null)
                                {
                                    dataGridView1.Rows[index].Cells[2].Value = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim())).Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("Online Only", "").Replace("online only", "");
                                }
                            }
                            #endregion title

                            #region description

                            _Description1 = "";
                            HtmlNodeCollection _description = _Work1doc.DocumentNode.SelectNodes("//span[@id=\"product_description\"]");
                            if (_description != null)
                            {
                                _Description1 = _description[0].InnerHtml.Replace("Product Description", "").Trim();

                                #region CodeToReMoveText
                                if (_Description1.Trim().Length > 0)
                                {
                                    List<string> _Remove = new List<string>();
                                    foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//span[@id=\"product_description\"]")[0].ChildNodes)
                                    {
                                        if (_Node.InnerText.ToLower().Contains("special orders") || _Node.InnerText.ToLower().Contains("stock is limited") || _Node.InnerText.ToLower().Contains("please note") || _Node.InnerText.ToLower().Contains("credit card"))
                                        {
                                            _Remove.Add(_Node.InnerHtml);
                                        }

                                    }

                                    foreach (string _rem in _Remove)
                                    {
                                        _Description1 = _Description1.Replace(_rem, "");
                                    }
                                }
                                #endregion CodeToReMoveText
                                _Description1 = CommanFunction.Removeunsuaalcharcterfromstring(CommanFunction.StripHTML(_Description1).Trim());

                            }
                            else
                            {


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

                            dataGridView1.Rows[index].Cells[3].Value = System.Net.WebUtility.HtmlDecode(_Description1.Replace("Â", "").Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("online only", "").Replace("Online Only", ""));

                            #endregion description

                            #region Bullets
                            if (_Description1.Length > 0)
                            {
                                if (_Description1.ToLower().Contains("features"))
                                {
                                    List<string> _Remove = new List<string>();
                                    try
                                    {
                                        foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//span[@id=\"product_description\"]")[0].SelectNodes(".//ul"))
                                        {
                                            try
                                            {
                                                if (_Node.PreviousSibling.InnerText.ToLower().Contains("features"))
                                                {
                                                    dataGridView1.Rows[index].Cells[4].Value = CommanFunction.StripHTML(_Node.InnerHtml);
                                                }
                                            }
                                            catch
                                            {
                                            }

                                        }
                                    }
                                    catch
                                    {
                                    }

                                }
                            }
                            #endregion Bullets

                            #region manufacturer
                            HtmlNodeCollection _manufacturer = _Work1doc.DocumentNode.SelectNodes("//meta[@itemprop=\"manufacturer\"]");
                            if (_manufacturer == null)
                            {
                                dataGridView1.Rows[index].Cells[5].Value = BrandName1;
                                dataGridView1.Rows[index].Cells[6].Value = BrandName1;
                            }
                            else
                            {
                                if (_manufacturer[0].InnerText.Trim() == "")
                                {
                                    dataGridView1.Rows[index].Cells[5].Value = BrandName1;
                                    dataGridView1.Rows[index].Cells[6].Value = BrandName1;
                                }
                                else
                                {
                                    dataGridView1.Rows[index].Cells[5].Value = _manufacturer[0].InnerText;
                                    dataGridView1.Rows[index].Cells[6].Value = _manufacturer[0].InnerText;
                                }
                            }
                            #endregion manufacturer

                            #region For decsription empty
                            try
                            {
                                if (dataGridView1.Rows[index].Cells[3].Value == null || dataGridView1.Rows[index].Cells[3].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[3].Value.ToString()))
                                {
                                    dataGridView1.Rows[index].Cells[3].Value = dataGridView1.Rows[index].Cells[2].Value.ToString().Replace(">", "").Replace("<", "");
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

                            HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@itemprop=\"offers\"]");
                            if (_Collection != null)
                            {
                                if (_Collection[0].InnerText.ToLower().Contains("out of stock"))
                                {
                                    dataGridView1.Rows[index].Cells[9].Value = "0";
                                }
                                else
                                {
                                    try
                                    {
                                        string Stock = _Collection[0].InnerText.ToLower();
                                        Stock = Stock.Substring(0, Stock.IndexOf("availability"));
                                        Stock = Stock.Substring(Stock.IndexOf("quantity in stock"));
                                        dataGridView1.Rows[index].Cells[9].Value = Convert.ToInt32(Stock.Replace(":", "").Replace("quantity in stock", "").Replace("availability", "").Trim());
                                    }
                                    catch
                                    {
                                        dataGridView1.Rows[index].Cells[9].Value = "5";
                                    }

                                }

                            }
                            else
                            {
                                dataGridView1.Rows[index].Cells[9].Value = "0";
                            }


                            HtmlNodeCollection _NodePrice = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]");
                            if (_NodePrice != null)
                            {
                                dataGridView1.Rows[index].Cells[7].Value = _NodePrice[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                            }


                            #endregion price,stock

                            #region sku

                            HtmlNodeCollection _Collectionsku = _Work1doc.DocumentNode.SelectNodes("//span[@class=\"product_code\"]");
                            if (_Collectionsku != null)
                            {
                                dataGridView1.Rows[index].Cells[1].Value = _Collectionsku[0].InnerText.Trim();
                            }

                            #endregion sku

                            #region Image
                            string Images = "";
                            HtmlNodeCollection _Collection1 = _Work1doc.DocumentNode.SelectNodes("//a[@rel=\"shadowbox[ProductImages]\"]");

                            if (_Collection1 != null)
                            {
                                foreach (HtmlNode _Node in _Collection1)
                                {
                                    foreach (HtmlAttribute _Node1 in _Node.Attributes)
                                    {
                                        if (_Node1.Name.ToLower() == "href")
                                        {
                                            if (!Images.Contains(_Node1.Value))
                                            {
                                                Images = Images + _Node1.Value + "@";
                                            }
                                        }
                                    }
                                }
                            }

                            if (Images.Length < 10)
                            {
                                HtmlNodeCollection _Collectionimg = _Work1doc.DocumentNode.SelectNodes("//img[@class=\"vCSS_img_product_photo\"]");
                                if (_Collectionimg != null)
                                {
                                    foreach (HtmlAttribute _Att in _Collectionimg[0].Attributes)
                                    {
                                        if (_Att.Name.ToLower() == "src")
                                        {
                                            Images = Images + _Att.Value + "@";
                                        }
                                    }

                                }
                            }
                            if (Images.Length > 0)
                            {
                                Images = Images.Substring(0, Images.Length - 1);
                            }
                            dataGridView1.Rows[index].Cells[10].Value = Images;
                            #endregion  Image
                        }
                    }
                    else
                    {
                    }
                }
            }
            #endregion CanadaDrones
        }
        public void work_RunWorkerAsync1(object sender, RunWorkerCompletedEventArgs e)
        {
            #region CanadaDrones
            if (_IScanadadrones)
            {
                if (_IsProduct)
                {
                    if (_Work1doc2.DocumentNode != null)
                    {
                        if (!_Work1doc2.DocumentNode.InnerHtml.ToLower().Contains("product_options_heading"))
                        {
                            int index = 0;

                            index = gridindex;
                            gridindex++;
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[index].Cells[0].Value = index;
                            dataGridView1.Rows[index].Cells[11].Value = Url2;

                            #region title
                            HtmlNodeCollection _Title = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"name\"]");
                            if (_Title != null)
                            {
                                dataGridView1.Rows[index].Cells[2].Value = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim())).Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("Online Only", "").Replace("online only", "");

                            }
                            else
                            {
                                HtmlNodeCollection _Title1 = _Work1doc2.DocumentNode.SelectNodes("//meta[@property=\"og:title\"]");
                                if (_Title1 != null)
                                {
                                    dataGridView1.Rows[index].Cells[2].Value = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title1[0].InnerText.Trim())).Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("Online Only", "").Replace("online only", "");
                                }
                            }
                            #endregion title

                            #region description

                            _Description2 = "";
                            HtmlNodeCollection _description = _Work1doc2.DocumentNode.SelectNodes("//span[@id=\"product_description\"]");
                            if (_description != null)
                            {
                                _Description2 = _description[0].InnerHtml.Replace("Product Description", "").Trim();

                                #region CodeToReMoveText
                                if (_Description2.Trim().Length > 0)
                                {
                                    List<string> _Remove = new List<string>();
                                    foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//span[@id=\"product_description\"]")[0].ChildNodes)
                                    {
                                        if (_Node.InnerText.ToLower().Contains("special orders") || _Node.InnerText.ToLower().Contains("stock is limited") || _Node.InnerText.ToLower().Contains("please note") || _Node.InnerText.ToLower().Contains("credit card"))
                                        {
                                            _Remove.Add(_Node.InnerHtml);
                                        }

                                    }

                                    foreach (string _rem in _Remove)
                                    {
                                        _Description2 = _Description2.Replace(_rem, "");
                                    }
                                }
                                #endregion CodeToReMoveText
                                _Description2 = CommanFunction.Removeunsuaalcharcterfromstring(CommanFunction.StripHTML(_Description2).Trim());

                            }
                            else
                            {


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

                            dataGridView1.Rows[index].Cells[3].Value = System.Net.WebUtility.HtmlDecode(_Description2.Replace("Â", "").Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("online only", "").Replace("Online Only", ""));

                            #endregion description

                            #region Bullets
                            if (_Description2.Length > 0)
                            {
                                if (_Description2.ToLower().Contains("features"))
                                {
                                    List<string> _Remove = new List<string>();
                                    try
                                    {
                                        foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//span[@id=\"product_description\"]")[0].SelectNodes(".//ul"))
                                        {
                                            try
                                            {
                                                if (_Node.PreviousSibling.InnerText.ToLower().Contains("features"))
                                                {
                                                    dataGridView1.Rows[index].Cells[4].Value = CommanFunction.StripHTML(_Node.InnerHtml);
                                                }
                                            }
                                            catch
                                            {
                                            }

                                        }
                                    }
                                    catch
                                    {
                                    }


                                }
                            }
                            #endregion Bullets

                            #region manufacturer
                            HtmlNodeCollection _manufacturer = _Work1doc2.DocumentNode.SelectNodes("//meta[@itemprop=\"manufacturer\"]");
                            if (_manufacturer == null)
                            {
                                dataGridView1.Rows[index].Cells[5].Value = BrandName2;
                                dataGridView1.Rows[index].Cells[6].Value = BrandName2;
                            }
                            else
                            {
                                if (_manufacturer[0].InnerText.Trim() == "")
                                {
                                    dataGridView1.Rows[index].Cells[5].Value = BrandName2;
                                    dataGridView1.Rows[index].Cells[6].Value = BrandName2;
                                }
                                else
                                {
                                    dataGridView1.Rows[index].Cells[5].Value = _manufacturer[0].InnerText;
                                    dataGridView1.Rows[index].Cells[6].Value = _manufacturer[0].InnerText;
                                }
                            }
                            #endregion manufacturer

                            #region For decsription empty
                            try
                            {
                                if (dataGridView1.Rows[index].Cells[3].Value == null || dataGridView1.Rows[index].Cells[3].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[3].Value.ToString()))
                                {
                                    dataGridView1.Rows[index].Cells[3].Value = dataGridView1.Rows[index].Cells[2].Value.ToString().Replace(">", "").Replace("<", "");
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

                            HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//div[@itemprop=\"offers\"]");
                            if (_Collection != null)
                            {
                                if (_Collection[0].InnerText.ToLower().Contains("out of stock"))
                                {
                                    dataGridView1.Rows[index].Cells[9].Value = "0";
                                }
                                else
                                {
                                    try
                                    {
                                        string Stock = _Collection[0].InnerText.ToLower();
                                        Stock = Stock.Substring(0, Stock.IndexOf("availability"));
                                        Stock = Stock.Substring(Stock.IndexOf("quantity in stock"));
                                        dataGridView1.Rows[index].Cells[9].Value = Convert.ToInt32(Stock.Replace(":", "").Replace("quantity in stock", "").Replace("availability", "").Trim());
                                    }
                                    catch
                                    {
                                        dataGridView1.Rows[index].Cells[9].Value = "5";
                                    }

                                }

                            }
                            else
                            {
                                dataGridView1.Rows[index].Cells[9].Value = "0";
                            }


                            HtmlNodeCollection _NodePrice = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]");
                            if (_NodePrice != null)
                            {
                                dataGridView1.Rows[index].Cells[7].Value = _NodePrice[0].InnerText.Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();

                            }
                            if (dataGridView1.Rows[index].Cells[7].Value == null || dataGridView1.Rows[index].Cells[7].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[7].Value.ToString()))
                            {
                                dataGridView1.Rows[index].Cells[7].Value = "0";
                            }

                            #endregion price,stock

                            #region sku

                            HtmlNodeCollection _Collectionsku = _Work1doc2.DocumentNode.SelectNodes("//span[@class=\"product_code\"]");
                            if (_Collectionsku != null)
                            {
                                dataGridView1.Rows[index].Cells[1].Value = _Collectionsku[0].InnerText.Trim();
                            }

                            #endregion sku

                            #region Image
                            string Images = "";
                            HtmlNodeCollection _Collection1 = _Work1doc2.DocumentNode.SelectNodes("//a[@rel=\"shadowbox[ProductImages]\"]");

                            if (_Collection1 != null)
                            {
                                foreach (HtmlNode _Node in _Collection1)
                                {
                                    foreach (HtmlAttribute _Node1 in _Node.Attributes)
                                    {
                                        if (_Node1.Name.ToLower() == "href")
                                        {
                                            if (!Images.Contains(_Node1.Value))
                                            {
                                                Images = Images + _Node1.Value + "@";
                                            }
                                        }
                                    }
                                }
                            }

                            if (Images.Length < 10)
                            {
                                HtmlNodeCollection _Collectionimg = _Work1doc2.DocumentNode.SelectNodes("//img[@class=\"vCSS_img_product_photo\"]");
                                if (_Collectionimg != null)
                                {
                                    foreach (HtmlAttribute _Att in _Collectionimg[0].Attributes)
                                    {
                                        if (_Att.Name.ToLower() == "src")
                                        {
                                            Images = Images + _Att.Value + "@";
                                        }
                                    }

                                }
                            }
                            if (Images.Length > 0)
                            {
                                Images = Images.Substring(0, Images.Length - 1);
                            }
                           
                            dataGridView1.Rows[index].Cells[10].Value = Images;
                            #endregion  Image
                        }
                    }
                    else
                    {
                    }
                }
            }
            #endregion CanadaDrones
        }
        public string GenrateSkuFromDatbase(string sku, string Name, string storename, decimal Price)
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
            _percent.Text = e.ProgressPercentage + "%  Completed";
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


        public void GenerateCSVFile()
        {
            string Filename = "data" + DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "");
            DataTable exceldt = new DataTable();
            exceldt.Columns.Add("Rowid", typeof(int));
            exceldt.Columns.Add("SKU", typeof(string));
            exceldt.Columns.Add("Product Name", typeof(string));
            exceldt.Columns.Add("Product Description", typeof(string));
            exceldt.Columns.Add("Bullet Points", typeof(string));
            exceldt.Columns.Add("Manufacturer", typeof(string));
            exceldt.Columns.Add("Brand Name", typeof(string));
            exceldt.Columns.Add("Price", typeof(string));
            exceldt.Columns.Add("Currency", typeof(string));
            exceldt.Columns.Add("In Stock", typeof(string));
            exceldt.Columns.Add("Image URL", typeof(string));
            exceldt.Columns.Add("Image URL1", typeof(string));
            exceldt.Columns.Add("Image URL2", typeof(string));



            for (int m = 0; m < dataGridView1.Rows.Count - 1; m++)
            {
                exceldt.Rows.Add();
                for (int n = 0; n < dataGridView1.Columns.Count - 1; n++)
                {
                    if (dataGridView1.Rows[m].Cells[n].Value == null || dataGridView1.Rows[m].Cells[n].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[m].Cells[n].Value.ToString()))
                        continue;
                    if (n == 10)
                    {
                        string[] Images = dataGridView1.Rows[m].Cells[n].Value.ToString().Split('@');
                        try
                        {
                            exceldt.Rows[m][n] = Images[0];
                        }
                        catch
                        {
                        }
                        if (Images.Length > 1)
                        {
                            try
                            {
                                exceldt.Rows[m][n + 1] = Images[1];
                            }
                            catch
                            {
                            }
                            try
                            {
                                exceldt.Rows[m][n + 2] = Images[2];
                            }
                            catch
                            {
                            }
                        }

                    }
                    else
                    {
                        exceldt.Rows[m][n] = dataGridView1.Rows[m].Cells[n].Value.ToString();

                    }
                }
            }


            try
            {
                using (CsvFileWriter writer = new CsvFileWriter(Application.StartupPath + "/" + Filename + ".txt"))
                {
                    CsvFileWriter.CsvRow row = new CsvFileWriter.CsvRow();//HEADER FOR CSV FILE



                    row.Add("SKU");
                    row.Add("Product Name");
                    row.Add("Product Description");
                    row.Add("Bullet Points");
                    row.Add("Manufacturer");
                    row.Add("Brand Name");
                    row.Add("Price");
                    row.Add("Currency");
                    row.Add("In Stock");
                    row.Add("Image URL");
                    row.Add("Image URL1");
                    row.Add("Image URL2");
                    writer.WriteRow(row);//INSERT TO CSV FILE HEADER

                    List<string> Skus = new System.Collections.Generic.List<string>();


                    DataTable _TableProceSort = (from dTable in
                                                     exceldt.AsEnumerable()
                                                 where Convert.ToDecimal(dTable["Price"]) > 0
                                                 orderby Convert.ToDecimal(dTable["Price"]) descending
                                                 select dTable).CopyToDataTable();
                    for (int m = 0; m < _TableProceSort.Rows.Count; m++)
                    {
                        try
                        {
                            if (_TableProceSort.Rows[m]["SKU"].ToString().Trim() != "")
                            {
                                if (!Skus.Contains(_TableProceSort.Rows[m]["SKU"].ToString()) && !_TableProceSort.Rows[m]["Product Name"].ToString().ToLower().Contains("in-store only") && !_TableProceSort.Rows[m]["Product Name"].ToString().ToLower().Contains("in-store items") && !_TableProceSort.Rows[m]["Product Description"].ToString().ToLower().Contains("in-store only") && !_TableProceSort.Rows[m]["Product Description"].ToString().ToLower().Contains("in-store items"))
                                {
                                    Skus.Add(_TableProceSort.Rows[m]["SKU"].ToString());
                                    CsvFileWriter.CsvRow row1 = new CsvFileWriter.CsvRow();
                                    for (int n = 1; n < _TableProceSort.Columns.Count; n++)
                                    {
                                        row1.Add(String.Format("{0}", _TableProceSort.Rows[m][n].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", "")));
                                    }
                                    writer.WriteRow(row1);
                                }
                                else
                                {

                                }
                            }
                            else
                            {

                            }
                        }
                        catch
                        {
                        }
                    }
                }
                System.Diagnostics.Process.Start(Application.StartupPath + "/" + Filename + ".txt");//OPEN THE CSV FILE ,,CSV FILE NAMED AS DATA.CSV
            }
            catch (Exception) { MessageBox.Show("file is already open\nclose the file"); }
            return;
        }

        public class CsvFileWriter : StreamWriter //Writing  data to CSV
        {
            public CsvFileWriter(Stream stream)
                : base(stream)
            {
            }

            public CsvFileWriter(string filename)
                : base(filename)
            {
            }
            public class CsvRow : List<string> //Making each CSV rows
            {
                public string LineText { get; set; }
            }

            public void WriteRow(CsvRow row)
            {
                StringBuilder builder = new StringBuilder();
                bool firstColumn = true;
                foreach (string value in row)
                {
                    builder.Append(value.Replace("\n", "") + "\t");
                }
                row.LineText = builder.ToString();
                WriteLine(row.LineText);
            }

        }
        private void btnsubmit_Click(object sender, System.EventArgs e)
        {
            Process();

        }

        private void Form1_Shown(object sender, System.EventArgs e)
        {

        }
    }
}
