using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BusinessModel.Business;
using BusinessModel.Models;

namespace ServiciosCertificadosElectronicosMS
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "ServicioCertificadosElectronicosMS" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione ServicioCertificadosElectronicosMS.svc o ServicioCertificadosElectronicosMS.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ServicioCertificadosElectronicosMS : IServicioCertificadosElectronicosMS
    {
        public RExiste ExisteCertificadoMS(string tokenDes, string cveIns, string CURP)
        {

            WSCertificacionBL objWSTitulacion = new WSCertificacionBL();
            if (objWSTitulacion.getAutenticacionDescarga(tokenDes, cveIns))
            {
                return new WSCertificacionBL().ExisteCertificado(CURP, tokenDes, cveIns);
            }
            else
            {
                throw new FaultException("Error de autenticación, verificar los datos ingresados.");
            }

        }
    }
    }
