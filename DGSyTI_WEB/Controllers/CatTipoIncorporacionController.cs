using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using BusinessModel.Business;
using DGSyTI_WEB.Models;
using DGSyTI_WEB.Helpers;

namespace DGSyTI_WEB.Controllers
{
    public class CatTipoIncorporacionController : Controller
    {
        CatTipoIncorporacionBL tipoIncorporacionBl = new CatTipoIncorporacionBL();
        CatTipoIncorporacionViewModel model = new CatTipoIncorporacionViewModel();

        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult Index()
        {

            model = paginar("", 1, 10);

            Session["urlAnterior"] = "";
            Session["urlActual"] = "";

            return View(model);

        }

        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult grid(String filtro, int pagina, int bloque)
        {

            return PartialView("grid", paginar(filtro, pagina, bloque));

        }





        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult critBusqueda()
        {
            return PartialView();
        }

        [ValidaAcceso(accion = "NoValidar")]
        private CatTipoIncorporacionViewModel filtros(string json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            CatTipoIncorporacionViewModel result = js.Deserialize<CatTipoIncorporacionViewModel>((json));

            return result;
        }

        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult agregar()
        {

            return PartialView(model);
        }

        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult agregarGuardar(CatTipoIncorporacionViewModel cadena)
        {

            if (ModelState.IsValid)
            {
                cadena.tincNombre = cadena.tincNombre != null ? cadena.tincNombre.Trim() : "";
                cadena.tIncAbreviatura = cadena.tIncAbreviatura != null ? cadena.tIncAbreviatura.Trim() : "";
                //Valida registro los duplicados al guardar nuevo registro
                if (!tipoIncorporacionBl.validaDuplicados(cadena))
                {
                    if (tipoIncorporacionBl.agregar(cadena))
                    {
                        return Content("<script>mensajeExito('" + Helpers.Dialog.mensajeSeAgrego + "');</script>");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Ocurrio un errror, vuelva a intentarlo.");
                        return PartialView("agregar", cadena);
                    }



                }
                else
                {
                    ModelState.AddModelError("Duplicado", Helpers.Dialog.mensajeCamposDuplicados("Nombre"));
                    return PartialView("agregar", cadena);
                }

            }
            else
            {
                return PartialView("agregar", cadena);
            }

        }

        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult editar(int id)
        {
            model = AutoMapper.Mapper.Map(tipoIncorporacionBl.getInstitucionesEducativas(id), model);
            return PartialView(model);
        }

        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult activoInactivo(int id)
        {
            model = AutoMapper.Mapper.Map(tipoIncorporacionBl.getInstitucionesEducativas(id), model);
            return PartialView(model);
        }

        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult editarGuardar(CatTipoIncorporacionViewModel cadena)
        {
            if (ModelState.IsValid)
            {
                cadena.tincNombre = cadena.tincNombre != null ? cadena.tincNombre.Trim() : "";
                cadena.tIncAbreviatura = cadena.tIncAbreviatura != null ? cadena.tIncAbreviatura.Trim() : "";
                //Valida registro los duplicados al guardar nuevo registro
                if (!tipoIncorporacionBl.validaDuplicados(cadena))
                {
                    if (tipoIncorporacionBl.editar(cadena))
                    {
                        return Content("<script>mensajeExito('" + Helpers.Dialog.mensajeSeEdito + "');</script>");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Ocurrio un errror, vuelva a intentarlo.");
                        return PartialView("editar", cadena);
                    }
                }
                else
                {
                    ModelState.AddModelError("Duplicado", Helpers.Dialog.mensajeCamposDuplicados("Nombre"));
                    return PartialView("editar", cadena);
                }

            }
            else
            {
                return PartialView("editar", cadena);
            }

        }

        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult activoInactivoGuardar(CatTipoIncorporacionViewModel cadena)
        {
            if (tipoIncorporacionBl.activarDesactivar(cadena))
            {
                return Content("<script>mensajeExito('" + Helpers.Dialog.mensajeCambioEstatus + "');</script>");
            }
            else
            {
                ModelState.AddModelError("Error", "Ocurrio un errror, vuelva a intentarlo.");
                return PartialView("editar", cadena);
            }

        }

        [ValidaAcceso(accion = "NoValidar")]
        public ActionResult htmlExcel(string filtro)
        {

            var datos = filtros(filtro);

            if (datos == null)
            {
                datos = new CatTipoIncorporacionViewModel();
                datos.tincNombre = "";
                datos.tIncAbreviatura = "";
            }
            try
            {
                AutoMapper.Mapper.Map(tipoIncorporacionBl.getCatTipoIncorporacionbyBusqueda(datos.tincNombre, datos.tIncAbreviatura, (int)datos.idEstatus, 1, -1), model);

                ExcelColumn<CatTipoIncorporacionViewModel> xl = new ExcelColumn<CatTipoIncorporacionViewModel>();

                ExcelColumnDefinition[] columns = new ExcelColumnDefinition[] {

                       xl.Column(x => x.tincNombre, "Nombre"),
                       xl.Column(x => x.tIncAbreviatura, "Clave"),
                       xl.Column(x => x.catEstatus.estNombre, "Estatus"),
                   };
                //return new ExportToExcel<CatTipoIncorporacionViewModel>(model.listaInstituciones.ToList()) { FileDownloadName = "contactos.xls", ColumnDefinitions = columns };

                return null;

            }
            catch (HttpException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;// new HttpException((int)System.Net.HttpStatusCode.InternalServerError, Auxiliares.UtilidadesGenerales.MENSAJE_PROBLEMA);
            }
            return View(model);

        }

        [ValidaAcceso(accion = "NoValidar")]
        public CatTipoIncorporacionViewModel paginar(String filtro, int pagina, int bloque)
        {
            var datos = filtros(filtro);

            Usuario user = (Usuario)Session["appUser"];


            if (datos == null)
            {
                datos = new CatTipoIncorporacionViewModel();
                datos.tincNombre = "";
                datos.tIncAbreviatura = "";
                datos.idEstatus = -1;
            }
            var lista = tipoIncorporacionBl.getCatTipoIncorporacionbyBusqueda(datos.tincNombre, datos.tIncAbreviatura, (int)datos.idEstatus, pagina, bloque);
            AutoMapper.Mapper.Map(lista, model);

            model.permiteAgregar = user.ListaAcciones.Where(cm => cm.AccNombre == "agregarTipoIncorporacion").Count() > 0;
            model.permiteEditar = user.ListaAcciones.Where(cm => cm.AccNombre == "editarTipoIncorporacion").Count() > 0;
            //model.permiteExportar = user.ListaAcciones.Where(cm => cm.AccNombre == "exportarTipoIncorporacion").Count() > 0;
            model.permiteActivar = user.ListaAcciones.Where(cm => cm.AccNombre == "activarTipoIncorporacion").Count() > 0;

            return model;
        }
    }
}