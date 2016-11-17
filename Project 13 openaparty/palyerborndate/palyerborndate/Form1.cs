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
using bestbuy;
using System.Text.RegularExpressions;
using System.Net;

namespace palyerborndate
{
    public partial class Form1 : System.Windows.Forms.Form
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

        int gridindex = 0;
        int time = 0;
        #endregion booltypevariable
        #region Buinesslayervariable
        List<BusinessLayer.Product> Products = new List<BusinessLayer.Product>();
        BusinessLayer.Mail _Mail = new BusinessLayer.Mail();
        BusinessLayer.ProductMerge _Prd = new BusinessLayer.ProductMerge();
        #endregion Buinesslayervariable
        #region intypevariable
        #endregion intypevariable
        #region stringtypevariable

        string Url1 = "";
        string Url2 = "";
        string _ScrapeUrl = "http://www.warriorsandwonders.com/index.php?main_page=advanced_search_result&keyword=keywords&search_in_description=1&product_type=&kfi_blade_length_from=0&kfi_blade_length_to=15&kfi_overall_length_from=0&kfi_overall_length_to=30&kfi_serration=ANY&kfi_is_coated=ANY&kfo_blade_length_from=0&kfo_blade_length_to=8&kfo_overall_length_from=0&kfo_overall_length_to=20&kfo_serration=ANY&kfo_is_coated=ANY&kfo_assisted=ANY&kk_blade_length_from=0&kk_blade_length_to=15&fl_lumens_from=0&fl_lumens_to=18000&fl_num_cells_from=1&fl_num_cells_to=10&fl_num_modes_from=1&fl_num_modes_to=15&sw_blade_length_from=0&sw_blade_length_to=60&sw_overall_length_from=0&sw_overall_length_to=70&inc_subcat=1&pfrom=0.01&pto=10000.00&x=36&y=6&perPage=60";
        string Category1 = "";
        string Category2 = "";
        decimal Weight = 0;
        #endregion listtypevariable
        #region listtypevariable

        Dictionary<string, string> CategoryUrl = new Dictionary<string, string>();
        Dictionary<string, string> _ProductUrl = new Dictionary<string, string>();

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

