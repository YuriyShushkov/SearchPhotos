using NATS.Client;

const string NatsServer = "nats://localhost:4444";

Console.WriteLine("Worker:");
ConnectionFactory cf = new ConnectionFactory();
Options opts = ConnectionFactory.GetDefaultOptions();
opts.Url = NatsServer;

IConnection c = cf.CreateConnection(opts);

EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
{
    Console.WriteLine($"worker received {args.Message}");
};

IAsyncSubscription s = c.SubscribeAsync("worker", h);

while (true)
{
    Console.WriteLine("worker listening...");
    Thread.Sleep(TimeSpan.FromSeconds(5));
}