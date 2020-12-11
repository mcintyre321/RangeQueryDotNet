using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RangeQueryDotNet.Example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        [HttpGet]
        public ContentResult Get()
        {
            var value = "{\"locations\":[{\"name\":\"Seattle\",\"state\":\"WA\"},{\"name\":\"New York\",\"state\":\"NY\"},{\"name\":\"Bellevue\",\"state\":\"WA\"},{\"name\":\"Olympia\",\"state\":\"WA\"}]}";
            return Content(value, "application/json");
        }
        
    }
}