using MasterDetailsCrud.Models;
using MasterDetailsCrud.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace MasterDetailsCrud.Controllers
{
    public class SalesController : Controller
    {
        SalesDB db = new SalesDB();
        [HttpGet]
        public ActionResult Single(int? id)
        {
            var item = new List<SelectListItem>
            {
                new SelectListItem { Value = "Food", Text = "Food"},
                new SelectListItem { Value = "Machine", Text = "Machine" }
            };

            VmSale oSale = null;
            var oSM = db.SaleMasters.Where(x => x.SaleId == id).FirstOrDefault();
            if (oSM != null)
            {
                oSale = new VmSale();
                oSale.SaleId = oSM.SaleId;
                oSale.CreateDate = oSM.CreateDate;
                oSale.CustomerName = oSM.CustomerName;
                oSale.CustomerAddress = oSM.CustomerAddress;
                oSale.Gender = oSM.Gender;
                oSale.Photo = oSM.Photo;
                oSale.ProductType = oSM.ProductType;
                var listSaleDetail = new List<VmSale.VmSaleDetail>();
                var listSD = db.SaleDetails.Where(x => x.SaleId == id).ToList();
                foreach (var oSD in listSD)
                {
                    var oSaleDetail = new VmSale.VmSaleDetail();
                    oSaleDetail.SaleDetailId = oSD.SaleDetailId;
                    oSaleDetail.SaleId = oSD.SaleId;
                    oSaleDetail.ProductName = oSD.ProductName;
                    oSaleDetail.UnitPrice = oSD.UnitPrice;
                    oSaleDetail.Quantity = oSD.Quantity;
                    oSaleDetail.TotalPrice = oSD.TotalPrice;
                    listSaleDetail.Add(oSaleDetail);
                }
                oSale.SaleDetails = listSaleDetail;
            }
            oSale = oSale == null ? new VmSale() : oSale;
            ViewData["List"] = db.SaleMasters.ToList();
            ViewData["item"] = item;
            return View(oSale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Single(VmSale model, HttpPostedFileBase img, FormCollection form)
        {
            var oSaleMaster = db.SaleMasters.Find(model.SaleId);
            string filename = null;
            if (img != null)
            {
                filename = img.FileName;
                string path = Path.Combine(Server.MapPath("~/Picture"), filename);
                img.SaveAs(path);
            }
            if (oSaleMaster == null)
            {
                oSaleMaster = new SaleMaster();
                oSaleMaster.CreateDate = model.CreateDate;
                oSaleMaster.CustomerName = model.CustomerName;
                oSaleMaster.CustomerAddress = model.CustomerAddress;
                oSaleMaster.Gender = model.Gender;
                oSaleMaster.Photo = "/Picture/" + filename;
                oSaleMaster.ProductType = form["SelectedValue"];
                db.SaleMasters.Add(oSaleMaster);
                ViewBag.Message = "Inserted successfully.";
                var listSaleDetail = new List<SaleDetail>();
                for (var i = 0; i < model.ProductName.Length; i++)
                {
                    if (!string.IsNullOrEmpty(model.ProductName[i]))
                    {
                        var oSaleDetail = new SaleDetail();
                        oSaleDetail.SaleId = oSaleMaster.SaleId;
                        oSaleDetail.ProductName = model.ProductName[i];
                        oSaleDetail.UnitPrice = model.UnitPrice[i];
                        oSaleDetail.Quantity = model.Quantity[i];
                        oSaleDetail.TotalPrice = model.UnitPrice[i] * model.Quantity[i];
                        listSaleDetail.Add(oSaleDetail);
                    }
                    else
                    {
                        ViewBag.Message = "Product name or attachment required.";
                    }
                }
                db.SaleMasters.Add(oSaleMaster);
                db.SaleDetails.AddRange(listSaleDetail);
                db.SaveChanges();
            }
            else
            {
                oSaleMaster.CreateDate = model.CreateDate;
                oSaleMaster.CustomerName = model.CustomerName;
                oSaleMaster.CustomerAddress = model.CustomerAddress;
                oSaleMaster.Gender = model.Gender;
                oSaleMaster.Photo = (filename != null) ? "/Picture/" + filename : oSaleMaster.Photo;
                oSaleMaster.ProductType = form["SelectedValue"];
                ViewBag.Message = "Updated successfully.";
                var listSaleDetailRem = db.SaleDetails.Where(x => x.SaleId == model.SaleId).ToList();
                var oSaleMasterRem = db.SaleMasters.Find(model.SaleId);
                db.SaleMasters.Remove(oSaleMasterRem);
                db.SaleDetails.RemoveRange(listSaleDetailRem);
                db.SaveChanges();
                var listSaleDetail = new List<SaleDetail>();

                for (var i = 0; i < model.ProductName.Length; i++)
                {
                    if (!string.IsNullOrEmpty(model.ProductName[i]))
                    {
                        var oSaleDetail = new SaleDetail();
                        oSaleDetail.SaleId = oSaleMaster.SaleId;
                        oSaleDetail.ProductName = model.ProductName[i];
                        oSaleDetail.UnitPrice = model.UnitPrice[i];
                        oSaleDetail.Quantity = model.Quantity[i];
                        oSaleDetail.TotalPrice = model.UnitPrice[i] * model.Quantity[i];
                        listSaleDetail.Add(oSaleDetail);
                    }
                    else
                    {
                        ViewBag.Message = "Product name or attachment required.";
                    }
                }
                db.SaleMasters.Add(oSaleMaster);
                db.SaleDetails.AddRange(listSaleDetail);
                db.SaveChanges();
            }
            return RedirectToAction("Single");
        }

        [HttpGet]
        public ActionResult SingleDelete(int id)
        {
            var oSaleMaster = (from o in db.SaleMasters where o.SaleId == id select o).FirstOrDefault();
            var oSaleDetail = (from o in db.SaleDetails where o.SaleId == id select o).ToList();

            if (oSaleDetail != null) db.SaleDetails.RemoveRange(oSaleDetail);
            db.SaleMasters.Remove(oSaleMaster);
            db.SaveChanges();
            return RedirectToAction("Single");
        }
    }
}