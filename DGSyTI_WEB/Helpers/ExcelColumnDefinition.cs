using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace DGSyTI_WEB.Helpers
{
    public class ExcelColumnDefinition
    {
        public string Format { get; set; }
        public string Header { get; set; }
        public Expression Expression { get; set; }

        public static ExcelColumnDefinition Create<T>(Expression<Func<T, object>> expression, string format = null, string header = null)
        {
            return new ExcelColumnDefinition { Expression = expression, Format = format, Header = header };
        }
    }
}