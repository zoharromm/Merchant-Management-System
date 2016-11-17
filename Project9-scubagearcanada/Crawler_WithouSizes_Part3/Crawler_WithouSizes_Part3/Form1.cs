using System;
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
using Crawler_WithouSizes_Part7;
using System.Xml;
using Newtonsoft.Json;
using WatiN.Core;
namespace Crawler_WithouSizes_Part3
{
    public partial class Form1 : System.Windows.Forms.Form
    {


        #region DatbaseVariable
        SqlConnection Connection = new SqlConnection(System.Configuration.ConfigurationSettings.
                                               AppSettings["connectionstring"]);
        #endregion DatbaseVariable
        StreamWriter _writer = new StreamWriter(Application.StartupPath + "/test.csv");

        #region ClassTypeVariable
        List<Crawler_WithouSizes_Part7.BusinessLayer.Product> Worker1Products = new List<Crawler_WithouSizes_Part7.BusinessLayer.Product>();
        List<Crawler_WithouSizes_Part7.BusinessLayer.Product> Worker2Products = new List<Crawler_WithouSizes_Part7.BusinessLayer.Product>();
        List<string> Url = new List<string>();
        #endregion ClassTypeVariable

        #region booltypevariable

        bool _ISscubagearcanada = true;
        bool _IsProduct = false;
        bool _IsCategory = true;
        bool _IsCategorypaging = false;
        bool _Stop = false;
        bool Erorr_scubagearcanada1 = true;
        bool Erorr_scubagearcanada2 = true;

        #endregion booltypevariable
        #region datatable

        DataTable _TableWork1 = new DataTable();
        DataTable _TableWork2 = new DataTable();

        #endregion datatable

        #region IeVariable

        IE _Worker1 = null;
        IE _Worker2 = null;

        #endregion IeVariable

        #region intypevariable

        int gridindex = 0;
        int _Saleeventindex = 0;

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
        List<string> ProductName = new List<string>();
        List<string> _Url = new List<string>();
        List<string> _dateofbirth = new List<string>();
        Dictionary<string, string> _ProductUrl = new Dictionary<string, string>();
        List<string> Skus = new List<string>();
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

            #region Datacolumn
            DataColumn _ColPrice = new DataColumn();
            _ColPrice.ColumnName = "Price";
            _ColPrice.DataType = Type.GetType("System.String");
            _TableWork1.Columns.Add(_ColPrice);

            DataColumn _ColSku = new DataColumn();
            _ColSku.ColumnName = "sku";
            _ColSku.DataType = Type.GetType("System.String");
            _TableWork1.Columns.Add(_ColSku);


            DataColumn _ColColor = new DataColumn();
            _ColColor.ColumnName = "Color";
            _ColColor.DataType = Type.GetType("System.String");
            _TableWork1.Columns.Add(_ColColor);

            DataColumn _Colsize = new DataColumn();
            _Colsize.ColumnName = "size";
            _Colsize.DataType = Type.GetType("System.String");
            _TableWork1.Columns.Add(_Colsize);


            DataColumn _ColStock = new DataColumn();
            _ColStock.ColumnName = "Stock";
            _ColStock.DataType = Type.GetType("System.String");
            _TableWork1.Columns.Add(_ColStock);

            _TableWork2 = _TableWork1.Clone();
            #endregion Datacolumn
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



