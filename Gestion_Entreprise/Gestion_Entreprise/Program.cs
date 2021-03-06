﻿using System;
using System.IO;
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

        public string Firstname
        {
            get { return this.firstname; }
        }

        public string Lastname
        {
            get { return this.lastname; }
        }

        public void AddSalary(int year, int salary)
        {
            this.salary_dico[year] = salary;
        }

        public int GetSalary(int year)
        {
            return this.salary_dico[year];            
        }
    }

    class DF : Director
    {
        private List<Employee> employeeList;

        private string report_manager = "";
        private string report_consultant = "";
        private string report_df = "";
        private string report_drh = "";
        private string report_director = "";

        public DF(string firstname, string lastname, int salary, string company, List<Employee> employeeList) : base(firstname, lastname, salary, company)
        {
            this.employeeList = employeeList;
        }

       public void GetReport(string year, string path)
        {
            foreach(Employee employee in employeeList)
            {
                if (employee is Manager)
                {                   
                    employee.ComputeSalary(2017);
                    report_manager += String.Format("{0} {1} : {2}€\n\t", employee.Lastname, employee.Firstname, employee.GetSalary(2017));
                }

                if (employee is Consultant)
                {
                    employee.ComputeSalary(2017);
                    report_consultant += String.Format("{0} {1} : {2}€\n\t", employee.Lastname, employee.Firstname, employee.GetSalary(2017));
                }

                if (employee is DF)
                {
                    employee.ComputeSalary(2017);
                    report_df += String.Format("{0} {1} : {2}€\n\t", employee.Lastname, employee.Firstname, employee.GetSalary(2017));
                }

                if (employee is DRH)
                {
                    employee.ComputeSalary(2017);
                    report_drh += String.Format("{0} {1} : {2}€\n\t", employee.Lastname, employee.Firstname, employee.GetSalary(2017));
                }

                if (employee is Director && !(employee is DRH) && !(employee is DF))
                {
                    employee.ComputeSalary(2017);
                    report_drh += String.Format("{0} {1} : {2}€\n\t", employee.Lastname, employee.Firstname, employee.GetSalary(2017));
                }
            }

            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(path + @"\DF_Report_" + year + ".txt");

                //Write a line of text
                sw.WriteLine("Le directeur financier ayant fait le rapport est {0}\n", report_df);

                sw.WriteLine("DRH :\n\t{0}\n", report_drh);

                sw.WriteLine("Directeur :\n\t{0}\n", report_director);

                sw.WriteLine("Manager :\n\t{0}\n", report_manager);

                sw.WriteLine("Consultant :\n\t{0}\n", report_consultant);

                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("[Console] Le rapport du Directeur financier a été généré\n");
            }
        }
    }

    class Manager : Employee
    {
        private List<Consultant> consultants = new List<Consultant>();
        private int salaryYear;
        private string report = "";

        public Manager(string firstname, string lastname, int salary, string company) : base(firstname, lastname, salary, company)
        {
            this.salaryYear = salary;
        }

        public override int ComputeSalary(int year)
        {
            salaryYear = salary + 500 * consultants.Count;
            AddSalary(year, salaryYear);
            return salaryYear;
        }

        public void AddConsultant(Consultant consultant)
        {
            consultants.Add(consultant);
        }

        public void GetReport(string year, string path)
        {
            foreach(Consultant consultant in consultants)
            {
                report += String.Format("{0} {1} est actuellement dans la boite \"{2}\".\n",consultant.Lastname, consultant.Firstname, consultant.GetClient().Name);
            }

            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(path + @"\Manager_Report_" + this.Lastname + "_" + year + ".txt");

                //Write a line of text
                sw.WriteLine(report);

                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("[Console] Le rapport du Manager a été généré\n");
            }
        }
        
    }

    class Director : Employee
    {
        public Director(string firstname, string lastname, int salary, string company) : base(firstname, lastname, salary,company)
        {
        }

        public override int ComputeSalary(int year)
        {
            base.AddSalary(year, salary);
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

        public string Name
        {
            get { return this.name; }
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

        public string StartPeriode
        {
            get{ return startPeriode; }
        }

        public string EndPeriode
        {
            get{ return endPeriode; }
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
                        if (i < 10)
                        {
                            string month = "0" + Convert.ToString(i);
                            days += months[month];
                        }
                        else
                        {
                            string month = Convert.ToString(i);
                            days += months[month];
                        }
                    }

                    return days += (months[start[1]] - startDay) + endDay; 
                }
                else { return -1; }
                
            }
            
            /*return -1 if error*/
            return -1;
        }
    }

    class Consultant:Employee
    {
        private Manager manager;
        private Consultation consultation;
        private List<Consultation> listConsultation = new List<Consultation>();
        private int salaryYear;

        public Consultant(string firstname,string lastname,int salary,string company,Manager manager, Consultation consultation) : base(firstname,lastname,salary,company)
        {
            this.consultation = consultation;
            this.manager = manager;
            this.AddConsultation(consultation);
            this.salaryYear = salary;
        }

        public Manager GetManager()
        {
            return this.manager;
        }

        /*Calculated salary/year with malus and bonus*/
        public override int ComputeSalary(int year)
        {
            foreach (Consultation consult in this.listConsultation)
            {
                Client client = consult.GetClient();
                int periode = consult.GetPeriode();

                /*Bonus of 250€/consultation*/
                if (client.Name != base.company)
                {
                    this.salaryYear += 250;
                }
                /*If the consultant work in company -> malus 10€/day*/
                else
                {
                    this.salaryYear -= 10 * periode;
                }
            }

            salaryYear += manager.ComputeSalary(year) * 1/100; 

            base.AddSalary(year, salaryYear);

            return salaryYear;
        }
        /*Recupére le client actuel*/
        public Client GetClient()
        {
            return this.consultation.GetClient();
        }

        public string GetCompany()
        {
            return company;
        }

        public Consultation GetConsultation()
        {
            return consultation;
        }

        public List<Consultation> GetListConsultations()
        {
            return this.listConsultation;
        }

        public void AddConsultation(Consultation consult)
        {
            listConsultation.Add(consult);
        }
    }

    class DRH:Director
    {
        private List<Consultant> consultants;
        private int salaryYear;

        public DRH(string firstname, string lastname, int salary, string company, List<Consultant> consultants) : base(firstname, lastname, salary, company)
        {
            this.consultants = consultants;
            this.salaryYear = salary;
        }

        public void GetReport(string company, string year, string path)
        {
            string report = "";

            foreach (Consultant consult in consultants)
            {
                Consultation consultation = consult.GetConsultation();

                if (consultation.GetClient().Name == company)
                {
                    report += String.Format("{0} {1}:\n", consult.Lastname, consult.Firstname);
                    report += "     *Client : " + consult.GetClient().Name + "\n";
                    report += "     *Periode : " + consult.GetConsultation().StartPeriode + "-" +
                                                   consult.GetConsultation().EndPeriode + "\n";
                }
                
            }

            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(path + @"\DRH_Report_" + year +".txt");

                //Write a line of text
                sw.WriteLine(report);

                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("[Console] Le rapport du Directeur des ressources humaines a été généré\n");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Consultant> consultantsList = new List<Consultant>();
            List<Employee> employeesList = new List<Employee>();

            Dictionary<string, Client> clientDico = new Dictionary<string, Client>();
            Dictionary<string, Manager> managerDico = new Dictionary<string, Manager>();

            Dictionary<string, string> infoManager = new Dictionary<string, string>();
            Dictionary<string, string> infoConsultant = new Dictionary<string, string>();
            Dictionary<string, string> infoConsultation = new Dictionary<string, string>();

            Dictionary<string, string> dicoTampon = new Dictionary<string, string>();

            Director director = null;
            DF df = null;
            DRH drh = null;

            StreamReader sr = null;

            string companyName = null;
            string line = null;
            
            string path = @"..\..\..";

            Console.WriteLine("Veuillez saisir l'année des rapports que vous souhaitez :");
            string year = Console.ReadLine();
            
            try
            {
                sr = new StreamReader(path + @"\entreprise_" + year + ".txt");                
            }
            catch
            {
                Console.WriteLine("Erreur lors du traitement du fichier texte, verifier l'incrementation ou l'annee donnee");
                Console.ReadKey();
                Environment.Exit(0);
            }

            line = sr.ReadLine();

            /*Take name company*/
            if (line.Split(':')[0] == "CompanyName")
            {
                companyName = line.Split(':')[1];
            }

            while (line != null)
            {
                line = sr.ReadLine();

                /*Create client*/
                if (line == "[Client]")
                {
                    line = sr.ReadLine();
                    string name = line.Split(':')[1];

                    clientDico.Add(name.ToLower(), new Client(name));
                }

                /*Create manager*/
                else if (line == "[Manager]")
                {
                    line = sr.ReadLine();

                    /*Take utils informations for manager*/
                    int i = 0;
                    while (i < 3)
                    {
                        infoManager.Add(line.Split(':')[0], line.Split(':')[1]);

                        line = sr.ReadLine();

                        i++;
                    }

                    try
                    {
                        /*manager creation*/
                        managerDico.Add(infoManager["lastname"].ToLower(),
                                        new Manager(infoManager["firstname"],
                                        infoManager["lastname"],
                                        Convert.ToInt32(infoManager["salary"]),
                                        companyName));

                        employeesList.Add(managerDico[infoManager["lastname"].ToLower()]);
                    }
                    catch
                    {
                        Console.WriteLine("Erreur lors de la création du manager, verifiez l'implementation");
                    }

                    /*While if we have several consultants*/
                    while (line != null && line != "")
                    {
                        /*Take utils informations for consultant*/
                        while (line != null && line != "        [Consultation]")
                        {
                            if (line.Trim() == "[Consultant]") { line = sr.ReadLine(); }

                            else
                            {
                                infoConsultant.Add(line.Split(':')[0].Trim(), line.Split(':')[1].Trim());
                                line = sr.ReadLine();
                            }
                        }


                        /*Take utils informations for consultation*/
                        while (line != null && line != "" && line != "    [Consultant]")
                        {
                            if (line.Trim() == "[Consultation]") { line = sr.ReadLine(); }

                            else
                            {
                                infoConsultation.Add(line.Split(':')[0].Trim(), line.Split(':')[1].Trim());
                                line = sr.ReadLine();
                            }
                        }

                        Dictionary<string, Consultant> consultantDico = new Dictionary<string, Consultant>();

                        /*creation of the consultant with his consultation*/
                        consultantDico.Add(infoConsultant["lastname"],
                                            new Consultant(infoConsultant["firstname"],
                                                            infoConsultant["lastname"],
                                                            Convert.ToInt32(infoConsultant["salary"]),
                                                            companyName,
                                                            managerDico[infoManager["lastname"].ToLower()],
                                            new Consultation(clientDico[infoConsultation["client"].ToLower()],
                                                            infoConsultation["startPeriode"],
                                                            infoConsultation["endPeriode"])));

                        /*adding the consultant in the manager*/
                        managerDico[infoManager["lastname"].ToLower()].AddConsultant(consultantDico[infoConsultant["lastname"]]);

                        /*addition of the consultant in the list for the DRH*/
                        consultantsList.Add(consultantDico[infoConsultant["lastname"]]);

                        /*addition of the consultant in the list for the DF*/
                        employeesList.Add(consultantDico[infoConsultant["lastname"]]);

                        infoConsultant.Clear();
                        infoConsultation.Clear();
                        consultantDico.Clear();

                    }

                    infoManager.Clear();
                }

                /*Create director*/
                else if (line == "[Director]")
                {
                    line = sr.ReadLine();

                    /*Take utils informations for director*/
                    int i = 0;
                    while (i < 3)
                    {
                        dicoTampon.Add(line.Split(':')[0], line.Split(':')[1]);

                        line = sr.ReadLine();

                        i++;
                    }

                    try
                    {
                        /*director creation*/
                        director = new Director(dicoTampon["firstname"],
                                                dicoTampon["lastname"],
                                                Convert.ToInt32(dicoTampon["salary"]),
                                                companyName);

                        employeesList.Add(director);

                        dicoTampon.Clear();
                    }
                    catch
                    {
                        Console.WriteLine("Erreur lors de la création du directeur, verifiez l'implementation");
                    }

                }

                /*Create financial director*/
                else if (line == "[DF]")
                {
                    line = sr.ReadLine();

                    /*Take utils informations for DF*/
                    int i = 0;
                    while (i < 3)
                    {
                        dicoTampon.Add(line.Split(':')[0], line.Split(':')[1]);

                        line = sr.ReadLine();

                        i++;
                    }

                    try
                    {
                        /*financial director creation*/
                        df = new DF(dicoTampon["firstname"],
                                                dicoTampon["lastname"],
                                                Convert.ToInt32(dicoTampon["salary"]),
                                                companyName,
                                                employeesList);

                        employeesList.Add(df);

                        dicoTampon.Clear();
                    }
                    catch
                    {
                        Console.WriteLine("Erreur lors de la création du directeur financier, verifiez l'implementation");
                    }
                }

                /*Create human resources director*/
                else if (line == "[DRH]")
                {
                    line = sr.ReadLine();

                    /*Take utils informations for DRH*/
                    int i = 0;
                    while (i < 3)
                    {
                        dicoTampon.Add(line.Split(':')[0], line.Split(':')[1]);

                        line = sr.ReadLine();

                        i++;
                    }

                    try
                    {
                        /*human resources director creation*/
                        drh = new DRH(dicoTampon["firstname"],
                                                dicoTampon["lastname"],
                                                Convert.ToInt32(dicoTampon["salary"]),
                                                companyName,
                                                consultantsList);

                        employeesList.Add(drh);

                        dicoTampon.Clear();
                    }
                    catch
                    {
                        Console.WriteLine("Erreur lors de la création du directeur des resources humaines, verifiez l'implementation");
                    }
                }
            }

            Console.WriteLine("[Console] Vous pouvez generer trois types de rapport:");
            Console.WriteLine("    *Rapport manager:");
            Console.WriteLine("        Tapez manager.GetReport");
            Console.WriteLine("    *Rapport Directeur financier:");
            Console.WriteLine("        Tapez df.GetReport");
            Console.WriteLine("    *Rapport Directeur des ressources humaines:");
            Console.WriteLine("        Tapez drh.GetReport");
            Console.WriteLine("La commande 'Done' permet de fermer le programme\n");

            while(true)
            {
                Console.WriteLine("[Console] En attente d'une commande:");
                string commande = Console.ReadLine();
                commande += ".";

                /*generate drh raport*/
                if (commande.Split('.')[0].ToLower() == "drh" && commande.Split('.')[1].ToLower() == "getreport")
                {
                    while (true)
                    {
                        Console.WriteLine("[Console] Donner moi une company");
                        string company = Console.ReadLine().ToLower();

                        try
                        {
                            Client client = clientDico[company];

                            drh.GetReport(client.Name,year, path);

                            break;
                        }
                        catch
                        {
                            Console.WriteLine("[Console] Le nom du client ne se trouve pas dans ma base de donnée, verifiez le\n");
                        }
                    }
                    
                }

                /*generate dr raport*/
                else if (commande.Split('.')[0].ToLower() == "df" && commande.Split('.')[1].ToLower() == "getreport")
                {
                    df.GetReport(year, path);
                }
                
                /*generate manager raport*/
                else if (commande.Split('.')[0].ToLower() == "manager" && commande.Split('.')[1].ToLower() == "getreport")
                {
                    while (true)
                    {
                        Console.WriteLine("\n[Console] Quel est le nom du manager ?");
                        string name = Console.ReadLine();

                        try
                        {
                            Manager manager = managerDico[name.ToLower()];

                            manager.GetReport(year, path);

                            break;
                        }
                        catch
                        {
                            Console.WriteLine("[Console] Le nom ne figure pas dans ma base de donnée, verifiez le\n");
                        }
                    }

                }

                /*end console*/
                else if (commande.ToLower() == "done.")
                {
                    break;
                }

                else
                {
                    Console.WriteLine("[Console] Je n'ai pas compris votre demande\n");
                }
            }
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
            Assert.That(Julien.Firstname, Is.EqualTo("Julien"));
        }

        [Test()]
        public void TestGetLastname()
        {
            Assert.That(Julien.Lastname, Is.EqualTo("Beard"));
        }

        /*[Test()]
        public void TestGetSalary()
        {
            Assert.That(Julien.GetSalary(2017), Is.EqualTo(3400));
        }*/


    }

    [TestFixture()]
    public class TestDF
    {
        List<Gestion_Entreprise.Employee> employeeList = new List<Gestion_Entreprise.Employee>();
        private Gestion_Entreprise.DF Pascal;
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
            Julien = new Gestion_Entreprise.Manager("Julien", "Beard", 3000, "Name_company");
            Ludovic = new Gestion_Entreprise.Consultant("Ludovic", "Merel", 2500, "Name_company", Julien, consult);
            Julien.AddConsultant(Ludovic);
            employeeList.Add(Julien);
            Julien.AddSalary(2017, 3500);
            employeeList.Add(Ludovic);
            Ludovic.AddSalary(2017, 2500);
            Pascal = new Gestion_Entreprise.DF("Pascal", "Willems", 120000, "Name_company", employeeList);
        }

        /*Test bug*/
        /*[Test()]
        public void TestGetReport()
        {
            //Assert.That(employeeList, Is.SubsetOf(new List<Gestion_Entreprise.Employee>{ Julien, Ludovic, Julien, Ludovic }));
            Assert.That(Pascal.GetReport(2017), Is.EqualTo("Julien Beard : 3500\nLudovic Merel : 2500"));
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
            Julien = new Gestion_Entreprise.Manager("Julien", "Beard", 3000, "Name_company");

            Julien.AddSalary(2017, 3400);
            Ludovic = new Gestion_Entreprise.Consultant("Ludovic", "Merel", 2500, "Name_company",Julien,consult);
            Julien.AddConsultant(Ludovic);
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
            Assert.That(client.Name, Is.EqualTo("Juju & C0"));
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
            startPeriode = "20/09/17";
            endPeriode = "25/09/17";
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
            Assert.That(consult.GetPeriode(), Is.EqualTo(5));
        }
    }

    [TestFixture()]
    public class TestConsultant
    {
        private Gestion_Entreprise.Client julien;
        private Gestion_Entreprise.Consultation firstConsult;
        private string startPeriode;
        private string endPeriode;
        private Gestion_Entreprise.Consultant ludovic;
        private Gestion_Entreprise.Manager bob;
            
        [SetUp()]
        public void Init()
        {
            startPeriode = "20/10/17";
            endPeriode = "20/11/17";

            julien = new Gestion_Entreprise.Client("Julien");
            bob = new Gestion_Entreprise.Manager("Julien", "Beard", 60000, "Name_company");
            firstConsult = new Gestion_Entreprise.Consultation(julien, startPeriode, endPeriode);

            ludovic = new Gestion_Entreprise.Consultant("Ludovic","Merel",35000,
                                                        "Name_compan",bob ,firstConsult);
            bob.AddConsultant(ludovic);
        }

        [Test()]
        public void TestGetManager()
        {
            Assert.That(ludovic.GetManager(), Is.EqualTo(bob));
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
            Assert.That(ludovic.GetListConsultations(), Is.EqualTo(new List<Gestion_Entreprise.Consultation> {firstConsult}));
        }

        [Test()]
        public void TestComputeSalary()
        {
            Assert.That(ludovic.ComputeSalary(2017), Is.EqualTo(35855));
        }

    }

    [TestFixture()]
    public class TestDRH
    {
        private Gestion_Entreprise.Client julien;
        private Gestion_Entreprise.Consultation firstConsult;
        private Gestion_Entreprise.DRH drh;
        private Gestion_Entreprise.Consultant ludovic;
        private Gestion_Entreprise.Manager bob;

        private List<Gestion_Entreprise.Consultant> listConsult= new List<Gestion_Entreprise.Consultant>();

        string startPeriode = "20/10/17";
        string endPeriode = "20/11/17";        

        [SetUp()]
        public void Init()
        {
            julien = new Gestion_Entreprise.Client("Julien");
            bob = new Gestion_Entreprise.Manager("bob", "Beard", 60000, "Name_company");
            firstConsult = new Gestion_Entreprise.Consultation(julien, startPeriode, endPeriode);
            ludovic = new Gestion_Entreprise.Consultant("Ludovic", "Merel", 35000,
                                                        "Name_compan", bob, firstConsult);

            listConsult.Add(ludovic);

            drh = new Gestion_Entreprise.DRH("Franck", "test", 70000, "Name_company", listConsult);
            
        }

        /*Test bug*/
        /*[Test()]
        public void TestGetReport()
        {
            string report = "Boîte : Julien" + "\n" + "Periode : 20/10/17 - 20/11/17" + "\n";

            Assert.That(drh.GetReport(), Is.EqualTo(report));            
        }*/
    }
}