using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Gestion_Entreprise;

namespace TestUnit
{
    [TestFixture()]
    public class Employee
    {
        [Test()]
        public void TestGetFirstname()
        {
            Assert.That(Julien.GetFirstname(),Is.EqualTo("Julien"));
        }
        

        [SetUp()]
        public void Init()
        {
            
        }
    }
}
