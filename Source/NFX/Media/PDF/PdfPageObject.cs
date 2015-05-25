using System.Collections;

namespace NFX.Media.PDF
{
    internal class PdfPageObject : IPdfObject
    {
        private string fontIndex;
        private string fontName;
        public string Index { get; private set; }
        private string parentIndex;
        private string procsetIndex;
        private string text;
        private readonly ArrayList streams;

        public PdfPageObject()
        {
            text = "";
            streams = new ArrayList();
        }

        public PdfPageObject(string output)
        {
            text = output;
            streams = new ArrayList();
        }

        public string GetText()
        {
            var str = "";
            str += "" + Index + " 0 obj\r\n";
            str += "<< /Type /Page\r\n";
            str += "/Parent " + parentIndex + " 0 R\r\n";
            str += "/MediaBox [0 0 612 792]\r\n";
            str += "/Contents [";
            for (var i = 0; i < streams.Count; i++)
            {
                str += (string) streams[i] + " 0 R\r\n";
            }
            str += "]\r\n";
            str += "/Resources << /Procset " + procsetIndex + " 0 R\r\n";
            str += "/Font << /" + fontName + " " + fontIndex + " 0 R >>\r\n";
            str += ">>\r\n";
            str += ">>\r\n";
            str += "endobj\r\n";
            return str;
        }

        public void SetIndex(string i)
        {
            Index = i;
        }

        public void SetParentIndex(string pi)
        {
            parentIndex = pi;
        }

        public void SetFontName(string fn)
        {
            fontName = fn;
        }

        public void SetFontIndex(string fi)
        {
            fontIndex = fi;
        }

        public void SetProcsetIndex(string spi)
        {
            procsetIndex = spi;
        }

        public void AddStream(string x)
        {
            streams.Add(x);
        }
    }
}