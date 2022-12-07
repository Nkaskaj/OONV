using System;
using System.Collections.Generic;   

namespace zapocet{

    class Program {         
        static void Main(string[] args)
        {
            bool run = true;
            bool lose = false;
            bool stageRun = true;
            int stage = 1;
            bool lukSkip = false;

            List<string> dropped = new List<string>();
            List<string> drops1 = new List<string>() {"Lektvar obnovy", "Přilba [Tier 2]", "Přilba [Tier 2]", "Přilba [Tier 2]", "Brnění [Tier 2]", "Brnění [Tier 2]", "Brnění [Tier 2]", "Chrániče [Tier 2]", "Chrániče [Tier 2]", "Chrániče [Tier 2]", "Meč [Tier 2]", "Meč [Tier 2]", "Meč [Tier 2]", "Štít [Tier 2]", "Luk [Tier 2]", "Scroll [Tier 2]"};
            List<string> drops2 = new List<string>() {"Pomocník [Tier 1]", "Přilba [Tier 3]", "Přilba [Tier 3]", "Přilba [Tier 3]", "Brnění [Tier 3]", "Brnění [Tier 3]", "Brnění [Tier 3]", "Chrániče [Tier 3]", "Chrániče [Tier 3]", "Chrániče [Tier 3]"};
            List<string> drops3 = new List<string>() {"Meč [Tier 3]", "Meč [Tier 3]", "Meč [Tier 3]", "Štít [Tier 3]", "Luk [Tier 3]", "Scroll [Tier 3]"};
            List<string> drops4 = new List<string>() {"Štít [Legendary]", "Luk [Legendary]", "Scroll [Legendary]"};
            List<List<string>> drops = new List<List<string>>() {drops1, drops2, drops3, drops4};

            (dropped, drops) = Drop(drops);
            foreach(string item in drops[0]){
                System.Console.WriteLine("- " + item);
            }
            Console.ReadLine();

            List<string> inventar = new List<string>();
            inventar.Add("OKO");
            inventar.Add("OKO");

            Kasarna kasarna = new Kasarna();

            LidskyVojakBuilder lidskyBuilder = new LidskyVojakBuilder();
            MonsterVojakBuilder monsterBuilder = new MonsterVojakBuilder();
            
            List<Postava> lideList = new List<Postava>();
            lideList.Add(kasarna.VytvorWarrirora(lidskyBuilder, "Lidský Warrior", stage));
            lideList.Add(kasarna.VytvorArchera(lidskyBuilder, "Lidský Archer", stage));
            lideList.Add(kasarna.VytvorMaga(lidskyBuilder, "Lidský Mage", stage));

            List<Postava> monstraList = new List<Postava>();
            monstraList = SpawnEnemy(kasarna, monsterBuilder, stage);
            
            while(run == true){
                MainMenu(lideList, stage.ToString(), inventar);
                stageRun = true;
                while(stageRun == true){
                    lukSkip = false;

                    lukSkip = Luk(lideList[0], monstraList[0]);
                    
                    if(lukSkip == false){
                        Round(lideList[0], monstraList[0]);
                        stageRun = WinCheck(monstraList,lideList);
                        if(stageRun == false) {break;}
                        Spell(lideList[0], monstraList[0]);
                        stageRun = WinCheck(monstraList,lideList);

                        if(stageRun == false) {break;}
                       
                        Round(monstraList[0], lideList[0]);
                        stageRun = WinCheck(lideList,monstraList);
                        if(stageRun == false) {break;}
                        Spell(monstraList[0], lideList[0]);
                        stageRun = WinCheck(lideList,monstraList);
                    }
                    stageRun = WinCheck(lideList,monstraList);
                    stageRun = WinCheck(monstraList,lideList);
                    
                    if(stageRun == false) {break;}

                    lideList = ChangePos(lideList);
                    monstraList = ChangePos(monstraList);
                    System.Console.WriteLine();
                    
                }
                System.Console.WriteLine();
                System.Console.WriteLine("                ---> Stage " + stage.ToString() + " dohrána <---   ");
                Console.ReadLine();
                lose = LoseCheck(lideList);
                if(lose){
                    run = false;
                }else{
                    stage += 1;
                    Console.Clear();
                    System.Console.WriteLine("                ---> Výsledky <---   ");
                    HealthRegen(lideList);
                    SipyRegen(lideList);
                    //DROPY
                    StageResult(lideList);
                    
                    Console.ReadLine();
                    monstraList = SpawnEnemy(kasarna, monsterBuilder, stage);
                }
                
                if(stage == 4) {run = false;}
                //dropy, WIN, give all
                //Itemy: Armor Ts, Zbrane Ts, Stit Ts
                //Bonusy: Full Regen HP+MANA, Revive, Pomocník
            }
            if(lose){
                System.Console.WriteLine("PROHRAL SI!");
            }else{
                System.Console.WriteLine("VYHRAL SI!");
            }
        }

