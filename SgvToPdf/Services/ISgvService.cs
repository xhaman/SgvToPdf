using System.Drawing;
using System.Web;

namespace SgvToPdf.Services
{
    interface ISgvService
    {
         Bitmap SgvToBitmap(string sgv);
         string SgvResize(string sgv, int maxHeight, int maxWidth);
         string getXmlfromFile(HttpPostedFileBase image);

        bool ValidateInlineSgv(string xml);
    }



}
