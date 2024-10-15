using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Persistence.BD_FrameWork;
using BusinessModel.Models;
namespace BusinessModel.DataAccess
{
    public class CatTipoIncorporacionDAL
    {
        
        public CatTipoIncorporacionML getCatTipoIncorporacion()
        {

            CatTipoIncorporacionML query = new CatTipoIncorporacionML();
            try
            {

                didaEntities dbCemsysi = new didaEntities();

                query.listaTipoIncorporacion = (from cm in dbCemsysi.catTipoIncorp select cm).ToList();
            }
            catch (Exception ex) { System.Console.WriteLine(ex.Message); throw; }

            return query;
        }

        
        public catTipoIncorp getCatTipoIncorporacion(int id)
        {

            CatTipoIncorporacionML query = new CatTipoIncorporacionML();
            try
            {

                didaEntities dbCemsysi = new didaEntities();

                query.tipoIncorporacion = (from cm in dbCemsysi.catTipoIncorp where cm.idTipoIncorporacion==id select cm).FirstOrDefault();
            }
            catch (Exception ex) { System.Console.WriteLine(ex.Message); throw; }

            return query.tipoIncorporacion;
        }


        
        public CatTipoIncorporacionML getCatTipoIncorporacionbyBusqueda(string nombre, string clave,int estatus, int pageNumber, int showItem)
        {

            CatTipoIncorporacionML query = new CatTipoIncorporacionML();

            try
            {
                nombre = nombre == null ? "" : nombre;

                didaEntities dbCemsysi = new didaEntities();
                dbCemsysi.Configuration.LazyLoadingEnabled = true;

                query.listaTipoIncorporacion = (from cm in dbCemsysi.catTipoIncorp where ((cm.tincNombre.Contains(nombre) || nombre.Equals("")) && (cm.tIncAbreviatura.Contains(clave) || clave.Equals("")) && (cm.idEstatus==estatus || estatus==-1)) select cm).ToList();
                query.totalRegistros = query.listaTipoIncorporacion.Count();
                if (showItem != -1)
                {
                    query.listaTipoIncorporacion = query.listaTipoIncorporacion.Skip((pageNumber - 1) * showItem).Take(showItem).ToList();
                }

            }
            catch (Exception ex) { System.Console.WriteLine(ex.Message); throw; }

            return query;
        }

        

        
        public bool agregar(catTipoIncorp value)
        {

            bool agrego = false;
            try
            {

                didaEntities dbCemsysi = new didaEntities();

                dbCemsysi.catTipoIncorp.Add(value);
                dbCemsysi.SaveChanges();
                agrego = true;
            }
            catch (Exception ex) { System.Console.WriteLine(ex.Message); throw; }

            return agrego;
        }



        
        public bool editar(catTipoIncorp valueEdit)
        {

            bool actualizo = false;
            try
            {

                didaEntities dbCemsysi = new didaEntities();
                dbCemsysi.Entry(valueEdit).State= System.Data.Entity.EntityState.Modified;                
                dbCemsysi.SaveChanges();
                actualizo = true;

            }
            catch (Exception ex)
            {
                actualizo = false;
                throw ex;
            }

            return actualizo;
        }


        public bool validaDuplicados(catTipoIncorp value)
        {

            bool duplicado = false;
            try
            {

                didaEntities dbCemsysi = new didaEntities();

               if(dbCemsysi.catTipoIncorp.Where(m => m.tincNombre.Trim() == value.tincNombre.Trim() && m.idTipoIncorporacion!=value.idTipoIncorporacion).Count()>0)
                {
                    duplicado = true;
                }
                
            }
            catch (Exception ex)
            {
                duplicado = false;
                throw ex;
            }

            return duplicado;
        }


        public bool activarDesactivar(catTipoIncorp value)
        {

            bool actualizo = false;
            try
            {
                didaEntities dbCemsysi = new didaEntities();

                catTipoIncorp valueAux = dbCemsysi.catTipoIncorp.Single(m => m.idTipoIncorporacion == value.idTipoIncorporacion);
                valueAux.idEstatus = value.idEstatus;
                dbCemsysi.SaveChanges();
                actualizo = true;

            }
            catch (Exception ex)
            {
                actualizo = false;
                throw ex;
            }

            return actualizo;
        }


        public List<CatTipoIncorporacionML> getCatTipoIncorporacionLista()
        {
            didaEntities dbCemsysi = new didaEntities();
            return dbCemsysi.catTipoIncorp.Where(w => w.idEstatus == 1).Select(s => new CatTipoIncorporacionML { idTipoIncorporacion = s.idTipoIncorporacion, tincNombre = s.tincNombre }).ToList();
        }
    }
}
