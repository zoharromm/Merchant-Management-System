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
namespace palyerborndate
{
    public partial class Form1 : Form
    {
        BackgroundWorker _Work = new BackgroundWorker();
        BackgroundWorker _Work1= new BackgroundWorker();
        bool _Iscompleted = false;
        bool _ISWarrior = false;
        bool _ISchilychiles = false;
        bool _IsAirsoft = false;
        bool _IsKnifezone = false;
        bool _IsLiveoutthere = false;
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


        public void work_RunWorkerAsync(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        public void work_RunWorkerAsync1(object sender, RunWorkerCompletedEventArgs e)
        {

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
            for (int i = 0; i < chkstorelist.Items.Count; i++)
            {
                chkstorelist.SetItemChecked(i, true);
            }
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
                _Work1doc.LoadHtml(_Client1.DownloadString(Url1));

            }
            catch
            {
                _Iserror = true;
            }

            int index = 0;
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
            else if(_ISchilychiles)
            {

            }


        }
        public void work_dowork1(object sender, DoWorkEventArgs e)
        {
            _Work1doc2.LoadHtml(_Client2.DownloadString(Url2));

            
            int index = 0;
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
            else if(_ISchilychiles)
            { }

        }
        private void Go_Click(object sender, EventArgs e)
        {
            
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
                         gridindex = 0;
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
                         _lblerror.Visible = true;
                         _lblerror.Text = "Oops Some issue Occured in scrapping data " + chkstorelist.Items[0].ToString()+ " Website";
                     }
                 }
                 catch
                 {
                     _lblerror.Visible = true;
                     _lblerror.Text = "Oops Some issue Occured in scrapping data " + chkstorelist.Items[0].ToString()+ " Website";
                 }
                 Disableallstores();
             }
              else if (chkstorelist.GetItemChecked(1))
             {
                 _ISchilychiles = true;

             }
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


            for (int m = 0; m < dataGridView1.Rows.Count; m++)
            {
                exceldt.Rows.Add();
                for (int n = 0; n < dataGridView1.Columns.Count-1; n++)
                {
                    if (dataGridView1.Rows[m].Cells[n].Value == null || dataGridView1.Rows[m].Cells[n].Value == DBNull.Value || String.IsNullOrEmpty(dataGridView1.Rows[m].Cells[n].Value.ToString()))
                        continue;

                    exceldt.Rows[m][n] = dataGridView1.Rows[m].Cells[n].Value.ToString();
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
