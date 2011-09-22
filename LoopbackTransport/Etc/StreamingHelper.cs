using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LoopbackTransport
{
    public class StreamingHelper
    {
        public static string CreateStringFromStream(Stream content, int codepage)
        {
            //Trace(258, string.Format("CreateStringFromStream start with codepage='{0}'", codepage.ToString()), System.Diagnostics.TraceEventType.Verbose);

            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            StreamReader sr;

            if (codepage == 0)
            {
                //the caller doesn't specify the encoding, try UTF-8 being the BizTalk default
                sr = new StreamReader(content);
            }
            else
            {
                Encoding enc = Encoding.GetEncoding(codepage);
                sr = new StreamReader(content, enc);
            }
            string contentString = sr.ReadToEnd();

            //Trace(258, "CreateStringFromStream exit", System.Diagnostics.TraceEventType.Verbose);

            return contentString;
        }

        public static string CreateStringFromStream(Stream content)
        {
            //two choices can be made: or take 0 and detect from BOM, or make UTF-8 (65001) since this is the biztalk default
            return CreateStringFromStream(content, 65001);
        }

    }
}
