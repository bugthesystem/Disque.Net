using System;
using System.Collections.Generic;
using Common.Testing.NUnit;
using FluentAssertions;
using NUnit.Framework;

namespace Disque.Net.Tests
{
    public class ConnectionTest : TestBase
    {
        private ISyncDisqueClient _q;

        [Test]
        public void IterateOverHosts()
        {
            _q = new DisqueClient(new List<Uri> { new Uri("disque://192.168.59.103:7711") });
            string info = _q.Info();

            info.Should().NotBeNullOrEmpty();

            _q.Close();
        }

        [Test]
        public void Throw_Exception_When_Nodes_Are_Unavailbale()
        {
            Assert.Throws<DisqueConnectionException>(() =>
            {
                _q = new DisqueClient(new List<Uri> { new Uri("disque://192.168.59.103:55666") });
                _q.Info();
                _q.Close();
            });
        }
    }
}
