﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using SgvToPdf.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using static SgvToPdf.Services.SgvNetService;

namespace SgvToPdf.Services
{
    public class PdfServiceItextSharp : IPdfService
    {
        public Stream MultipleItems(IEnumerable<ScalableVectorGraphic> listOfSgv)
        {
            try
            {


                Document doc = new Document(PageSize.A4, 10, 10, 42, 35);
                MemoryStream stream = new MemoryStream();
                PdfWriter wri = PdfWriter.GetInstance(doc, stream);
                wri.CloseStream = false;
                doc.Open();

                foreach (var sgv in listOfSgv)
                {
                    ISvgService sgvToBitmap = new SgvNetService();
                    var bitmap = sgvToBitmap.SgvToBitmap(sgv.SgvSpecs);

                    Font pdfLabel = new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.GRAY);
                    Chunk labelTitleChunk = new Chunk("Title", pdfLabel);
                    Paragraph titleLabel = new Paragraph(labelTitleChunk);
                    titleLabel.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titleLabel);

                    Font Title = new Font(Font.FontFamily.HELVETICA, 18f, Font.NORMAL,BaseColor.BLACK);
                    Chunk c1 = new Chunk(sgv.Title, Title);
                    Paragraph title = new Paragraph(c1);
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);


                    Chunk DateChunk = new Chunk("Created on: " + sgv.DateCreated, pdfLabel);
                    Paragraph Date = new Paragraph(DateChunk);
                    Date.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(Date);

                    doc.Add(new Chunk("\n"));
                    Chunk labelImageChunk = new Chunk("Image", pdfLabel);
                    Paragraph imgaeLabel = new Paragraph(labelImageChunk);
                    imgaeLabel.Alignment = Element.ALIGN_CENTER;
                    doc.Add(imgaeLabel);

                    iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp);
                    doc.Add(pic);

                 

                    doc.NewPage();
                }


                doc.Close();
                stream.Flush();
                stream.Position = 0;
                return stream;
            }
            catch (DocumentException de)
            {
                throw new DocumentException("Your document could not be created");
        
            }
            catch (IOException ioe)
            {
                throw new DocumentException("Your document could not be saved, Please check permisions");

            }
            catch (SgvToBitmapDrawException sgve)
            {
                throw new DocumentException(sgve.Message);

            }
            catch (Exception ex)
            {
                throw new Exception("Sorry, something goes really  wrong");
            }

        }

        public Stream WriteSingleItem()
        {
            throw new NotImplementedException();
        }
    }
}