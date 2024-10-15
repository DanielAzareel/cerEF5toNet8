using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    [DataContract]
    public class RExiste
    {
        [DataMember] public bool Existe { set; get; }
        [DataMember] public string Token { set; get; }

        public RExiste()
        {
            Existe = false;
        }


    }
}
