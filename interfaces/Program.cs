// interface 
using System;
using System.Collections.Generic;

namespace interfaces
{
        class Program
    {
        static void Main(string[] args)
        {
            Automobil auto = new Automobil();
            Bicykl kolo = new Bicykl();
            MotorovyClun clun = new MotorovyClun();
            Kachnicka kachna = new Kachnicka();
            Garaz garaz = new Garaz();
            auto.doleva();
            auto.dopredu();
            auto.zaparkuj();
            garaz.zaparkuj(auto);
            garaz.vyparkuj(auto);
            /*
            kachna.doleva();
            clun.zaparkuj();
            */
        }
    }

    interface IPojizdne
    {
        void dopredu();
        void doleva();
        void doprava();
    }

    interface IZaparkovatelne
    {
        void zaparkuj();
        void vyparkuj();
    }

    class Automobil : IPojizdne, IZaparkovatelne
    {
        int x = 0;
        int y = 0;
        public void dopredu()
        {
            x++;
            Console.WriteLine(x + " " + y);
        }

        public void doleva()
        {
            y--;
            Console.WriteLine(x + " " + y);
        }
        public void doprava()
        {
            y++;
            Console.WriteLine(x + " " + y);
        }
        public void zaparkuj()
        {
            x = 0;
            y = 0;
            Console.WriteLine("zaparkováno " + this.ToString());
        }
        public void vyparkuj()
        {
            x = 1;
            Console.WriteLine("vyparkováno " + this.ToString());
        }
    }
    class Bicykl : IPojizdne , IZaparkovatelne
    {
        public void dopredu()
        {
            x++;
            Console.WriteLine(x + " " + y);
        }

        public void doleva()
        {
            y--;
            Console.WriteLine(x + " " + y);
        }
        public void doprava()
        {
            y++;
            Console.WriteLine(x + " " + y);
        }
        int x = 0;
        int y = 0;
        public void zaparkuj()
        {
            x = 0;
            y = 0;
            Console.WriteLine("zaparkováno " + this.ToString());
        }
        public void vyparkuj()
        {
            x = 1;
            Console.WriteLine("vyparkováno " + this.ToString());
        }
    }

    class MotorovyClun : IPojizdne
    {
        public void dopredu()
        {
            x++;
            Console.WriteLine(x + " " + y);
        }

        public void doleva()
        {
            y--;
            Console.WriteLine(x + " " + y);
        }
        public void doprava()
        {
            y++;
            Console.WriteLine(x + " " + y);
        }
        int x = 0;
        int y = 0;
    }

    class Kachnicka
    {
        int x = 0;
        int y = 0;
    }

    class Hrabe
    {
        int x = 0;
        int y = 0;
    }
    class Garaz
    {
        List<IZaparkovatelne> objekty = new List<IZaparkovatelne>();

        public void zaparkuj(IZaparkovatelne objekt)
        {
            objekty.Add(objekt);
            System.Console.WriteLine("Zaparkováno:");
            objekty.ForEach(Console.WriteLine);
        }

        public void vyparkuj(IZaparkovatelne objekt)
        {
            objekt.vyparkuj();
            objekty.Remove(objekt);
            System.Console.WriteLine("Zaparkováno:");
            objekty.ForEach(Console.WriteLine);
        }

    }
}
