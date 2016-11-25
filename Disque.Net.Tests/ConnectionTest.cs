using System;
using System.Net.Sockets;
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
            _q = new DisqueClient(new Uri(TestConsts.ConnectionString));
            string info = _q.Info();

            info.Should().NotBeNullOrEmpty();

            _q.Close();
        }

        [Test]
        public void Throw_Exception_When_Nodes_Are_Unavailbale()
        {
            Assert.Throws<SocketException>(() =>
            {
                _q = new DisqueClient(new Uri(TestConsts.ConnectionString));
                _q.Info();
                _q.Close();
            });
        }
    }
}
