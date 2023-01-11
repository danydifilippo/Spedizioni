using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Spedizioni.Models
{
    public class Spedizione
    {
        public int IdSpedizione { get; set; }

        public int IdCliente { get; set; }

        [Display(Name = "Mittente")]

        public string Mittente { get; set; }

        public DateTime DataSpedizione { get; set; }

        [Display(Name = "Peso in Kg")]
        public string Peso { get; set; }

        public string Destinatario { get; set; }

        [Display(Name = "Indirizzo Destinatario")]

        public string IndirizzoDestinatario { get; set; }

        [Display(Name = "Città Destinatario")]

        public string CittaDestinatario { get; set; }

        [Display(Name = "Costo Spedizione")]

        public string CostiSpedizione { get; set; }


        [Display(Name = "Data Prevista Consegna")]

        public DateTime DataConsegna { get; set; }

        

        [Display(Name = "Stato Consegna")]

        public string Aggiornamento { get; set; }
    }
}