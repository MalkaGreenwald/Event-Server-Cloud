using BL;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
//using System.Web.Mvc;
using Entities;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("WebApi/Image")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImageController : ApiController
    {
        [Route("InsertImages")]
        [HttpPost]
        [RequireHttps]
        public async Task<IHttpActionResult> InsertImages()
        {
            return Ok(await InitImages.InsertImages());
        }
        [Route("GetImages")]
        [HttpGet]
        [RequireHttps]
        public IHttpActionResult GetImages()
        {
            return Ok(Images.GetImages());
        }
        [Route("InsertGroom")]
        [HttpPost]
        [RequireHttps]
        public IHttpActionResult InsertImagesGroom()
        {
            return Ok(InitGroom.InsertGroom());
        }

        [Route("DeleteImage")]
        [HttpPost]
        [RequireHttps]
        public IHttpActionResult DeleteImage(ImageEntity img)
        {
            return Ok(Images.DeleteImg(img.url));
        }

        [Route("getRecycleBin")]
        [HttpGet]
        [RequireHttps]
        public IHttpActionResult GetRecycleBin()
        {
            return Ok(Images.getRecycleBin());
        }

        [Route("HasGroom")]
        [HttpGet]
        [RequireHttps]
        public IHttpActionResult HasGroom()
        {
            return Ok(Images.HasGroom());
        }

    }
}