        public void GetCategoryInfo(HtmlAgilityPack.HtmlDocument _doc, string url, string Category)
        {
            HtmlNodeCollection coll = _doc.DocumentNode.SelectNodes("//table[@class=\"productListingData\"]");

            if (coll != null)
            {
                foreach (HtmlNode node4 in coll)
                {
                    HtmlNodeCollection coll1 = node4.SelectNodes(".//li");
                    if (coll1 != null)
                    {
                        foreach (HtmlNode node in coll1)
                        {
                            HtmlNodeCollection coll2 = node.SelectNodes(".//a");
                            if (coll2 != null)
                            {
                                foreach (HtmlNode node1 in coll2)
                                {
                                    foreach (HtmlAttribute attr in node1.Attributes)
                                    {
                                        if (attr.Name == "href")
                                        {
                                            try
                                            {
                                                _ProductUrl.Add(attr.Value, Category);
                                            }
                                            catch
                                            {
                                            }
                                        }

                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
                WriteLogEvent(url, "h2[@class=\"product-name\"]/a tag is not found");
        }
        public void GetProductInfo(HtmlAgilityPack.HtmlDocument _doc, string url, string Category)
        {
            BusinessLayer.Product product = new BusinessLayer.Product();
            try
            {
                #region title price
                HtmlNodeCollection formColl = _doc.DocumentNode.SelectNodes("//div[@id=\"bodyContent\"]");
                if (formColl != null)
                {
                    if (formColl[0].SelectNodes(".//h1") != null)
                    {
                        int Counter = 0;
                        foreach (HtmlNode node in formColl[0].SelectNodes(".//h1"))
                        {
                            if (Counter == 0)
                            {
                                decimal Price = 0;
                                decimal.TryParse(node.InnerText.ToLower().Replace("$", "").Replace("ca", "").Trim(), out Price);
                                product.Price = Price.ToString();
                                if (Price == 0)
                                {
                                    product.Price = "0";
                                    WriteLogEvent(url, "Price not found");
                                }
                            }
                            else
                                product.Name = System.Net.WebUtility.HtmlDecode(node.InnerText.Trim());
                            Counter++;
                            if (product.Price == "0")
                                break;
                        }
                    }
                    else
                        WriteLogEvent(url, "title not found");
                #endregion title price


                    #region Brand

                    product.Brand = "JZ HOLDINGS";
                    product.Manufacturer = "JZ HOLDINGS";
                    #endregion Brand

                    #region Category
                    product.Category = Category;
                    #endregion Category

                    product.Currency = "CAD";

                    #region description
                    string Description = "";
                    HtmlNodeCollection desCollection = _doc.DocumentNode.SelectNodes("//div[@class=\"contentText\"]");
                    if (desCollection != null)
                    {
                        foreach (HtmlNode node in desCollection[0].ChildNodes)
                        {
                            if (!node.InnerText.ToLower().Contains("wholesale") && !node.InnerText.ToLower().Contains("$") && !node.InnerText.ToLower().Contains("http"))
                                Description = Description + Removeunsuaalcharcterfromstring(StripHTML(node.InnerText).Trim() + "    ");
                        }

                        Description = Removeunsuaalcharcterfromstring(StripHTML(Description).Trim()).Replace("httpwww.partysavvy.co.uk", "").Replace("httpwww.openparty.com", "");
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

                    HtmlNodeCollection imgCollection = _doc.DocumentNode.SelectNodes("//div[@id=\"piGal\"]");
                    if (imgCollection != null)
                    {
                        HtmlNodeCollection imgCollection1 = imgCollection[0].SelectNodes(".//a");
                        foreach (HtmlNode node in imgCollection1)
                        {
                            foreach (HtmlAttribute attr in node.Attributes)
                            {
                                if (attr.Name == "href")
                                    Images = Images + attr.Value.Trim() + ",";
                            }
                            break;
                        }
                    }
                    else
                        WriteLogEvent(url, "Main Images not found");

                    if (Images.Length > 0)
                        Images = Images.Substring(0, Images.Length - 1);

                    product.Image = Images;
                    #endregion Image
                    product.Isparent = true;
                    #region sku

                    if (_doc.DocumentNode.SelectNodes("//input[@name=\"products_id\"]") != null)
                    {
                        foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//input[@name=\"products_id\"]")[0].Attributes)
                        {
                            if (attr.Name == "value")
                            {
                                product.SKU = "OPAR" + attr.Value.Trim();
                                product.parentsku = "OPAR" + attr.Value.Trim();
                            }
                        }
                    }
                    else
                        WriteLogEvent(url, "SKU not found");

                    #endregion sku

                    #region stock
                    product.Stock = "1";
                    #endregion stock

                    product.URL = url;
                    Products.Add(product);
                }
                else
                    WriteLogEvent(url, "Issue accured in reading product info from given product url. exp: ");
            }
            catch (Exception exp)
            {
                WriteLogEvent(url, "Issue accured in reading product info from given product url. exp: " + exp.Message);
            }

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
                    GetCategoryInfo(_Work1doc, Url1, Category1);
                    #region GetcategoryPaging
                    try
                    {
                        List<string> subcat = new List<string>();
                        HtmlNodeCollection coll = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"contentContainer\"]/table");
                        if (coll == null)
                            coll = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"contentContainer\"]/div[@class=\"contentText\"]/table");
                        if (coll != null)
                        {
                            HtmlNodeCollection subCat = coll[0].SelectNodes(".//a");
                            if (subCat != null)
                            {
                                foreach (HtmlNode node in subCat)
                                {
                                    foreach (HtmlAttribute attr in node.Attributes)
                                    {
                                        if (attr.Name == "href")
                                        {
                                            subcat.Add(attr.Value);
                                        }
                                    }
                                }
                            }

                        }
                        else
                            WriteLogEvent(Url1, "Issue accured in getting total No of products in given category");

                        foreach (string subCatUrl in subcat)
                        {
                            do
                            {
                                try
                                {
                                    counterReload++;
                                    _Work1doc.LoadHtml(_Client1.DownloadString(subCatUrl));
                                    _Iserror = false;
                                    Application.DoEvents();

                                }
                                catch
                                {
                                    _Iserror = true;
                                }
                            } while (counterReload < 25 && _Iserror);
                            if (!_Iserror)
                                GetCategoryInfo(_Work1doc, subCatUrl, Category1);
                        }
                    }
                    catch
                    {
                        WriteLogEvent(Url1, "Issue accured in getting total No of products in given category");
                    }
                    #endregion GetcategoryPaging

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
                    GetProductInfo(_Work1doc, Url1, Category1);
                }
                catch
                { WriteLogEvent(Url1, "Issue accured in reading product Info."); }

                /**********Report progress**************/
                gridindex++;
                _Work.ReportProgress((gridindex * 100 / _ProductUrl.Count));

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
                    GetCategoryInfo(_Work1doc2, Url2, Category2);
                    #region GetcategoryPaging
                    try
                    {
                        List<string> subcat = new List<string>();
                        HtmlNodeCollection coll = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"contentContainer\"]/table");
                        if (coll == null)
                            coll = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"contentContainer\"]/div[@class=\"contentText\"]/table");
                        if (coll != null)
                        {
                            HtmlNodeCollection subCat = coll[0].SelectNodes(".//a");
                            if (subCat != null)
                            {
                                foreach (HtmlNode node in subCat)
                                {
                                    foreach (HtmlAttribute attr in node.Attributes)
                                    {
                                        if (attr.Name == "href")
                                        {
                                            subcat.Add(attr.Value);
                                        }
                                    }
                                }
                            }

                        }
                        else
                            WriteLogEvent(Url2, "Issue accured in getting total No of products in given category");

                        foreach (string subCatUrl in subcat)
                        {
                            counterReload = 0;
                            do
                            {
                                try
                                {
                                    counterReload++;
                                    _Work1doc2.LoadHtml(_Client2.DownloadString(subCatUrl));
                                    _Iserror = false;
                                    Application.DoEvents();

                                }
                                catch
                                {
                                    _Iserror = true;
                                }
                            } while (counterReload < 25 && _Iserror);
                            if (!_Iserror)
                                GetCategoryInfo(_Work1doc2, subCatUrl, Category2);
                        }
                    }
                    catch
                    {
                        WriteLogEvent(Url2, "Issue accured in getting total No of products in given category");
                    }
                    #endregion GetcategoryPaging

                }
                catch
                { WriteLogEvent(Url2, "Issue accured in reading produts from category page"); }

                /**********Report progress**************/
                gridindex++;
                _Work1.ReportProgress((gridindex * 100 / CategoryUrl.Count));

                /****************end*******************/
            }
            else if (_IsProduct && !_Iserror)
            {
                try
                {
                    GetProductInfo(_Work1doc2, Url2, Category2);
                }
                catch
                { WriteLogEvent(Url2, "Issue accured in reading product Info."); }

                /**********Report progress**************/
                gridindex++;
                _Work1.ReportProgress((gridindex * 100 / _ProductUrl.Count));

                /****************end*******************/
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
            gridindex = 0;
            _IsCategory = true;
            _Stop = false;
            time = 0;

            #region openaparty.com

            _ISBuy = true;
            _ScrapeUrl = "http://www.openaparty.com/applications/Category/guidedSearch.asp?CatId=12&cm_re=Homepage-_--_-CatId_12";
            try
            {

                _lblerror.Visible = true;
                _lblerror.Text = "We are going to read category Link for openaparty.com Website";

                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-all-foil-and-latex-balloons-colour-c-879_1688_999#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-balloon-weights-c-879_1688_1173#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-balloon-bouquets-c-879_1688_1160#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-foil-and-latex-french-balloons-c-879_1688_1169#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-happy-birthday-foil-balloons-c-879_1688_1182#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-happy-birthday-latex-balloons-c-879_1688_1166#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-jumbo-letter-foils-c-879_1688_1181#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-jumbo-number-foil-balloons-c-879_1688_1180#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-latex-and-foil-age-balloons-c-879_1688_1175#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-patterned-latex-balloons-c-879_1688_1172#bc", "OPAR200 balloons");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/all-balloons-curling-ribbon-c-879_1688_1191#bc", "OPAR200 balloons");

                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/school-supplies-stationary-products-crayons-c-943_1270_1357#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/school-supplies-stationary-products-pens-c-943_1270_1355#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/school-supplies-stationary-products-erasers-sharpeners-toppers-grips-c-943_1270_1586#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/school-supplies-stationary-products-pencils-pencil-crayons-c-943_1270_1356#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/school-supplies-stationary-products-stationary-sets-c-943_1270_1739#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/school-supplies-stationary-products-stencils-c-943_1270_1732#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/school-supplies-stationary-products-stickers-sticker-scenes-c-943_1270_1361#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/school-supplies-stationary-products-paper-c-943_1270_1595#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/school-supplies-stationary-products-dry-erase-products-c-943_1270_1360#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/teaching-category-classroom-pocket-charts-c-943_1666_1261#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/teaching-category-dry-erase-supplies-and-tools-c-943_1666_1260#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/teaching-category-reading-corner-supplies-c-943_1666_1596#bc", "OPAR200 Crafts and supplies");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/teaching-subject-language-arts-and-literacy-supplies-c-943_1667_1669#bc", "OPAR200 Crafts and supplies");

                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/teaching-resources-crafts-and-supplies-classroom-themes-c-943_1587#bc", "OPAR200 Crafts and supplies");
         
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/teaching-resources-crafts-and-supplies-classroom-crafts-c-943_1364#bc", "OPAR200 Crafts and supplies");


                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/200-filled-loot-boxes-c-70#bc", "OPAR200 Filled Loot Boxes");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/5000-novelty-toys-c-474#bc", "OPAR200 Filled Loot Boxes");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/200-filled-loot-boxes-100s-empty-loot-boxes-c-70_948", "OPAR200 Filled Loot Boxes");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/200-filled-loot-boxes-100s-favor-pails-and-buckets-c-70_950", "OPARBuckets and Pails");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/200-filled-loot-boxes-100s-gift-and-favor-bags-c-70_947", "OPARGift And Favor Bags");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/200-filled-loot-boxes-cool-popcorn-treat-boxes-c-70_949", "OPARPopcorn Treat Boxes");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/party-decorations-and-balloons-designer-straws-c-879_1255#bc", "OPARDesigner Straws");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/solid-colour-and-patterned-partyware-c-879_880#bc", "OPARSolid Colour and Patterned Partyware");
                CategoryUrl.Add("http://openaparty.com/open-a-party-shop/index.php/party-decorations-and-balloons-cake-candles-c-879_881#bc", "OPARCandles");
                while (_Work.IsBusy || _Work1.IsBusy)
                {
                    Application.DoEvents();

                }
                if (CategoryUrl.Count() > 0)
                {
                    gridindex = 0;
                    _Bar1.Value = 0;
                    _percent.Visible = false;
                    _lblerror.Visible = true;
                    _lblerror.Text = "We are going to read products from category page.";
                    _Stop = false;
                    time = 0;
                    _IsCategory = true;
                    tim(3);
                    totalrecord.Visible = true;
                    totalrecord.Text = "Total No Pages :" + CategoryUrl.Count.ToString();

                    foreach (var url in CategoryUrl)
                    {
                        while (_Work.IsBusy || _Work1.IsBusy)
                        {
                            Application.DoEvents();
                        }

                        if (!_Work.IsBusy)
                        {
                            Url1 = url.Key;
                            Category1 = url.Value;
                            _Work.RunWorkerAsync();
                        }
                        else
                        {
                            Url2 = url.Key;
                            Category2 = url.Value;
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

                    totalrecord.Text = "Total No Products :" + _ProductUrl.Count.ToString();

                    foreach (var url in _ProductUrl)
                    {
                        while (_Work.IsBusy && _Work1.IsBusy)
                        {
                            Application.DoEvents();
                        }

                        if (!_Work.IsBusy)
                        {
                            Url1 = url.Key;
                            Category1 = url.Value;
                            _Work.RunWorkerAsync();
                        }
                        else
                        {
                            Url2 = url.Key;
                            Category2 = url.Value;
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
                        _Prd.ProductDatabaseIntegration(Products, "openaparty.com", 1);

                    }
                    else
                    {
                        BusinessLayer.DB _Db = new BusinessLayer.DB();
                        _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='openaparty.com'");
                        _Prd.ProductDatabaseIntegration(Products, "openaparty.com", 1);
                        _Mail.SendMail("OOPS there is no any product scrapped by app for openaparty.com Website." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);
                    }
                    #endregion InsertScrappedProductInDatabase
                }
                else
                {
                    BusinessLayer.DB _Db = new BusinessLayer.DB();
                    _Prd.ProductDatabaseIntegration(Products, "openaparty.com", 1);
                    _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='openaparty.com'");
                    _lblerror.Text = "Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for openaparty.com Website";
                    /****************Email****************/
                    _Mail.SendMail("Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for openaparty.com Website as soon as possible because noscrapping of given store is stopped working." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);
                    /*******************End********/
                }



            }
            catch
            {
                BusinessLayer.DB _Db = new BusinessLayer.DB();
                _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='openaparty.com'");
                _lblerror.Visible = true;
                _Mail.SendMail("Oops Some issue Occured in scrapping data openaparty.com Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

            }
            while (_Work.IsBusy || _Work1.IsBusy)
            {
                Application.DoEvents();

            }
            # endregion openaparty.com
            writer.Close();

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
