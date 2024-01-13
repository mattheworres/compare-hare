using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.MediatR {
    public abstract class ApiRequestHandlerBase
    {
        protected static OkResult Ok()
        {
            return new OkResult();
        }

        protected static OkObjectResult Ok(object value)
        {
            return new OkObjectResult(value);
        }

        protected static StatusCodeResult Forbid()
        {
            return new StatusCodeResult((int)HttpStatusCode.Forbidden);
        }

        protected static StatusCodeResult BadRequest()
        {
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }
    }

}
