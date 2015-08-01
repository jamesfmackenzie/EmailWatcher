# EmailWatcher

.NET library. Receive an event every time a new email drops into your inbox. Great for automation.

## Example

```csharp
// setup some options
var options = new EmailWatcherOptions {
  Host = "<POP host>",
  Username = "<POP username>",
  Password = "<POP password>",
  TimeBetweenRefreshes = 30 // seconds
  };
  
// create an Email Watcher and register a listener
var watcher = EmailWatcher.Public.EmailWatcher.WithOptions(options);
watcher.EmailReceivedEvent += (sender, args)
  => Console.WriteLine("Email Received! Id {0} Subject: {1}, Body: {2}", args.Message.Id, args.Message.Subject, args.Message.Body);

// start watching
watcher.StartWatching();
```
