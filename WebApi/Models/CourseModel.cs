﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLogic.Contracts;

namespace WebApi.Models
{
    /// <summary>
    /// ДТО курса
    /// </summary>
    public class CourseModel
    {
        /// <summary>
        /// Название
        /// </summary>
        [MaxLength]
        public string Name { get; set; }
        
        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Уроки
        /// </summary>
        public List<LessonDto> Lessons { get; set; }
        /// <summary>
        /// Удалено
        /// </summary>
        public bool Deleted { get; set; }
    }
}