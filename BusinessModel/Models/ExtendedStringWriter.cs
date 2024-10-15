using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
     
        public sealed class ExtendedStringWriter : StringWriter
        {
            private readonly Encoding stringWriterEncoding;
            public ExtendedStringWriter(Encoding desiredEncoding)

            {
                this.stringWriterEncoding = desiredEncoding;
            }

            public override Encoding Encoding
            {
                get
                {
                    return this.stringWriterEncoding;
                }
            }
        }
}
