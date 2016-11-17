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
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
namespace palyerborndate
{
    public partial class Form1 : System.Windows.Forms.Form
    {

        #region DatbaseVariable
        SqlConnection Connection = new SqlConnection(System.Configuration.ConfigurationSettings.
                                               AppSettings["connectionstring"]);
        #endregion DatbaseVariable
        #region booltypevariable
        bool PagingExist = true;
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
        Dictionary<string, string> SubCategoryUrl = new Dictionary<string, string>();
        Dictionary<string, string> CategoryUrl = new Dictionary<string, string>();
        Dictionary<string, string> _ProductUrl = new Dictionary<string, string>();
        List<string> variationTheme = new List<string>();

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
            variationTheme.Add("select size");
            variationTheme.Add("select skate width");
            variationTheme.Add("select color");
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
            HtmlNodeCollection coll = _doc.DocumentNode.SelectNodes("//div[@class=\"ty-grid-list__image\"]/a");
            if (coll == null)
                coll = _doc.DocumentNode.SelectNodes("//div[@class=\"cm-gallery-item cm-item-gallery\"]/a");

            if (coll != null)
            {
                foreach (HtmlNode node1 in coll)
                {

                    foreach (HtmlAttribute attr in node1.Attributes)
                    {
                        if (attr.Name == "href")
                        {
                            try
                            {
                                _ProductUrl.Add(attr.Value.ToLower(), Category);
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
                PagingExist = false;
                WriteLogEvent(url, "h2[@class=\"product-name\"]/a tag is not found");
            }
        }


        public void GetProductInfo(HtmlAgilityPack.HtmlDocument _doc, string url, string Category)
        {


            BusinessLayer.Product product = new BusinessLayer.Product();
            try
            {
                #region title
                HtmlNodeCollection formColl = _doc.DocumentNode.SelectNodes("//h1[@class=\"mainbox-title\"]");
                if (formColl != null)
                    product.Name = System.Net.WebUtility.HtmlDecode(formColl[0].InnerText).Trim();

                else if (_doc.DocumentNode.SelectNodes("//h1[@class=\"ty-product-block-title\"]") != null)
                    product.Name = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//h1[@class=\"ty-product-block-title\"]")[0].InnerText).Trim();
                else
                    WriteLogEvent(url, "title not found");
                #endregion title

                #region Price
                string priceString = "";
                double Price = 0;
                if (_doc.DocumentNode.SelectNodes("//span[@class=\"ty-price\"]") != null)
                {
                    priceString = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//span[@class=\"ty-price\"]")[0].InnerText).Replace("$", "").Trim();
                    double.TryParse(priceString, out Price);
                    if (Price != 0)
                        product.Price = Price.ToString(); //System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//span[@class=\"regular-price\"]/span[@class=\"price\"]")[0].InnerText).re;
                    else
                        WriteLogEvent(url, "Price not found");
                }
                else if (_doc.DocumentNode.SelectNodes("//meta[@itemprop=\"price\"]") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//meta[@itemprop=\"price\"]")[0].Attributes)
                    {
                        if (attr.Name == "content")
                            priceString = System.Net.WebUtility.HtmlDecode(attr.Value).Replace("$", "").Trim();
                    }

                    double.TryParse(priceString, out Price);
                    if (Price != 0)
                        product.Price = Price.ToString(); //System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//span[@class=\"regular-price\"]/span[@class=\"price\"]")[0].InnerText).re;
                    else
                        WriteLogEvent(url, "Price not found");
                }
                else
                    WriteLogEvent(url, "Price not found");
                #endregion Price


                #region Brand
                //if (_doc.DocumentNode.SelectNodes("//img[@class=\"brandlogo\"]") != null)
                //{
                //    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//img[@class=\"brandlogo\"]")[0].Attributes)
                //    {
                //        if (attr.Name == "title")
                //        {
                //            product.Brand = attr.Value.Trim();
                //            product.Manufacturer = attr.Value.Trim();
                //        }

                //    }
                //}
                //else
                //{
                //    product.Brand = "JZ HOLDINGS";
                //    product.Manufacturer = "JZ HOLDINGS";
                //}
                product.Brand = Category;
                product.Manufacturer = Category;
                #endregion Brand

                #region Category
                HtmlNodeCollection coll = _doc.DocumentNode.SelectNodes("//div[@class=\"breadcrumbs clearfix\"]");
                if (coll != null)
                {
                    HtmlNodeCollection coll1 = coll[0].SelectNodes(".//a");
                    foreach (HtmlNode node in coll1)
                    {
                        if (node.InnerText.ToLower().Trim() != "home")
                        {
                            product.Category = "POOLSP" + node.InnerText.Trim();
                            break;
                        }
                    }
                }
                else
                    product.Category = "POOLSPJZ HOLDINGS";
                #endregion Category

                product.Currency = "CAD";

                #region description
                string Description = "";
                string BulletPoints = "";
                HtmlNodeCollection desCollection = _doc.DocumentNode.SelectNodes("//div[@id=\"content_description\"]");
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
                        WriteLogEvent(url, "Description not found");
                    }
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

                HtmlNodeCollection divCollection = _doc.DocumentNode.SelectNodes("//div[@class=\"ty-product-img cm-preview-wrapper\"]");
                if (divCollection != null)
                {
                    HtmlNodeCollection imgCollection = divCollection[0].SelectNodes(".//img[@class=\"ty-pict    \"]");
                    if (imgCollection != null)
                    {

                        foreach (HtmlNode node in imgCollection)
                        {
                            foreach (HtmlAttribute attr in node.Attributes)
                            {
                                if (attr.Name == "src")
                                    Images = Images + attr.Value.Trim() + ",";
                            }

                        }
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
                string sku = "";
                if (_doc.DocumentNode.SelectNodes("//span[@class=\"ty-control-group__item\"]") != null)
                {

                    product.SKU = "POOLSP" + _doc.DocumentNode.SelectNodes("//span[@class=\"ty-control-group__item\"]")[0].InnerText.Trim();
                    product.parentsku = "POOLSP" + _doc.DocumentNode.SelectNodes("//span[@class=\"ty-control-group__item\"]")[0].InnerText.Trim();


                }
                else
                    WriteLogEvent(url, "SKU not found");

                #endregion sku

                #region stock
                product.Stock = "0";
                if (_doc.DocumentNode.SelectNodes("//span[@class=\"ty-qty-in-stock ty-control-group__item\"]") != null)
                {
                    if (_doc.DocumentNode.SelectNodes("//span[@class=\"ty-qty-in-stock ty-control-group__item\"]")[0].InnerText.ToLower() == "in stock")
                        product.Stock = "1";
                }
                #endregion stock
                product.URL = url;
                if (!_doc.DocumentNode.InnerHtml.ToLower().Contains("weeks"))
                    Products.Add(product);
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
                }
                catch
                { WriteLogEvent(Url1, "Issue accured in reading produts from category page"); }

                #region GetSubcat
                HtmlNodeCollection coll = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"Agppoolbox\"]");
                if (coll == null)
                    coll = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"abgpools\"]");
                if (coll == null)
                    coll = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"sig_pool_container\"]");
                if (coll != null)
                {
                    foreach (HtmlNode node1 in coll)
                    {
                        HtmlNodeCollection coll1 = node1.SelectNodes(".//a");
                        if (coll1 != null)
                        {
                            foreach (HtmlNode node in coll1)
                            {
                                foreach (HtmlAttribute attr in node.Attributes)
                                {
                                    if (attr.Name == "href")
                                    {
                                        try
                                        {
                                            SubCategoryUrl.Add(attr.Value.ToLower().Contains("poolsuppliescanada.ca") ? attr.Value : "https://www.poolsuppliescanada.ca" + attr.Value, "JZ HOLDINGS");
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion GetSubcat

                /**********Report progress**************/


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
                }
                catch
                { WriteLogEvent(Url2, "Issue accured in reading produts from category page"); }


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

            //string ss="=202{203}202{567}";
            //string[] cominations = ss.Split(new string[] { "202"}, StringSplitOptions.None);
            _IsProduct = false;
            _percent.Visible = false;
            _Bar1.Value = 0;
            _lblerror.Visible = false;
            gridindex = 0;
            _IsCategory = true;
            _Stop = false;
            time = 0;

            #region poolsuppliescanada.ca
            #region GetBrands
            _Work1doc2.LoadHtml(_Client2.DownloadString("https://www.poolsuppliescanada.ca/brands/"));
            HtmlNodeCollection brandColl = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"subcategories\"]/ul/li/a");
            if (brandColl != null)
            {
                foreach (HtmlNode node in brandColl)
                {
                    foreach (HtmlAttribute attr in node.Attributes)
                    {
                        if (attr.Name == "href")
                        {
                            try
                            {
                                CategoryUrl.Add(attr.Value, Removeunsuaalcharcterfromstring(StripHTML(node.InnerText)));
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            #region Category
            HtmlNodeCollection catColl = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"ty-menu__submenu\"]");
            if (catColl != null)
            {
                foreach (HtmlNode node in catColl)
                {
                    HtmlNodeCollection anchorColl = node.SelectNodes(".//a");
                    if (anchorColl != null)
                    {
                        foreach (HtmlNode node1 in anchorColl)
                        {
                            foreach (HtmlAttribute attr in node1.Attributes)
                            {
                                if (attr.Name == "href")
                                {
                                    try
                                    {
                                        CategoryUrl.Add(attr.Value, "JZ HOLDINGS");
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion Category
            #endregion GetBrands
            try
            {
                CategoryUrl.Add("https://www.poolsuppliescanada.ca/?subcats=Y&pcode_from_q=Y&pshort=Y&pfull=Y&pname=Y&pkeywords=Y&search_performed=Y&search_id=&q=&dispatch=products.search", "JZ HOLDINGS");
            }
            catch
            {
            }
            try
            {
                CategoryUrl.Add("https://www.poolsuppliescanada.ca/toys/", "JZ HOLDINGS");
            }
            catch
            {
            }
            _ISBuy = true;
            if (CategoryUrl.Count > 0)
            {
                try
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
                    totalrecord.Visible = false;
                    _Bar1.Visible = false;
                    foreach (var caturl in CategoryUrl)
                    {
                        PagingExist = true;
                        for (int i = 1; i < 1000; i++)
                        {
                            while (_Work.IsBusy || _Work1.IsBusy)
                            {
                                Application.DoEvents();
                            }
                            if (!PagingExist)
                                break;
                            if (!_Work.IsBusy)
                            {
                                Url1 = caturl.Key.Contains("?") ? caturl.Key + "&page=" + i : caturl.Key + "?page=" + i;
                                Category1 = caturl.Value;
                                _Work.RunWorkerAsync();
                            }
                            else
                            {
                                Url2 = caturl.Key.Contains("?") ? caturl.Key + "&page=" + i : caturl.Key + "?page=" + i;
                                Category2 = caturl.Value;
                                _Work1.RunWorkerAsync();
                            }


                        }
                        while (_Work.IsBusy || _Work1.IsBusy)
                        {
                            Application.DoEvents();

                        }
                    }


                    Dictionary<string, string> SubCategoryUrl1 = new Dictionary<string, string>();
                    foreach (var dic in SubCategoryUrl)
                    {
                        SubCategoryUrl1.Add(dic.Key, dic.Value);
                    }
                    while (SubCategoryUrl1.Count() > 0)
                    {

                        SubCategoryUrl.Clear();
                        foreach (var caturl in SubCategoryUrl1)
                        {
                            PagingExist = true;
                            for (int i = 1; i < 1000; i++)
                            {
                                while (_Work.IsBusy || _Work1.IsBusy)
                                {
                                    Application.DoEvents();
                                }
                                if (!PagingExist)
                                    break;
                                if (!_Work.IsBusy)
                                {
                                    Url1 = caturl.Key.Contains("?") ? caturl.Key + "&page=" + i : caturl.Key + "?page=" + i;
                                    Category1 = caturl.Value;
                                    _Work.RunWorkerAsync();
                                }
                                else
                                {
                                    Url2 = caturl.Key.Contains("?") ? caturl.Key + "&page=" + i : caturl.Key + "?page=" + i;
                                    Category2 = caturl.Value;
                                    _Work1.RunWorkerAsync();
                                }

                            }
                            while (_Work.IsBusy || _Work1.IsBusy)
                            {
                                Application.DoEvents();

                            }
                        }
                        SubCategoryUrl1.Clear();
                        foreach (var dic in SubCategoryUrl)
                        {
                            SubCategoryUrl1.Add(dic.Key, dic.Value);
                        }

                    }

                    #region GetProductUrlFromDB
                    try
                    {
                        BusinessLayer.DB _Db = new BusinessLayer.DB();
                        DataSet ds = _Db.GetDataset("select url,[Brand Name] from product prd join productstore store on store.productid=prd.productid where StoreID=19", CommandType.Text, "");
                        if (ds.Tables.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                if (!_ProductUrl.Keys.Contains(row[0].ToString().ToLower()))
                                {
                                    try
                                    {
                                        _ProductUrl.Add(row[0].ToString(), row[1].ToString());
                                    }
                                    catch
                                    { }
                                }
                            }
                        }
                    }
                    catch
                    { }
                    #endregion GetProductUrlFromDB

                    _Bar1.Visible = true;
                    totalrecord.Visible = true;
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
                        _Prd.ProductDatabaseIntegration(Products, "poolsuppliescanada.ca", 1);

                    }
                    else
                    {
                        BusinessLayer.DB _Db = new BusinessLayer.DB();
                        _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='poolsuppliescanada.ca'");
                        _Prd.ProductDatabaseIntegration(Products, "poolsuppliescanada.ca", 1);
                        _Mail.SendMail("OOPS there is no any product scrapped by app for poolsuppliescanada.ca Website." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);
                    }
                    #endregion InsertScrappedProductInDatabase
                }
                catch
                {
                    BusinessLayer.DB _Db = new BusinessLayer.DB();
                    _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='poolsuppliescanada.ca'");
                    _lblerror.Visible = true;
                    _Mail.SendMail("Oops Some issue Occured in scrapping data poolsuppliescanada.ca Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

                }
            }
            else
            {
                BusinessLayer.DB _Db = new BusinessLayer.DB();
                _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='poolsuppliescanada.ca'");
                _lblerror.Visible = true;
                _Mail.SendMail("Oops Some issue Occured in getting brands for poolsuppliescanada.ca Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

            }
            while (_Work.IsBusy || _Work1.IsBusy)
            {
                Application.DoEvents();

            }
            # endregion poolsuppliescanada.ca
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
    public class Option2
    {
        public string id { get; set; }
        public string label { get; set; }
        public string price { get; set; }
        public string oldPrice { get; set; }
        public List<string> products { get; set; }
    }
    public class skate_width
    {
        public string id { get; set; }
        public string code { get; set; }
        public string label { get; set; }
        public List<Option2> options { get; set; }
    }
    public class color
    {
        public string id { get; set; }
        public string code { get; set; }
        public string label { get; set; }
        public List<Option2> options { get; set; }
    }
    public class size
    {
        public string id { get; set; }
        public string code { get; set; }
        public string label { get; set; }
        public List<Option2> options { get; set; }
    }
    public class size_skates
    {
        public string id { get; set; }
        public string code { get; set; }
        public string label { get; set; }
        public List<Option2> options { get; set; }
    }
    public class Attributes
    {
        public skate_width skate_width { get; set; }
        public size_skates size_skates { get; set; }
        public color color { get; set; }
        public size size { get; set; }
    }
    public class TaxConfig
    {
        public bool includeTax { get; set; }
        public bool showIncludeTax { get; set; }
        public bool showBothPrices { get; set; }
        public double defaultTax { get; set; }
        public int currentTax { get; set; }
        public string inclTaxTitle { get; set; }
    }
    public class RootObject
    {
        public Attributes attributes { get; set; }
        public string template { get; set; }
        public string basePrice { get; set; }
        public string oldPrice { get; set; }
        public string productId { get; set; }
        public string chooseText { get; set; }
        public TaxConfig taxConfig { get; set; }
    }
}
