using HtmlAgilityPack;
using MakeOverApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MakeOverApi.Business
{
    public class HtmlParser
    {
        const string HOT_DEALS_URL = "https://www.amazon.in/gp/goldbox";

        public List<ProductCatalogue> ParseHtmlResponse()
        {
            List<ProductCatalogue> productList = new List<ProductCatalogue>();
            
            try
            {
                GetHotDeals();

                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load("https://amzn.to/2tYHjT8 ");
                var orderedListRoot = htmlDoc.DocumentNode.Descendants("body").SelectMany(y => y.Descendants("ol").Where(item => item.Attributes["role"].Value == "list")).ToList();
                var listItems = orderedListRoot.First().Descendants("li").ToList();


                //HtmlDocument

                //id="widgetContent"
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }

            return productList;
        }

        public List<ProductCatalogue> GetHotDeals()
        {

            List<HtmlNode> result = new List<HtmlNode>();
            ProductCatalogue product = new ProductCatalogue();
            List<ProductCatalogue> productList = new List<ProductCatalogue>();
            string dealPriceRangeText = "";

            try
            {               

                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load("https://amazon.in");
                var orderedListRoot = htmlDoc.DocumentNode.Descendants("body").SelectMany(y => y.Descendants("div").Where(item => item.Attributes["class"] != null && item.Attributes["class"].Value == "a-section a-spacing-none shogun-widget deals-shoveler aui-desktop fresh-shoveler")).ToList();
                var ulList = orderedListRoot.First().Descendants("ul").ToList();
                var listItems = ulList.FirstOrDefault().Descendants("li").ToList(); 

                //All Deals List <li>
                foreach (var listItem in listItems)
                {
                    //Product Description under class = "deals-shoveler-card-image"
                    var imageDivs = listItem.Descendants("div").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "deals-shoveler-card-image").ToList();
                    var imgTag = imageDivs.SelectMany(item => item.Descendants("img").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "product-image")).ToList().SingleOrDefault();

                    //Price list under class="deals-shoveler-card-bottom"
                    var priceDivsList = listItem.Descendants("div").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "deals-shoveler-card-bottom").ToList();

                    //Deal Price DIV container under class="dealPrice"
                    var dealPrice = priceDivsList.SelectMany(item => item.Descendants("div").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "dealPrice")).ToList();
                    //Deal Price under tag="span"
                    var dealPriceTags = dealPrice.SelectMany(span => span.Descendants("span"));

                    //Original Price DIV container under class="dealListPrice"
                    var originalPrice = priceDivsList.SelectMany(item => item.Descendants("div").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "dealListPrice")).ToList();
                    //Original Price under tag="span"
                    var originalPriceTags = originalPrice.SelectMany(span => span.Descendants("span"));

                    //If no Original price, then take the offer price range
                    if (originalPriceTags.Count() == 0)
                    {
                        var dealPriceRangeTag = dealPrice.SelectMany(span => span.Descendants("span").Where(attr => attr.Attributes["class"] != null && attr.Attributes["class"].Value == "aok-hide-text")).SingleOrDefault();
                        dealPriceRangeText = dealPriceRangeTag.InnerText;
                    }

                    //Create Product item
                    product = new ProductCatalogue();
                    product.title = imgTag.Attributes["alt"].Value;
                    product.imageUrl = imgTag.Attributes["src"].Value;

                    //if original price not found then offer price only exists
                    if (originalPriceTags.Count() > 0)
                    {
                        //product.actualprice = originalPriceTags.First().InnerText;
                        product.offerPrice = dealPriceTags.First().InnerText;
                    }
                    else
                    {
                        product.offerPrice = dealPriceRangeText.Replace("from", "").TrimStart();
                    }

                    //If original Price exists then assign else not
                    if (originalPriceTags.Count() > 0)
                    {
                        product.actualprice = originalPriceTags.First().InnerText;
                    }

                    productList.Add(product);

                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return productList;
        }

    }
   
   
}
