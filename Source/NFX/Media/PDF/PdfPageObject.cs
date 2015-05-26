using System.Collections;

namespace NFX.Media.PDF
{
    public class PdfPageObject : IPdfObject
    {
        #region .ctor
        public PdfPageObject()
        {
            m_text = "";
            m_streams = new ArrayList();
        }

        public PdfPageObject(string output)
        {
            m_text = output;
            m_streams = new ArrayList();
        }
        #endregion

        #region Fields

        private string m_fontIndex;
        private string m_fontName;
        private string m_parentIndex;
        private string m_procsetIndex;
        private string m_text;
        private readonly ArrayList m_streams;
        
        #endregion

        #region Properties

        public string Index { get; private set; }

        #endregion

        #region Public

        public string GetText()
        {
            var str = "";
            str += "" + Index + " 0 obj\r\n";
            str += "<< /Type /Page\r\n";
            str += "/Parent " + m_parentIndex + " 0 R\r\n";
            str += "/MediaBox [0 0 612 792]\r\n";
            str += "/Contents [";
            for (var i = 0; i < m_streams.Count; i++)
            {
                str += (string) m_streams[i] + " 0 R\r\n";
            }
            str += "]\r\n";
            str += "/Resources << /Procset " + m_procsetIndex + " 0 R\r\n";
            str += "/Font << /" + m_fontName + " " + m_fontIndex + " 0 R >>\r\n";
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
            m_parentIndex = pi;
        }

        public void SetFontName(string fn)
        {
            m_fontName = fn;
        }

        public void SetFontIndex(string fi)
        {
            m_fontIndex = fi;
        }

        public void SetProcsetIndex(string spi)
        {
            m_procsetIndex = spi;
        }

        public void AddStream(string x)
        {
            m_streams.Add(x);
        }

        #endregion
    }
}