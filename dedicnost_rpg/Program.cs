using System;
using System.Collections.Generic;   

namespace dedicnost_rpg{

    class Program {         
        static void Main(string[] args)
        {
            bool run = true;

            Warrior valecnik = new Warrior("Valečník");
            Mage mage = new Mage("Mág");
            Archer lukostrelec = new Archer("Lukostřelec");

            Drak drak = new Drak("Drak");
            Ovce ovecka = new Ovce("Ovce");
            Skvor skvor = new Skvor("Škvor");

            List<Postava> lideList = new List<Postava>();
            List<Postava> monsterList = new List<Postava>();
            List<Postava> stunnedList = new List<Postava>();

            lideList.Add(valecnik);
            lideList.Add(mage);
            lideList.Add(lukostrelec);

            monsterList.Add(drak);
            monsterList.Add(ovecka);
            monsterList.Add(skvor);

            while(run == true){
                System.Console.WriteLine("Nyní bojuje " + lideList[0].jmeno + " [" + lideList[0].hp + "] s " + monsterList[0].jmeno + " [" + monsterList[0].hp + "]!");
                
                Round(lideList[0], monsterList[0], stunnedList);
                run = WinCheck(valecnik, mage, lukostrelec, drak, ovecka, skvor);
                KillCheck(lideList);
                KillCheck(monsterList);
                
                if(run == false){break;}

                Round(monsterList[0], lideList[0], stunnedList);
                run = WinCheck(valecnik, mage, lukostrelec, drak, ovecka, skvor);

                lideList = ChangePos(lideList);
                monsterList = ChangePos(monsterList);
                
                System.Console.WriteLine("");
                
            }
            if(valecnik.hp > 0 || mage.hp > 0 || lukostrelec.hp > 0){
                System.Console.WriteLine("Vyhrávají lidé!");
            }
            else{
                System.Console.WriteLine("Vyhrávají monstra!");
            }
        }

        public static void Round(Postava postava1, Postava postava2, List<Postava> stunnedList){
            if(stunnedList.Contains(postava1)){
                    Stun(postava1, true, stunnedList);
                    System.Console.WriteLine(postava1.jmeno + " [" + postava1.hp + "] nebojuje, protože je stunnutý!");
            }
            else{
                bool dodged = false;
                if(postava2.dodge != 0){
                    if(postava2.dodge >= Roll()){
                        dodged = true;
                    }
                }
                if(dodged == false){
                    Utok(postava1, postava2);
                    if(postava1.stun != 0){
                        if(postava1.stun >= Roll() && postava2.hp > 0){
                            Stun(postava2, false, stunnedList);
                            System.Console.WriteLine(postava2.jmeno + " byl stunnutej!");
                        }
                    }
                    if(postava1.mana >= 70 && postava2.hp > 0){
                        Spell(postava1, postava2);
                    }
                }
                else{
                    System.Console.WriteLine(postava2.jmeno + " dodgnul útok!");
                }
            }

            HealthRegen(postava1);
            if(postava1.mana >= 0){ManaRegen(postava1);}
        }

        public static void Utok(Postava postava1, Postava postava2){
            int dmg = 0;
            
            dmg = postava1.DmgCalc();
            dmg -= postava2.armor;
            if(dmg<0){
                dmg = 0;
            }
            postava2.DostanDmg(dmg);
            
            System.Console.Write(postava1.jmeno + " [" + postava1.hp + "] hitnul " + postava2.jmeno + " [" + postava2.hp + "] za " + dmg.ToString() + " HP!");
            if(postava2.armor > 0){
                System.Console.WriteLine(" Vyblokováno " + postava2.armor + " damage!");
            }
            else{
                System.Console.WriteLine("");
            }
        }

        public static void Stun(Postava postava, bool stunned, List<Postava> stunnedList){
            if(stunned == false){
                stunnedList.Add(postava);
            }
            else{
                stunnedList.Remove(postava);
            }
        }

        public static void Spell(Postava postava1, Postava postava2){
            postava1.mana -= 70;
            int dmg = postava1.DmgCalc() * 3;
            postava2.hp -= dmg;
            System.Console.WriteLine(postava1.jmeno + " hitnul spellem " + postava2.jmeno + " za " + dmg.ToString() + " damage!");
        }

        public static void ManaRegen(Postava postava){
            postava.mana += 20;
        }

        public static void HealthRegen(Postava postava){
            postava.hp += 1;
        }

        public static List<Postava> KillCheck(List<Postava> list){
            foreach(Postava postava in list){
                if(postava.hp <= 0){
                    list.Remove(postava);
                    break;
                }
            }
            return list;
        }
        public static bool WinCheck(Postava valecnik, Postava mage, Postava lukostrelec, Postava drak, Postava ovecka, Postava skvor){
            if(valecnik.hp <= 0 && mage.hp <= 0 && lukostrelec.hp <= 0 || drak.hp <= 0 && ovecka.hp <= 0 && skvor.hp <= 0){
            return false;        
            }
            else{
                return true;
            }
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

        public static int Roll(){
            Random random = new Random();
            int chance = random.Next(0,100);
            return chance;
        }
    }

    interface IPostava {
        public string jmeno {get;}
        public int hp {get;set;}
        public int mindmg {get;set;}
        public int maxdmg {get;set;}
        public int armor {get;set;}
        public int stun {get;set;}
        public int mana {get;set;}
        public int dodge {get;set;}
        public void DostanDmg(int dmg);
        public int DmgCalc();
    }

    class Postava : IPostava {
        public string jmeno {get;} = "?";
        public int hp {get;set;} = 0;
        public int mindmg {get;set;} = 0;
        public int maxdmg {get;set;} = 0;
        public int armor {get;set;} = 0;
        public int stun {get;set;} = 0;
        public int mana {get;set;} = -1;
        public int dodge {get;set;} = 0;
        public Postava(string jmeno){
            this.jmeno = jmeno;
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

    class Lide : Postava, IPostava {

        public Lide(string jmeno):base(jmeno){

        }
    }
    class Warrior : Lide {
        
        public Warrior(string jmeno):base(jmeno){
            this.hp = 40;
            this.mindmg = 20;
            this.maxdmg = 30;
            this.armor = 15;
        }
    }
    class Mage : Lide {
        public Mage(string jmeno):base(jmeno){
            this.hp = 50;
            this.mindmg = 10;
            this.maxdmg = 15;
            this.mana = 100;
        }
    }
    class Archer : Lide {
        public Archer(string jmeno):base(jmeno){
            this.hp = 100;
            this.mindmg = 15;
            this.maxdmg = 25;
            this.dodge = 50;
        }
    }

    class Monster : Postava, IPostava {
        public Monster(string jmeno):base(jmeno){

        }

    }
    class Drak : Monster {
        
        public Drak(string jmeno):base(jmeno){
            this.hp = 50;
            this.mindmg = 25;
            this.maxdmg = 60;
            this.armor = 5;
        }
    }
    class Ovce : Monster {
        public Ovce(string jmeno):base(jmeno){
            this.hp = 200;
            this.mindmg = 0;
            this.maxdmg = 0;
        }
    }
    class Skvor : Monster {
        public Skvor(string jmeno):base(jmeno){
            this.hp = 1000;
            this.mindmg = 15;
            this.maxdmg = 20;
            this.stun = 50;
        }
    }
}
