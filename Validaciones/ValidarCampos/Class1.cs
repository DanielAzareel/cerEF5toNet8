using System;
using System.Collections.Generic;
using System.Text;

namespace ValidarCampos
{
    public class Class1
    {
        public static string validarPromedioAprovechamiento(string PromedioAprovechamiento, string campo)
        {
            int numEntero;
            decimal numDecimal;
            string resul = "";
            double valor = 2.4;
            decimal calificacion;
            double cali;

            bool esNumeroCalificacion = decimal.TryParse("2.42134", out calificacion);
            bool esNumeroCalificacion2 = Double.TryParse("2.42134", out cali);
            if (esNumeroCalificacion)
            {
                numEntero = int.Parse(PromedioAprovechamiento.Split('.')[0]);
                numDecimal = decimal.Parse(PromedioAprovechamiento.Split('.')[1]);
                if (((calificacion < 6) || (calificacion > 10)) || (numDecimal > 9))
                {
                    resul = "<li>Error: la calificación ingresada es incorrecta en el campo: " + campo + "</li>";
                }

            }
            else
            {
                resul = "<li>Error: la calificación ingresada es incorrecta en el campo:" + campo + " es incorrecta</li>";
            }

            return resul;
        }
    }
}
