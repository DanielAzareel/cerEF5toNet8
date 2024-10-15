using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DGSyTI_WEB.Models;
namespace DGSyTI_WEB
{
    public class CodeBase64
    {
        public static String decodeVariable(String encodedData)
        {
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
                string returnValue = System.Text.UTF8Encoding.UTF8.GetString(encodedDataAsBytes);
                return returnValue.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return String.Empty;
        }

        public static String encodVariable(String toEncode)
        {
            try
            {
                byte[] toEncodeAsBytes = System.Text.UTF8Encoding.UTF8.GetBytes(toEncode.ToCharArray());
                string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
                return returnValue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return String.Empty;
        }
    }

}
