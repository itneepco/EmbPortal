﻿using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class WorkOrderItemRequest
    {
        [Required, MaxLength(250)]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int ItemNo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int UomId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Please enter a non zero value")]
        public decimal UnitRate { get; set; }

        [Range(float.Epsilon, float.MaxValue, ErrorMessage = "Please enter a non zero value")]
        public float PoQuantity { get; set; }
    }
}
