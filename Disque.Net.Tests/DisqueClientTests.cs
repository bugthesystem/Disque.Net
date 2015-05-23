using System;
using System.Collections.Generic;
using Common.Testing.NUnit;
using FluentAssertions;
using NUnit.Framework;

namespace Disque.Net.Tests
{
    //DOCKERFILE: https://registry.hub.docker.com/u/squiidz/disque/
    public class DisqueClientTests : TestBase
    {
        private ISyncDisqueClient q;

        protected override void FinalizeSetUp()
        {
            q = new DisqueClient(new List<Uri> { new Uri("disque://192.168.59.103:7711") });
        }

        protected override void FinalizeTearDown()
        {
            q.Close();
        }


        [Test]
        public void AddJob()
        {
            string jobId = q.AddJob(GetQueueName(), "message", 10);
            jobId.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void AddJobWithParams()
        {
            JobParams jobParams = new JobParams
            {
                Replicate = 1,
                Retry = 10,
                Ttl = 10,
                Maxlen = 10,
                Delay = 10,
                Async = true
            };

            string jobId = q.AddJob(GetQueueName(), "message", 10, jobParams);
            jobId.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetJob()
        {

        }

        [Test]
        public void GetJobWithParams()
        {

        }

        [Test]
        public void AckJob()
        {

        }

        [Test]
        public void FastAck()
        {

        }

        [Test]
        public void Info()
        {

        }


        [Test]
        public void Qlen()
        {

        }
        [Test]
        [Ignore("pending")]
        public void Hello()
        {

        }

        [Test]
        [Ignore("pending (not yet implemented)")]
        public void Qstat()
        {
        }

        [Test]
        public void Qpeek()
        {
            // We're testing also the response parsing here

        }

        [Test]
        public void QpeekEmpty()
        {

        }

        [Test]
        public void QpeekInverse()
        {

        }

        [Test]
        public void Enqueue()
        {
        }

        [Test]
        public void Dequeue()
        {

        }

        [Test]
        public void DelJob()
        {

        }

        [Test]
        public void Show()
        {

        }

        [Test]
        public void Ping()
        {
        }

        [Test]
        public void Working()
        {

        }

        [Test]
        [Ignore("pending")]
        public void Scan()
        {
        }

        private static string GetQueueName()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}