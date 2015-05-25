namespace NFX.Media.PDF
{
    internal class PdfLineStream : IPdfObject
    {
        private string fromX;
        private string fromY;
        private string index;
        private string text;
        private string toX;
        private string toY;

        public PdfLineStream()
        {
            index = "";
            fromX = "";
            fromY = "";
            toY = "";
            toX = "";
            text = "";
        }

        public string GetText()
        {
            if (text == "")
            {
                text += "" + index + " 0 obj\r\n";
                text += "<< /Length 73 >>\r\n";
                text += "stream\r\n";
                text += "" + fromX + " " + fromY + " m\r\n";
                text += "" + toX + " " + toY + " l\r\n";
                text += "S\r\nendstream\r\nendobj\r\n";
            }
            return text;
        }

        public void SetIndex(string i)
        {
            index = i;
        }

        public void SetCoords(int x, int y, int x2, int y2)
        {
            fromX = "" + x;
            fromY = "" + y;
            toX = "" + x2;
            toY = "" + y2;
        }
    }
}