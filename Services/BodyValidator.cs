using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebToPdf.Models;

namespace WebToPdf.Services
{
    public class BodyValidator
    {
        static public BodyError UrlList(string[] Urls)
        {            
            if(Urls == null || Urls.Length == 0) 
            {
                BodyError errorUrls = new(){
                    Error = true,
                    Message = "Las Urls son requeridas"
                };
                return errorUrls;
            }

            else if(Urls.Length > 6)
            {
                BodyError errorMax = new(){
                    Error = true,
                    Message = "Se puede procesar máximo 6 urls"
                };

                return errorMax;
            }

            BodyError newError = new(){
                Error = false,
                Message = ""
            };

            return newError;

        }
    }
}