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
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using bestbuy;
using System.Text.RegularExpressions;
using System.Net;
using WatiN.Core;
namespace palyerborndate
{
    public partial class Form1 : System.Windows.Forms.Form
    {

        #region DatbaseVariable
        SqlConnection Connection = new SqlConnection(System.Configuration.ConfigurationSettings.
                                               AppSettings["connectionstring"]);
        #endregion DatbaseVariable
        #region booltypevariable

        bool _IsProduct = false;
        bool _IsCategory = true;
        bool _Issubcat = false;
        bool _Stop = false;
        bool _Iscompleted = false;

        #endregion booltypevariable
        #region Buinesslayervariable
        List<BusinessLayer.Product> Products = new List<BusinessLayer.Product>();
        BusinessLayer.Mail _Mail = new BusinessLayer.Mail();
        BusinessLayer.ProductMerge _Prd = new BusinessLayer.ProductMerge();
        #endregion Buinesslayervariable
        #region intypevariable
        int _Pages = 0;
        int _TotalRecords = 0;
        int gridindex = 0;
        int time = 0;

        #endregion intypevariable


        #region IeVariable

        IE _Worker1 = null;
        IE _Worker2 = null;

        #endregion IeVariable
        #region stringtypevariable

        string Url1 = "";
        string Url2 = "";
        string _ScrapeUrl = "";
        string Category = "";
        decimal Weight = 0;
        #endregion listtypevariable
        #region listtypevariable

        List<string> _Url = new List<string>();
        List<string> _ProductUrl = new List<string>();
        List<string> _Name = new List<string>();
        List<string> CategoryUrl = new List<string>();
        Dictionary<string, string> Producturl = new Dictionary<string, string>();

        #endregion stringtypevariable
        #region backgroundworker

        BackgroundWorker _Work = new BackgroundWorker();
        BackgroundWorker _Work1 = new BackgroundWorker();


        #endregion backgroundworker
        #region webclient

        ExtendedWebClient _Client2 = new ExtendedWebClient();
        ExtendedWebClient _Client1 = new ExtendedWebClient();
        ExtendedWebClient _Client3 = new ExtendedWebClient();
        ExtendedWebClient _Client4 = new ExtendedWebClient();

        #endregion webclient
        #region htmlagility

        HtmlAgilityPack.HtmlDocument _Work1doc = new HtmlAgilityPack.HtmlDocument();
        HtmlAgilityPack.HtmlDocument _Work1doc2 = new HtmlAgilityPack.HtmlDocument();
        HtmlAgilityPack.HtmlDocument _Work1doc3 = new HtmlAgilityPack.HtmlDocument();
        HtmlAgilityPack.HtmlDocument _Work1doc4 = new HtmlAgilityPack.HtmlDocument();


