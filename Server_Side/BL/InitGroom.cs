using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BL.Helpers;
using Entities;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BL
{
    public class InitGroom
    {
        public static WebResult<List<ImageEntity>> InsertGroom()
        {

            List<ImageEntity> images = null;
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                var httpRequest = HttpContext.Current.Request;
                images = Images.GetImages().Value;
                if (httpRequest.Files.Count == 1)
                {

                    var postedFile = httpRequest.Files[0];
                    if (string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(postedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(postedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
                    {
                        string urlStorage = InitImages.SendToStorage(postedFile.FileName, postedFile.InputStream);
                        if (urlStorage == "")
                            return new WebResult<List<ImageEntity>>()
                            {
                                Status = false,
                                Message = "failed to load image",
                                Value = images
                            };
                        GroomEntity groom = new GroomEntity();
                        groom.url = urlStorage;
                        string token = getFaceToken(urlStorage);
                        if (token == "")
                            return new WebResult<List<ImageEntity>>()
                            {
                                Status = false,
                                Message = "failed",
                                Value = images
                            };
                        groom.token = token;
                        groom.name = postedFile.FileName;
                        if (GroomEntity.Get().ToList().Count > 0)
                        {
                            GroomEntity.RemoveAll();
                            groom.Add();
                            InitImagesAfterChangeGroom();
                            images = Images.GetImages().Value;
                            return new WebResult<List<ImageEntity>>()
                            {
                                Status = true,
                                Message = "Ok",
                                Value = images
                            };
                        }
                        groom.Add();
                        return new WebResult<List<ImageEntity>>()
                        {
                            Status = true,
                            Message = "Ok",
                            Value = images
                        };
                    }
                }

                return new WebResult<List<ImageEntity>>()
                {
                    Status = false,
                    Message = "That's not an image",
                    Value = images
                };

            }
            catch (Exception e)
            {
                return new WebResult<List<ImageEntity>>()
                {
                    Status = false,
                    Message = e.Message,
                    Value = images
                };
            }
        }

        private static void InitImagesAfterChangeGroom()
        {
            List<ImageEntity> images = ImageEntity.Get();
            for (int i = 0; i < images.Count; i++)
            {
                var res = InitImages.getResultFacePP(images[i].url);
                bool isGroom = InitImages.IsGroom(res);
                ImageEntity.UpdateIsGroom(images[i].url, isGroom);
            }
        }

        private static string getFaceToken(string url)
        {

            const string API_Key = "Yoxjj0Tu2hUPY5D5K-iQ4ZkJoGm2W2r3";
            const string API_Secret = "q97dj2QUarKmaC5NdTOdBXjC4XI2USta";
            const string BaseUrl = "https://api-us.faceplusplus.com/facepp/v3/detect";
            IRestClient _client = new RestClient(BaseUrl);
            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("api_key", API_Key);
            request.AddParameter("api_secret", API_Secret);
            request.AddParameter("image_url", url);
            request.AddParameter("return_attributes", "eyestatus");
            try
            {
                var response = _client.Execute(request);
                JObject results = JObject.Parse(response.Content);
                string faceToken = results["faces"].First.Last.First.ToString();
                return faceToken;

            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}