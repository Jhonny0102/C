using Microsoft.AspNetCore.Mvc;
using MvcCoreCrudDoctores.Models;
using MvcCoreCrudDoctores.Repositories;

namespace MvcCoreCrudDoctores.Controllers
{
    public class DoctoresController : Controller
    {
        RepositoryDoctores repo;

        public DoctoresController()
        {
            this.repo = new RepositoryDoctores();
        }
        //Controller donde vemos todos los doctores
        public async Task<IActionResult> Index()
        {
            List<Doctores> doctores = await this.repo.GetDoctoresAsync();
            return View(doctores);
        }

        public async Task<IActionResult> Details(int id)
        {
            Doctores doctor = await this.repo.FindDoctoresAsync(id);
            return View(doctor);
        }

        // Zona Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(int id_hospital, string apellido, string especialidad , int salario)
        {
            await this.repo.InsertDoctor(id_hospital,apellido,especialidad,salario);
            return RedirectToAction("Index");
        }
    }
}