        StreamWriter writer = new StreamWriter(Application.StartupPath + "/log.txt");
        #endregion htmlagility

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
        private void Form1_Load(object sender, EventArgs e)
        {

            /********************End*************************************/
            /***************Grid view************************************/
            totalrecord.Visible = false;
            _lblerror.Visible = false;
            _percent.Visible = false;

            /****************BackGround worker *************************/
        }
        public void tim(int t)
        {
            time = 0;
            timer1.Start();
            try
            {
                while (time <= t)
                {

                    Application.DoEvents();
                }
            }
            catch (Exception) { }
            timer1.Stop();

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
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
        public void GetCategoryInfo(HtmlAgilityPack.HtmlDocument _doc, string url)
        {
            HtmlNodeCollection Collection = _doc.DocumentNode.SelectNodes("//div[@class=\"nxt-product-details\"]//div[@class=\"nxt-product-name\"]//a");

            if (Collection != null)
            {
                foreach (HtmlNode node in Collection)
                {

                    foreach (HtmlAttribute attr in node.Attributes)
                    {
                        if (attr.Name == "href")
                        {
                            if (!Producturl.Keys.Contains(attr.Value.ToLower()))
                            {
                                try
                                {
                                    Producturl.Add(attr.Value.ToLower(), "");
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
                WriteLogEvent(url, "ctl00_CC_ProductSearchResultListing_SearchProductListing tag is not found");
        }
        public void GetProductInfo(HtmlAgilityPack.HtmlDocument _doc, string url)
        {
            BusinessLayer.Product product = new BusinessLayer.Product();
            try
            {
                #region title
                if (_doc.DocumentNode.SelectNodes("//h1[@class=\"productDescLarge\"]") != null)
                    product.Name = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//h1[@class=\"productDescLarge\"]")[0].InnerText.Trim());
                else
                    WriteLogEvent(url, "title not found");
                #endregion title
                #region price
                if (_doc.DocumentNode.SelectNodes("//span[@class=\"productPriceLarge\"]") != null)
                    product.Price = _doc.DocumentNode.SelectNodes("//span[@class=\"productPriceLarge\"]")[0].InnerText.Replace("$", "").Trim();
                else
                {
                    product.Price = "0";
                    WriteLogEvent(url, "Price not found");
                }
                #endregion price
                #region Brand

                try
                {
                    if (product.Name.Length > 0)
                    {
                        product.Brand = product.Name.Substring(0, product.Name.IndexOf(' '));
                        product.Manufacturer = product.Name.Substring(0, product.Name.IndexOf(' '));
                        product.Brand = "JZ HOLDINGS";
                        product.Manufacturer = "JZ HOLDINGS";
                    }
                    else
                    {
                        product.Brand = "JZ HOLDINGS";
                        product.Manufacturer = "JZ HOLDINGS";
                    }
                }
                catch
                {
                    product.Brand = "JZ HOLDINGS";
                    product.Manufacturer = "JZ HOLDINGS";
                }
                #endregion Brand
                #region Category
                if (_doc.DocumentNode.SelectNodes("//a[@id=\"ctl00_MainBodyContent_lnkMoreCatetory\"]") != null)
                {
                    try
                    {
                        string Category = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//a[@id=\"ctl00_MainBodyContent_lnkMoreCatetory\"]")[0].InnerText);
                        if (Category.Contains("->"))
                            Category = Category.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        product.Category ="FDCA"+ Category.Trim();
                    }
                    catch
                    { WriteLogEvent(url, "Category not found"); }
                }
                else
                    WriteLogEvent(url, "Category not found");
                #endregion Category
                product.Currency = "CAD";
                #region description
                string Description = "";
                var allElementsWithClassFloat = _doc.DocumentNode.SelectNodes("//span[@id=\"ctl00_MainBodyContent_lblSpecification\"]");
                if (allElementsWithClassFloat != null)
                {
                    foreach (HtmlNode node in allElementsWithClassFloat)
                    {
                        Description = Description + " " + node.InnerText.Trim();
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
                    product.Description = System.Net.WebUtility.HtmlDecode(Description.Replace("Â", "")).Replace("factorydirect.ca", "");
                }
                else
                    WriteLogEvent(url, "Description not found");
                #endregion description

                #region BulletPoints
                string Feature = "";
                string Bullets = "";

                HtmlNodeCollection collection = _doc.DocumentNode.SelectNodes("//span[@id=\"ctl00_MainBodyContent_lblSpecification\"]");
                if (collection != null)
                {
                    int PointCounter = 1;
                    foreach (HtmlNode node in collection)
                    {
                        try
                        {
                            Feature = node.InnerText;
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
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(Bullets))
                        Bullets = Bullets.Trim().Replace("factorydirect.ca", "");

                }
                else
                    WriteLogEvent(url, "BulletPoints not found");

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
                if (_doc.DocumentNode.SelectNodes("//a[@id=\"ctl00_MainBodyContent_ProductImageHyperLink\"]") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//a[@id=\"ctl00_MainBodyContent_ProductImageHyperLink\"]")[0].Attributes)
                    {
                        if (attr.Name == "href")
                            Images = attr.Value.Trim();
                    }
                }
                else if (_doc.DocumentNode.SelectNodes("//img[@id=\"ctl00_MainBodyContent_imgProduct\"]") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//img[@id=\"ctl00_MainBodyContent_imgProduct\"]")[0].Attributes)
                    {
                        if (attr.Name == "src")
                            Images = attr.Value.Trim();
                    }
                }
                else
                    WriteLogEvent(url, "Main Images not found");


                product.Image = Images;
                #endregion Image

                #region sku
                if (_doc.DocumentNode.SelectNodes("//span[@id=\"ctl00_MainBodyContent_lblProductCode\"]") != null)
                {
                    product.SKU = "FDCA" + _doc.DocumentNode.SelectNodes("//span[@id=\"ctl00_MainBodyContent_lblProductCode\"]")[0].InnerText.Trim();
                    product.parentsku = "FDCA" + _doc.DocumentNode.SelectNodes("//span[@id=\"ctl00_MainBodyContent_lblProductCode\"]")[0].InnerText.Trim();
                }


                #endregion sku
                product.Isparent = true;
                product.Stock = "0";
                if (_doc.DocumentNode.SelectNodes("//input[@id=\"ctl00_MainBodyContent_btnAdd\"]") != null)
                    product.Stock = "1";
                product.URL = url;
                if (product.Brand.ToUpper() != "SOLOGEAR")
                    Products.Add(product);
            }
            catch
            { }
        }


        public string GetUPC(string Response)
        {
            string Result = "";
            foreach (var ch in Response.ToCharArray())
            {
                if (char.IsNumber(ch))
                    Result = Result + ch;
                else
                    break;

            }
            Int64 n;
            bool isNumeric = Int64.TryParse(Result, out n);
            if (n != 0)
                return Result;
            else
                return "";


        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public void WriteLogEvent(string url, string Detail)
        {
            writer.WriteLine(Detail + "/t" + url);
        }
        public void work_dowork(object sender, DoWorkEventArgs e)
        {

            bool _Iserror = false;
            int counterReload = 0;
            int checkcounter = 0;
            if (_IsProduct)
            {
                do
                {
                    try
                    {
                        counterReload++;
                        _Work1doc.LoadHtml(_Client1.DownloadString(Url1));
                        _Iserror = false;
                        Application.DoEvents();

                    }
                    catch
                    {
                        _Iserror = true;
                    }
                } while (counterReload < 25 && _Iserror);
            }
            else
            {
                do
                {
                    try
                    {
                        counterReload++;
                        _Worker1.GoTo(Url1);
                        _Iserror = false;
                        _Worker1.WaitForComplete();
                        if (_Worker1.Html == null || !_Worker1.Html.ToLower().Contains("class=\"nxt-product-list\""))
                        {
                            do
                            {
                                System.Threading.Thread.Sleep(20);
                                Application.DoEvents();
                                checkcounter++;
                            } while ((_Worker1.Html == null || !_Worker1.Html.ToLower().Contains("class=\"nxt-product-list\"")) && checkcounter < 10000);
                        }
                        _Work1doc.LoadHtml(_Worker1.Html);
                        Application.DoEvents();


                    }
                    catch
                    {
                        _Iserror = true;
                    }
                } while (counterReload < 25 && _Iserror);
            }
            if (_Iserror)
                WriteLogEvent(Url1, "issue accured in loading Given URL is not found");
            if (_IsCategory && !_Iserror)
            {
                try
                {
                    GetCategoryInfo(_Work1doc, Url1);
                }
                catch
                { WriteLogEvent(Url1, "Issue accured in reading produts from category page"); }

                /**********Report progress**************/
                gridindex++;
                _Work.ReportProgress((gridindex * 100 / CategoryUrl.Count));

                /****************end*******************/
            }
            else if (_IsProduct && !_Iserror)
            {
                try
                {
                    GetProductInfo(_Work1doc, Url1);
                }
                catch
                { WriteLogEvent(Url1, "Issue accured in reading product Info."); }

                /**********Report progress**************/
                gridindex++;
                _Work.ReportProgress((gridindex * 100 / Producturl.Count));

                /****************end*******************/
            }

        }
        public void work_RunWorkerAsync(object sender, RunWorkerCompletedEventArgs e)
        {


        }
        public void work_dowork1(object sender, DoWorkEventArgs e)
        {

            bool _Iserror = false;
            int checkcounter = 0;
            int counterReload = 0;
            if (_IsProduct)
            {
                do
                {
                    try
                    {
                        counterReload++;
                        _Work1doc2.LoadHtml(_Client2.DownloadString(Url2));
                        _Iserror = false;
                        Application.DoEvents();
                    }
                    catch
                    {
                        _Iserror = true;
                    }
                } while (counterReload < 25 && _Iserror);
            }
            else
            {
                do
                {
                    try
                    {
                        counterReload++;
                        _Worker2.GoTo(Url2);
                        _Iserror = false;
                        _Worker2.WaitForComplete();
                        if (_Worker2.Html == null || !_Worker2.Html.ToLower().Contains("class=\"nxt-product-list\""))
                        {
                            do
                            {
                                System.Threading.Thread.Sleep(20);
                                Application.DoEvents();
                                checkcounter++;
                            } while ((_Worker2.Html == null || !_Worker2.Html.ToLower().Contains("class=\"nxt-product-list\"")) && checkcounter < 10000);
                        }
                        _Work1doc2.LoadHtml(_Worker2.Html);
                        Application.DoEvents();


                    }
                    catch
                    {
                        _Iserror = true;
                    }
                } while (counterReload < 25 && _Iserror);
            }
            if (_Iserror)
                WriteLogEvent(Url2, "issue accured in loading Given URL is not found");
            if (_IsCategory && !_Iserror)
            {
                try
                {
                    GetCategoryInfo(_Work1doc2, Url2);
                }
                catch
                { WriteLogEvent(Url2, "Issue accured in reading produts from category page"); }
                gridindex++;
                _Work1.ReportProgress((gridindex * 100 / CategoryUrl.Count));
            }
            else if (_IsProduct && !_Iserror)
            {
                try
                {
                    GetProductInfo(_Work1doc2, Url2);
                }
                catch
                { WriteLogEvent(Url2, "Issue accured in reading product Info."); }
                gridindex++;
                _Work1.ReportProgress((gridindex * 100 / Producturl.Count));
            }
        }
        public void work_RunWorkerAsync1(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        public string Removeunsuaalcharcterfromstring(string name)
        {
            return name.Replace("â€“", "-").Replace("Ã±", "ñ").Replace("â€™", "'").Replace("Ã¢â‚¬â„¢", "'").Replace("ÃƒÂ±", "ñ").Replace("Ã¢â‚¬â€œ", "-").Replace("Â ", "").Replace("Â", "").Trim();

        }
        private void Go_Click(object sender, EventArgs e)
        {

            _IsProduct = false;
            _percent.Visible = false;
            _Bar1.Value = 0;
            _lblerror.Visible = false;
            _Pages = 0;
            _TotalRecords = 0;
            gridindex = 0;
            _IsCategory = true;
            _Stop = false;
            time = 0;

            _Worker1 = new IE();
            _Worker2 = new IE();

            #region Factory.ca
            _ScrapeUrl = "http://www.factorydirect.ca/SearchResults.aspx";
            try
            {

                _lblerror.Visible = true;
                _lblerror.Text = "We are going to read category Link for factorydirect.ca Website";
                int counterReload = 0;
                bool isError = false;

                _Worker1.GoTo(_ScrapeUrl);
                _Worker1.WaitForComplete();
                System.Threading.Thread.Sleep(10000);
                _Work1doc.LoadHtml(_Worker1.Html);

                HtmlNodeCollection _CollectionCatLink = _Work1doc.DocumentNode.SelectNodes("//b[@class=\"nxt-result-total\"]");
                if (_CollectionCatLink != null)
                {
                    try
                    {
                        _TotalRecords = Convert.ToInt32(_CollectionCatLink[0].InnerText.Trim());
                        if ((_TotalRecords % 36) == 0)
                        {
                            _Pages = Convert.ToInt32(_TotalRecords / 36);
                        }
                        else
                        {
                            _Pages = Convert.ToInt32(_TotalRecords / 36) + 1;
                        }
                    }
                    catch
                    { }

                    while (_Work.IsBusy || _Work1.IsBusy)
                    {
                        Application.DoEvents();

                    }
                    if (_TotalRecords > 0)
                    {
                        gridindex = 0;
                        _Bar1.Value = 0;
                        _percent.Visible = false;
                        _lblerror.Visible = true;
                        _lblerror.Text = "We are going to read products from search page.";
                        _Stop = false;
                        time = 0;
                        _IsCategory = true;
                        tim(3);
                        totalrecord.Visible = true;

                        for (int Page = 1; Page <= _Pages; Page++)
                        {
                            CategoryUrl.Add("http://www.factorydirect.ca/SearchResults.aspx#/?search_return=all&res_per_page=36&page=" + Page);
                        }
                        totalrecord.Text = "Total No Pages :" + CategoryUrl.Count.ToString();

                        foreach (string url in CategoryUrl)
                        {
                            while (_Work.IsBusy && _Work1.IsBusy)
                            {
                                Application.DoEvents();
                            }

                            if (!_Work.IsBusy)
                            {
                                Url1 = url;
                                _Work.RunWorkerAsync();
                            }
                            else
                            {
                                Url2 = url;
                                _Work1.RunWorkerAsync();
                            }

                        }
                        while (_Work.IsBusy || _Work1.IsBusy)
                        {
                            Application.DoEvents();

                        }
                        _lblerror.Visible = true;
                        _lblerror.Text = "We are going to read product info.";
                        _IsCategory = false;
                        _IsProduct = true;
                        gridindex = 0;
                        totalrecord.Text = "Total No Products :" + Producturl.Count.ToString();
                        foreach (var url in Producturl)
                        {
                            while (_Work.IsBusy && _Work1.IsBusy)
                            {
                                Application.DoEvents();
                            }

                            if (!_Work.IsBusy)
                            {
                                Url1 = url.Key; 
                                _Work.RunWorkerAsync();
                            }
                            else
                            {
                                Url2 = url.Key;
                                _Work1.RunWorkerAsync();
                            }
                        }
                        while (_Work.IsBusy || _Work1.IsBusy)
                        {
                            Application.DoEvents();

                        }

                        #region InsertScrappedProductInDatabase

                        if (Products.Count() > 0)
                        {
                            _Prd.ProductDatabaseIntegration(Products, "factorydirect.ca", 1);

                        }
                        else
                        {
                            BusinessLayer.DB _Db = new BusinessLayer.DB();
                            _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='factorydirect.ca'");
                            _Prd.ProductDatabaseIntegration(Products, "factorydirect.ca", 1);
                            _Mail.SendMail("OOPS there is no any product scrapped by app for factorydirect.ca Website." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);
                        }
                        #endregion InsertScrappedProductInDatabase
                    }
                    else
                    {
                        BusinessLayer.DB _Db = new BusinessLayer.DB();
                        _Prd.ProductDatabaseIntegration(Products, "factorydirect.ca", 1);
                        _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='factorydirect.ca'");
                        _lblerror.Text = "Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for factorydirect.ca Website";
                        /****************Email****************/
                        _Mail.SendMail("Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for factorydirect.ca Website as soon as possible because noscrapping of given store is stopped working." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);
                        /*******************End********/
                    }


                }

                else
                {
                    BusinessLayer.DB _Db = new BusinessLayer.DB();
                    _Prd.ProductDatabaseIntegration(Products, "factorydirect.ca", 1);
                    _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='factorydirect.ca'");
                    _lblerror.Text = "Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for factorydirect.ca Website";
                    /****************Email****************/
                    _Mail.SendMail("Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for factorydirect.ca Website as soon as possible because noscrapping of given store is stopped working." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);
                    /*******************End********/
                }
            }
            catch
            {
                BusinessLayer.DB _Db = new BusinessLayer.DB();
                _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='factorydirect.ca'");
                _lblerror.Visible = true;
                _Mail.SendMail("Oops Some issue Occured in scrapping data factorydirect.ca Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

            }
            while (_Work.IsBusy || _Work1.IsBusy)
            {
                Application.DoEvents();

            }
            # endregion Factory.CA
            writer.Close();

            try { _Worker1.Close(); _Worker2.Close(); }
            catch
            {

            }
            this.Close();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            base.Show();
            this.Go_Click(null, null);
        }
    }

    public class ExtendedWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 120000;
            return w;
        }
    }

}
