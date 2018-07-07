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
            HtmlParser parser = new HtmlParser();
            return new ResponseDto()
            {
                faceId = "DUMMY-ID-FOR-HOT-DEALS", // no faceid for hot deals
                prodlist = parser.GetHotDeals()
            };

        }
    }
}
