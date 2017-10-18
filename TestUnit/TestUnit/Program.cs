using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestUnit
{

    [TestFixture()]
    public class TestEmployee
    {
        [Test()]
        public void TestGetFirstname()
        {
            Assert.That(Julien.GetFirstname(), Is.EqualTo("Julien"));
        }
    }

    [TestFixture()]
    public class TestClient
    {    
        private Client client;


        [SetUp()]
        public void Init()
        {
            client = new Client("Juju & C0");
        }

        [Test()]
        public void TestGetName()
        {
            Assert.That(client.GetName(), Is.EqualTo("Juju & C0"));
        }
    }
}
