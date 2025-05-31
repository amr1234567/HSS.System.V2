using FluentResults;
using HSS.System.V2.Services.DTOs.MedicalLabDTOs;
using HSS.System.V2.Services.DTOs.PatientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Services.Contracts
{
    public interface IMedicalLabServices
    {
        Task<Result<MedicaLabTestDTO>> GetTestRequred(string appointmentId);
    }
}
