﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Gestion_Entreprise
{
    class Employee
    {
        protected string firstname;
        protected string lastname;
        protected int salary;
        protected int id;
        protected string company;
        protected Dictionary<int, int> salary_dico = new Dictionary<int, int>();

        public Employee(string firstname,string lastname,int salary,int id,string company)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.salary = salary;
            this.id = id;
            this.company = company;
        }

        public virtual int ComputeSalary()
        {
            return 0;
        }

        public string GetFirstname()
        {
            return this.firstname;
        }

        public string GetLastname()
        {
            return this.lastname;
        }

        public void AddSalary(int year, int salary)
        {
            this.salary_dico.Add(year, salary);
        }

        public int GetSalary(int year)
        {
            return 0;            
        }
    }

    class Client
    {
        private string name;

        public Client(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return this.name;
        }
    }

    class Consultation
    { 
        private Client client;
        private int startPeriode;
        private int endPeriode;

        public Consultation(Client client, int startPeriode, int endPeriode)
        { 
            this.client = client;
            this.startPeriode = startPeriode;
            this.endPeriode = endPeriode;
        }

        public Client GetClient()
        {
            return this.client;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}

namespace TestUnit
{

    [TestFixture()]
    public class TestEmployee
    {
        private Gestion_Entreprise.Employee Julien;

        [SetUp()]
        public void Init()
        {
            Julien = new Gestion_Entreprise.Employee("Julien", "Beard", 3000, 15172, "Name_company");
            Julien.AddSalary(2017, 3400);
        }

        [Test()]
        public void TestGetFirstname()
        {
            Assert.That(Julien.GetFirstname(), Is.EqualTo("Julien"));
        }

        [Test()]
        public void TestGetLastname()
        {
            Assert.That(Julien.GetLastname(), Is.EqualTo("Beard"));
        }

        [Test()]
        public void TestGetSalary()
        {
            Assert.That(Julien.GetSalary(2017), Is.EqualTo(3400));
        }


    }

    [TestFixture()]
    public class TestClient
    {
        private Gestion_Entreprise.Client client;

        [SetUp()]
        public void Init()
        {
            client = new Gestion_Entreprise.Client("Juju & C0");
        }

        [Test()]
        public void TestGetName()
        {
            Assert.That(client.GetName(), Is.EqualTo("Juju & C0"));
        }
    }

    [TestFixture()]
    public class TestConsultation
    {
        private Gestion_Entreprise.Client client;
        private Gestion_Entreprise.Consultation consult;

        [SetUp()]
        public void Init()
        {
            client = new Gestion_Entreprise.Client("Ludovic");
            consult = new Gestion_Entreprise.Consultation(client, 12, 13);
        }

        [Test()]
        public void TestGetClient()
        {
            Assert.That(consult.GetClient(), Is.EqualTo(client));
        }
    }
}