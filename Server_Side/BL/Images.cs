using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Helpers;
using Entities;

namespace BL
{
    public class Images
    {
        
        public static WebResult<List<ImageEntity>> GetImages()
        {
            try
            {
                List<ImageEntity> listEntity = new List<ImageEntity>();
                listEntity = ImageEntity.Get();
                return new WebResult<List<ImageEntity>>()
                {
                    Status=true,
                    Message="Ok",
                    Value=listEntity
                };

            }
            catch (Exception e)
            {
                return new WebResult<List<ImageEntity>>()
                {
                    Status = false,
                    Message = e.Message,
                    Value = null
                };
            }
        }

        public static WebResult<bool> DeleteImg(string url)
        {
            try
            {
                ImageEntity.UpdateRecycleBin(url, true);
                return new WebResult<bool>()
                {
                    Status = true,
                    Message = "Ok",
                    Value = true
                };
            }
            catch (Exception e)
            {
                return new WebResult<bool>()
                {
                    Status = false,
                    Message = e.Message,
                    Value = false
                };
            }
        }

        public static WebResult<List<ImageEntity>> getRecycleBin()
        {
            try
            {
                List<ImageEntity> rec = ImageEntity.Get().Where(r => r.isInRecycleBin != null && r.isInRecycleBin == true).ToList();
                return new WebResult<List<ImageEntity>>()
                {
                    Status = true,
                    Message = "Ok",
                    Value = rec
                };
            }
            catch (Exception e)
            {
                return new WebResult<List<ImageEntity>>()
                {
                    Status = false,
                    Message = e.Message,
                    Value = null
                };
            }
        }
        public static WebResult<bool> HasGroom()
        {
            try
            {
                if (GroomEntity.Get().ToList().Count == 0)
                    return new WebResult<bool>()
                    {
                        Status=true,
                        Message="Ok",
                        Value=false
                    };
                return new WebResult<bool>()
                {
                    Status = true,
                    Message = "Ok",
                    Value = true
                };
            }
            catch (Exception e)
            {
                return new WebResult<bool>()
                {
                    Status = false,
                    Message = e.Message,
                    Value = false
                };
            }

        }
    }

}