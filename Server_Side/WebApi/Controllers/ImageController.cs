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
        //[RequireHttps]
        public async Task<IHttpActionResult> InsertImages([FromBody]List<String> base64arr)
        {
                    return Ok(await InitImages.InsertImages(base64arr));
        }
        [Route("GetImages")]
        [HttpGet]
        //[RequireHttps]
        public IHttpActionResult GetImages()
        {
            return Ok(Images.GetImages());
        }
        [Route("InsertGroom")]
        [HttpPost]
        //[RequireHttps]
        public IHttpActionResult InsertImagesGroom(List<String> base64arr)
        {
            return Ok(InitGroom.InsertGroom(base64arr));
        }

        [Route("DeleteImage")]
        [HttpPost]
        //[RequireHttps]
        public IHttpActionResult DeleteImage(ImageEntity img)
        {
            return Ok(Images.DeleteImg(img.url));
        }
        [Route("UndoDelete")]
        [HttpPost]
        //[RequireHttps]
        public IHttpActionResult UndoDelete(ImageEntity img)
        {
            return Ok(Images.UndoDelete(img.url));
        }

        [Route("getRecycleBin")]
        [HttpGet]
        //[RequireHttps]
        public IHttpActionResult GetRecycleBin()
        {
            return Ok(Images.getRecycleBin());
        }

        [Route("HasGroom")]
        [HttpGet]
        //[RequireHttps]
        public IHttpActionResult HasGroom()
        {
            return Ok(Images.HasGroom());
        }

        [Route("Reset")]
        [HttpGet]
        //[RequireHttps]
        public IHttpActionResult Reset()
        {
            return Ok(Images.Reset());
        }
        [Route("ElectronDetailsForImage")]
        [HttpPost]
        public IHttpActionResult ElectronDetailsForImage(string base64)
        {
            return Ok();
        }

    }
}