        public static void Round(Postava postava1, Postava postava2){
            System.Console.WriteLine("Nyní bojuje " + postava1.jmeno + " [" + postava1.hp + "] proti " + postava2.jmeno + " [" + postava2.hp + "]!");
            bool dodged = false;
            if(postava2.dodge != 0){
                if(postava2.dodge >= Roll()){
                    dodged = true;
                }
            }
            if(dodged == false){
                Utok(postava1, postava2);
            }
            else{
                System.Console.WriteLine("     [#] " + postava2.jmeno + " dodgnul útok!");
            }
        }

            

        public static void Utok(Postava postava1, Postava postava2){
            double reduction= 0.0;
            int dmg = 0;
            int rawDMG = 0;
            
            rawDMG = postava1.DmgCalc();
            reduction = 4.0 / ( 4.0 + Convert.ToDouble(postava2.armor));
            dmg = Convert.ToInt32(Convert.ToDouble(rawDMG) * reduction);

            postava2.DostanDmg(dmg);
            System.Console.WriteLine(postava1.jmeno + " [" + postava1.hp + "] hitnul " + postava2.jmeno + " [" + postava2.hp + "] za " + dmg.ToString() + " HP! Vyblokováno " + (rawDMG - dmg).ToString() + " damage!");
        }

        public static void UtokLukem(Postava postava1, Postava postava2){
            double reduction= 0.0;
            int dmg = 0;
            int rawDMG = 0;
            int luk = Int32.Parse(postava1.inventar["Luk"].Substring(postava1.inventar["Luk"].Length - 2, 1));
            int toulec1 = Int32.Parse(postava1.inventar["Toulec"]);
            int toulec2 = Int32.Parse(postava2.inventar["Toulec"]);

            System.Console.WriteLine("Nyní střílí lukem " + postava1.jmeno + " [" + postava1.hp + "] do " + postava2.jmeno + " [" + postava2.hp + "]!");
            rawDMG = 30 * luk;
            reduction = 4.0 / ( 4.0 + Convert.ToDouble(postava2.armor));
            dmg = Convert.ToInt32(Convert.ToDouble(rawDMG) * reduction);
            postava2.DostanDmg(dmg);
            System.Console.WriteLine(postava1.jmeno + " [" + postava1.hp + "] hitnul lukem " + postava2.jmeno + " [" + postava2.hp + "] za " + dmg.ToString() + " HP! Vyblokováno " + (rawDMG - dmg).ToString() + " damage!");

            toulec1 = toulec1 - 1;
            postava1.inventar["Toulec"] = toulec1.ToString();
        }

        public static void UtokSpellem(Postava postava1, Postava postava2){
            double reduction= 0.0;
            int dmg = 0;
            int rawDMG = 0;
            int scroll = Int32.Parse(postava1.inventar["Scroll"].Substring(postava1.inventar["Scroll"].Length - 2, 1));

            rawDMG = 60 * scroll;
            reduction = 3.0 / ( 3.0 + Convert.ToDouble(postava2.armor));
            dmg = Convert.ToInt32(Convert.ToDouble(rawDMG) * reduction);
            postava2.DostanDmg(dmg);
            System.Console.WriteLine("     [*] " + postava1.jmeno + " [" + postava1.hp + "] hitnul spellem " + postava2.jmeno + " [" + postava2.hp + "] za " + dmg.ToString() + " HP! Vyblokováno " + (rawDMG - dmg).ToString() + " damage!");
        }

