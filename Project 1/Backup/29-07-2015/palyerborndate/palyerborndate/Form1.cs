using System;
using System.Collections.Generic;
using System.ComponentModel;
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
namespace palyerborndate
{
    public partial class Form1 : Form
    {
        BackgroundWorker _Work = new BackgroundWorker();
        BackgroundWorker _Work1= new BackgroundWorker();
        bool _Iscompleted = false;
        List<string> _ProductUrl = new List<string>();
        List<string> _Name = new List<string>();
        List<string> CategoryUrl = new List<string>();
        List<string> SubCategoryUrl = new List<string>();
        bool _ISWarrior = false;
        bool _ISchilychiles = false;
        bool _Isreadywebbrowser1 = false;
        bool _Isreadywebbrowser2 = false;
        bool _IsProduct = false;
        bool _IsAirsoft = false;
        bool _IsKnifezone = false;
        bool _IsLiveoutthere = false;
        int _Chillyindex=0;
        string Url1 = "";
        int _Workindex = 0;
        int _WorkIndex1 = 0;
        List<string> _Url = new List<string>();
        List<string> _dateofbirth = new List<string>();
        string Url2 = "";
        WebClient _Client2 = new WebClient();
        WebClient _Client1 = new WebClient();
        HtmlAgilityPack.HtmlDocument _Work1doc = new HtmlAgilityPack.HtmlDocument();
        HtmlAgilityPack.HtmlDocument _Work1doc2 = new HtmlAgilityPack.HtmlDocument();

        DataTable _Tbale = new DataTable();
        string _ScrapeUrl = "http://www.warriorsandwonders.com/index.php?main_page=advanced_search_result&keyword=keywords&search_in_description=1&product_type=&kfi_blade_length_from=0&kfi_blade_length_to=15&kfi_overall_length_from=0&kfi_overall_length_to=30&kfi_serration=ANY&kfi_is_coated=ANY&kfo_blade_length_from=0&kfo_blade_length_to=8&kfo_overall_length_from=0&kfo_overall_length_to=20&kfo_serration=ANY&kfo_is_coated=ANY&kfo_assisted=ANY&kk_blade_length_from=0&kk_blade_length_to=15&fl_lumens_from=0&fl_lumens_to=18000&fl_num_cells_from=1&fl_num_cells_to=10&fl_num_modes_from=1&fl_num_modes_to=15&sw_blade_length_from=0&sw_blade_length_to=60&sw_overall_length_from=0&sw_overall_length_to=70&inc_subcat=1&pfrom=0.01&pto=10000.00&x=36&y=6&perPage=60";
        
        int _Pages = 0;
        int _TotalRecords = 0;
        int gridindex = 0;
        bool _IsCategory = true;
        bool _Issubcat = false;

        bool _Stop = false;
      
        int time = 0;
        string Bullets = "";
        string _Description = "";
        public Form1()
        {
             InitializeComponent();
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

            /****************Code to select all check boxes*************/
            /************Uncomment durng live
            //for (int i = 0; i < chkstorelist.Items.Count; i++)
            //{
            //    chkstorelist.SetItemChecked(i, true);
            //}
            /********************End*************************************/
            /***************Grid view************************************/
            totalrecord.Visible = false;
            _lblerror.Visible = false;
            createcsvfile.Enabled = false;
            _percent.Visible = false;
            createcsvfile.Enabled = false;
            Pause.Enabled = false;
            dataGridView1.Columns.Add("RowID", "RowID");
            dataGridView1.Columns.Add("SKU", "SKU");
            dataGridView1.Columns.Add("Product Name", "Product Name");
            dataGridView1.Columns.Add("Product Description", "Product Description");
            dataGridView1.Columns.Add("Bullet Points", "Bullet Points");
            dataGridView1.Columns.Add("Manufacturer","Manufacturer");
            dataGridView1.Columns.Add("Brand Name", "Brand Name");
            dataGridView1.Columns.Add("Price", "Price");
            dataGridView1.Columns.Add("Currency", "Currency");
            dataGridView1.Columns.Add("In Stock", "In Stock");
            dataGridView1.Columns.Add("Image URL", "Image URL");
            dataGridView1.Columns.Add("URL", "URL");
           

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        public void Disableallstores()
        {
             _ISWarrior = false;
             _Iscompleted = false;
             _ISchilychiles = false;
             _IsAirsoft = false;
             _IsKnifezone = false;
             _IsLiveoutthere = false;        
        }


        public void work_dowork(object sender, DoWorkEventArgs e)
        {

            bool _Iserror = false;
            try
            {
                if (!_IsProduct)
                {
                    _Work1doc.LoadHtml(_Client1.DownloadString(Url1));
                }

            }
            catch
            {
                _Iserror = true;
            }

            int index = 0;
            #region warrior
            if (_ISWarrior)
            {
                if (_IsCategory)
                {

                    index = gridindex;
                    gridindex++;

                    try
                    {
                        HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//table[@id=\"catTable\"]//tr");
                        if (_Collection != null)
                        {
                            foreach (HtmlNode _Node in _Collection)
                            {
                                if (_Node.Attributes[0].Value.ToLower() == "productlisting-odd" || _Node.Attributes[0].Value.ToLower() == "productlisting-even")
                                {
                                    DataRow _Dr = _Tbale.NewRow();


                                    _Dr[8] = "CDN";

                                    HtmlNodeCollection _Collection1 = _Node.SelectNodes("td");
                                    if (_Collection1 != null)
                                    {

                                        /***************Sku**************/
                                        try
                                        {
                                            _Dr[1] = _Collection1[0].InnerText;
                                        }
                                        catch
                                        {
                                        }
                                        /************End*****************/

                                        /***************product name**************/
                                        try
                                        {
                                            string test = _Collection1[3].SelectNodes("h3")[0].InnerText;
                                            _Dr[2] = _Collection1[3].SelectNodes("h3")[0].InnerText;
                                        }
                                        catch
                                        {
                                        }
                                        /************manufacturer*****************/
                                        try
                                        {
                                            _Dr[5] = _Collection1[1].InnerText;
                                            _Dr[6] = _Collection1[1].InnerText;
                                        }
                                        catch
                                        {
                                        }

                                        /***************Price**************/
                                        try
                                        {
                                            string Price = "";
                                            if (_Collection1[4].SelectNodes("span//p//span[@class=\"productSpecialPrice\"]") != null)
                                            {
                                                Price = _Collection1[4].SelectNodes("span//p//span[@class=\"productSpecialPrice\"]")[0].InnerText;
                                            }
                                            else if (_Collection1[4].SelectNodes("span//p//span[@class=\"productSalePrice\"]") != null)
                                            {
                                                Price = _Collection1[4].SelectNodes("span//p//span[@class=\"productSalePrice\"]")[0].InnerText;
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    Price = _Collection1[4].SelectNodes("span[@class=\"product_list_price\"]")[0].InnerText;
                                                }
                                                catch
                                                {
                                                }
                                            }
                                            Price = Price.Replace("$", "");
                                            Price = Price.ToLower().Replace("price", "").Replace("cdn", "").Trim();
                                            _Dr[7] = Price;

                                        }
                                        catch
                                        {
                                        }

                                        /***************End******************/
                                        /***************In stock**************/
                                        try
                                        {

                                            if (_Collection1[4].InnerText.ToLower().Contains("out of stock"))
                                            {
                                                _Dr[9] = "N";
                                            }
                                            else
                                            {
                                                _Dr[9] = "Y";
                                            }
                                        }
                                        catch
                                        {
                                        }


                                        /**************Image****************/
                                        try
                                        {
                                            if (_Collection1[2].SelectNodes("a//img") != null)
                                            {
                                                _Dr[10] = "http://www.warriorsandwonders.com/" + _Collection1[2].SelectNodes("a//img")[0].Attributes[0].Value;
                                            }
                                        }
                                        catch
                                        {
                                        }

                                        /****************End*****************/

                                        /**************Link**************/
                                        try
                                        {
                                            if (_Collection1[2].SelectNodes("a") != null)
                                            {
                                                _Dr[11] = _Collection1[2].SelectNodes("a")[0].Attributes[0].Value;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        /*****************End*************/


                                    }
                                    _Tbale.Rows.Add(_Dr);
                                }

                            }
                            /***********Sku**************/

                            /**************End*************/
                        }
                    }
                    catch
                    {
                    }


                    /**********Report progress**************/
                    _Work.ReportProgress((gridindex * 100 / _Pages));

                    /****************end*******************/
                }
                else
                {
                    index = gridindex;

                    gridindex++;

                    try
                    {
                        HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"productDescription\"]");
                        if (_Collection != null)
                        {
                            _Description = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"productDescription\"]")[0].InnerHtml;

                            try
                            {

                                List<string> _Remove = new List<string>();
                                foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//div[@id=\"productDescription\"]")[0].ChildNodes)
                                {
                                    if (_Node.InnerText.ToLower().Contains("price:") || _Node.InnerText.ToLower().Contains("msrp:"))
                                    {
                                        _Remove.Add(_Node.InnerHtml);
                                    }

                                }

                                foreach (string _rem in _Remove)
                                {
                                    _Description = _Description.Replace(_rem, "");
                                }
                            }
                            catch
                            {
                            }

                        }
                    }
                    catch
                    {
                        _Description = "";
                    }
                    _Description = StripHTML(_Description);


                    try
                    {
                        HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//ul[@id=\"productDetailsList\"]");
                        if (_Collection != null)
                        {
                            Bullets = StripHTML(_Work1doc.DocumentNode.SelectNodes("//ul[@id=\"productDetailsList\"]")[0].InnerHtml);
                        }
                    }
                    catch
                    {
                        Bullets = "";
                    }
                    _Iscompleted = true;
                    if (_Iserror)
                    {
                        _Description = "";
                        Bullets = "";
                    }
                    _Work.ReportProgress((gridindex * 100 / _Tbale.Rows.Count));

                }
            }
#endregion warrior
            #region chilly
            else if(_ISchilychiles)
            {
                if(_IsCategory)
                {
                    HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"content\"]/div/div/a");
                    if (_Collection != null)
                    {
                        foreach(HtmlNode _Node in _Collection)
                        {
                            foreach(HtmlAttribute _Attr in _Node.Attributes)
                            {
                                if(_Attr.Name.ToLower()=="href")
                                {
                                    _ProductUrl.Add("http://chillychiles.com/" + _Attr.Value);
                                }
                            }
                        }
                    }
                    _Chillyindex++;
                    _Work.ReportProgress((_Chillyindex * 100 / _Pages));
                }
                else
                {
                   _Chillyindex++;
                   _Work.ReportProgress((_Chillyindex * 100 / _ProductUrl.Count()));
                }
            }
            #endregion chilly
            #region aircraft
            else if (_IsAirsoft)
            {
                #region cat
                if (_Issubcat)
                {
                    if (_Work1doc.DocumentNode.SelectNodes("//div[@id=\"catStaticContentLeft\"]") != null)
                    {

                        foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//div[@id=\"catStaticContentLeft\"]/a"))
                        {
                            foreach (HtmlAttribute _Att in _Node.Attributes)
                            {
                                if (_Att.Name == "href")
                                {
                                    if (!SubCategoryUrl.Contains(_Att.Value))
                                    {
                                       
                                            if(_Att.Value.Contains("?"))
                                            {
                                             SubCategoryUrl.Add(_Att.Value+"&limit=all");
                                            }
                                        else
                                            {
                                        SubCategoryUrl.Add(_Att.Value+"?limit=all");
                                            }
                                   }
                                }
                            }
                        }
                    }
                    else
                    {
                        SubCategoryUrl.Add(Url1);
                    }
                    _Chillyindex++;
                    _Work.ReportProgress((_Chillyindex * 100 / CategoryUrl.Count()));

                }
                #endregion cat

                #region subcat
                else if (_IsCategory)
                {
                    if (_Work1doc.DocumentNode.SelectNodes("//a[@class=\"product-image\"]") != null)
                    {
                        string _Url = "";
                        bool _IsproductOurl = false;
                        foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//a[@class=\"product-image\"]"))
                        {
                            _IsproductOurl = false;
                            _Url = "";
                            foreach (HtmlAttribute _Att in _Node.Attributes)
                            {
                                if (_Att.Name.ToLower() == "href")
                                {
                                    _Url = _Att.Value;
                                }
                                else if (_Att.Name.ToLower() == "class" && _Att.Value == "product-image")
                                {
                                    _IsproductOurl = true;
                                }
                                else if (_Att.Name.ToLower() == "title")
                                {
                                   if(_Name.Contains(_Att.Value))
                                   {
                                       _IsproductOurl = false;
                                   }
                                    else
                                   {
                                       _Name.Add(_Att.Value);
                                   }
                                }
                            }

                            if (_IsproductOurl)
                            {
                                if (!_ProductUrl.Contains(_Url))
                                {
                                    _ProductUrl.Add(_Url);
                                }
                            }

                        }
                    }


                    _Chillyindex++;
                    _Work.ReportProgress((_Chillyindex * 100 / SubCategoryUrl.Count()));
                }
                #endregion subcat
                else
                {
                    _Chillyindex++;
                    _Work.ReportProgress((_Chillyindex * 100 / _ProductUrl.Count()));
                }
            }
            #endregion aircraft

