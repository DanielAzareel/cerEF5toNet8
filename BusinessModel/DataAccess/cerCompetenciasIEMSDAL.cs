using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerCompetenciasIEMSDAL
    {
        public List<cerCompetenciasIEMS> GetCompetenciaByIdDocumento(List<string> lstIdDocumento)
        {
            List<cerCompetenciasIEMS> listCerCompetenciaDocumento = new List<cerCompetenciasIEMS>();
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosEntities.Configuration.LazyLoadingEnabled = false;

                listCerCompetenciaDocumento = (from competenciaDocumento in oCertificadosEntities.cerCompetenciasIEMS
                                               where lstIdDocumento.Contains(competenciaDocumento.docId)
                                               select competenciaDocumento).OrderBy(x=> x.CompetenciasIEMSorden).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listCerCompetenciaDocumento;
        }

        public List<cerCompetenciasIEMS> GetCompetenciaIEMSByDocumento(string sIdDocumento)
        {
            List<cerCompetenciasIEMS> listCerCompetenciaDocumento = new List<cerCompetenciasIEMS>();
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosEntities.Configuration.LazyLoadingEnabled = false;

                listCerCompetenciaDocumento = (from competenciaDocumento in oCertificadosEntities.cerCompetenciasIEMS
                                               where competenciaDocumento.docId== sIdDocumento
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
