using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Organic_Zone.Models;

namespace Organic_Zone.Controllers
{
    public class ItemsController : Controller
    {
        private OrganicZoneDBContext db = new OrganicZoneDBContext();

        // GET: Items
        public ActionResult Index(string search)
        {
            if (search != null)
            {
                ViewBag.Title = search.ToUpper();
                return View(db.Items.Where(p => p.ItemName.Contains(search) || p.Description.Contains(search)));
            }

            ViewBag.Title = "Store";

            return View(db.Items.ToList());
        }

        // GET: Items/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ItemName,Description,Price,Discount")] Item item, HttpPostedFileBase Image)
        {
            // Uploading Thumbnail
            string pic = System.IO.Path.GetFileName(Image.FileName);

            string MyPath = "/Images/Items/" + item.ItemName + "/";

            bool exists = System.IO.Directory.Exists(Server.MapPath(MyPath));

            if (!exists)
                System.IO.Directory.CreateDirectory(Server.MapPath(MyPath));

            string path = System.IO.Path.Combine(Server.MapPath(MyPath), pic);

            Image.SaveAs(path);

            item.Thumbnail = MyPath + pic;

            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string? ItemName)
        {
            if (ItemName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(ItemName);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemName,Thumbnail,Description,Price,Discount")] Item item, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                // Uploading Thumbnail
                string pic = System.IO.Path.GetFileName(Image.FileName);

                string MyPath = "/Images/Items/" + item.ItemName + "/";

                bool exists = System.IO.Directory.Exists(Server.MapPath(MyPath));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(MyPath));

                string path = System.IO.Path.Combine(Server.MapPath(MyPath), pic);

                Image.SaveAs(path);

                item.Thumbnail = MyPath + pic;
            }

            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string? ItemName)
        {
            if (ItemName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(ItemName);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string ItemName)
        {
            Item item = db.Items.Find(ItemName);
            db.Items.Remove(item);
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
