using System;
using System.Collections.Generic;
using System.ComponentModel;


//private string _name;
//public string Name
//{
//    get { return _name; }
//    set { _name = value; }
//}

namespace oopCreateObject
{
    public enum Lesion
    {
        None = 0,
        Mid = 50,
        Death = 100
    };

    [DisplayName("BioSystem")]
    [Serializable]
    public class BiologicalSystem
    {
        public string NameOwner { get; set; }
        public Lesion Lesion { get; set; }
        public int Days { get; set; }

        public BiologicalSystem()
        {
            NameOwner = "Hospital";
            Lesion = Lesion.None;
            Days = 0;
        }

        public string GetDays()
        {
            return Days.ToString();
        }

        public override string ToString()
        {
            return NameOwner;
        }
    }

    [DisplayName("Gastrointestinal System")]
    [Serializable]
    public class GastrointestinalSystem : BiologicalSystem
    {
        public int Pepsin { get; set; }
        public int Gelatin { get; set; }

        public GastrointestinalSystem()
        {
            Pepsin = 15;
            Gelatin = 4;
        }
    }

    // печень
    [DisplayName("Liver")]
    [Serializable]
    public class Liver : GastrointestinalSystem
    {
        public int Fats { get; set; }
        public int Proteins { get; set; }
        public int Carbohydr { get; set; }

        public Liver()
        {
            Fats = 55;
            Proteins = 111;
            Carbohydr = 12;
        }

        //public override string ToString()
        //{
        //    return NameOwner.ToString();
        //}
    }

    [DisplayName("Respiratory System")]
    [Serializable]
    public class RespiratorySystem : BiologicalSystem
    {
        public bool PassiveResp { get; set; }
        
        public RespiratorySystem()
        {
        }
    }

    [DisplayName("Nose")]
    [Serializable]
    public class Nose : RespiratorySystem
    {
        public int Pause { get; set; }
        public string Quality { get; set; }

        public Nose()
        {
            Pause = 12;
            Quality = "very";
        }

        //public override string ToString()
        //{
        //    return NameOwner.ToString();
        //}
    }

    [DisplayName("Lungs")]
    [Serializable]
    public class Lungs : RespiratorySystem
    {
        public bool Breath { get; set; }
        public int Inhale { get; set; }
        [Description("Aggregation")]
        public Nose Nose { get; set; }

        public Lungs()
        {
            Inhale = 0;
        }
    }

    [DisplayName("Immune System")]
    [Serializable]
    public class ImmuneSystem : BiologicalSystem
    {
        public int CapasityLimfa { get; set; }

        public ImmuneSystem()
        {
            CapasityLimfa = 123;
        }
    }

    //селезенка
    [DisplayName("Spleen")]
    [Serializable]
    public class Spleen : ImmuneSystem
    {
        public int CapasityBlood { get; set; }
        public bool BloodFiltration { get; set; }

        public Spleen()
        {
            CapasityBlood = 5000;
        }

        public override string ToString()
        {
            return NameOwner.ToString();
        }
    }

    [DisplayName("Central Nervous System")]
    [Serializable]
    public class CentralNervousSystem : BiologicalSystem
    {
        public bool Passive { get; set; }

        public CentralNervousSystem()
        {
        }
    }

    [DisplayName("Brains")]
    [Serializable]
    public class Brains : CentralNervousSystem
    {
        //public int CapasityBlood { get; set; }
        public string Sensitivity { get; set; }
        public bool SpeechFunction { get; set; }
        [Description("Aggregation")]
        public Bonebrains Bonebrains { get; set; }

        public Brains()
        {
            Sensitivity = "high";
        }

        public override string ToString()
        {
            return NameOwner.ToString();
        }
    }

    [DisplayName("Bloodcirculation System")]
    [Serializable]
    public class BloodCirculationSystem : BiologicalSystem
    {
        public int Blood { get; set; }
        public bool SpeechFunction { get; set; }

        public BloodCirculationSystem()
        {
            Blood = 5;
            SpeechFunction = true;
        }
    }

    [DisplayName("Bonebrains")]
    [Serializable]
    public class Bonebrains : BloodCirculationSystem
    {
        public int Active { get; set; }
        public bool ReflexFunction { get; set; }

        public Bonebrains()
        {
            Active = 1;
            ReflexFunction = false;
        }
    }

    [DisplayName("Heart")]
    [Serializable]
    public class Heart : BloodCirculationSystem
    {
        public int PressureHigh { get; set; }
        public int PressureLow { get; set; }
        public int Diastle { get; set; }
        public int Sistle { get; set; }
        public double HeartCycle { get; set; }

        public Heart()
        {
            PressureHigh = 120;
            PressureLow = 80;
            Sistle = 10;
            Diastle = 20;
            HeartCycle = 0.5;
        }

        public override string ToString()
        {
            return NameOwner.ToString();
        }
    }
}
