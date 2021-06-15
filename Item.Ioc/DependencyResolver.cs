using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.BLL;
using ToDoList.BLL.Interfaces;
using ToDoList.DAL;
using ToDoList.DAL.Interfaces;

namespace Item.Ioc
{
    public class DependencyResolver
    {
        private static IItemDao itemDao;
        public static IItemDao ItemDao => itemDao ?? (itemDao = new ItemDao());

        private static IItemLogic itemLogic;
        public static IItemLogic ItemLogic => itemLogic ?? (itemLogic = new ItemLogic(ItemDao));
    }
}
