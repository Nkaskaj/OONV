namespace zapocet{
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
            this._monsterpostava.armor += 1 + this._monsterpostava.multiplier - 2;
        }
        public void PridejBrneni(){
            this._monsterpostava.inventar["Brnění"] = "Monster brnění [" + this._monsterpostava.multiplier.ToString() + "]";
            this._monsterpostava.armor += 2 + this._monsterpostava.multiplier - 2;
        }
        public void PridejChranice(){
            this._monsterpostava.inventar["Chrániče"] = "Monster chrániče [Tier " + this._monsterpostava.multiplier.ToString() + "]";
            this._monsterpostava.armor += 1 + this._monsterpostava.multiplier - 2;
        }
        public void PridejMec(){
            this._monsterpostava.inventar["Zbraň"] = "Monster meč [Tier " + this._monsterpostava.multiplier.ToString() + "]";
            this._monsterpostava.mindmg = 5 + (this._monsterpostava.multiplier - 2) * 2;
            this._monsterpostava.maxdmg = 10 + (this._monsterpostava.multiplier - 2) * 2;
        }
        public void PridejStit(){
            this._monsterpostava.inventar["Štít"] = "Monster štít";
            this._monsterpostava.dodge = 20;
        }
        public void PridejLuk(){
            this._monsterpostava.inventar["Luk"] = "Monster luk [Tier 1]";
        }
        public void PridejNaNSipy(){
            this._monsterpostava.inventar["Toulec"] = "0";
        }
        public void PridejSipy(){
            this._monsterpostava.inventar["Toulec"] = "1";
        }
        public void PridejScroll(){
            this._monsterpostava.inventar["Scroll"] = "Monster scroll [Tier 1]";
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

        public Postava VytvorCompaniona(IBuilder builderRasy, string jmeno, int multiplier){
            builderRasy.Reset();
            builderRasy.Multiplier(multiplier);
            builderRasy.JmenoSett(jmeno);
            builderRasy.PridejPrilbu();
            builderRasy.PridejBrneni();
            builderRasy.PridejChranice();
            builderRasy.PridejMec();
            builderRasy.PridejNaNSipy();
            return builderRasy.VytrenujVojaka();
        }
    }
}