﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.DTOs.PatientDTOs
{
    public class DebartmentDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CurrentWorkingEmpolyee { get; set; } = "غير متوفر حاليا";
        public TimeSpan StartAt { get; set; }
        public TimeSpan EndAt { get; set; }
    }
}
