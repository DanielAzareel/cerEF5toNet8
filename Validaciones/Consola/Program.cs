using System;
using System.Collections.Generic;
using System.Text;

namespace Consola
{
    class Program
    {
        
        static void Main(string[] args)
        {

            string a = "<li> Error: El número de control contiene caracteres especiales:|\\\\~&'<+.</li>";

            //var promedio2 = Validar.ValidarDatos.ValidarCaracteresEspeciales("", "promedio");
            //bool esNumeroCalificacion = decimal.TryParse("2.42134", out calificacion);
            var periodoEscolar = Validar.ValidarDatos.ValidaPeriodoEscolar("2017/10/A","mate");
            var fecha = Validar.ValidarDatos.ValidaFormatoFecha("01/02/2018", " ", "fecha inicio", "fecha fin");
            //var promedio = Validar.ValidarDatos.validarPromedioAprovechamiento("A","promedio");
            //var resultadoValidarFecha = Validar.ValidarDatos.ValidaFormatoFecha("12-08-2010 00:00:00", "29-04-2020 00:00:00", "[Periodo inicio]", "[Periodo término]");
            var promedioUAC = Validar.ValidarDatos.validarPromedioAprovechamientoUAC("9.0", "mate 1", "A,AC");
            var permitidos = Validar.ValidarDatos.validarCaracteresEspecialesDeParametros("carlos zavala fonseca","%", @"|\~&'<+'");
            //var rel = Validar.Class1.ValidarCorreo("ddsasa");
            //var rel2 = Validar.Class1.ValidarCaracteresEspeciales("ddsasa");

            Console.WriteLine();
        }
    }
}
