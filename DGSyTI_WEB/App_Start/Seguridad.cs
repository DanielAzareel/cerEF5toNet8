using Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGSyTI_WEB.Controllers
{
    public class ValidarSesionAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = Usuario.GetUser();

            if (user == null)
            {
                var urlSieg = ConfigurationManager.AppSettings["urlSieg"];
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var contentResult = new ContentResult();
                    contentResult.Content = "<script>window.location.replace('" + urlSieg + "');</script>";
                    contentResult.ContentType = "text/html; charset=utf-8";
                    filterContext.Result = contentResult;
                }
                else
                {
                    filterContext.Result = new RedirectResult(urlSieg);
                }
            }
        }
    }
}