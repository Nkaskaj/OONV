using System;
using System.Collections.Generic;   

namespace zapocet{

    partial class Program {         
        static void Main(string[] args)
        {
            bool run = true;
            bool lose = false;
            bool stageRun = true;
            int stage = 1;
            bool lukSkip = false;
            bool skip = false;
            bool skipAn = false;
            string input;

            List<string> dropped = new List<string>();
            List<string> drops1 = new List<string>() {"▲ Lektvar obnovy", "Přilba [Tier 2]", "Přilba [Tier 2]", "Přilba [Tier 2]", "Brnění [Tier 2]", "Brnění [Tier 2]", "Brnění [Tier 2]", "Chrániče [Tier 2]", "Chrániče [Tier 2]", "Chrániče [Tier 2]", "Meč [Tier 2]", "Meč [Tier 2]", "Meč [Tier 2]", "Štít [Tier 2]", "Luk [Tier 2]", "Scroll [Tier 2]"};
            List<string> drops2 = new List<string>() {"▲ Pomocník", "Přilba [Tier 3]", "Přilba [Tier 3]", "Přilba [Tier 3]", "Brnění [Tier 3]", "Brnění [Tier 3]", "Brnění [Tier 3]", "Chrániče [Tier 3]", "Chrániče [Tier 3]", "Chrániče [Tier 3]"};
            List<string> drops3 = new List<string>() {"▲ Kámen vzkříšení", "Meč [Tier 3]", "Meč [Tier 3]", "Meč [Tier 3]", "Štít [Tier 3]", "Luk [Tier 3]", "Scroll [Tier 3]"};
            List<string> drops4 = new List<string>() {"Štít [Legendary]", "Luk [Legendary]", "Scroll [Legendary]"};
            List<List<string>> drops = new List<List<string>>() {drops1, drops2, drops3, drops4};

            List<string> inventar = new List<string>();

            Kasarna kasarna = new Kasarna();

            LidskyVojakBuilder lidskyBuilder = new LidskyVojakBuilder();
            MonsterVojakBuilder monsterBuilder = new MonsterVojakBuilder();
            
            List<Postava> hrob = new List<Postava>();
            List<Postava> lideList = new List<Postava>();
            lideList.Add(kasarna.VytvorWarrirora(lidskyBuilder, "Lidský Warrior", stage));
            lideList.Add(kasarna.VytvorArchera(lidskyBuilder, "Lidský Archer", stage));
            lideList.Add(kasarna.VytvorMaga(lidskyBuilder, "Lidský Mage", stage));

            List<Postava> monstraList = new List<Postava>();
            monstraList = SpawnEnemy(kasarna, monsterBuilder, stage);
            
            Dictionary<dynamic, dynamic> data = new Dictionary<dynamic, dynamic>();
            data.Add("lidiList", lideList);
            data.Add("stage", stage);
            data.Add("inventar", inventar);
            data.Add("drops", drops);
            data.Add("hrob", hrob);

            while(run == true){
                MainMenu(data);
                Console.Clear();
                stageRun = true;
                skip = false;
                skipAn = false;
                while(stageRun == true){
                    lukSkip = false;

                    lukSkip = Luk(data["lidiList"][0], monstraList[0]);
                    
                    if(lukSkip == false){
                        Round(data["lidiList"][0], monstraList[0]);
                        stageRun = WinCheck(monstraList,data["lidiList"],data["hrob"]);
                        if(stageRun == false) {break;}
                        Spell(data["lidiList"][0], monstraList[0]);
                        stageRun = WinCheck(monstraList,data["lidiList"],data["hrob"]);

                        if(skipAn == false){System.Console.WriteLine(); System.Console.WriteLine("Stiskni [Enter] pro další souboj nebo 1 pro dokončení stage!"); skipAn=true;}
                        if(skip == false){input = Console.ReadLine(); if(input == "1" || input == "+"){skip = true;}}
                        
                        if(stageRun == false) {break;}
                       
                        Round(monstraList[0], data["lidiList"][0]);
                        stageRun = WinCheck(data["lidiList"],monstraList,data["hrob"]);
                        if(stageRun == false) {break;}
                        Spell(monstraList[0], data["lidiList"][0]);
                        stageRun = WinCheck(data["lidiList"],monstraList,data["hrob"]);
                        if(skip == false){input = Console.ReadLine(); if(input == "1" || input == "+"){skip = true;}}
                    }
                    stageRun = WinCheck(data["lidiList"],monstraList,data["hrob"]);
                    stageRun = WinCheck(monstraList,data["lidiList"],data["hrob"]);
                    
                    if(stageRun == false) {break;}

                    data["lidiList"] = ChangePos(data["lidiList"]);
                    monstraList = ChangePos(monstraList);
                    System.Console.WriteLine();
                    
                }
                System.Console.WriteLine();
                System.Console.WriteLine("                ---> Stage " + data["stage"].ToString() + " dohrána <---   ");
                Console.ReadLine();
                lose = LoseCheck(data["lidiList"]);
                if(lose){
                    run = false;
                }else{
                    data["stage"] += 1;
                    dropped.Clear();
                    (dropped, data) = Drop(data, false, dropped);
                    
                    StageResult(data["lidiList"], dropped);
                    Console.ReadLine();

                    monstraList = SpawnEnemy(kasarna, monsterBuilder, data["stage"]);
                }
                
                if(data["stage"] == 10) {run = false;}
            }
            if(lose){
                Console.Clear();
                System.Console.WriteLine("                --->    Prohrál si!    <---");
                System.Console.WriteLine("                --->    Stage: " + data["stage"] + "!    <---");
                Console.ReadLine();
            }else{
                Console.Clear();
                System.Console.WriteLine("                --->    Vyhrál si!    <---");
                System.Console.WriteLine("                --->    Stage: 10!    <---");
                Console.ReadLine();
            }
        }

