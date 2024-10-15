using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validaciones
{
    public class Class1
    {
        public bool validarDato(string valor)
        {
            bool retornara = false;
            if(valor != "")
            {
                retornara = true;
            }
            return retornara;
        }
    }
}
