namespace proxy_decorator
{
    using System.IO;
    using System.Collections.Generic; 
    class Program {         
        static void Main(string[] args)
        {
            CSVedis csvedis = new CSVedis();
            System.Console.WriteLine(csvedis.read("pepa"));
            System.Console.WriteLine(csvedis.read("pepa"));
            System.Console.WriteLine(csvedis.read("martin"));
            System.Console.WriteLine(csvedis.read("pepa"));
            System.Console.WriteLine(csvedis.read("pepa"));
            System.Console.WriteLine(csvedis.read("mates"));
            System.Console.WriteLine(csvedis.read("martin"));
            System.Console.WriteLine(csvedis.read("martin"));
            System.Console.WriteLine(csvedis.read("anička"));
            System.Console.WriteLine(csvedis.read("anička"));
        }
    }

    class CSVedis {
        string cesta;
        Dictionary<string, List<string>> cache;
        
        public CSVedis(){
            this.cesta = "C:\\Users\\PC\\Desktop\\Škola\\OONV\\proxy_decorator\\znamky.csv";
            this.cache = new Dictionary<string, List<string>>(){};
        }

        public string read(string zak){
            List<string> itemCheck = check(zak);
            if(itemCheck != null){
                System.Console.WriteLine(zak + " bude načten z CSVedisu!");
                return this.view(itemCheck, zak);
            }

            var lines = File.ReadLines(cesta);
            string name = "";
            foreach(var line in lines){
                List<string> znamky = new List<string>();
                name = "";
                var values = line.Split(',');
                foreach(var value in values){
                    if(value == zak){
                        name = zak;
                    }else{
                        znamky.Add(value);
                    }
                }
                if(name != ""){
                    this.cache.Add(name, znamky);
                    return this.view(znamky, name);
                }
            }
            return "Žák jménem " + zak + " neexistuje!";
        }

        public List<string> check(string zak){
            foreach (string key in this.cache.Keys){
                if(key == zak){
                    return this.cache[key];
                }
            }
            return null;
        }

        public string view(List<string> znamky, string zak){
            string finZnamky = "";
            foreach(var znamka in znamky){
                finZnamky = finZnamky + znamka + ",";
            }
            finZnamky = zak + ": " + finZnamky.Substring(0, finZnamky.Length - 1);
            return finZnamky;
        }

        public void preview(){
            string values = "";
            foreach(KeyValuePair<string, List<string>> kv in this.cache){
                values = "";
                for(int i = 0; i <= kv.Value.Count -1; i++){
                    values = values + kv.Value[i];
                }
                Console.WriteLine("PREVIEW: " + kv.Key + " " + values);
            }
        }
    }
}