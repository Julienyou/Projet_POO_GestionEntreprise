using System;
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
        protected string company;
        protected Dictionary<int, int> salary_dico = new Dictionary<int, int>();

        public Employee(string firstname,string lastname,int salary,string company)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.salary = salary;
            this.company = company;
        }

        public virtual int ComputeSalary(int year)
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
            return this.salary_dico[year];            
        }
    }

    class Manager : Employee
    {
        private List<Consultant> consultants = new List<Consultant>();
        private Consultant consultant;

        public Manager(string firstname, string lastname, int salary, string company,Consultant consultant) : base(firstname, lastname, salary, company)
        {
            this.consultant = consultant;
            this.AddConsultant(this.consultant);
        }

        public override int ComputeSalary(int year)
        {
            salary += 500 * consultants.Count;
            AddSalary(year, salary);
            return salary;
        }

        public void AddConsultant(Consultant consultant)
        {
            consultants.Add(consultant);
        }

        public void RemoveConsultant(Consultant consultant)
        {
            consultants.Remove(consultant);
        }

        /*public string GetReport()
        {

        }*/
        
    }

    class Director : Employee
    {
        public Director(string firstname, string lastname, int salary, string company) : base(firstname, lastname, salary,company)
        {
        }

        public override int ComputeSalary(int year)
        {
            return salary;
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
        private string startPeriode;
        private string endPeriode;

        public Consultation(Client client, string startPeriode, string endPeriode)
        { 
            this.client = client;
            this.startPeriode = startPeriode;
            this.endPeriode = endPeriode;
        }

        public Client GetClient()
        {
            return this.client;
        }

        /*Give days between 2 dates*/
        public int GetPeriode()
        {
            Dictionary<string, int> months = new Dictionary<string, int>()
            {
                {"01", 31},
                {"02", 28},
                {"03", 31},
                {"04", 30},
                {"05", 31},
                {"06", 30},
                {"07", 31},
                {"08", 31},
                {"09", 30},
                {"10", 31},
                {"11", 30},
                {"12", 31}
            };

            int days = 0;

            String[] start = this.startPeriode.Split('/');
            String[] end = this.endPeriode.Split('/');

            int startDay = Convert.ToInt32(start[0]);
            int endDay = Convert.ToInt32(end[0]);
            int startMonth = Convert.ToInt32(start[1]);
            int endMonth = Convert.ToInt32(end[1]);
            int startYear = Convert.ToInt32(start[2]);
            int endYear = Convert.ToInt32(end[2]);

            /*If months and years are sames -> diff days*/
            if (endYear - startYear == 0)
            {
                /*If same years and month*/
                if (endMonth - startMonth == 0 && endDay - startDay != 0)
                {
                    return endDay - startDay;
                }
                else if (endMonth - startMonth == 1)
                {
                    return (months[start[1]] - startDay) + endDay;
                }
                /*If same years but not months*/
                else if (endMonth - startMonth > 1)
                {
                    for (int i = startMonth+1 ; i < endMonth; i++)
                    {
                        string month = Convert.ToString(i);

                        days += months[month];
                    }

                    /*  /!\ bug  */
                    return days += (months[start[1]] - startDay) + endDay; 
                }
                else { return 0; }
                
            }
            
            /*return -1 if error*/
                    return -1;
        }
    }

    class Consultant:Employee
    {
        //private Manager manager;
        private Consultation consultation;
        private List<Consultation> listConsultation = new List<Consultation>();
        private int prime;

        public Consultant(string firstname,string lastname,int salary,string company,Consultation consultation) : base(firstname,lastname,salary,company)
        {
            this.consultation = consultation;
            this.AddConsultation(consultation);
            this.prime = salary;
        }

        /*public Manager GetManager()
        {
            return this.manager;
        }*/

        /*Calculated salary/year with malus and bonus*/
        public override int ComputeSalary(int year)
        {
            foreach (Consultation consult in this.listConsultation)
            {
                Client client = consult.GetClient();
                int periode = consult.GetPeriode();

                /*Bonus of 250€/consultation*/
                if (client.GetName() != base.company)
                {
                    this.prime += 250;
                }
                /*If the consultant work in company -> malus 10€/day*/
                else
                {
                    this.prime -= 10 * periode;
                }
            }

            base.AddSalary(year,prime);

            return prime;
        }

        public Client GetClient()
        {
            return this.consultation.GetClient();
        }

        public string GetCompany()
        {
            return company;
        }

        public List<Consultation> GetConsultations()
        {
            return this.listConsultation;
        }

        public void AddConsultation(Consultation consult)
        {
            listConsultation.Add(consult);
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
            Julien = new Gestion_Entreprise.Employee("Julien", "Beard", 3000, "Name_company");
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

        /*[Test()]
        public void TestGetSalary()
        {
            Assert.That(Julien.GetSalary(2017), Is.EqualTo(3400));
        }*/


    }

    [TestFixture()]
    public class TestManager
    {
        private Gestion_Entreprise.Manager Julien;
        private Gestion_Entreprise.Consultant Ludovic;
        private Gestion_Entreprise.Client client;
        private Gestion_Entreprise.Consultation consult;
        private string startPeriode;
        private string endPeriode;

        [SetUp()]
        public void Init()
        {
            client = new Gestion_Entreprise.Client("Ludovic");
            startPeriode = "21/10/17";
            endPeriode = "05/11/17";
            consult = new Gestion_Entreprise.Consultation(client, startPeriode, endPeriode);
            Julien = new Gestion_Entreprise.Manager("Julien", "Beard", 3000, "Name_company",Ludovic);
            Julien.AddSalary(2017, 3400);
            Ludovic = new Gestion_Entreprise.Consultant("Ludovic", "Merel", 2500, "Name_company",consult);
            //Julien.AddConsultant(Ludovic);
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

        /*   [Test()]
           public void TestGetReport()
           {
               Assert.That(Julien.GetReport(), Is.EqualTo(3400));
           }*/
    }

    [TestFixture()]
    public class TestDirector
    {
        private Gestion_Entreprise.Director Bastien;

        [SetUp()]
        public void Init()
        {
            Bastien = new Gestion_Entreprise.Director("Bastien", "Paul", 3000, "Name_company");
            Bastien.AddSalary(2017, 3400);
        }

        [Test()]
        public void TestGetFirstname()
        {
            Assert.That(Bastien.GetFirstname(), Is.EqualTo("Bastien"));
        }

        [Test()]
        public void TestGetLastname()
        {
            Assert.That(Bastien.GetLastname(), Is.EqualTo("Paul"));
        }

        [Test()]
        public void TestGetSalary()
        {
            Assert.That(Bastien.GetSalary(2017), Is.EqualTo(3400));
        }

        /*   [Test()]
           public void TestComputeSalary(int year)
           {
               Assert.That(Bastien.ComputeSalary(2017), Is.EqualTo(...); ///à calculer
           }
       }*/
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
        private string startPeriode;
        private string endPeriode;

        [SetUp()]
        public void Init()
        {
            client = new Gestion_Entreprise.Client("Ludovic");
            startPeriode = "21/10/17";
            endPeriode = "05/11/17";
            consult = new Gestion_Entreprise.Consultation(client, startPeriode, endPeriode);
        }

        [Test()]
        public void TestGetClient()
        {
            Assert.That(consult.GetClient(), Is.EqualTo(client));
        }

        [Test()]
        public void TestGetPeriode()
        {
            Assert.That(consult.GetPeriode(), Is.EqualTo(15));
        }
    }

    [TestFixture()]
    public class TestConsultant
    {
        private Gestion_Entreprise.Client julien;
        private Gestion_Entreprise.Client name_compan;
        private Gestion_Entreprise.Consultation firstConsult;
        private Gestion_Entreprise.Consultation secondConsult;
        private string startPeriode;
        private string endPeriode;
        private Gestion_Entreprise.Consultant ludovic;
            
        [SetUp()]
        public void Init()
        {
            startPeriode = "20/10/17";
            endPeriode = "20/11/17";

            julien = new Gestion_Entreprise.Client("Julien");
            name_compan = new Gestion_Entreprise.Client("Name_compan");
            firstConsult = new Gestion_Entreprise.Consultation(julien, startPeriode, endPeriode);
            secondConsult = new Gestion_Entreprise.Consultation(name_compan, "20/10/17", "25/10/17");
            ludovic = new Gestion_Entreprise.Consultant("Ludovic","Merel",35000,
                                                        "Name_compan",firstConsult);
            ludovic.AddConsultation(secondConsult);
        }

        [Test()]
        public void TestGetClient()
        {
            Assert.That(ludovic.GetClient(), Is.EqualTo(julien));
        }

        [Test()]
        public void TestGetCompany()
        {
            Assert.That(ludovic.GetCompany(), Is.EqualTo("Name_compan"));
        }

        [Test()]
        public void TestGetConsultation()
        {
            Assert.That(ludovic.GetConsultations(), Is.EqualTo(new List<Gestion_Entreprise.Consultation> {firstConsult, secondConsult}));
        }

        [Test()]
        public void TestComputeSalary()
        {
            Assert.That(ludovic.ComputeSalary(2017), Is.EqualTo(35200));
        }

    }
}
