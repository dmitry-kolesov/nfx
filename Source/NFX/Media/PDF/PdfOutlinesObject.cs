namespace NFX.Media.PDF
{
    public class PdfOutlinesObject : IPdfObject
    {
        #region .ctor

        public PdfOutlinesObject()
        {
            m_text = "";
        }

        public PdfOutlinesObject(string output)
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