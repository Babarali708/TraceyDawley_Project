﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TraceyDawley.Filters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        public bool CheckException;

        public ExceptionFilter()
        {
            this.CheckException = true;
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (CheckException)
            {
                string controller = filterContext.RouteData.Values["controller"].ToString();
                string action = filterContext.RouteData.Values["action"].ToString();
                Exception e = filterContext.Exception;
                filterContext.ExceptionHandled = true;

                int line = (new StackTrace(e, true)).GetFrame(0).GetFileLineNumber();

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                    { "controller", "Error" },{ "action", "NotFoundPage" }, });
            }
        }
    }

}