        public static void Game(Dictionary<dynamic, dynamic> data, bool run, bool stageRun, bool lukSkip, bool lose, int stage, List<Postava> monstraList, MonsterVojakBuilder monsterBuilder, Kasarna kasarna, List<string> dropped){
            
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
            int luk = 0;
            if(postava1.inventar["Luk"].Substring(postava1.inventar["Luk"].Length - 2, 1) == "y"){ luk = 4;}
            else{luk=Int32.Parse(postava1.inventar["Luk"].Substring(postava1.inventar["Luk"].Length - 2, 1));}
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
            int scroll = 0;
            if(postava1.inventar["Scroll"].Substring(postava1.inventar["Scroll"].Length - 2, 1) == "y"){ scroll = 4;}
            else{scroll=Int32.Parse(postava1.inventar["Scroll"].Substring(postava1.inventar["Scroll"].Length - 2, 1));}

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
            bool added = false;
            foreach(Postava postava in list){
                if(postava.inventar["Luk"] != ""){
                    added = true;
                    postava.inventar["Toulec"] = "3";
                }
            }
            if(added == true){
                System.Console.WriteLine("Archerovi byly přidány 3 šípy!");
            }
        }

        public static void FullRegen(List<Postava> list){
            foreach(Postava postava in list){
                postava.hp = 100;
                if(postava.mana != -1){
                    postava.mana = 100;
                }
            }
            Console.Clear();
            System.Console.WriteLine("Všechny postavy byly plně vyléčeny a mana byla doplněna!");
            Console.ReadLine();
        }

        public static void Companion(List<Postava> list){
            Kasarna kasarna = new Kasarna();
            LidskyVojakBuilder lidskyBuilder = new LidskyVojakBuilder();
            list.Add(kasarna.VytvorCompaniona(lidskyBuilder, "Pomocník", 1));
            Console.Clear();
            System.Console.WriteLine("Pomocník se přidal do tvé bojové skupiny!");
            Console.ReadLine();
        }

        public static void Revive(List<Postava> list, Postava postava){
            postava.hp = 100;
            if(postava.mana != -1){
                postava.mana = 100;
            }
            list.Add(postava);
            Console.Clear();
            System.Console.WriteLine("Postava " + postava.jmeno + " byla vzkříšena!");
            Console.ReadLine();
        }

