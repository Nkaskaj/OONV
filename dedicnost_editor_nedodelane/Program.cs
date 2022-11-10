namespace dedicnost_editor_nedodelane
{
    class Program {         
        static void Main(string[] args)
        {
            Editor editor = new Editor();
            string cesta=@"C:\Users\PC\Desktop\Škola\OONV\dedicnost_editor_nedodelane\soubory";
            editor.createFile("txt", cesta);


            
        }
    }

    interface IUlozitelne {
        void ulozit();
    }

    interface INahratelne {
        void nahrat();
    }

    abstract class Soubor : IUlozitelne, INahratelne{
        public int id;
        public abstract void zapisodstavec(string text);
        public abstract void zapisnadpis(string text);
        public void ulozit(){
            System.Console.WriteLine("ulozit");
        }
        public void nahrat(){
            System.Console.WriteLine("nahrat");
        }
    }

    class TxtSoubor : Soubor {
        public override void zapisodstavec(string text){
            System.Console.WriteLine(text);
        }

        public override void zapisnadpis(string text)
        {
            System.Console.WriteLine(text);
        }
    }

    class HtmlSoubor : Soubor {
        public override void zapisodstavec(string text){
            System.Console.WriteLine(text);
        }

        public override void zapisnadpis(string text)
        {
            System.Console.WriteLine(text);
        }
    }

    class Editor {
        public void createFile(string pripona, string cesta){
            string file = @"\file." + pripona;
            using (TextWriter writer = File.CreateText(cesta + file))
            {
                if(pripona == "txt"){
                    writer.WriteLine("TXT soubor:");
                    TxtSoubor txtSoubor = new TxtSoubor();
                }
                else{
                    writer.WriteLine("HTML soubor:");
                    HtmlSoubor htmlSoubor = new HtmlSoubor();
                }
            }
        }
    }
}