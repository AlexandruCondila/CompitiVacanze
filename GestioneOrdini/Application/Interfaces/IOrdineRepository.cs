using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Model.Entities;

namespace Application.Interfaces;

public interface IOrdineRepository
{
    List<Ordine> GetAll();
    Ordine? GetById(int id);
    void Save(Ordine ordine);
    bool Delete(int id);
}
