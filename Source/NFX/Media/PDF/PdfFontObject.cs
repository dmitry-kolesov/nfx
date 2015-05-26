namespace NFX.Media.PDF
{
    public class PdfFontObject : IPdfObject
    {
        #region .ctor

        public PdfFontObject()
        {
            m_text = "";
        }

        public PdfFontObject(string output)
        {
            m_text = output;
        }

        #endregion

        #region Fields

        private readonly string m_text;

        #endregion

        #region Public

        public string GetText()
        {
            return m_text;
        }

        #endregion
    }
}