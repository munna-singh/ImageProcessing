using MakeOverApi.Business;
using MakeOverApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;



namespace MakeOverApi.Controllers.HotDeals
{
    public class SuggestController : ApiController
    {

        [HttpPost]
        public ResponseDto Post(FaceDataRequestDto[] faceDto)
        {

           //request is array of factDto data - we are dealing with Single face id only - so take first or default
           var faceInfo = faceDto.FirstOrDefault();

            //determine male or female
            var gender = faceInfo.faceAttributes.gender == GenericConstantData.Female ? 0 : 1;

            //having specs or glasses or none
            var glass = faceInfo.faceAttributes.glasses;

            var glassType = GenericConstantData.NoGlasses;
            if (glass.StartsWith("Reading")) glassType = CategoryCode.Specs;
            else if (glass.StartsWith("Sun")) glassType = CategoryCode.SunGlass;

          //beard??
          var hasBeard = gender>0 &&  faceInfo.faceAttributes.facialHair.beard > 0.4;

            //read all data from GenderData Table
            using (var context = new makeoverEntities())
            {
                var genderData = context.GenderDatas.Where(g=>g.Gender == gender);

                ////////// PRIORITY ///////////////////
                //1 - Smile takes high priority - CODE here for smile

                //2 - No smile - Male 
                // ---------- Has Beard?
                if (gender > 0 && hasBeard)
                    genderData = genderData.Where(g => g.CategoryCode == CategoryCode.Beard);

                // ---------- Has No Beard? - Go for glass check
                if (gender > 0 && !hasBeard)
                {
                    //3 -- check for glasses
                    if (glassType != GenericConstantData.NoGlasses)
                        genderData = genderData.Where(g => g.CategoryCode == glassType);
                    else
                        //4--- no glasses return watches (general category)
                        genderData = genderData.Where(g => g.CategoryCode == CategoryCode.General);

                }

                //2 - No smile - Female
                if (gender == 0)
                {
                    //3 --check for glasses
                    if (glassType != GenericConstantData.NoGlasses)
                        genderData = genderData.Where(g => g.CategoryCode == glassType);
                    else
                        //4--- no glasses return watches (general category)
                        genderData = genderData.Where(g => g.CategoryCode == CategoryCode.General); //TODO once smile is taken care - change to gen category
                }

                var firstMatch = genderData.FirstOrDefault();

                //return dynamic response here
                HtmlParser parser = new HtmlParser();
                var suggestionList = parser.GetBestSellers(firstMatch.ShortUrl);

                //TODO: if suggestionList is empty then return default prodlist that is working - WATCH for men, or ReadingGlass for women
                
                return new ResponseDto()
                {
                    faceId = faceInfo.faceId,
                    prodlist = suggestionList
                };
                             
            }         
        }

    }
}
