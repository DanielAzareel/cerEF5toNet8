using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpers
{
    public enum PAGINAR { INICIO, ANTERIOR, SIGUIENTE, FIN, BLOQUE };
    public class Paginacion
    {
        public Paginacion() { }
        public Paginacion(int totalRegistros, int bloque)
        {
            this.totalRegistros = totalRegistros;
            this.bloque = bloque;
            calculaPaginas();
            this.paginaActual = 1;
            this.paginaAnterior = 1;
            this.paginaSiguiente = paginas == 1 ? 1 : paginaActual + 1;
            this.inicio = 1;
            this.fin = paginas;
        }
        public int totalRegistros { set; get; }
        public int paginas { set; get; }
        public int paginaActual { set; get; }
        public int paginaSiguiente { set; get; }
        public int paginaAnterior { set; get; }
        public int inicio { set; get; }
        public int fin { set; get; }
        public int bloque { set; get; }

        private void calculaPaginas()
        {
            //if (totalRegistros == 0 || bloque == 0) {
            //    throw new Exception("Total de registro y el bloque no pueden ser 0.");
            //}
            if (bloque == 0) { bloque = 10; }
            if ((totalRegistros <= bloque) || bloque == -1)
            {
                this.paginas = 1;
            }
            else
            {
                this.paginas = (int)Math.Ceiling((double)((double)this.totalRegistros / (double)this.bloque));
            }
        }
        public void goInicio()
        {
            calculaPaginas();
            this.paginaActual = 1;
            this.paginaAnterior = 1;
            this.paginaSiguiente = paginas == 1 ? paginas : paginaActual + 1;
            this.inicio = 1;
            this.fin = paginas;
        }
        public void goFin()
        {
            calculaPaginas();
            this.paginaActual = paginas;
            this.paginaAnterior = paginas == 1 ? paginas : paginaActual - 1;
            this.paginaSiguiente = paginas;
            this.inicio = 1;
            this.fin = paginas;
        }
        public void goSiguiente()
        {
            calculaPaginas();
            if (paginaActual == paginas)
            {
                goFin();
            }
            else
            {
                this.paginaAnterior = paginaActual;
                this.paginaActual = paginaSiguiente;
                this.paginaSiguiente = (paginaActual + 1) <= paginas ? paginaActual + 1 : paginas;
                this.inicio = 1;
                this.fin = paginas;
            }
        }
        public void goAnterior()
        {
            calculaPaginas();
            if (paginaActual == 1)
            {
                goInicio();
            }
            else
            {
                this.paginaSiguiente = paginaActual;
                this.paginaActual = paginaAnterior;
                this.paginaAnterior = (paginaActual - 1) > 0 ? paginaActual - 1 : paginas;
                this.inicio = 1;
                this.fin = paginas;
            }
        }

        public void goBloque(int bloque)
        {
            this.bloque = bloque;
            calculaPaginas();
            this.paginaActual = 1;
            this.paginaAnterior = 1;
            this.paginaSiguiente = paginas == 1 ? 1 : paginaActual + 1;
            this.inicio = 1;
            this.fin = paginas;
        }
    }
}