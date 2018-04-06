using SgvToPdf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgvToPdf.ViewModels
{
    public class SgvWithResizedInline
    {
        public  ScalableVectorGraphic sgv { get; set; }
        public string SgvResized { get; set; }
    }
}