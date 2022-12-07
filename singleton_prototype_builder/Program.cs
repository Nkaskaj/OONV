namespace singleton_prototype_builder
{
    using System.IO;
    class Program {         
        static void Main(string[] args){
            ///Singleton start
            System.Console.WriteLine(CSVDatabaze.getInstance().get());
            Analytik anal = new Analytik();
            anal.write("cus mrdko");
            anal.read();
            Analytik anal2 = new Analytik();
            anal2.write("cus mrdko HAHA");
            anal2.read();
            CSVDatabaze.getInstance().set("C:\\Users\\PC\\Desktop\\Škola\\OONV\\singleton_prototype_builder\\csv\\soubor2.csv");
            anal.write("cus mrdko už v jinym souboru");
            anal.read();
            anal2.write("cus mrdko v jinym soboru druhym analem");
            anal2.read();
            ///Singleton end

            ///Prototype start
            Postava postava1 = new Postava("frajer", 30, 5, 1, 0, 0);
            Postava postava2 = postava1.Clone();
            postava2.name = "Naklonovanej frajer";
            postava2.getAttack(postava1.attack - postava1.armor);
            postava1.info();
            postava2.info();
            ///Prototype end

            ///Builder start
            Kasarna kasarna = new Kasarna();
            
            ElfiVojakBuilder elfiBuilder = new ElfiVojakBuilder();
            NemrtvyVojakBuilder nemrtvyBuilder = new NemrtvyVojakBuilder();

            List<Vojak> elfiVojaci = new List<Vojak>(){};
            elfiVojaci.Add(kasarna.VytvorPesaka(elfiBuilder));
            elfiVojaci.Add(kasarna.VytvorKopinika(elfiBuilder));
            elfiVojaci.Add(kasarna.VytvorMaga(elfiBuilder));
            elfiVojaci.Add(kasarna.VytvorKopinika(elfiBuilder));

            List<Vojak> nemrtviVojaci = new List<Vojak>(){};
            nemrtviVojaci.Add(kasarna.VytvorPesaka(nemrtvyBuilder));
            nemrtviVojaci.Add(kasarna.VytvorPesaka(nemrtvyBuilder));
            nemrtviVojaci.Add(kasarna.VytvorLucistnika(nemrtvyBuilder));
            nemrtviVojaci.Add(kasarna.VytvorMaga(nemrtvyBuilder));
            ///Builder end
        }
    }
    ///Singleton start
    public class CSVDatabaze {
            private static CSVDatabaze instance;
            private static string cesta_k_csv_souboru;
            private CSVDatabaze(){
                cesta_k_csv_souboru = "C:\\Users\\PC\\Desktop\\Škola\\OONV\\singleton_prototype_builder\\csv\\soubor.csv";
            }
            public static CSVDatabaze getInstance()
            {
                if (instance == null)
                {
                    instance = new CSVDatabaze();
                }
                return instance;
            }
            public string get(){
                return cesta_k_csv_souboru;
            }

            public void set(string val){
                cesta_k_csv_souboru = val;
            }
        }

    public class Analytik {
        private string cesta;
        public Analytik(){
            reload();
        }
        public void read(){
            reload();
            using(var reader = new StreamReader(cesta)){
                System.Console.WriteLine(reader.ReadLine()); 
            }
        }
        public void write(string text){
            reload();
            using(var reader = new StreamWriter(cesta)){
                reader.WriteLine(text);
            }
        }
        public void reload(){
            cesta = CSVDatabaze.getInstance().get();
        }
    }
    ///Singleton end

    ///Prototype start
    interface IClone<T>{
        public T Clone();
    }

    public class Postava : IClone<Postava>{
        public string name {get; set;}
        public int hp {get; set;}
        public int armor {get; set;}
        public int attack {get; set;}
        public int xPos {get; set;}
        public int yPos {get; set;}

        public Postava(string name, int hp, int attack, int armor, int xPos, int yPos){
            this.name = name;
            this.hp = hp;
            this.attack = attack;
            this.armor = armor;
            this.xPos = xPos;
            this.yPos = yPos;
        }

        public void move(int xPos, int yPos){
            this.xPos += xPos;
            this.yPos += yPos;
        }

        public void getAttack(int attack){
            this.hp -= attack;
        }

        public void info(){
            System.Console.WriteLine("Postava " + this.name + " má " + this.hp.ToString() + " HP, " + this.armor.ToString() + " armoru a nachází se na pozici [" + this.xPos.ToString() + "," + this.yPos.ToString() + "]!");
        }

        public Postava Clone(){
            return new Postava(this.name, this.hp, this.attack, this.armor, this.xPos, this.yPos);
        }
    }
    ///Prototype end

    ///Builder start
    class Vojak{

        public int Hp {get; set;}
        public int Mp {get; set;}
        public int Attack {get; set;}
        public int Defense {get; set;}
        public List<string> Inventar {get; set;}

        public Vojak(int hp, int mp, int attack, int defense){
            this.Hp = hp;
            this.Mp = mp;
            this.Attack = attack;
            this.Defense = defense;
            this.Inventar = new List<string>();
        }
    }

    interface IBuilder{
        void Reset();
        void PridejBrneni();
        void PridejMec();
        void PridejLuk();
        void PridejKopi();
        void PridejPrilbu();
        void PridejChranice();
        void PridejStit();
        void PridejMagickouKnihu();
        Vojak VytrenujVojaka();
    }
    
    class LidskyVojakBuilder: IBuilder{
        private Vojak _lidskyVojak;

        public LidskyVojakBuilder(){
            this.Reset();
        }

        public void Reset(){
            this._lidskyVojak = new Vojak(50, 10, 5, 5);
        }

        public Vojak VytrenujVojaka(){
            Vojak vytvoreny_vojak = this._lidskyVojak;
            this.Reset();
            return vytvoreny_vojak;
        }

        public void PridejMec(){
            this._lidskyVojak.Inventar.Add("lidsky mec");
            this._lidskyVojak.Attack += 5;
        }

        public void PridejKopi(){
            this._lidskyVojak.Inventar.Add("lidske kopi");
            this._lidskyVojak.Attack += 7;
        }

        public void PridejLuk(){
            this._lidskyVojak.Inventar.Add("lidsky luk");
            this._lidskyVojak.Attack += 5;
        }

        public void PridejStit(){
            this._lidskyVojak.Inventar.Add("lidsky stit");
            this._lidskyVojak.Defense += 5;
        }

        public void PridejBrneni(){
            this._lidskyVojak.Inventar.Add("lidske brneni");
            this._lidskyVojak.Defense += 5;
        }
        
        public void PridejPrilbu(){
            this._lidskyVojak.Inventar.Add("lidska prilba");
            this._lidskyVojak.Defense += 3;
        }

        public void PridejChranice(){
            this._lidskyVojak.Inventar.Add("lidske chranice");
            this._lidskyVojak.Defense += 2;
        }

        public void PridejMagickouKnihu(){
            this._lidskyVojak.Inventar.Add("lidska magicka kniha");
            this._lidskyVojak.Mp += 10;
        }
    }

    class OrciVojakBuilder: IBuilder{
        private Vojak _orciVojak;

        public OrciVojakBuilder(){
            this.Reset();
        }

        public void Reset(){
            this._orciVojak = new Vojak(50, 10, 5, 5);
        }

        public Vojak VytrenujVojaka(){
            Vojak vytvoreny_vojak = this._orciVojak;
            this.Reset();
            return vytvoreny_vojak;
        }

        public void PridejMec(){
            this._orciVojak.Inventar.Add("orci mec");
            this._orciVojak.Attack += 5;
        }

        public void PridejKopi(){
            this._orciVojak.Inventar.Add("orci kopi");
            this._orciVojak.Attack += 7;
        }

        public void PridejLuk(){
            this._orciVojak.Inventar.Add("orci luk");
            this._orciVojak.Attack += 5;
        }

        public void PridejStit(){
            this._orciVojak.Inventar.Add("orci stit");
            this._orciVojak.Defense += 5;
        }

        public void PridejBrneni(){
            this._orciVojak.Inventar.Add("orci brneni");
            this._orciVojak.Defense += 5;
        }
        
        public void PridejPrilbu(){
            this._orciVojak.Inventar.Add("orci prilba");
            this._orciVojak.Defense += 3;
        }

        public void PridejChranice(){
            this._orciVojak.Inventar.Add("orci chranice");
            this._orciVojak.Defense += 2;
        }

        public void PridejMagickouKnihu(){
            this._orciVojak.Inventar.Add("orci magicka kniha");
            this._orciVojak.Mp += 10;
        }
    }

    class ElfiVojakBuilder: IBuilder{
        private Vojak _elfiVojak;

        public ElfiVojakBuilder(){
            this.Reset();
        }

        public void Reset(){
            this._elfiVojak = new Vojak(50, 10, 5, 5);
        }

        public Vojak VytrenujVojaka(){
            Vojak vytvoreny_vojak = this._elfiVojak;
            this.Reset();
            return vytvoreny_vojak;
        }

        public void PridejMec(){
            this._elfiVojak.Inventar.Add("elfi mec");
            this._elfiVojak.Attack += 5;
        }

        public void PridejKopi(){
            this._elfiVojak.Inventar.Add("elfi kopi");
            this._elfiVojak.Attack += 7;
        }

        public void PridejLuk(){
            this._elfiVojak.Inventar.Add("elfi luk");
            this._elfiVojak.Attack += 5;
        }

        public void PridejStit(){
            this._elfiVojak.Inventar.Add("elfi stit");
            this._elfiVojak.Defense += 5;
        }

        public void PridejBrneni(){
            this._elfiVojak.Inventar.Add("elfi brneni");
            this._elfiVojak.Defense += 5;
        }
        
        public void PridejPrilbu(){
            this._elfiVojak.Inventar.Add("elfi prilba");
            this._elfiVojak.Defense += 3;
        }

        public void PridejChranice(){
            this._elfiVojak.Inventar.Add("elfi chranice");
            this._elfiVojak.Defense += 2;
        }

        public void PridejMagickouKnihu(){
            this._elfiVojak.Inventar.Add("elfi magicka kniha");
            this._elfiVojak.Mp += 10;
        }
    }

    class NemrtvyVojakBuilder: IBuilder{
        private Vojak _nemrtvyVojak;

        public NemrtvyVojakBuilder(){
            this.Reset();
        }

        public void Reset(){
            this._nemrtvyVojak = new Vojak(50, 10, 5, 5);
        }

        public Vojak VytrenujVojaka(){
            Vojak vytvoreny_vojak = this._nemrtvyVojak;
            this.Reset();
            return vytvoreny_vojak;
        }

        public void PridejMec(){
            this._nemrtvyVojak.Inventar.Add("nemrtvy mec");
            this._nemrtvyVojak.Attack += 5;
        }

        public void PridejKopi(){
            this._nemrtvyVojak.Inventar.Add("nemrtve kopi");
            this._nemrtvyVojak.Attack += 7;
        }

        public void PridejLuk(){
            this._nemrtvyVojak.Inventar.Add("nemrtvy luk");
            this._nemrtvyVojak.Attack += 5;
        }

        public void PridejStit(){
            this._nemrtvyVojak.Inventar.Add("nemrtvy stit");
            this._nemrtvyVojak.Defense += 5;
        }

        public void PridejBrneni(){
            this._nemrtvyVojak.Inventar.Add("nemrtve brneni");
            this._nemrtvyVojak.Defense += 5;
        }
        
        public void PridejPrilbu(){
            this._nemrtvyVojak.Inventar.Add("nemrtva prilba");
            this._nemrtvyVojak.Defense += 3;
        }

        public void PridejChranice(){
            this._nemrtvyVojak.Inventar.Add("nemrtve chranice");
            this._nemrtvyVojak.Defense += 2;
        }

        public void PridejMagickouKnihu(){
            this._nemrtvyVojak.Inventar.Add("nemrtva magicka kniha");
            this._nemrtvyVojak.Mp += 10;
        }
    }

    class Kasarna{
        public Vojak VytvorPesaka(IBuilder builderRasy){
            builderRasy.Reset();
            builderRasy.PridejMec();
            builderRasy.PridejStit();
            builderRasy.PridejBrneni();
            builderRasy.PridejPrilbu();
            return builderRasy.VytrenujVojaka();
        }

        public Vojak VytvorLucistnika(IBuilder builderRasy){
            builderRasy.Reset();
            builderRasy.PridejLuk();
            builderRasy.PridejPrilbu();
            return builderRasy.VytrenujVojaka();
        }

        public Vojak VytvorKopinika(IBuilder builderRasy){
            builderRasy.Reset();
            builderRasy.PridejKopi();
            builderRasy.PridejPrilbu();
            builderRasy.PridejChranice();
            return builderRasy.VytrenujVojaka();
        }

        public Vojak VytvorMaga(IBuilder builderRasy){
            builderRasy.Reset();
            builderRasy.PridejChranice();
            builderRasy.PridejMagickouKnihu();
            return builderRasy.VytrenujVojaka();
        }
    }
    ///Builder end
}