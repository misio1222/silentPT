using MVCIdentityConfirm.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.IO;
using System.Drawing;
using System.Web;
using MVCIdentityConfirm.Necessary;

namespace MVCIdentityConfirm.Controllers
{
    public class RatingController : Controller
    {
        // GET: Rating
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult RatPrzelozony(int id)
        {
            dataDB dt = new dataDB();
            ocenaPrzelozony oc = new ocenaPrzelozony();
            string userId = User.Identity.GetUserId().ToString();
            var manager = dt.Przelozony.Where(x => x.Id == id).Select(y => y).FirstOrDefault();
            var manWydz = dt.Wydzial.Where(z => z.PrzelozonyId == id).Select(h => h).FirstOrDefault();
            var manFirma = dt.Company.Where(v => v.Id == manWydz.CompanyID).Select(s => s).FirstOrDefault();
            var ocenaGet = dt.ocenaPrzelozony.Where(a => a.przelozonyId == id && a.userId == userId).Select(m => m).FirstOrDefault();
            if (ocenaGet != null) {
                oc = ocenaGet;
            }
            else
            {
                oc.userId = "0";
                oc.przelozonyId = 0;
                oc.kulturaOs = 50;
                oc.inteligencja = 50;
                oc.cwaniastwo = 50;
                oc.przyznanieBlad = 50;
                oc.radzenieKrytyka = 50;
                oc.rzetenaWiedza = 50;
                oc.udzielaniePochwal = 50;
                oc.umieKomunikowania = 50;
                oc.umiSluchania = 50;
            }
            managerInfo mag = new managerInfo();

            mag.imie = manager.Imie;
            mag.nazwisko = manager.Nazwisko;
            mag.stanowisko = manager.Stanowisko;
            mag.wydzial = manWydz.Wydzial1;
            mag.firma = manFirma.CompanyName;
            mag.id = manager.Id;
            mag.oceny = oc;
            mag.companyId = manFirma.Id;

            return View(mag);
        }
         
        public ActionResult rateCompany(int id, int? przelozony)
        {
            dataDB dta = new dataDB();
            string userId = User.Identity.GetUserId();
            var oc = dta.ocenaCompany.Where(x => x.companyId == id && x.userId == userId).Select(y => y).FirstOrDefault();
            var infoComp = dta.Company.Where(z => z.Id == id).Select(v => v).FirstOrDefault();
            ocenaCompanyModel ocenaModel = new ocenaCompanyModel();
            ocenaCompany ocenaC = new ocenaCompany();

            if(oc != null)
            {
                ocenaC = oc;
            }else
            {
                ocenaC.atmosfera = 50;
                ocenaC.awans = 50;
                ocenaC.companyId = 0;
                ocenaC.dodatki = 50;
                ocenaC.ksztalcenie = 50;
                ocenaC.mobbing = 50;
                ocenaC.uklady_ = 50;
                ocenaC.userId = "0";
                ocenaC.uznanie = 50;
                ocenaC.zarobki = 2500;
                ocenaC.socjal = 50;
                ocenaC.stres = 50;
            }

            ocenaModel.companyID = infoComp.Id;
            ocenaModel.companyName = infoComp.CompanyName;
            ocenaModel.adres = infoComp.Ulica + ", " + infoComp.Miejscowosc;
            ocenaModel.ocena = ocenaC;
            ocenaModel.userId = userId;
            if (przelozony != null)
            {
                ocenaModel.przelozonyId = przelozony;
            }else
            {
                ocenaModel.przelozonyId = 0;
            }

            return View(ocenaModel);
        }