        public static void Spell(Postava postava1, Postava postava2){
            if(postava1.mana >= 70){
                UtokSpellem(postava1, postava2);
                postava1.mana -= 70;
            }
            if(postava1.mana >= 0){
                ManaRegen(postava1);
            }
        }

        public static bool Luk(Postava postava1, Postava postava2){
            if(postava1.inventar["Luk"] != "" && postava1.inventar["Toulec"] != "0" && postava2.inventar["Luk"] != "" && postava2.inventar["Toulec"] != "0"){
                UtokLukem(postava1, postava2);
                UtokLukem(postava2, postava1);
                return true;
            }
            else if(postava1.inventar["Luk"] != "" && postava1.inventar["Toulec"] != "0"){
                UtokLukem(postava1, postava2);
                return true;
            }
            else if(postava2.inventar["Luk"] != "" && postava2.inventar["Toulec"] != "0"){
                UtokLukem(postava2, postava1);
                return true;
            }
            return false;
        }

        public static void ManaRegen(Postava postava){
            postava.mana += 15;
        }

        public static void HealthRegen(List<Postava> list){
            foreach(Postava postava in list){
                postava.hp += 35;
                if(postava.hp > 100){
                    postava.hp = 100;
                }
            }
            System.Console.WriteLine("Všechny postavy byly vyléčeny o 35 HP!");
        }
        public static void SipyRegen(List<Postava> list){
            foreach(Postava postava in list){
                if(postava.inventar["Luk"] != ""){
                    postava.inventar["Toulec"] = "3";
                }
            }
            System.Console.WriteLine("Archerovi byly přidány 3 šípy!");
        }

        public static void MainMenu(List<Postava> lidiList, string stage, List<string> inventar){
            System.Console.Clear();
            System.Console.WriteLine("   ---> Menu <---   ");
            System.Console.WriteLine("1) Spustit stage " + stage);
            System.Console.WriteLine("2) Postavy");
            System.Console.WriteLine("3) Předměty (" + inventar.Count().ToString() + ")");
            System.Console.WriteLine("4) Šance na dropy");
            System.Console.WriteLine("5) Pravidla");
            System.Console.WriteLine();

            switch(Console.ReadLine()){
                case "1":
                case "+":
                    return;
                case "2":
                case "ě":
                    InventarMenu(lidiList, stage, inventar);
                    return;
                case "3":
                case "š":
                    HracuvInventar(lidiList, stage, inventar);
                    return;
                case "4":
                case "č":
                    ChancesMenu(lidiList, stage, inventar);
                    return;
                case "5":
                case "ř":
                    RulesMenu(lidiList, stage, inventar);
                    return;
                default:
                    MainMenu(lidiList, stage, inventar);
                    return;
            }
        }

        public static void InventarMenu(List<Postava> lidiList, string stage, List<string> inventar){
            System.Console.Clear();
            System.Console.WriteLine("   ---> Postavy <---   ");
            int i = 0;
            int key = 0;
            string input;
            foreach(Postava clovek in lidiList){
                i += 1;
                System.Console.WriteLine(i.ToString() + ") " + clovek.jmeno + " [" + clovek.hp + "]");
            }
            System.Console.WriteLine((i+1).ToString() + ") Zpět");
            System.Console.WriteLine();
            while(true){
                input = Console.ReadLine();
                if(input != ""){break;}
            }
            key = Parser(input);
            if(key == i+1){
                MainMenu(lidiList, stage, inventar);
            }else if(key == 0 || key > i+1){
                InventarMenu(lidiList, stage, inventar);
            }else{
                InventarONMenu(lidiList[key-1], lidiList, stage, inventar);
            }
            
        }

