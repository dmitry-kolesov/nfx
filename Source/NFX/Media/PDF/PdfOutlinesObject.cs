namespace NFX.Media.PDF
{
    internal class PdfOutlinesObject : IPdfObject
    {
        private readonly string text;

        public PdfOutlinesObject()
        {
            var text = "";
        }

        public PdfOutlinesObject(string output)
        {
            text = output;
        }

        public string GetText()
        {
            return text;
        }
    }
}