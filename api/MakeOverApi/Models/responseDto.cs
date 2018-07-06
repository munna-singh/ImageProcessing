using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MakeOverApi.Models
{
    public class Prodlist
    {
        public string imageUrl { get; set; }
        public string title { get; set; }
        public string rating { get; set; }
        public string actualprice { get; set; }
        public string offerPrice { get; set; }
    }

    public class ResponseDto
    {
        public string faceId { get; set; }
        public List<Prodlist> prodlist { get; set; }
    }
}