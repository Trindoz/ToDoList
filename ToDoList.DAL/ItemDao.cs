using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ToDoList.DAL.Interfaces;

namespace ToDoList.DAL
{
    public class ItemDao : IItemDao
    {
        public bool Add(Item item)
        {
            try
            {
                BinaryFormatter binFormat = new BinaryFormatter();
                List<Item> items = GetAll();
                if (items.FirstOrDefault(i => i.Id == item.Id) != null) throw new Exception($"Заметкa с №{item.Id} уже существует!");
                if (items.FirstOrDefault(i => i.Name == item.Name) != null) throw new Exception($"Заметкa с именем {item.Name} уже существует!");
                using (Stream fStream = new FileStream("Item.dat", FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    try
                    {
                        binFormat.Serialize(fStream, item);
                    }
                    catch (SerializationException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
                return true;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                List<Item> items = GetAll();
                var item = items.FirstOrDefault(i => i.Id == id);
                if (item == null) throw new Exception($"Заметки с №{id} не найдено");
                items.Remove(item);
                BinaryFormatter binFormat = new BinaryFormatter();
                using (Stream fStream = new FileStream("Item.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                {                    
                    foreach (var it in items)
                    {
                        binFormat.Serialize(fStream, it);
                    }
                }
                return true;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Item> GetAll()
        {
            List<Item> items = new List<Item>();
            try
            {
                BinaryFormatter binFormat = new BinaryFormatter();
                using (Stream fStream = new FileStream("Item.dat", FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    try
                    {
                        while (fStream.Position != fStream.Length)
                        {
                            items.Add((Item)binFormat.Deserialize(fStream));
                        }   
                    }
                    catch(SerializationException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return items;
                    }
                }
                return items;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return items;
            }
        }

        public Item GetByName(string name)
        { 
            try
            {
                var item=GetAll().FirstOrDefault(i => i.Name.ToUpper() == name.ToUpper());
                if (item == null) throw new Exception($"Заметки с именем {name} не найдено");
                return item;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool Update(Item item, int id)
        {
            try
            {
                List<Item> items = GetAll();
                var itemDB = items.Find(i => i.Id == id);
                if (itemDB == null) Console.WriteLine($"Задача {id} не найдена!"); 
                items.Remove(itemDB);
                items.Add(item);
                BinaryFormatter binFormat = new BinaryFormatter();
                using (Stream fStream = new FileStream("Item.dat", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    foreach (var it in items)
                    {
                        binFormat.Serialize(fStream, it);
                    }
                }
                return true;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
