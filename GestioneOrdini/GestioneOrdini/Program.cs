using System;
using Application.Dto;
using Application.UseCases;
using Infrastructure.Repositories;

namespace UIConsole;

internal class Program
{
    private static void Main(string[] args)
    {
        var repository = new OrdineJsonRepository();
        var service = new OrdineService(repository);

        bool continuaEsecuzione = true;

        while (continuaEsecuzione)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("  GESTIONE ORDINI (CLEAN ARCHITECTURE)  ");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Visualizza tutti gli ordini");
            Console.WriteLine("2. Cerca ordine per ID");
            Console.WriteLine("3. Aggiungi nuovo ordine");
            Console.WriteLine("4. Elimina ordine");
            Console.WriteLine("0. Esci");
            Console.WriteLine("----------------------------------------");
            Console.Write("Seleziona un'opzione: ");

            string? scelta = Console.ReadLine();

            if (scelta == "1")
            {
                VisualizzaTuttiGliOrdini(service);
                AttendiTasto();
            }
            else if (scelta == "2")
            {
                CercaOrdinePerId(service);
                AttendiTasto();
            }
            else if (scelta == "3")
            {
                AggiungiNuovoOrdine(service);
                AttendiTasto();
            }
            else if (scelta == "4")
            {
                EliminaOrdine(service);
                AttendiTasto();
            }
            else if (scelta == "0")
            {
                Console.WriteLine("\nChiusura dell'applicazione...");
                continuaEsecuzione = false;
            }
            else
            {
                Console.WriteLine("\nOpzione non valida. Riprova.");
                AttendiTasto();
            }
        }
    }

    private static void AttendiTasto()
    {
        Console.WriteLine("\nPremi un tasto per continuare...");
        Console.ReadKey();
    }

    private static void VisualizzaTuttiGliOrdini(OrdineService service)
    {
        Console.WriteLine("\n--- ELENCO ORDINI ---");
        var ordini = service.OttieniTuttiGliOrdini();

        if (ordini.Count == 0)
        {
            Console.WriteLine("Nessun ordine presente a catalogo.");
        }
        else
        {
            foreach (var ordine in ordini)
            {
                StampaDettaglioOrdine(ordine);
            }
        }
    }

    private static void CercaOrdinePerId(OrdineService service)
    {
        Console.WriteLine("\n--- CERCA ORDINE PER ID ---");
        Console.Write("Inserisci l'ID dell'ordine: ");

        try
        {
            int id = int.Parse(Console.ReadLine() ?? "0");
            var ordine = service.OttieniOrdinePerId(id);

            if (ordine != null)
            {
                StampaDettaglioOrdine(ordine);
            }
            else
            {
                Console.WriteLine($"Nessun ordine trovato con ID: {id}");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Errore: l'ID inserito non è un numero valido.");
        }
    }

    private static void AggiungiNuovoOrdine(OrdineService service)
    {
        Console.WriteLine("\n--- INSERIMENTO NUOVO ORDINE ---");
        try
        {
            Console.Write("Nome cliente: ");
            string nomeCliente = Console.ReadLine() ?? string.Empty;

            var nuovoOrdineDto = new OrdineDto
            {
                Cliente = nomeCliente,
                Data = DateTime.Now,
                Stato = "InAttesa"
            };

            bool continuaInserimentoProdotti = true;

            while (continuaInserimentoProdotti)
            {
                Console.Write("\nVuoi inserire un prodotto all'ordine? (s/n): ");
                string? risposta = Console.ReadLine()?.Trim().ToLower();

                if (risposta == "s")
                {
                    try
                    {
                        Console.Write("Nome prodotto: ");
                        string nomeProdotto = Console.ReadLine() ?? string.Empty;

                        Console.Write("Quantità: ");
                        int quantita = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Prezzo unitario (€): ");
                        decimal prezzoUnitario = decimal.Parse(Console.ReadLine() ?? "0");

                        nuovoOrdineDto.Prodotti.Add(new ProdottoOrdineDto
                        {
                            Nome = nomeProdotto,
                            Quantita = quantita,
                            PrezzoUnitario = prezzoUnitario
                        });
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Dati del prodotto non validi. Voce scartata.");
                    }
                }
                else
                {
                    continuaInserimentoProdotti = false;
                }
            }

            int idAssegnato = service.CreaOrdine(nuovoOrdineDto);
            Console.WriteLine($"\nOrdine registrato con successo con ID #{idAssegnato}!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErrore durante il salvataggio: {ex.Message}");
        }
    }

    private static void EliminaOrdine(OrdineService service)
    {
        Console.WriteLine("\n--- ELIMINAZIONE ORDINE ---");
        Console.Write("Inserisci l'ID dell'ordine da eliminare: ");

        try
        {
            int id = int.Parse(Console.ReadLine() ?? "0");
            bool eliminato = service.CancellaOrdine(id);

            if (eliminato)
            {
                Console.WriteLine($"Ordine con ID {id} rimosso con successo.");
            }
            else
            {
                Console.WriteLine($"Nessun ordine trovato con ID {id}. Nessuna modifica effettuata.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Errore: l'ID deve essere un numero valido.");
        }
    }

    private static void StampaDettaglioOrdine(OrdineDto o)
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"Ordine #{o.Id} | Stato: {o.Stato} | Data: {o.Data:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Cliente: {o.Cliente}");
        Console.WriteLine("Articoli:");

        if (o.Prodotti.Count == 0)
        {
            Console.WriteLine("  (Nessun prodotto presente nell'ordine)");
        }
        else
        {
            foreach (var p in o.Prodotti)
            {
                Console.WriteLine($"  - {p.Nome} | Quantità: {p.Quantita} | Prezzo unitario: {p.PrezzoUnitario:F2} € | Subtotale: {p.TotaleParziale:F2} €");
            }
        }

        Console.WriteLine($"TOTALE COMPLESSIVO: {o.TotaleOrdine:F2} €");
    }
}