            #region Knife
            else if (_IsKnifezone)
            {
                if (_IsCategory)
                {
                    bool Confirm = true;
                    while (Confirm)
                    {
                        Confirm = false;
                        if (_Work1doc.DocumentNode.SelectNodes("//font[@size=\"+1\"]") != null)
                        {
                            foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//font[@size=\"+1\"]/a"))
                            {
                                //if (!_Name.Contains(_Node.InnerText.Trim()))
                                //{
                                    _Name.Add(_Node.InnerText.Trim());
                                    foreach (HtmlAttribute _Att in _Node.Attributes)
                                    {
                                        if (_Att.Name.ToLower() == "href")
                                        {
                                            _ProductUrl.Add("http://www.knifezone.ca/" + _Att.Value.Replace("../", ""));
                                        }
                                    }

                               // }
                            }
                        }
                        if (_Work1doc.DocumentNode.SelectNodes("//img") != null)
                        {

                            foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//img"))
                            {
                                foreach (HtmlAttribute _Att in _Node.Attributes)
                                {
                                    if (_Att.Name.ToLower() == "alt")
                                    {
                                        try
                                        {
                                            if (_Att.Value == "next")
                                            {
                                                HtmlNode _Node2 = _Node.ParentNode;
                                                foreach (HtmlAttribute _Att1 in _Node2.Attributes)
                                                {
                                                    if (_Att1.Name.ToLower() == "href")
                                                    {
                                                        string _Url = Reverse(Url1);
                                                        _Url = _Url.Substring(_Url.IndexOf("/"));
                                                        _Url = Reverse(_Url);
                                                        if (!SubCategoryUrl.Contains(_Url + _Att1.Value))
                                                        {
                                                            SubCategoryUrl.Add(_Url + _Att1.Value);
                                                            _Work1doc.LoadHtml(_Client1.DownloadString(_Url + _Att1.Value));
                                                            Confirm = true;
                                                        }

                                                    }
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
                    }
                    _Chillyindex++;
                    _Work.ReportProgress((_Chillyindex * 100 / CategoryUrl.Count()));
                }
                else
                {
                     _Chillyindex++;
                    _Work.ReportProgress((_Chillyindex * 100 / _ProductUrl.Count()));
                }

            }
            #endregion knife

            //#region liveoutthere
            //else if (_IsLiveoutthere)
            //{
            //    #region category
            //    if (_IsCategory)
            //    {
            //        try
            //        {
            //            if (_Work1doc.DocumentNode.SelectNodes("//span[@class=\"color--orange plp-h1-count\"]") != null)
            //            {
            //                int pages = 0;
            //                int Noofproducts = Convert.ToInt32(_Work1doc.DocumentNode.SelectNodes("//span[@class=\"color--orange plp-h1-count\"]")[0].InnerText.ToLower().Replace("products", "").Trim().Replace(",", ""));
            //                if (Noofproducts % 500 == 0)
            //                {
            //                    pages = Convert.ToInt32(Noofproducts / 500);
            //                }
            //                else
            //                {
            //                    pages = Convert.ToInt32(Noofproducts / 500) + 1;
            //                }

            //                for (int i = 1; i <= pages; i++)
            //                {
            //                    SubCategoryUrl.Add(Url1 + "?n=500&p=" + i);
            //                }
            //            }
            //        }
            //        catch
            //        {
            //        }
            //        _Chillyindex++;
            //        _Work.ReportProgress((_Chillyindex * 100 / CategoryUrl.Count()));
            //    }
            //    #endregion category
            //    #region subcategory
            //    else if (_Issubcat)
            //    {
            //        try
            //        {

            //            if(_Work1doc.DocumentNode.SelectNodes("//div[@class=\"plp-tile-name\"]/a")!=null)
            //            {
            //                foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//div[@class=\"plp-tile-name\"]/a"))
            //                {
            //                    foreach (HtmlAttribute _Att in _Node.Attributes)
            //                    {
            //                        if (_Att.Name == "href")
            //                        {
            //                            _ProductUrl.Add("https://www.liveoutthere.com" + _Att.Value);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        catch
            //        {
            //        }
            //        _Chillyindex++;
            //        _Work.ReportProgress((_Chillyindex * 100 / SubCategoryUrl.Count()));
            //    }
            //    #endregion subcategory
            //}
            //#endregion liveoutthere
        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public void work_RunWorkerAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            #region chilly
            if (_ISchilychiles)
            {
                if (!_IsCategory)
                {
                    int index = 0;
                    index = gridindex;
                    gridindex++;
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = index;
                    dataGridView1.Rows[index].Cells[11].Value = Url1;
                    /*************Title****************/
                    HtmlNodeCollection _Title = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"page-title\"]/h1");
                    if (_Title != null)
                    {
                        dataGridView1.Rows[index].Cells[2].Value = _Title[0].InnerText.Trim();
                        dataGridView1.Rows[index].Cells[1].Value = GenerateSku("CHCH", _Title[0].InnerText.Trim());

                    }
                    else
                    {
                        HtmlNodeCollection _Title1 = _Work1doc.DocumentNode.SelectNodes("//h1");
                        if (_Title1 != null)
                        {
                            dataGridView1.Rows[index].Cells[2].Value = _Title1[0].InnerText.Trim();
                            dataGridView1.Rows[index].Cells[1].Value = GenerateSku("CHCH", _Title1[0].InnerText.Trim());
                        }
                    }
                    /*******************end************/
                    /***************Description***********/
                    HtmlNodeCollection _description = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"product-description\"]");
                    if (_description != null)
                    {
                        string manufacturer = "";
                        List<string> _Remove = new List<string>();
                        foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//div[@id=\"product-description\"]")[0].ChildNodes)
                        {
                            if (!_Node.InnerText.Replace("Manufacturered", "manufactured").ToLower().Contains("manufactured in") && (_Node.InnerText.ToLower().Contains("manufactured") || _Node.InnerText.ToLower().Contains("manufacturer") || _Node.InnerText.ToLower().Contains("brand")))
                            {
                                manufacturer = manufacturer + _Node.InnerText.Trim().Replace("&nbsp;", "").Replace("Â","");
                                _Remove.Add(_Node.InnerHtml);
                                
                            }

                        }
                        if (_Remove.Count() == 0)
                        {
                            foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//div[@id=\"product-description\"]")[0].ChildNodes)
                            {
                                if (_Node.InnerText.ToLower().Contains("brand") )
                                {
                                    manufacturer = _Node.InnerText.Trim().Replace("&nbsp;", ""); ;
                                    _Remove.Add(_Node.InnerHtml);
                                    break;
                                }

                            }
                        }


                            _Description = StripHTML(_description[0].InnerHtml).Trim();
                        
                        try
                        {
                            if (_Description.Length > 2000)
                            {
                                _Description = _Description.Substring(0, 1997) + "...";

                            }
                        }
                        catch
                        {
                        }

                        dataGridView1.Rows[index].Cells[3].Value = _Description.Replace("Â", "");

                        /************Manufacturer**********************/
                       
                        if (manufacturer.Length > 0)
                        {
                            manufacturer = manufacturer.Replace("&nbsp;", "");
                            manufacturer = manufacturer.Replace("Manufacturered", "Manufactured").Replace("Manufacturerd", "Manufactured");
                           
                            if (manufacturer.ToLower().Contains("brand:") && (manufacturer.ToLower().Contains("manufactured") || manufacturer.ToLower().Contains("manufacturer")))
                            {
                                string brand = "";
                                string mantext = "";
                                try
                                {
                                    brand = manufacturer.Substring(manufacturer.ToLower().IndexOf("brand:"));
                                    if (brand.Length > 0)
                                    {
                                        if (brand.ToLower().Contains("manufactured"))
                                        {
                                            brand = brand.Substring(0, brand.ToLower().IndexOf("manufactured")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("For", "").Replace("for", "").Replace("manufacturer", "").Replace("Manufacturer", "").Trim();
                                            dataGridView1.Rows[index].Cells[6].Value = brand.Replace(":", "").Trim();
                                        }
                                        else if (brand.ToLower().Contains("manufacturer"))
                                        {
                                            brand = brand.Substring(0, brand.ToLower().IndexOf("manufacturer")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();
                                            dataGridView1.Rows[index].Cells[6].Value = brand.Replace(":", "").Trim();
                                        }
                                        else
                                        {
                                            brand = brand.Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();
                                            dataGridView1.Rows[index].Cells[6].Value = brand.Replace(":", "").Trim();
                                        }

                                    }
                                    /**********Mantext*******************/
                                    if (manufacturer.ToLower().IndexOf("manufactured") >= 0)
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufactured"));
                                    }
                                    else
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufacturer"));
                                    }

                                    if (mantext.ToLower().Contains("brand"))
                                    {
                                        mantext = mantext.Substring(0, mantext.ToLower().IndexOf("brand")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();


                                        try
                                        {
                                            if (mantext.Length > 25)
                                            {
                                                if (mantext.IndexOf(".") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf("."));
                                                }
                                                if (mantext.Length > 0)
                                                {
                                                    if (mantext.Substring(0, 1) == ":")
                                                    {
                                                        mantext = mantext.Substring(1);
                                                    }
                                                }
                                                if (mantext.IndexOf(":") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf(":"));
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }

                                        dataGridView1.Rows[index].Cells[5].Value = mantext.Replace(":", "").Trim();
                                    }
                                    else
                                    {
                                        mantext = mantext.Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();

                                        try
                                        {
                                            if (mantext.Length > 25)
                                            {
                                                if (mantext.IndexOf(".") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf("."));
                                                }
                                                if (mantext.Length > 0)
                                                {
                                                    if (mantext.Substring(0, 1) == ":")
                                                    {
                                                        mantext = mantext.Substring(1);
                                                    }
                                                }
                                                if (mantext.IndexOf(":") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf(":"));
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }

                                        dataGridView1.Rows[index].Cells[5].Value = mantext.Replace(":", "").Trim();
                                    }
                                    /**************End*****************/
                                    if ((dataGridView1.Rows[index].Cells[5].Value == null || dataGridView1.Rows[index].Cells[5].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[5].Value.ToString())) && (dataGridView1.Rows[index].Cells[6].Value == null || dataGridView1.Rows[index].Cells[6].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[6].Value.ToString())))
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufactured"));
                                        if (mantext.ToLower().Contains("brand"))
                                        {
                                            mantext = mantext.Substring(0, mantext.ToLower().IndexOf("brand")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();

                                        }
                                        else
                                        {
                                            mantext = mantext.Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();

                                        }
                                        try
                                        {
                                            if (mantext.Length > 25)
                                            {
                                                if (mantext.IndexOf(".") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf("."));
                                                }
                                                if (mantext.Length > 0)
                                                {
                                                    if (mantext.Substring(0, 1) == ":")
                                                    {
                                                        mantext = mantext.Substring(1);
                                                    }
                                                }
                                                if (mantext.IndexOf(":") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf(":"));
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }

                                        dataGridView1.Rows[index].Cells[5].Value = mantext.Replace(":", "").Trim();
                                        dataGridView1.Rows[index].Cells[6].Value = mantext.Replace(":", "").Trim();
                                    }
                                    else if (dataGridView1.Rows[index].Cells[5].Value == null || dataGridView1.Rows[index].Cells[5].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[5].Value.ToString()))
                                    {
                                        dataGridView1.Rows[index].Cells[5].Value = brand;
                                    }
                                    else if (dataGridView1.Rows[index].Cells[6].Value == null || dataGridView1.Rows[index].Cells[6].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[6].Value.ToString()))
                                    {
                                        dataGridView1.Rows[index].Cells[6].Value = mantext.Replace(":", "").Trim();
                                    }


                                }
                                catch
                                {
                                    try
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufactured"));
                                    }
                                    catch
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufacturer"));
                                    }
                                    if (mantext.ToLower().Contains("brand"))
                                    {
                                        mantext = mantext.Substring(0, mantext.ToLower().IndexOf("brand")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("For", "").Replace("for", "").Trim();

                                    }
                                    else
                                    {
                                        mantext = mantext.Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();

                                    }

                                    try
                                    {
                                        if (mantext.Length > 25)
                                        {
                                            if (mantext.IndexOf(".") > 0)
                                            {
                                                mantext = mantext.Substring(0, mantext.IndexOf("."));
                                            }
                                            if (mantext.Length > 0)
                                            {
                                                if (mantext.Substring(0, 1) == ":")
                                                {
                                                    mantext = mantext.Substring(1);
                                                }
                                            }
                                            if (mantext.IndexOf(":") > 0)
                                            {
                                                mantext = mantext.Substring(0, mantext.IndexOf(":"));
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    dataGridView1.Rows[index].Cells[5].Value = mantext.Replace(":", "").Trim();
                                    dataGridView1.Rows[index].Cells[6].Value = mantext.Replace(":", "").Trim();

                                }
                            }
                            else
                            {
                                if (manufacturer.ToLower().IndexOf("brand:") >= 0)
                                {
                                    manufacturer = manufacturer.Substring(manufacturer.ToLower().IndexOf("brand:")+6).Trim();
                                    if (manufacturer.Substring(0, 1) == ":")
                                    {
                                        manufacturer = manufacturer.Substring(1);
                                    }
                                    try
                                    {
                                        if (manufacturer.Length > 25)
                                        {
                                            if (manufacturer.IndexOf(".") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf("."));
                                            }
                                            if (manufacturer.Length > 0)
                                            {
                                                if (manufacturer.Substring(0, 1) == ":")
                                                {
                                                    manufacturer = manufacturer.Substring(1);
                                                }
                                            }
                                            if (manufacturer.IndexOf(":") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf(":"));
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    dataGridView1.Rows[index].Cells[5].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured For", "").Replace("Manufactured for", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                    dataGridView1.Rows[index].Cells[6].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured for", "").Replace("Manufactured For", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                

                                }
                                   
                                else if (manufacturer.ToLower().IndexOf("manufactured") >= 0)
                                {
                                    manufacturer = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufactured") + 12).Trim();
                                    if (manufacturer.Substring(0, 1) == ":")
                                    {
                                        manufacturer = manufacturer.Substring(1);
                                    }
                                    try
                                    {
                                        if (manufacturer.Length > 25)
                                        {
                                            if (manufacturer.IndexOf(".") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf("."));
                                            }
                                            if (manufacturer.Length > 0)
                                            {
                                                if (manufacturer.Substring(0, 1) == ":")
                                                {
                                                    manufacturer = manufacturer.Substring(1);
                                                }
                                            }
                                            if (manufacturer.IndexOf(":") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf(":"));
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    dataGridView1.Rows[index].Cells[5].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured For", "").Replace("Manufactured for", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                    dataGridView1.Rows[index].Cells[6].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured for", "").Replace("Manufactured For", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                
                                }
                                else if (manufacturer.ToLower().IndexOf("manufacturer") >= 0)
                                {
                                    manufacturer = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufacturer") + 12).Trim();
                                    if (manufacturer.Substring(0, 1) == ":")
                                    {
                                        manufacturer = manufacturer.Substring(1);
                                    }
                                    try
                                    {
                                        if (manufacturer.Length > 25)
                                        {
                                            if (manufacturer.IndexOf(".") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf("."));
                                            }
                                            if (manufacturer.Length > 0)
                                            {
                                                if (manufacturer.Substring(0, 1) == ":")
                                                {
                                                    manufacturer = manufacturer.Substring(1);
                                                }
                                            }
                                            if (manufacturer.IndexOf(":") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf(":"));
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }


                                    dataGridView1.Rows[index].Cells[5].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured For", "").Replace("Manufactured for", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                    dataGridView1.Rows[index].Cells[6].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured for", "").Replace("Manufactured For", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                }
                                }
                        }
                        try
                        {
                            if (dataGridView1.Rows[index].Cells[6].Value.ToString().Length > 25)
                            {
                                dataGridView1.Rows[index].Cells[6].Value = dataGridView1.Rows[index].Cells[5].Value;
                            }
                        }
                        catch
                        {
                        }
                        /*****************End*****************/
                    }
                    /***************End****************/
                    /*************For decsription empty********************/
                    try
                    {
                        if (dataGridView1.Rows[index].Cells[3].Value == null || dataGridView1.Rows[index].Cells[3].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[3].Value.ToString()))
                        {
                            dataGridView1.Rows[index].Cells[3].Value=dataGridView1.Rows[index].Cells[2].Value;
                        }
                    }
                    catch
                    {
                        }
                    /*********************End*****************/

                    /*************For manufacturer Not sure**********************/
                    try{
                        if (dataGridView1.Rows[index].Cells[5].Value != null || dataGridView1.Rows[index].Cells[5].Value != DBNull.Value || !String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[5].Value.ToString()))
                        {
                            if (dataGridView1.Rows[index].Cells[5].Value.ToString().ToLower().Contains("not sure"))
                            {
                                dataGridView1.Rows[index].Cells[5].Value = "";
                            }
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (dataGridView1.Rows[index].Cells[6].Value != null || dataGridView1.Rows[index].Cells[6].Value != DBNull.Value || !String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[6].Value.ToString()))
                        {
                            if (dataGridView1.Rows[index].Cells[6].Value.ToString().ToLower().Contains("not sure"))
                            {
                                dataGridView1.Rows[index].Cells[6].Value = "";
                            }
                        }
                    }
                    catch
                    {
                    }
                    /***************End******************************************/

                    /*************Currency********************/
                    #region currency
                    dataGridView1.Rows[index].Cells[8].Value = "CDN";
                    #endregion currency

                    /****************End***********************/

                    #region price,stock

                    /***********Instock***********************/

                    if (_Work1doc.DocumentNode.SelectNodes("//form[@action=\"/cart/add\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[9].Value = "Y";

                        /************Price**************************/
                        string price = "";
                        foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//script"))
                        {
                            if (_Node.InnerText.Contains("\"price\""))
                            {
                                price = _Node.InnerText.Substring(_Node.InnerText.ToLower().IndexOf("\"price\""));
                                price = price.Substring(0, price.IndexOf("\","));
                                price = price.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();
                                break;
                            }
                        }
                        dataGridView1.Rows[index].Cells[7].Value = price.Replace(":", "");

                        /***************End**************************/

                    }
                    else
                    {
                        dataGridView1.Rows[index].Cells[9].Value = "N";
                        /************Price**************************/
                        string price = "";
                        foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//script"))
                        {
                            if (_Node.InnerText.Contains("\"price\""))
                            {
                                price = _Node.InnerText.Substring(_Node.InnerText.ToLower().IndexOf("\"price\""));
                                price = price.Substring(0, price.IndexOf("\","));
                                price = price.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();
                                break;
                            }
                        }
                        dataGridView1.Rows[index].Cells[7].Value = price.Replace(":", "");
                        /***************End************************/

                    }
                    /******************end*********************/
                    #endregion price,stock

                    /***********Url******************/

                    /**************end****************/

                    /*************Image Url***************/
                    if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"four columns alpha\"]/img") != null)
                    {
                        foreach (HtmlAttribute _Att in _Work1doc.DocumentNode.SelectNodes("//div[@class=\"four columns alpha\"]/img")[0].Attributes)
                        {
                            if (_Att.Name.ToLower() == "src")
                            {
                                dataGridView1.Rows[index].Cells[10].Value = _Att.Value;
                            }
                        }
                    }


                    /********************end***************/
                }
            }
                        #endregion chilly
            #region airsoft
            else if (_IsAirsoft)
            {

                if (!_IsCategory && !_Issubcat)
                {

                    int index = 0;
                    index = gridindex;
                    gridindex++;
                    dataGridView1.Rows.Add();
                    /*****************rowid**********************/
                    dataGridView1.Rows[index].Cells[0].Value = index;
                    dataGridView1.Rows[index].Cells[11].Value = Url1;
                    /******************End**********************/

                    #region Name
                    /*****************Name**********************/
                    if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"titlebar tl tr\"]/span[@class=\"titlebar-title green left titlebar-product-title h1\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[2].Value = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"titlebar tl tr\"]/span[@class=\"titlebar-title green left titlebar-product-title h1\"]")[0].InnerText.Trim();
                    }
                    /******************End**********************/
                    #endregion Name

                    #region sku
                    /*****************Sku**********************/

                    if (dataGridView1.Rows[index].Cells[2].Value != null || dataGridView1.Rows[index].Cells[2].Value != DBNull.Value || !String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[2].Value.ToString()))
                    {
                        dataGridView1.Rows[index].Cells[1].Value = GenerateSku("BA", dataGridView1.Rows[index].Cells[2].Value.ToString());
                    }
                    /******************End**********************/
                    #endregion sku



                    #region Description
                    /*****************Description**********************/
                    if (_Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab1\"]/div[@class=\"collateral-box bl br\"]") != null)
                    {
                        _Description = StripHTML(_Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab1\"]/div[@class=\"collateral-box bl br\"]")[0].InnerText).Trim();
                    }



