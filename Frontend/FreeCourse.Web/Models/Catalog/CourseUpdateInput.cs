﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog
{
    public class CourseUpdateInput
    {

        [Required]
        public string Id { get; set; }

        [Required, DisplayName("Kurs İsmi")]
        public string Name { get; set; }

        [Required, DisplayName("Kurs Açıklaması")]
        public string Description { get; set; }

        [Required, DisplayName("Kurs Fiyatı")]
        public decimal Price { get; set; }

        public string UserId { get; set; }


        [DisplayName("Kurs Resmi")]
        public string Picture { get; set; }

        public FeatureVM Feature { get; set; }

        [Required, DisplayName("Kategori")]
        public string CategoryId { get; set; }
    }
}