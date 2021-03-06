using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.DAL.Interfaces
{
    public interface IItemDao
    {
        bool Add(Item item);
        bool Delete(int id);
        bool Update(Item item, int id);
        Item GetByName(string name);
        List<Item> GetAll();
    }
}
