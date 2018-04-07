using System.Drawing;
using System.Web;

namespace SgvToPdf.Services
{
    public interface ISvgService
    {
         Bitmap SgvToBitmap(string sgv);
         string SgvResize(string sgv, int maxHeight, int maxWidth);
         string getXmlfromFile(HttpPostedFileBase image);

        bool ValidateInlineSgv(string xml);
    }



}
