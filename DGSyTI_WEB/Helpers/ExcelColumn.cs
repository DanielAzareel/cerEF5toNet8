using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace DGSyTI_WEB.Helpers
{
    public class ExcelColumn<T>
    {
        public ExcelColumn()
        {

        }

        public ExcelColumnDefinition Column(Expression<Func<T, object>> expression, string header = null, string format = null)
        {
            return ExcelColumnDefinition.Create(expression, format, header);
        }
    }
}