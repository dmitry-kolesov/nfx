namespace NFX.Media.PDF
{
    internal class PdfStreamObject : IPdfObject
    {
        private string index;
        private string size;
        private string str;
        private string text;
        private string x;
        private string y;

        public PdfStreamObject()
        {
            text = "";
        }

        public PdfStreamObject(string output)
        {
            text = output;
        }

        public string GetText()
        {
            if (text == "")
            {
                text += "" + index + " 0 obj\r\n";
                text += "<< /Length 73 >>\r\n";
                text += "stream\r\n";
                text += "BT\r\n";
                text += "/F1 " + size + " Tf\r\n";
                text += x + " " + y + " " + "Td\r\n";
                text += "(" + str + ") Tj\r\n";
                text += "ET\r\nendstream\r\nendobj\r\n";
            }
            return text;
        }

        public void SetIndex(string i)
        {
            index = i;
        }

        public void Set(string xCoord, string yCoord, string s, string sz)
        {
            x = xCoord;
            y = yCoord;
            str = s;
            size = sz;
        }
    }
}