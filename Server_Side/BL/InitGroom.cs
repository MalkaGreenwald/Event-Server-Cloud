using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
        public static WebResult<List<ImageEntity>> InsertGroom(List<String> base64arr)
        {

            List<ImageEntity> images = null;
            try
            {
                images = Images.GetImages().Value;
                if (base64arr.Count == 2)
                {
                    var base64 = Regex.Replace(base64arr[0], @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
                    byte[] byteArray = System.Convert.FromBase64String(base64);
                    MemoryStream stream = new MemoryStream(byteArray);
                    string urlStorage = InitImages.SendToStorage(base64arr[1], stream);
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
                    groom.name = base64arr[1];
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
                        Value = Images.GetImages().Value
                    };
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