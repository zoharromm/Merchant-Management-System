using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Product
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Weight { get; set; }
        public decimal Shipping { get; set; }
        public string Bulletpoints { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
        public string Stock { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public bool Isparent { get; set; }
        public string parentsku { get; set; }
        public string Bulletpoints1 { get; set; }
        public string Bulletpoints2 { get; set; }
        public string Bulletpoints3 { get; set; }
        public string Bulletpoints4 { get; set; }
        public string Bulletpoints5 { get; set; }
        public string Category { get; set; }
        public string Style { get; set; }
        public int MinimumAgeRecommend { get; set; }
        public string AgeUnitMeasure { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string WeightUnitMeasure { get; set; }
        public string UPC { get; set; }
        public string ManPartNO { get; set; }
        public bool ISGtin { get; set; }
    }
    public class ProductMerge
    {
        public bool ProductDatabaseIntegration(List<Product> Products, string StoreName, int ErrorTypeID)
        {
            #region delete Products with no sku and name information
            Mail _Mail = new Mail();

            List<Product> PrdListSku = (from _prd in Products
                                        where _prd.SKU != null
                                        select _prd).ToList();
            List<Product> PrdListName = (from _prd in PrdListSku
                                         where _prd.Name != null
                                         select _prd).ToList();
            PrdListName.ForEach(x => x.Name = x.Name.Replace("- Previously Played", ""));
            PrdListName.ForEach(x => x.Name = x.Name.Replace("-Previously Played", ""));
            PrdListName.ForEach(x => x.Name = x.Name.Replace("Previously Played", ""));
            PrdListName.ForEach(x => x.Name = x.Name.Replace("previously Played", ""));
            PrdListName.ForEach(x => x.Name = x.Name.Replace("Previously played", ""));
            PrdListName.ForEach(x => x.Name = x.Name.Replace("previously played", ""));

            List<Product> PrdListPrice = (from _prd in PrdListName
                                          where _prd.Price != "0"
                                          select _prd).ToList();
            List<Product> PrdList1 = (from _prd in PrdListPrice
                                      where _prd.SKU.Trim() != "" && _prd.Name.Trim() != ""
                                      select _prd).ToList();
            List<Product> PrdList2 = (from _prd in PrdList1
                                      where !_prd.Name.Trim().ToLower().Contains("pre-order")
                                      select _prd).ToList();
            List<Product> PrdList3 = (from _prd in PrdList2
                                      where !_prd.Name.Trim().ToLower().Contains("pre order")
                                      select _prd).ToList();
            List<Product> PrdList4 = (from _prd in PrdList3
                                      where !_prd.Name.Trim().ToLower().Contains("gift card")
                                      select _prd).ToList();
            List<Product> PrdList5 = (from _prd in PrdList4
                                      where !_prd.Name.Trim().ToLower().Contains("tournament entry")
                                      select _prd).ToList();
            List<Product> PrdList6 = (from _prd in PrdList5
                                      where !_prd.Name.Trim().ToLower().Contains("regionals")
                                      select _prd).ToList();
            List<Product> PrdList7 = (from _prd in PrdList6
                                      where _prd.Description != null
                                      select _prd).ToList();
            List<Product> PrdList8 = (from _prd in PrdList7
                                      where _prd.Bulletpoints1 != null
                                      select _prd).ToList();
            List<Product> PrdList9 = (from _prd in PrdList8
                                      where !_prd.Description.Trim().ToLower().Contains("pre-order")
                                      select _prd).ToList();
            List<Product> PrdList10 = (from _prd in PrdList9
                                       where !_prd.Description.Trim().ToLower().Contains("pre order")
                                       select _prd).ToList();
            List<Product> PrdList11 = (from _prd in PrdList10
                                       where !_prd.Bulletpoints1.Trim().ToLower().Contains("pre order")
                                       select _prd).ToList();
            List<Product> PrdList12 = (from _prd in PrdList11
                                       where !_prd.Bulletpoints1.Trim().ToLower().Contains("pre-order")
                                       select _prd).ToList();
            List<Product> PrdList13 = (from _prd in PrdList12
                                       where _prd.Image != null
                                       select _prd).ToList();
            List<Product> PrdList = (from _prd in PrdList13
                                     where _prd.Image.Trim() != ""
                                     select _prd).ToList();
            List<Product> query1 = (PrdList.GroupBy(x => x.SKU).Select(y => y.FirstOrDefault())).ToList();
            List<Product> query = new List<Product>();
            //    if (StoreName.ToLower() == "bestbuy.ca")
            query = query1;
            //else
            //    query = (query1.GroupBy(x => x.Name).Select(y => y.FirstOrDefault())).ToList();
            #endregion delete Products with no sku and name information
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
            exceldt.Columns.Add("Style", typeof(string));
            exceldt.Columns.Add("Shipping", typeof(string));
            exceldt.Columns.Add("Minimum_Manufacturer_Age_Recommended", typeof(string));
            exceldt.Columns.Add("AGE_Unit_Of_Measure", typeof(string));
            exceldt.Columns.Add("Width", typeof(string));
            exceldt.Columns.Add("Height", typeof(string));
            exceldt.Columns.Add("WeightUnitMeasure", typeof(string));
            exceldt.Columns.Add("UPC", typeof(string));
            exceldt.Columns.Add("ManPartNO", typeof(string));
            exceldt.Columns.Add("ISGtin", typeof(string));
            try
            {
                int Counter = 0;
                foreach (Product Prd in query)
                {
                    string Brand = string.IsNullOrEmpty(Prd.Brand) ? "" : Prd.Brand.ToUpper();

                    if (Brand != "SOLOGEAR" && Brand != "OCTANE SEATING")
                    {
                        try
                        {

                            exceldt.Rows.Add();
                            exceldt.Rows[Counter][0] = Counter;
                            exceldt.Rows[Counter][1] = string.IsNullOrEmpty(Prd.SKU) ? "" : Regex.Replace(Prd.SKU, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][2] = string.IsNullOrEmpty(Prd.Name) ? "" : Regex.Replace(Prd.Name, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][3] = string.IsNullOrEmpty(Prd.Description) ? "" : Regex.Replace(Prd.Description, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][4] = string.IsNullOrEmpty(Prd.Bulletpoints) ? "" : Regex.Replace(Prd.Bulletpoints, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][5] = string.IsNullOrEmpty(Prd.Manufacturer) ? "" : Regex.Replace(Prd.Manufacturer, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][6] = string.IsNullOrEmpty(Prd.Brand) ? "Default Manufacturer" : Regex.Replace(Prd.Brand, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][7] = Prd.Price;
                            exceldt.Rows[Counter][8] = Prd.Currency;
                            exceldt.Rows[Counter][9] = Prd.Stock;
                            exceldt.Rows[Counter][10] = string.IsNullOrEmpty(Prd.Image) ? "" : Regex.Replace(Prd.Image, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][11] = string.IsNullOrEmpty(Prd.URL) ? "" : Regex.Replace(Prd.URL, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][12] = string.IsNullOrEmpty(Prd.Size) ? "" : Regex.Replace(Prd.Size, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][13] = string.IsNullOrEmpty(Prd.Color) ? "" : Regex.Replace(Prd.Color, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][14] = Prd.Isparent;
                            exceldt.Rows[Counter][15] = string.IsNullOrEmpty(Prd.parentsku) ? "" : Regex.Replace(Prd.parentsku, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][16] = string.IsNullOrEmpty(Prd.Bulletpoints1) ? "" : Regex.Replace(Prd.Bulletpoints1, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][17] = string.IsNullOrEmpty(Prd.Bulletpoints2) ? "" : Regex.Replace(Prd.Bulletpoints2, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][18] = string.IsNullOrEmpty(Prd.Bulletpoints3) ? "" : Regex.Replace(Prd.Bulletpoints3, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][19] = string.IsNullOrEmpty(Prd.Bulletpoints4) ? "" : Regex.Replace(Prd.Bulletpoints4, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][20] = string.IsNullOrEmpty(Prd.Bulletpoints5) ? "" : Regex.Replace(Prd.Bulletpoints5, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][21] = string.IsNullOrEmpty(Prd.Category) ? "" : Regex.Replace(Prd.Category, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][22] = Prd.Weight;
                            exceldt.Rows[Counter][23] = string.IsNullOrEmpty(Prd.Style) ? "" : Regex.Replace(Prd.Style, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][24] = Prd.Shipping;
                            exceldt.Rows[Counter][25] = Prd.MinimumAgeRecommend;
                            exceldt.Rows[Counter][26] = string.IsNullOrEmpty(Prd.AgeUnitMeasure) ? "" : Regex.Replace(Prd.AgeUnitMeasure, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][27] = string.IsNullOrEmpty(Prd.Width) ? "" : Regex.Replace(Prd.Width, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][28] = string.IsNullOrEmpty(Prd.Height) ? "" : Regex.Replace(Prd.Height, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][29] = string.IsNullOrEmpty(Prd.WeightUnitMeasure) ? "" : Regex.Replace(Prd.WeightUnitMeasure, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][30] = string.IsNullOrEmpty(Prd.UPC) ? "" : Regex.Replace(Prd.UPC, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][31] = string.IsNullOrEmpty(Prd.ManPartNO) ? "" : Regex.Replace(Prd.ManPartNO, @"\\t|\n|\r", " ").Replace("\t", " ");
                            exceldt.Rows[Counter][32] = Prd.ISGtin;
                        }
                        catch
                        {

                        }
                        Counter++;
                    }
                }

                #region DBChanges
                int ProcessedStatus = 1;
                DB _Db = new DB();

                #region MarkProductsAsOutofStock
                try
                {
                    _Db.GetDatasetByPassDatatable("MarkProductsAsOutOfStock", exceldt, "@Products", CommandType.StoredProcedure, "@StoreName," + StoreName);
                }
                catch
                {
                    _Mail.SendMail("OOPS there is issue accured in mark products as out of stock in database " + StoreName, ", due to which all products that is going out of stock on website is remain in stock on amazon.", false, true, ErrorTypeID);
                }
                #endregion MarkProductsAsOutofStock

                int TotalNoOfRecords = exceldt.Rows.Count;
                int NoOfFeeds = 0;
                if (TotalNoOfRecords % 2000 == 0)
                    NoOfFeeds = Convert.ToInt32(TotalNoOfRecords / 2000);
                else
                    NoOfFeeds = Convert.ToInt32(TotalNoOfRecords / 2000) + 1;
                for (int i = 0; i < NoOfFeeds; i++)
                {
                    if (!_Db.ProductInsert(StoreName, exceldt.AsEnumerable().Skip(i * 2000).Take(2000).CopyToDataTable()))
                    {
                        _Mail.SendMail("OOPS issue accured in insertion of products in database for store " + StoreName, "Issue Occured In insertion of products in database", false, true, ErrorTypeID);
                        ProcessedStatus = 0;
                    }
                }
                if (exceldt.Rows.Count == 0)
                {
                    _Mail.SendMail("OOPS there is no any product go to insert in database for " + StoreName + ", due to which all products of this store have updated inventory to 0 in database", "Issue Occured In insertion of products in database", false, true, ErrorTypeID);
                    ProcessedStatus = 0;
                }
                _Db.ExecuteCommand("update Schduler set LastProcessedStatus=" + ProcessedStatus + " where StoreName='" + StoreName + "'");
                #endregion DBChanges
                return true;
            }
            catch (Exception exp)
            {
                _Mail.SendMail("OOPS issue accured in insertion of products for store " + StoreName + " exp=" + exp.Message, "Issue Occured In insertion of products in database", false, true, ErrorTypeID);

                return false;
            }
        }
    }
    public class SyncerProductData
    {
        public void SyncInventory(List<InventorySync> Products, string Prefix, int type)
        {
            try
            {
                List<InventorySync> PrdListSku = (from _prd in Products
                                                  where _prd.SKU != null
                                                  select _prd).ToList();

                List<InventorySync> PrdListPrice = (from _prd in PrdListSku
                                                    where _prd.Price != "0"
                                                    select _prd).ToList();
                List<InventorySync> PrdList1 = (from _prd in PrdListPrice
                                                where _prd.SKU.Trim() != ""
                                                select _prd).ToList();
                if (PrdList1.Count() > 0)
                {
                    DataTable tbInventorySync = new System.Data.DataTable();
                    tbInventorySync.Columns.Add("sku");
                    tbInventorySync.Columns.Add("Inventory", Type.GetType("System.Int32"));
                    tbInventorySync.Columns.Add("Price", Type.GetType("System.Decimal"));
                    foreach (InventorySync prd in PrdList1)
                    {
                        decimal Price = 0;
                        int Inventory = 0;
                        decimal.TryParse(prd.Price, out Price);
                        int.TryParse(prd.Stock, out Inventory);

                        if (Price > 0)
                        {
                            DataRow row = tbInventorySync.NewRow();
                            row[0] = Prefix + prd.SKU;
                            row[1] = Inventory;
                            row[2] = Price;
                            tbInventorySync.Rows.Add(row);
                        }
                    }
                    if (tbInventorySync.Rows.Count > 0)
                    {
                        DB _Db = new DB();
                        _Db.GetDatasetByPassDatatable("Markeplacehub_Supplier_SyncInventory", tbInventorySync, "@Product", CommandType.StoredProcedure, "@Type," + type);
                    }

                }
            }
            catch
            {

            }
        }
    }

    public class InventorySync
    {
        public string SKU { get; set; }
        public string Stock { get; set; }
        public string Price { get; set; }
    }
}
