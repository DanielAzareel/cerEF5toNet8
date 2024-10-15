using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerCompetenciaDocumentoDAL
    {
        public List<cerCompetenciaDocumento> GetCompetenciaByListIdDocumento(List<string> lstIdDocumento)
        {
            List<cerCompetenciaDocumento> listCerCompetenciaDocumento = new List<cerCompetenciaDocumento>();
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosEntities.Configuration.LazyLoadingEnabled = false;

                listCerCompetenciaDocumento = (from competenciaDocumento in oCertificadosEntities.cerCompetenciaDocumento
                                               where lstIdDocumento.Contains(competenciaDocumento.docId)
                                               select competenciaDocumento).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listCerCompetenciaDocumento;
        }

        public List<cerCompetenciaDocumento> GetCompetenciaByIdDocumento(string sIdDocumento)
        {
            List<cerCompetenciaDocumento> listCerCompetenciaDocumento = new List<cerCompetenciaDocumento>();
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosEntities.Configuration.LazyLoadingEnabled = false;

                listCerCompetenciaDocumento = (from competenciaDocumento in oCertificadosEntities.cerCompetenciaDocumento
                                               where sIdDocumento == (competenciaDocumento.docId)
                                               select competenciaDocumento).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listCerCompetenciaDocumento;
        }
    }
}
