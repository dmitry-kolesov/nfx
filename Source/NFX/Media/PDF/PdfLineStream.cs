namespace NFX.Media.PDF
{
    public class PdfLineStream : IPdfObject
    {
        #region .ctor

        public PdfLineStream()
        {
            m_index = "";
            m_fromX = "";
            m_fromY = "";
            m_toY = "";
            m_toX = "";
            m_text = "";
        }

        #endregion

        #region Fields

        private string m_fromX;
        private string m_fromY;
        private string m_index;
        private string m_text;
        private string m_toX;
        private string m_toY;

        #endregion

        #region Public

        public string GetText()
        {
            if (m_text == "")
            {
                m_text += "" + m_index + " 0 obj\r\n";
                m_text += "<< /Length 73 >>\r\n";
                m_text += "stream\r\n";
                m_text += "" + m_fromX + " " + m_fromY + " m\r\n";
                m_text += "" + m_toX + " " + m_toY + " l\r\n";
                m_text += "S\r\nendstream\r\nendobj\r\n";
            }
            return m_text;
        }

        public void SetIndex(string i)
        {
            m_index = i;
        }

        public void SetCoords(int x, int y, int x2, int y2)
        {
            m_fromX = "" + x;
            m_fromY = "" + y;
            m_toX = "" + x2;
            m_toY = "" + y2;
        }

        #endregion
    }
}