using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;

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
    //[Serializable]
    public class BiologicalSystem
    {
        public string NameOwner { get; set; }
        public Lesion Lesion { get; set; }
        public int QualityofSystems { get; set; }
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
    //[Serializable]
    public class GastrointestinalSystem : BiologicalSystem
    {
        public float Pepsin { get; set; }
        public float Gelatin { get; set; }

        public GastrointestinalSystem()
        {
            Pepsin = 15;
            Gelatin = 4;
        }
    }

    // печень
    [DisplayName("Liver")]
    //[Serializable]
    public class Liver : GastrointestinalSystem
    {
        public float Fats { get; set; }
        public float Proteins { get; set; }
        public float Carbohydrates { get; set; }

        public Liver()
        {
            Fats = 55;
            Proteins = 111;
            Carbohydrates = 12;
        }
    }

    [DisplayName("Respiratory System")]
    //[Serializable]
    public class RespiratorySystem : BiologicalSystem
    {
        public bool PassiveResp { get; set; }
        
        public RespiratorySystem()
        {
            PassiveResp = false;
        }
    }

    [DisplayName("Nose")]
    //[Serializable]
    public class Nose : RespiratorySystem
    {
        public int Pause { get; set; }
        public bool CleaningAir { get; set; }

        public Nose()
        {
            Pause = 12;
            CleaningAir = true;
        }
    }

    [DisplayName("Lungs")]
    //[Serializable]
    public class Lungs : RespiratorySystem
    {
        public bool Breath { get; set; }
        public bool Inhale { get; set; }
        [Description("Aggregation")]
        public Nose Nose { get; set; }

        public Lungs()
        {
            Breath = true;
            Inhale = true;
        }
    }

    [DisplayName("Immune System")]
    ///[Serializable]
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
    //[Serializable]
    public class Spleen : ImmuneSystem
    {
        public int CapasityBlood { get; set; }
        public bool BloodFiltration { get; set; }

        public Spleen()
        {
            CapasityBlood = 5000;
            BloodFiltration = true;
        }
    }

    [DisplayName("Central Nervous System")]
    //[Serializable]
    public class CentralNervousSystem : BiologicalSystem
    {
        public bool Passive { get; set; }

        public CentralNervousSystem()
        {
            Passive = false;
        }
    }

    [DisplayName("Brains")]
    //[Serializable]
    public class Brains : CentralNervousSystem
    {
        //public int CapasityBlood { get; set; }
        public string Sensitivity { get; set; }
        public bool SpeechFunction { get; set; }
        [Description("Composition")]
        public Bonebrains BoneBrains { get; set; }

        public Brains()
        {
            Sensitivity = " ";
            SpeechFunction = true;
        }
    }

    [DisplayName("Bloodcirculation System")]
    //[Serializable]
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
    //[Serializable]
    public class Bonebrains : BloodCirculationSystem
    {
        public bool Active { get; set; }
        public bool ReflexFunction { get; set; }

        public Bonebrains()
        {
            SpeechFunction = true;
        }
    }

    [DisplayName("Heart")]
    //[Serializable]
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
            Sistle = 0;
            Diastle = 10;
            HeartCycle = 0.5;
        }
    }
}
