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
    [Authorize(Roles ="Admin")]
    public class GestioneController : Controller
    {
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
                s.Aggiornamento = reader["StatoSpedizione"].ToString();

                listaSped.Add(s);
                
            }


            sql.Close();

            return View(listaSped);
        }

        public ActionResult EditPrivate(int id)
        {
            SqlConnection sql = Shared.GetConnection();
            sql.Open();

            SqlCommand com = Shared.GetCommand("SELECT * from CLIENTI where IdCliente = @IdCliente", sql);
            com.Parameters.AddWithValue("IdCliente", id);
            SqlDataReader reader = com.ExecuteReader();

            Clienti c = new Clienti();

            while (reader.Read())
            {
                c.IdCliente = Convert.ToInt32(reader["IdCliente"]);
                c.Cognome = reader["Cognome"].ToString();
                c.Nome = reader["Nome"].ToString();
                c.CF = reader["CodiceFiscale"].ToString();
                c.Indirizzo = reader["Residenza_SedeLegale"].ToString();
                c.Telefono = reader["Telefono"].ToString();
                c.email = reader["email"].ToString();

            }
            sql.Close();
            return View(c);
        }

        [HttpPost]
        public ActionResult EditPrivate(Clienti custom)
        {
            SqlConnection sql = Shared.GetConnection();
            sql.Open();

            try
            {

                SqlCommand com = Shared.GetCommand("UPDATE CLIENTI set Cognome=@Cognome, Nome=@Nome, CodiceFiscale=@CodiceFiscale, Residenza_SedeLegale=@Indirizzo," +
                    " Telefono=@Telefono, email=@email where IdCliente = @IdCliente", sql);

                com.Parameters.AddWithValue("IdCliente", custom.IdCliente);
                com.Parameters.AddWithValue("Cognome", custom.Cognome);
                com.Parameters.AddWithValue("Nome", custom.Nome);
                com.Parameters.AddWithValue("CodiceFiscale", custom.CF);
                com.Parameters.AddWithValue("Indirizzo", custom.Indirizzo);
                com.Parameters.AddWithValue("Telefono", custom.Telefono);
                com.Parameters.AddWithValue("email", custom.email);

                int row = com.ExecuteNonQuery();

                if (row > 0)
                {
                    ViewBag.confirm = "Scheda cliente modificata con successo";
                }

            }
            catch (Exception ex)
            {
                ViewBag.errore = ex.Message;
            }
            finally { sql.Close(); }

            return RedirectToAction("Privati");
        }

        public ActionResult CreatePrivate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePrivate(Clienti custom)
        {
            SqlConnection sql = Shared.GetConnection();
            sql.Open();

            try
            {

                SqlCommand com = Shared.GetCommand("INSERT INTO CLIENTI (Cognome, Nome, CodiceFiscale, Residenza_SedeLegale," +
                    " Telefono, email) values (@Cognome, @Nome, @CF, @Indirizzo, @Telefono, @email) ", sql);

                
                com.Parameters.AddWithValue("Cognome", custom.Cognome);
                com.Parameters.AddWithValue("Nome", custom.Nome);
                com.Parameters.AddWithValue("CF", custom.CF);
                com.Parameters.AddWithValue("Indirizzo", custom.Indirizzo);
                com.Parameters.AddWithValue("Telefono", custom.Telefono);
                com.Parameters.AddWithValue("email", custom.email);

                int row = com.ExecuteNonQuery();

                if (row > 0)
                {
                    ViewBag.confirm = "Scheda cliente creata con successo";
                }

            }
            catch (Exception ex)
            {
                ViewBag.errore = ex.Message;
            }
            finally { sql.Close(); }

          
            return RedirectToAction("Privati");
        }

        public ActionResult CreateDelivery()
        {
            return View();
        }


            [HttpPost]
        public ActionResult CreateDelivery(Spedizione del)
        {
            SqlConnection sql = Shared.GetConnection();
            sql.Open();

            try
            {

                SqlCommand com = Shared.GetCommand("INSERT INTO SPEDIZIONI (Data_Spedizione, Peso, Destinatario, Ind_Destinatario," +
                    " Citta_Destinatario, Costo_Spedizione, Data_Consegna, IdCliente) values (@DataInvio, @Peso, " +
                    "@Destinatario, @Ind_Destinatario, @Citta_Dest, @Costo_Spedizione, @Data_Consegna, @IdCliente) ", sql);


                com.Parameters.AddWithValue("DataInvio", del.DataSpedizione);
                com.Parameters.AddWithValue("Peso", del.Peso);
                com.Parameters.AddWithValue("Destinatario", del.Destinatario);
                com.Parameters.AddWithValue("Ind_Destinatario", del.IndirizzoDestinatario);
                com.Parameters.AddWithValue("Citta_Dest", del.CittaDestinatario);
                com.Parameters.AddWithValue("Costo_Spedizione", del.CostiSpedizione);
                com.Parameters.AddWithValue("Data_Consegna", del.DataConsegna);
                com.Parameters.AddWithValue("IdCliente", del.IdCliente);


                int row = com.ExecuteNonQuery();

                if (row > 0)
                {
                    ViewBag.confirm = "Spedizione inserita con successo";
                }

            }
            catch (Exception ex)
            {
                ViewBag.errore = ex.Message;
            }
            finally { sql.Close(); }


            return RedirectToAction("Spedizioni");
        }
    }
}