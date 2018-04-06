using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SgvToPdf.Models
{
    public class ScalableVectorGraphic
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="The Title of the sgv is required")]
        [Display(Name ="Title")]
        [StringLength(150, ErrorMessage ="Title can not be more that 150 Charaters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The SVG specification are required")]
        [Display(Name = "SVG specification")]
        [DataType(DataType.MultilineText)]
        public string SgvSpecs { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}