        public static void InventarONMenu(Postava clovek, List<Postava> lidiList, string stage, List<string> inventar){
            System.Console.Clear();
            Inventar(clovek);
            System.Console.WriteLine();
            System.Console.WriteLine("1) Zpět");
            switch(Console.ReadLine()){
                case "1":
                case "+":
                    InventarMenu(lidiList, stage, inventar);
                    return;
                default:
                    InventarONMenu(clovek, lidiList, stage, inventar);
                    return;
            }
        }
        public static void Inventar(Postava postava){
            if(postava.mana != -1){
                System.Console.WriteLine("   ---> " + postava.jmeno + " [" + postava.hp.ToString() +";" + postava.mana.ToString() + "] <---   ");
            }else{
                System.Console.WriteLine("   ---> " + postava.jmeno + " [" + postava.hp.ToString() +"] <---   ");
            }
            System.Console.WriteLine(postava.inventar["Přilba"]);
            System.Console.WriteLine(postava.inventar["Brnění"]);
            System.Console.WriteLine(postava.inventar["Chrániče"]);
            System.Console.WriteLine(postava.inventar["Zbraň"]);
            if(postava.inventar["Štít"] != ""){
                System.Console.WriteLine(postava.inventar["Štít"]);
            }
            if(postava.inventar["Scroll"] != ""){
                System.Console.WriteLine(postava.inventar["Scroll"]);
            }
            if(postava.inventar["Luk"] != ""){
                System.Console.WriteLine(postava.inventar["Luk"]);
            }
            if(postava.inventar["Luk"] != ""){
                System.Console.WriteLine("Toulec: " + postava.inventar["Toulec"]);
            }
        }

        

        public static void ChancesMenu(List<Postava> lidiList, string stage, List<string> inventar){
            System.Console.Clear();
            System.Console.WriteLine("   ---> Šance na dropy <---   ");
            System.Console.WriteLine();

            System.Console.WriteLine("1) Zpět");
            switch(Console.ReadLine()){
                case "1":
                case "+": 
                    MainMenu(lidiList, stage, inventar);
                    return;
                default:
                    ChancesMenu(lidiList, stage, inventar);
                    return;
            }
        }

        public static void RulesMenu(List<Postava> lidiList, string stage, List<string> inventar){
            System.Console.Clear();
            System.Console.WriteLine("   ---> Pravidla <---   ");
            System.Console.WriteLine();

            System.Console.WriteLine("1) Zpět");
            switch(Console.ReadLine()){
                case "1":
                case "+": 
                    MainMenu(lidiList, stage, inventar);
                    return;
                default:
                    RulesMenu(lidiList, stage, inventar);
                    return;
            }
        }

        public static void HracuvInventar(List<Postava> lidiList, string stage, List<string> inventar){
            System.Console.Clear();
            System.Console.WriteLine("   ---> Inventář <---   ");
            foreach(string item in inventar){
                System.Console.WriteLine("- " + item);
            }
            System.Console.WriteLine();

            System.Console.WriteLine("1) Zpět");
            switch(Console.ReadLine()){
                case "1":
                case "+": 
                    MainMenu(lidiList, stage, inventar);
                    return;
                default:
                    HracuvInventar(lidiList, stage, inventar);
                    return;
            }
        }

        public static void StageResult(List<Postava> list){
            System.Console.WriteLine();
            System.Console.WriteLine("   ---> Postavy <---   ");
            foreach(Postava postava in list){
                if(postava.mana != -1){
                    System.Console.WriteLine("- " + postava.jmeno + " [" + postava.hp.ToString() +";" + postava.mana.ToString() + "]");
                }else{
                    System.Console.WriteLine("- " + postava.jmeno + " [" + postava.hp.ToString() +"]");
                }
            }
            System.Console.WriteLine();
            System.Console.WriteLine("   ---> Dropy <---   ");
            System.Console.WriteLine("- OKO");
            System.Console.WriteLine();
        }

