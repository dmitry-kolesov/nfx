using System;
using System.Collections;
using System.IO;
using System.Text;

namespace NFX.Media.PDF
{
    public class PdfDocument
    {
        #region consts

        private const int SizeH = 790;

        #endregion

        #region .ctor

        public PdfDocument()
        {
            m_curIndex = 8;
            Objs = new ArrayList();
            CreateDocument();
        }

        #endregion

        #region Fields

        private int m_curIndex;
        private PdfPageObject m_currentPage;
        private PdfPagesObject m_pages;

        #endregion

        #region Properties

        public ArrayList Objs { get; private set; }

        #endregion

        #region Public
        public void CreateDocument()
        {
            Objs.Add(new PdfCatalogObject(@"1 0 obj
<< /Type /Catalog
/Outlines 2 0 R
/Pages 3 0 R
>>
endobj
"));

            Objs.Add(new PdfOutlinesObject(@"2 0 obj
<< /Type /Outlines
/Count 0
>>
endobj
"));

            m_pages = new PdfPagesObject();
            m_pages.SetIndex("3");
            m_pages.AddPage("4");

            m_currentPage = new PdfPageObject();
            m_currentPage.SetIndex("4");
            m_currentPage.SetFontName("F1");
            m_currentPage.SetFontIndex("7");
            m_currentPage.SetParentIndex("3");
            m_currentPage.SetProcsetIndex("6");

            Objs.Add(new PdfFontObject(@"6 0 obj
[/PDF /Text]
endobj
"));

            Objs.Add(new PdfFontObject(@"7 0 obj
<< /Type /Font
/Subtype /Type1
/Name /F1
/BaseFont /Helvetica
/Encoding /MacRomanEncoding
>>
endobj
"));
        }

        public void Draw(int x, int y, string text, int size)
        {
            y = SizeH - y;
            var stream = new PdfStreamObject();
            stream.SetIndex("" + m_curIndex);
            stream.Set("" + x, "" + y, text, "" + size);
            Objs.Add(stream);
            m_currentPage.AddStream("" + m_curIndex);
            m_curIndex++;
        }

        public void DrawLine(int fromX, int fromY, int toX, int toY)
        {
            fromY = SizeH - fromY;
            toY = SizeH - toY;
            var stream = new PdfLineStream();
            stream.SetIndex("" + m_curIndex);
            stream.SetCoords(fromX, fromY, toX, toY);
            Objs.Add(stream);
            m_currentPage.AddStream("" + m_curIndex);
            m_curIndex++;
        }

        public int Save(string filename)
        {
            try
            {
                var sw = new StreamWriter(filename);
                sw.Write(Output());
                sw.Close();
            }
            catch (Exception e)
            {
                return -1;
            }

            return 1;
        }

        public string Output()
        {
            Objs.Add(m_pages);
            Objs.Add(m_currentPage);


            StringBuilder pdf = new StringBuilder("%PDF-1.3\r\n");
            // display all of the Objs
            for (var i = 0; i < Objs.Count; i++)
            {
                pdf.Append(((IPdfObject)Objs[i]).GetText());
            }
            // Draw xref table
            pdf.Append("xref\r\n");
            pdf.Append("0 " + (Objs.Count + 1) + "\r\n");
            pdf.Append("0000000000 65535 f\r\n");
            pdf.Append("0000000009 00000 n\r\n");
            var bytecount = 9 + ((IPdfObject)Objs[0]).GetText().Length;
            for (var i = 1; i < Objs.Count; i++)
            {
                var strCount = "" + bytecount;
                while (strCount.Length < 10)
                    strCount = "0" + strCount;
                pdf.Append(strCount + " 00000 n\r\n");
                bytecount += ((IPdfObject)Objs[i]).GetText().Length;
            }
            pdf.Append("trailer\r\n");
            pdf.Append("<< /size " + (Objs.Count + 1) + "\r\n");
            pdf.Append("/Root 1 0 R\r\n");
            pdf.Append(">>\r\n");
            pdf.Append("startxref\r\n");
            pdf.Append("625\r\n");
            pdf.Append("%%EOF");
            return pdf.ToString();
        }
        #endregion
    }
}