﻿using System.Collections.Generic;

namespace BusinessLogic.Contracts
{
    /// <summary>
    /// ДТО курса
    /// </summary>
    public class CourseDto
    {
        /// <summary>
        /// Название
        /// </summary>
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