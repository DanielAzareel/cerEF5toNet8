using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Business
{
    public class ConversionesBL
    {
        //Nuevas variables
        static readonly string[] unidades = { "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
        static readonly string[] especiales = { "", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };
        static readonly string[] decenas = { "", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        static readonly string[] deci = { "", "", "veinti", "treinta y", "cuarenta y", "cincuenta y", "sesenta y", "setenta y", "ochenta y", "noventa y" };
        static readonly string[] centenas = { "", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };
        static readonly string[] milesimas = { "", "mil", "dos mil", "tres mil", "cuatro mil", "cinco mil", "seis mil", "siete mil", "ocho mil", "nueve mil" };

        //Actualizado Sep2023 Alfredo Peña
        public string ToDateStringCustom(int dia, int mes, int anio) //No se usa: Consideraba < 2000 "de" y > 1999 "del"
        {
            String strReturn = "";
            if (dia > 1)
            {
                string dialetra = ConvertirNumeroFonetico(dia);
                if (dia == 21 || dia == 31)
                    dialetra = dialetra.Remove(dialetra.Length - 1);
                strReturn += "a los " + dialetra + " días ";
            }
            else
            {
                strReturn += "al primer día ";
            }
            strReturn += "del mes de " + ConvertirMesFonetico(mes) + " de " + ConvertirNumeroFonetico(anio);
            return strReturn.ToLower();
        }

        //Actualizado Sep2023 Alfredo Peña
        public string ConvertirFecha(int dia, int mes, int anio)
        {
            String strReturn = "";
            if (dia > 1)
            {
                string dialetra = ConvertirNumeroFonetico(dia);
                if (dia == 21 || dia == 31)
                    dialetra = dialetra.Remove(dialetra.Length - 1);
                strReturn += "a los " + dialetra + " días ";
            }
            else
            {
                strReturn += "al primer día ";
            }

            strReturn += "del mes de " + ConvertirMesFonetico(mes) + " de " + ConvertirNumeroFonetico(anio);
            return strReturn.ToLower();
        }

        //Actualizado Sep2023 Alfredo Peña
        public string ConvertirCalificacion(string calif)
        {
            string res = " ";
            string[] calificacion = calif.Split('.');
            int ent = int.Parse(calificacion[0]);
            int dec = 0;
            try
            {
                dec = int.Parse(calificacion[1]);
            }
            catch
            {
            }
            res += char.ToUpper(ConvertirNumeroFonetico(ent)[0]) + ConvertirNumeroFonetico(ent).Substring(1);
            if (ent != 10)
            {
                res += " punto " + ConvertirNumeroFonetico(dec);
            }
            return res;
        }

        //Actualizado Sep2023 Alfredo Peña
        public static string ConvertirMesFonetico(int mes)
        {
            string strReturn = "";
            switch (mes)
            {
                case 1: strReturn = "enero"; break;
                case 2: strReturn = "febrero"; break;
                case 3: strReturn = "marzo"; break;
                case 4: strReturn = "abril"; break;
                case 5: strReturn = "mayo"; break;
                case 6: strReturn = "junio"; break;
                case 7: strReturn = "julio"; break;
                case 8: strReturn = "agosto"; break;
                case 9: strReturn = "septiembre"; break;
                case 10: strReturn = "octubre"; break;
                case 11: strReturn = "noviembre"; break;
                case 12: strReturn = "diciembre"; break;
                default: break;
            }
            return strReturn;
        }

        public static string UpperFirstChar(string InputString)
        {
            if (string.IsNullOrEmpty(InputString))
            {
                return null;
            }

            return char.ToUpper(InputString[0]) + InputString.Substring(1);
        }

        //Comienza correccion y optimizacion Alfredo Peña  Septiembre 2023
        static public string ConvertirNumeroFonetico(int numero)
        {

            //Ejecuta UNIDADES
            if (numero >= 0 && numero <= 9)
            {
                return unidades[numero];
            }

            //Ejecuta 10 y mas
            else if (numero == 10)
            {
                return "diez";
            }
            else if (numero >= 11 && numero <= 19)
            {
                return especiales[numero - 10];
            }
            else if (numero >= 20 && numero <= 99)
            {
                int decena = numero / 10;
                int unidad = numero % 10;
                if (unidad == 0)
                {
                    return decenas[decena];
                }
                else if (unidad != 0 && decena == 2)
                {
                    //coloca acento en 22, 23 y 26
                    if (unidad == 2)
                    {
                        return deci[decena] + "dós";
                    }
                    else if (unidad == 3)
                    {
                        return deci[decena] + "trés";
                    }
                    else if (unidad == 6)
                    {
                        return deci[decena] + "séis";
                    }
                    else
                    {
                        return deci[decena] + unidades[unidad];
                    }
                }
                else
                {
                    return deci[decena] + " " + unidades[unidad];
                }
            }

            //Ejecuta 100 y mas
            else if (numero >= 100 && numero <= 999)
            {
                int centena = numero / 100;
                int resto = numero % 100;
                if (resto == 0)
                {
                    if (centena == 1)
                    {
                        return "cien";
                    }
                    else
                    {
                        return centenas[centena];
                    }
                }
                else
                {
                    return centenas[centena] + " " + ConvertirNumeroFonetico(resto);
                }
            }

            //Ejecuta 1000 y mas
            else if (numero >= 1000 && numero <= 9999)
            {
                int milesima = numero / 1000;
                int resto = numero % 1000;
                if (resto == 0)
                {
                    return milesimas[milesima];
                }
                else
                {
                    return milesimas[milesima] + " " + ConvertirNumeroFonetico(resto);
                }
            }

            else
            {
                return "Número fuera de rango";
            }
        }



    }

}

