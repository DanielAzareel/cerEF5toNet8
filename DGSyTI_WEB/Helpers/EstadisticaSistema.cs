using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using BusinessModel.Models;
using BusinessModel.Business;
using Helpers;

namespace DGSyTI_WEB.Helpers
{
    public class EstadisticaSistema : Attribute
    {
        public string pantalla { get; set; }

        public EstadisticaSistema(string pantalla)
        {
            this.pantalla = pantalla;
            var user = (Usuario)HttpContext.Current.Session["appUser"];

            if (user != null)
            {
                /*EstadisticasBL estadistica = new EstadisticasBL();
                EstadisticasML registro = new EstadisticasML();
                registro.estadisticaSistema.estPantalla = pantalla;
                registro.estadisticaSistema.estUser = user.Login;
                estadistica.agregar(registro);*/
            }
        }


        /*public void registrarAccion(string pantalla)
        {

            var user = (Usuario)HttpContext.Current.Session["appUser"];

            if (user != null)
            {
                EstadisticasBL estadistica = new EstadisticasBL();
                EstadisticasML registro = new EstadisticasML();
                registro.estadisticaSistema.estPantalla = pantalla;
                registro.estadisticaSistema.estUser = user.Login;
                estadistica.agregar(registro);
            }

        }*/

    }
}