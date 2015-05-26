namespace NFX.Media.PDF
{
    public class PdfStreamObject : IPdfObject
    {
        #region .ctor
        public PdfStreamObject()
        {
            m_text = "";
        }

        public PdfStreamObject(string output)
        {
            m_text = output;
        }
        #endregion

        #region Fields

        private string m_index;
        private string m_size;
        private string m_str;
        private string m_text;
        private string m_x;
        private string m_y;

        #endregion

        #region Public
        public string GetText()
        {
            if (m_text == "")
            {
                m_text += "" + m_index + " 0 obj\r\n";
                m_text += "<< /Length 73 >>\r\n";
                m_text += "stream\r\n";
                m_text += "BT\r\n";
                m_text += "/F1 " + m_size + " Tf\r\n";
                m_text += m_x + " " + m_y + " " + "Td\r\n";
                m_text += "(" + m_str + ") Tj\r\n";
                m_text += "ET\r\nendstream\r\nendobj\r\n";
            }
            return m_text;
        }

        public void SetIndex(string i)
        {
            m_index = i;
        }

        public void Set(string xCoord, string yCoord, string s, string sz)
        {
            m_x = xCoord;
            m_y = yCoord;
            m_str = s;
            m_size = sz;
        }

        #endregion
    }
}