﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DataTransferObjects.JoggingDtos
{
    public class JoggingCreateDto
    {
        [Required(ErrorMessage = "Jogging Date is required")]
        public DateTime JoggingDate { get; set; }
        [Required(ErrorMessage = "DistanceInMeters is required")]
        [Range(1, double.MaxValue)]
        public double DistanceInMeters { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }
        [Required(ErrorMessage = "JoggingDurationInMinutes is required")]
        [Range(1, int.MaxValue)]
        public int JoggingDurationInMinutes { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