        public static void MainMenu(Dictionary<dynamic, dynamic> data){
            System.Console.Clear();
            System.Console.WriteLine("   ---> Menu <---   ");
            System.Console.WriteLine("1) Spustit stage " + data["stage"]);
            System.Console.WriteLine("2) Postavy");
            System.Console.WriteLine("3) Předměty (" + data["inventar"].Count.ToString() + ")");
            System.Console.WriteLine("4) Šance na dropy");
            System.Console.WriteLine("5) Pravidla");
            System.Console.WriteLine();

            switch(Console.ReadLine()){
                case "1":
                case "+":
                    return;
                case "2":
                case "ě":
                    InventarMenu(data);
                    return;
                case "3":
                case "š":
                    HracuvInventar(data);
                    return;
                case "4":
                case "č":
                    ChancesMenu(data);
                    return;
                case "5":
                case "ř":
                    RulesMenu(data);
                    return;
                case "giveall":
                    List<string> dropped = new List<string>();
                    (dropped, data) = Drop(data, true, dropped);
                    MainMenu(data);
                    return;
                default:
                    MainMenu(data);
                    return;
            }
        }

        public static void InventarMenu(Dictionary<dynamic, dynamic> data){
            System.Console.Clear();
            System.Console.WriteLine("   ---> Postavy <---   ");
            int i = 0;
            int key = 0;
            string input;
            foreach(Postava clovek in data["lidiList"]){
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
                MainMenu(data);
                return;
            }else if(key == 0 || key > i+1){
                InventarMenu(data);
                return;
            }else{
                InventarONMenu(data["lidiList"][key-1], data);
                return;
            }
        }

