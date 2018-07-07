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

        public List<ProductCatalogue> GetBestSellers(string shortUrl)
        {
            List<HtmlNode> result = new List<HtmlNode>();
            ProductCatalogue product = new ProductCatalogue();
            List<ProductCatalogue> productList = new List<ProductCatalogue>();

            try
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(shortUrl);
                var orderedListRoot = htmlDoc.DocumentNode.Descendants("body").SelectMany(y => y.Descendants("ol")).ToList();
                if (orderedListRoot.Count == 0) return productList;

                var listItems = orderedListRoot.First().Descendants("li").Where(item => item.Attributes["class"] != null && item.Attributes["class"].Value == "a-carousel-card acswidget-carousel__card").ToList();

                foreach (var listItem in listItems)
                {
                    var imgTag = listItem.Descendants("img").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "aok-align-center").ToList();
                    //var imgTag = imageDivs.SelectMany(item => item.Descendants("img").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "product-image")).ToList().SingleOrDefault();

                    var prodTitle = listItem.Descendants("div").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "a-box-group a-spacing-top-micro acs_product-title").ToList();
                    // var dealPrice = priceDivsList.SelectMany(item => item.Descendants("div").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "dealPrice")).ToList();
                    var prodTitleSpantag = prodTitle.SelectMany(span => span.Descendants("span"));

                    var prodPriceDiv = listItem.Descendants("div").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "a-box-group a-size-small a-spacing-none acs_product-price").ToList();
                    var prodOriginalPriceSpanTag = prodPriceDiv.SelectMany(span => span.Descendants("span").Where(price => price.Attributes["class"] != null && price.Attributes["class"].Value == "a-size-mini a-color-secondary acs_product-price__list a-text-strike")).ToList();

                    var prodStarRatingSpanTag = prodPriceDiv.SelectMany(span => span.Descendants("span").Where(price => price.Attributes["class"] != null && price.Attributes["class"].Value == "a-size-mini a-color-secondary acs_product-price__list a-text-strike")).ToList();

                    //var prodOfferPrice = listItem.Descendants("div").Where(div => div.Attributes["class"] != null && div.Attributes["class"].Value == "a-size-base a-color-price acs_product-price__buying").ToList();
                    var prodOfferPriceSpantag = prodPriceDiv.SelectMany(span => span.Descendants("span").Where(price => price.Attributes["class"] != null && price.Attributes["class"].Value == "a-size-base a-color-price acs_product-price__buying")).ToList();

                    //if (originalPriceTags.Count() == 0)
                    //{
                    //    var dealPriceRangeTag = dealPrice.SelectMany(span => span.Descendants("span").Where(attr => attr.Attributes["class"] != null && attr.Attributes["class"].Value == "aok-hide-text")).SingleOrDefault();

                    //}

                    product = new ProductCatalogue();
                    product.title = prodTitleSpantag.FirstOrDefault().InnerText;
                    product.imageUrl = imgTag.FirstOrDefault().Attributes["src"].Value;
                    if (prodOriginalPriceSpanTag.Count() > 0)
                    {
                        //"&#x20B9;" this is html code for INR (Indian Rupee), no font available, so removing for now.
                        product.actualprice = prodOriginalPriceSpanTag.First().InnerText.Replace("&#x20B9;","").Replace("&nbsp;", "").Trim();
                    }

                    if (prodOfferPriceSpantag.Count() > 0)
                    {
                        product.offerPrice = prodOfferPriceSpantag.First().InnerText.Replace("&#x20B9;", "").Replace("&nbsp;", "").Trim();
                    }

                    productList.Add(product);
                }


                //HtmlDocument

                //id="widgetContent"
            }
            catch (Exception ex)
            {
                throw;
            }

            return productList;
        }

    }
   
}
