using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Business
{
   public  class FirmanteBL
    {
        public FirmanteML GetFirmanteActivo(string insId)
        {
            FirmanteML firmanteML = new FirmanteML();

            try
            {
            cerCatFirmante cerCatFirmante = new CerCatFirmanteDAL().GetFirmanteActivoByInsId(insId);

            AutoMapper.Mapper.Map(cerCatFirmante, firmanteML);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return firmanteML;

        }

    }
}
