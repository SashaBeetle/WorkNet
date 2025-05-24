using Worknet.API.Util;

var app = WebAppBuilder.BuildWebApp(args);
await WebAppConfigurer.ConfugureWebApp(app);

app.Run();