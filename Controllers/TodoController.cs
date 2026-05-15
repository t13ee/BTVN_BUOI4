using Microsoft.AspNetCore.Mvc;
using BTVN4.Models;
using System.Collections.Generic;
using System.Linq;

namespace BTVN4.Controllers
{
    // LỖI: Thiếu ": Controller" sẽ dẫn đến lỗi "View does not exist"
    public class TodoController : Controller
    {
        // LỖI: Biến này phải nằm TRONG class nhưng NGOÀI các hàm
        private static List<TodoItem> _todoList = new List<TodoItem>
        {
            new TodoItem { Id = 1, Name = "Đi chợ", IsCompleted = true },
            new TodoItem { Id = 2, Name = "Chơi thể thao", IsCompleted = false }
        };

        public IActionResult Index()
        {
            return View(_todoList); // View() chỉ tồn tại nếu class kế thừa : Controller
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            // ModelState chỉ tồn tại nếu kế thừa : Controller
            if (ModelState.IsValid)
            {
                _todoList.Add(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // Các hàm Edit, Delete cũng phải nằm trong này...
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _todoList.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Todo/Edit/1
        // Hàm này dùng để cập nhật dữ liệu vào danh sách sau khi nhấn nút "Sửa"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _todoList.FirstOrDefault(x => x.Id == item.Id);
                if (existingItem != null)
                {
                    existingItem.Name = item.Name;
                    existingItem.IsCompleted = item.IsCompleted;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }
        // 1. Chi tiết (Details)
        public IActionResult Details(int id)
        {
            var item = _todoList.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // 2. Xoá - Trang xác nhận (GET)
        public IActionResult Delete(int id)
        {
            var item = _todoList.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // 3. Xoá - Thực thi (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _todoList.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _todoList.Remove(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}