using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGSyTI_WEB.Helpers
{
    public class ExcelFormat
    {
        public const string Integer = "0";
        public const string Decimal = "[=0]0;.##";
        public const string Date = "mm/dd/yyyy";
        public const string DateTime = "mm/dd/yyyy hh:mm:ss AM/PM";
        public const string Money = "\"$\"#,##0.00;[Red]\"$\"#,##0.00";
        public const string Percent = "0.00%;[Red]-0.00%";
        public const string Time = "hh:mm AM/PM";
    }
}