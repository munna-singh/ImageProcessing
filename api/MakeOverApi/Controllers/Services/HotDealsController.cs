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
            return new ResponseDto()
            {
                faceId = "6aeef070-aeb0-40f2-8cf2-6cf3ec415e76",
                prodlist = new List<Prodlist>()
                {
                    new Prodlist()
                    {
                        imageUrl = "https://images-eu.ssl-images-amazon.com/images/I/71oh4nqgS0L._AC_SY230_.jpg",
                        title= "Magic Attitude Fisherman Denim Cap",
                        rating="4",
                        actualprice="Rs.599",
                        offerPrice="Rs.299"
                    },
                    new Prodlist()
                    {
                        imageUrl = "https://images-eu.ssl-images-amazon.com/images/I/81Wlvm5weoL._AC_SY230_.jpg",
                        title= "KIDANIA Kids Cotton Fancy Hat/Cap for Girls",
                        rating="3",
                        actualprice="Rs.499",
                        offerPrice="Rs.199"
                    },
                      new Prodlist()
                    {
                        imageUrl = "https://images-eu.ssl-images-amazon.com/images/I/81aTY-m4PKL._AC_SY230_.jpg",
                        title= "Zacharias Girl's Visor Tennis Cap",
                        rating="3.5",
                        actualprice="Rs.200",
                        offerPrice="Rs.299"
                    }
                }
            };
        }
    }
}
