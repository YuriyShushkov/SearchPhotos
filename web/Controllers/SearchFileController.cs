using Microsoft.AspNetCore.Mvc;
using NATS.Client;
using System.Text;

namespace web.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchFileController : ControllerBase
{
    private const string NatsServer = "nats://localhost:4444";

    [HttpPost]
    public IActionResult Post([FromBody] string value)
    {
        Console.WriteLine($"api/snaphost {value}");
        var cf = new ConnectionFactory();
        var opts = ConnectionFactory.GetDefaultOptions();

        opts.Url = NatsServer;

        var c = cf.CreateConnection(opts);

        c.Publish("worker", Encoding.UTF8.GetBytes($"hello, world: {value}"));
        c.Close();
        return Ok();
    }
}