        public static List<Postava> KillCheck(List<Postava> list){
            foreach(Postava postava in list){
                if(postava.hp <= 0){
                    list.Remove(postava);
                    System.Console.WriteLine("                --->    " + postava.jmeno + " byl zabit!    <---    ");
                    break;
                }
            }
            return list;
        }
        public static bool WinCheck(List<Postava> list1, List<Postava> list2){
            int i = 0;
            int x = 0;
            KillCheck(list1);
            foreach(Postava postava in list1){
                x += 1;
                if(postava.hp <= 0){
                    i += 1;
                }
            }
            if(i == x) {return false;}
            x = 0;
            i = 0;
            KillCheck(list2);
            foreach(Postava postava in list2){
                x += 1;
                if(postava.hp <= 0){
                    i += 1;
                }
            }
            if(i == x) {return false;}
            return true;
        }

        public static bool LoseCheck(List<Postava> lidelist){
            if(lidelist.Count() == 0){
                return true;
            }
            return false;
        }

        public static List<Postava> SpawnEnemy(Kasarna kasarna, MonsterVojakBuilder monsterBuilder, int stage){
            List<Postava> monstraList = new List<Postava>();
            monstraList.Add(kasarna.VytvorWarrirora(monsterBuilder, "Monster Warrior", stage));
            monstraList.Add(kasarna.VytvorArchera(monsterBuilder, "Monster Archer", stage));
            monstraList.Add(kasarna.VytvorMaga(monsterBuilder, "Monster Mage", stage));
            return monstraList;
        }

        public static List<Postava> ChangePos(List<Postava> list){
            if(list[0].hp > 0){
                list.Add(list[0]);
                list.RemoveAt(0);
            }
            else{
                list.RemoveAt(0);
            }
            return list;
        }

        public static int Parser(string input){
            if(input == "+"){ return 1;}
            if(input == "ě"){ return 2;}
            if(input == "š"){ return 3;}
            if(input == "č"){ return 4;}
            if(input == "ř"){ return 5;}
            if(input == "ž"){ return 6;}
            if(input == "ý"){ return 7;}
            if(input == "á"){ return 8;}
            if(input == "í"){ return 9;}
            if(Int32.TryParse(input, out int intinput)){return intinput;}
            return 0;
        }

        public static (List<string>, List<List<string>>) Drop(List<List<string>> drops){
            List<string> dropped = new List<string>();
            string drop;
            int chance = Roll();
            int numberOfDrops = 1;
            if(chance >= 50){ numberOfDrops=2;}
            if(chance >= 75){ numberOfDrops=3;}
            if(chance >= 90){ numberOfDrops=4;}
            if(chance >= 95){ numberOfDrops=5;}
            System.Console.WriteLine(numberOfDrops);
            for(int i = 1; i <= numberOfDrops; i++){
                chance = Roll();
                if(chance >= 97){ drop=DropSelect(drops[3]); dropped.Add(drop); drops[3]=DropRemover(drops[3],drop); continue;}
                if(chance >= 90){ drop=DropSelect(drops[2]); dropped.Add(drop); drops[2]=DropRemover(drops[2],drop); continue;}
                if(chance >= 75){ drop=DropSelect(drops[1]); dropped.Add(drop); drops[1]=DropRemover(drops[1],drop); continue;}
                drop=DropSelect(drops[0]); dropped.Add(drop); drops[0]=DropRemover(drops[0],drop);
            }
            System.Console.WriteLine("DROPPED: ");
            foreach(string item in dropped){
                System.Console.WriteLine("- " + item);
            }
            System.Console.WriteLine();
            System.Console.WriteLine("T1 dropy: ");
            return (dropped, drops);

        }

        public static string DropSelect(List<string> drops){
            var random = new Random();
            int randomItem = random.Next(drops.Count);
            return drops[randomItem];
        }

        public static List<string> DropRemover(List<string> drops, string drop){
            if(drops[0].Substring(drops[0].Length - 1, 1) != "]"){
                System.Console.WriteLine("SUB " + drops[0].Substring(drops[0].Length - 1, 1));
                System.Console.WriteLine("Nulka " + drops[0]);
                System.Console.WriteLine("NOT REMOVED " + drop);
                return drops;
            }
            System.Console.WriteLine("REMOVED " + drop);
            drops.Remove(drop);
            return drops;
        }

        public static int Roll(){
            Random random = new Random();
            int chance = random.Next(0,100);
            return chance;
        }
    }

