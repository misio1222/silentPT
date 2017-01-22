using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCIdentityConfirm.Models
{
    public class CompInformation
    {
        public Company comp { get; set; }
        public int awans { get; set; }
        public int uznanie { get; set; }
        public int ksztalcenie { get; set; }
        public int zarobki { get; set; }
        public int atmosfera { get; set; }
        public int uklady { get; set; }
        public int mobbing { get; set; }
        public int dodatki { get; set; }
        public int socjal { get; set; }
        public int stres { get; set; }
        public int liczbaGlosow { get; set; }
        public float zarobkiProcent { get; set; }
        public bool isAdded { get; set; }
        public int liczPracownikow { get; set; }

    }
}