            #region scubagearcanada
            _ISscubagearcanada = true;
            _ScrapeUrl = "http://www.scubagearcanada.ca/";
            try
            {

                _lblerror.Visible = true;
                _lblerror.Text = "We are going to read  category url for " + chkstorelist.Items[0].ToString() + " Website";
                _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));
                HtmlNodeCollection _Collection = null;
                _Collection = _Work1doc.DocumentNode.SelectNodes("//ul[@class=\"sf-horizontal category-list treeview\"]/li");
                if (_Collection == null)
                    _Collection = _Work1doc.DocumentNode.SelectNodes("//ul[@class=\"sf-menu sf-horizontal\"]/li");
                if (_Collection != null)
                {
                    foreach (HtmlNode _Node in _Collection)
                    {
                        try
                        {
                            HtmlAttributeCollection _AttributeCollection = _Node.SelectNodes(".//a")[0].Attributes;
                            foreach (HtmlAttribute _Attribute in _AttributeCollection)
                            {
                                if (_Attribute.Name.ToLower() == "href")
                                {
                                    if (!_Node.SelectNodes(".//a")[0].InnerText.ToLower().StartsWith("all") && _Attribute.Value.Trim().Length > 0 && _Attribute.Value != "#")
                                    {
                                        try
                                        {
                                            CategoryUrl.Add(_Attribute.Value, _Node.SelectNodes(".//a")[0].InnerText.Trim());
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
                        Recusion(_Node);
                    }
                }
             //   CategoryUrl.Clear();
                if (CategoryUrl.Count() > 0)
                {

                    #region Category

                    //DisplayRecordProcessdetails("We are going to read paging  from category pages for " + chkstorelist.Items[0].ToString() + " Website", "Total  Category :" + CategoryUrl.Count());
                    //_IsCategorypaging = true;
                    //foreach (var Caturl in CategoryUrl)
                    //{

                    //    while (_Work.IsBusy || _Work1.IsBusy)
                    //    {
                    //        Application.DoEvents();

                    //    }

                    //    while (_Stop)
                    //    {
                    //        Application.DoEvents();
                    //    }



                    //    if (!_Work.IsBusy)
                    //    {
                    //        Url1 = Caturl.Key;
                    //        BrandName1 = Caturl.Value;
                    //        _Work.RunWorkerAsync();
                    //    }

                    //    else
                    //    {
                    //        Url2 = Caturl.Key;
                    //        BrandName2 = Caturl.Value;
                    //        _Work1.RunWorkerAsync();

                    //    }

                    //}

                    while (_Work.IsBusy || _Work1.IsBusy)
                    {
                        Application.DoEvents();

                    }
                    #endregion Category

                    #region ProductUrl

                    System.Threading.Thread.Sleep(1000);
                    _Bar1.Value = 0;
                    _Saleeventindex = 0;
                    _IsCategorypaging = false;
                    _IsCategory = true;
                    SubCategoryUrl = CategoryUrl;
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
                _Saleeventindex = 0;
                _IsCategory = false;
                _IsProduct = true;

                DisplayRecordProcessdetails("We are going to read Product Information for   " + chkstorelist.Items[0].ToString() + " Website", "Total  Products :" + _ProductUrl.Count());
                #region iebrowser intialization

                _Worker1 = new IE();
                _Worker2 = new IE();
                //_Worker1.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
                //_Worker2.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
                ////_ProductUrl.Add("http://scubagearcanada.ca/gomask-for-gopro-original/#.VkteD9zyHIU", "");
                ////_ProductUrl.Add("http://scubagearcanada.ca/akona-armoretex-kevlar-5mm-glove/#.Vkt5atzyHIU", "");
                ////_ProductUrl.Add("http://scubagearcanada.ca/ultra-quick-dry-towel/#.Vkt7UdzyHIU", "");
                //_ProductUrl.Add("http://scubagearcanada.ca/ultra-quick-dry-towel/#.Vky4dNzyHIV", "");
                //_ProductUrl.Add("http://scubagearcanada.ca/akona-armoretex-kevlar-5mm-glove/#.Vkt5atzyHIU", "");
                
                #endregion iebrowser intialization
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

                _Worker1.Close();
                _Worker2.Close();

                #region InsertdataIngrid

                foreach (Crawler_WithouSizes_Part7.BusinessLayer.Product prd in Worker1Products)
                {
                    if (prd.Name.Trim().Length > 0)
                    {
                        int index = gridindex;
                        gridindex++;
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[index].Cells[0].Value = index;
                        dataGridView1.Rows[index].Cells[1].Value = prd.SKU;
                        dataGridView1.Rows[index].Cells[2].Value = prd.Name;
                        dataGridView1.Rows[index].Cells[3].Value = prd.Description;
                        dataGridView1.Rows[index].Cells[4].Value = prd.Bulletpoints;
                        dataGridView1.Rows[index].Cells[5].Value = prd.Manufacturer;
                        dataGridView1.Rows[index].Cells[6].Value = prd.Brand;
                        dataGridView1.Rows[index].Cells[7].Value = prd.Price;
                        dataGridView1.Rows[index].Cells[8].Value = prd.Currency;
                        dataGridView1.Rows[index].Cells[9].Value = prd.Stock;
                        dataGridView1.Rows[index].Cells[10].Value = prd.Image;
                        dataGridView1.Rows[index].Cells[11].Value = prd.URL;
                        dataGridView1.Rows[index].Cells[12].Value = prd.Size;
                        dataGridView1.Rows[index].Cells[13].Value = prd.Color;
                        dataGridView1.Rows[index].Cells[14].Value = prd.Isparent;
                        dataGridView1.Rows[index].Cells[15].Value = prd.parentsku;
                        dataGridView1.Rows[index].Cells[16].Value = prd.Bulletpoints1;
                        dataGridView1.Rows[index].Cells[17].Value = prd.Bulletpoints2;
                        dataGridView1.Rows[index].Cells[18].Value = prd.Bulletpoints3;
                        dataGridView1.Rows[index].Cells[19].Value = prd.Bulletpoints4;
                        dataGridView1.Rows[index].Cells[20].Value = prd.Bulletpoints5;
                        dataGridView1.Rows[index].Cells[21].Value = prd.Category;
                        dataGridView1.Rows[index].Cells[22].Value = prd.Weight;
                    }

                }
                foreach (Crawler_WithouSizes_Part7.BusinessLayer.Product prd in Worker2Products)
                {
                    if (prd.Name.Trim().Length > 0)
                    {
                        int index = gridindex;
                        gridindex++;
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[index].Cells[0].Value = index;
                        dataGridView1.Rows[index].Cells[1].Value = prd.SKU;
                        dataGridView1.Rows[index].Cells[2].Value = prd.Name;
                        dataGridView1.Rows[index].Cells[3].Value = prd.Description;
                        dataGridView1.Rows[index].Cells[4].Value = prd.Bulletpoints;
                        dataGridView1.Rows[index].Cells[5].Value = prd.Manufacturer;
                        dataGridView1.Rows[index].Cells[6].Value = prd.Brand;
                        dataGridView1.Rows[index].Cells[7].Value = prd.Price;
                        dataGridView1.Rows[index].Cells[8].Value = prd.Currency;
                        dataGridView1.Rows[index].Cells[9].Value = prd.Stock;
                        dataGridView1.Rows[index].Cells[10].Value = prd.Image;
                        dataGridView1.Rows[index].Cells[11].Value = prd.URL;
                        dataGridView1.Rows[index].Cells[12].Value = prd.Size;
                        dataGridView1.Rows[index].Cells[13].Value = prd.Color;
                        dataGridView1.Rows[index].Cells[14].Value = prd.Isparent;
                        dataGridView1.Rows[index].Cells[15].Value = prd.parentsku;
                        dataGridView1.Rows[index].Cells[16].Value = prd.Bulletpoints1;
                        dataGridView1.Rows[index].Cells[17].Value = prd.Bulletpoints2;
                        dataGridView1.Rows[index].Cells[18].Value = prd.Bulletpoints3;
                        dataGridView1.Rows[index].Cells[19].Value = prd.Bulletpoints4;
                        dataGridView1.Rows[index].Cells[20].Value = prd.Bulletpoints5;
                        dataGridView1.Rows[index].Cells[21].Value = prd.Category;
                        dataGridView1.Rows[index].Cells[22].Value = prd.Weight;
                    }
                }
                #endregion InsertdataIngrid
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

            #endregion scubagearcanada
            _writer.Close();
        }

        public void Recusion(HtmlNode _Node)
        {
            HtmlNodeCollection _InnerCollection = null;
            _InnerCollection = _Node.SelectNodes(".//ul/li");
            if (_InnerCollection != null)
            {
                foreach (HtmlNode _Nodeinner in _InnerCollection)
                {
                    try
                    {
                        foreach (HtmlAttribute _Attributeinner in _Nodeinner.SelectNodes(".//a")[0].Attributes)
                        {
                            if (!_Nodeinner.SelectNodes(".//a")[0].InnerText.ToLower().StartsWith("all") && _Attributeinner.Value.Trim().Length > 0 && _Attributeinner.Value != "#")
                            {
                                if (_Attributeinner.Name.ToLower() == "href")
                                {
                                    try
                                    {
                                        CategoryUrl.Add(_Attributeinner.Value, _Nodeinner.SelectNodes(".//a")[0].InnerText.Trim());
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
                    Recusion(_Nodeinner);
                }
            }
        }
        public void work_dowork(object sender, DoWorkEventArgs e)
        {
            bool _Iserror = false;
            int CountError = 0;
            if (!_IsProduct)
            {
                do
                {
                    try
                    {
                        CountError++;
                        _Work1doc.LoadHtml(_Client1.DownloadString(Url1));
                        _Iserror = false;
                    }
                    catch
                    {
                        _Iserror = true;
                    }
                } while (_Iserror && CountError < 5);
            }
            else
            {
                Erorr_scubagearcanada1 = true;
                int CounterError = 0;
                do
                {
                    try
                    {
                        _Worker1.GoToNoWait(Url1);
                        Erorr_scubagearcanada1 = false;
                    }
                    catch
                    {
                        CounterError++;
                    }
                } while (Erorr_scubagearcanada1 && CounterError < 20);
            }
            #region scubagearcanada
            if (_ISscubagearcanada)
            {

                if (_IsCategorypaging)
                {
                    if (!_Iserror)
                    {
                        HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"col-sm-6 text-right\"]");
                        if (_Collection != null)
                        {
                            try
                            {
                                string PagingText = _Collection[0].InnerText.ToLower();
                                PagingText = PagingText.Substring(0, PagingText.IndexOf("pages"));
                                PagingText = PagingText.Substring(PagingText.IndexOf("(")).Trim();
                                int _TotalPages = Convert.ToInt32(Regex.Replace(PagingText.Replace("\r", "").Replace("\n", "").ToLower().Replace("page", "").Replace("of", "").Trim(), "[^0-9+]", string.Empty));
                                for (int Page = 1; Page <= _TotalPages; Page++)
                                {
                                    try
                                    {
                                        SubCategoryUrl.Add(Url1 + "?page=" + Page, BrandName1);
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
                            SubCategoryUrl.Add(Url1, BrandName1);
                        }

                    }
                    else
                    {
                    }
                    _Saleeventindex++;
                    _Work.ReportProgress((_Saleeventindex * 100 / CategoryUrl.Count()));

                }
                else if (_IsCategory)
                {
                    if (!_Iserror)
                    {
                        HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"ProductDetails\"]/a");
                        if (_Collection != null)
                        {

                            foreach (HtmlNode _Node in _Collection)
                            {
                                if (!ProductName.Contains(_Node.InnerText.Trim()))
                                {
                                    ProductName.Add(_Node.InnerText.Trim());
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

                        }
                        else
                        {
                        }

                        _Saleeventindex++;
                        _Work.ReportProgress((_Saleeventindex * 100 / SubCategoryUrl.Count()));
                    }
                    else
                    {
                    }

                }
                else
                {
                    _Saleeventindex++;
                    _Work.ReportProgress((_Saleeventindex * 100 / _ProductUrl.Count()));
                }
            }

            #endregion scubagearcanada

        }
        public void work_dowork1(object sender, DoWorkEventArgs e)
        {

            bool _Iserror = false;
            int CountError = 0;
            if (!_IsProduct)
            {
                do
                {
                    try
                    {
                        CountError++;
                        _Work1doc2.LoadHtml(_Client2.DownloadString(Url2));
                        _Iserror = false;
                    }
                    catch
                    {
                        _Iserror = true;
                    }
                } while (_Iserror && CountError < 5);
            }
            else
            {
                Erorr_scubagearcanada2 = true;
                int CounterError = 0;
                do
                {
                    try
                    {
                        _Worker2.GoToNoWait(Url2);
                        Erorr_scubagearcanada2 = false;
                    }
                    catch
                    {
                        CounterError++;
                    }
                } while (Erorr_scubagearcanada2 && CounterError < 20);
            }
            #region scubagearcanada
            if (_ISscubagearcanada)
            {

                if (_IsCategorypaging)
                {
                    if (!_Iserror)
                    {
                        HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"col-sm-6 text-right\"]");
                        if (_Collection != null)
                        {
                            try
                            {
                                string PagingText = _Collection[0].InnerText.ToLower();
                                PagingText = PagingText.Substring(0, PagingText.IndexOf("pages"));
                                PagingText = PagingText.Substring(PagingText.IndexOf("(")).Trim();
                                int _TotalPages = Convert.ToInt32(Regex.Replace(PagingText.Replace("\r", "").Replace("\n", "").ToLower().Replace("page", "").Replace("of", "").Trim(), "[^0-9+]", string.Empty));
                                for (int Page = 1; Page <= _TotalPages; Page++)
                                {
                                    try
                                    {
                                        SubCategoryUrl.Add(Url2 + "?page=" + Page, BrandName2);
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
                            SubCategoryUrl.Add(Url2, BrandName2);
                        }

                    }
                    else
                    {
                    }
                    _Saleeventindex++;
                    _Work1.ReportProgress((_Saleeventindex * 100 / CategoryUrl.Count()));

                }
                else if (_IsCategory)
                {
                    if (!_Iserror)
                    {
                        HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"ProductDetails\"]/a");
                        if (_Collection != null)
                        {

                            foreach (HtmlNode _Node in _Collection)
                            {
                                if (!ProductName.Contains(_Node.InnerText.Trim()))
                                {
                                    ProductName.Add(_Node.InnerText.Trim());
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

                        }
                        else
                        {
                        }

                        _Saleeventindex++;
                        _Work1.ReportProgress((_Saleeventindex * 100 / SubCategoryUrl.Count()));
                    }
                    else
                    {
                    }

                }
                else
                {
                    _Saleeventindex++;
                    _Work1.ReportProgress((_Saleeventindex * 100 / _ProductUrl.Count()));
                }
            }

            #endregion scubagearcanada
        }
        public void work_RunWorkerAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            #region scubagearcanada
            if (_ISscubagearcanada)
            {
                if (_IsProduct)
                {
                    if (!Erorr_scubagearcanada1)
                    {
                        _Worker1.WaitForComplete();
                        #region CheckPageLoaded

                        #region variable
                        int checkcounter = 0;
                        #endregion variable

                        if (_Worker1.Html == null)
                        {
                            do
                            {
                                System.Threading.Thread.Sleep(10);
                                Application.DoEvents();
                                checkcounter++;
                            } while (_Worker1.Html == null && checkcounter < 10);
                        }

                        #endregion CheckPageLoaded
                        if (_Worker1.Html != null)
                        {
                            _Work1doc.LoadHtml(_Worker1.Html);
                            try
                            {

                                try
                                {

                                    #region Title
                                    string Title = "";
                                    HtmlNodeCollection _Title = null;
                                    _Title = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"ProductDetailsGrid desktop PriceBorderBottom\"]/div[@class=\"DetailRow\"]/h1");
                                    if (_Title == null)
                                        _Title = _Work1doc.DocumentNode.SelectNodes("//meta=[@property=\"og:title\"]");
                                    if (_Title != null)
                                    {
                                        Title = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim())).Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("Online Only", "").Replace("online only", "").Replace("â„¢", "™");
                                    }

                                    #endregion Title

                                    #region Description

                                    _Description1 = "";
                                    HtmlNodeCollection _description = _Work1doc.DocumentNode.SelectNodes("//div[@itemprop=\"description\"]");
                                    if (_description != null)
                                    {
                                        _Description1 = _description[0].InnerHtml.Replace("Quick Overview", "").Trim();
                                        _Description1 = CommanFunction.Removeunsuaalcharcterfromstring(CommanFunction.StripHTML(_Description1).Trim());

                                    }
                                    try
                                    {
                                        if (_Description1.Length > 2000)
                                            _Description1 = _Description1.Substring(0, 1997) + "...";
                                    }
                                    catch
                                    {
                                    }

                                    string Desc = System.Net.WebUtility.HtmlDecode(_Description1.Replace("Â", "").Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("online only", "").Replace("Online Only", "")).Replace(",", " ");
                                    if (Desc.Trim() != "")
                                    {
                                        if (Desc.Substring(0, 1) == "\"")
                                            _Description1 = Desc.Substring(1);
                                        else
                                            _Description1 = Desc;
                                    }

                                    #endregion Description

                                    #region BulletPoints
                                    string BulletPoints = "";
                                    List<string> LstBulletPoints = new List<string>();
                                    HtmlNodeCollection _Bullets1 = null;
                                    _Bullets1 = _Work1doc.DocumentNode.SelectNodes("//div[@itemprop=\"description\"]");

                                    if (_Bullets1 != null)
                                    {

                                        foreach (HtmlNode _BullNode in _Bullets1)
                                        {
                                            BulletPoints = BulletPoints + System.Net.WebUtility.HtmlDecode(CommanFunction.StripHTML(_BullNode.InnerText).Trim()) + ".";
                                        }

                                    }
                                    if (BulletPoints.Trim() != "")
                                    {
                                        if (BulletPoints.Length >= 500)
                                            LstBulletPoints.Add(BulletPoints.Substring(0, 497).Replace("â„¢", "™"));
                                        else
                                            LstBulletPoints.Add(BulletPoints.Replace("â„¢", "™"));
                                    }
                                    #endregion BulletPoints

                                    #region Brand

                                    string Brand = "";
                                    HtmlNodeCollection _Brand = null;
                                    _Brand = _Work1doc.DocumentNode.SelectNodes("//h4[@class=\"BrandName\"]/a");
                                    if (_Brand == null)
                                        _Brand = _Work1doc.DocumentNode.SelectNodes("//h4[@class=\"BrandName\"]/a/span");
                                    if (_Brand != null)
                                    {
                                        Brand = _Brand[0].InnerText.Trim();
                                    }

                                    if (Brand.Trim() == "")
                                        Brand = "SCUBA";
                                    #endregion Brand

                                    #region Images

                                    string Images = "";

                                    HtmlNodeCollection _Image = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"ProductThumbImage\"]/a");
                                    if (_Image != null)
                                    {
                                        foreach (HtmlAttribute _Att in _Image[0].Attributes)
                                        {
                                            if (_Att.Name == "href")
                                                Images = _Att.Value.Trim() + "@";
                                        }
                                    }
                                    Dictionary<string, string> _ThumbImages = new Dictionary<string, string>();
                                    string ImageUrl = "";
                                    string AltText = "";
                                    HtmlNodeCollection _ThumImage1 = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"ProductTinyImageList\"]");
                                    if (_ThumImage1 != null)
                                    {
                                        HtmlNodeCollection _ThumImage = _ThumImage1[0].SelectNodes(".//a");
                                        foreach (HtmlNode ThumNode in _ThumImage)
                                        {
                                            AltText = "";
                                            ImageUrl = "";
                                            foreach (HtmlAttribute _Att in ThumNode.Attributes)
                                            {
                                                if (_Att.Name.ToLower() == "rel")
                                                {
                                                    string LargeImage = _Att.Value;
                                                    try
                                                    {
                                                        LargeImage = LargeImage.Substring(LargeImage.IndexOf("\"largeimage\": \"")).Replace("\"largeimage\": \"", "");
                                                        LargeImage = LargeImage.Substring(0, LargeImage.IndexOf("\""));
                                                    }
                                                    catch
                                                    {
                                                        LargeImage = LargeImage.Substring(LargeImage.IndexOf("\"smallimage\": \"")).Replace("\"smallimage\": \"", "");
                                                        LargeImage = LargeImage.Substring(0, LargeImage.IndexOf("\""));
                                                    }
                                                    finally
                                                    {
                                                    }
                                                    if (!Images.Contains(LargeImage))
                                                    {
                                                        ImageUrl = LargeImage;
                                                        HtmlNodeCollection _CollectionImgalt = ThumNode.SelectNodes(".//img");
                                                        if (_CollectionImgalt != null)
                                                        {
                                                            foreach (HtmlAttribute _Attimg in _CollectionImgalt[0].Attributes)
                                                            {
                                                                if (_Attimg.Name.ToLower() == "alt")
                                                                    AltText = _Attimg.Value.ToLower().Trim();
                                                            }
                                                        }
                                                        Images = Images + LargeImage.Trim() + "@";
                                                        _ThumbImages.Add(ImageUrl, AltText);
                                                    }
                                                }

                                            }
                                        }
                                    }


                                    if (Images.Length > 0)
                                        Images = Images.Substring(0, Images.Length - 1);
                                    #endregion Images

                                    int VariantsCounter = 0;
                                    string ParentSku = "";
                                    VariantsCounter++;
                                    string Price = "";
                                    string Stock = "";
                                    string Sku = "";

                                    #region Price
                                    HtmlNodeCollection _Price = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]");
                                    if (_Price != null)
                                    {
                                        Price = _Price[0].InnerText.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace("cdn", "").Replace(":", "").Trim();
                                    }
                                    #endregion price

                                    #region stock
                                    Stock = "5";
                                    HtmlNodeCollection _Stock1 = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"content\"]");
                                    if (_Stock1 != null)
                                    {

                                        HtmlNodeCollection _Stock = _Stock1[0].SelectNodes(".//ul[@class=\"list-unstyled\"]");
                                        if (_Stock != null)
                                        {
                                            if (_Stock[0].InnerHtml.ToLower().Contains("out of stock") || _Stock[0].InnerHtml.ToLower().Contains("out stock"))
                                                Stock = "0";
                                        }
                                    }

                                    #endregion stock

                                    #region sku
                                    HtmlNodeCollection _sku = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");

                                    if (_sku != null)
                                    {
                                        Sku = _sku[0].InnerText.ToLower().Replace("product code:", "").Trim();
                                        ParentSku = Sku;

                                    }

                                    if (ParentSku == "")
                                    {
                                        ParentSku = CommanFunction.GeneratecolorSku("", Title);
                                        Sku = ParentSku;
                                    }
                                    ParentSku = ParentSku + "prnt";

                                    #endregion sku


                                    if (Skus.Contains(Sku))
                                        return;
                                    else
                                        Skus.Add(Sku);
                                    HtmlNodeCollection _Coll = null;
                                    _Coll = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"productAddToCartRight\"]");

                                    if (_Coll != null)
                                        _Coll = _Coll[0].SelectNodes(".//select");
                                    string ID = "";
                                    if (_Coll != null)
                                    {
                                        if (_Coll.Count == 1)
                                        {
                                            if (_Coll[0].Id == "qty_")
                                                ID = "qty_";

                                        }
                                    }
                                    if (_Coll == null || (ID.Length > 0))
                                    {

                                        Crawler_WithouSizes_Part7.BusinessLayer.Product Prd = new Crawler_WithouSizes_Part7.BusinessLayer.Product();
                                        Prd.Brand = Brand;
                                        Prd.Category = BrandName1;
                                        Prd.Manufacturer = Brand;
                                        Prd.Currency = "CAD";
                                        if (_Description1.Trim() != "")
                                            Prd.Description = _Description1;
                                        else
                                            Prd.Description = Title;
                                        Prd.URL = Url1;
                                        int BulletPointCounter = 0;
                                        foreach (var Points in LstBulletPoints)
                                        {
                                            BulletPointCounter++;
                                            switch (BulletPointCounter)
                                            {
                                                case 1:
                                                    Prd.Bulletpoints1 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                                case 2:
                                                    Prd.Bulletpoints2 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                                case 3:
                                                    Prd.Bulletpoints3 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                                case 4:
                                                    Prd.Bulletpoints4 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                                case 5:
                                                    Prd.Bulletpoints5 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                            }


                                        }

                                        Prd.Isparent = true;
                                        if (Sku.Length + 3 > 30)
                                            Prd.SKU = "SGC" + Sku.Substring(0, 27);
                                        else
                                            Prd.SKU = "SGC" + Sku;
                                        Prd.Stock = Stock;
                                        Prd.Price = Price;
                                        if (ParentSku.Length + 3 > 30)
                                            Prd.parentsku = "SGC" + ParentSku.Substring(0, 27);
                                        else
                                            Prd.parentsku = "SGC" + ParentSku;

                                        Prd.Weight = "0";
                                        Prd.Name = Title;
                                        Prd.Image = Images;
                                        Worker1Products.Add(Prd);

                                    }
                                    else
                                    {
                                        bool Kit = false;
                                        Dictionary<string, string> Options = new Dictionary<string, string>();
                                        foreach (HtmlNode _Node in _Coll)
                                        {
                                            foreach (HtmlAttribute _Att in _Node.Attributes)
                                            {
                                                if (_Att.Name.ToLower() == "id")
                                                {
                                                    if (_Att.Value.ToLower() != "qty_")
                                                    {
                                                        HtmlNodeCollection _LblColllection = _Work1doc.DocumentNode.SelectNodes("//label[@for=\"" + _Att.Value + "\"]");
                                                        if (_LblColllection != null)
                                                        {
                                                            if (_LblColllection[0].InnerText.Trim().ToLower().Contains("size"))
                                                                Options.Add(_Att.Value, "size");
                                                            else if (_LblColllection[0].InnerText.Trim().ToLower().Contains("color") || _LblColllection[0].InnerText.Trim().ToLower().Contains("colour"))
                                                                Options.Add(_Att.Value, "color");
                                                            else
                                                            {
                                                                Kit = true;
                                                                Options.Add(_Att.Value, _LblColllection[0].InnerText.Trim().ToLower().Replace(":", "").Replace("*", ""));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Kit = true;
                                                            Options.Add(_Att.Value, "");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (Options.Count > 0 && !Kit)
                                        {
                                            int Variantcounter = 0;
                                            SelectList _List = _Worker1.SelectList(Find.ById(Options.Keys.ElementAt(0)));
                                            int Counter = 0;
                                            bool DuplicacayExist = true;
                                            int CheckCounter = 0;
                                            string CheckSkuDuplicacy = "";
                                            foreach (Option option in _List.Options)
                                            {

                                                if (!option.Text.Trim().ToLower().Contains("please "))
                                                {
                                                    DuplicacayExist = true;
                                                    CheckCounter = 0;
                                                    _TableWork1.Rows.Clear();
                                                    _Worker1.SelectList(Find.ById(Options.Keys.ElementAt(0))).Option(option.Text).Select();
                                                    _Worker1.SelectList(Find.ById(Options.Keys.ElementAt(0))).Option(option.Text).Click();
                                                    _Worker1.WaitForComplete();
                                                    System.Threading.Thread.Sleep(2000);
                                                    if (Options.Count == 1)
                                                    {
                                                        _Work1doc.LoadHtml(_Worker1.Html);
                                                        while (DuplicacayExist && CheckCounter < 10)
                                                        {
                                                            HtmlNodeCollection _skuatt1 = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");
                                                            if (_skuatt1 != null)
                                                            {
                                                                if ((CheckSkuDuplicacy == _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim()) || _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim() == "")
                                                                {
                                                                    Application.DoEvents();
                                                                    _Work1doc.LoadHtml(_Worker1.Html);
                                                                }
                                                                else
                                                                {
                                                                    DuplicacayExist = false;
                                                                    CheckSkuDuplicacy = _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim();
                                                                }
                                                                CheckCounter++;
                                                            }
                                                        }
                                                        if (DuplicacayExist)
                                                            return;
                                                    }

                                                    if (Options.Count > 1)
                                                    {
                                                        SelectList _List1 = _Worker1.SelectList(Find.ById(Options.Keys.ElementAt(1)));
                                                        int Counter1 = 0;
                                                        foreach (var option1 in _List1.Options)
                                                        {
                                                            if (!option1.Text.ToLower().Contains("please "))
                                                            {
                                                                _Worker1.SelectList(Find.ById(Options.Keys.ElementAt(1))).Option(option1.Text).Select();
                                                                _Worker1.SelectList(Find.ById(Options.Keys.ElementAt(1))).Option(option1.Text).Click();
                                                                System.Threading.Thread.Sleep(2000);
                                                                _Work1doc.LoadHtml(_Worker1.Html);
                                                                while (DuplicacayExist && CheckCounter < 10)
                                                                {
                                                                    HtmlNodeCollection _skuatt1 = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");
                                                                    if (_skuatt1 != null)
                                                                    {
                                                                        if ((CheckSkuDuplicacy == _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim()) || _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim() == "")
                                                                        {
                                                                            Application.DoEvents();
                                                                            _Work1doc.LoadHtml(_Worker1.Html);
                                                                        }
                                                                        else
                                                                        {
                                                                            DuplicacayExist = false;
                                                                            CheckSkuDuplicacy = _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim();
                                                                        }
                                                                        CheckCounter++;
                                                                    }
                                                                }
                                                                if (DuplicacayExist)
                                                                    return;
                                                                option1.SelectNoWait();
                                                                DataRow _Row = _TableWork1.NewRow();
                                                                HtmlNodeCollection _PriceAtt = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]");
                                                                if (_PriceAtt != null)
                                                                {
                                                                    _Row[0] = _PriceAtt[0].InnerText.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace("cdn", "").Replace(":", "").Trim();
                                                                }
                                                                HtmlNodeCollection _skuatt = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");
                                                                if (_skuatt != null)
                                                                {
                                                                    _Row[1] = _skuatt[0].InnerText.ToLower().Replace("product code:", "").Trim();
                                                                }
                                                                if (Options.Values.ElementAt(0) == "color")
                                                                {
                                                                    _Row[2] = option.Text.Trim();
                                                                    _Row[3] = option1.Text.Trim();
                                                                }
                                                                else
                                                                {
                                                                    _Row[3] = option.Text.Trim();
                                                                    _Row[2] = option1.Text.Trim();
                                                                }
                                                                _Row[4] = "5";
                                                                _TableWork1.Rows.Add(_Row);
                                                            }
                                                            Counter1++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DataRow _Row = _TableWork1.NewRow();
                                                        HtmlNodeCollection _PriceAtt = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]");
                                                        if (_PriceAtt != null)
                                                        {
                                                            _Row[0] = _PriceAtt[0].InnerText.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace("cdn", "").Replace(":", "").Trim();
                                                        }
                                                        HtmlNodeCollection _skuatt = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");
                                                        if (_skuatt != null)
                                                        {
                                                            _Row[1] = _skuatt[0].InnerText.ToLower().Replace("product code:", "").Trim();
                                                        }
                                                        if (Options.Values.ElementAt(0) == "color")
                                                            _Row[2] = option.Text.Trim();
                                                        else
                                                            _Row[3] = option.Text.Trim();
                                                        _Row[4] = "5";
                                                        _TableWork1.Rows.Add(_Row);
                                                    }
                                                    Counter++;

                                                    foreach (DataRow Dr in _TableWork1.Rows)
                                                    {
                                                        if (Dr[1].ToString() + "prnt" != ParentSku)
                                                        {
                                                            Crawler_WithouSizes_Part7.BusinessLayer.Product PrdCheck = null;
                                                            try
                                                            {
                                                                PrdCheck = Worker1Products.Find(M => M.SKU == "SGC" + Dr[1].ToString());
                                                            }
                                                            catch
                                                            {
                                                            }
                                                            if (PrdCheck == null)
                                                            {
                                                                Variantcounter++;
                                                                Crawler_WithouSizes_Part7.BusinessLayer.Product Prd = new Crawler_WithouSizes_Part7.BusinessLayer.Product();
                                                                Prd.Brand = Brand;
                                                                Prd.Category = BrandName1;
                                                                Prd.Manufacturer = Brand;
                                                                Prd.Currency = "CAD";
                                                                if (_Description1.Trim() != "")
                                                                    Prd.Description = _Description1;
                                                                else
                                                                    Prd.Description = Title;
                                                                Prd.URL = Url1;
                                                                int BulletPointCounter = 0;
                                                                foreach (var Points in LstBulletPoints)
                                                                {
                                                                    BulletPointCounter++;
                                                                    switch (BulletPointCounter)
                                                                    {
                                                                        case 1:
                                                                            Prd.Bulletpoints1 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                        case 2:
                                                                            Prd.Bulletpoints2 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                        case 3:
                                                                            Prd.Bulletpoints3 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                        case 4:
                                                                            Prd.Bulletpoints4 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                        case 5:
                                                                            Prd.Bulletpoints5 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                    }


                                                                }
                                                                Prd.Name = Title;
                                                                if (Variantcounter == 1)
                                                                    Prd.Isparent = true;
                                                                Prd.Size = Dr[3].ToString();
                                                                Prd.Color = Dr[2].ToString();
                                                                Prd.SKU = "SGC" + Dr[1].ToString();
                                                                Prd.Stock = Dr[4].ToString();
                                                                Prd.Price = Dr[0].ToString();
                                                                if (ParentSku.Length + 3 > 30)
                                                                    Prd.parentsku = "SGC" + ParentSku.Substring(0, 27);
                                                                else
                                                                    Prd.parentsku = "SGC" + ParentSku;

                                                                Prd.Weight = "0";
                                                                try
                                                                {
                                                                    var _Images = (from Img in _ThumbImages
                                                                                   where Dr[2].ToString().ToLower().Contains(Img.Value.Trim().ToLower())
                                                                                   select Img.Key).ToArray();
                                                                    if (_Images == null || _Images.Count() == 0)
                                                                    {
                                                                        var _Imagessize = (from Img in _ThumbImages
                                                                                           where Dr[3].ToString().ToLower().Contains(Img.Value.Trim().ToLower())
                                                                                           select Img.Key).ToArray();
                                                                        if (_Imagessize == null || _Imagessize.Count() == 0)
                                                                            Prd.Image = Images;

                                                                        else
                                                                        {
                                                                            string ColorImages = _Imagessize[0].ToString() + "@" + Images.Replace(_Imagessize[0].ToString(), "");
                                                                            Prd.Image = ColorImages.Replace("@@", "@");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        string ColorImages = _Images[0].ToString() + "@" + Images.Replace(_Images[0].ToString(), "");
                                                                        Prd.Image = ColorImages.Replace("@@", "@");
                                                                    }
                                                                }
                                                                catch
                                                                {
                                                                    Prd.Image = Images;
                                                                }
                                                                Worker1Products.Add(Prd);
                                                            }
                                                            else
                                                            {
                                                                _writer.WriteLine(Url1 + "Duplicacy issue in sku.");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _writer.WriteLine(Url1 + "Url with More Options.");
                                        }
                                    }

                                }
                                catch
                                {
                                    _writer.WriteLine(Url1 + "error occured in code to process this link");
                                }
                            }
                            catch
                            {
                                _writer.WriteLine(Url1 + "error occured in code to process this link");
                            }
                        }
                        else
                        {
                            _writer.WriteLine(Url1 + "Url data is not loaded.");
                        }

                    }
                    else
                    {
                        _writer.WriteLine(Url1 + "Url data is not loaded.");
                    }

                }
                else
                {

                }

            }
            else
            {
            }
            #endregion scubagearcanada
        }



        public void work_RunWorkerAsync1(object sender, RunWorkerCompletedEventArgs e)
        {
            #region scubagearcanada
            if (_ISscubagearcanada)
            {
                if (_IsProduct)
                {
                    if (!Erorr_scubagearcanada2)
                    {
                        _Worker2.WaitForComplete();
                        #region CheckPageLoaded

                        #region variable
                        int checkcounter = 0;
                        #endregion variable

                        if (_Worker2.Html == null)
                        {
                            do
                            {
                                System.Threading.Thread.Sleep(10);
                                Application.DoEvents();
                                checkcounter++;
                            } while (_Worker2.Html == null && checkcounter < 10);
                        }

                        #endregion CheckPageLoaded
                        if (_Worker2.Html != null)
                        {
                            _Work1doc2.LoadHtml(_Worker2.Html);
                            try
                            {

                                try
                                {

                                    #region Title
                                    string Title = "";
                                    HtmlNodeCollection _Title = null;
                                    _Title = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"ProductDetailsGrid desktop PriceBorderBottom\"]/div[@class=\"DetailRow\"]/h1");
                                    if (_Title == null)
                                        _Title = _Work1doc2.DocumentNode.SelectNodes("//meta=[@property=\"og:title\"]");
                                    if (_Title != null)
                                    {
                                        Title = System.Net.WebUtility.HtmlDecode(CommanFunction.Removeunsuaalcharcterfromstring(_Title[0].InnerText.Trim())).Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("Online Only", "").Replace("online only", "").Replace("â„¢", "™");
                                    }

                                    #endregion Title

                                    #region Description

                                    _Description2 = "";
                                    HtmlNodeCollection _description = _Work1doc2.DocumentNode.SelectNodes("//div[@itemprop=\"description\"]");
                                    if (_description != null)
                                    {
                                        _Description2 = _description[0].InnerHtml.Replace("Quick Overview", "").Trim();
                                        _Description2 = CommanFunction.Removeunsuaalcharcterfromstring(CommanFunction.StripHTML(_Description2).Trim());

                                    }
                                    try
                                    {
                                        if (_Description2.Length > 2000)
                                            _Description2 = _Description2.Substring(0, 1997) + "...";
                                    }
                                    catch
                                    {
                                    }

                                    string Desc = System.Net.WebUtility.HtmlDecode(_Description2.Replace("Â", "").Replace(">", "").Replace("<", "").Replace("- Online Only", "").Replace("- online only", "").Replace("online only", "").Replace("Online Only", "")).Replace(",", " ");
                                    if (Desc.Trim() != "")
                                    {
                                        if (Desc.Substring(0, 1) == "\"")
                                            _Description2 = Desc.Substring(1);
                                        else
                                            _Description2 = Desc;
                                    }

                                    #endregion Description

                                    #region BulletPoints
                                    string BulletPoints = "";
                                    List<string> LstBulletPoints = new List<string>();
                                    HtmlNodeCollection _Bullets1 = null;
                                    _Bullets1 = _Work1doc2.DocumentNode.SelectNodes("//div[@itemprop=\"description\"]");

                                    if (_Bullets1 != null)
                                    {

                                        foreach (HtmlNode _BullNode in _Bullets1)
                                        {
                                            BulletPoints = BulletPoints + System.Net.WebUtility.HtmlDecode(CommanFunction.StripHTML(_BullNode.InnerText).Trim()) + ".";
                                        }

                                    }
                                    if (BulletPoints.Trim() != "")
                                    {
                                        if (BulletPoints.Length >= 500)
                                            LstBulletPoints.Add(BulletPoints.Substring(0, 497).Replace("â„¢", "™"));
                                        else
                                            LstBulletPoints.Add(BulletPoints.Replace("â„¢", "™"));
                                    }
                                    #endregion BulletPoints

                                    #region Brand

                                    string Brand = "";
                                    HtmlNodeCollection _Brand = null;
                                    _Brand = _Work1doc2.DocumentNode.SelectNodes("//h4[@class=\"BrandName\"]/a");
                                    if (_Brand == null)
                                        _Brand = _Work1doc2.DocumentNode.SelectNodes("//h4[@class=\"BrandName\"]/a/span");
                                    if (_Brand != null)
                                    {
                                        Brand = _Brand[0].InnerText.Trim();
                                    }

                                    if (Brand.Trim() == "")
                                        Brand = "SCUBA";
                                    #endregion Brand

                                    #region Images

                                    string Images = "";

                                    HtmlNodeCollection _Image = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"ProductThumbImage\"]/a");
                                    if (_Image != null)
                                    {
                                        foreach (HtmlAttribute _Att in _Image[0].Attributes)
                                        {
                                            if (_Att.Name == "href")
                                                Images = _Att.Value.Trim() + "@";
                                        }
                                    }
                                    Dictionary<string, string> _ThumbImages = new Dictionary<string, string>();
                                    string ImageUrl = "";
                                    string AltText = "";
                                    HtmlNodeCollection _ThumImage1 = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"ProductTinyImageList\"]");
                                    if (_ThumImage1 != null)
                                    {
                                        HtmlNodeCollection _ThumImage = _ThumImage1[0].SelectNodes(".//a");
                                        foreach (HtmlNode ThumNode in _ThumImage)
                                        {
                                            AltText = "";
                                            ImageUrl = "";
                                            foreach (HtmlAttribute _Att in ThumNode.Attributes)
                                            {
                                                if (_Att.Name.ToLower() == "rel")
                                                {
                                                    string LargeImage = _Att.Value;
                                                    try
                                                    {
                                                        LargeImage = LargeImage.Substring(LargeImage.IndexOf("\"largeimage\": \"")).Replace("\"largeimage\": \"", "");
                                                        LargeImage = LargeImage.Substring(0, LargeImage.IndexOf("\""));
                                                    }
                                                    catch
                                                    {
                                                        LargeImage = LargeImage.Substring(LargeImage.IndexOf("\"smallimage\": \"")).Replace("\"smallimage\": \"", "");
                                                        LargeImage = LargeImage.Substring(0, LargeImage.IndexOf("\""));
                                                    }
                                                    finally
                                                    {
                                                    }
                                                    if (!Images.Contains(LargeImage))
                                                    {
                                                        ImageUrl = LargeImage;
                                                        HtmlNodeCollection _CollectionImgalt = ThumNode.SelectNodes(".//img");
                                                        if (_CollectionImgalt != null)
                                                        {
                                                            foreach (HtmlAttribute _Attimg in _CollectionImgalt[0].Attributes)
                                                            {
                                                                if (_Attimg.Name.ToLower() == "alt")
                                                                    AltText = _Attimg.Value.ToLower().Trim();
                                                            }
                                                        }
                                                        Images = Images + LargeImage.Trim() + "@";
                                                        _ThumbImages.Add(ImageUrl, AltText);
                                                    }
                                                }

                                            }
                                        }
                                    }


                                    if (Images.Length > 0)
                                        Images = Images.Substring(0, Images.Length - 1);
                                    #endregion Images

                                    int VariantsCounter = 0;
                                    string ParentSku = "";
                                    VariantsCounter++;
                                    string Price = "";
                                    string Stock = "";
                                    string Sku = "";

                                    #region Price
                                    HtmlNodeCollection _Price = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]");
                                    if (_Price != null)
                                    {
                                        Price = _Price[0].InnerText.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace("cdn", "").Replace(":", "").Trim();
                                    }
                                    #endregion price

                                    #region stock
                                    Stock = "5";
                                    HtmlNodeCollection _Stock1 = _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"content\"]");
                                    if (_Stock1 != null)
                                    {

                                        HtmlNodeCollection _Stock = _Stock1[0].SelectNodes(".//ul[@class=\"list-unstyled\"]");
                                        if (_Stock != null)
                                        {
                                            if (_Stock[0].InnerHtml.ToLower().Contains("out of stock") || _Stock[0].InnerHtml.ToLower().Contains("out stock"))
                                                Stock = "0";
                                        }
                                    }

                                    #endregion stock

                                    #region sku
                                    HtmlNodeCollection _sku = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");

                                    if (_sku != null)
                                    {
                                        Sku = _sku[0].InnerText.ToLower().Replace("product code:", "").Trim();
                                        ParentSku = Sku;

                                    }

                                    if (ParentSku == "")
                                    {
                                        ParentSku = CommanFunction.GeneratecolorSku("", Title);
                                        Sku = ParentSku;
                                    }
                                    ParentSku = ParentSku + "prnt";

                                    #endregion sku


                                    if (Skus.Contains(Sku))
                                        return;
                                    else
                                        Skus.Add(Sku);
                                    HtmlNodeCollection _Coll = null;
                                    _Coll = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"productAddToCartRight\"]");
                                    if (_Coll != null)
                                        _Coll = _Coll[0].SelectNodes(".//select");
                                    string ID = "";
                                    if (_Coll != null)
                                    {
                                        if (_Coll.Count == 1)
                                        {
                                            if (_Coll[0].Id == "qty_")
                                                ID = "qty_";

                                        }
                                    }
                                    if (_Coll == null || (ID.Length > 0))
                                    {

                                        Crawler_WithouSizes_Part7.BusinessLayer.Product Prd = new Crawler_WithouSizes_Part7.BusinessLayer.Product();
                                        Prd.Brand = Brand;
                                        Prd.Category = BrandName1;
                                        Prd.Manufacturer = Brand;
                                        Prd.Currency = "CAD";
                                        if (_Description2.Trim() != "")
                                            Prd.Description = _Description2;
                                        else
                                            Prd.Description = Title;
                                        Prd.URL = Url2;
                                        int BulletPointCounter = 0;
                                        foreach (var Points in LstBulletPoints)
                                        {
                                            BulletPointCounter++;
                                            switch (BulletPointCounter)
                                            {
                                                case 1:
                                                    Prd.Bulletpoints1 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                                case 2:
                                                    Prd.Bulletpoints2 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                                case 3:
                                                    Prd.Bulletpoints3 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                                case 4:
                                                    Prd.Bulletpoints4 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                                case 5:
                                                    Prd.Bulletpoints5 = Points.Replace("..", "").Replace("â€", "\"");
                                                    break;
                                            }


                                        }

                                        Prd.Isparent = true;
                                        if (Sku.Length + 3 > 30)
                                            Prd.SKU = "SGC" + Sku.Substring(0, 27);
                                        else
                                            Prd.SKU = "SGC" + Sku;
                                        Prd.Stock = Stock;
                                        Prd.Price = Price;
                                        if (ParentSku.Length + 3 > 30)
                                            Prd.parentsku = "SGC" + ParentSku.Substring(0, 27);
                                        else
                                            Prd.parentsku = "SGC" + ParentSku;

                                        Prd.Weight = "0";
                                        Prd.Name = Title;
                                        Prd.Image = Images;
                                        Worker2Products.Add(Prd);

                                    }
                                    else
                                    {
                                        bool Kit = false;
                                        Dictionary<string, string> Options = new Dictionary<string, string>();
                                        foreach (HtmlNode _Node in _Coll)
                                        {
                                            foreach (HtmlAttribute _Att in _Node.Attributes)
                                            {
                                                if (_Att.Name.ToLower() == "id")
                                                {
                                                    if (_Att.Value.ToLower() != "qty_")
                                                    {
                                                        HtmlNodeCollection _LblColllection = _Work1doc2.DocumentNode.SelectNodes("//label[@for=\"" + _Att.Value + "\"]");
                                                        if (_LblColllection != null)
                                                        {
                                                            if (_LblColllection[0].InnerText.Trim().ToLower().Contains("size"))
                                                                Options.Add(_Att.Value, "size");
                                                            else if (_LblColllection[0].InnerText.Trim().ToLower().Contains("color") || _LblColllection[0].InnerText.Trim().ToLower().Contains("colour"))
                                                                Options.Add(_Att.Value, "color");
                                                            else
                                                            {
                                                                Kit = true;
                                                                Options.Add(_Att.Value, _LblColllection[0].InnerText.Trim().ToLower().Replace(":", "").Replace("*", ""));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Kit = true;
                                                            Options.Add(_Att.Value, "");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (Options.Count > 0 && !Kit)
                                        {
                                            string CheckSkuDuplicacy = "";
                                            bool DuplicacayExist = true;
                                            int CheckCounter = 0;
                                            int Variantcounter = 0;
                                            SelectList _List = _Worker2.SelectList(Find.ById(Options.Keys.ElementAt(0)));
                                            int Counter = 0;
                                            foreach (Option option in _List.Options)
                                            {
                                                if (!option.Text.Trim().ToLower().Contains("please "))
                                                {
                                                    DuplicacayExist = true;
                                                    CheckCounter = 0;
                                                    _TableWork2.Rows.Clear();
                                                    _Worker2.SelectList(Find.ById(Options.Keys.ElementAt(0))).Option(option.Text).Select();
                                                    _Worker2.SelectList(Find.ById(Options.Keys.ElementAt(0))).Option(option.Text).Click();
                                                    _Worker2.WaitForComplete();
                                                    System.Threading.Thread.Sleep(2000);
                                                    if (Options.Count == 1)
                                                    {
                                                        _Work1doc2.LoadHtml(_Worker2.Html);
                                                        while (DuplicacayExist && CheckCounter < 10)
                                                        {
                                                            HtmlNodeCollection _skuatt1 = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");
                                                            if (_skuatt1 != null)
                                                            {
                                                                if ((CheckSkuDuplicacy == _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim()) || _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim() == "")
                                                                {
                                                                    Application.DoEvents();
                                                                    _Work1doc2.LoadHtml(_Worker2.Html);
                                                                }
                                                                else
                                                                {
                                                                    DuplicacayExist = false;
                                                                    CheckSkuDuplicacy = _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim();
                                                                }
                                                                CheckCounter++;
                                                            }
                                                        }
                                                        if (DuplicacayExist)
                                                            return;
                                                    }

                                                    #region CheckPageLoading

                                                    #endregion

                                                    if (Options.Count > 1)
                                                    {

                                                        SelectList _List1 = _Worker2.SelectList(Find.ById(Options.Keys.ElementAt(1)));
                                                        int Counter1 = 0;
                                                        foreach (var option1 in _List1.Options)
                                                        {
                                                            if (!option1.Text.ToLower().Contains("please "))
                                                            {
                                                                _Worker2.SelectList(Find.ById(Options.Keys.ElementAt(1))).Option(option1.Text).Select();
                                                                _Worker2.SelectList(Find.ById(Options.Keys.ElementAt(1))).Option(option1.Text).Click();
                                                                System.Threading.Thread.Sleep(2000);
                                                                _Work1doc2.LoadHtml(_Worker2.Html);
                                                                while (DuplicacayExist && CheckCounter < 10)
                                                                {
                                                                    HtmlNodeCollection _skuatt1 = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");
                                                                    if (_skuatt1 != null)
                                                                    {
                                                                        if ((CheckSkuDuplicacy == _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim()) || _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim() == "")
                                                                        {
                                                                            Application.DoEvents();
                                                                            _Work1doc2.LoadHtml(_Worker2.Html);
                                                                        }
                                                                        else
                                                                        {
                                                                            DuplicacayExist = false;
                                                                            CheckSkuDuplicacy = _skuatt1[0].InnerText.ToLower().Replace("product code:", "").Trim();
                                                                        }
                                                                        CheckCounter++;
                                                                    }
                                                                }
                                                                if (DuplicacayExist)
                                                                    return;

                                                                option1.SelectNoWait();
                                                                DataRow _Row = _TableWork2.NewRow();
                                                                HtmlNodeCollection _PriceAtt = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]");
                                                                if (_PriceAtt != null)
                                                                {
                                                                    _Row[0] = _PriceAtt[0].InnerText.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace("cdn", "").Replace(":", "").Trim();
                                                                }
                                                                HtmlNodeCollection _skuatt = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");
                                                                if (_skuatt != null)
                                                                {
                                                                    _Row[1] = _skuatt[0].InnerText.ToLower().Replace("product code:", "").Trim();

                                                                }
                                                                if (Options.Values.ElementAt(0) == "color")
                                                                {
                                                                    _Row[2] = option.Text.Trim();
                                                                    _Row[3] = option1.Text.Trim();
                                                                }
                                                                else
                                                                {
                                                                    _Row[3] = option.Text.Trim();
                                                                    _Row[2] = option1.Text.Trim();
                                                                }
                                                                _Row[4] = "5";
                                                                _TableWork2.Rows.Add(_Row);
                                                            }
                                                            Counter1++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DataRow _Row = _TableWork2.NewRow();
                                                        HtmlNodeCollection _PriceAtt = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]");
                                                        if (_PriceAtt != null)
                                                        {
                                                            _Row[0] = _PriceAtt[0].InnerText.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace("cdn", "").Replace(":", "").Trim();
                                                        }
                                                        HtmlNodeCollection _skuatt = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"sku\"]");
                                                        if (_skuatt != null)
                                                        {
                                                            _Row[1] = _skuatt[0].InnerText.ToLower().Replace("product code:", "").Trim();

                                                        }
                                                        if (Options.Values.ElementAt(0) == "color")
                                                            _Row[2] = option.Text.Trim();
                                                        else
                                                            _Row[3] = option.Text.Trim();
                                                        _Row[4] = "5";
                                                        _TableWork2.Rows.Add(_Row);
                                                    }
                                                    Counter++;

                                                    foreach (DataRow Dr in _TableWork2.Rows)
                                                    {
                                                        if (Dr[1].ToString() + "prnt" != ParentSku)
                                                        {
                                                            Crawler_WithouSizes_Part7.BusinessLayer.Product PrdCheck = null;
                                                            try
                                                            {
                                                                PrdCheck = Worker2Products.Find(M => M.SKU == "SGC" + Dr[1].ToString());
                                                            }
                                                            catch
                                                            {
                                                            }
                                                            if (PrdCheck == null)
                                                            {
                                                                Variantcounter++;
                                                                Crawler_WithouSizes_Part7.BusinessLayer.Product Prd = new Crawler_WithouSizes_Part7.BusinessLayer.Product();
                                                                Prd.Brand = Brand;
                                                                Prd.Category = BrandName1;
                                                                Prd.Manufacturer = Brand;
                                                                Prd.Currency = "CAD";
                                                                if (_Description2.Trim() != "")
                                                                    Prd.Description = _Description2;
                                                                else
                                                                    Prd.Description = Title;
                                                                Prd.URL = Url2;
                                                                int BulletPointCounter = 0;
                                                                foreach (var Points in LstBulletPoints)
                                                                {
                                                                    BulletPointCounter++;
                                                                    switch (BulletPointCounter)
                                                                    {
                                                                        case 1:
                                                                            Prd.Bulletpoints1 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                        case 2:
                                                                            Prd.Bulletpoints2 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                        case 3:
                                                                            Prd.Bulletpoints3 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                        case 4:
                                                                            Prd.Bulletpoints4 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                        case 5:
                                                                            Prd.Bulletpoints5 = Points.Replace("..", "").Replace("â€", "\"");
                                                                            break;
                                                                    }


                                                                }
                                                                Prd.Name = Title;
                                                                if (Variantcounter == 1)
                                                                    Prd.Isparent = true;
                                                                Prd.Size = Dr[3].ToString();
                                                                Prd.Color = Dr[2].ToString();
                                                                Prd.SKU = "SGC" + Dr[1].ToString();
                                                                Prd.Stock = Dr[4].ToString();
                                                                Prd.Price = Dr[0].ToString();
                                                                if (ParentSku.Length + 3 > 30)
                                                                    Prd.parentsku = "SGC" + ParentSku.Substring(0, 27);
                                                                else
                                                                    Prd.parentsku = "SGC" + ParentSku;

                                                                Prd.Weight = "0";
                                                                try
                                                                {
                                                                    var _Images = (from Img in _ThumbImages
                                                                                   where Dr[2].ToString().ToLower().Contains(Img.Value.Trim().ToLower())
                                                                                   select Img.Key).ToArray();
                                                                    if (_Images == null || _Images.Count() == 0)
                                                                    {
                                                                        var _Imagessize = (from Img in _ThumbImages
                                                                                           where Dr[3].ToString().ToLower().Contains(Img.Value.Trim().ToLower())
                                                                                           select Img.Key).ToArray();
                                                                        if (_Imagessize == null || _Imagessize.Count() == 0)
                                                                            Prd.Image = Images;

                                                                        else
                                                                        {
                                                                            string ColorImages = _Imagessize[0].ToString() + "@" + Images.Replace(_Imagessize[0].ToString(), "");
                                                                            Prd.Image = ColorImages.Replace("@@", "@");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        string ColorImages = _Images[0].ToString() + "@" + Images.Replace(_Images[0].ToString(), "");
                                                                        Prd.Image = ColorImages.Replace("@@", "@");
                                                                    }
                                                                }
                                                                catch
                                                                {
                                                                    Prd.Image = Images;
                                                                }
                                                                Worker2Products.Add(Prd);
                                                            }
                                                            else
                                                            {
                                                                _writer.WriteLine(Url2 + "Duplicacy issue in sku.");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _writer.WriteLine(Url2 + "Url with More Options.");
                                        }
                                    }

                                }
                                catch
                                {
                                    _writer.WriteLine(Url2 + "error occured in code to process this link");
                                }
                            }
                            catch
                            {
                                _writer.WriteLine(Url2 + "error occured in code to process this link");
                            }
                        }
                        else
                        {
                            _writer.WriteLine(Url2 + "Url data is not loaded.");
                        }

                    }
                    else
                    {
                        _writer.WriteLine(Url2 + "Url data is not loaded.");
                    }

                }
                else
                {

                }

            }
            else
            {
            }
            #endregion scubagearcanada
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
            dataGridView1.Columns.Add("RowID", "RowID");
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
            dataGridView1.Columns.Add("Size", "Size");
            dataGridView1.Columns.Add("Color", "Color");
            dataGridView1.Columns.Add("Isdefault", "Isdefault");
            dataGridView1.Columns.Add("ParentSku", "ParentSku");
            dataGridView1.Columns.Add("BullPoint1", "BullPoint1");
            dataGridView1.Columns.Add("BullPoint2", "BullPoint2");
            dataGridView1.Columns.Add("BullPoint3", "BullPoint3");
            dataGridView1.Columns.Add("BullPoint4", "BullPoint4");
            dataGridView1.Columns.Add("BullPoint5", "BullPoint5");
            dataGridView1.Columns.Add("Category", "Category");
            dataGridView1.Columns.Add("Weight", "Weight");


            /****************BackGround worker *************************/

        }


        public void GenerateCSVFile()
        {

            try
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
                exceldt.Columns.Add("Price", typeof(decimal));
                exceldt.Columns.Add("Currency", typeof(string));
                exceldt.Columns.Add("In Stock", typeof(string));
                exceldt.Columns.Add("Image URL", typeof(string));
                exceldt.Columns.Add("URL", typeof(string));
                exceldt.Columns.Add("Size", typeof(string));
                exceldt.Columns.Add("Color", typeof(string));
                exceldt.Columns.Add("Isdefault", typeof(bool));
                exceldt.Columns.Add("ParentSku", typeof(string));
                exceldt.Columns.Add("Bulletpoints1", typeof(string));
                exceldt.Columns.Add("Bulletpoints2", typeof(string));
                exceldt.Columns.Add("Bulletpoints3", typeof(string));
                exceldt.Columns.Add("Bulletpoints4", typeof(string));
                exceldt.Columns.Add("Bulletpoints5", typeof(string));
                exceldt.Columns.Add("Category", typeof(string));
                exceldt.Columns.Add("Weight", typeof(string));


                for (int m = 0; m < dataGridView1.Rows.Count; m++)
                {
                    exceldt.Rows.Add();

                    try
                    {
                        for (int n = 0; n < dataGridView1.Columns.Count; n++)
                        {
                            if (dataGridView1.Rows[m].Cells[n].Value == null || dataGridView1.Rows[m].Cells[n].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[m].Cells[n].Value.ToString()))
                                continue;

                            exceldt.Rows[m][n] = dataGridView1.Rows[m].Cells[n].Value.ToString();


                        }
                    }
                    catch
                    {

                    }
                }


                #region sqlcode
                DataSet _Ds = new DataSet();
                DataTable _Product = new DataTable();
                using (SqlCommand Cmd = new SqlCommand())
                {
                    try
                    {
                        if (Connection.State == ConnectionState.Closed)
                            Connection.Open();
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.CommandTimeout = 0;
                        Cmd.Connection = Connection;
                        Cmd.CommandText = "MarkHub_ScrapeProductMerging";
                        Cmd.Parameters.AddWithValue("@StoreName", "scubagearcanada.ca");
                        Cmd.Parameters.AddWithValue("@Products", exceldt);
                        SqlDataAdapter _ADP = new SqlDataAdapter(Cmd);
                        _ADP.Fill(_Ds);
                    }
                    catch
                    {

                    }
                }
                _Product = _Ds.Tables[0];

                #endregion sqlcode


                if (_Product.Rows.Count > 0)
                {

                    try
                    {

                        using (CsvFileWriter writer = new CsvFileWriter(Application.StartupPath + "/" + Filename + ".txt"))
                        {
                            CsvFileWriter.CsvRow row = new CsvFileWriter.CsvRow();//HEADER FOR CSV FILE

                            foreach (DataColumn _Dc in _Product.Columns)
                            {
                                row.Add(_Dc.ColumnName);
                            }

                            row.Add("Image URL2");
                            row.Add("Image URL3");
                            row.Add("Image URL4");
                            row.Add("Image URL5");
                            writer.WriteRow(row);//INSERT TO CSV FILE HEADER


                            for (int m = 0; m < _Product.Rows.Count; m++)
                            {
                                CsvFileWriter.CsvRow row1 = new CsvFileWriter.CsvRow();

                                for (int n = 0; n < _Product.Columns.Count; n++)
                                {

                                    if (n == _Product.Columns.Count - 1)
                                    {
                                        if (_Product.Rows[m][n] != null)
                                        {
                                            string[] ImageArray = _Product.Rows[m][n].ToString().Split('@');
                                            for (int count = 0; count < ImageArray.Length; count++)
                                            {
                                                if (count < 5)
                                                    row1.Add(String.Format("{0}", ImageArray[count].ToString()));
                                            }
                                        }
                                        else
                                            row1.Add(String.Format("{0}", ""));
                                    }
                                    else
                                    {
                                        if (_Product.Rows[m][n] != null)
                                            row1.Add(String.Format("{0}", _Product.Rows[m][n].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", "")));
                                        else
                                            row1.Add(String.Format("{0}", ""));
                                    }
                                }
                                writer.WriteRow(row1);
                            }
                        }
                        System.Diagnostics.Process.Start(Application.StartupPath + "/" + Filename + ".txt");//OPEN THE CSV FILE ,,CSV FILE NAMED AS DATA.CSV
                    }
                    catch (Exception) { MessageBox.Show("file is already open\nclose the file"); }
                    return;

                }

                else
                {
                    _lblerror.Visible = true;
                    _lblerror.Text = "OOPS there is some iossue occured. Please contact developer as soon as possible";
                }
            }
            catch
            {
                _lblerror.Visible = true;
                _lblerror.Text = "OOPS there is some iossue occured. Please contact developer as soon as possible";
            }
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

        private void chkstorelist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
