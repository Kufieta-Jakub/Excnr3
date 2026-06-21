using System;
using System.Collections.Generic;

namespace Excnr3
{
    // Abstrakcja
    abstract class MaterialBiblioteczny
    {
        // Enkapsulacja
        public int Id { get; private set; }
        public string Tytul { get; private set; }
        public string Autor { get; private set; }
        public bool CzyDostepny { get; private set; }

        public MaterialBiblioteczny(int id, string tytul, string autor)
        {
            Id = id;
            Tytul = tytul;
            Autor = autor;
            CzyDostepny = true;
        }

        // Polimorfizm
        public abstract string OpisTypu();
        public abstract int DniWypozyczenia();

        public void Wypozycz()
        {
            CzyDostepny = false;
        }

        public void Zwroc()
        {
            CzyDostepny = true;
        }

        public void PokazInfo()
        {
            string status = CzyDostepny ? "dostepny" : "wypozyczony";
            Console.WriteLine("[" + Id + "] " + Tytul + " - " + Autor +
                " (" + OpisTypu() + ") - " + status);
        }
    }

    // Dziedziczenie
    class Ksiazka : MaterialBiblioteczny
    {
        public int LiczbaStron { get; private set; }

        public Ksiazka(int id, string tytul, string autor, int liczbaStron)
            : base(id, tytul, autor)
        {
            LiczbaStron = liczbaStron;
        }

        public override string OpisTypu()
        {
            return "Ksiazka";
        }

        public override int DniWypozyczenia()
        {
            return 30;
        }
    }

    class Czasopismo : MaterialBiblioteczny
    {
        public int NumerWydania { get; private set; }

        public Czasopismo(int id, string tytul, string autor, int numerWydania)
            : base(id, tytul, autor)
        {
            NumerWydania = numerWydania;
        }

        public override string OpisTypu()
        {
            return "Czasopismo nr " + NumerWydania;
        }

        public override int DniWypozyczenia()
        {
            return 7;
        }
    }

    class Audiobook : MaterialBiblioteczny
    {
        public int DlugoscMinuty { get; private set; }

        public Audiobook(int id, string tytul, string autor, int dlugoscMinuty)
            : base(id, tytul, autor)
        {
            DlugoscMinuty = dlugoscMinuty;
        }

        public override string OpisTypu()
        {
            return "Audiobook (" + DlugoscMinuty + " min)";
        }

        public override int DniWypozyczenia()
        {
            return 14;
        }
    }

    // Abstrakcja
    interface IPowiadomienie
    {
        void Wyslij(string wiadomosc);
    }

    class PowiadomienieEmail : IPowiadomienie
    {
        public void Wyslij(string wiadomosc)
        {
            Console.WriteLine("E-MAIL: " + wiadomosc);
        }
    }

    class PowiadomienieSms : IPowiadomienie
    {
        public void Wyslij(string wiadomosc)
        {
            Console.WriteLine("SMS: " + wiadomosc);
        }
    }

    class Czytelnik
    {
        public int Id { get; private set; }
        public string Imie { get; private set; }
        public string Nazwisko { get; private set; }

        // Enkapsulacja
        private string email;

        public List<Wypozyczenie> Historia { get; private set; }

        public Czytelnik(int id, string imie, string nazwisko, string email)
        {
            Id = id;
            Imie = imie;
            Nazwisko = nazwisko;
            UstawEmail(email);
            Historia = new List<Wypozyczenie>();
        }

        public void UstawEmail(string nowyEmail)
        {
            if (nowyEmail.Contains("@"))
            {
                email = nowyEmail;
            }
            else
            {
                Console.WriteLine("Blad: nieprawidlowy e-mail, zostaje stary.");
            }
        }

        public string PobierzEmail()
        {
            return email;
        }
    }

    class Wypozyczenie
    {
        public Czytelnik Czytelnik { get; private set; }
        public MaterialBiblioteczny Material { get; private set; }
        public int DniNaZwrot { get; private set; }
        public bool CzyOddane { get; private set; }