    interface IPostava {
        public string jmeno {get;}
        public int hp {get;set;}
        public int multiplier {get; set;}
        public int mindmg {get;set;}
        public int maxdmg {get;set;}
        public int armor {get;set;}
        public int mana {get;set;}
        public int dodge {get;set;}
        public Dictionary<string, string> inventar {get; set;}
        public void DostanDmg(int dmg);
        public int DmgCalc();
    }

    class Postava: IPostava {
        public string jmeno {get;set;} = "?";
        public int hp {get;set;} = 0;
        public int multiplier {get;set;} = 0;
        public int mindmg {get;set;} = 0;
        public int maxdmg {get;set;} = 0;
        public int armor {get;set;} = 0;
        public int mana {get;set;} = -1;
        public int dodge {get;set;} = 0;
        public Dictionary<string, string> inventar {get; set;}

        public Postava(int hp){
            this.hp = hp;
            this.jmeno = "?";
            this.inventar = new Dictionary<string, string>();
            this.inventar.Add("Přilba", "");
            this.inventar.Add("Brnění", "");
            this.inventar.Add("Chrániče", "");
            this.inventar.Add("Zbraň", "");
            this.inventar.Add("Štít", "");
            this.inventar.Add("Scroll", "");
            this.inventar.Add("Luk", "");
            this.inventar.Add("Toulec", "");
        }

        public void DostanDmg(int dmg){
            this.hp -= dmg;
        }

        public int DmgCalc(){
            Random random = new Random();
            int dmg = random.Next(this.mindmg, this.maxdmg);
            return dmg;
        }
    }

    interface IBuilder{
        void Reset();
        void JmenoSett(string jmeno);
        void Multiplier(int multiplier);
        void PridejPrilbu();
        void PridejBrneni();
        void PridejChranice();
        void PridejMec();
        void PridejStit();
        void PridejLuk();
        void PridejSipy();
        void PridejNaNSipy();
        void PridejScroll();
        Postava VytrenujVojaka();
    }

    class LidskyVojakBuilder: IBuilder {
        private Postava _lidskaPostava;

        public LidskyVojakBuilder(){
            this.Reset();
        }

        public void Reset(){
            this._lidskaPostava = new Postava(100);
        }

        public Postava VytrenujVojaka(){
            Postava vytvoreny_vojak = this._lidskaPostava;
            this.Reset();
            return vytvoreny_vojak;
        }
        public void JmenoSett(string jmeno){
            this._lidskaPostava.jmeno = jmeno;
        }

        public void Multiplier(int multiplier){
            this._lidskaPostava.multiplier = 1;
        }
        public void PridejPrilbu(){
            this._lidskaPostava.inventar["Přilba"] = "Přilba [Tier 1]";
            this._lidskaPostava.armor += 1;
        }
        public void PridejBrneni(){
            this._lidskaPostava.inventar["Brnění"] = "Brnění [Tier 1]";
            this._lidskaPostava.armor += 2;
        }
        public void PridejChranice(){
            this._lidskaPostava.inventar["Chrániče"] = "Chrániče [Tier 1]";
            this._lidskaPostava.armor += 1;
        }
        public void PridejMec(){
            this._lidskaPostava.inventar["Zbraň"] = "Meč [Tier 1]";
            this._lidskaPostava.mindmg = 10;
            this._lidskaPostava.maxdmg = 15;
        }
        public void PridejStit(){
            this._lidskaPostava.inventar["Štít"] = "Štít [Tier 1]";
            this._lidskaPostava.dodge = 30;
        }
        public void PridejLuk(){
            this._lidskaPostava.inventar["Luk"] = "Luk [Tier 1]";
        }
        public void PridejNaNSipy(){
            this._lidskaPostava.inventar["Toulec"] = "0";
        }
        public void PridejSipy(){
            this._lidskaPostava.inventar["Toulec"] = "3";
        }
        public void PridejScroll(){
            this._lidskaPostava.inventar["Scroll"] = "Scroll [Tier 1]";
            this._lidskaPostava.mana = 100;
        }
    }

