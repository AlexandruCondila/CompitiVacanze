using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.ValueObjects;

public class Cliente
{
    public string Nome { get; }
    public string Email { get; }

    public Cliente(string nome, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Il nome del cliente non può essere vuoto.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("L'email del cliente non può essere vuota.");

        Nome = nome;
        Email = email;
    }
}
