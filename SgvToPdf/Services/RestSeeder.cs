using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SgvToPdf.Models;

namespace SgvToPdf.Services
{
    public class RestSeeder
    {

        public void SeedSgvItems()
        {
            var client = new RestClient("http://localhost:55972/api/sgvApi/");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");


            var sgvItem = new ScalableVectorGraphic()
            {
                Id= Guid.NewGuid(),
                Title ="Sample 1",
                DateCreated= DateTime.UtcNow,
                SgvSpecs = "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">\r\n<svg x=\"0\" y=\"0\" width=\"210mm\" height=\"297mm\" overflow=\"inherit\" viewBox=\"0, 0, 210, 297\" preserveAspectRatio=\"xMidYMid\" font-size=\"0\" id=\"svg8\" xml:space=\"default\" dc=\"http://purl.org/dc/elements/1.1/\" cc=\"http://creativecommons.org/ns#\" rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" svg=\"http://www.w3.org/2000/svg\" sodipodi=\"http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd\" inkscape=\"http://www.inkscape.org/namespaces/inkscape\" docname=\"inkscape boxes.svg\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:xml=\"http://www.w3.org/XML/1998/namespace\" version=\"1.1\">\r\n  <defs id=\"defs2\" xml:space=\"default\" />\r\n  <namedview id=\"base\" xml:space=\"default\" pagecolor=\"#ffffff\" bordercolor=\"#666666\" borderopacity=\"1.0\" pageopacity=\"0.0\" pageshadow=\"2\" zoom=\"0.35\" cx=\"400\" cy=\"560\" document-units=\"mm\" current-layer=\"layer1\" showgrid=\"false\" window-width=\"1366\" window-height=\"705\" window-x=\"-8\" window-y=\"892\" window-maximized=\"1\" />\r\n  <metadata id=\"metadata5\" xml:space=\"default\"></metadata>\r\n  <g id=\"layer1\" xml:space=\"default\" label=\"Layer 1\" groupmode=\"layer\">\r\n    <rect x=\"22.67857\" y=\"24.10119\" width=\"167.8214\" height=\"216.2024\" stroke-width=\"0.2645833\" id=\"rect3699\" xml:space=\"default\" />\r\n    <rect x=\"67.27976\" y=\"70.97024\" width=\"77.86309\" height=\"108.8571\" stroke-width=\"0.2645833\" id=\"rect3701\" xml:space=\"default\" style=\"fill:#FF0000;\" />\r\n  </g>\r\n</svg>"

            };

            request.AddJsonBody(sgvItem);
            IRestResponse response = client.Execute(request);
        }

    }
}