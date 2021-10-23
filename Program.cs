using System;
using System.Drawing;
using foxit;
using foxit.common;
using foxit.common.fxcrt;
using foxit.pdf;
using foxit.pdf.annots;
using foxit.pdf.graphics;


namespace pdf_example
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Initialize FoxIt");

      ErrorCode error_code = Library.Initialize(Constants.foxitSerial, Constants.foxitKey);
      if (error_code != ErrorCode.e_ErrSuccess)
      {
        return;
      }

      Console.WriteLine("Creating PDF");
      RichTextStyle header = new RichTextStyle(
          new foxit.common.Font(foxit.common.Font.StandardID.e_StdIDHelvetica),
          text_size: 24.0f,
          text_alignment: Alignment.e_AlignmentLeft,
          text_color: 0x000000,
          is_bold: true,
          is_italic: false,
          is_underline: false,
          is_strikethrough: false,
          mark_style: RichTextStyle.CornerMarkStyle.e_CornerMarkNone
        );

      try
      {
        using (PDFDoc doc = new PDFDoc())
        {
          using (PDFPage page = doc.InsertPage(0, PDFPage.Size.e_SizeLetter))
          {

            var path = new Path();
            path.AppendEllipse(new RectF(200f, 200f, 300f, 300f));
            path.CloseFigure();

            var go = PathObject.Create();
            go.SetFillOpacity(0.5f);
            go.SetPathData(path);
            go.SetFillMode(FillMode.e_FillModeWinding);
            go.SetFillColor(0xFF0000);

            page.InsertGraphicsObject(0, go);

            go = PathObject.Create();
            path = new Path();
            // path.AppendRect(new RectF(150, 150, 250, 250));
            path.MoveTo(new foxit.common.fxcrt.PointF(200f, 150f));
            path.LineTo(new foxit.common.fxcrt.PointF(250f, 200f));
            path.LineTo(new foxit.common.fxcrt.PointF(200f, 250f));
            path.LineTo(new foxit.common.fxcrt.PointF(150f, 200f));
            path.CloseFigure();
            go.SetPathData(path);

            page.InsertGraphicsObject(0, go);

            page.AddText("Testing", new RectF(100f, 100f, 200f, 200f), header);

            page.Normalize();
            page.GenerateContent();
          }

          doc.SaveAs("test.pdf", 0);
        }
      }
      catch (PDFException e)
      {
          Console.WriteLine(e.Message);
        Console.WriteLine("Exception: {e}");
      }

    }
  }
}