        public JsonResult saveRatingCompany (int company, int[] data)
        {
            string userId = User.Identity.GetUserId();
            dataDB dta = new dataDB();
            var check = dta.ocenaCompany.Where(x => x.companyId == company && x.userId == userId).Select(y => y);

            var rate = dta.ratingCompany.Where(g => g.companyId == company).Select(z => z).FirstOrDefault();

            if (check.Count() != 0)
            {
                var update= dta.ocenaCompany.Where(x => x.companyId == company && x.userId == userId).Select(y => y).FirstOrDefault();
                var rte = dta.ratingCompany.Where(b => b.companyId == company).Select(m => m).FirstOrDefault();

                if (rte != null)
                {
                    rte.atmosfera -= update.atmosfera;
                    rte.awans -= update.awans;
                    rte.dodatki -= update.dodatki;
                    rte.ksztalcenie -= update.ksztalcenie;
                    rte.mobbing -= update.mobbing;
                    rte.socjal -= update.socjal;
                    rte.stres -= update.stres;
                    rte.uklady -= update.uklady_;
                    rte.uznanie -= update.uznanie;
                    rte.zarobki -= update.zarobki;
                    rte.ogolnyWynik -= update.zarobki + update.uznanie + update.uklady_ + update.stres + update.socjal + update.mobbing + update.ksztalcenie + update.dodatki + update.awans + update.atmosfera;
                }

                update.atmosfera = data[0];
                update.awans = data[1];
                update.dodatki = data[2];
                update.ksztalcenie = data[3];
                update.mobbing = data[4];
                update.uklady_ = data[5];
                update.uznanie = data[6];
                update.zarobki = data[7];
                update.socjal = data[8];
                update.stres = data[9];

                
                rte.atmosfera += data[0];
                rte.awans += data[1];
                rte.dodatki += data[2];
                rte.ksztalcenie += data[3];
                rte.mobbing += data[4];
                rte.socjal += data[8];
                rte.stres += data[9];
                rte.uklady += data[5];
                rte.uznanie += data[6];
                rte.zarobki += data[7];
                rte.ogolnyWynik += data.Sum();

                dta.SaveChanges();

            }
            else
            {
                ocenaCompany oc = new ocenaCompany();
                oc.atmosfera = data[0];
                oc.awans = data[1];
                oc.dodatki = data[2];
                oc.ksztalcenie = data[3];
                oc.mobbing = data[4];
                oc.uklady_ = data[5];
                oc.uznanie = data[6];
                oc.zarobki = data[7];
                oc.socjal = data[8];
                oc.stres = data[9];
                oc.userId = userId;
                oc.companyId = company;

                dta.ocenaCompany.Add(oc);

                if (rate != null)
                {
                    
                    rate.companyId = company;
                    rate.atmosfera += data[0];
                    rate.awans += data[1];
                    rate.dodatki += data[2];
                    rate.ksztalcenie += data[3];
                    rate.mobbing += data[4];
                    rate.uklady += data[5];
                    rate.uznanie += data[6];
                    rate.zarobki += data[7];
                    rate.socjal += data[8];
                    rate.stres += data[9];
                    rate.ogolnyWynik += data.Sum();
                    rate.liczbaVoice += 1;
                }else
                {
                    
                    ratingCompany rt = new ratingCompany();
                    rt.atmosfera = data[0];
                    rt.awans = data[1];
                    rt.dodatki = data[2];
                    rt.ksztalcenie = data[3];
                    rt.mobbing = data[4];
                    rt.uklady = data[5];
                    rt.uznanie = data[6];
                    rt.zarobki = data[7];
                    rt.socjal = data[8];
                    rt.stres = data[9];
                    rt.companyId = company;
                    rt.liczbaVoice = 1;
                    rt.ogolnyWynik = data.Sum() ;

                    dta.ratingCompany.Add(rt);
                }

                dta.SaveChanges();
                

            }

            return Json(JsonRequestBehavior.AllowGet);
        }


        public JsonResult saveRating(int idPrzelozony, int[] dane)
        {
            string userId = User.Identity.GetUserId().ToString();
            dataDB dt = new dataDB();
            var checkExist = dt.ocenaPrzelozony.Where(g => g.przelozonyId == idPrzelozony && g.userId == userId).Select(c => c);
            
            if (checkExist.Count() != 0)
            {
                var check = dt.ocenaPrzelozony.Where(g => g.przelozonyId == idPrzelozony && g.userId == userId).Select(c => c).FirstOrDefault();
                check.userId = userId;
                check.przelozonyId = idPrzelozony;
                check.kulturaOs = dane[0];
                check.inteligencja = dane[1];
                check.umiSluchania = dane[2];
                check.przyznanieBlad = dane[3];
                check.cwaniastwo = dane[4];
                check.udzielaniePochwal = dane[5];
                check.umieKomunikowania = dane[6];
                check.radzenieKrytyka = dane[7];
                check.rzetenaWiedza = dane[8];

                dt.SaveChanges();
            }else
            {
                ocenaPrzelozony op = new ocenaPrzelozony();

                op.userId = userId;
                op.przelozonyId = idPrzelozony;
                op.kulturaOs = dane[0];
                op.inteligencja = dane[1];
                op.umiSluchania = dane[2];
                op.przyznanieBlad = dane[3];
                op.cwaniastwo = dane[4];
                op.udzielaniePochwal = dane[5];
                op.umieKomunikowania = dane[6];
                op.radzenieKrytyka = dane[7];
                op.rzetenaWiedza = dane[8];


                dt.ocenaPrzelozony.Add(op);
                dt.SaveChanges();
            }



            return Json(JsonRequestBehavior.AllowGet);
        }


        public JsonResult getCompRating (int getData)
        {
            
            dataDB dt = new dataDB();
            int?[] sendData = new int?[2];

            var dane = dt.ratingCompany.Where(x => x.companyId == getData).Select(y => y).FirstOrDefault() ;
            if(dane != null)
            {
                  
                sendData[0] = ((((dane.ogolnyWynik - dane.zarobki) - (dane.mobbing + dane.uklady + dane.stres) * 2) + ((dane.zarobki  * 100) / 20000)) / dane.liczbaVoice);
                sendData[1] = dane.liczbaVoice;
           }
            else
            {
                sendData[0] = 0;
                sendData[1] = 0;
            }

            return Json(sendData ,JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMenRat (int getD)
        {
            long? suma = 0;
            dataDB data = new dataDB();
            var dta = data.ocenaPrzelozony.Where(x => x.przelozonyId == getD).Select(y => y);

            foreach (var z in dta)
            {
                suma = suma + (z.inteligencja + z.kulturaOs + z.przyznanieBlad + z.radzenieKrytyka + z.rzetenaWiedza + z.udzielaniePochwal + z.umieKomunikowania + z.umiSluchania) - z.cwaniastwo;
            }
            if(suma != 0)
            {
                suma = suma / dta.Count();
            }

            long?[] dane = new long?[2];
            dane[0] = suma;
            dane[1] = dta.Count();

            return Json(dane, JsonRequestBehavior.AllowGet);
        }

    }
}