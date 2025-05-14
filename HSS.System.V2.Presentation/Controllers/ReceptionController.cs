using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Services.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionController : CustomBaseController
    {
        private readonly IReceptionServices _receptionServices;

        public ReceptionController(IReceptionServices receptionServices)
        {
            _receptionServices = receptionServices;
        }
    }
}
