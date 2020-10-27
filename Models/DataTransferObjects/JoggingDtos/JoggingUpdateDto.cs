using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DataTransferObjects.JoggingDtos
{
    public class JoggingUpdateDto
    {
        [Required(ErrorMessage = "Jogging Date is required")]
        public DateTime JoggingDate { get; set; }
        [Required(ErrorMessage = "DistanceInMeters is required")]
        public double DistanceInMeters { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }
        [Required(ErrorMessage = "JoggingDurationInMinutes is required")]
        public int JoggingDurationInMinutes { get; set; }
    }
}
