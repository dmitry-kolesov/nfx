namespace NFX.Media.PDF
{
    internal class PdfCatalogObject : IPdfObject
    {
        private readonly string text;

        public PdfCatalogObject()
        {
            var text = "";
        }

        public PdfCatalogObject(string output)
        {
            text = output;
        }

        public string GetText()
        {
            return text;
        }
    }
}