using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeautyShop.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BeautyShop.Controllers
{
    public class service_workController : Controller
    {
        private BeautyDataEntities db = new BeautyDataEntities();

        public FileStreamResult DownloadExcel()
        {
            IQueryable<BeautyShop.Models.service_work> works = db.service_work;
            int rows = works.Count();
            int columns = 4;
            string[,] data = new string[rows, columns];
            int i = 0;
            foreach(var work in works)
            {
                data[i, 0] = work.id_service.ToString();
                data[i, 1] = work.service_name;
                data[i, 2] = work.service_price.ToString();
                i++;
            }
            MemoryStream memoryStream = GenerateExcel(data);
            memoryStream.Position = 0;
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "ServiceWorks.xlsx"
            };
        }

        public MemoryStream GenerateExcel(string[,] data)
        {
            MemoryStream mStream = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(mStream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                Row headerRow = new Row();
                headerRow.Append(
                    new Cell(new CellValue("ID")) { DataType = CellValues.String },
                    new Cell(new CellValue("УСЛУГА")) { DataType = CellValues.String },
                    new Cell(new CellValue("СТОИМОСТЬ")) { DataType = CellValues.String });
                sheetData.Append(headerRow);
                for (var i = 0; i <= data.GetUpperBound(0); i++)
                {
                    Row row = new Row();
                    for (var j = 0; j <= data.GetUpperBound(1); j++)
                    {
                        Cell cell = new Cell(new CellValue(data[i, j])) { DataType = CellValues.String };
                        row.Append(cell);
                    }
                    sheetData.Append(row);
                }

                Columns columns = new Columns();

                for (int col = 1; col <= data.GetUpperBound(1) + 1; col++)
                {
                    columns.Append(new Column() { Min = (uint)col, Max = (uint)col, Width = 15, CustomWidth = true });
                }
                worksheetPart.Worksheet.InsertAt(columns, 0);

                document.WorkbookPart.Workbook.Save();
                return mStream;
            }
        }

        // GET: service_work
        public ActionResult Index()
        {
            if (Session["id_user"] != null)
            {
                var service_work = db.service_work.Include(s => s.service_type);
                var sortedServiceWorks = service_work.OrderBy(s => s.type_id).ToList();
                return View(sortedServiceWorks);
            }
            else
            {
                return View();
            }
            
        }

        // GET: service_work/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_work service_work = db.service_work.Find(id);
            if (service_work == null)
            {
                return HttpNotFound();
            }
            return View(service_work);
        }

        // GET: service_work/Create
        public ActionResult Create()
        {
            ViewBag.type_id = new SelectList(db.service_type, "id_type", "type_name");
            return View();
        }

        // POST: service_work/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_service,type_id,service_name,service_price")] service_work service_work)
        {
            if (ModelState.IsValid)
            {
                db.service_work.Add(service_work);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.type_id = new SelectList(db.service_type, "id_type", "type_name", service_work.type_id);
            return View(service_work);
        }

        // GET: service_work/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_work service_work = db.service_work.Find(id);
            if (service_work == null)
            {
                return HttpNotFound();
            }
            ViewBag.type_id = new SelectList(db.service_type, "id_type", "type_name", service_work.type_id);
            return View(service_work);
        }

        // POST: service_work/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_service,type_id,service_name,service_price")] service_work service_work)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service_work).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.type_id = new SelectList(db.service_type, "id_type", "type_name", service_work.type_id);
            return View(service_work);
        }

        // GET: service_work/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_work service_work = db.service_work.Find(id);
            if (service_work == null)
            {
                return HttpNotFound();
            }
            return View(service_work);
        }

        // POST: service_work/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            service_work service_work = db.service_work.Find(id);
            db.service_work.Remove(service_work);
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
