using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Validar
{
    public class ValidarDatos
    {
        public static string ValidarCorreo(string correo="", string campo="")
        {
            Regex exp = new Regex("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$");
            string resul = "";
            if (!exp.IsMatch(correo) && correo != "")
            {
                resul = "<li>Advertencia: El " + campo + " no es válido.</li>";
            }

            return resul;
        }
        public static string ValidarCaracteresEspeciales(string dato="", string campo="")
        {
            Regex exp = new Regex("^([^|\\\\~&<\\n']+)$");
            string resul = "";
            if (!exp.IsMatch(dato))
            {
                resul =dato==""?"":"<li>Error: El " + campo + " contiene caracteres especiales:|\\~&'< .</li>";
            }

            return resul;
        }
        public static string ValidaFormatoFecha(string fecha_inicio="", string fecha_fin="", string campo_fecha_inicio="", string campo_fecha_fin="")
        {
           
            Regex exp = new Regex(@"(0[1-9]|[12][0-9]|3[01])[\/|-](0[1-9]|1[012])[\/|-]\d{4}");
            string resul = "";
            if (fecha_inicio != "" || fecha_fin != "")
            {
                if (!exp.IsMatch(fecha_inicio) && fecha_inicio.Trim() != "")
                {
                    resul = fecha_inicio == "" ? "" : "<li>Error: El " + campo_fecha_inicio + " no contiene el formato dd/mm/aaaa o dd-mm-aaaa.</li>";
                }
                if (!exp.IsMatch(fecha_fin) && fecha_fin.Trim() != "")
                {
                    resul += "<li>Error: El " + campo_fecha_fin + " no contiene el formato dd/mm/aaaa o dd-mm-aaaa.</li>";
                }
                if (resul == "")
                {
                    if (fecha_inicio.Trim() != "" && fecha_fin.Trim() != "")
                    {
                        resul = resul + validarFechaInicioCertificacion(fecha_inicio, fecha_fin, campo_fecha_inicio, campo_fecha_fin);
                        resul = resul + validarFechaFinCertificacion(fecha_inicio, fecha_fin, campo_fecha_inicio, campo_fecha_fin);
                    }
                }

            }

            return resul;
        }
        public static string validarFechaInicioCertificacion(string fecha_inicio, string fecha_fin, string campo_fecha_inicio, string campo_fecha_fin)
        {
            DateTime fecha1 = Convert.ToDateTime(fecha_inicio);
            DateTime fecha2 = Convert.ToDateTime(fecha_fin);
            string resul = "";
            int result = DateTime.Compare(fecha1.Date, fecha2.Date);
            
            if (result >= 0)
            {
                resul = "<li>Error: El " + campo_fecha_inicio + " no puede ser mayor o igual al " + campo_fecha_fin + ".</li>";
            }

            return resul;
        }
        public static string validarFechaFinCertificacion(string fecha_inicio, string fecha_fin, string campo_fecha_inicio, string campo_fecha_fin)
        {
            DateTime fecha1 = Convert.ToDateTime(fecha_inicio);
            DateTime fecha2 = Convert.ToDateTime(fecha_fin);
            string resul = "";
            int result2 = DateTime.Compare(fecha2.Date, DateTime.Now);

            if (result2 > 0)
            {
                resul = "<li>Error: El " + campo_fecha_fin + " no puede ser mayor a la fecha actual.</li>";
            }

            int result = DateTime.Compare(fecha2.Date, fecha1.Date);

            if (result <= 0)
            {
                resul=resul+ "<li> Error: El " + campo_fecha_fin + " no puede ser menor o igual al " + campo_fecha_inicio + ".</li>";
            }
            return resul;
        }

        public static string validarPromedioAprovechamiento(string promedioAprovechamiento="", string campo="")
        {
            bool tieneDecimal = false;
            int numEntero;
            decimal numDecimal;
            string resul = "";
            decimal calificacion;
            decimal numDecimal2;
            //if (promedioAprovechamiento == "A" || promedioAprovechamiento == "AC")
            //{
            //    resul = "";
            //}
            //else
            //{
                bool esNumeroCalificacion = decimal.TryParse(promedioAprovechamiento, out calificacion);

                if (esNumeroCalificacion)
                {
                    numEntero = int.Parse(promedioAprovechamiento.Split('.')[0]);
                    try
                    {
                        numDecimal = decimal.Parse(promedioAprovechamiento.Split('.')[1]);
                        tieneDecimal = true;
                    }
                    catch (Exception)
                    {
                        numDecimal = 0;


                    }

                    try
                    {
                        numDecimal2 = decimal.Parse(promedioAprovechamiento.Split('.')[2]);
                        resul = "<li>Error: La calificación ingresada es incorrecta en " + campo + ".</li>";
                    }
                    catch (Exception e)
                    {
                        resul = "";

                    }

                    if (((numEntero < 6) || (numEntero > 10)) || (numDecimal > 9) || (numEntero > 9 && tieneDecimal == true))
                    {
                        resul = "<li>Error: La calificación ingresada es incorrecta en " + campo + ".</li>";
                    }

                }
                else
                {
                    resul = promedioAprovechamiento == "" ? "" : "<li>Error: La calificación ingresada es incorrecta en " + campo + ".</li>";
                }
            //}

            return resul;
        }

        public static string validarPromedioAprovechamientoCompetencia(string promedioAprovechamiento = "", string campo = "", string literalesAprobatorias = "")
        {
            string[] literalesPermitidas = literalesAprobatorias.Split(',');
            bool tieneDecimal = false;
            int numEntero;
            decimal numDecimal;
            string resul = "";
            decimal calificacion;
            decimal numDecimal2;
            if (buscarLiteral(promedioAprovechamiento, literalesPermitidas))
            {
                resul = "";
            }
            else
            {
                bool esNumeroCalificacion = decimal.TryParse(promedioAprovechamiento, out calificacion);

            if (esNumeroCalificacion)
            {
                numEntero = int.Parse(promedioAprovechamiento.Split('.')[0]);
                try
                {
                    numDecimal = decimal.Parse(promedioAprovechamiento.Split('.')[1]);
                    tieneDecimal = true;
                }
                catch (Exception)
                {
                    numDecimal = 0;


                }

                try
                {
                    numDecimal2 = decimal.Parse(promedioAprovechamiento.Split('.')[2]);
                    resul = "<li>Error: La calificación ingresada es incorrecta en " + campo + ".</li>";
                }
                catch (Exception e)
                {
                    resul = "";

                }

                if (((numEntero < 6) || (numEntero > 10)) || (numDecimal > 9) || (numEntero > 9 && tieneDecimal == true))
                {
                    resul = "<li>Error: La calificación ingresada es incorrecta en " + campo + ".</li>";
                }

            }
            else
            {
                resul = promedioAprovechamiento == "" ? "" : "<li>Error: La calificación ingresada es incorrecta en " + campo + ".</li>";
            }
            }

            return resul;
        }
        public static string ValidaPeriodoEscolar(string periodoEscolar = "", string campo = "")
        {
            //^\d{ 4} ([\-/.])(0?[1 - 9] | 1[1 - 2])\1(3[01] |[12][0 - 9] | 0?[1 - 9])$

            Regex exp = new Regex(@"^\d{4}([\/.])(0?[1-9]|1[012])\1([A-B])$");
            string resul = "";
            if (periodoEscolar != "***") {

                if (!exp.IsMatch(periodoEscolar))
                {
                    resul = "<li>Error: El " + campo + "  no contiene el formato aaaa/mm/qna o *** .</li> ";
                }
            }
               

            return resul;
        }
        public static string validarPromedioAprovechamientoUAC(string promedioAprovechamiento = "", string campo = "", string literales="")
        {
            literales = literales + "," + "****";
            string[] literalesPermitidas = literales.Split(',');
            bool tieneDecimal = false;
            int numEntero;
            decimal numDecimal;
            string resul = "";
            decimal calificacion;
            decimal numDecimal2;
            if (buscarLiteral(promedioAprovechamiento, literalesPermitidas))
            {
                resul = "";
            }
            else
            {
                bool esNumeroCalificacion = decimal.TryParse(promedioAprovechamiento, out calificacion);

                if (esNumeroCalificacion)
                {
                    numEntero = int.Parse(promedioAprovechamiento.Split('.')[0]);
                    try
                    {
                        numDecimal = decimal.Parse(promedioAprovechamiento.Split('.')[1]);
                        tieneDecimal = true;
                    }
                    catch (Exception)
                    {
                        numDecimal = 0;


                    }

                    try
                    {
                        numDecimal2 = decimal.Parse(promedioAprovechamiento.Split('.')[2]);
                        resul = "<li>Error: La calificación ingresada es incorrecta en " + campo + ".</li>";
                    }
                    catch (Exception e)
                    {
                        resul = "";

                    }

                    if (((numEntero < 5) || (numEntero > 10)) || (numDecimal > 9) || (numEntero > 9 && tieneDecimal == true))
                    {
                        resul = "<li>Error: La calificación ingresada es incorrecta en " + campo + ".</li>";
                    }

                }
                else
                {
                    resul = promedioAprovechamiento == "" ? "" : "<li>Error: La calificación ingresada es incorrecta en " + campo + ".</li>";
                }
            }

            return resul;
        }

        public static bool buscarLiteral(string valorBuscar, string []array)
        {
            bool encontroRegistro = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == valorBuscar)
                {
                    encontroRegistro = true;
                }
            }
            return encontroRegistro;
        }
        public static string validarCaracteresEspecialesDeParametros(string campo, string @cadenaAvalidar, string caracteresNoPermitidos="")
        {
            string result = "";
            string carateresNoValidos = "";
            char[] arrayCaracteresNoPermitidos = caracteresNoPermitidos.ToCharArray();
            char[] arrayCadenaAvalidar = cadenaAvalidar.ToCharArray();
            foreach (char caracter in arrayCaracteresNoPermitidos)
            {

                if (caracter == '\\')
                {
                    carateresNoValidos += @"\\\";
                }
                else if (caracter == '\'')
                {
                    carateresNoValidos += "\\'";
                }
                else {
                    carateresNoValidos += caracter;
                }
            }
            foreach (char caracter in arrayCaracteresNoPermitidos)
            {
                if (cadenaAvalidar.Contains(caracter.ToString()))
                {
                    result = "<li> Error: El " + campo + " contiene caracteres especiales:"+ carateresNoValidos + ".</li>";
                    break;
                } 
            }

            return result;

        }


    }
}
