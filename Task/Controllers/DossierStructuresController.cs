using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Task1.Models;

namespace Task1.Controllers
{
    public class DossierStructuresController : Controller
    {
        private readonly TaskContext _context;

        public DossierStructuresController(TaskContext context)
        {
            _context = context;
        }

        // GET: DossierStructures
        public async Task<IActionResult> Index()
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            foreach (var type in _context.DossierStructures.Include(d => d.Parent))
            {
                nodes.Add(new TreeViewNode { id = type.Id.ToString(), parent = type.ParentId.ToString() != "" ? type.ParentId.ToString() : "#", text = type.SectionCode + " " + type.Name, orderNumber = type.OrderNumber.ToString() });
            }
            if (nodes.Count == 0)
            {
                nodes.Add(new TreeViewNode { id = "1", parent = "#", text = "1.1 Административная информация", orderNumber = "0" });
                nodes.Add(new TreeViewNode { id = "2", parent = "1", text = "1.2 Общая информация", orderNumber = "1" });
            }
            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();

        }
        [HttpPost]
        public IActionResult Index(string selectedItems)
        {
            if (selectedItems.Length == 0) { return RedirectToAction("Index"); }
            _context.DossierStructures.RemoveRange(_context.DossierStructures); // удалить все записи из таблицы Users
            _context.SaveChanges();
            List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);
            DossierStructure doss = new DossierStructure();
            int lastParent = 0;
            int OrderNumber = 0;
            Dictionary<string,int> idParentId = new Dictionary<string, int>(); // 
            foreach (var a in items)
            {

                if (a.parent == "#")
                {
                    doss = new DossierStructure()
                    {
                        Name = TextRegexS(a.text),
                        SectionCode = NumberRegexS(a.text),
                        OrderNumber = 0
                    };
                    OrderNumber = 0;
                }
                else
                {
                    OrderNumber++;
                    doss = new DossierStructure()
                    {
                        Name = TextRegexS(a.text),
                        ParentId = idParentId.ContainsKey(a.parent) ? idParentId[a.parent] : lastParent,
                    SectionCode = NumberRegexS(a.text),
                        OrderNumber = OrderNumber
                    };
                }

                _context.DossierStructures.Add(doss);
                _context.SaveChanges();
                idParentId.TryAdd(a.id, doss.Id); // добавляем пару ключ-значение в словарь, если ключ не существует
                if (a.parent == "#")
                    lastParent = doss.Id;
            }


            return RedirectToAction("Index");
        }

        public string NumberRegexS(string s)
        {
            string pattern = @"\d+(\.\d+)*";

            // Создать объект Regex с этим шаблоном
            Regex regex = new Regex(pattern);

            // Найти первое совпадение в строке
            Match match = regex.Match(s);
            return match.Value;
        }
        public string TextRegexS(string s)
        {
            string pattern = @"\d+([\.,]\d+)*";

            // Создать объект Regex с этим шаблоном
            Regex regex = new Regex(pattern);

            // Заменить все совпадения на пустую строку
            string result = regex.Replace(s, "");
            return result;
        }

    }
}
