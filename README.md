# Disque.Net
A simple .Net client for the Disque in-memory distributed queue [https://github.com/antirez/disque](https://github.com/antirez/disque)
> [Disque](https://github.com/antirez/disque) is ongoing experiment to build a distributed, in memory, message broker. Its goal is to capture the essence of the "Redis as a jobs queue" use case, which is usually implemented using blocking list operations, and move it into an ad-hoc, self-contained, scalable, and fault tolerant design, with simple to understand properties and guarantees, but still resembling Redis in terms of simplicity, performances, and implementation as a C non-blocking networked server.

**Installation**  
[NuGet - Disque.Net](https://www.nuget.org/packages/Disque.Net/)
```
Install-Package Disque.Net
```

####List of supported methods
- [Ping](#ping)
- [Info](#info)
- [Info By Section](#info-by-section)
- [AddJob](#addjob)
- [AddJob with Parameters](#addjob-with-params)
- [GetJob](#getjob)
- [GetJob with Parameters](#getjob-with-params)
- [Ackjob](#ackjob)
- [Qlen](#qlen)
- [Qpeek](#qpeek)
- [DelJob](#deljob)
- [Dequeue](#dequeue)
- [Enqueue](#enqueue)
- [Fastack](#fastack)
- [Show](#show)
- [Working](#working)

## Ping
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string pong = q.Ping();
```
## Info
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string info = q.Info();
```
## Info By Section
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string info = q.Info("server");
```
## AddJob
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string jobId = q.AddJob("myqueue", "message", 10);
```

## AddJob with Parameters
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
JobParams jobParams = new JobParams
            {
                Replicate = 1,
                Retry = 10,
                Ttl = 20,
                Maxlen = 10,
                Delay = 10,
                Async = true
            };
string jobId = q.AddJob("myqueue", "message", 10, jobParams);
```
## GetJob
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
List<Job> jobs = q.GetJob("myqueue");
```
## GetJob with Parameters
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
List<Job> jobs = q.GetJob(100, 2, "myqueue");
```
## AckJob
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string jobId = q.AddJob("myqueue", "message", 10);
long count = q.Ackjob(jobId);
```
## Qlen
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
long qlen = q.Qlen("myqueue");
```
## Qpeek
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
List<Job> jobs = q.Qpeek("myqueue", 2);
```
## DelJob
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string jobId = q.AddJob("myqueue", "testJob", 10);
long count = q.DelJob(jobId);
```
## Dequeue
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string jobId = q.AddJob("myqueue", "testJob", 10);
long count = q.Dequeue(jobId);
```
## Enqueue
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string jobId = q.AddJob("myqueue", "testJob", 10);
long count = q.Enqueue(jobId);
```
## Fastack
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string jobId = q.AddJob("fastack", "message", 10);
long count = q.Fastack(jobId);
```
## Show
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
string jobId = q.AddJob("myqueue", "testJob", 10);
JobInfo info = q.Show(jobId);
```
## Working
```csharp
var q = new DisqueClient(new Uri("disque://192.168.59.103:7711"));
String jobId = q.AddJob("myqueue", "testJob", 10);
long secs = q.Working(jobId);
```

##Bugs
If you encounter a bug, performance issue, or malfunction, please add an [Issue](https://github.com/ziyasal/Disque.Net/issues) with steps on how to reproduce the problem.

##TODO
- Async methods

##License

Code and documentation are available according to the *MIT* License (see [LICENSE](https://github.com/ziyasal/Disque.Net/blob/master/LICENSE))