        public Wypozyczenie(Czytelnik czytelnik, MaterialBiblioteczny material)
        {
            Czytelnik = czytelnik;
            Material = material;
            // Polimorfizm
            DniNaZwrot = material.DniWypozyczenia();
            CzyOddane = false;
        }

        public void Oddaj()
        {
            CzyOddane = true;
        }
    }

    class Biblioteka
    {
        private List<MaterialBiblioteczny> materialy;
        private List<Czytelnik> czytelnicy;
        private List<Wypozyczenie> wypozyczenia;

        private IPowiadomienie powiadomienie;

        public Biblioteka(IPowiadomienie powiadomienie)
        {
            materialy = new List<MaterialBiblioteczny>();
            czytelnicy = new List<Czytelnik>();
            wypozyczenia = new List<Wypozyczenie>();
            this.powiadomienie = powiadomienie;
        }

        public void DodajMaterial(MaterialBiblioteczny material)
        {
            materialy.Add(material);
        }

        public void ZarejestrujCzytelnika(Czytelnik czytelnik)
        {
            czytelnicy.Add(czytelnik);
        }

        public void WypozyczMaterial(Czytelnik czytelnik, MaterialBiblioteczny material)
        {
            if (material.CzyDostepny == false)
            {
                Console.WriteLine("Material \"" + material.Tytul + "\" jest juz wypozyczony.");
                return;
            }

            material.Wypozycz();
            Wypozyczenie wyp = new Wypozyczenie(czytelnik, material);
            wypozyczenia.Add(wyp);
            czytelnik.Historia.Add(wyp);

            powiadomienie.Wyslij(czytelnik.Imie + ", wypozyczyles \"" + material.Tytul +
                "\" na " + wyp.DniNaZwrot + " dni.");
        }

        public void ZwrocMaterial(Czytelnik czytelnik, MaterialBiblioteczny material)
        {
            material.Zwroc();
            foreach (Wypozyczenie wyp in czytelnik.Historia)
            {
                if (wyp.Material == material && wyp.CzyOddane == false)
                {
                    wyp.Oddaj();
                }
            }
            powiadomienie.Wyslij(czytelnik.Imie + ", dziekujemy za zwrot \"" + material.Tytul + "\".");
        }

        public void PokazMaterialy()
        {
            Console.WriteLine("--- Materialy w bibliotece ---");
            foreach (MaterialBiblioteczny m in materialy)
            {
                m.PokazInfo();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Biblioteka biblioteka = new Biblioteka(new PowiadomienieEmail());

            Ksiazka ksiazka = new Ksiazka(1, "Pan Tadeusz", "Adam Mickiewicz", 350);
            Czasopismo czasopismo = new Czasopismo(2, "Swiat Nauki", "Redakcja", 5);
            Audiobook audiobook = new Audiobook(3, "Wiedzmin", "Andrzej Sapkowski", 720);

            biblioteka.DodajMaterial(ksiazka);
            biblioteka.DodajMaterial(czasopismo);
            biblioteka.DodajMaterial(audiobook);

            Czytelnik czytelnik = new Czytelnik(101, "Anna", "Kowalska", "anna@mail.com");
            biblioteka.ZarejestrujCzytelnika(czytelnik);

            czytelnik.UstawEmail("zly-email");

            Console.WriteLine();
            biblioteka.PokazMaterialy();

            Console.WriteLine();
            biblioteka.WypozyczMaterial(czytelnik, ksiazka);
            biblioteka.WypozyczMaterial(czytelnik, audiobook);
            biblioteka.WypozyczMaterial(czytelnik, ksiazka);

            Console.WriteLine();
            biblioteka.PokazMaterialy();

            Console.WriteLine();
            biblioteka.ZwrocMaterial(czytelnik, ksiazka);

            Console.WriteLine();
            biblioteka.PokazMaterialy();

            Console.WriteLine();
            Console.WriteLine("Liczba wypozyczen czytelnika " + czytelnik.Imie +
                ": " + czytelnik.Historia.Count);
        }
    }
}