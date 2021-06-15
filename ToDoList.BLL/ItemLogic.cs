using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.BLL.Interfaces;
using ToDoList.DAL.Interfaces;

namespace ToDoList.BLL
{
    public class ItemLogic : IItemLogic
    {
        private readonly IItemDao itemDao;
        public ItemLogic(IItemDao itemDao)
        {
            this.itemDao = itemDao;
        }
        public bool Add(Item item)
        {
            return itemDao.Add(item);
        }

        public bool Delete(int id)
        {
            return itemDao.Delete(id);
        }

        public List<Item> GetAll()
        {
            return itemDao.GetAll();
        }

        public Item GetByName(string name)
        {
            return itemDao.GetByName(name);
        }
        public bool Update(Item item, int id)
        {
            return itemDao.Update(item, id);
        }
    }
}
