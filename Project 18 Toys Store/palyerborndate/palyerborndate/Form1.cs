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
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
namespace palyerborndate
{
    public partial class Form1 : Form
    {

        #region DatbaseVariable
        SqlConnection Connection = new SqlConnection(System.Configuration.ConfigurationSettings.
                                               AppSettings["connectionstring"]);
        #endregion DatbaseVariable
        #region booltypevariable

        bool _ISBuy = false;
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
        #region stringtypevariable

        string Url1 = "";
        string Url2 = "";
        string _ScrapeUrl = "http://www.warriorsandwonders.com/index.php?main_page=advanced_search_result&keyword=keywords&search_in_description=1&product_type=&kfi_blade_length_from=0&kfi_blade_length_to=15&kfi_overall_length_from=0&kfi_overall_length_to=30&kfi_serration=ANY&kfi_is_coated=ANY&kfo_blade_length_from=0&kfo_blade_length_to=8&kfo_overall_length_from=0&kfo_overall_length_to=20&kfo_serration=ANY&kfo_is_coated=ANY&kfo_assisted=ANY&kk_blade_length_from=0&kk_blade_length_to=15&fl_lumens_from=0&fl_lumens_to=18000&fl_num_cells_from=1&fl_num_cells_to=10&fl_num_modes_from=1&fl_num_modes_to=15&sw_blade_length_from=0&sw_blade_length_to=60&sw_overall_length_from=0&sw_overall_length_to=70&inc_subcat=1&pfrom=0.01&pto=10000.00&x=36&y=6&perPage=60";
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

        #region supplier
        List<Supplier> Suppliers = new List<Supplier>();
        Supplier workingSupplier = new Supplier();
        #endregion supplier

