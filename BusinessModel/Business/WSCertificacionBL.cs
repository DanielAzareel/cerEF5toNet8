using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Persistence;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BusinessModel.Business
{
    public class WSCertificacionBL
    {
   

        public RExiste ExisteCertificado(string curp, string  tokenDes, string insId)
        {
            RExiste resultado = new RExiste();

            try
            {
                List<int> estatus = new List<int>() { 4 };
                //var certificados = new CerDocumentoDAL().GetDocumentoCurp(curp, estatus, EncriptarAES.EncryptStringAES(tokenDes), insId);
                var certificados = new CerDocumentoDAL().GetDocumentoCurp(curp, estatus,  tokenDes, insId);
                if (certificados != null && certificados.Any())
                {

                    string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                    resultado.Existe = true;
                    resultado.Token = HttpUtility.UrlEncode(EncriptarAES.EncryptStringAES(curp+"|"+ insId + "|"+fecha));
                   // resultado.Token = EncriptarAES.DecryptStringAES(resultado.Token);
                    return resultado;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return resultado;
            }

        }

        public bool getAutenticacionDescarga(string token, string claveConcentradora)
        {
            bool acceso = false;

            WSAutenticacionDAL wsAuth = new WSAutenticacionDAL();
           // acceso = wsAuth.boolAutenticacionDescarga(EncriptarAES.EncryptStringAES(token), claveConcentradora);
            acceso = wsAuth.boolAutenticacionDescarga(token, claveConcentradora);
            return acceso;
        }

    }
}
