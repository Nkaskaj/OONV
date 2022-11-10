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
}