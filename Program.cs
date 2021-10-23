using System;
using System.Drawing;
using System.IO;
using System.Text;
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

      try
      {
        using (PDFDoc doc = new PDFDoc())
        {
          var sources = Directory.EnumerateFiles(args[0], "*.pdf");

          foreach (var source in sources)
          {
            var shortName = System.IO.Path.GetFileName(source);
            Console.WriteLine($"Loading {shortName}");

            var lastPage = doc.GetPageCount();
            using (PDFDoc src = new PDFDoc(source))
            {
              src.Load(Encoding.ASCII.GetBytes(""));
              doc.InsertDocument(0, src, 0);
            }
          }

          Console.WriteLine("Saving to test.pdf");
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
