using BusinessModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiciosCertificadosElectronicosMS
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IServicioCertificadosElectronicosMS" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IServicioCertificadosElectronicosMS
    { 
            [OperationContract]
            RExiste ExisteCertificadoMS(string tokenDes, string cveIns, string CURP);
        
    }
}
