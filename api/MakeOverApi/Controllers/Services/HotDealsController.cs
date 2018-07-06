using MakeOverApi.Business;
using MakeOverApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MakeOverApi.Controllers.HotDeals
{
    public class HotDealsController : ApiController
    {
        public ResponseDto Get()
        {
            try
            {
                HtmlParser parser = new HtmlParser();
                return new ResponseDto()
                {
                    faceId = "6aeef070-aeb0-40f2-8cf2-6cf3ec415e76",
                    prodlist = parser.GetHotDeals()
                };
            }
            catch (Exception ex)
            {
                return null;
            }          
        }
    }
}
