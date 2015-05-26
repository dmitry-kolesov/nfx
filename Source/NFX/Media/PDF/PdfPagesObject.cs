using System.Collections;

namespace NFX.Media.PDF
{
    public class PdfPagesObject : IPdfObject
    {
        #region .ctor
        public PdfPagesObject()
        {
            m_text = "";
            m_pageIndexes = new ArrayList();
        }

        public PdfPagesObject(string output)
        {
            m_text = output;
        }
        #endregion

        #region Fields

        private string m_text;
        private readonly ArrayList m_pageIndexes;

        #endregion

        #region Properties

        public string Index { get; private set; }
        
        #endregion

        #region Public
        public string GetText()
        {
            if (m_text == "")
            {
                m_text += "" + Index + " 0 obj\r\n";
                m_text += "<<\r\n";
                m_text += "/Type /Pages\r\n";
                m_text += "/Kids [";
                for (var i = 0; i < m_pageIndexes.Count; i++)
                {
                    m_text += ((string) m_pageIndexes[i]) + " 0 R ";
                }
                m_text += "]\r\n";
                m_text += "/Count " + m_pageIndexes.Count + "\r\n";
                m_text += ">>\r\nendobj\r\n";
            }

            return m_text;
        }

        public void SetIndex(string i)
        {
            Index = i;
        }

        public void AddPage(string index)
        {
            m_pageIndexes.Add(index);
        }
        #endregion
    }
}