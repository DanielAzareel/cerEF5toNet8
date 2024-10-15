using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGSyTI_WEB.Helpers
{
    public static class Dialog
    {
        public static string ContainerId { get; set; }
        public static string Title { get; set; }
        public static string Message { get; set; }
        public static string Action { get; set; }
        public static object ActionParams { get; set; }

        public static string mensajeCambioEstatus = "Se actualizó el estatus correctamente.";
        public static string mensajeSeAgrego = "Se guardó correctamente.";
        public static string mensajeSeEdito = "Se actualizó correctamente.";
        public static string mensajeSeEliminar = "Se eliminó correctamente.";

        public static string mensajeCamposDuplicados(string campos)
        {
            string mensaje = "";
            var listaCampos = campos.Split(',');

            int totalCampos = listaCampos.Count();
            int contador = 1;
            foreach (var campo in listaCampos)
            {
                if (totalCampos == 1)
                {
                    mensaje = mensaje +"'" + campo.Trim() + "'";
                }
                else {
                    if (contador == totalCampos)
                    {
                        mensaje = mensaje + " y " +"'"+ campo.Trim()+"'";
                    }
                    else
                    {
                        mensaje = mensaje+ "'" + campo.Trim() + "'";
                    }
                }
                contador = contador + 1;
                if(contador < totalCampos)
                {
                    mensaje = mensaje + ", ";
                }
            }
            mensaje = "Ya existe un registro con los mismos valores para: " + mensaje+".";
            return mensaje;
        }

        public static string mensajeCampoRequerido(string campo)
        {
            string mensaje = "El campo '{0}' es requerido.";
            mensaje = mensaje.Replace("{0}", campo);
            return mensaje;
        }

        public static string mensajeCampoMayorMenor(string menor, string mayor)
        {
            string mensaje = "El campo '{0}' debe ser mayor al campo '{1}'.";
            mensaje = mensaje.Replace("{0}", mayor);
            mensaje = mensaje.Replace("{1}", menor);
            return mensaje;
        }
    }
}