        public static void InventarONMenu(Postava clovek, Dictionary<dynamic, dynamic> data){
            System.Console.Clear();
            Inventar(clovek);
            System.Console.WriteLine();
            System.Console.WriteLine("1) Zpět");
            switch(Console.ReadLine()){
                case "1":
                case "+":
                    InventarMenu(data);
                    return;
                default:
                    InventarONMenu(clovek, data);
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
            System.Console.WriteLine(postava.inventar["Zbraň"] + " (" + postava.mindmg.ToString() + "-" + postava.maxdmg.ToString() + ")");
            if(postava.inventar["Štít"] != ""){
                System.Console.WriteLine(postava.inventar["Štít"] + " (" + postava.dodge.ToString() + " %)");
            }
            if(postava.inventar["Scroll"] != ""){
                if(postava.inventar["Scroll"].Substring(postava.inventar["Scroll"].Length - 2, 1) == "y"){ System.Console.WriteLine(postava.inventar["Scroll"] + " (240 damage)");}else{System.Console.WriteLine(postava.inventar["Scroll"] + " (" + (60*Int32.Parse(postava.inventar["Scroll"].Substring(postava.inventar["Scroll"].Length - 2, 1))).ToString() + " damage)");};
            }
            if(postava.inventar["Luk"] != ""){
                if(postava.inventar["Luk"].Substring(postava.inventar["Luk"].Length - 2, 1) == "y"){ System.Console.WriteLine(postava.inventar["Luk"] + " (120 damage)");}else{System.Console.WriteLine(postava.inventar["Luk"] + " (" + (30*Int32.Parse(postava.inventar["Luk"].Substring(postava.inventar["Luk"].Length - 2, 1))).ToString() + " damage)");};
            }
            if(postava.inventar["Luk"] != ""){
                System.Console.WriteLine("Toulec: " + postava.inventar["Toulec"]);
            }
            System.Console.WriteLine("Armor: " + postava.armor.ToString());
        }

        public static void ChancesMenu(Dictionary<dynamic, dynamic> data){
            System.Console.Clear();
            System.Console.WriteLine("   ---> Šance na N dropů <---   ");
            System.Console.WriteLine("- 1 drop  100 %");
            System.Console.WriteLine("- 2 dropy 50 %");
            System.Console.WriteLine("- 3 dropy 25 %");
            System.Console.WriteLine("- 4 dropy 10 %");
            System.Console.WriteLine("- 5 dropů 5 %");
            System.Console.WriteLine();
            System.Console.WriteLine("   ---> Dostupné Tier 1 (100 %) dropy <---   ");
            foreach(string drop in data["drops"][0]){
                System.Console.WriteLine("- " + drop);
            }
            System.Console.WriteLine();
            System.Console.WriteLine("   ---> Dostupné Tier 2 (25 %) dropy <---   ");
            foreach(string drop in data["drops"][1]){
                System.Console.WriteLine("- " + drop);
            }
            System.Console.WriteLine();
            System.Console.WriteLine("   ---> Dostupné Tier 3 (10 %) dropy <---   ");
            foreach(string drop in data["drops"][2]){
                System.Console.WriteLine("- " + drop);
            }
            System.Console.WriteLine();
            System.Console.WriteLine("   ---> Dostupné Legendary (3 %) dropy <---   ");
            foreach(string drop in data["drops"][3]){
                System.Console.WriteLine("- " + drop);
            }
            if(data["drops"][3].Count == 0){System.Console.WriteLine("- Žádné dropy nejsou dostupné");}
            System.Console.WriteLine();
            System.Console.WriteLine("1) Zpět");
            switch(Console.ReadLine()){
                case "1":
                case "+": 
                    MainMenu(data);
                    return;
                default:
                    ChancesMenu(data);
                    return;
            }
        }

        public static void RulesMenu(Dictionary<dynamic, dynamic> data){
            System.Console.Clear();
            System.Console.WriteLine("                ---> Pravidla <---   ");
            System.Console.WriteLine("- Hra má dohromady 10 stagí a jedná se o RNG based tahovou hru");
            System.Console.WriteLine("- V menu se můžeš pohybovat i pomocí české klávesnice (1=+,2=ě,3=š,...)");
            System.Console.WriteLine("- Nejdříve útočíš do nepřátelské postavy, poté ona útočí do tebe");
            System.Console.WriteLine("- Tvá bojová skupina se skládá z Warriora, Mága a Archera a každá postava má unikátní zbraň");
            System.Console.WriteLine("- Warrior má štít, který mu přidává šanci na vyhnutí se útoku");
            System.Console.WriteLine("- Mág má scroll, kterým může za 70 many způsobit dodatečné poškození");
            System.Console.WriteLine("- Archer má luk, ze kterého střílí z dálky, a tak do něj nemůže nikdo zblízka zaútočit");
            System.Console.WriteLine("- Po skončení kola se tvoji hrdinové vyléčí o 35 HP, Mág získá 100 many a Archer získá 3 šípy");
            System.Console.WriteLine("- Po skončení kola máš šanci na 1-5 dropů různých rarit (Tier 1-3 nebo Legendary)");
            System.Console.WriteLine("- Vybavitelnými předměty můžeš vybavit každého ze své skupiny");
            System.Console.WriteLine("- Po vybavení nemůžeš předmět z postavy sundat (lze ho jen vyměnit)");
            System.Console.WriteLine("- Aktivovatelné předměty můžeš použít mezi stagí");
            System.Console.WriteLine("- Vybavitelné předměty mají omezený drop (pro více info koukni do tabu Šance na dropy)");
            System.Console.WriteLine("");
            System.Console.WriteLine("- Pro získání všech předmětů napiš příkaz 'giveall' v Menu");
            System.Console.WriteLine("");

            System.Console.WriteLine("1) Zpět");
            switch(Console.ReadLine()){
                case "1":
                case "+": 
                    MainMenu(data);
                    return;
                default:
                    RulesMenu(data);
                    return;
            }
        }

        public static void HracuvInventar(Dictionary<dynamic, dynamic> data){
            System.Console.Clear();
            System.Console.WriteLine("   ---> Inventář <---   ");
            System.Console.WriteLine();
            List<string> sortedInventar = new List<string>();
            sortedInventar.Clear();
            bool active = true;
            bool passive = true;
            int i = 0;
            int key = 0;
            string input;
            foreach(string item in data["inventar"]){
                if(item.Substring(0,1) == "▲"){
                    if(active){System.Console.WriteLine(" -> Aktivovatelné předměty <-  ");}
                    active = false;
                    i++;
                    sortedInventar.Add(item);
                    System.Console.WriteLine(i.ToString() +") " + item);
                }
            }
            foreach(string item in data["inventar"]){
                if(item.Substring(0,1) != "▲"){
                    if(passive){if(active == false){System.Console.WriteLine();}System.Console.WriteLine(" -> Vybavitelné předměty <-  ");}
                    passive = false;
                    i++;
                    sortedInventar.Add(item);
                    System.Console.WriteLine(i.ToString() +") " + item);
                }
            }
            System.Console.WriteLine();
            System.Console.WriteLine((i+1).ToString() + ") Zpět");
            System.Console.WriteLine();
            while(true){
                input = Console.ReadLine();
                if(input != ""){break;}
            }
            key = Parser(input);
            if(key == i + 1){
                MainMenu(data);
                return;
            }else if(key == 0 || key > i + 1){
                HracuvInventar(data);
                return;
            }else{
                UseItem(sortedInventar[key-1], data);
                return;
            }
            return;
        }

        public static void UseItem(string item, Dictionary<dynamic, dynamic> data){
            bool used = false;
            if(item.Substring(0,1) == "▲"){
                if(item == "▲ Lektvar obnovy"){
                    FullRegen(data["lidiList"]);
                    data["inventar"].Remove(item);
                    HracuvInventar(data);
                    return;
                }
                else if(item == "▲ Pomocník"){
                    Companion(data["lidiList"]);
                    data["inventar"].Remove(item);
                    HracuvInventar(data);
                    return;
                }
                else if(item == "▲ Kámen vzkříšení"){
                    Console.Clear();
                    int i = 0;
                    int key = 0;
                    string input;
                    System.Console.WriteLine("   ---> Vyberte postavu na kterou chcete vzkřísit <---   ");
                    foreach(Postava clovek in data["hrob"]){
                        i++;
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
                        HracuvInventar(data);
                        return;
                    }else if(key == 0 || key > i+1){
                        UseItem(item, data);
                        data["inventar"].Remove(item);
                        return;
                    }else{
                        Revive(data["lidiList"], data["hrob"][key-1]);
                        HracuvInventar(data);
                        return;
                    }
                }
            }else{
                if(item.Substring(0,3) == "Luk" || item.Substring(0,3) == "Scr" || item.Substring(0,3) == "Ští"){
                    Postava selectedClovek = data["lidiList"][data["lidiList"].IndexOf(ClassFinder(data["lidiList"], item))];
                    string olditem = TypeFinder(selectedClovek, item, true);
                    AcceptChange(data,selectedClovek,item,olditem);
                    return;
                }else{
                    SelectPostava(data, item);
                    return;
                }
            }
            return;
        }

        public static void SelectPostava(Dictionary<dynamic, dynamic> data, string item){
            Console.Clear();
            int i = 0;
            int key = 0;
            string input;
            string olditem;
            List<string> olditems = new List<string>();
            System.Console.WriteLine("   ---> Vyberte postavu na kterou chcete nasadit " + item + " <---   ");
            foreach(Postava clovek in data["lidiList"]){
                i++;
                olditem = TypeFinder(clovek, item, true);
                olditems.Add(olditem);
                System.Console.WriteLine(i.ToString() + ") " + clovek.jmeno + " (" + olditem + ")" );
            }
            System.Console.WriteLine((i+1).ToString() + ") Zpět");
            System.Console.WriteLine();
            while(true){
                input = Console.ReadLine();
                if(input != ""){break;}
            }
            key = Parser(input);
            if(key == i+1){
                HracuvInventar(data);
                return;
            }else if(key == 0 || key > i+1){
                SelectPostava(data, item);
                return;
            }else{
                AcceptChange(data, data["lidiList"][key-1], item, olditems[key-1]);
                return;
            }
        }
        
        public static void AcceptChange(Dictionary<dynamic, dynamic> data, Postava clovek, string item, string olditem){
            Console.Clear();
            System.Console.WriteLine("1) Potvrdit");
            System.Console.WriteLine("2) Zpět");
            System.Console.WriteLine();
            System.Console.WriteLine("Opravdu chcete vyměnit " + olditem + " za " + item + " na postavě " + clovek.jmeno + "?");
            switch(Console.ReadLine()){
                case "1":
                case "+":
                    string type = TypeFinder(clovek,item,false);
                    data["lidiList"][data["lidiList"].IndexOf(clovek)].inventar[type] = item;
                    data["inventar"].Remove(item);
                    CalcStat(clovek, item, type, olditem);
                    HracuvInventar(data);
                    return;
                case "2":
                case "ě":
                    HracuvInventar(data);
                    return;
                default:
                    AcceptChange(data, clovek, item, olditem);
                    return;
            }
        }

        public static Postava ClassFinder(List<Postava> list, string item){
            foreach(Postava clovek in list){
                foreach(KeyValuePair<string,string> kvp in clovek.inventar){
                    if(kvp.Value.Length > 2){
                        if(kvp.Value.Substring(0,3) == item.Substring(0,3)){
                            return clovek;
                        }
                    }
                }
            }
            return list[0];
        }

        public static string TypeFinder(Postava clovek, string item, bool value){
            foreach(KeyValuePair<string, string> olditem in clovek.inventar){
                if(olditem.Value.Length > 2){
                    if(olditem.Value.Substring(0,3) == item.Substring(0,3)){
                        if(value){
                            return olditem.Value;
                        }
                    return olditem.Key;
                    }
                }
            }
            return "???";
        }

        public static void CalcStat(Postava clovek, string item, string type, string olditem){
            if(item.Substring(0,3) == "Luk" || item.Substring(0,3) == "Scr"){ return;}
            if(item.Substring(0,3) == "Ští"){
                if(item.Substring(item.Length - 2, 1) == "y"){ clovek.dodge = 60; return;}
                clovek.dodge = ((Int32.Parse(item.Substring(item.Length - 2, 1)) - 1) * 10) + 30; return;
            }
            if(item.Substring(0,3) == "Meč"){
                clovek.mindmg = (Int32.Parse(item.Substring(item.Length - 2, 1)) * 10);
                clovek.maxdmg = (Int32.Parse(item.Substring(item.Length - 2, 1)) * 15);
                return;
            }
            if(item.Substring(0,3) == "Brn"){
                clovek.armor -= Int32.Parse(olditem.Substring(olditem.Length - 2, 1)) * 2;
                clovek.armor += Int32.Parse(item.Substring(item.Length - 2, 1)) * 2;
                return;
            }
            else{
                clovek.armor -= Int32.Parse(olditem.Substring(olditem.Length - 2, 1));
                clovek.armor += Int32.Parse(item.Substring(item.Length - 2, 1));
                return;
            }
        }

        public static void StageResult(List<Postava> list, List<string> drops){
            Console.Clear();
            System.Console.WriteLine("                ---> Výsledky <---   ");
            HealthRegen(list);
            SipyRegen(list);
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
            foreach(string drop in drops){
                System.Console.WriteLine("- " + drop);
            }
            System.Console.WriteLine();
        }

        public static List<Postava> KillCheck(List<Postava> list, List<Postava> hrob){
            foreach(Postava postava in list){
                if(postava.hp <= 0){
                    if(postava.jmeno.Substring(0,3) == "Lid"){
                        hrob.Add(postava);
                    }
                    list.Remove(postava);
                    System.Console.WriteLine("                --->    " + postava.jmeno + " byl zabit!    <---    ");
                    break;
                }
            }
            return list;
        }
        public static bool WinCheck(List<Postava> list1, List<Postava> list2, List<Postava> hrob){
            int i = 0;
            int x = 0;
            KillCheck(list1, hrob);
            foreach(Postava postava in list1){
                x += 1;
                if(postava.hp <= 0){
                    i += 1;
                }
            }
            if(i == x) {return false;}
            x = 0;
            i = 0;
            KillCheck(list2, hrob);
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
            if(Int32.TryParse(input, out int intinput)){ return intinput;}
            int rounds = 1;
            string jednotky = "";
            string desitky = "";
            string input2 = "";
            if(input.Length == 2){
                rounds = 2;
                input2 = input.Substring(1,1);
                input = input.Substring(0,1);
            }
            for(int i = 0; i < rounds; i++){
                if(input == "+"){ if(jednotky.Length > 0){desitky="1";}else{jednotky="1";};}
                if(input == "ě"){ if(jednotky.Length > 0){desitky="2";}else{jednotky="2";};}
                if(input == "š"){ if(jednotky.Length > 0){desitky="3";}else{jednotky="3";};}
                if(input == "č"){ if(jednotky.Length > 0){desitky="4";}else{jednotky="4";};}
                if(input == "ř"){ if(jednotky.Length > 0){desitky="5";}else{jednotky="5";};}
                if(input == "ž"){ if(jednotky.Length > 0){desitky="6";}else{jednotky="6";};}
                if(input == "ý"){ if(jednotky.Length > 0){desitky="7";}else{jednotky="7";};}
                if(input == "á"){ if(jednotky.Length > 0){desitky="8";}else{jednotky="8";};}
                if(input == "í"){ if(jednotky.Length > 0){desitky="9";}else{jednotky="9";};}
                if(input == "é"){ if(jednotky.Length > 0){desitky="0";}else{jednotky="0";};}
                if(i==rounds-1 && jednotky != ""){ return Int32.Parse(jednotky+desitky);}
                if(rounds==2){input=input2;}
            }
            return 0;
        }

        public static (List<string>, Dictionary<dynamic, dynamic>) Drop(Dictionary<dynamic, dynamic> data, bool giveall, List<string> dropped){
            string drop;
            int chance = Roll();
            int numberOfDrops = 1;
            if(chance >= 50){ numberOfDrops=2;}
            if(chance >= 75){ numberOfDrops=3;}
            if(chance >= 90){ numberOfDrops=4;}
            if(chance >= 95){ numberOfDrops=5;}
            if(giveall == true){numberOfDrops=40;}
            for(int i = 1; i <= numberOfDrops; i++){
                chance = Roll();
                if(data["drops"][0].Count == 0){ chance = 75;}
                if(data["drops"][1].Count == 0){ chance = 90;}
                if(data["drops"][2].Count == 0){ chance = 97;}
                if(data["drops"][3].Count == 0 && data["drops"][2].Count == 0 && data["drops"][1].Count == 0 && data["drops"][0].Count == 0){ chance = 101;}
                if(chance == 101){break;}
                if(data["drops"][3].Count != 0){if(chance >= 97){ drop=DropSelect(data["drops"][3]); data["inventar"].Add(drop); dropped.Add(drop); data["drops"][3].Remove(drop); continue;}}
                if(data["drops"][2].Count != 0){if(chance >= 90){ drop=DropSelect(data["drops"][2]); data["inventar"].Add(drop); dropped.Add(drop); data["drops"][2].Remove(drop); continue;}}
                if(data["drops"][1].Count != 0){if(chance >= 75){ drop=DropSelect(data["drops"][1]); data["inventar"].Add(drop); dropped.Add(drop); data["drops"][1].Remove(drop); continue;}}
                if(data["drops"][0].Count != 0){drop=DropSelect(data["drops"][0]); data["inventar"].Add(drop); dropped.Add(drop); data["drops"][0].Remove(drop);}
            }
            data["drops"] = DropReplenish(data["drops"]);
            return (dropped, data);
        }

        public static string DropSelect(List<string> drops){
            var random = new Random();
            int randomItem = random.Next(drops.Count);
            return drops[randomItem];
        }

        public static List<List<string>> DropReplenish(List<List<string>> drops){
            int i = 0;
            foreach(List<string> drop in drops){
                i++;
                if(i<4){
                    if(drop.Count == 0){
                        if(i==1){drop.Insert(0, "▲ Lektvar obnovy");}
                        if(i==2){drop.Insert(0, "▲ Pomocník");}
                        if(i==3){drop.Insert(0, "▲ Kámen vzkříšení");}
                    }
                    else if(drop[0].Substring(0, 1) != "▲"){
                        if(i==1){drop.Insert(0, "▲ Lektvar obnovy");}
                        if(i==2){drop.Insert(0, "▲ Pomocník");}
                        if(i==3){drop.Insert(0, "▲ Kámen vzkříšení");}
                    }
                }
            }
            return drops;
        }

        public static int Roll(){
            Random random = new Random();
            int chance = random.Next(0,100);
            return chance;
        }
    }
}
