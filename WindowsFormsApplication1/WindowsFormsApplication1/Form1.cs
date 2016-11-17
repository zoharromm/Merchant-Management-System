using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool result = false;
        HtmlAgilityPack.HtmlDocument _Document2 = new HtmlAgilityPack.HtmlDocument();
        List<string> urls = new List<string>();
        public Form1()
        {
            InitializeComponent();
           
            urls.Add("http://store.401games.ca/catalog/94040C/action-figures-apparel#st=&begin=1&nhit=40&dir=asc&cat=94040");
            urls.Add("http://store.401games.ca/catalog/94040C/action-figures-apparel#st=&begin=41&nhit=40&dir=asc&cat=94040");

           
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                _Document2.LoadHtml(webBrowser1.DocumentText.ToString());
                while (!result)
                {
                    if (_Document2.DocumentNode.SelectNodes("//div[@class=\"pages\"]/ul/li") != null)
                    {
                        result = true;
                    }
                    else
                    {
                        Application.DoEvents();

                    }
                }

            }
            catch
            {

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(string url in urls)
            {
                result=false;
                webBrowser1.Navigate(url);
                while (!result)
                {
                    Application.DoEvents();
                }

            }
        }
    }
}
