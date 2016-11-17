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
            HtmlNodeCollection coll = _doc.DocumentNode.SelectNodes("//a[@class=\"product-image\"]");

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
                                _ProductUrl.Add(attr.Value, Category);
                            }
                            catch
                            {
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
                #region title
                HtmlNodeCollection formColl = _doc.DocumentNode.SelectNodes("//meta[@property=\"og:title\"]");
                if (formColl != null)
                {
                    foreach (HtmlAttribute attr in formColl[0].Attributes)
                    {
                        if (attr.Name == "content")
                            product.Name = System.Net.WebUtility.HtmlDecode(attr.Value).Trim();
                    }

                }
                else if (_doc.DocumentNode.SelectNodes("//div[@class=\"product-name\"]/h1") != null)
                    product.Name = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//div[@class=\"product-name\"]/h1")[0].InnerText).Trim();
                else
                    WriteLogEvent(url, "title not found");
                #endregion title

                #region Price
                if (_doc.DocumentNode.SelectNodes("//span[@class=\"regular-price\"]/span[@class=\"price\"]") != null)
                {
                    double Price = 0;
                    string priceString = System.Net.WebUtility.HtmlDecode(_doc.DocumentNode.SelectNodes("//span[@class=\"regular-price\"]/span[@class=\"price\"]")[0].InnerText).Replace("$", "").Trim();
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

                product.Brand = "JZ HOLDINGS";
                product.Manufacturer = "JZ HOLDINGS";
                #endregion Brand

                #region Category
                product.Category = Category;
                #endregion Category

                product.Currency = "CAD";

                #region description
                string Description = "";
                string BulletPoints = "";
                HtmlNodeCollection desCollection = _doc.DocumentNode.SelectNodes("//ul[@class=\"slides\"]/li");
                if (desCollection != null)
                {
                    try
                    {
                        foreach (HtmlNode node in desCollection)
                        {
                            if (node.InnerText.ToLower().Contains("overview"))
                                Description = Description + Removeunsuaalcharcterfromstring(StripHTML(node.InnerText).Trim() + "    ");
                            else if (node.InnerText.ToLower().Contains("specifications"))
                            {
                                if (node.SelectNodes(".//tr") != null)
                                {

                                    foreach (HtmlNode node1 in node.SelectNodes(".//tr"))
                                    {
                                        if (node1.SelectNodes(".//td") != null)
                                        {
                                            string Header = "";
                                            string Value = "";
                                            try
                                            {
                                                Header = node1.SelectNodes(".//th")[0].InnerText.Trim();
                                                Value = node1.SelectNodes(".//td")[0].InnerText.Trim();
                                                if (Header.ToLower() == "brand")
                                                    if (Value.ToLower() != "no")
                                                    {
                                                        product.Manufacturer = Value;
                                                        product.Brand = Value;
                                                    }
                                                BulletPoints = BulletPoints + Header + " " + Value + " ";
                                            }
                                            catch
                                            { }


                                        }
                                    }
                                }
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
                        WriteLogEvent(url, "Description not found");
                    }
                    if (!string.IsNullOrEmpty(BulletPoints.Trim()))
                        product.Bulletpoints1 = BulletPoints;
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

                HtmlNodeCollection imgCollection = _doc.DocumentNode.SelectNodes("//img[@class=\"gallery-image\"]");
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
                else if (_doc.DocumentNode.SelectNodes("//img[@class=\"gallery-image visible\"]") != null)
                {
                    foreach (HtmlNode node in _doc.DocumentNode.SelectNodes("//img[@class=\"gallery-image visible\"]"))
                    {
                        foreach (HtmlAttribute attr in node.Attributes)
                        {
                            if (attr.Name == "src")
                                Images = Images + attr.Value.Trim() + ",";
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
                if (_doc.DocumentNode.SelectNodes("//input[@name=\"product\"]") != null)
                {
                    foreach (HtmlAttribute attr in _doc.DocumentNode.SelectNodes("//input[@name=\"product\"]")[0].Attributes)
                    {
                        if (attr.Name == "value")
                        {
                            product.SKU = "HCKYSUP" + attr.Value.Trim();
                            product.parentsku = "HCKYSUP" + attr.Value.Trim();
                            sku = product.SKU;
                        }
                    }
                }
                else
                    WriteLogEvent(url, "SKU not found");

                #endregion sku

                #region stock
                product.Stock = "0";
                if (_doc.DocumentNode.SelectNodes("//span[@id=\"stock-availability\"]") != null)
                {
                    if (_doc.DocumentNode.SelectNodes("//span[@id=\"stock-availability\"]")[0].InnerText.ToLower() == "in stock")
                        product.Stock = "1";
                }
                #endregion stock
                product.URL = url;


                #region Variation
                bool _isVariantProduct = false;
                bool _isKitProduct = false;
                string attributeScript = "";
                string attributeCombScript = "";
                HtmlNodeCollection collOption = _doc.DocumentNode.SelectNodes("//div[@class=\"product-options\"]");
                if (collOption != null)
                {
                    _isVariantProduct = true;
                    if (collOption[0].SelectNodes(".//label") != null)
                    {

                        if (collOption[0].SelectNodes(".//label").Count > 2)
                            _isKitProduct = true;
                        else
                        {
                            foreach (HtmlNode node in collOption[0].SelectNodes(".//label"))
                            {
                                if (!variationTheme.Contains(node.InnerText.Trim().ToLower()))
                                {
                                    _isKitProduct = true;
                                    break;
                                }
                            }
                        }


                    }
                    else
                    {
                        _isKitProduct = true;
                        WriteLogEvent(url, "option heading Not found, due to which product marked as kit. For more information please check code if(collOption[0].SelectNodes(\".//label\")!=null)");
                    }


                }
                if (!_isKitProduct)
                {
                    HtmlNodeCollection collScript = _doc.DocumentNode.SelectNodes("//script");
                    if (collScript != null)
                    {
                        foreach (HtmlNode scriptNode in collScript)
                        {
                            if (scriptNode.InnerText.ToLower().Contains("spconfig"))
                            {
                                string script = scriptNode.InnerText.ToLower();
                                _isVariantProduct = true;
                                try
                                {
                                    attributeScript = script.Substring(0, script.IndexOf("var allperm")).Replace("var allperm", "").Replace("\"176\":{", "\"skate_width\":{").Replace("\"197\":{", "\"size_skates\":{").Replace("\"92\":{", "\"color\":{").Replace("\"136\":{", "\"size\":{").Replace("\"198\":{", "\"size\":{").Replace("\"199\":{", "\"size\":{").Trim();
                                    attributeScript = attributeScript.Substring(attributeScript.IndexOf("(")).Replace(");", "").Replace("(", "");
                                    attributeCombScript = script.Substring(script.IndexOf("var allperm")).Replace("var allperm", "").Trim();

                                }
                                catch
                                {
                                    _isKitProduct = true;
                                    WriteLogEvent(url, "script tag is not in well format, due to which this product marked as kit");
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        _isKitProduct = true;
                        WriteLogEvent(url, "script tag is not found, due to which product marked as kit");
                    }
                }

                if (!_isKitProduct && !_isVariantProduct)
                    Products.Add(product);
                else if (!_isKitProduct && _isVariantProduct)
                {
                    try
                    {
                        string color = "";
                        string size = "";
                        string stock = "";
                        string saleInfo = "";
                        string price = "";
                        string filterString = "";
                        RootObject deserializedProduct = JsonConvert.DeserializeObject<RootObject>(attributeScript.Trim());
                        List<Option2> option1 = deserializedProduct.attributes.color == null ? deserializedProduct.attributes.skate_width == null ? null : deserializedProduct.attributes.skate_width.options : deserializedProduct.attributes.color.options;
                        List<Option2> option2 = deserializedProduct.attributes.size == null ? deserializedProduct.attributes.size_skates == null ? null : deserializedProduct.attributes.size_skates.options : deserializedProduct.attributes.size.options;
                        int variantCounter = 1;
                        if (option1 != null || option2 != null)
                        {

                            if (option1 == null)
                            {
                                try
                                {
                                    foreach (Option2 sizeOption in option2)
                                    {
                                        bool isStockStringExist = true;
                                        try
                                        {
                                            filterString = attributeCombScript.Substring(attributeCombScript.IndexOf("\"" + sizeOption.id + "\":{")).ToLower();
                                        }
                                        catch
                                        {
                                            isStockStringExist = false;
                                        }
                                        if (isStockStringExist)
                                        {
                                            BusinessLayer.Product sizeProduct = new BusinessLayer.Product();
                                            sizeProduct = (BusinessLayer.Product)product.Clone();
                                            sizeProduct.parentsku = sku + "-parent";
                                            sizeProduct.SKU = sku + "-" + sizeOption.id;
                                            sizeProduct.Size = sizeOption.label;

                                            #region getAvailability

                                            stock = filterString.Substring(filterString.IndexOf("\"availability\"")).Replace("\"availability\"", "");
                                            stock = stock.Substring(0, stock.IndexOf(","));
                                            saleInfo = filterString.Substring(filterString.IndexOf("\"saleinfo\"")).Replace("\"saleinfo\"", "");
                                            saleInfo = saleInfo.Substring(0, saleInfo.IndexOf(","));
                                            if (stock.Contains("normal") && saleInfo.Contains("in stock"))
                                                sizeProduct.Stock = "1";
                                            else
                                                sizeProduct.Stock = "0";

                                            price = filterString.Substring(filterString.IndexOf("\"specialprice\"")).Replace("\"specialprice\"", "");
                                            price = price.Substring(0, price.IndexOf(",")).Replace("$", "");

                                            double specialPrice = 0;
                                            double.TryParse(price, out specialPrice);
                                            if (specialPrice != 0)
                                                sizeProduct.Price = specialPrice.ToString();
                                            else
                                            {
                                     
                                                WriteLogEvent(url, "issue accured in getting price  info for variants");
                                            }
                                            if (variantCounter != 1)
                                                sizeProduct.Isparent = false;
                                            Products.Add(sizeProduct);
                                            #endregion getAvailability
                                            variantCounter++;
                                        }
                                        else
                                        {
                                            WriteLogEvent(url, "Stock string not find for size:" + sizeOption.label);
                                        }
                                    }
                                }
                                catch
                                { WriteLogEvent(url, "issue accured in getting stock etc info for variants"); }

                            }
                            else if (option2 == null)
                            {
                                try
                                {
                                    foreach (Option2 colorOption in option1)
                                    {
                                        bool isStockStringExist = true;
                                        try
                                        {
                                            filterString = attributeCombScript.Substring(attributeCombScript.IndexOf("\"" + colorOption.id + "\":{")).ToLower();
                                        }
                                        catch
                                        {
                                            isStockStringExist = false;
                                        }
                                        if (isStockStringExist)
                                        {
                                            BusinessLayer.Product sizeProduct = new BusinessLayer.Product();
                                            sizeProduct = (BusinessLayer.Product)product.Clone();
                                            sizeProduct.parentsku = sku + "-parent";
                                            sizeProduct.SKU = sku + "-" + colorOption.id;
                                            sizeProduct.Color = colorOption.label;

                                            #region getAvailability

                                            filterString = attributeCombScript.Substring(attributeCombScript.IndexOf("\"" + colorOption.id + "\":{")).ToLower();
                                            stock = filterString.Substring(filterString.IndexOf("\"availability\"")).Replace("\"availability\"", "");
                                            stock = stock.Substring(0, stock.IndexOf(","));
                                            saleInfo = filterString.Substring(filterString.IndexOf("\"saleinfo\"")).Replace("\"saleinfo\"", "");
                                            saleInfo = saleInfo.Substring(0, saleInfo.IndexOf(","));
                                            if (stock.Contains("normal") && saleInfo.Contains("in stock"))
                                                sizeProduct.Stock = "1";
                                            else
                                                sizeProduct.Stock = "0";

                                            price = filterString.Substring(filterString.IndexOf("\"specialprice\"")).Replace("\"specialprice\"", "");
                                            price = price.Substring(0, price.IndexOf(",")).Replace("$", "");

                                            double specialPrice = 0;
                                            double.TryParse(price, out specialPrice);
                                            if (specialPrice != 0)
                                                sizeProduct.Price = specialPrice.ToString();
                                            else
                                            {
                                     
                                                WriteLogEvent(url, "issue accured in getting price  info for variants");
                                            }
                                            if (variantCounter != 1)
                                                sizeProduct.Isparent = false;
                                            Products.Add(sizeProduct);
                                            variantCounter++;
                                        }
                                        else
                                        { WriteLogEvent(url, "Stock string not find for color:" + colorOption.label); }

                                    }
                                            #endregion getAvailability
                                }
                                catch
                                { WriteLogEvent(url, "issue accured in getting stock etc info for variants"); }

                            }
                            else
                            {
                                try
                                {
                                    foreach (Option2 sizeOption in option2)
                                    {
                                        int loopCounter = 1;
                                        string[] cominations = attributeCombScript.Split(new string[] { "\"" + sizeOption.id + "\":{" }, StringSplitOptions.None);
                                        bool jsonFine = false;
                                        try
                                        {
                                            foreach (string comb in cominations)
                                            {
                                                filterString = comb;
                                                if (filterString.Length > 13)
                                                {
                                                    if (loopCounter == 1 && filterString.Contains("="))
                                                    {
                                                    }
                                                    else if (!filterString.Substring(0, 13).Contains("availability") && filterString.Contains("availability"))
                                                    {

                                                        jsonFine = true;
                                                        break;
                                                    }
                                                }
                                                loopCounter++;
                                            }
                                        }
                                        catch
                                        {

                                        }
                                        if (jsonFine)
                                        {
                                            foreach (Option2 colorOption in option1)
                                            {
                                                bool isStockStringExist = true;
                                                string filterString1 = "";
                                                try
                                                {
                                                    filterString1 = filterString.Substring(filterString.IndexOf("\"" + colorOption.id + "\":{")).ToLower();
                                                }
                                                catch
                                                { isStockStringExist = false; }
                                                if (isStockStringExist)
                                                {
                                                    BusinessLayer.Product sizeProduct = new BusinessLayer.Product();
                                                    sizeProduct = (BusinessLayer.Product)product.Clone();
                                                    sizeProduct.parentsku = sku + "-parent";
                                                    sizeProduct.SKU = sku + "-" + sizeOption.id + "-" + colorOption.id;
                                                    sizeProduct.Color = colorOption.label;
                                                    sizeProduct.Size = sizeOption.label;

                                                    #region getAvailability


                                                    stock = filterString1.Substring(filterString1.IndexOf("\"availability\"")).Replace("\"availability\"", "");
                                                    stock = stock.Substring(0, stock.IndexOf(","));
                                                    saleInfo = filterString1.Substring(filterString1.IndexOf("\"saleinfo\"")).Replace("\"saleinfo\"", "");
                                                    saleInfo = saleInfo.Substring(0, saleInfo.IndexOf(","));
                                                    if (stock.Contains("normal") && saleInfo.Contains("in stock"))
                                                        sizeProduct.Stock = "1";
                                                    else
                                                        sizeProduct.Stock = "0";

                                                    price = filterString1.Substring(filterString1.IndexOf("\"specialprice\"")).Replace("\"specialprice\"", "");
                                                    price = price.Substring(0, price.IndexOf(",")).Replace("$", "").Replace(":", "").Replace("\"", "");

                                                    double specialPrice = 0;
                                                    double.TryParse(price, out specialPrice);
                                                    if (specialPrice != 0)
                                                        sizeProduct.Price = specialPrice.ToString();
                                                    else
                                                    {
                                              
                                                        WriteLogEvent(url, "issue accured in getting price  info for variants");
                                                    }
                                                    if (variantCounter != 1)
                                                        sizeProduct.Isparent = false;
                                                    Products.Add(sizeProduct);
                                                    variantCounter++;
                                                }
                                                else
                                                { WriteLogEvent(url, "Stock string not find for color:" + colorOption.label); }

                                                    #endregion getAvailability
                                            }
                                        }
                                        else
                                            WriteLogEvent(url, "Not find size option in json in order to find options stocks, size option label " + sizeOption.label + " size option id " + sizeOption.id);
                                    }
                                }
                                catch
                                { WriteLogEvent(url, "issue accured in getting stock etc info for variants"); }
                            }
                        }
                        else
                            WriteLogEvent(url, "there is no any size and color in deserialize script. Please check this on urgent basis.");
                    }
                    catch (Exception exp)
                    {
                        WriteLogEvent(url, "Issue accured in deserialize script, due to which this product had not sync to db.");
                    }
                }

                #endregion Variation


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

            #region hockeysupremacy.com

            _ISBuy = true;
            _ScrapeUrl = "https://hockeysupremacy.com/";
            try
            {
                _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));
                if (_Work1doc.DocumentNode.SelectNodes("//a[@class=\"level1 view-all\"]") != null)
                {
                    foreach (HtmlNode node in _Work1doc.DocumentNode.SelectNodes("//a[@class=\"level1 view-all\"]"))
                    {
                        foreach (HtmlAttribute _attr in node.Attributes)
                        {
                            if (_attr.Name == "href")
                            {
                                try
                                {
                                    CategoryUrl.Add(_attr.Value.Contains("?") ? _attr.Value + "&limit=all" : _attr.Value + "?limit=all", "HCKYSUP" + node.InnerText.Trim().ToLower().Replace("view all", ""));
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }

                _lblerror.Visible = true;
                _lblerror.Text = "We are going to read category Link for hockeysupremacy.com Website";
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
                        _Prd.ProductDatabaseIntegration(Products, "hockeysupremacy.com", 1);

                    }
                    else
                    {
                        BusinessLayer.DB _Db = new BusinessLayer.DB();
                        _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='hockeysupremacy.com'");
                        _Prd.ProductDatabaseIntegration(Products, "hockeysupremacy.com", 1);
                        _Mail.SendMail("OOPS there is no any product scrapped by app for hockeysupremacy.com Website." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);
                    }
                    #endregion InsertScrappedProductInDatabase
                }
                else
                {
                    BusinessLayer.DB _Db = new BusinessLayer.DB();
                    _Prd.ProductDatabaseIntegration(Products, "hockeysupremacy.com", 1);
                    _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='hockeysupremacy.com'");
                    _lblerror.Text = "Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for hockeysupremacy.com Website";
                    /****************Email****************/
                    _Mail.SendMail("Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for hockeysupremacy.com Website as soon as possible because noscrapping of given store is stopped working." + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);
                    /*******************End********/
                }



            }
            catch
            {
                BusinessLayer.DB _Db = new BusinessLayer.DB();
                _Db.ExecuteCommand("update Schduler set LastProcessedStatus=0 where StoreName='hockeysupremacy.com'");
                _lblerror.Visible = true;
                _Mail.SendMail("Oops Some issue Occured in scrapping data hockeysupremacy.com Website" + DateTime.Now.ToString(), "Urgent issue in Scrapper.", false, false, 1);

            }
            while (_Work.IsBusy || _Work1.IsBusy)
            {
                Application.DoEvents();

            }
            # endregion hockeysupremacy.com
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
