using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Persistence.BD_FrameWork;
namespace BusinessModel.Business
{
    public class CatTipoIncorporacionBL
    {
        CatTipoIncorporacionDAL tipoIncorporacionDal = new CatTipoIncorporacionDAL();
        public CatTipoIncorporacionML getCatTipoIncorporacion()
        {            
            try
            {
                return tipoIncorporacionDal.getCatTipoIncorporacion();
            }
            catch (Exception ex) { System.Console.WriteLine(ex.Message); throw; }          
        }


        public CatTipoIncorporacionML getInstitucionesEducativas(int id)
        {
                        
            try
            {
                
                return AutoMapper.Mapper.Map<CatTipoIncorporacionML>(tipoIncorporacionDal.getCatTipoIncorporacion(id)); ;
            }
            catch (Exception ex) { System.Console.WriteLine(ex.Message); throw; }

           
        }



        public CatTipoIncorporacionML getCatTipoIncorporacionbyBusqueda(string nombre, string clave, int estatus, int pageNumber, int showItem)
        {

            

            try
            {

              return  tipoIncorporacionDal.getCatTipoIncorporacionbyBusqueda(nombre, clave, estatus, pageNumber, showItem);
            }
            catch (Exception ex) { System.Console.WriteLine(ex.Message); throw; }

            
        }




        public bool agregar(CatTipoIncorporacionML value)
        {
           
            return tipoIncorporacionDal.agregar(AutoMapper.Mapper.Map<catTipoIncorp>(value));            
        }


        public bool editar(CatTipoIncorporacionML valueEdit)
        {

            return tipoIncorporacionDal.editar(AutoMapper.Mapper.Map<catTipoIncorp>(valueEdit));
        }

        public bool validaDuplicados(CatTipoIncorporacionML valueEdit)
        {

            return tipoIncorporacionDal.validaDuplicados(AutoMapper.Mapper.Map<catTipoIncorp>(valueEdit));
        }


        public bool activarDesactivar(CatTipoIncorporacionML value)
        {
            value.idEstatus = value.idEstatus == 1 ? 2 : 1;
            return tipoIncorporacionDal.activarDesactivar(AutoMapper.Mapper.Map<catTipoIncorp>(value));
        }

        public List<CatTipoIncorporacionML> getCatTipoIncorporacionLista()
        {

            return tipoIncorporacionDal.getCatTipoIncorporacionLista();
        }
    }
}
