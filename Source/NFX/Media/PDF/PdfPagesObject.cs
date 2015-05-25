using System.Collections;

namespace NFX.Media.PDF
{
    internal class PdfPagesObject : IPdfObject
    {
        public string Index { get; private set; }
        private string text;
        private readonly ArrayList pageIndexes;

        public PdfPagesObject()
        {
            text = "";
            pageIndexes = new ArrayList();
        }

        public PdfPagesObject(string output)
        {
            text = output;
        }

        public string GetText()
        {
            if (text == "")
            {
                text += "" + Index + " 0 obj\r\n";
                text += "<<\r\n";
                text += "/Type /Pages\r\n";
                text += "/Kids [";
                for (var i = 0; i < pageIndexes.Count; i++)
                {
                    text += ((string) pageIndexes[i]) + " 0 R ";
                }
                text += "]\r\n";
                text += "/Count " + pageIndexes.Count + "\r\n";
                text += ">>\r\nendobj\r\n";
            }

            return text;
        }

        public void SetIndex(string i)
        {
            Index = i;
        }

        public void AddPage(string index)
        {
            pageIndexes.Add(index);
        }
    }
}