                    /*************what will you Material***********************/
                    try
                    {
                        if (_Description.Length < 2000)
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab3\"]/div[@class=\"attribute-specs\"]/ul") != null)
                            {
                                string _material = StripHTML(_Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab3\"]/div[@class=\"attribute-specs\"]/ul")[0].InnerHtml.Replace("<ul>", "").Replace("</ul>", "").Replace("<UL>", "").Replace("<li>", "").Replace("</li>", ",").Replace("</LI>", ",").Replace("<LI>", ""));

                                if (_material.Length > 0)
                                {
                                    if (_material.Substring(_material.Length - 1) == ",")
                                    {
                                        _material = _material.Substring(0, _material.Length - 1);

                                    }
                                    _material = "Material: " + _material;
                                    _Description = _Description + " " + _material.Trim();
                                }
                            }
                        }

                    }
                    catch
                    {
                    }
                    /**************End**************************/


                    /*************what will you need***********************/
                    try
                    {
                        if (_Description.Length < 2000)
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab5\"]/div[@class=\"collateral-box bl br\"]/ul") != null)
                            {
                                string _Need = StripHTML(_Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab5\"]/div[@class=\"collateral-box bl br\"]/ul")[0].InnerHtml.Replace("<ul>", "").Replace("</ul>", "").Replace("<UL>", "").Replace("<li>", "").Replace("</li>", ",").Replace("</LI>", ",").Replace("<LI>", ""));

                                if (_Need.Length > 0)
                                {
                                    if (_Need.Substring(_Need.Length - 1) == ",")
                                    {
                                        _Need = _Need.Substring(0, _Need.Length - 1);

                                    }
                                    _Need = "What you will need: " + _Need;
                                    _Description = _Description + " " + _Need.Trim();
                                }
                            }
                        }

                    }
                    catch
                    {
                    }
                    /**************End**************************/
                    
                    /**********What is in this box***************/
                    try
                    {
                        if(_Description.Length<2000)
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab4\"]/div[@class=\"collateral-box bl br\"]/ul") != null)
                            {
                                string _IsinBox = StripHTML(_Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab4\"]/div[@class=\"collateral-box bl br\"]/ul")[0].InnerHtml.Replace("<ul>", "").Replace("</ul>", "").Replace("<UL>", "").Replace("<li>", "").Replace("</li>", ",").Replace("</LI>", ",").Replace("<LI>", ""));

                                if (_IsinBox.Length > 0)
                                {
                                    if (_IsinBox.Substring(_IsinBox.Length - 1) == ",")
                                    {
                                        _IsinBox = _IsinBox.Substring(0, _IsinBox.Length - 1);

                                    }
                                    _IsinBox = "What's in the Box: " + _IsinBox;
                                    _Description = _Description + " " + _IsinBox.Trim();
                                }
                            }
                        }
                       
                    }
                    catch
                    {
                    }
                    if (_Description.Length > 2000)
                    {
                        _Description = _Description.Substring(0, 1997)+"...";
                    }
                    dataGridView1.Rows[index].Cells[3].Value = _Description;
                    /**************End**************************/
                    /******************End**********************/
                    #endregion Description

                    #region bulletpoints
                    /*****************bulletpoints**********************/
                    if (_Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab2\"]/div[@class=\"attribute-specs\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[4].Value = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"tab2\"]/div[@class=\"attribute-specs\"]")[0].InnerHtml.Trim();
                    }
                    /******************End**********************/
                    #endregion bulletpoints

                    #region Manufacturer
                    /*****************Manufacturer**********************/
                    /******************End**********************/
                    #endregion Manufacturer

                    #region Brandname
                    /*****************Brandname**********************/
                    /******************End**********************/
                    #endregion Brandname

                    #region Price
                    /*****************Price**********************/
                    if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"price-box\"]/span/span[@class=\"price\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[7].Value = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"price-box\"]/span/span[@class=\"price\"]")[0].InnerText.Replace("$", "").Trim();
                    }
                    else
                    {
                        if (_Work1doc.DocumentNode.SelectNodes("//span[@class=\"price\"]") != null)
                        {
                            dataGridView1.Rows[index].Cells[7].Value = _Work1doc.DocumentNode.SelectNodes("//span[@class=\"price\"]")[0].InnerText.Replace("$", "").Trim();
                        }
                    }
                    /******************End**********************/
                    #endregion Price

                    #region Currency
                    /*****************Currency**********************/
                    dataGridView1.Rows[index].Cells[8].Value = "CDN";
                    /******************End**********************/
                    #endregion Currency

                    #region stock
                    /*****************stock**********************/
                    if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"titlebar tl tr\"]/span[@class=\"stock-notification green tl tr bl br right\"]") != null)
                    {
                        if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"titlebar tl tr\"]/span[@class=\"stock-notification green tl tr bl br right\"]")[0].InnerText.ToLower().Trim() == "in stock")
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "Y";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "N";
                        }
                    }
                    else
                    {
                        if (_Work1doc.DocumentNode.SelectNodes("//span[@class=\"stock-notification green tl tr bl br right\"]") != null)
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//span[@class=\"stock-notification green tl tr bl br right\"]")[0].InnerText.ToLower().Trim() == "in stock")
                            {
                                dataGridView1.Rows[index].Cells[9].Value = "Y";
                            }
                            else
                            {
                                dataGridView1.Rows[index].Cells[9].Value = "N";
                            }
                        }
                        else if (_Work1doc.DocumentNode.SelectNodes("//span[@class=\"stock-notification red tl tr bl br right\"]") != null)
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "N";
                        }
                        else if (_Work1doc.DocumentNode.SelectNodes("//span[@class=\"stock-notification yellow tl tr bl br right\"]") != null)
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "Y";
                        }
                    }
                    /******************End**********************/
                    #endregion stock

                    #region Image
                    /*****************Image url**********************/
                    string _ImageUrl = "";
                    if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"main-product-img\"]/img") != null)
                    {
                        HtmlNode _Node = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"main-product-img\"]/img")[0];
                        foreach (HtmlAttribute _Att in _Node.Attributes)
                        {
                            if (_Att.Name.ToLower() == "src")
                            {
                                _ImageUrl = _Att.Value;
                            }
                        }

                        /*********For multiple images*********************/
                        try{
                        if (_Work1doc.DocumentNode.SelectNodes("//script") != null)
                        {

                            foreach (HtmlNode _Node1 in _Work1doc.DocumentNode.SelectNodes("//script"))
                            {
                                string Imagetext = "";
                                if (_Node1.InnerText.ToLower().Contains("media_thumb_2()"))
                                {
                                    Imagetext = _Node1.InnerText.ToLower().Substring(_Node1.InnerText.ToLower().IndexOf("https://www.buyairsoft.ca")).Replace("<img src=", "").Trim() ;
                                    
                                    Imagetext = Imagetext.Substring(0, Imagetext.IndexOf("\"")).Replace("\"", "");
                                    if (Imagetext.Length > 0)
                                    {
                                        if (Imagetext.Substring(Imagetext.Length - 1) == @"\")
                                            Imagetext = Imagetext.Substring(0, Imagetext.Length - 1);
                                    }
                                    _ImageUrl = _ImageUrl + "@" + Imagetext;
                                }
                                if (_Node1.InnerText.ToLower().Contains("media_thumb_3()"))
                                {

                                    Imagetext = _Node1.InnerText.ToLower().Substring(_Node1.InnerText.ToLower().IndexOf("https://www.buyairsoft.ca")).Replace("<img src=", "").Trim();

                                    Imagetext = Imagetext.Substring(0, Imagetext.IndexOf("\"")).Replace("\"", "");
                                    if (Imagetext.Length > 0)
                                    {
                                        if (Imagetext.Substring(Imagetext.Length - 1) == @"\")
                                            Imagetext = Imagetext.Substring(0, Imagetext.Length - 1);
                                    }
                                    _ImageUrl = _ImageUrl + "@" + Imagetext;
                                }
                            }
                        }
                        }
                        catch
                        {
                        }
                        /***************End********************************/

                        if (_ImageUrl.Length > 0)
                        {
                            if (_ImageUrl.Substring(_ImageUrl.Length - 1) == ",")
                            {
                                _ImageUrl = _ImageUrl.Substring(0, _ImageUrl.Length - 1);
                            }
                        }
                        dataGridView1.Rows[index].Cells[10].Value = _ImageUrl;

                    }
                    /******************End**********************/
                    #endregion Image

                    
                }
            }
            #endregion airsoft
            #region knife
            else if (_IsKnifezone)
            {
                if (!_IsCategory)
                {
                    int index = 0;
                    index = gridindex;
                    gridindex++;
                    dataGridView1.Rows.Add();
                    /*****************rowid**********************/
                    dataGridView1.Rows[index].Cells[0].Value = index;
                    dataGridView1.Rows[index].Cells[11].Value = Url1;
                    dataGridView1.Rows[index].Cells[8].Value = "CDN";
                    /******************End**********************/

                    /**********Sku Bullet Points manufacturer brand Name*****************/
                    string Title = "";
                    if (_Work1doc.DocumentNode.SelectNodes("//div/h1") != null)
                    {
                        HtmlNode _Node = _Work1doc.DocumentNode.SelectNodes("//div/h1")[0];
                        Title = _Node.InnerText.Trim();
                        dataGridView1.Rows[index].Cells[2].Value = _Node.InnerText.Trim();

                        Dictionary<string, string> Manskubullet = new Dictionary<string, string>();
                        string test = _Node.XPath.Replace("div[1]/h1[1]", "") + "/table/tr/td/font[@color=\"color\"]";
                        HtmlNodeCollection _Nodecollkey = _Work1doc.DocumentNode.SelectNodes(_Node.XPath.Replace("div[1]/h1[1]", "") + "/table/tr/td/font[@color=\"black\"]");
                        HtmlNodeCollection _Nodecollvalue = _Work1doc.DocumentNode.SelectNodes(_Node.XPath.Replace("div[1]/h1[1]", "") + "/table/tr/td/font[@color=\"blue\"]");
                        if (_Nodecollkey != null)
                        {
                            for (int i = 0; i < _Nodecollkey.Count; i++)
                            {
                                try
                                {
                                    Manskubullet.Add(_Nodecollkey[i].InnerText.Trim(), _Nodecollvalue[i].InnerText.Trim());
                                }
                                catch
                                {
                                }
                                if (Manskubullet.Last().Key.ToLower().Contains("retail") || Manskubullet.Last().Key.ToLower().Contains("price"))
                                    break;
                            }
                        }

                        string Bullets = "<ul>";
                        foreach (var Items in Manskubullet)
                        {


                            if (Items.Key.ToLower().Contains("manufacturer"))
                            {
                                dataGridView1.Rows[index].Cells[5].Value = Items.Value.Trim();
                                dataGridView1.Rows[index].Cells[6].Value = Items.Value.Trim();
                            }
                            else if (Items.Key.ToLower().Contains("name"))
                            {
                                Title = Items.Value.Trim();
                                dataGridView1.Rows[index].Cells[2].Value = Items.Value.Trim();
                            }

                            else if (Items.Key.ToLower().Contains("model"))
                            {
                                dataGridView1.Rows[index].Cells[1].Value = Items.Value.Trim();
                            }
                            else if (Items.Key.ToLower().Contains("retail"))
                            {
                                dataGridView1.Rows[index].Cells[7].Value = Items.Key.ToLower().Replace("retail:", "").Replace("$", "").Trim();
                            }
                            else if (!Items.Key.ToLower().Contains("price"))
                            {
                                Bullets = Bullets + "<li>" + Items.Key + Items.Value + "</li>";
                            }


                        }
                        Bullets = Bullets + "</ul>";

                        if (Bullets.Length > 10)
                        {
                            dataGridView1.Rows[index].Cells[4].Value = Bullets;
                        }


                        /*********Description Images*******************/
                        try
                        {
                            bool _IsImageFind = false;
                            string xpath = "";
                            foreach (HtmlNode _imgNode in _Work1doc.DocumentNode.SelectNodes("//img"))
                            {
                                foreach (HtmlAttribute _Att in _imgNode.Attributes)
                                {
                                    if (_Att.Name.ToLower() == "alt" && _Att.Value.ToLower().Contains(Title.ToLower()))
                                    {
                                        _IsImageFind = true;
                                        xpath = _imgNode.XPath;
                                    }
                                }
                                //if (_IsImageFind)
                                //    break;
                            }

                            //if (xpath.Length == 0)
                            xpath = "/body[1]/table[5]/tr[1]/td[2]/table[1]/tr[1]/td[2]/table[1]/tr[1]/td[2]/img[1]";

                            try
                            {
                                if (xpath.Length > 0)
                                {
                                    HtmlNode _imgNode = _Work1doc.DocumentNode.SelectNodes(xpath)[0];
                                    xpath = "";
                                    foreach (HtmlAttribute _Att in _imgNode.Attributes)
                                    {
                                        if (_Att.Name.ToLower() == "src" || _Att.Name.ToLower() == "data-cfsrc")
                                        {
                                            dataGridView1.Rows[index].Cells[10].Value = _Att.Value.Trim();
                                            xpath = _imgNode.XPath.Replace("/img[1]", "");
                                        }
                                    }



                                }

                                #region description
                                if (xpath.Length > 0)
                                {
                                    try
                                    {
                                        _Description = _Work1doc.DocumentNode.SelectNodes(xpath + "/font[@color=\"black\"]")[0].InnerText.Trim();
                                        if (_Description.Length > 2000)
                                        {
                                            dataGridView1.Rows[index].Cells[3].Value = _Description.Substring(0, 1997) + "...";
                                        }
                                        else
                                        {
                                            dataGridView1.Rows[index].Cells[3].Value = _Description;
                                        }

                                    }
                                    catch
                                    {
                                    }
                                }
                                #endregion decsription
                            }
                            catch
                            {
                            }
                        }
                        catch
                        {
                        }



                        /*****************End*******************/

                        /***************Price************************/

                        if (dataGridView1.Rows[index].Cells[1].Value == null || dataGridView1.Rows[index].Cells[1].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[1].Value.ToString()))
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"identifier\"]") != null)
                            {
                                dataGridView1.Rows[index].Cells[1].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"identifier\"]")[0].InnerText.Trim();
                            }
                        }

                        if (dataGridView1.Rows[index].Cells[5].Value == null || dataGridView1.Rows[index].Cells[5].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[5].Value.ToString()))
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]") != null)
                            {
                                dataGridView1.Rows[index].Cells[5].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]")[0].InnerText.Trim();
                                dataGridView1.Rows[index].Cells[6].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]")[0].InnerText.Trim();
                            }
                        }


                        if (dataGridView1.Rows[index].Cells[7].Value == null || dataGridView1.Rows[index].Cells[7].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[7].Value.ToString()))
                        {
                            if (_Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]") != null)
                            {
                                dataGridView1.Rows[index].Cells[7].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]")[0].InnerText.Trim();
                            }
                        }
                        /**********************End********************/

                        #region stock
                        if (_Work1doc.DocumentNode.InnerHtml.ToLower().Contains("temporarily unavailable"))
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "N";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "Y";
                        }
                        #endregion stock

                    }
                    else
                    {


                        if (_Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"name\"]") != null)
                            {
                             /***************Sku***************/
                                try
                                {
                                    dataGridView1.Rows[index].Cells[1].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"identifier\"]")[0].InnerText;
                                }
                                catch
                                {
                                }
                            /****************End*****************/


                                /***************Name***************/
                                try
                                {
                                    dataGridView1.Rows[index].Cells[2].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"name\"]")[0].InnerText;
                                }
                                catch
                                {
                                }
                                /****************End*****************/


                                /***************Description***************/
                                try
                                {
                                    string Xpath = "/body[1]/table[2]/tr[1]/td[2]/font[1]";
                                    _Description = _Work1doc.DocumentNode.SelectNodes(Xpath)[0].InnerText;

                                    if (_Description.Length > 2000)
                                    {
                                        dataGridView1.Rows[index].Cells[3].Value = _Description.Substring(0, 1997) + "...";
                                    }
                                    else
                                    {
                                        dataGridView1.Rows[index].Cells[3].Value = _Description;
                                    }
                                }
                                catch
                                {
                                }
                                /****************End*****************/


                                /***************Bullepoint***************/
                                //try
                                //{
                                //    dataGridView1.Rows[index].Cells[2].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"name\"]")[0].InnerText;
                                //}
                                //catch
                                //{
                                //}
                                /****************End*****************/


                                /***************Brand***************/
                                try
                                {
                                    dataGridView1.Rows[index].Cells[5].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]")[0].InnerText;
                                    dataGridView1.Rows[index].Cells[6].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]")[0].InnerText;
                                }
                                catch
                                {
                                    try
                                    {
                                        dataGridView1.Rows[index].Cells[5].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"manufacturer\"]")[0].InnerText;
                                        dataGridView1.Rows[index].Cells[6].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"manufacturer\"]")[0].InnerText;
                                    }
                                    catch
                                    {
                                    }
                                }
                                /****************End*****************/

                            /****************Price********************/
                                foreach (HtmlNode _priceNode in _Work1doc.DocumentNode.SelectNodes("//font"))
                                {
                                    if (_priceNode.InnerText.ToLower().Contains("retail"))
                                    {
                                        dataGridView1.Rows[index].Cells[7].Value = _priceNode.InnerText.ToLower().Replace("retail", "").Replace(":", "").Replace("$", "").Trim();
                                    }
                                }
                            /***************End***********************/

                                /****************Stock********************/
                                if (_Work1doc.DocumentNode.InnerHtml.ToLower().Contains("temporarily unavailable"))
                                {
                                    dataGridView1.Rows[index].Cells[9].Value = "N";
                                }
                                else
                                {
                                    dataGridView1.Rows[index].Cells[9].Value = "Y";
                                }
                                /***************End***********************/


                            /***************Image*******************/
                                try
                                {
                                    string Xpath = "/body[1]/table[2]/tr[1]/td[1]/img[1]";

                                    foreach (HtmlAttribute _Att in _Work1doc.DocumentNode.SelectNodes(Xpath)[0].Attributes)
                                    {
                                        if (_Att.Name.ToLower() == "src" || _Att.Name.ToLower() == "data-cfsrc")
                                        {
                                            dataGridView1.Rows[index].Cells[10].Value = _Att.Value;
                                        }
                                    }

                                }
                                catch
                                {
                                }

                            /********************End**********************************/

                            }
                        

                    }


                    /*******************End****************************************/

                }
            }
            #endregion knife
            //#region liveoutthere
            //else if (_IsLiveoutthere)
            //{
            //    if (_IsProduct)
            //    {
            //        webBrowser1.Navigate(Url1);
            //        while (!_Isreadywebbrowser1)
            //        {
            //            Application.DoEvents();
            //        }

            //        #region codecolorsize
            //        foreach (HtmlElement element in webBrowser1.Document.GetElementsByTagName("div"))
            //        {
            //            string temp = element.GetAttribute("class");

            //            if (temp.Equals("size ng-scope") == true)
            //            {

            //                element.Focus();
            //                element.InvokeMember("click");
                           




            //            }
            //        }
            //        #endregion

            //    }
            //}
            //#endregion liveoutthere
        }


        //private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
            
        //    _Work1doc.LoadHtml(_Client1.DownloadString(Url1));

        //    while (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"size ng-scope\"]") == null)
        //    {
        //        Application.DoEvents();
        //        _Work1doc.LoadHtml(webBrowser1.DocumentText.ToString());
        //        tim(2);
        //    }
        //    _Isreadywebbrowser1 = true;
        //}
        public void work_dowork1(object sender, DoWorkEventArgs e)
        {
            _Work1doc2.LoadHtml(_Client2.DownloadString(Url2));

            
            int index = 0;
            #region warrior
            if (_ISWarrior)
            {
                if (_IsCategory)
                {

                    index = gridindex;
                    gridindex++;

                    try
                    {
                        HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//table[@id=\"catTable\"]//tr");
                        if (_Collection != null)
                        {
                            foreach (HtmlNode _Node in _Collection)
                            {
                                if (_Node.Attributes[0].Value.ToLower() == "productlisting-odd" || _Node.Attributes[0].Value.ToLower() == "productlisting-even")
                                {
                                    DataRow _Dr = _Tbale.NewRow();


                                    _Dr[8] = "CDN";

                                    HtmlNodeCollection _Collection1 = _Node.SelectNodes("td");
                                    if (_Collection1 != null)
                                    {

                                        /***************Sku**************/
                                        try
                                        {
                                            _Dr[1] = _Collection1[0].InnerText;
                                        }
                                        catch
                                        {
                                        }
                                        /************End*****************/

                                        /***************product name**************/
                                        try
                                        {
                                            string test = _Collection1[3].SelectNodes("h3")[0].InnerText;
                                            _Dr[2] = _Collection1[3].SelectNodes("h3")[0].InnerText;
                                        }
                                        catch
                                        {
                                        }
                                        /************manufacturer*****************/
                                        try
                                        {
                                            _Dr[5] = _Collection1[1].InnerText;
                                            _Dr[6] = _Collection1[1].InnerText;
                                        }
                                        catch
                                        {
                                        }

                                        /***************Price**************/
                                        try
                                        {
                                            string Price = "";
                                            if (_Collection1[4].SelectNodes("span//p//span[@class=\"productSpecialPrice\"]") != null)
                                            {
                                                Price = _Collection1[4].SelectNodes("span//p//span[@class=\"productSpecialPrice\"]")[0].InnerText;
                                            }
                                            else if (_Collection1[4].SelectNodes("span//p//span[@class=\"productSalePrice\"]") != null)
                                            {
                                                Price = _Collection1[4].SelectNodes("span//p//span[@class=\"productSalePrice\"]")[0].InnerText;
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    Price = _Collection1[4].SelectNodes("span[@class=\"product_list_price\"]")[0].InnerText;
                                                }
                                                catch
                                                {
                                                }
                                            }
                                            Price = Price.Replace("$", "");
                                            Price = Price.ToLower().Replace("price", "").Replace("cdn", "").Trim();
                                            _Dr[7] = Price;

                                        }
                                        catch
                                        {
                                        }

                                        /***************End******************/
                                        /***************In stock**************/
                                        try
                                        {

                                            if (_Collection1[4].InnerText.ToLower().Contains("out of stock"))
                                            {
                                                _Dr[9] = "N";
                                            }
                                            else
                                            {
                                                _Dr[9] = "Y";
                                            }
                                        }
                                        catch
                                        {
                                        }


                                        /**************Image****************/
                                        try
                                        {
                                            if (_Collection1[2].SelectNodes("a//img") != null)
                                            {
                                                _Dr[10] = "http://www.warriorsandwonders.com/" + _Collection1[2].SelectNodes("a//img")[0].Attributes[0].Value;
                                            }
                                        }
                                        catch
                                        {
                                        }

                                        /****************End*****************/

                                        /**************Link**************/
                                        try
                                        {
                                            if (_Collection1[2].SelectNodes("a") != null)
                                            {
                                                _Dr[11] = _Collection1[2].SelectNodes("a")[0].Attributes[0].Value;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        /*****************End*************/


                                    }
                                    _Tbale.Rows.Add(_Dr);
                                }

                            }
                            /***********Sku**************/

                            /**************End*************/
                        }
                    }
                    catch
                    {
                    }


                    /**********Report progress**************/
                    _Work1.ReportProgress((gridindex * 100 / _Pages));

                    /****************end*******************/
                }
                else
                {
                }
            }
            #endregion warrior

            #region chilly
            else if(_ISchilychiles)
            {
                if (_IsCategory)
                {
                    HtmlNodeCollection _Collection = _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"content\"]/div/div/a");
                    if (_Collection != null)
                    {
                        foreach (HtmlNode _Node in _Collection)
                        {
                            foreach (HtmlAttribute _Attr in _Node.Attributes)
                            {
                                if (_Attr.Name.ToLower() == "href")
                                {
                                    _ProductUrl.Add("http://chillychiles.com/" + _Attr.Value);
                                }
                            }
                        }
                    }
                    _Chillyindex++;
                    _Work1.ReportProgress((_Chillyindex * 100 / _Pages));
                }
                else
                {
                    _Chillyindex++;
                    _Work1.ReportProgress((_Chillyindex * 100 / _ProductUrl.Count()));
                }
            }
            #endregion chilly
            #region aircraft
            else if (_IsAirsoft)
            {
                #region cat
                if (_Issubcat)
                {
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"catStaticContentLeft\"]") != null)
                    {

                        foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"catStaticContentLeft\"]/a"))
                        {
                            foreach (HtmlAttribute _Att in _Node.Attributes)
                            {
                                if (_Att.Name == "href")
                                {
                                    if (!SubCategoryUrl.Contains(_Att.Value))
                                    {

                                        if (_Att.Value.Contains("?"))
                                        {
                                            SubCategoryUrl.Add(_Att.Value + "&limit=all");
                                        }
                                        else
                                        {
                                            SubCategoryUrl.Add(_Att.Value + "?limit=all");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SubCategoryUrl.Add(Url1);
                    }
                    _Chillyindex++;
                    _Work1.ReportProgress((_Chillyindex * 100 / CategoryUrl.Count()));

                }
                #endregion cat

                #region subcat
                else if (_IsCategory)
                {
                    if (_Work1doc2.DocumentNode.SelectNodes("//a[@class=\"product-image\"]") != null)
                    {
                        string _Url = "";
                        bool _IsproductOurl = false;
                        foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//a[@class=\"product-image\"]"))
                        {
                            _IsproductOurl = false;
                            _Url = "";
                            foreach (HtmlAttribute _Att in _Node.Attributes)
                            {
                                if (_Att.Name.ToLower() == "href")
                                {
                                    _Url = _Att.Value;
                                }
                                else if (_Att.Name.ToLower() == "class" && _Att.Value == "product-image")
                                {
                                    _IsproductOurl = true;
                                }
                                else if (_Att.Name.ToLower() == "title")
                                {
                                    if (_Name.Contains(_Att.Value))
                                    {
                                        _IsproductOurl = false;
                                    }
                                    else
                                    {
                                        _Name.Add(_Att.Value);
                                    }
                                }
                            }

                            if (_IsproductOurl)
                            {
                                if (!_ProductUrl.Contains(_Url))
                                {
                                    _ProductUrl.Add(_Url);
                                }
                            }

                        }
                    }


                    _Chillyindex++;
                    _Work1.ReportProgress((_Chillyindex * 100 / SubCategoryUrl.Count()));
                }
                #endregion subcat
                #region product
                else
                {
                    _Chillyindex++;
                    _Work1.ReportProgress((_Chillyindex * 100 / _ProductUrl.Count()));
                }
                #endregion product
            }
            #endregion aircraft

            #region Knife
            else if (_IsKnifezone)
            {
                if (_IsCategory)
                {
                    bool Confirm = true;
                    while (Confirm)
                    {
                        Confirm = false;
                        if (_Work1doc2.DocumentNode.SelectNodes("//font[@size=\"+1\"]") != null)
                        {
                            foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//font[@size=\"+1\"]/a"))
                            {
                                //if (!_Name.Contains(_Node.InnerText.Trim()))
                                //{
                                    _Name.Add(_Node.InnerText.Trim());
                                    foreach (HtmlAttribute _Att in _Node.Attributes)
                                    {
                                        if (_Att.Name.ToLower() == "href")
                                        {
                                            _ProductUrl.Add("http://www.knifezone.ca/" + _Att.Value.Replace("../", ""));
                                        }
                                    }

                                //}
                            }
                        }
                        if (_Work1doc2.DocumentNode.SelectNodes("//img") != null)
                        {

                            foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//img"))
                            {
                                foreach (HtmlAttribute _Att in _Node.Attributes)
                                {
                                    if (_Att.Name.ToLower() == "alt")
                                    {
                                        try
                                        {
                                            if (_Att.Value == "next")
                                            {
                                                HtmlNode _Node2 = _Node.ParentNode;
                                                foreach (HtmlAttribute _Att1 in _Node2.Attributes)
                                                {
                                                    if (_Att1.Name.ToLower() == "href")
                                                    {
                                                        string _Url = Reverse(Url2);
                                                        _Url = _Url.Substring(_Url.IndexOf("/"));
                                                        _Url = Reverse(_Url);
                                                        if (!SubCategoryUrl.Contains(_Url + _Att1.Value))
                                                        {
                                                            SubCategoryUrl.Add(_Url + _Att1.Value);
                                                            _Work1doc2.LoadHtml(_Client2.DownloadString(_Url + _Att1.Value));
                                                            Confirm = true;
                                                        }

                                                    }
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
                    }
                    _Chillyindex++;
                    _Work1.ReportProgress((_Chillyindex * 100 / CategoryUrl.Count()));
                }
                else
                {
                    _Chillyindex++;
                    _Work1.ReportProgress((_Chillyindex * 100 / _ProductUrl.Count()));
                }

            }
            #endregion knife

        }

        public void work_RunWorkerAsync1(object sender, RunWorkerCompletedEventArgs e)
        {
            #region chilly
            if (_ISchilychiles)
            {
                if (!_IsCategory)
                {
                    int index = 0;
                    index = gridindex;
                    gridindex++;
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = index;
                    dataGridView1.Rows[index].Cells[11].Value = Url2;
                    /*************Title****************/
                    HtmlNodeCollection _Title = _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"page-title\"]/h1");
                    if (_Title != null)
                    {
                        dataGridView1.Rows[index].Cells[2].Value = _Title[0].InnerText.Trim();
                        dataGridView1.Rows[index].Cells[1].Value = GenerateSku("CHCH", _Title[0].InnerText.Trim());

                    }
                    else
                    {
                        HtmlNodeCollection _Title1 = _Work1doc2.DocumentNode.SelectNodes("//h1");
                        if (_Title1 != null)
                        {
                            dataGridView1.Rows[index].Cells[2].Value = _Title1[0].InnerText.Trim();
                            dataGridView1.Rows[index].Cells[1].Value = GenerateSku("CHCH", _Title1[0].InnerText.Trim());
                        }
                    }
                    /*******************end************/

                    /***************Description***********/
                    HtmlNodeCollection _description = _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"product-description\"]");
                    if (_description != null)
                    {
                        string manufacturer = "";
                        List<string> _Remove = new List<string>();
                        foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"product-description\"]")[0].ChildNodes)
                        {
                            if (!_Node.InnerText.Replace("Manufacturered", "manufactured").ToLower().Contains("manufactured in") && (_Node.InnerText.ToLower().Contains("manufactured") || _Node.InnerText.ToLower().Contains("manufacturer") || _Node.InnerText.ToLower().Contains("brand")))
                            {
                                manufacturer = manufacturer + _Node.InnerText.Trim().Replace("&nbsp;", "").Replace("Â", "");
                                _Remove.Add(_Node.InnerHtml);

                            }

                        }
                        if (_Remove.Count() == 0)
                        {
                            foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"product-description\"]")[0].ChildNodes)
                            {
                                if (_Node.InnerText.ToLower().Contains("brand"))
                                {
                                    manufacturer = _Node.InnerText.Trim().Replace("&nbsp;", "").Replace("Â", "");
                                    _Remove.Add(_Node.InnerHtml);
                                    break;
                                }

                            }
                        }


                        _Description = StripHTML(_description[0].InnerHtml).Trim();

                        try
                        {
                            if (_Description.Length > 2000)
                            {
                                _Description = _Description.Substring(0, 1997) + "...";

                            }
                        }
                        catch
                        {
                        }

                        dataGridView1.Rows[index].Cells[3].Value = _Description.Replace("Â", "");

                        /************Manufacturer**********************/

                        if (manufacturer.Length > 0)
                        {
                            manufacturer = manufacturer.Replace("&nbsp;", "");
                            manufacturer = manufacturer.Replace("Manufacturered", "Manufactured").Replace("Manufacturerd", "Manufactured");

                            if (manufacturer.ToLower().Contains("brand:") && (manufacturer.ToLower().Contains("manufactured") || manufacturer.ToLower().Contains("manufacturer")))
                            {
                                string brand = "";
                                string mantext = "";
                                try
                                {
                                    brand = manufacturer.Substring(manufacturer.ToLower().IndexOf("brand:"));
                                    if (brand.Length > 0)
                                    {
                                        if (brand.ToLower().Contains("manufactured"))
                                        {
                                            brand = brand.Substring(0, brand.ToLower().IndexOf("manufactured")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("For", "").Replace("for", "").Replace("manufacturer", "").Replace("Manufacturer", "").Trim();
                                            dataGridView1.Rows[index].Cells[6].Value = brand.Replace(":", "").Trim();
                                        }
                                        else if (brand.ToLower().Contains("manufacturer"))
                                        {
                                            brand = brand.Substring(0, brand.ToLower().IndexOf("manufacturer")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();
                                            dataGridView1.Rows[index].Cells[6].Value = brand.Replace(":", "").Trim();
                                        }
                                        else
                                        {
                                            brand = brand.Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();
                                            dataGridView1.Rows[index].Cells[6].Value = brand.Replace(":", "").Trim();
                                        }

                                    }
                                    /**********Mantext*******************/
                                    if (manufacturer.ToLower().IndexOf("manufactured") >= 0)
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufactured"));
                                    }
                                    else
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufacturer"));
                                    }

                                    if (mantext.ToLower().Contains("brand"))
                                    {
                                        mantext = mantext.Substring(0, mantext.ToLower().IndexOf("brand")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();


                                        try
                                        {
                                            if (mantext.Length > 25)
                                            {
                                                if (mantext.IndexOf(".") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf("."));
                                                }
                                                if (mantext.Length > 0)
                                                {
                                                    if (mantext.Substring(0, 1) == ":")
                                                    {
                                                        mantext = mantext.Substring(1);
                                                    }
                                                }
                                                if (mantext.IndexOf(":") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf(":"));
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }

                                        dataGridView1.Rows[index].Cells[5].Value = mantext.Replace(":", "").Trim();
                                    }
                                    else
                                    {
                                        mantext = mantext.Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();

                                        try
                                        {
                                            if (mantext.Length > 25)
                                            {
                                                if (mantext.IndexOf(".") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf("."));
                                                }
                                                if (mantext.Length > 0)
                                                {
                                                    if (mantext.Substring(0, 1) == ":")
                                                    {
                                                        mantext = mantext.Substring(1);
                                                    }
                                                }
                                                if (mantext.IndexOf(":") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf(":"));
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }

                                        dataGridView1.Rows[index].Cells[5].Value = mantext.Replace(":", "").Trim();
                                    }
                                    /**************End*****************/
                                    if ((dataGridView1.Rows[index].Cells[5].Value == null || dataGridView1.Rows[index].Cells[5].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[5].Value.ToString())) && (dataGridView1.Rows[index].Cells[6].Value == null || dataGridView1.Rows[index].Cells[6].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[6].Value.ToString())))
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufactured"));
                                        if (mantext.ToLower().Contains("brand"))
                                        {
                                            mantext = mantext.Substring(0, mantext.ToLower().IndexOf("brand")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();

                                        }
                                        else
                                        {
                                            mantext = mantext.Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();

                                        }
                                        try
                                        {
                                            if (mantext.Length > 25)
                                            {
                                                if (mantext.IndexOf(".") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf("."));
                                                }
                                                if (mantext.Length > 0)
                                                {
                                                    if (mantext.Substring(0, 1) == ":")
                                                    {
                                                        mantext = mantext.Substring(1);
                                                    }
                                                }
                                                if (mantext.IndexOf(":") > 0)
                                                {
                                                    mantext = mantext.Substring(0, mantext.IndexOf(":"));
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }

                                        dataGridView1.Rows[index].Cells[5].Value = mantext.Replace(":", "").Trim();
                                        dataGridView1.Rows[index].Cells[6].Value = mantext.Replace(":", "").Trim();
                                    }
                                    else if (dataGridView1.Rows[index].Cells[5].Value == null || dataGridView1.Rows[index].Cells[5].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[5].Value.ToString()))
                                    {
                                        dataGridView1.Rows[index].Cells[5].Value = brand;
                                    }
                                    else if (dataGridView1.Rows[index].Cells[6].Value == null || dataGridView1.Rows[index].Cells[6].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[6].Value.ToString()))
                                    {
                                        dataGridView1.Rows[index].Cells[6].Value = mantext.Replace(":", "").Trim();
                                    }


                                }
                                catch
                                {
                                    try
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufactured"));
                                    }
                                    catch
                                    {
                                        mantext = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufacturer"));
                                    }
                                    if (mantext.ToLower().Contains("brand"))
                                    {
                                        mantext = mantext.Substring(0, mantext.ToLower().IndexOf("brand")).Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("For", "").Replace("for", "").Trim();

                                    }
                                    else
                                    {
                                        mantext = mantext.Replace("Brand", "").Replace("Manufactured", "").Replace("manufactured", "").Replace("brand", "").Replace("By", "").Replace("by", "").Replace("manufacturer", "").Replace("Manufacturer", "").Replace("For", "").Replace("for", "").Trim();

                                    }

                                    try
                                    {
                                        if (mantext.Length > 25)
                                        {
                                            if (mantext.IndexOf(".") > 0)
                                            {
                                                mantext = mantext.Substring(0, mantext.IndexOf("."));
                                            }
                                            if (mantext.Length > 0)
                                            {
                                                if (mantext.Substring(0, 1) == ":")
                                                {
                                                    mantext = mantext.Substring(1);
                                                }
                                            }
                                            if (mantext.IndexOf(":") > 0)
                                            {
                                                mantext = mantext.Substring(0, mantext.IndexOf(":"));
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    dataGridView1.Rows[index].Cells[5].Value = mantext.Replace(":", "").Trim();
                                    dataGridView1.Rows[index].Cells[6].Value = mantext.Replace(":", "").Trim();

                                }
                            }
                            else
                            {
                                if (manufacturer.ToLower().IndexOf("brand:") >= 0)
                                {
                                    manufacturer = manufacturer.Substring(manufacturer.ToLower().IndexOf("brand:") + 6).Trim();
                                    if (manufacturer.Substring(0, 1) == ":")
                                    {
                                        manufacturer = manufacturer.Substring(1);
                                    }
                                    try
                                    {
                                        if (manufacturer.Length > 25)
                                        {
                                            if (manufacturer.IndexOf(".") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf("."));
                                            }
                                            if (manufacturer.Length > 0)
                                            {
                                                if (manufacturer.Substring(0, 1) == ":")
                                                {
                                                    manufacturer = manufacturer.Substring(1);
                                                }
                                            }
                                            if (manufacturer.IndexOf(":") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf(":"));
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    dataGridView1.Rows[index].Cells[5].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured For", "").Replace("Manufactured for", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                    dataGridView1.Rows[index].Cells[6].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured for", "").Replace("Manufactured For", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();


                                }
                                else if (manufacturer.ToLower().IndexOf("manufactured") >= 0)
                                {
                                    manufacturer = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufactured") + 12).Trim();
                                    if (manufacturer.Substring(0, 1) == ":")
                                    {
                                        manufacturer = manufacturer.Substring(1);
                                    }
                                    try
                                    {
                                        if (manufacturer.Length > 25)
                                        {
                                            if (manufacturer.IndexOf(".") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf("."));
                                            }
                                            if (manufacturer.Length > 0)
                                            {
                                                if (manufacturer.Substring(0, 1) == ":")
                                                {
                                                    manufacturer = manufacturer.Substring(1);
                                                }
                                            }
                                            if (manufacturer.IndexOf(":") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf(":"));
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    dataGridView1.Rows[index].Cells[5].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured For", "").Replace("Manufactured for", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                    dataGridView1.Rows[index].Cells[6].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured for", "").Replace("Manufactured For", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();

                                }
                                else if (manufacturer.ToLower().IndexOf("manufacturer") >= 0)
                                {
                                    manufacturer = manufacturer.Substring(manufacturer.ToLower().IndexOf("manufacturer") + 12).Trim();
                                    if (manufacturer.Substring(0, 1) == ":")
                                    {
                                        manufacturer = manufacturer.Substring(1);
                                    }
                                    try
                                    {
                                        if (manufacturer.Length > 25)
                                        {
                                            if (manufacturer.IndexOf(".") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf("."));
                                            }
                                            if (manufacturer.Length > 0)
                                            {
                                                if (manufacturer.Substring(0, 1) == ":")
                                                {
                                                    manufacturer = manufacturer.Substring(1);
                                                }
                                            }
                                            if (manufacturer.IndexOf(":") > 0)
                                            {
                                                manufacturer = manufacturer.Substring(0, manufacturer.IndexOf(":"));
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }


                                    dataGridView1.Rows[index].Cells[5].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured For", "").Replace("Manufactured for", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                    dataGridView1.Rows[index].Cells[6].Value = manufacturer.Replace("Manufactured by", "").Replace("by", "").Replace("for", "").Replace("Manufactured for", "").Replace("Manufactured For", "").Replace("Manufacturer", "").Replace("Manufactured By", "").Replace(":", "").Replace("&nbsp;", "").Trim();
                                }
                            }
                        }
                        try
                        {
                            if (dataGridView1.Rows[index].Cells[6].Value.ToString().Length > 25)
                            {
                                dataGridView1.Rows[index].Cells[6].Value = dataGridView1.Rows[index].Cells[5].Value;
                            }
                        }
                        catch
                        {
                        }
                        /*****************End*****************/
                    }
                    /***************End****************/

/*************For decsription empty********************/
                    try
                    {
                        if (dataGridView1.Rows[index].Cells[3].Value == null || dataGridView1.Rows[index].Cells[3].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[3].Value.ToString()))
                        {
                            dataGridView1.Rows[index].Cells[3].Value=dataGridView1.Rows[index].Cells[2].Value;
                        }
                    }
                    catch
                    {
                        }
                    /*********************End*****************/

                    /*************For manufacturer Not sure**********************/
                    try{
                        if (dataGridView1.Rows[index].Cells[5].Value != null || dataGridView1.Rows[index].Cells[5].Value != DBNull.Value || !String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[5].Value.ToString()))
                        {
                            if (dataGridView1.Rows[index].Cells[5].Value.ToString().ToLower().Contains("not sure"))
                            {
                                dataGridView1.Rows[index].Cells[5].Value = "";
                            }
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (dataGridView1.Rows[index].Cells[6].Value != null || dataGridView1.Rows[index].Cells[6].Value != DBNull.Value || !String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[6].Value.ToString()))
                        {
                            if (dataGridView1.Rows[index].Cells[6].Value.ToString().ToLower().Contains("not sure"))
                            {
                                dataGridView1.Rows[index].Cells[6].Value = "";
                            }
                        }
                    }
                    catch
                    {
                    }
                    /***************End******************************************/



                    /*************Currency********************/
                    #region currency
                    dataGridView1.Rows[index].Cells[8].Value = "CDN";
                    #endregion currency

                    /****************End***********************/

                    #region price,stock

                    /***********Instock***********************/

                    if (_Work1doc2.DocumentNode.SelectNodes("//form[@action=\"/cart/add\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[9].Value = "Y";

                        /************Price**************************/
                        string price = "";
                        foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//script"))
                        {
                            if (_Node.InnerText.Contains("\"price\""))
                            {
                                price = _Node.InnerText.Substring(_Node.InnerText.ToLower().IndexOf("\"price\""));
                                price = price.Substring(0, price.IndexOf("\","));
                                price = price.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();
                                break;
                            }
                        }
                        dataGridView1.Rows[index].Cells[7].Value = price.Replace(":", "");

                        /***************End**************************/

                    }
                    else
                    {
                        dataGridView1.Rows[index].Cells[9].Value = "N";
                        /************Price**************************/
                        string price = "";
                        foreach (HtmlNode _Node in _Work1doc2.DocumentNode.SelectNodes("//script"))
                        {
                            if (_Node.InnerText.Contains("\"price\""))
                            {
                                price = _Node.InnerText.Substring(_Node.InnerText.ToLower().IndexOf("\"price\""));
                                price = price.Substring(0, price.IndexOf("\","));
                                price = price.ToLower().Replace("\"", "").Replace(",", "").Replace("$", "").Replace("price", "").Replace(":", "").Trim();
                                break;
                            }
                        }
                        dataGridView1.Rows[index].Cells[7].Value = price.Replace(":", "");
                        /***************End************************/

                    }
                    /******************end*********************/
                    #endregion price,stock

                    /***********Url******************/

                    /**************end****************/

                    /*************Image Url***************/
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"four columns alpha\"]/img") != null)
                    {
                        foreach (HtmlAttribute _Att in _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"four columns alpha\"]/img")[0].Attributes)
                        {
                            if (_Att.Name.ToLower() == "src")
                            {
                                dataGridView1.Rows[index].Cells[10].Value = _Att.Value;
                            }
                        }
                    }


                    /********************end***************/
                }
            }
            #endregion chilly
            #region airsoft
            else if (_IsAirsoft)
            {

                if (!_IsCategory && !_Issubcat)
                {

                    int index = 0;
                    index = gridindex;
                    gridindex++;
                    dataGridView1.Rows.Add();
                    /*****************rowid**********************/
                    dataGridView1.Rows[index].Cells[0].Value = index;
                    dataGridView1.Rows[index].Cells[11].Value = Url2;
                    /******************End**********************/

                    #region Name
                    /*****************Name**********************/
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"titlebar tl tr\"]/span[@class=\"titlebar-title green left titlebar-product-title h1\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[2].Value = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"titlebar tl tr\"]/span[@class=\"titlebar-title green left titlebar-product-title h1\"]")[0].InnerText.Trim();
                    }
                    /******************End**********************/
                    #endregion Name

                    #region sku
                    /*****************Sku**********************/

                    if (dataGridView1.Rows[index].Cells[2].Value != null || dataGridView1.Rows[index].Cells[2].Value != DBNull.Value || !String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[2].Value.ToString()))
                    {
                        dataGridView1.Rows[index].Cells[1].Value = GenerateSku("BA", dataGridView1.Rows[index].Cells[2].Value.ToString());
                    }
                    /******************End**********************/
                    #endregion sku



                    #region Description
                    /*****************Description**********************/
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab1\"]/div[@class=\"collateral-box bl br\"]") != null)
                    {
                        _Description = StripHTML(_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab1\"]/div[@class=\"collateral-box bl br\"]")[0].InnerText).Trim();
                    }



                    /*************what will you Material***********************/
                    try
                    {
                        if (_Description.Length < 2000)
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab3\"]/div[@class=\"attribute-specs\"]/ul") != null)
                            {
                                string _material = StripHTML(_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab3\"]/div[@class=\"attribute-specs\"]/ul")[0].InnerHtml.Replace("<ul>", "").Replace("</ul>", "").Replace("<UL>", "").Replace("<li>", "").Replace("</li>", ",").Replace("</LI>", ",").Replace("<LI>", ""));

                                if (_material.Length > 0)
                                {
                                    if (_material.Substring(_material.Length - 1) == ",")
                                    {
                                        _material = _material.Substring(0, _material.Length - 1);

                                    }
                                    _material = "Material: " + _material;
                                    _Description = _Description + " " + _material.Trim();
                                }
                            }
                        }

                    }
                    catch
                    {
                    }
                    /**************End**************************/


                    /*************what will you need***********************/
                    try
                    {
                        if (_Description.Length < 2000)
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab5\"]/div[@class=\"collateral-box bl br\"]/ul") != null)
                            {
                                string _Need = StripHTML(_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab5\"]/div[@class=\"collateral-box bl br\"]/ul")[0].InnerHtml.Replace("<ul>", "").Replace("</ul>", "").Replace("<UL>", "").Replace("<li>", "").Replace("</li>", ",").Replace("</LI>", ",").Replace("<LI>", ""));

                                if (_Need.Length > 0)
                                {
                                    if (_Need.Substring(_Need.Length - 1) == ",")
                                    {
                                        _Need = _Need.Substring(0, _Need.Length - 1);

                                    }
                                    _Need = "What you will need: " + _Need;
                                    _Description = _Description + " " + _Need.Trim();
                                }
                            }
                        }

                    }
                    catch
                    {
                    }
                    /**************End**************************/

                    /**********What is in this box***************/
                    try
                    {
                        if (_Description.Length < 2000)
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab4\"]/div[@class=\"collateral-box bl br\"]/ul") != null)
                            {
                                string _IsinBox = StripHTML(_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab4\"]/div[@class=\"collateral-box bl br\"]/ul")[0].InnerHtml.Replace("<ul>", "").Replace("</ul>", "").Replace("<UL>", "").Replace("<li>", "").Replace("</li>", ",").Replace("</LI>", ",").Replace("<LI>", ""));

                                if (_IsinBox.Length > 0)
                                {
                                    if (_IsinBox.Substring(_IsinBox.Length - 1) == ",")
                                    {
                                        _IsinBox = _IsinBox.Substring(0, _IsinBox.Length - 1);

                                    }
                                    _IsinBox = "What's in the Box: " + _IsinBox;
                                    _Description = _Description + " " + _IsinBox.Trim();
                                }
                            }
                        }

                    }
                    catch
                    {
                    }
                    if (_Description.Length > 2000)
                    {
                        _Description = _Description.Substring(0, 1997) + "...";
                    }
                    dataGridView1.Rows[index].Cells[3].Value = _Description;
                    /**************End**************************/
                    /******************End**********************/
                    #endregion Description

                    #region bulletpoints
                    /*****************bulletpoints**********************/
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab2\"]/div[@class=\"attribute-specs\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[4].Value = _Work1doc2.DocumentNode.SelectNodes("//div[@id=\"tab2\"]/div[@class=\"attribute-specs\"]")[0].InnerHtml.Trim();
                    }
                    /******************End**********************/
                    #endregion bulletpoints

                    #region Manufacturer
                    /*****************Manufacturer**********************/
                    /******************End**********************/
                    #endregion Manufacturer

                    #region Brandname
                    /*****************Brandname**********************/
                    /******************End**********************/
                    #endregion Brandname

                    #region Price
                    /*****************Price**********************/
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"price-box\"]/span/span[@class=\"price\"]") != null)
                    {
                        dataGridView1.Rows[index].Cells[7].Value = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"price-box\"]/span/span[@class=\"price\"]")[0].InnerText.Replace("$", "").Trim();
                    }
                    else
                    {
                        if (_Work1doc2.DocumentNode.SelectNodes("//span[@class=\"price\"]") != null)
                        {
                            dataGridView1.Rows[index].Cells[7].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@class=\"price\"]")[0].InnerText.Replace("$", "").Trim();
                        }
                    }
                    /******************End**********************/
                    #endregion Price

                    #region Currency
                    /*****************Currency**********************/
                    dataGridView1.Rows[index].Cells[8].Value = "CDN";
                    /******************End**********************/
                    #endregion Currency

                    #region stock
                    /*****************stock**********************/
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"titlebar tl tr\"]/span[@class=\"stock-notification green tl tr bl br right\"]") != null)
                    {
                        if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"titlebar tl tr\"]/span[@class=\"stock-notification green tl tr bl br right\"]")[0].InnerText.ToLower().Trim() == "in stock")
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "Y";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "N";
                        }
                    }
                    else
                    {
                        if (_Work1doc2.DocumentNode.SelectNodes("//span[@class=\"stock-notification green tl tr bl br right\"]") != null)
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//span[@class=\"stock-notification green tl tr bl br right\"]")[0].InnerText.ToLower().Trim() == "in stock")
                            {
                                dataGridView1.Rows[index].Cells[9].Value = "Y";
                            }
                            else
                            {
                                dataGridView1.Rows[index].Cells[9].Value = "N";
                            }
                        }
                        else if (_Work1doc2.DocumentNode.SelectNodes("//span[@class=\"stock-notification red tl tr bl br right\"]") != null)
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "N";
                        }
                        else if (_Work1doc2.DocumentNode.SelectNodes("//span[@class=\"stock-notification yellow tl tr bl br right\"]") != null)
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "Y";
                        }
                    }
                    /******************End**********************/
                    #endregion stock

                    #region Image
                    /*****************Image url**********************/
                    string _ImageUrl = "";
                    if (_Work1doc2.DocumentNode.SelectNodes("//div[@class=\"main-product-img\"]/img") != null)
                    {
                        HtmlNode _Node = _Work1doc2.DocumentNode.SelectNodes("//div[@class=\"main-product-img\"]/img")[0];
                        foreach (HtmlAttribute _Att in _Node.Attributes)
                        {
                            if (_Att.Name.ToLower() == "src")
                            {
                                _ImageUrl = _Att.Value;
                            }
                        }

                        /*********For multiple images*********************/
                        try
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//script") != null)
                            {

                                foreach (HtmlNode _Node1 in _Work1doc2.DocumentNode.SelectNodes("//script"))
                                {
                                    string Imagetext = "";
                                    if (_Node1.InnerText.ToLower().Contains("media_thumb_2()"))
                                    {
                                        Imagetext = _Node1.InnerText.ToLower().Substring(_Node1.InnerText.ToLower().IndexOf("https://www.buyairsoft.ca")).Replace("<img src=", "").Trim();

                                        Imagetext = Imagetext.Substring(0, Imagetext.IndexOf("\"")).Replace("\"", "");
                                        if (Imagetext.Length > 0)
                                        {
                                            if (Imagetext.Substring(Imagetext.Length - 1) == @"\")
                                                Imagetext = Imagetext.Substring(0, Imagetext.Length - 1);
                                        }
                                        _ImageUrl = _ImageUrl + "@" + Imagetext;
                                    }
                                    if (_Node1.InnerText.ToLower().Contains("media_thumb_3()"))
                                    {

                                        Imagetext = _Node1.InnerText.ToLower().Substring(_Node1.InnerText.ToLower().IndexOf("https://www.buyairsoft.ca")).Replace("<img src=", "").Trim();

                                        Imagetext = Imagetext.Substring(0, Imagetext.IndexOf("\"")).Replace("\"", "");
                                        if (Imagetext.Length > 0)
                                        {
                                            if (Imagetext.Substring(Imagetext.Length - 1) == @"\")
                                                Imagetext = Imagetext.Substring(0, Imagetext.Length - 1);
                                        }
                                        _ImageUrl = _ImageUrl + "@" + Imagetext;
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        /***************End********************************/

                        if (_ImageUrl.Length > 0)
                        {
                            if (_ImageUrl.Substring(_ImageUrl.Length - 1) == ",")
                            {
                                _ImageUrl = _ImageUrl.Substring(0, _ImageUrl.Length - 1);
                            }
                        }
                        dataGridView1.Rows[index].Cells[10].Value = _ImageUrl;

                    }
                    /******************End**********************/
                    #endregion Image


                }
            }
            #endregion airsoft
            #region knife
            else if (_IsKnifezone)
            {
                if (!_IsCategory)
                {
                    int index = 0;
                    index = gridindex;
                    gridindex++;
                    dataGridView1.Rows.Add();
                    /*****************rowid**********************/
                    dataGridView1.Rows[index].Cells[0].Value = index;
                    dataGridView1.Rows[index].Cells[11].Value = Url2;
                    dataGridView1.Rows[index].Cells[8].Value = "CDN";
                    /******************End**********************/

                    /**********Sku Bullet Points manufacturer brand Name*****************/
                    string Title = "";
                    if (_Work1doc2.DocumentNode.SelectNodes("//div/h1") != null)
                    {
                        HtmlNode _Node = _Work1doc2.DocumentNode.SelectNodes("//div/h1")[0];
                        Title = _Node.InnerText.Trim();
                        dataGridView1.Rows[index].Cells[2].Value = _Node.InnerText.Trim();

                        Dictionary<string, string> Manskubullet = new Dictionary<string, string>();
                        string test = _Node.XPath.Replace("div[1]/h1[1]", "") + "/table/tr/td/font[@color=\"color\"]";
                        HtmlNodeCollection _Nodecollkey = _Work1doc2.DocumentNode.SelectNodes(_Node.XPath.Replace("div[1]/h1[1]", "") + "/table/tr/td/font[@color=\"black\"]");
                        HtmlNodeCollection _Nodecollvalue = _Work1doc2.DocumentNode.SelectNodes(_Node.XPath.Replace("div[1]/h1[1]", "") + "/table/tr/td/font[@color=\"blue\"]");
                        if (_Nodecollkey != null)
                        {
                            for (int i = 0; i < _Nodecollkey.Count; i++)
                            {
                                try
                                {
                                    Manskubullet.Add(_Nodecollkey[i].InnerText.Trim(), _Nodecollvalue[i].InnerText.Trim());
                                }
                                catch
                                {
                                }
                                if (Manskubullet.Last().Key.ToLower().Contains("retail") || Manskubullet.Last().Key.ToLower().Contains("price"))
                                    break;
                            }
                        }

                        string Bullets = "<ul>";
                        foreach (var Items in Manskubullet)
                        {


                            if (Items.Key.ToLower().Contains("manufacturer"))
                            {
                                dataGridView1.Rows[index].Cells[5].Value = Items.Value.Trim();
                                dataGridView1.Rows[index].Cells[6].Value = Items.Value.Trim();
                            }
                            else if (Items.Key.ToLower().Contains("name"))
                            {
                                Title = Items.Value.Trim();
                                dataGridView1.Rows[index].Cells[2].Value = Items.Value.Trim();
                            }

                            else if (Items.Key.ToLower().Contains("model"))
                            {
                                dataGridView1.Rows[index].Cells[1].Value = Items.Value.Trim();
                            }
                            else if (Items.Key.ToLower().Contains("retail"))
                            {
                                dataGridView1.Rows[index].Cells[7].Value = Items.Key.ToLower().Replace("retail:", "").Replace("$", "").Trim();
                            }
                            else if (!Items.Key.ToLower().Contains("price"))
                            {
                                Bullets = Bullets + "<li>" + Items.Key + Items.Value + "</li>";
                            }


                        }
                        Bullets = Bullets + "</ul>";

                        if (Bullets.Length > 10)
                        {
                            dataGridView1.Rows[index].Cells[4].Value = Bullets;
                        }


                        /*********Description Images*******************/
                        try
                        {

                            bool _IsImageFind = false;
                            string xpath = "";
                            foreach (HtmlNode _imgNode in _Work1doc2.DocumentNode.SelectNodes("//img"))
                            {
                                foreach (HtmlAttribute _Att in _imgNode.Attributes)
                                {
                                    if (_Att.Name.ToLower() == "alt" && _Att.Value.ToLower().Contains(Title.ToLower()))
                                    {
                                        _IsImageFind = true;
                                        xpath = _imgNode.XPath;
                                    }
                                }
                                //if (_IsImageFind)
                                //    break;
                            }
                            //if (xpath.Length == 0)
                                xpath = "/body[1]/table[5]/tr[1]/td[2]/table[1]/tr[1]/td[2]/table[1]/tr[1]/td[2]/img[1]";

                            try
                            {
                                if (xpath.Length > 0)
                                {
                                    HtmlNode _imgNode = _Work1doc2.DocumentNode.SelectNodes(xpath)[0];
                                    xpath = "";
                                    foreach (HtmlAttribute _Att in _imgNode.Attributes)
                                    {
                                        if (_Att.Name.ToLower() == "src" || _Att.Name.ToLower() == "data-cfsrc")
                                        {
                                            dataGridView1.Rows[index].Cells[10].Value = _Att.Value.Trim();
                                            xpath = _imgNode.XPath.Replace("/img[1]", "");
                                        }
                                    }



                                }

                                #region description
                                if (xpath.Length > 0)
                                {
                                    try
                                    {
                                        _Description = _Work1doc2.DocumentNode.SelectNodes(xpath + "/font[@color=\"black\"]")[0].InnerText.Trim();
                                        if (_Description.Length > 2000)
                                        {
                                            dataGridView1.Rows[index].Cells[3].Value = _Description.Substring(0, 1997) + "...";
                                        }
                                        else
                                        {
                                            dataGridView1.Rows[index].Cells[3].Value = _Description;
                                        }

                                    }
                                    catch
                                    {
                                    }
                                }
                                #endregion decsription
                            }
                            catch
                            {
                            }
                        }
                        catch
                        {
                        }



                        /*****************End*******************/

                        /***************Price************************/

                        if (dataGridView1.Rows[index].Cells[1].Value == null || dataGridView1.Rows[index].Cells[1].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[1].Value.ToString()))
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"identifier\"]") != null)
                            {
                                dataGridView1.Rows[index].Cells[1].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"identifier\"]")[0].InnerText.Trim();
                            }
                        }

                        if (dataGridView1.Rows[index].Cells[5].Value == null || dataGridView1.Rows[index].Cells[5].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[5].Value.ToString()))
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]") != null)
                            {
                                dataGridView1.Rows[index].Cells[5].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]")[0].InnerText.Trim();
                                dataGridView1.Rows[index].Cells[6].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]")[0].InnerText.Trim();
                            }
                        }


                        if (dataGridView1.Rows[index].Cells[7].Value == null || dataGridView1.Rows[index].Cells[7].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[index].Cells[7].Value.ToString()))
                        {
                            if (_Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]") != null)
                            {
                                dataGridView1.Rows[index].Cells[7].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"price\"]")[0].InnerText.Trim();
                            }
                        }
                        /**********************End********************/

                        #region stock
                        if (_Work1doc2.DocumentNode.InnerHtml.ToLower().Contains("temporarily unavailable"))
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "N";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[9].Value = "Y";
                        }
                        #endregion stock

                    }


                    else
                    {


                        if (_Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"name\"]") != null)
                        {
                            /***************Sku***************/
                            try
                            {
                                dataGridView1.Rows[index].Cells[1].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"identifier\"]")[0].InnerText;
                            }
                            catch
                            {
                            }
                            /****************End*****************/


                            /***************Name***************/
                            try
                            {
                                dataGridView1.Rows[index].Cells[2].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"name\"]")[0].InnerText;
                            }
                            catch
                            {
                            }
                            /****************End*****************/


                            /***************Description***************/
                            try
                            {
                                string Xpath = "/body[1]/table[2]/tr[1]/td[2]/font[1]";
                                _Description = _Work1doc2.DocumentNode.SelectNodes(Xpath)[0].InnerText;

                                if (_Description.Length > 2000)
                                {
                                    dataGridView1.Rows[index].Cells[3].Value = _Description.Substring(0, 1997) + "...";
                                }
                                else
                                {
                                    dataGridView1.Rows[index].Cells[3].Value = _Description;
                                }
                            }
                            catch
                            {
                            }
                            /****************End*****************/


                            /***************Bullepoint***************/
                            //try
                            //{
                            //    dataGridView1.Rows[index].Cells[2].Value = _Work1doc.DocumentNode.SelectNodes("//span[@itemprop=\"name\"]")[0].InnerText;
                            //}
                            //catch
                            //{
                            //}
                            /****************End*****************/


                            /***************Brand***************/
                            try
                            {
                                dataGridView1.Rows[index].Cells[5].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]")[0].InnerText;
                                dataGridView1.Rows[index].Cells[6].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"brand\"]")[0].InnerText;
                            }
                            catch
                            {
                                try
                                {
                                    dataGridView1.Rows[index].Cells[5].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"manufacturer\"]")[0].InnerText;
                                    dataGridView1.Rows[index].Cells[6].Value = _Work1doc2.DocumentNode.SelectNodes("//span[@itemprop=\"manufacturer\"]")[0].InnerText;
                                }
                                catch
                                {
                                }
                            }
                            /****************End*****************/

                            /****************Price********************/
                            foreach (HtmlNode _priceNode in _Work1doc2.DocumentNode.SelectNodes("//font"))
                            {
                                if (_priceNode.InnerText.ToLower().Contains("retail"))
                                {
                                    dataGridView1.Rows[index].Cells[7].Value = _priceNode.InnerText.ToLower().Replace("retail", "").Replace(":", "").Replace("$", "").Trim();
                                }
                            }
                            /***************End***********************/

                            /****************Stock********************/
                            if (_Work1doc2.DocumentNode.InnerHtml.ToLower().Contains("temporarily unavailable"))
                            {
                                dataGridView1.Rows[index].Cells[9].Value = "N";
                            }
                            else
                            {
                                dataGridView1.Rows[index].Cells[9].Value = "Y";
                            }
                            /***************End***********************/


                            /***************Image*******************/
                            try
                            {
                                string Xpath = "/body[1]/table[2]/tr[1]/td[1]/img[1]";

                                foreach (HtmlAttribute _Att in _Work1doc2.DocumentNode.SelectNodes(Xpath)[0].Attributes)
                                {
                                    if (_Att.Name.ToLower() == "src" || _Att.Name.ToLower() == "data-cfsrc")
                                    {
                                        dataGridView1.Rows[index].Cells[10].Value = _Att.Value;
                                    }
                                }

                            }
                            catch
                            {
                            }

                            /********************End**********************************/

                        }


                    }
                    /*******************End****************************************/

                }
            }
            #endregion knife

        }

        public string GenerateSku(string starttext, string productname)
        {
            string result = "";
            foreach (var c in productname)
            {
                int ascii = (int)c;
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ')
                {

                    result += c;
                }
            }
            string[] name = result.Split(' ');
            string firstcharcter = "";
            foreach (string _name in name)
            {
                firstcharcter = _name.Trim();
                if (firstcharcter.Length > 0)
                    starttext = starttext + firstcharcter.Substring(0, 1).ToUpper();
            }
            return starttext;
        }
        public void SendMail(string body, string subject, bool Isattachment, bool Exception)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                MailAddress Fromaddress = new MailAddress("consultancy874@gmail.com", "Vishal Consultancy");

                message.From = Fromaddress;
                message.Subject = subject;
                message.To.Add(new MailAddress("consultancy874@gmail.com"));
                message.Body = body;
                message.IsBodyHtml = true;
                System.Net.Mail.SmtpClient mclient = new System.Net.Mail.SmtpClient();
                mclient.Host = "smtp.gmail.com";
                mclient.Port = 587;
                mclient.EnableSsl = true;
                if (!Exception)
                {
                    try
                    {
                        string name = Convert.ToString(System.Configuration.ConfigurationSettings.
                                               AppSettings["Contacts"]);
                        string[] mails = name.Split(',');
                        foreach (string mail in mails)
                        {
                            if (mail.Length > 0)
                            {
                                message.CC.Add(mail);
                            }
                        }
                    }
                    catch
                    {
                    }
                    if (Isattachment)
                    {
                    }

                }
                mclient.Credentials = new System.Net.NetworkCredential("consultancy874@gmail.com", "(123456@#Aa)");
                mclient.Send(message);
            }
            catch
            {
            }
        }
        private void Go_Click(object sender, EventArgs e)
        {
           
            _IsProduct = false;
            _Name.Clear();
            _Chillyindex = 0;
             CategoryUrl.Clear();
             SubCategoryUrl.Clear();
            _ProductUrl.Clear();
            _percent.Visible = false;
             Go.Enabled = false;
             Pause.Enabled = true;
             createcsvfile.Enabled = true;
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
             #region warrior
             if (chkstorelist.GetItemChecked(0))
             {
                 _ISWarrior = true;
                 try
                 {
                     
                     _lblerror.Visible = true;
                     _lblerror.Text = "We are going to read sku and manufacturer information form category page of " + chkstorelist.Items[0].ToString() + " Website";
                     _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));
                     HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@id=\"productsListingTopNumber\"]//strong");
                     if (_Collection != null)
                     {
                         _TotalRecords = Convert.ToInt32(_Work1doc.DocumentNode.SelectNodes("//div[@id=\"productsListingTopNumber\"]//strong")[2].InnerText);
                         _Pages = Convert.ToInt32(_TotalRecords / 60) + 1;
                         for (int i = 1; i <= _Pages; i++)
                         {

                             while (_Work.IsBusy && _Work1.IsBusy)
                             {
                                 Application.DoEvents();

                             }

                             while (_Stop)
                             {
                                 Application.DoEvents();
                             }


                             tim(2);
                             if (!_Work.IsBusy)
                             {
                                 Url1 = _ScrapeUrl + "&page=" + i;
                                 _Work.RunWorkerAsync();
                             }

                             else
                             {
                                 Url2 = _ScrapeUrl + "&page=" + i;
                                 _Work1.RunWorkerAsync();
                             }
                             tim(2);
                             tim(2);
                         }

                         while (_Work.IsBusy || _Work1.IsBusy)
                         {
                             Application.DoEvents();

                         }

                         _Bar1.Value = 0;
                         _percent.Visible = false;
                         _lblerror.Visible = true;
                         _lblerror.Text = "We are going to read Decsription and Bullets information from product page of " + chkstorelist.Items[0].ToString() + " Website";
                         _Pages = 0;
                         _TotalRecords = 0;
                         _IsCategory = true;
                         _Stop = false;
                         time = 0;
                         _IsCategory = false;
                         tim(3);
                         totalrecord.Visible = true;
                         totalrecord.Text = "Total Products :" + _Tbale.Rows.Count.ToString();
                         int Counter = 0;
                         foreach (DataRow _Row in _Tbale.Rows)
                         {
                             while (_Work.IsBusy)
                             {
                                 Application.DoEvents();

                             }
                             while (_Stop)
                             {
                                 Application.DoEvents();
                             }


                             _Iscompleted = false;
                             _Description = "";
                             Bullets = "";
                             Url1 = _Row[11].ToString();
                             _Work.RunWorkerAsync();

                             while (!_Iscompleted)
                             {
                                 Application.DoEvents();
                             }
                             dataGridView1.Rows.Add();
                             for (int i = 0; i < 12; i++)
                             {
                                 dataGridView1.Rows[Counter].Cells[i].Value = _Row[i].ToString();
                             }
                             dataGridView1.Rows[Counter].Cells[3].Value = _Description;
                             dataGridView1.Rows[Counter].Cells[4].Value = Bullets;
                             Counter++;

                         }
                         while (_Work.IsBusy || _Work1.IsBusy)
                         {
                             Application.DoEvents();

                         }


                         _lblerror.Visible = true;
                         _lblerror.Text = "All Products Scrapped for " + chkstorelist.Items[0].ToString()+ " Website";

                     }

                     else
                     {
                         _lblerror.Text = "Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for " + chkstorelist.Items[0].ToString() + " Website";
                         /****************Email****************/
                        SendMail("Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for " + chkstorelist.Items[0].ToString() + " Website" + DateTime.Now.ToString(), "Urgenr issue in Scrapper.", false, false);
                         /*******************End********/
                     }
                 }
                 catch
                 {
                     _lblerror.Visible = true;
                     _lblerror.Text = "Oops Some issue Occured in scrapping data " + chkstorelist.Items[0].ToString()+ " Website";
                 }
                 while (_Work.IsBusy || _Work1.IsBusy)
                     {
                       Application.DoEvents();

                     }

                 Disableallstores();
             }
             # endregion warrior

             #region chilly
             else if (chkstorelist.GetItemChecked(1))
             {
                 _Chillyindex = 0;
                 gridindex = dataGridView1.Rows.Count;
                 if (gridindex == 1)
                 {
                     if (dataGridView1.Rows[0].Cells[1].Value == null || dataGridView1.Rows[0].Cells[1].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[0].Cells[1].Value.ToString()))
                     {
                         gridindex=0;
                     }
                 }

                _Bar1.Value = 0;
                 _percent.Visible = false;

                 _lblerror.Visible = true;
                  _Pages = 0;
                 _TotalRecords = 0;
                 _Stop = false;
                 time = 0;
                      
                 _IsCategory = true;
                 _ISchilychiles = true;
                 _lblerror.Visible = true;
                 _lblerror.Text = "We are going to read product Url for " + chkstorelist.Items[1].ToString() + " Website";
                  _ScrapeUrl="http://chillychiles.com/collections/all";
                  _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));
                 try
                 {

                     if (_Work1doc.DocumentNode.SelectNodes("//div[@class=\"pagination\"]/span") != null)
                     {
                         HtmlNodeCollection _Collection = _Work1doc.DocumentNode.SelectNodes("//div[@class=\"pagination\"]/span");
                         string test = _Collection[_Collection.Count - 1].InnerText;
                         _Pages = Convert.ToInt32(_Collection[_Collection.Count - 2].InnerText);
                         totalrecord.Visible = true;
                         totalrecord.Text = "Total Category :" + _Pages;

                         for (int i = 1; i <= _Pages; i++)
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
                                 Url1 = _ScrapeUrl + "?page=" + i;
                                 _Work.RunWorkerAsync();
                             }

                             else
                             {
                                 Url2 = _ScrapeUrl + "?page=" + i;
                                 _Work1.RunWorkerAsync();
                             }

                         }


                         while (_Work.IsBusy || _Work1.IsBusy)
                         {
                             Application.DoEvents();

                         }
                         tim(2);
                         _Bar1.Value = 0;
                         _percent.Visible = false;
                         _lblerror.Visible = true;
                         _lblerror.Text = "We are going to read product information of " + chkstorelist.Items[1].ToString() + " Website";
                         _Pages = 0;
                         _TotalRecords = 0;
                         _IsCategory = true;
                         _Stop = false;
                         time = 0;
                         _IsCategory = false;
                         tim(3);
                         totalrecord.Visible = true;
                         totalrecord.Text = "Total Products :" + _ProductUrl.Count();
                         _Chillyindex = 0;

                         int counter=0;




                      //   _ProductUrl.Add("http://chillychiles.com/products/ole-ray-s-sloppy-sauce");
                      //   _ProductUrl.Add("http://chillychiles.com//products/dallesandro-chipotle-peppers-in-adobo-sauce");
                      //   _ProductUrl.Add("http://chillychiles.com//products/sancto-scorpio-hot-sauce");
                      ////_ProductUrl.Add("http://chillychiles.com//products/historic-lynchburg-tennessee-whiskey-jalapeno-cocktail-sauce");
                      //_ProductUrl.Add("http://chillychiles.com//products/daves-gourmet-6-pure-dried-chiles");
                      //_ProductUrl.Add("http://chillychiles.com//products/tabasco-sweet-and-spicy-pepper-sauce");
                      //_ProductUrl.Add("http://chillychiles.com//products/tabasco-chipotle-pepper-sauce-1");
                      //   _ProductUrl.Add("http://chillychiles.com//products/georgia-peach-and-vidalia-onion-hot-sauce");
                      //   _ProductUrl.Add("http://chillychiles.com//products/crabanero-hot-sauce");
                      //   _ProductUrl.Add("http://chillychiles.com//products/the-torture-trio-3-pack");
                      //   _ProductUrl.Add("http://chillychiles.com//products/daves-gourmet-whole-ghost-peppers");
                      //   _ProductUrl.Add("http://chillychiles.com//products/hoboken-eddies-apple-brandy-bbq-sauce");
                      //   _ProductUrl.Add("http://chillychiles.com//products/historic-lynchburg-tennessee-whiskey-diabetic-friendly-mild-gourmet-deli-grillin-sauce");
                      //   _ProductUrl.Add("http://chillychiles.com//products/historic-lynchburg-tennessee-whiskey-diabetic-friendly-hot-spicy-bbq");
                      //   _ProductUrl.Add("http://chillychiles.com//products/tabasco-raspberry-chipotle-hot-sauce");
                      //   _ProductUrl.Add("http://chillychiles.com//products/tabasco-flavoured-jelly-belly-jelly-beans");
                      //   _ProductUrl.Add("http://chillychiles.com//products/ass-in-hell-hot-sauce");
                      //   _ProductUrl.Add("http://chillychiles.com//products/ole-rays-red-delicious-apple-bourbon-bbq-and-cooking-sauce");
                       
                         foreach (string Url in _ProductUrl)
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
                                 Url1 = Url;
                                 _Work.RunWorkerAsync();
                             }

                             else
                             {
                                 Url2 = Url;
                                 _Work1.RunWorkerAsync();
            
                             }
                             counter++;
                         }

                         while (_Work.IsBusy || _Work1.IsBusy)
                         {
                             Application.DoEvents();

                         }
                     }

                     else
                     {
                         _lblerror.Visible = true;
                         _lblerror.Text = "Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for " + chkstorelist.Items[1].ToString() + " Website";
                    /****************Email****************/

                       SendMail("Oops there is change in html code  on client side. You need to contact with developer in order to check this issue for " + chkstorelist.Items[1].ToString() + " Website" + DateTime.Now.ToString(), "Urgenr issue in Scrapper.", false, false);
            
                         /*******************End********/
                     
                     }
                  }
                 catch
                 {
                     _lblerror.Visible = true;
                     _lblerror.Text = "Oops Some issue Occured in scrapping data " + chkstorelist.Items[1].ToString() + " Website";
                 }
                 while (_Work.IsBusy || _Work1.IsBusy)
                 {
                     Application.DoEvents();

                 }

                 Disableallstores();

             }
             # endregion chilly

             #region airsoft

             else if (chkstorelist.GetItemChecked(2))
             {
                 gridindex = dataGridView1.Rows.Count;
                 if (gridindex == 1)
                 {
                     if (dataGridView1.Rows[0].Cells[1].Value == null || dataGridView1.Rows[0].Cells[1].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[0].Cells[1].Value.ToString()))
                     {
                         gridindex = 0;
                     }
                 }
                 _Chillyindex = 0;
                  CategoryUrl.Clear();
                  SubCategoryUrl.Clear();
                 _Bar1.Value = 0;
                 _percent.Visible = false;
                 _Pages = 0;
                 _TotalRecords = 0;
                 _Stop = false;
                  time = 0;
                 _IsCategory = false;
                 _Issubcat = true;
                 _IsAirsoft= true;
                 _ScrapeUrl = "https://www.buyairsoft.ca/";
                 _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));
                 /***************Code to read Parent category Url*********************/
                 if (_Work1doc.DocumentNode.SelectNodes("//div[@id=\"top-nav\"]/ul/li/a") != null)
                 {

                     #region category
                     foreach (HtmlNode _node in _Work1doc.DocumentNode.SelectNodes("//div[@id=\"top-nav\"]/ul/li/a"))
                     {

                         foreach(HtmlAttribute _Att in _node.Attributes)
                         {
                             if (_Att.Name.ToLower() == "href")
                                 CategoryUrl.Add(_Att.Value);
                         }
                     }


                     _lblerror.Visible = true;
                     _lblerror.Text = "We are going to read Sub category Url for " + chkstorelist.Items[2].ToString() + " Website";

                     totalrecord.Visible = true;
                     totalrecord.Text = "Total Parent Category :" + CategoryUrl.Count();

                     foreach (string subcaturl in CategoryUrl)
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
                             Url1 = subcaturl;
                             _Work.RunWorkerAsync();
                         }

                         else
                         {
                             Url2 = subcaturl;
                             _Work1.RunWorkerAsync();

                         }
                         
                     }
                     while (_Work.IsBusy ||_Work1.IsBusy)
                     {
                         Application.DoEvents();

                     }

                     if (!SubCategoryUrl.Contains("https://www.buyairsoft.ca/deals/refurbished.html?limit=all"))
                     {
                         SubCategoryUrl.Add("https://www.buyairsoft.ca/deals/refurbished.html?limit=all");
                     }
                     tim(3);
                     _Bar1.Value = 0;
                     _percent.Visible = false;
                     _lblerror.Visible = true;
                     _lblerror.Text = "We are going to read product Url for " + chkstorelist.Items[2].ToString() + " Website";
                     _Pages = 0;
                     _TotalRecords = 0;
                     _Stop = false;
                      time = 0;
                      totalrecord.Visible = true;
                      totalrecord.Text = "Total Sub Categories :" + SubCategoryUrl.Count();
                     _Chillyindex = 0;
                     CategoryUrl.Clear();
                     _Issubcat = false;
                     _IsCategory = true;
                      
                      #endregion category

                     #region sub category

                      foreach (string subcaturl in SubCategoryUrl)
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
                              Url1 = subcaturl;
                              _Work.RunWorkerAsync();
                          }

                          else
                          {
                              Url2 = subcaturl;
                              _Work1.RunWorkerAsync();

                          }

                      }
                      while (_Work.IsBusy || _Work1.IsBusy)
                      {
                          Application.DoEvents();

                      }
                      #endregion subcategory

                      #region Productpage

                      tim(3);
                      _Bar1.Value = 0;
                      _percent.Visible = false;
                      _lblerror.Visible = true;
                      _lblerror.Text = "We are going to read product Information for " + chkstorelist.Items[2].ToString() + " Website";
                      _Pages = 0;
                      _TotalRecords = 0;
                      _Stop = false;
                      time = 0;
                      totalrecord.Visible = true;
                      totalrecord.Text = "Total :" + _ProductUrl.Count();
                      _Chillyindex = 0;
                      CategoryUrl.Clear();
                      SubCategoryUrl.Clear();
                      _Issubcat = false;
                      _IsCategory = false;
                      

                      foreach (string url in _ProductUrl)
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
                     

                      #endregion productpage



                      tim(2);
                      Disableallstores();
                     
                 }
                 else
                 {
                     _lblerror.Visible = true;
                     _lblerror.Text = "Oops Some issue Occured in scrapping data " + chkstorelist.Items[2].ToString() + " Website";
                 
                 }
                 /*************************End**************************************/


                 

             }
            #endregion airsoft

             #region Knife
             else if (chkstorelist.GetItemChecked(3))
             {
                 gridindex = dataGridView1.Rows.Count;
                 if (gridindex == 1)
                 {
                     if (dataGridView1.Rows[0].Cells[1].Value == null || dataGridView1.Rows[0].Cells[1].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[0].Cells[1].Value.ToString()))
                     {
                         gridindex = 0;
                     }
                 }
                 _Chillyindex = 0;
                 CategoryUrl.Clear();
                 SubCategoryUrl.Clear();
                 _ProductUrl.Clear();
                 _Bar1.Value = 0;
                 _percent.Visible = false;
                 _Pages = 0;
                 _TotalRecords = 0;
                 _Stop = false;
                 time = 0;
                 _IsCategory = true;
                 _IsKnifezone = true;
                 _ScrapeUrl = "http://www.knifezone.ca/";
                 _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));

                 #region category
                 if (_Work1doc.DocumentNode.SelectNodes("//font") != null)
                 {
                     bool _CategoryDiv = false;
                     foreach (HtmlNode _Node in _Work1doc.DocumentNode.SelectNodes("//font"))
                     {
                         foreach (HtmlAttribute _Att in _Node.Attributes)
                         {
                             if (_Att.Name.ToLower() == "size")
                             {
                                 if (_Att.Value == "-1")
                                     _CategoryDiv = true;
                             }
                         }

                         if (_CategoryDiv)
                         {
                             if (_Node.SelectNodes("a") != null)
                             {
                                 foreach (HtmlNode _Node1 in _Node.SelectNodes("a"))
                                 {
                                     foreach (HtmlAttribute _Att in _Node1.Attributes)
                                     {
                                         if (_Att.Name.ToLower() == "href")
                                         {
                                             string _Indexurl = _Att.Value;
                                             try
                                             {
                                                 if (_Indexurl.Contains("../"))
                                                 {
                                                     _Indexurl = _Indexurl.Replace("../", "");
                                                     _Indexurl = _Indexurl.Substring(0, _Indexurl.IndexOf("/"));
                                                     _Indexurl = _Indexurl + "/index.htm";
                                                 }
                                                 else
                                                 {
                                                     _Indexurl = Reverse(_Indexurl);
                                                     _Indexurl = _Indexurl.Substring(_Indexurl.IndexOf("/"));
                                                     _Indexurl = Reverse(_Indexurl) + "/index.htm";
                                                 }
                                             }
                                             catch
                                             {

                                                 _Indexurl = _Att.Value;
                                             }
                                             CategoryUrl.Add("http://www.knifezone.ca/" + _Indexurl);
                                         }
                                     }

                                 }
                             }
                             else
                             {
                                 _CategoryDiv = false;
                             }
                         }
                         if (_CategoryDiv)
                             break;
                     }


                     _lblerror.Visible = true;
                     CategoryUrl.Remove("http://www.knifezone.ca/specials//index.htm");
                     CategoryUrl.Add("http://www.knifezone.ca/grohmannoutdoor/index.htm");
                     CategoryUrl.Add("http://www.knifezone.ca/grohmannkitchenpoly/index.htm");
                     CategoryUrl.Add("http://www.knifezone.ca/grohmannkitchenregular/index.htm");
                     CategoryUrl.Add("http://www.knifezone.ca/grohmannkitchenfulltang/index.htm");
                     CategoryUrl.Add("http://www.knifezone.ca/grohmannkitchenforged/index.htm");
                     _lblerror.Text = "We are going to read Product Url for " + chkstorelist.Items[3].ToString() + " Website";

                     #endregion category

                     totalrecord.Visible = true;
                     totalrecord.Text = "Total  Categories :" + CategoryUrl.Count();

                      #region ProductURL

                      foreach (string caturl in CategoryUrl)
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
                             Url1 = caturl;
                             _Work.RunWorkerAsync();
                         }

                         else
                         {
                             Url2 = caturl;
                             _Work1.RunWorkerAsync();

                         }


                     }
                     while (_Work.IsBusy || _Work1.IsBusy)
                     {
                         Application.DoEvents();

                     }


                     #endregion Producturl
                     #region productinformation

                     tim(3);
                      _Bar1.Value = 0;
                      _percent.Visible = false;
                      _lblerror.Visible = true;
                      _lblerror.Text = "We are going to read product Information for " + chkstorelist.Items[3].ToString() + " Website";
                      _Pages = 0;
                      _TotalRecords = 0;
                      _Stop = false;
                      time = 0;
                      totalrecord.Visible = true;

                      totalrecord.Text = "Total :" + _ProductUrl.Count();
                      _Chillyindex = 0;
                      CategoryUrl.Clear();
                      SubCategoryUrl.Clear();
                      _Issubcat = false;
                      _IsCategory = false;
                      _Chillyindex=0;
                      //_ProductUrl.Clear();
                     // _ProductUrl.Add("http://www.knifezone.ca/grohmannoutdoor/originalwaterbuffalo.htm");
                      //_ProductUrl.Add("http://www.knifezone.ca/crkt/firesparkknife.htm");
                      //_ProductUrl.Add("http://www.knifezone.ca/coldsteel/tiliteswitchblade.htm");

                       foreach (string Prdurl in _ProductUrl)
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
                             Url1 = Prdurl;
                             _Work.RunWorkerAsync();
                         }

                         else
                         {
                             Url2 = Prdurl;
                             _Work1.RunWorkerAsync();

                         }


                     }
                     while (_Work.IsBusy || _Work1.IsBusy)
                     {
                         Application.DoEvents();

                     }


                     Disableallstores();



                 }


                 #endregion productinformation

             }
             #endregion knfie

             //#region liveoutthere
             //else if (chkstorelist.GetItemChecked(4))
             //{
             //    gridindex = dataGridView1.Rows.Count;
             //    if (gridindex == 1)
             //    {
             //        if (dataGridView1.Rows[0].Cells[1].Value == null || dataGridView1.Rows[0].Cells[1].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[0].Cells[1].Value.ToString()))
             //        {
             //            gridindex = 0;
             //        }
             //    }
             //    _Chillyindex = 0;
             //    CategoryUrl.Clear();
             //    SubCategoryUrl.Clear();
             //    _ProductUrl.Clear();
             //    _Bar1.Value = 0;
             //    _percent.Visible = false;
             //    _Pages = 0;
             //    _TotalRecords = 0;
             //    _Stop = false;
             //    time = 0;
             //    _IsCategory = true;
             //    _IsLiveoutthere = true;
             //    _ScrapeUrl = "https://www.liveoutthere.com";
             //    _Work1doc.LoadHtml(_Client1.DownloadString(_ScrapeUrl));

             //    #region category
             //    if(_Work1doc.DocumentNode.SelectNodes("//a[@class=\"navbar-submenu-container-item-shop-all\"]")!=null)
             //    {
             //        foreach(HtmlNode _node in _Work1doc.DocumentNode.SelectNodes("//a[@class=\"navbar-submenu-container-item-shop-all\"]"))
             //        {
             //            foreach (HtmlAttribute _Att in _node.Attributes)
             //            {
             //                if (_Att.Name.ToLower() == "href")
             //                    CategoryUrl.Add("https://www.liveoutthere.com/" + _Att.Value);
             //            }
             //        }
             //    }
             //    CategoryUrl.Remove(CategoryUrl[CategoryUrl.Count-1]);
             //    #endregion category

             //    _IsCategory = true;
             //    _lblerror.Text = "We are going to read Category Url for " + chkstorelist.Items[4].ToString() + " Website";
             //    #region subcategory
             //    foreach (string url in CategoryUrl)
             //    {

             //        while (_Work.IsBusy || _Work1.IsBusy)
             //        {
             //            Application.DoEvents();

             //        }

             //        while (_Stop)
             //        {
             //            Application.DoEvents();
             //        }



             //        if (!_Work.IsBusy)
             //        {
             //            Url1 = url;
             //            _Work.RunWorkerAsync();
             //        }

             //        else
             //        {
             //            Url2 = url;
             //            _Work1.RunWorkerAsync();

             //        }

             //        break;
             //    }


             //    while (_Work.IsBusy || _Work1.IsBusy)
             //    {
             //        Application.DoEvents();

             //    }
             //    #endregion subcategory
             //    tim(3);
             //    _Bar1.Value = 0;
             //    _percent.Visible = false;
             //    _lblerror.Visible = true;
             //    _lblerror.Text = "We are going to read Product Url for " + chkstorelist.Items[4].ToString() + " Website";

             //    #region producturl
             //    _Chillyindex = 0;
             //    _IsCategory = false;
             //    _Issubcat = true;
             //    foreach (string url in SubCategoryUrl)
             //    {

             //        while (_Work.IsBusy || _Work1.IsBusy)
             //        {
             //            Application.DoEvents();

             //        }

             //        while (_Stop)
             //        {
             //            Application.DoEvents();
             //        }



             //        if (!_Work.IsBusy)
             //        {
             //            Url1 = url;
             //            _Work.RunWorkerAsync();
             //        }

             //        else
             //        {
             //            Url2 = url;
             //            _Work1.RunWorkerAsync();

             //        }
             //        break;
             //    }

             //        while (_Work.IsBusy || _Work1.IsBusy)
             //        {
             //            Application.DoEvents();

             //        }
             //        int count = _ProductUrl.Count();

             //    #endregion producturl

             //    #region productinformation
             //         tim(3);
             //         _Isreadywebbrowser1 = false;
             //         _Isreadywebbrowser2 = false;
             //        _Bar1.Value = 0;
             //        _percent.Visible = false;
             //        _lblerror.Visible = true;
             //        _lblerror.Text = "We are going to read Product Information for " + chkstorelist.Items[4].ToString() + " Website";
             //        _Issubcat = false;
             //        _IsProduct = true;
             //        _ProductUrl.Clear();
             //        _ProductUrl.Add("https://www.liveoutthere.com/saxx-mens-vibe-modern-fit-boxer.html");
             //       foreach (string url in _ProductUrl)
             //        {

             //            while (_Work.IsBusy && _Work1.IsBusy)
             //            {
             //                Application.DoEvents();

             //            }

             //            while (_Stop)
             //            {
             //                Application.DoEvents();
             //            }



             //            if (!_Work.IsBusy)
             //            {
             //                _Isreadywebbrowser1 = false;
             //                Url1 = url;
             //                _Work.RunWorkerAsync();
             //            }

             //            else
             //            {
             //                _Isreadywebbrowser2 = false;
             //                Url2 = url;
             //                _Work1.RunWorkerAsync();

             //            }
             //        }


             //        while (_Work.IsBusy || _Work1.IsBusy)
             //        {
             //            Application.DoEvents();

             //        }
             //    #endregion productinformation
             //}

             //#endregion liveoutthere
             MessageBox.Show("Process Completed.");

             Pause.Enabled = false;
             Go.Enabled = true;
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            if (Pause.Text.ToUpper() == "PAUSE")//for pause and resume process
            {
                _Stop = true;
                Pause.Text = "RESUME";
            }
            else
            {
                _Stop = false;
                Pause.Text = "Pause";
            }
        }

        private void createcsvfile_Click(object sender, EventArgs e)
        {
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



            for (int m = 0; m < dataGridView1.Rows.Count; m++)
            {
                exceldt.Rows.Add();
                for (int n = 0; n < dataGridView1.Columns.Count-1; n++)
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
                using (CsvFileWriter writer = new CsvFileWriter(Application.StartupPath + "/" + "data.txt"))
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
                    for (int m = 0; m < exceldt.Rows.Count ; m++)
                    {
                        CsvFileWriter.CsvRow row1 = new CsvFileWriter.CsvRow();
                        for (int n = 1; n < exceldt.Columns.Count; n++)
                        {
                            row1.Add(String.Format("{0}", exceldt.Rows[m][n].ToString().Replace("\n", "").Replace("\r", "").Replace("\t", "")));
                        }
                        writer.WriteRow(row1);
                    }
                }
                System.Diagnostics.Process.Start(Application.StartupPath + "/" + "data.txt");//OPEN THE CSV FILE ,,CSV FILE NAMED AS DATA.CSV
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
                    builder.Append( value.Replace("\n", "") + "\t");
                }
                row.LineText = builder.ToString();
                WriteLine(row.LineText);
            }
           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void totalrecord_Click(object sender, EventArgs e)
        {

        }

        private void _percent_Click(object sender, EventArgs e)
        {

        }


        

    }


}
