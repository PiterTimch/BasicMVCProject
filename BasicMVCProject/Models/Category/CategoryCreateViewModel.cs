﻿using System.ComponentModel.DataAnnotations;

namespace BasicMVCProject.Models.Category
{
    public class CategoryCreateViewModel
    {
        [Display(Name = "Назва категорії")]
        [DataType(DataType.Text)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Опис")]
        [DataType(DataType.Text)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Зображення")]
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
