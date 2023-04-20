using NATS.Client;
using System.Text;

const string NatsServer = "nats://localhost:4444";

Console.WriteLine("Worker Dir:");
ConnectionFactory cf = new ConnectionFactory();
Options opts = ConnectionFactory.GetDefaultOptions();
opts.Url = NatsServer;

IConnection c = cf.CreateConnection(opts);

EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
{
    Console.WriteLine($"worker received {args.Message}");

    var dirPath = Encoding.UTF8.GetString(args.Message.Data);
    if (Directory.Exists(dirPath))
    {
        Directory.GetDirectories(dirPath).ToList().ForEach(dir => c.Publish("directories", Encoding.UTF8.GetBytes(dir)));

        Directory.GetFiles(dirPath).ToList().ForEach(file => c.Publish("files", Encoding.UTF8.GetBytes(file)));
    }
    
};

IAsyncSubscription s = c.SubscribeAsync("directories", h);

while (true)
{
    Console.WriteLine("worker listening...");
    Thread.Sleep(TimeSpan.FromSeconds(5));
}