    class MonsterVojakBuilder: IBuilder{
        private Postava _monsterpostava;

        public MonsterVojakBuilder(){
            this.Reset();
        }

        public void Reset(){
            this._monsterpostava = new Postava(100);
        }

        public Postava VytrenujVojaka(){
            Postava vytvoreny_vojak = this._monsterpostava;
            this.Reset();
            return vytvoreny_vojak;
        }
        
        public void JmenoSett(string jmeno){
            this._monsterpostava.jmeno = jmeno + " (Level: " + this._monsterpostava.multiplier.ToString() + ")";
        }
        public void Multiplier(int multiplier){
            this._monsterpostava.multiplier = multiplier;
        }
        public void PridejPrilbu(){
            this._monsterpostava.inventar["Přilba"] = "Monster přilba [" + this._monsterpostava.multiplier.ToString() + "]";
            this._monsterpostava.armor += (1 * this._monsterpostava.multiplier) / 2;
        }
        public void PridejBrneni(){
            this._monsterpostava.inventar["Brnění"] = "Monster brnění [" + this._monsterpostava.multiplier.ToString() + "]";
            this._monsterpostava.armor += (2 * this._monsterpostava.multiplier) / 2;
        }
        public void PridejChranice(){
            this._monsterpostava.inventar["Chrániče"] = "Monster chrániče [" + this._monsterpostava.multiplier.ToString() + "]";
            this._monsterpostava.armor += (1 * this._monsterpostava.multiplier) / 2;
        }
        public void PridejMec(){
            this._monsterpostava.inventar["Zbraň"] = "Monster meč [" + this._monsterpostava.multiplier.ToString() + "]";
            this._monsterpostava.mindmg = (10 * this._monsterpostava.multiplier) / 2;
            this._monsterpostava.maxdmg = (15 * this._monsterpostava.multiplier) / 2;
        }
        public void PridejStit(){
            this._monsterpostava.inventar["Štít"] = "Monster štít";
            this._monsterpostava.dodge = 20;
        }
        public void PridejLuk(){
            this._monsterpostava.inventar["Luk"] = "Monster luk [1]";
        }
        public void PridejNaNSipy(){
            this._monsterpostava.inventar["Toulec"] = "0";
        }
        public void PridejSipy(){
            this._monsterpostava.inventar["Toulec"] = "1";
        }
        public void PridejScroll(){
            this._monsterpostava.inventar["Scroll"] = "Monster scroll [1]";
            this._monsterpostava.mana = 50;
        }
    }
    
    class Kasarna{
        public Postava VytvorWarrirora(IBuilder builderRasy, string jmeno, int multiplier){
            builderRasy.Reset();
            builderRasy.Multiplier(multiplier);
            builderRasy.JmenoSett(jmeno);
            builderRasy.PridejPrilbu();
            builderRasy.PridejBrneni();
            builderRasy.PridejChranice();
            builderRasy.PridejMec();
            builderRasy.PridejStit();
            builderRasy.PridejNaNSipy();
            return builderRasy.VytrenujVojaka();
        }

        public Postava VytvorArchera(IBuilder builderRasy, string jmeno, int multiplier){
            builderRasy.Reset();
            builderRasy.Multiplier(multiplier);
            builderRasy.JmenoSett(jmeno);
            builderRasy.PridejPrilbu();
            builderRasy.PridejBrneni();
            builderRasy.PridejChranice();
            builderRasy.PridejMec();
            builderRasy.PridejLuk();
            builderRasy.PridejSipy();
            return builderRasy.VytrenujVojaka();
        }

        public Postava VytvorMaga(IBuilder builderRasy, string jmeno, int multiplier){
            builderRasy.Reset();
            builderRasy.Multiplier(multiplier);
            builderRasy.JmenoSett(jmeno);
            builderRasy.PridejPrilbu();
            builderRasy.PridejBrneni();
            builderRasy.PridejChranice();
            builderRasy.PridejMec();
            builderRasy.PridejScroll();
            builderRasy.PridejNaNSipy();
            return builderRasy.VytrenujVojaka();
        }
    }
}
