using SgvToPdf.Models;
using System.Collections.Generic;
using System.IO;

namespace SgvToPdf.Services
{
   public interface IPdfService
    {
         Stream WriteSingleItem();

        Stream MultipleItems(IEnumerable<ScalableVectorGraphic> listOfSgv);
    }

   
}
