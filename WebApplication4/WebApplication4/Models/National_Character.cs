using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class National_Character
    {
        public string Unicode { get; set; }
        public string BIG_5 { get; set; }
        public string CNS { get; set; }
        public string EUC { get; set; }
        public int Integer { get; set; }
        public string Character { get; set; }
        public string Phonetic { get; set; }
        public string Radical { get; set; }
        public int Strokes_Total { get; set; }
        public int Strokes_Radical { get; set; }
        public National_Character()
        {
            Unicode = string.Empty;
            BIG_5 = string.Empty;
            CNS = string.Empty;
            EUC = string.Empty;
            Integer = 0;
            Character = string.Empty;
            Phonetic = string.Empty;
            Radical = string.Empty;
            Strokes_Total = 0;
            Strokes_Radical = 0;
        }
    /*    public Student(string _id, string _name, int _score)
        {
            id = _id;
            name = _name;
            score = _score;
        }
    */
    }
}
