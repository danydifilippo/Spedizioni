using Spedizioni.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Spedizioni.Controllers
{
    public class GestioneController : Controller
    {
        // GET: Gestione
        public ActionResult Privati()
        {
            List<Clienti> listaClienti = new List<Clienti>();
            
            SqlConnection sql = Shared.GetConnection();
            sql.Open();
            SqlCommand com = Shared.GetCommand("SELECT * from CLIENTI where TipoCliente = 'Privato'", sql);
            SqlDataReader reader = com.ExecuteReader();

            while(reader.Read())
            {
                Clienti c = new Clienti();
                    c.IdCliente = Convert.ToInt32(reader["IdCliente"]);
                    c.Cognome = reader["Cognome"].ToString();
                    c.Nome = reader["Nome"].ToString();
                    c.CF = reader["CodiceFiscale"].ToString();
                    c.Indirizzo = reader["Residenza_SedeLegale"].ToString();
                    c.Telefono = reader["Telefono"].ToString();
                    c.email = reader["email"].ToString();
                    listaClienti.Add(c);
            }
            sql.Close();

            return View(listaClienti);
        }

        public ActionResult Aziende()
        {
            List<Clienti> listaClienti = new List<Clienti>();

            SqlConnection sql = Shared.GetConnection();
            sql.Open();
            SqlCommand com = Shared.GetCommand("SELECT * from CLIENTI where TipoCliente = 'Azienda'", sql);
            SqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                Clienti c = new Clienti();
                c.IdCliente = Convert.ToInt32(reader["IdCliente"]);
                c.RagSociale = reader["RagioneSociale"].ToString();
                c.PIVA = reader["P_IVA"].ToString();
                c.Indirizzo = reader["Residenza_SedeLegale"].ToString();
                c.Telefono = reader["Telefono"].ToString();
                c.email = reader["email"].ToString();
                listaClienti.Add(c);
            }
            sql.Close();

            return View(listaClienti);
        }

        public ActionResult Spedizioni()
        {
            List<Spedizione> listaSped = new List<Spedizione>();

            SqlConnection sql = Shared.GetConnection();
            sql.Open();

            SqlCommand com = Shared.GetCommand("SELECT * FROM SPEDIZIONE inner join " +
                "CLIENTI ON SPEDIZIONE.IdCliente = CLIENTI.IdCliente", sql);
            SqlDataReader reader = com.ExecuteReader();

          

            while (reader.Read())
            {
                Spedizione s = new Spedizione();

                s.Mittente = reader["Cognome"].ToString() +" "+ reader["Nome"].ToString();
                s.IdSpedizione = Convert.ToInt32(reader["IdSpedizione"]);
                s.DataSpedizione = Convert.ToDateTime(reader["Data_Spedizione"]);
                s.Peso = reader["Peso"].ToString();
                s.Destinatario = reader["Destinatario"].ToString();
                s.IndirizzoDestinatario = reader["Ind_Destinatario"].ToString();
                s.CittaDestinatario = reader["Citta_Destinatario"].ToString();
                s.CostiSpedizione = reader["Costo_Spedizione"].ToString();
                s.DataConsegna = Convert.ToDateTime(reader["Data_Consegna"]);

                listaSped.Add(s);
                
            }

            reader.Close();
            sql.Close();

            SqlConnection con = Shared.GetConnection();
            con.Open();

            foreach (Spedizione item in listaSped)
            {

                SqlCommand c = Shared.GetCommand("SELECT top(1) * from AGGIORNAMENTO inner join STATO_SPEDIZIONE " +
                    "ON STATO_SPEDIZIONE.IdStato = Aggiornamento.IdStato where IdSpedizione = @IdSped order by DataAggiornamento Desc", con);
                c.Parameters.AddWithValue("IdSped", item.IdSpedizione);
                SqlDataReader read = c.ExecuteReader();
                
                while (read.Read())
                {
                    item.Aggiornamento = read["Stato"].ToString();        
                }

            }

            con.Close();

            return View(listaSped);
        }
    }
}