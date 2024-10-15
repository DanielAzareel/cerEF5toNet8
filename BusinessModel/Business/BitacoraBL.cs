using BusinessModel.DataAccess;
using BusinessModel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Business
{
    public class BitacoraBL
    {
        public void guardaBitacora(string descripcion, string usrLogin, string accion,bool exitoso)
        {
            List<BitacoraML> listBitacora = new List<BitacoraML>();
            listBitacora.Add(new BitacoraML()
            {
                bitId = Guid.NewGuid().ToString(),
                bitFecha = DateTime.Now,
                bitUsuario = usrLogin,
                bitDescripcion = descripcion,
                bitExitoso = exitoso,
                accId = accion
            });

            DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
            StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");
        }
    }
}
