using HSS.System.V2.Domain.Attributes;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Controllers
{
    [AuthorizeByEnum(UserRole.Doctor)]
    [ApiExplorerSettings(GroupName = "ClinicAPI")]
    public class ClinicController : CustomBaseController
    {
        private readonly IClinicServices _clinicServices;

        public ClinicController(IClinicServices clinicServices)
        {
            _clinicServices = clinicServices;
        }
    }
}
