using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace SgvToPdf.Services
{
    class SgvNetService : ISvgService
    {


        // Sgv Operation Methods using https://github.com/vvvv/SVG 

        public Bitmap SgvToBitmap(string sgv)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sgv);
            SvgDocument FSvgDoc = new SvgDocument();
            FSvgDoc = SvgDocument.Open(xml);

            var bitmap = FSvgDoc.Draw();
            Size size = new Size() { Height = (int)FSvgDoc.Height, Width = (int)FSvgDoc.Width };
            var AspectRatio = ResizeKeepAspect(size, 400, 600);
            bitmap = new Bitmap(bitmap, new Size(AspectRatio.Width, AspectRatio.Height ));

            bitmap = DrawWhiteBackground(bitmap);
            return bitmap;

        }

        public Bitmap DrawWhiteBackground(Bitmap DrawArea)
        {
            Bitmap blank = new Bitmap(DrawArea.Width, DrawArea.Height);
            Graphics g = Graphics.FromImage(blank);
            g.Clear(Color.White);
            g.DrawImage(DrawArea, 0, 0, DrawArea.Width, DrawArea.Height);

            Bitmap tempImage = new Bitmap(blank);
            blank.Dispose();
            DrawArea.Dispose();

            DrawArea = new Bitmap(tempImage);


            tempImage.Dispose();
            return DrawArea;
        }


        public string SgvResize(string sgv, int maxHeight, int maxWidth)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sgv);

            SvgDocument FSvgDoc = new SvgDocument();
            FSvgDoc = SvgDocument.Open(xml);


            SizeF size = FSvgDoc.GetDimensions();
    
            //var Resized = ResizeKeepAspect(size, maxHeight, maxWidth);
  

            FSvgDoc.ViewBox = new SvgViewBox() { Height = size.Height, Width = size.Width };//TODO: Fix proper ratio
            FSvgDoc.Width = maxWidth;// Resized.Width;
            FSvgDoc.Height = new SvgUnit(SvgUnitType.None, 0);


            return FSvgDoc.GetXML();

        }

        public string getXmlfromFile(HttpPostedFileBase image)
        {
            if (image.ContentLength < 5000000 && (image.ContentType == "image/svg+xml"))
            {
                var document = image;
                Stream documentConverted = document.InputStream;//Using
                SvgDocument FSvgDoc = new SvgDocument();
                FSvgDoc = SvgDocument.Open<SvgDocument>(documentConverted);
                return FSvgDoc.GetXML();
            }
            else
            {
                return "";
            }
        }
 
        public bool ValidateInlineSgv(string xmlString)
        {
            try
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xmlString);
                XmlNodeList elemList = xmlDoc.GetElementsByTagName("svg");
                if (elemList.Count > 0)
                {
                    return true;
                }            
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static Size ResizeKeepAspect(Size src, int maxWidth, int maxHeight)
        {
            decimal rnd = Math.Min(maxWidth / (decimal)src.Width, maxHeight / (decimal)src.Height);
            return new Size((int)Math.Round(src.Width * rnd), (int)Math.Round(src.Height * rnd));
        }
    }
}