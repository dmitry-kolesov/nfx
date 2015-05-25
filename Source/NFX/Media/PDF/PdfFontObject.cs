namespace NFX.Media.PDF
{
    internal class PdfFontObject : IPdfObject
    {
        private readonly string text;

        public PdfFontObject()
        {
            var text = "";
        }

        public PdfFontObject(string output)
        {
            text = output;
        }

        public string GetText()
        {
            return text;
        }
    }
}