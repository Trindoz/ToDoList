using Item.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.BLL.Interfaces;

namespace ConsolePL.View
{
    public class CRUD
    {
        private readonly IItemLogic itemLogic;
        public CRUD()
        {
            itemLogic = DependencyResolver.ItemLogic;
        }
        public static void HelloPage()
        {
            Console.WriteLine("Добро пожаловать в список дел!");
            Console.WriteLine("1-Показать список дел");
            Console.WriteLine("2-Добавить задачу");
            Console.WriteLine("3-Изменить задачу");
            Console.WriteLine("4-Удалить задачу");
            Console.WriteLine("5-Завершить задачу");
            Console.WriteLine("0-Выход");
        }
        public bool Add(int command)
        {
            Models.Item itemView = new Models.Item();
            var itemsDB = new List<Entities.Item>();
            itemsDB = itemLogic.GetAll();
            switch (command)
            {
                case 1:
                    Console.WriteLine("Добавление задачи");
                    Console.WriteLine("Введите номер задачи");
                    int id = 0;
                    string textId = Console.ReadLine();
                    while (!Int32.TryParse(textId, out id))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 1;
                    }
                    if (itemsDB.FirstOrDefault(i => i.Id == id) != null)
                    {
                        Console.WriteLine("Задача с таким номером уже существует!");
                        goto case 1;
                    }
                    itemView.Id = id;
                    goto case 2;
                case 2:
                    Console.WriteLine("Введите имя задачи");
                    string name = Console.ReadLine();
                    if (itemsDB.Count != 0&& itemLogic.GetAll().FirstOrDefault(i=>i.Name.ToUpper()==name.ToUpper()) != null)
                    {
                        Console.WriteLine("Задача с таким именем уже существует!");
                        goto case 2;
                    }
                    itemView.Name = name;
                    goto case 3;
                case 3:
                    Console.WriteLine("Введите текст задачи");
                    string text = Console.ReadLine();
                    itemView.Text = text;
                    goto case 4;
                case 4:
                    Console.WriteLine("Укажите приоритет задачи");
                    Console.WriteLine("1-Высокий");
                    Console.WriteLine("2-Низкий");
                    int priority = 0;
                    string textPriority = Console.ReadLine();
                    while(!Int32.TryParse(textPriority, out priority))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 4;
                    }
                    if (priority == 1)
                    {
                        itemView.Priority = "1";
                        break;
                    }
                    if (priority == 2)
                    {
                        itemView.Priority = "2";
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Значение должно быть 1 или 2");
                        goto case 4;
                    }
            }
            Save(itemView);
            Console.WriteLine("Задача добавлена");
            return true;
        }
        public void ItemsList()
        {

            if (itemLogic.GetAll() == null) Console.WriteLine("Список дел пуст!");
            else
            {
                var itemsDB = itemLogic.GetAll().OrderBy(i => i.Priority == Entities.Priority.Low).ToList();
                foreach (var itemDB in itemsDB)
                {
                    Console.WriteLine($"Номер задачи: {itemDB.Id}");
                    Console.WriteLine($"Имя задачи: {itemDB.Name}");
                    Console.WriteLine($"Текст задачи: {itemDB.Text}");
                    if (itemDB.Priority == Entities.Priority.High)
                    {
                        Console.WriteLine("Приоритет: Высокий");
                    }
                    if (itemDB.Priority == Entities.Priority.Low)
                    {
                        Console.WriteLine("Приоритет: Низкий");
                    }
                    if (itemDB.Finished)
                    {
                        Console.WriteLine("Задача завершена");
                    }
                    if (!itemDB.Finished)
                    {
                        Console.WriteLine("Задача активна");
                    }
                    Console.WriteLine();
                }
            }
        }
        public void Update(int command)
        {
            var tempItems = itemLogic.GetAll();
            var itemIdToUpdate = 0;
            Entities.Item itemToUpdate = new Entities.Item();
            switch (command)
            {
                case 1:
                    Console.WriteLine("Выберите номер задачи которую хотите изменить");
                    ItemsList();
                    int id = 0;
                    string textId = Console.ReadLine();
                    while (!Int32.TryParse(textId, out id))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 1;
                    }
                    itemToUpdate = tempItems.FirstOrDefault(i => i.Id == id);
                    itemIdToUpdate = itemToUpdate.Id;
                    tempItems.Remove(itemToUpdate);
                    if (itemToUpdate == default)
                    {
                        Console.WriteLine($"Задачи с номером {id} не существует");
                        goto case 1;
                    }
                    goto case 2;
                case 2:
                    Console.WriteLine($"Номер задачи:{itemToUpdate.Id}");
                    Console.WriteLine("Изменить номер задачи?");
                    Console.WriteLine("1-Да");
                    Console.WriteLine("2-Нет");
                    int s = 0;                    
                    while (!Int32.TryParse(Console.ReadLine(), out s))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 2;
                    }
                    if (ChangeNumber(s, tempItems, itemToUpdate, out itemToUpdate)) goto case 3;
                    if (s == 2) goto case 3;
                    else
                    {
                        Console.WriteLine("Число должно быть 1 или 2");
                        goto case 2;
                    }
                case 3:
                    Console.WriteLine($"Имя задачи:{itemToUpdate.Name}");
                    Console.WriteLine("Изменить имя задачи?");
                    Console.WriteLine("1-Да");
                    Console.WriteLine("2-Нет");
                    int s1 = 0;
                    while (!Int32.TryParse(Console.ReadLine(), out s1))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 3;
                    }
                    if (s1 == 2) goto case 4;
                    if (ChangeName(s1, tempItems, itemToUpdate, out itemToUpdate)) goto case 4;
                    else
                    {
                        Console.WriteLine("Число должно быть 1 или 2");
                        goto case 3;
                    }
                case 4:
                    Console.WriteLine($"Текст задачи:{itemToUpdate.Text}");
                    Console.WriteLine("Изменить текст задачи?");
                    Console.WriteLine("1-Да");
                    Console.WriteLine("2-Нет");
                    int s2 = 0;
                    while (!Int32.TryParse(Console.ReadLine(), out s2))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 4;
                    }
                    if (s2 == 1)
                    {
                        Console.WriteLine("Введите новый текст задачи");
                        string newText = Console.ReadLine();
                        itemToUpdate.Text = newText;
                        goto case 5;
                    }
                    if (s2 == 2) goto case 5;
                    else
                    {
                        Console.WriteLine("Число должно быть 1 или 2");
                        goto case 4;
                    }
                case 5:
                    Console.WriteLine($"Приоритет задачи:{itemToUpdate.Priority}");
                    Console.WriteLine("Изменить приоритет задачи?");
                    Console.WriteLine("1-Да");
                    Console.WriteLine("2-Нет");
                    int s3 = 0;
                    while (!Int32.TryParse(Console.ReadLine(), out s3))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 4;
                    }
                    if (s3 == 2) goto case 6;
                    if (ChangePriority(s3, tempItems, itemToUpdate, out itemToUpdate)) goto case 6;
                    else
                    {
                        Console.WriteLine("Число должно быть 1 или 2");
                        goto case 5;
                    }
                case 6:                 
                    Console.WriteLine($"Статус задачи:{(itemToUpdate.Finished?"Завершена":"В работе")}");
                    Console.WriteLine("Изменить статус задачи?");
                    Console.WriteLine("1-Да");
                    Console.WriteLine("2-Нет");
                    int s4 = 0;
                    while (!Int32.TryParse(Console.ReadLine(), out s4))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 6;
                    }
                    if (ChangeStatus(s4, tempItems, itemToUpdate, out itemToUpdate, itemIdToUpdate)) break;
                    else
                    {
                        Console.WriteLine("Число должно быть 1 или 2");
                        goto case 6;
                    }                   
            }
        }
        public void Delete()
        {
            Console.WriteLine("Выберите номер задачи чтобы удалить");
            ItemsList();
            int id = 0;
            while (!Int32.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Введеное значение должно быть числом!");
                Delete();
            }
            var itemToDelete = itemLogic.GetAll().FirstOrDefault(i=>i.Id==id);
            if (itemToDelete == null)
            {
                Console.WriteLine($"Задачи с номером {id} не существует");
                Delete();
            }
            else
            {
                itemLogic.Delete(id);
                Console.WriteLine($"Задача {id} удалена");
            }
            
        }
        public void Finish()
        {
            Console.WriteLine("Выберите задачу котрую надо завершить");
            ItemsList();
            int id = 0;
            while (!Int32.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Введеное значение должно быть числом!");
                Finish();
            }
            var itemToFinish = itemLogic.GetAll().FirstOrDefault(i => i.Id == id);            
            if (itemToFinish == null)
            {
                Console.WriteLine($"Задачи с номером {id} не существует");
                Finish();
            }
            else
            {
                itemToFinish.Finished = true;
                itemLogic.Update(itemToFinish, id);
                Console.WriteLine($"Задача {id} завершена");
            }
        }
        private void Save(Models.Item item) 
        {
            var itemDB = new Entities.Item();
            itemDB.Id = item.Id;
            itemDB.Name = item.Name;
            itemDB.Text = item.Text;
            if (item.Priority == "1")
            {
                itemDB.Priority = Entities.Priority.High;
            }
            if (item.Priority == "2")
            {
                itemDB.Priority = Entities.Priority.Low;
            }
            itemDB.Finished = false;
            itemLogic.Add(itemDB);
        }
        private bool ChangeNumber(int number, List<Entities.Item> tempItems, Entities.Item itemToUpdate, out Entities.Item updatedItem)
        {
            updatedItem = itemToUpdate;
            switch (number)
            {
                case 1:
                    Console.WriteLine("Введите новый номер задачи");
                    int newId = 0;
                    while (!Int32.TryParse(Console.ReadLine(), out newId))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 1;
                    }
                    if (tempItems.FirstOrDefault(i => i.Id == newId) != null)
                    {
                        Console.WriteLine($"Задача с номером {newId} уже существет!");
                        goto case 1;
                    }
                    itemToUpdate.Id = newId;
                    Console.WriteLine("Номер задачи изменен");
                    Console.WriteLine($"Новый номер {itemToUpdate.Id}");
                    updatedItem = itemToUpdate;
                    return true;
            }
            return false;
        }
        private bool ChangeName(int number, List<Entities.Item> tempItems, Entities.Item itemToUpdate, out Entities.Item updatedItem)
        {
            updatedItem = itemToUpdate;
            switch (number)
            {
                case 1:
                    Console.WriteLine("Введите новое имя задачи");
                    string newName = Console.ReadLine();
                    if (tempItems.FirstOrDefault(i => i.Name.ToUpper().Replace(" ", "") == newName.ToUpper().Replace(" ", "")) != null)
                    {
                        Console.WriteLine($"Здача с именем {newName} уже существует");
                        goto case 1;
                    }
                    itemToUpdate.Name = newName;
                    return true;
            }
            return false;
        }
        private bool ChangePriority(int number, List<Entities.Item> tempItems, Entities.Item itemToUpdate, out Entities.Item updatedItem)
        {
            updatedItem = itemToUpdate;
            switch (number)
            {
                case 1:
                    Console.WriteLine("Выберите приоритет");
                    Console.WriteLine("1-Высокий");
                    Console.WriteLine("2-Низкий");
                    int newPriority = 0;
                    while (!Int32.TryParse(Console.ReadLine(), out newPriority))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 1;
                    }
                    if (newPriority == 1)
                    {
                        itemToUpdate.Priority = Entities.Priority.High;
                        return true;
                    }
                    if (newPriority == 2)
                    {
                        itemToUpdate.Priority = Entities.Priority.Low;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Число должно быть 1 или 2");
                        goto case 1;
                    }
            }
            return false;
        }
        private bool ChangeStatus(int number, List<Entities.Item> tempItems, Entities.Item itemToUpdate, out Entities.Item updatedItem, int itemIdToUpdate)
        {
            updatedItem = itemToUpdate;
            switch (number)
            {
                case 1:
                    Console.WriteLine("1-Задача завершена");
                    Console.WriteLine("2-Задача в работе");
                    int stat = 0;
                    while (!Int32.TryParse(Console.ReadLine(), out stat))
                    {
                        Console.WriteLine("Введеное значение должно быть числом!");
                        goto case 1;
                    }
                    if (stat == 1)
                    {
                        itemToUpdate.Finished = true;
                        itemLogic.Update(itemToUpdate, itemIdToUpdate);
                        Console.WriteLine("Задача изменена");
                        return true;
                    }
                    if (stat == 2)
                    {
                        itemToUpdate.Finished = false;
                        itemLogic.Update(itemToUpdate, itemIdToUpdate);
                        Console.WriteLine("Задача изменена");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Число должно быть 1 или 2");
                        goto case 1;
                    }
                case 2:
                    itemLogic.Update(itemToUpdate, itemIdToUpdate);
                    Console.WriteLine("Задача изменена");
                    return true;
            }
            return false;
        }
    }
}
