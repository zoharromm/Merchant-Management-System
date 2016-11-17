using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bestbuy
{
    class ComonFunction
    {
    }
    public class Pickup
    {
        public string status { get; set; }
        public bool purchasable { get; set; }
    }

    public class Shipping
    {
        public string status { get; set; }
        public bool purchasable { get; set; }
    }

    public class Availability
    {
        public Pickup pickup { get; set; }
        public Shipping shipping { get; set; }
        public string sku { get; set; }
        public string saleChannelExclusivity { get; set; }
        public bool scheduledDelivery { get; set; }
        public bool isGiftCard { get; set; }
        public bool isService { get; set; }
    }

    public class RootObject
    {
        public List<Availability> availabilities { get; set; }
        public PdpProduct pdpProduct { get; set; }
    }

    #region pdproduct
    public class AdditionalMedia
    {
        public string thumbnailUrl { get; set; }
        public string url { get; set; }
        public string mimeType { get; set; }
    }

    public class Seller
    {
        public object name { get; set; }
        public object id { get; set; }
        public string url { get; set; }
    }

    public class PdpProduct
    {
        public string sku { get; set; }
        public string name { get; set; }
        public double regularPrice { get; set; }
        public double salePrice { get; set; }
        public string thumbnailImage { get; set; }
        public string productUrl { get; set; }
        public double ehf { get; set; }
        public bool hideSavings { get; set; }
        public bool isSpecialDelivery { get; set; }
        public object saleEndDate { get; set; }
        public List<AdditionalMedia> additionalMedia { get; set; }
        public Seller seller { get; set; }
        public bool isMarketplace { get; set; }
        public string brandName { get; set; }
    }


    #endregion pdpproduct

}
