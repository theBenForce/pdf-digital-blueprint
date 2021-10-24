using System;
using System.IO;
using System.Text;
using foxit;
using foxit.common;
using foxit.pdf;
using System.Linq;
using foxit.pdf.annots;
using foxit.common.fxcrt;

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
      RichTextStyle headerStyle = new RichTextStyle(
          new foxit.common.Font(foxit.common.Font.StandardID.e_StdIDHelvetica),
          text_size: 24.0f,
          text_alignment: Alignment.e_AlignmentLeft,
          text_color: 0xA0A0A0,
          is_bold: true,
          is_italic: false,
          is_underline: false,
          is_strikethrough: false,
          mark_style: RichTextStyle.CornerMarkStyle.e_CornerMarkNone
        );

      RichTextStyle bodyStyle = new RichTextStyle(
          new foxit.common.Font(foxit.common.Font.StandardID.e_StdIDHelvetica),
          text_size: 12.0f,
          text_alignment: Alignment.e_AlignmentLeft,
          text_color: 0x000000,
          is_bold: false,
          is_italic: false,
          is_underline: false,
          is_strikethrough: false,
          mark_style: RichTextStyle.CornerMarkStyle.e_CornerMarkNone
        );

      try
      {
        using (PDFDoc doc = new PDFDoc())
        {
          var sources = Directory.EnumerateFiles("input", "*.pdf").OrderBy((t) => t);

          foreach (var source in sources)
          {
            var shortName = System.IO.Path.GetFileName(source);
            Console.WriteLine($"Loading {source}");

            using (PDFDoc src = new PDFDoc(source))
            {
              src.Load(Encoding.ASCII.GetBytes(""));

              var lastPage = doc.GetPageCount();
              doc.InsertDocument(lastPage, src, 0);
            }
          }

          using (PDFPage page = doc.InsertPage(0, PDFPage.Size.e_SizeLetter))
          {
            var pageHeight = page.GetHeight();
            var pageWidth = page.GetWidth();
            var headerLocation = new RectF(
                left1: 36f,
                bottom1: pageHeight - 50f,
                right1: pageWidth - 36f,
                top1: pageHeight - 36f
              );

            page.AddText("Sources", headerLocation, headerStyle);


            var sourceList = new StringBuilder()
              .AppendJoin("\n", sources)
              .ToString();

            var sourceLocation = new RectF(36f, 36f, pageWidth - 36f, pageHeight - 64f);
            page.AddText(sourceList, sourceLocation, bodyStyle);

            page.Normalize();
            page.GenerateContent();

            using (var annot = page.AddAnnot(Annot.Type.e_Stamp, new RectF(36, 36, 200, 200)))
            {
              var stamp = new Stamp(annot);

              Image image = new Image("input/approved.png");
              stamp.SetImage(image, 0, 0);
              stamp.ResetAppearanceStream();
            }
          }

          Console.WriteLine("Saving to output.pdf");

          doc.SaveAs("output.pdf", 0);
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
