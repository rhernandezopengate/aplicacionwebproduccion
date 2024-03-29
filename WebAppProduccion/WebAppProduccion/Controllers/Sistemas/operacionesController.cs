﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProduccion.Entities.ModulosSistemas;

namespace WebAppProduccion.Controllers.Sistemas
{
    public class operacionesController : Controller
    {
        private DB_A3F19C_producccionEntities1 db = new DB_A3F19C_producccionEntities1();

        // GET: operaciones
        public ActionResult Index()
        {
            var operaciones = db.operaciones.Include(o => o.modulos);
            return View(operaciones.ToList());
        }

        // GET: operaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            operaciones operaciones = db.operaciones.Find(id);
            if (operaciones == null)
            {
                return HttpNotFound();
            }
            return View(operaciones);
        }

        // GET: operaciones/Create
        public ActionResult Create()
        {
            ViewBag.Modulos_Id = new SelectList(db.modulos, "id", "descripcion");
            return View();
        }

        // POST: operaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Nombre,Modulos_Id")] operaciones operaciones)
        {
            if (ModelState.IsValid)
            {
                db.operaciones.Add(operaciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Modulos_Id = new SelectList(db.modulos, "id", "descripcion", operaciones.Modulos_Id);
            return View(operaciones);
        }

        // GET: operaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            operaciones operaciones = db.operaciones.Find(id);
            if (operaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.Modulos_Id = new SelectList(db.modulos, "id", "descripcion", operaciones.Modulos_Id);
            return View(operaciones);
        }

        // POST: operaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Nombre,Modulos_Id")] operaciones operaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(operaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Modulos_Id = new SelectList(db.modulos, "id", "descripcion", operaciones.Modulos_Id);
            return View(operaciones);
        }

        // GET: operaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            operaciones operaciones = db.operaciones.Find(id);
            if (operaciones == null)
            {
                return HttpNotFound();
            }
            return View(operaciones);
        }

        // POST: operaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            operaciones operaciones = db.operaciones.Find(id);
            db.operaciones.Remove(operaciones);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
