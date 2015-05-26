namespace NFX.Media.PDF
{
    public class PdfCatalogObject : IPdfObject
    {
        #region .ctor

        public PdfCatalogObject()
        {
            m_text = "";
        }

        public PdfCatalogObject(string output)
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