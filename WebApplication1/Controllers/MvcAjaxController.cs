using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class MvcAjaxController : Controller
    {
        SalesDbEntities1 db = new SalesDbEntities1();
        public ActionResult Single(int? id)
        {
           
            VmSale oSale = null;
            var oSM = db.SaleMasters.Where(x => x.SaleId == id).FirstOrDefault();
            if (oSM != null)
            {
                oSale = new VmSale();
                oSale.CreateDate = oSM.CreateDate;
                oSale.CustomerName = oSM.CustomerName;
                oSale.Gender = oSM.Gender;
                oSale.SaleId = oSM.SaleId;
                var listSaleDetail = new List<VmSale.VmSaleDetail>();
                var listSD = db.SaleDetails.Where(x => x.SaleId == id).ToList();
                foreach (var oSD in listSD)
                {
                    var oSaleDetail = new VmSale.VmSaleDetail();
                    oSaleDetail.Price = oSD.Price;
                    oSaleDetail.ProductName = oSD.ProductName;
                    oSaleDetail.SaleDetailId = oSD.SaleDetailId;
                    oSaleDetail.SaleId = oSD.SaleId;
                    listSaleDetail.Add(oSaleDetail);
                }
                oSale.SaleDetails = listSaleDetail;
            }
            oSale = oSale == null ? new VmSale() : oSale;
            ViewData["List"] = db.SaleMasters.ToList();
            return View(oSale);
        }

        [HttpPost]
        public ActionResult Single(VmSale model)
        {
            var oSaleMaster = db.SaleMasters.Find(model.SaleId);
            if (oSaleMaster == null)
            {
                #region insert
                oSaleMaster = new SaleMaster();
                oSaleMaster.CreateDate = model.CreateDate;
                oSaleMaster.CustomerName = model.CustomerName;
                oSaleMaster.Gender = model.Gender;
                db.SaleMasters.Add(oSaleMaster);
                #endregion
            }
            else
            {
                #region update
                oSaleMaster.CreateDate = model.CreateDate;
                oSaleMaster.CustomerName = model.CustomerName;
                oSaleMaster.Gender = model.Gender;
                var listSaleDetailRem = db.SaleDetails.Where(x => x.SaleId == oSaleMaster.SaleId).ToList();
                db.SaleDetails.RemoveRange(listSaleDetailRem);
                #endregion
            }
            db.SaveChanges();
            var listSaleDetail = new List<SaleDetail>();
            for (var i = 0; i < model.ProductName.Length; i++)
            {
                if (!string.IsNullOrEmpty(model.ProductName[i]))
                {
                    var oSaleDetail = new SaleDetail();
                    oSaleDetail.Price = model.Price[i];
                    oSaleDetail.ProductName = model.ProductName[i];
                    oSaleDetail.SaleId = oSaleMaster.SaleId;
                    listSaleDetail.Add(oSaleDetail);
                }

            }
            db.SaleDetails.AddRange(listSaleDetail);
            db.SaveChanges();
            return RedirectToAction("Single");
        }
        public ActionResult SingleDelete(int id)
        {

            var oSaleMaster = (from o in db.SaleMasters where o.SaleId == id select o).FirstOrDefault();
            var oSaleDetail = (from o in db.SaleDetails where o.SaleId == id select o).ToList();
            if (oSaleMaster != null)
            {
                db.SaleMasters.Remove(oSaleMaster);
                db.SaleDetails.RemoveRange(oSaleDetail);
                db.SaveChanges();
            }
            return RedirectToAction("Single");
        }
    }
}