using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models.Personalizados
{
    public class TabML
    {
        public string metodo { set; get; }
        public string title { set; get; }
        public string controller { set; get; }
        public string label { set; get; }
        public string parametro { set; get; }
        public bool activa { set; get; }
        public string idTablaFiltrado { set; get; }
        public List<TabML> subTabs { set; get; }

        public void Tab()
        {
            metodo = "";
            title = "";
            controller = "";
            label = "Sin definir título";
            parametro = "N/A";
            activa = true;
            subTabs = null;
        }

    }
}
