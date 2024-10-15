using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerUACDocumentoDAL
    {
        public List<cerUACDocumento> GetListUACDocumento(List<string> lstDocumentoId)
        {
            List<cerUACDocumento> listcerUACDocumento = new List<cerUACDocumento>();
            try
            {

                CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                listcerUACDocumento = (from UACDocumento in oCertificadosMediaSuperiorEntities.cerUACDocumento
                                       where lstDocumentoId.Contains(UACDocumento.docId)
                                       select UACDocumento).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listcerUACDocumento;

        }

        public List<cerUACDocumento> GetListUACDocumento(string sIdDocumento)
        {
            List<cerUACDocumento> listcerUACDocumento = new List<cerUACDocumento>();
            try
            {

                CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                listcerUACDocumento = (from UACDocumento in oCertificadosMediaSuperiorEntities.cerUACDocumento
                                       where UACDocumento.docId == sIdDocumento
                                       select UACDocumento).OrderBy(x=> x.orden).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listcerUACDocumento;

        }
    }
}