        public Form1()
        {

            InitializeComponent();

            Suppliers.Add(new Supplier { Url = "http://www.toysrus.com/shop/index.jsp?categoryId=2255956",Domain="http://www.toysrus.com/" StoreID = 1, Prefix = "TYRSCM", SupplierName = "toysrus.com" });
            Suppliers.Add(new Supplier { Url = "http://www.toysrus.ca/home/index.jsp?categoryId=2567269",Domain="http://www.toysrus.ca/", StoreID = 2, Prefix = "TYRSCA", SupplierName = "toysrus.ca" });
            Suppliers.Add(new Supplier { Url = "https://www.ebgames.ca/",Domain="https://www.ebgames.ca/", StoreID = 3, Prefix = "EBGMCA", SupplierName = "ebgames.ca" });
            Suppliers.Add(new Supplier { Url = "http://www.gamestop.com/",Domain="http://www.gamestop.com/", StoreID = 4, Prefix = "GMSTPCM", SupplierName = "gamestop.com" });

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
            List<BusinessLayer.InventorySync> PrdData = new List<BusinessLayer.InventorySync>();
            string skus = "";
            HtmlNodeCollection productCollection = _doc.DocumentNode.SelectNodes("//div[@id=\"ctl00_CC_ProductSearchResultListing_SearchProductListing\"]/ul//li");
            if (productCollection != null)
            {
                foreach (HtmlNode prd in productCollection)
                {
                    HtmlNodeCollection _CollectionCatLink = prd.SelectNodes(".//div[@class=\"prod-image\"]/a");
                    if (_CollectionCatLink != null)
                    {
                        foreach (HtmlNode node in _CollectionCatLink)
                        {
                            foreach (HtmlAttribute attr in node.Attributes)
                            {
                                if (attr.Name == "href")
                                    if (!Producturl.Keys.Contains("http://www.bestToyStores/" + attr.Value))
                                    {
                                        HtmlNodeCollection _CollectionStock = prd.SelectNodes(".//ul[@class=\"prod-availability list-layout-prod-availability\"]");
                                        if (_CollectionStock != null)
                                        {
                                            string sku = "";
                                            foreach (HtmlAttribute attrStock in _CollectionStock[0].Attributes)
                                            {
                                                if (attrStock.Name == "data-sku")
                                                    sku = attrStock.Value.Trim();
                                            }
                                            if (sku != "")
                                            {
                                                try
                                                {
                                                    Producturl.Add("http://www.bestToyStores/" + attr.Value, sku);
                                                    skus = skus + sku + "|";
                                                    #region Price
                                                    try
                                                    {
                                                        HtmlNodeCollection _price = prd.SelectNodes(".//div[@class=\"prodprice\"]/span[@class=\"amount\"]");
                                                        if (_price == null)
                                                            _price = prd.SelectNodes(".//div[@class=\"prodprice price-onsale\"]/span[@class=\"amount\"]");

                                                        if (_price != null)
                                                        {
                                                            BusinessLayer.InventorySync data = new BusinessLayer.InventorySync();
                                                            data.SKU = sku;
                                                            data.Price = _price[0].InnerText.Replace("$", "").Trim();
                                                            data.Stock = "1";
                                                            PrdData.Add(data);
                                                        }
                                                    }
                                                    catch { }
                                                    #endregion Price
                                                }
                                                catch { }
                                            }
                                            else
                                                WriteLogEvent(url, "sku not found");
                                        }
                                        else
                                            WriteLogEvent(url, "Availibility tag is not found");
                                    }
                            }
                        }
                    }
                    else
                        WriteLogEvent(url, "prod-image tag is not found");
                }
                if (skus.Length > 0)
                    GetOnlineStock(skus, PrdData);
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
                if (_doc.DocumentNode.SelectNodes("//meta[@property=\"og:title\"]") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//meta[@property=\"og:title\"]")[0].Attributes)
                    {
                        if (attr.Name == "content")
                            product.Name = System.Net.WebUtility.HtmlDecode(attr.Value);
                    }

                }
                else if (_doc.DocumentNode.SelectNodes("//h1[@itemprop=\"name\"]") != null)
                    product.Name = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//h1[@itemprop=\"name\"]")[0].InnerText.Trim());
                else
                    WriteLogEvent(url, "title not found");
                #endregion title
                #region price
                if (_doc.DocumentNode.SelectNodes("//div[@itemprop=\"price\"]") != null)
                    product.Price = _doc.DocumentNode.SelectNodes("//div[@itemprop=\"price\"]")[0].InnerText.Replace("$", "").Trim();
                else
                {
                    product.Price = "0";
                    WriteLogEvent(url, "Price not found");
                }
                #endregion price
                #region Brand
                if (_doc.DocumentNode.SelectNodes("//span[@class=\"brand-logo\"]//img") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//span[@class=\"brand-logo\"]//img")[0].Attributes)
                    {
                        if (attr.Name == "alt")
                        {
                            product.Brand = System.Net.WebUtility.HtmlDecode(attr.Value.Trim());
                            product.Manufacturer = System.Net.WebUtility.HtmlDecode(attr.Value.Trim());
                        }
                    }
                    if (product.Brand == "")
                    {
                        if (_doc.DocumentNode.SelectNodes("//div[@class=\"brand-logo\"]//a") != null)
                        {
                            product.Brand = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//div[@class=\"brand-logo\"]//a")[0].InnerText.Trim());
                            product.Manufacturer = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//div[@class=\"brand-logo\"]//a")[0].InnerText.Trim());
                        }
                        else
                            WriteLogEvent(url, "Brand not found");
                    }
                }
                else if (_doc.DocumentNode.SelectNodes("//div[@class=\"brand-logo\"]//a") != null)
                {
                    product.Brand = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//div[@class=\"brand-logo\"]//a")[0].InnerText.Trim());
                    product.Manufacturer = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//div[@class=\"brand-logo\"]//a")[0].InnerText.Trim());
                }
                else
                    WriteLogEvent(url, "Brand not found");
                #endregion Brand
                #region Category
                if (_doc.DocumentNode.SelectNodes("//span[@property=\"itemListElement\"]//a") != null)
                {
                    try
                    {
                        product.Category = System.Net.WebUtility.HtmlDecode("BBYCA" + _doc.DocumentNode.SelectNodes("//span[@property=\"itemListElement\"]//a")[1].InnerText.Trim());
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
                if (_doc.DocumentNode.SelectNodes("//div[@class=\"tab-overview-item\"]") != null)
                {
                    foreach (HtmlNode node in _doc.DocumentNode.SelectNodes("//div[@class=\"tab-overview-item\"]"))
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
                    product.Description = System.Net.WebUtility.HtmlDecode(Description.Replace("Â", ""));
                }
                else
                    WriteLogEvent(url, "Description not found");
                #endregion description
                #region BulletPoints
                string Feature = "";
                string Bullets = "";

                HtmlNodeCollection collection = _doc.DocumentNode.SelectNodes("//ul[@class=\"std-tablist\"]//li");
                if (collection == null)
                    collection = _doc.DocumentNode.SelectNodes("//ul[@class=\"std-tablist nobpadding\"]//li");
                if (collection != null)
                {
                    string Header = "";
                    string Value = "";
                    int PointCounter = 1;
                    foreach (HtmlNode node in collection)
                    {
                        try
                        {
                            Header = System.Net.WebUtility.HtmlDecode(node.SelectNodes(".//span")[0].InnerText.Trim());
                            Value = System.Net.WebUtility.HtmlDecode(node.SelectNodes(".//div")[0].InnerText.Trim());
                            if (Value != "")
                            {
                                if (Header.ToLower() == "color" || Header.ToLower() == "colour")
                                    product.Color = Value;
                                else if (Header.ToLower() == "size")
                                    product.Size = Value;
                                else if (Header.ToLower() == "appropriate ages")
                                {
                                    if (Value.ToLower().Contains("year"))
                                        product.AgeUnitMeasure = "Year";
                                    else
                                        product.AgeUnitMeasure = "Month";
                                    string childAge = System.Net.WebUtility.HtmlDecode(Value);
                                    childAge = Regex.Replace(childAge, @"[^\d]", String.Empty);
                                    int Age = 0;
                                    int.TryParse(childAge, out Age);
                                    product.MinimumAgeRecommend = Age == 0 || Age > 50 ? 1 : Age;
                                }
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
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(Bullets))
                        Bullets = Bullets.Trim();

                }
                else
                    WriteLogEvent(url, "BulletPoints not found");


                #region ItemsInBox
                if (_doc.DocumentNode.SelectNodes("//div[@class=\"tab-content-right dynamic-content-column\"]//li") != null)
                    Bullets = Bullets + "@@" + System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//div[@class=\"tab-content-right dynamic-content-column\"]//li")[0].InnerText) + ". ";
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
                #endregion ItemsInBox


                #endregion BulletPoints
                #region Image
                string Images = "";
                if (_doc.DocumentNode.SelectNodes("//div[@id=\"pdp-gallery\"]//img") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//div[@id=\"pdp-gallery\"]//img")[0].Attributes)
                    {
                        if (attr.Name == "src")
                            Images = attr.Value.Trim();
                    }
                }
                else
                    WriteLogEvent(url, "Main Images not found");

                foreach (HtmlNode node in _doc.DocumentNode.SelectNodes("//script"))
                {
                    if (node.InnerText.Contains("pdpProduct"))
                    {
                        try
                        {
                            string script = "{\"" + node.InnerText.Substring(node.InnerText.IndexOf("pdpProduct")).Replace("//]]>", "");
                            script = script.Substring(0, script.IndexOf("if(config)")).Trim();
                            script = script.Substring(0, script.Length - 1);
                            RootObject deserializedProduct = JsonConvert.DeserializeObject<RootObject>(script.Trim());
                            Images = "";
                            product.SKU = "BBYCA" + deserializedProduct.pdpProduct.sku;
                            product.parentsku = "BBYCA" + deserializedProduct.pdpProduct.sku;
                            int ImageCounter = 0;
                            foreach (AdditionalMedia image in deserializedProduct.pdpProduct.additionalMedia)
                            {


                                if (image.mimeType.ToLower() == "image")
                                {
                                    ImageCounter++;
                                    Images = Images + image.url + ",";
                                }
                                if (ImageCounter >= 9)
                                    break;

                            }
                            if (Images.Length > 0 && Images.Contains(","))
                                Images = Images.Substring(0, Images.Length - 1);
                        }
                        catch
                        {
                            WriteLogEvent(url, "Json conversion failed for PDPproduct script");
                        }
                        break;
                    }
                }

                product.Image = Images;
                #endregion Image
                product.Isparent = true;
                #region sku
                if (product.SKU == "")
                {
                    if (_doc.DocumentNode.SelectNodes("//span[@itemprop=\"productid\"]") != null)
                    {
                        product.SKU = "BBYCA" + _doc.DocumentNode.SelectNodes("//span[@itemprop=\"productid\"]")[0].InnerText;
                        product.parentsku = "BBYCA" + _doc.DocumentNode.SelectNodes("//span[@itemprop=\"productid\"]")[0].InnerText;
                    }
                    else
                        WriteLogEvent(url, "SKU not found");
                }
                #endregion sku
                product.Stock = "1";
                product.URL = url;
                #region UPC
                if (_doc.DocumentNode.SelectNodes("//div[@class=\"tab-overview-item\"]") != null)
                {
                    foreach (HtmlNode node in _doc.DocumentNode.SelectNodes("//div[@class=\"tab-overview-item\"]"))
                    {
                        string Innertext = System.Web.HttpUtility.HtmlDecode(node.InnerText.ToLower()).ToLower();
                        if (Innertext.Contains("upc:"))
                        {
                            string UPC = GetUPC(Innertext.Substring(Innertext.IndexOf("upc:")).Replace("upc:", "").Trim());
                            if (UPC.Length > 0)
                                product.UPC = UPC;
                            break;
                        }
                    }
                }


                #endregion UPC
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
        public bool GetOnlineStock(string skus, List<BusinessLayer.InventorySync> PrdData)
        {

            bool Result = false;
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://api.bestToyStores/availability/products?accept-language=en-CA&skus=" + skus + "&accept=application%2Fvnd.bestbuy.simpleproduct.v1%2Bjson");
                HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string responseText = reader.ReadToEnd();
                try
                {
                    RootObject deserializedProduct = JsonConvert.DeserializeObject<RootObject>(responseText);
                    if (deserializedProduct != null)
                    {

                        foreach (Availability Avail in deserializedProduct.availabilities)
                        {
                            if (Avail.shipping.status == "InStock" || Avail.shipping.status == "InStockOnlineOnly")
                            {
                                if (Avail.scheduledDelivery)
                                {
                                    try
                                    {
                                        try
                                        {
                                            foreach (BusinessLayer.InventorySync sync in PrdData)
                                            {
                                                if (sync.SKU == Avail.sku)
                                                {
                                                    sync.Stock = "0";
                                                    break;
                                                }
                                            }
                                        }
                                        catch
                                        {

                                        }
                                        Producturl.Remove(Producturl.First(m => m.Value == Avail.sku).Key);

                                    }
                                    catch (Exception EXP)
                                    {
                                        WriteLogEvent(Avail.sku, "Error accured in removing sku from dictionary for out of stock product." + EXP.Message);
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    try
                                    {
                                        foreach (BusinessLayer.InventorySync sync in PrdData)
                                        {
                                            if (sync.SKU == Avail.sku)
                                            {
                                                sync.Stock = "0";
                                                break;
                                            }
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    Producturl.Remove(Producturl.First(m => m.Value == Avail.sku).Key);

                                }
                                catch (Exception EXP)
                                {
                                    WriteLogEvent(Avail.sku, "Error accured in removing sku from dictionary for out of stock product." + EXP.Message);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    foreach (string sku in skus.Split('|'))
                    {
                        try
                        {
                            Producturl.Remove(Producturl.First(m => m.Value == sku).Key);
                        }
                        catch (Exception EXP)
                        {
                            WriteLogEvent(sku, "Error accured in removing sku from dictionary for out of stock product." + EXP.Message);
                        }

                    }
                    try
                    {
                        foreach (BusinessLayer.InventorySync sync in PrdData)
                        {
                            sync.Stock = "0";
                        }
                    }
                    catch
                    {

                    }
                    WriteLogEvent(skus, "Error accured in conversion of json to c# for stock");
                }
            }
            catch
            { }

            #region DbSyncSkuData
            try
            {
                if (PrdData.Count() > 0)
                {
                    BusinessLayer.SyncerProductData syncData = new BusinessLayer.SyncerProductData();
                    syncData.SyncInventory(PrdData, "BBYCA", 1);
                }

            }
            catch
            { }
            #endregion DbSyncSkuData
            return Result;
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
            int counterReload = 0;
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

            #region ToyStores


            try
            {
                foreach (Supplier supp in Suppliers)
                {
                    workingSupplier = supp;
                    _IsCategory = false;
                    _IsProduct = false;
                    CategoryUrl.Clear();
                    Producturl.Clear();
                    _lblerror.Visible = true;
                    _lblerror.Text = "We are going to read category Link for " + supp.SupplierName + " Website";
                    int counterReload = 0;
                    bool isError = false;

                    do
                    {
                        try
                        {
                            counterReload++;
                            _Work1doc.LoadHtml(_Client1.DownloadString(supp.Url));
                            isError = false;
                            Application.DoEvents();
                            tim(2);
                        }
                        catch
                        {
                            isError = true;
                        }
                    } while (isError && counterReload < 25);
                    try
                    {
                        if (supp.StoreID == 1)
                        {
                            HtmlNodeCollection _CollectionCatLink = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"ctl00_CC_ProductSearchResultListing_topPaging\"]//div//div//span[@class=\"display-total\"]");
                            _TotalRecords = Convert.ToInt32(_CollectionCatLink[0].InnerText.Trim());
                            if ((_TotalRecords % 32) == 0)
                            {
                                _Pages = Convert.ToInt32(_TotalRecords / 32);
                            }
                            else
                            {
                                _Pages = Convert.ToInt32(_TotalRecords / 32) + 1;
                            }


                            while (_Work.IsBusy || _Work1.IsBusy)
                            {
                                Application.DoEvents();

                            }

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
                                CategoryUrl.Add("http://www.bestToyStores/Search/SearchResults.aspx?path=ca77b9b4beca91fe414314b86bb581f8en20&page=" + Page);
                            }
                            totalrecord.Text = "Total No Pages :" + CategoryUrl.Count.ToString();
                        }

                        #region categoryPageUrl
                        foreach (string url in CategoryUrl)
                        {
                            while (_Work.IsBusy || _Work1.IsBusy)
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
                        #endregion categoryPageUrl
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
                                Url1 = url.Key; //"http://www.bestToyStores//en-CA/product/-/b0007063.aspx?path=57d9708c19625082a2c2820fd20a3b2cen02";// "http://www.bestToyStores/en-CA/product/traxxas-traxxas-x-maxx-brushless-electric-rc-monster-truck-blue-77076-4/10400679.aspx?path=e334459dbb1955f57c8d232171133dbben02";
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
                    }
                    catch(Exception exp) {
                        _Mail.SendMail("Oops Some issue Occured in scrapping data"+supp.Url+" Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);
                    
                    }
                }

            }
            catch
            {
                BusinessLayer.DB _Db = new BusinessLayer.DB();
                _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='BestToyStores'");
                _lblerror.Visible = true;
                _Mail.SendMail("Oops Some issue Occured in scrapping data BestToyStores Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

            }
            while (_Work.IsBusy || _Work1.IsBusy)
            {
                Application.DoEvents();

            }
                    # endregion ToyStores
            writer.Close();

            this.Close();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
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

    public class Supplier
    {
        public string Url { get; set; }
        public string Domain { get; set; }
        public string Prefix { get; set; }
        public int StoreID { get; set; }
        public string SupplierName { get; set; }
        public decimal Currency { get; set; }
    }
}
