using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Application.Interfaces;
using Domain.Model.Entities;
using Infrastructure.Dto;
using Infrastructure.Mappers;

namespace Infrastructure.Repositories;

public class OrdineJsonRepository : IOrdineRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public OrdineJsonRepository(string filePath = "ordini.json")
    {
        _filePath = filePath;
    }

    private List<OrdinePersistenceDto> LeggiFile()
    {
        if (!File.Exists(_filePath))
            return new List<OrdinePersistenceDto>();

        try
        {
            string json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<OrdinePersistenceDto>();

            // Parsing tollerante: legge il JSON grezzo per gestire sia stringhe che oggetti
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Array)
                return new List<OrdinePersistenceDto>();

            var lista = new List<OrdinePersistenceDto>();

            foreach (var elemento in doc.RootElement.EnumerateArray())
            {
                var dto = new OrdinePersistenceDto();

                if (elemento.TryGetProperty("Id", out var idProp))
                    dto.Id = idProp.GetInt32();

                // Gestisce sia "Cliente": "Mario" sia "Cliente": { "Nome": "Mario" }
                if (elemento.TryGetProperty("Cliente", out var clienteProp))
                {
                    if (clienteProp.ValueKind == JsonValueKind.String)
                    {
                        dto.Cliente = clienteProp.GetString() ?? string.Empty;
                    }
                    else if (clienteProp.ValueKind == JsonValueKind.Object)
                    {
                        if (clienteProp.TryGetProperty("Nome", out var nomeProp))
                            dto.Cliente = nomeProp.GetString() ?? string.Empty;
                    }
                }
                else if (elemento.TryGetProperty("NomeCliente", out var nomeClienteProp))
                {
                    dto.Cliente = nomeClienteProp.GetString() ?? string.Empty;
                }

                if (elemento.TryGetProperty("Data", out var dataProp))
                {
                    if (dataProp.TryGetDateTime(out var dt))
                        dto.Data = dt;
                }

                if (elemento.TryGetProperty("Stato", out var statoProp))
                {
                    if (statoProp.ValueKind == JsonValueKind.Number)
                        dto.Stato = statoProp.GetInt32();
                    else if (statoProp.ValueKind == JsonValueKind.String)
                        dto.Stato = 0;
                }

                if (elemento.TryGetProperty("Prodotti", out var prodArray) && prodArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var p in prodArray.EnumerateArray())
                    {
                        var pDto = new ProdottoOrdinePersistenceDto();
                        if (p.TryGetProperty("Nome", out var pNome)) pDto.Nome = pNome.GetString() ?? string.Empty;
                        if (p.TryGetProperty("Quantita", out var pQta)) pDto.Quantita = pQta.GetInt32();
                        if (p.TryGetProperty("PrezzoUnitario", out var pPrezzo)) pDto.PrezzoUnitario = pPrezzo.GetDecimal();
                        dto.Prodotti.Add(pDto);
                    }
                }

                lista.Add(dto);
            }

            return lista;
        }
        catch
        {
            // Se il file era corrotto o incompatibile, lo ignora e riparte da una lista vuota
            return new List<OrdinePersistenceDto>();
        }
    }

    private void ScriviFile(List<OrdinePersistenceDto> lista)
    {
        string json = JsonSerializer.Serialize(lista, _options);
        File.WriteAllText(_filePath, json);
    }

    public List<Ordine> GetAll()
    {
        return LeggiFile().Select(OrdinePersistenceMapper.ToDomain).ToList();
    }

    public Ordine? GetById(int id)
    {
        var dto = LeggiFile().FirstOrDefault(x => x.Id == id);
        return dto != null ? OrdinePersistenceMapper.ToDomain(dto) : null;
    }

    public void Save(Ordine ordine)
    {
        var dtos = LeggiFile();
        var itemDto = OrdinePersistenceMapper.ToPersistence(ordine);
        int index = dtos.FindIndex(x => x.Id == ordine.Id);

        if (index >= 0)
            dtos[index] = itemDto;
        else
            dtos.Add(itemDto);

        ScriviFile(dtos);
    }

    public bool Delete(int id)
    {
        var dtos = LeggiFile();
        var item = dtos.FirstOrDefault(x => x.Id == id);
        if (item == null)
            return false;

        dtos.Remove(item);
        ScriviFile(dtos);
        return true;
    }
}