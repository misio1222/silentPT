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
using System.Text.RegularExpressions;
using LumenWorks.Framework.IO;
using LumenWorks.Framework.IO.Csv;

namespace MVCIdentityConfirm.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index(int? DodajId, string message, int? powrot, int? cid)
        {

            
            if (DodajId != null)
            {
                string useId = User.Identity.GetUserId();
                dataDB add = new dataDB();
                var chec = add.UsersCompany.Where(x => x.CompanyID == DodajId && x.UserID == useId).Select(z => z).Count();

                if (chec == 0)
                {
                    UsersCompany uCompany = new UsersCompany
                    {
                        UserID = User.Identity.GetUserId(),
                        CompanyID = (int)DodajId,
                        WydzialID = null


                    };

                    add.UsersCompany.Add(uCompany);
                    add.SaveChanges();
                    add.Dispose();
                }else
                {
                    ViewBag.ErrorNoSelect = "Firma jest już na Twojej liście!";
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.ErrorNoSelect = message;
            }
            string uId = User.Identity.GetUserId();
           
            dataDB data = new dataDB();
            List<SelectListItem> CompanyList = new List<SelectListItem>();

            var usersCompany = data.UsersCompany.Where(z => z.UserID == uId).Select(r => r);
                
            if (usersCompany.Count() != 0)
            {
                foreach (var users in usersCompany)
                {
                    var company = data.Company.Where(x => x.Id == users.CompanyID).Select(a => a);
                    foreach (Company c in company)
                    {
                        SelectListItem selectListItem = new SelectListItem()
                        {
                            Text = c.CompanyName + ", " + c.Miejscowosc,
                            Value = c.Id.ToString(),
                            Selected = (bool)c.IsSelected
                        };
                        CompanyList.Add(selectListItem);
                    }
                }
            }else
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = "Nie dodałeś żadnej firmy",
                    Value = "0",
                    Selected = false
                };
                CompanyList.Add(selectListItem);
            }
            CompanyViewModel companyViewModel = new CompanyViewModel();
            companyViewModel.Company = CompanyList;
            //int? page = 1;
            

            if (powrot != null && powrot != 0 )
            {
                dataDB dba = new dataDB();
                var wydzial = dba.Wydzial.Where(f => f.PrzelozonyId == powrot).Select(r => r).FirstOrDefault();
                companyViewModel.powrotcompany = wydzial.CompanyID;
                companyViewModel.powrotWczesniej = wydzial.Id;
            }
            else
            {
                companyViewModel.powrotWczesniej = 0;
                companyViewModel.powrotcompany = 0;
            }
            if (cid != null && cid != 0)
            {
                companyViewModel.powrotcompany = cid;
                companyViewModel.powrotWczesniej = 0;
                return View(companyViewModel);
            }


            //companyViewModel.wypowiedz = data.wypowiedzUser.Where(x => x.userId == "b61575c5-2313-47a2-8639-371e501b1ba1").Select(v => v).OrderByDescending(k => k.Id).ToList().ToPagedList(page ?? 1, 3);
            return View(companyViewModel);
        }





        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult ChooseCompany()
        {
            dataDB db = new dataDB();
            List<SelectListItem> CompanyList = new List<SelectListItem>();

            foreach (Company comp in db.Company)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = comp.CompanyName + ", " + comp.Miejscowosc,
                    Value = comp.Id.ToString(),
                    Selected = (bool)comp.IsSelected
                };
                CompanyList.Add(selectListItem);

            }
            CompanyViewModel companyViewModel = new CompanyViewModel();
            companyViewModel.Company = CompanyList;


            return View(companyViewModel);
        }

        CompanyViewModel chComp()
        {
            dataDB db = new dataDB();
            List<SelectListItem> CompanyList = new List<SelectListItem>();

            foreach (Company comp in db.Company)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = comp.CompanyName + ", " + comp.Miejscowosc,
                    Value = comp.Id.ToString(),
                    Selected = (bool)comp.IsSelected
                };
                CompanyList.Add(selectListItem);

            }
            CompanyViewModel companyViewModel = new CompanyViewModel();
            companyViewModel.Company = CompanyList;


            return companyViewModel;
        }


        [HttpPost]
        public ActionResult ChooseCompany(IEnumerable<string> selectedCompany, string serch, CompanyViewModel model)
        {
            CompanyViewModel companyViewModel = new CompanyViewModel();
            dataDB db = new dataDB();
            string str = Request.Params["btn"];
            if (str == "SZUKAJ")
            {
                if (string.IsNullOrEmpty(serch))
                {
                    List<SelectListItem> CompanyList = new List<SelectListItem>();

                    foreach (Company comp in db.Company)
                    {
                        SelectListItem selectListItem = new SelectListItem()
                        {
                            Text = comp.CompanyName + ", " + comp.Miejscowosc,
                            Value = comp.Id.ToString(),
                            Selected = (bool)comp.IsSelected
                        };
                        CompanyList.Add(selectListItem);

                    }
                    //CompanyViewModel companyViewModel = new CompanyViewModel();
                    companyViewModel.Company = CompanyList;


                    return View(companyViewModel);
                }
                else
                {

                    List<SelectListItem> CompanyList = new List<SelectListItem>();

                    foreach (Company comp in db.Company.Where(x => x.CompanyName.StartsWith(serch)))
                    {
                        SelectListItem selectListItem = new SelectListItem()
                        {
                            Text = comp.CompanyName + ", " + comp.Miejscowosc,
                            Value = comp.Id.ToString(),
                            Selected = (bool)comp.IsSelected
                        };
                        CompanyList.Add(selectListItem);

                    }

                    companyViewModel.Company = CompanyList;


                    return View(companyViewModel);
                }
            }
            if (str == "WYBIERZ")
            {
                if (ModelState.IsValid)
                {
                    if (selectedCompany == null)
                    {
                        ViewBag.Marcin = "Proszę wybrać przynajmniej jedną z firm!";
                        //ModelState.AddModelError("", "Wybierz chociaż jedną firmę z listy");
                    }
                    else
                    {
                        dataDB dbBase = new dataDB();
                        string UId = User.Identity.GetUserId();
                        int test = 0;

                        var selComp = dbBase.UsersCompany.Where(y => y.UserID == UId).Select(z => z);



                        foreach (string x in selectedCompany)
                        {
                            int exi = selComp.Where(c => c.CompanyID.ToString() == x).Select(m => m).Count();
                            if (exi != 0)
                            {

                                ViewBag.Marcin = "Firma jest już przypisana do twojego konta!";
                                return View(chComp());
                            }
                            else
                            {
                                UsersCompany UserC = new UsersCompany();
                                UserC.CompanyID = Int32.Parse(x.ToString());
                                UserC.UserID = UId;
                                dbBase.UsersCompany.Add(UserC);
                            }

                        }

                        dbBase.SaveChanges();
                        dbBase.Dispose();

                        //ApplicationUser user = new ApplicationUser();
                        //string id = User.Identity.GetUserId();
                        //StringBuilder sb = new StringBuilder();
                        //sb.Append("You selected " + string.Join(", ", selectedCompany));
                        //ViewBag.kotek = id;  //sb.ToString();
                        return RedirectToAction("Index","Home");
                    }
                }
            }

            if (str == "DODAJ")
            {
                //try
                //{
                dataDB data = new dataDB();

                var check = from x in data.Company where x.CompanyName == model.createModel.CompanyName && x.Miejscowosc == model.createModel.Miejscowosc select x;

                if (check.Count() == 0)
                {
                    if (ModelState.IsValid)
                    {



                        if (model.createModel != null)
                        {
                            Company comp = new Company
                            {
                                CompanyName = model.createModel.CompanyName.ToUpper(),
                                IsSelected = false,
                                Miejscowosc = model.createModel.Miejscowosc,
                                Ulica = model.createModel.Ulica,
                                Wojewodztwo = model.createModel.Wojewodztwo,
                                NIP = model.createModel.NIP,
                                Regon = model.createModel.Regon,

                            };
                            if (model.createModel.Logo != null)
                            {
                                string extension = Path.GetExtension(model.createModel.Logo.FileName);
                                if (extension == ".png" || extension == ".jpg")
                                {
                                    comp.Logo = imageToByteArray(model.createModel.Logo);
                                }
                                else { ViewBag.Exist = "Możesz dodać tylko zdjęcie (.png .jpg)"; }
                            }

                            data.Company.Add(comp);
                            data.SaveChanges();

                            data.Dispose();
                            ViewBag.Exist = "Firma została dodana do listy. ";

                            return View(chComp());
                        }

                    }
                    else { ModelState.AddModelError("", "Proszę wypenić wszystkie pola."); }
                }

                else { ViewBag.Exist = "Firma którą chcesz dodać już istnieje!"; }
                //}
                //catch { ViewBag.Exist = "Plik jest zbyt duży, max. 2MB."; }
            }

            return View(chComp());
        }







        public byte[] imageToByteArray(HttpPostedFileBase imageIn)
        {


            byte[] data = new byte[imageIn.ContentLength];
            imageIn.InputStream.Read(data, 0, imageIn.ContentLength);

            Image x = (Bitmap)((new ImageConverter()).ConvertFrom(data));

            neededClass nClass = new neededClass();
            var newImage = nClass.resizeImage(150, 150, x);

            ImageConverter converter = new ImageConverter();
            byte[] imgArray = (byte[])converter.ConvertTo(newImage, typeof(byte[]));


            return imgArray;

        }

        [Authorize]
        public ActionResult searchResult(string serch)
        {
            
            dataDB db = new dataDB();
            List<SelectListItem> CompanyList = new List<SelectListItem>();

            var searched = from z in db.Company where z.CompanyName.StartsWith(serch) select z;

           

            foreach (Company comp in searched)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = comp.CompanyName + ", " + comp.Miejscowosc,
                    Value = comp.Id.ToString(),
                    Selected = (bool)comp.IsSelected
                };
                CompanyList.Add(selectListItem);

            }
            CompanyViewModel companyViewModel = new CompanyViewModel();
            companyViewModel.Company = CompanyList;


            return View(companyViewModel);
        }



        
        //public ActionResult searchResult(IEnumerable<string> selectedCompany)
        //{
        //    if (selectedCompany != null)
        //    {
        //        foreach (var v in selectedCompany)
        //        {
        //            var data = new dataDB();
        //            int cmpId = Int32.Parse(v);
        //            var check = data.Company.Where(x => x.Id == cmpId).Select(z => z).Any();
        //            if (check)
        //            {
        //                return RedirectToAction("Index", "Home", new { id = v });

        //            }
        //        }


        //    }
        //    return RedirectToAction("Index", "Home", new { id = 1 });
        //}




        [Authorize]
        public ActionResult CompanyInfo(int id)
        {

            dataDB companyData = new dataDB();
            string imageBase64Data;

            var compInfo = companyData.Company.Where(z => z.Id == id).Select(x => x);
            CompInformation cmpInfo = new CompInformation();
            Company xyz = new Company();

            foreach (var z in compInfo)
            {
                xyz = z;
            }
            var uId = User.Identity.GetUserId();
            var ratData = companyData.ratingCompany.Where(b => b.companyId == id).Select(k => k).FirstOrDefault();
            var added = companyData.UsersCompany.Where(ml =>  ml.UserID == uId && ml.CompanyID == id).Select(lk => lk).Any();
            var liczbaPracownikow = companyData.UsersCompany.Where(vb => vb.CompanyID == id).Select(zxc => zxc).Count();
            if (ratData != null)
            {
                if (ratData.atmosfera != 0)
                {
                    cmpInfo.atmosfera = (ratData.atmosfera / ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.atmosfera = 0;
                }
                if (ratData.awans != 0)
                {
                    cmpInfo.awans = (ratData.awans / ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.awans = 0;
                }
                if (ratData.dodatki != 0)
                {
                    cmpInfo.dodatki = (ratData.dodatki/ ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.dodatki = 0;
                }
                if (ratData.ksztalcenie != 0)
                {
                    cmpInfo.ksztalcenie = (ratData.ksztalcenie / ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.ksztalcenie = 0;
                }
                if (ratData.mobbing != 0)
                {
                    cmpInfo.mobbing = (ratData.mobbing / ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.mobbing = 0;
                }
                if (ratData.socjal != 0)
                {
                    cmpInfo.socjal = (ratData.socjal / ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.socjal = 0;
                }
                if (ratData.stres != 0)
                {
                    cmpInfo.stres = (ratData.stres / ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.stres = 0;
                }
                if (ratData.uklady != 0)
                {
                    cmpInfo.uklady = (ratData.uklady / ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.uklady = 0;
                }
                if (ratData.uznanie != 0)
                {
                    cmpInfo.uznanie = (ratData.uznanie/ ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.uznanie = 0;
                }
                if (ratData.zarobki != 0)
                {
                    cmpInfo.zarobki = (ratData.zarobki / ratData.liczbaVoice);
                }
                else
                {
                    cmpInfo.zarobki = 0;
                }
                if (ratData.zarobki != 0)
                {
                    cmpInfo.zarobkiProcent = (ratData.zarobki / ratData.liczbaVoice) ;
                }
                else
                {
                    cmpInfo.zarobkiProcent = 0;
                }
                cmpInfo.liczbaGlosow = ratData.liczbaVoice;
                
            }
            cmpInfo.isAdded = added;
            cmpInfo.liczPracownikow = liczbaPracownikow;
            cmpInfo.comp = xyz;

            if (xyz.Logo == null)
            {
                string path = Path.Combine(Server.MapPath("~/images/dlogo.png"));
                Image img = Image.FromFile(path);
                neededClass nClass = new neededClass();
                var nImg = nClass.resizeImage(150, 150, img);
                ImageConverter converter = new ImageConverter();
                byte[] imgArray = (byte[])converter.ConvertTo(nImg, typeof(byte[]));
                imageBase64Data = Convert.ToBase64String(imgArray);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                ViewBag.ImageData = imageDataURL;
            }
            else
            {
                imageBase64Data = Convert.ToBase64String(xyz.Logo);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                ViewBag.ImageData = imageDataURL;
            }

            return View(cmpInfo);
        }


        public JsonResult GetCompany(string term)
        {
            dataDB data = new dataDB();

            List<string> company;
            company = data.Company.Where(x => x.CompanyName.StartsWith(term)).Select(z => z.CompanyName).Distinct().ToList();

            return Json(company.Take(10), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getWydzialInfo (int idWydzial)
        {
            dataDB data = new dataDB();

            var dt = data.Wydzial.Where(x => x.Id == idWydzial).Select(y => y).FirstOrDefault();
            var przeloz = data.Przelozony.Where(z => z.Id == dt.PrzelozonyId).Select(u => u).FirstOrDefault();


            
            wydzialModClass wyd = new wydzialModClass();
            List<wypoModel> listWypo = new List<wypoModel>();            

            wyd.wydzialName = dt.Wydzial1;
            wyd.przelozony = przeloz.Nazwisko + " " + przeloz.Imie;
            wyd.stanowisko = przeloz.Stanowisko;
            wyd.przelozonyID = przeloz.Id;
            return Json(wyd, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAnswers(int idWydzial)
        {
            dataDB data = new dataDB();
            var wypow = data.wypowiedzUser.Where(b => b.wydzialId== idWydzial).OrderByDescending(r => r.Id).Select(v => v);
            List<wypoModel> listWypo = new List<wypoModel>();
            
            if (wypow != null)
            {
                foreach (var c in wypow)
                {
                    string uName = "";
                    if (c.NameOrLogin)
                    {
                        uName = c.userImie + " " + c.userNazwisko;
                    }
                    else
                    {
                        uName = c.userLogin;
                    }

                    string mId = User.Identity.GetUserId();

                    checkWulgar cWulgar = new checkWulgar();

                    

                    wypoModel komentarz = new wypoModel
                    {
                        companyId = c.companyId,
                        content = cWulgar.sprawdzam(c.content),
                        id = c.Id,
                        logOrName = c.NameOrLogin,
                        userId = c.userId,
                        userName = uName,
                        wydzialId = c.wydzialId,
                        mojeId = mId,
                        DateTi = c.dateTime.ToString(),
                        like = c.like,
                        notLike = c.notLike

                    };

                    listWypo.Add(komentarz);
                }
            }
            return Json(listWypo, JsonRequestBehavior.AllowGet);
        }




        public JsonResult getLike (int wypoId)
        {
            
            dataDB dt = new dataDB();
            string user = User.Identity.GetUserId();
            int notLikeNumber = dt.wypowiedzUser.Where(y => y.Id == wypoId).Select(f => f.notLike).FirstOrDefault();
            int likeNumber = dt.wypowiedzUser.Where(y => y.Id == wypoId).Select(f => f.like).FirstOrDefault();
            var check = dt.Likes.Where(x => x.wypowiedzID == wypoId && x.userId == user).Select(z => z).Any();
            if(!check)
            {
                var data = dt.wypowiedzUser.Where(y => y.Id == wypoId).Select(f => f).FirstOrDefault();
                data.like += 1;
                likeNumber += 1;

                Likes lk = new Likes {
                    userId = user,
                    wypowiedzID = wypoId,
                    likeOrNot=true
                };

                dt.Likes.Add(lk);
                dt.SaveChanges();
            }
            if (check)
            {
                var choice = dt.Likes.Where(x => x.wypowiedzID == wypoId && x.userId == user && x.likeOrNot == false).Select(z => z).Any();
                if (choice)
                {
                    var data = dt.wypowiedzUser.Where(y => y.Id == wypoId).Select(f => f).FirstOrDefault();
                    data.notLike -= 1;
                    notLikeNumber -= 1;
                    data.like += 1;
                    likeNumber += 1;

                    var lk = dt.Likes.Where(x => x.wypowiedzID == wypoId && x.userId == user && x.likeOrNot == false).Select(z => z).FirstOrDefault();
                    lk.userId = user;
                    lk.wypowiedzID = wypoId;
                    lk.likeOrNot = true;
                    dt.SaveChanges();
                }
            }
            dt.Dispose();
            addedLikes aLNot = new addedLikes
            {
                alreadyAdd = check,
                likesInt = likeNumber,
                notLikesInt = notLikeNumber,
            };

            return Json(aLNot, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getNoLike(int wypoId)
        {
            dataDB dt = new dataDB();
            string user = User.Identity.GetUserId();
            int notLikeNumber = dt.wypowiedzUser.Where(y => y.Id == wypoId).Select(f => f.notLike).FirstOrDefault();
            int likeNumber = dt.wypowiedzUser.Where(y => y.Id == wypoId).Select(f => f.like).FirstOrDefault();
            var check = dt.Likes.Where(x => x.wypowiedzID == wypoId && x.userId == user).Select(z => z).Any();
            if (!check)
            {
                var data = dt.wypowiedzUser.Where(y => y.Id == wypoId).Select(f => f).FirstOrDefault();
                data.notLike += 1;
                notLikeNumber += 1;

                Likes lk = new Likes
                {
                    userId = user,
                    wypowiedzID = wypoId,
                    likeOrNot = false
                };

                dt.Likes.Add(lk);
                dt.SaveChanges();
            }
            if (check)
            {
                var choice = dt.Likes.Where(x => x.wypowiedzID == wypoId && x.userId == user && x.likeOrNot == true).Select(z => z).Any();
                if (choice)
                {
                    var data = dt.wypowiedzUser.Where(y => y.Id == wypoId).Select(f => f).FirstOrDefault();
                    data.notLike += 1;
                    notLikeNumber += 1;
                    data.like -= 1;
                    likeNumber -= 1;

                    var lk = dt.Likes.Where(x => x.wypowiedzID == wypoId && x.userId == user && x.likeOrNot == true).Select(z => z).FirstOrDefault();
                    lk.userId = user;
                    lk.wypowiedzID = wypoId;
                    lk.likeOrNot = false;
                    dt.SaveChanges();
                }
            }
            dt.Dispose();
            addedLikes aLNot = new addedLikes
            {
                alreadyAdd = check,
                likesInt = likeNumber,
                notLikesInt = notLikeNumber,
            };
            return Json(aLNot, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult changeLike(int wypoId, bool like)
        //{

        //    dataDB dt = new dataDB();
        //    string user = User.Identity.GetUserId();
        //    int notLikeNumber = dt.wypowiedzUser.Where(t => t.Id == wypoId).Select(t => t.notLike).FirstOrDefault();
        //    int likeNumber = dt.wypowiedzUser.Where(p => p.Id == wypoId).Select(w => w.like).FirstOrDefault();
        //    var check = dt.Likes.Where(x => x.wypowiedzID == wypoId && x.userId == user && x.likeOrNot != like).Select(z => z).Any();
        //    if (check)
        //    {
        //        var data = dt.wypowiedzUser.Where(y => y.Id == wypoId).Select(f => f).FirstOrDefault();
        //        data.like -= 1;
        //        likeNumber -= 1;
        //        notLikeNumber += 1;
        //        data.notLike += 1;
                
        //        Likes lk = new Likes
        //        {
        //            userId = user,
        //            wypowiedzID = wypoId,
        //            likeOrNot = false
        //        };

        //        dt.Likes.Add(lk);
        //        dt.SaveChanges();
        //    }
        //    dt.Dispose();

        //    changLike chL = new changLike {
        //        likeInt = likeNumber,
        //        notLike = notLikeNumber
        //    };


        //    return Json(chL, JsonRequestBehavior.AllowGet);
        //}



        [ValidateInput(false)]
        public JsonResult saveAnsware (string userAns, int[] compAndWydzial, bool checkedLog)
        {
            dataDB data = new dataDB();
            userDB usr = new userDB();
            string userId = User.Identity.GetUserId();
            var user = usr.AspNetUsers.Where(x => x.Id == userId).Select(y => y).First();
            int idCom = compAndWydzial[0];
            int idWydz = compAndWydzial[1];
            wypowiedzUser wypowiedz = new wypowiedzUser
            {
                companyId = idCom,
                content =  userAns ,
                userId = userId,
                userImie = user.Imie,
                userNazwisko = user.Nazwisko,
                userLogin = user.UserName,
                wydzialId= idWydz,
                NameOrLogin = checkedLog,
                dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                like = 0,
                notLike = 0
            };

            data.wypowiedzUser.Add(wypowiedz);
            data.SaveChanges();
                    
            var wypow = data.wypowiedzUser.Where(b => b.wydzialId== idWydz).OrderByDescending(r => r.Id).Select(v => v);
            List<wypoModel> listWypo = new List<wypoModel>();
            
             
            if (wypow != null)
            {
                foreach (var c in wypow)
                {
                    string uName = "";
                    if (c.NameOrLogin)
                    {
                        uName = c.userImie + " " + c.userNazwisko;
                    }
                    else
                    {
                        uName = c.userLogin;
                    }

                    string mId = User.Identity.GetUserId();

                    checkWulgar cWulgar = new checkWulgar();

                    wypoModel komentarz = new wypoModel
                    {
                        companyId = c.companyId,
                        content = cWulgar.sprawdzam(c.content),
                        id = c.Id,
                        logOrName = c.NameOrLogin,
                        userId = c.userId,
                        userName = uName,
                        wydzialId = c.wydzialId,
                        mojeId = mId,
                        DateTi = c.dateTime.ToString(),
                        like = c.like,
                        notLike = c.notLike
                    };

                    listWypo.Add(komentarz);
                }
            }


            return Json(listWypo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult delAnsware (int idAnswer, int idWydz)
        {
            dataDB td = new dataDB();
            var usr = User.Identity.GetUserId();
            List<wypoModel> listWypo = new List<wypoModel>();
            var getAns = td.wypowiedzUser.Where(x => x.Id == idAnswer).Select(y => y).FirstOrDefault();
            var remZgl = td.zgloszenieNar.Where(v => v.idAnswear == idAnswer).Select(g => g).FirstOrDefault();
            
                if (getAns != null)
                {
                    td.wypowiedzUser.Remove(getAns);
                    td.SaveChanges();
                    var wypow = td.wypowiedzUser.Where(b => b.wydzialId == idWydz).OrderByDescending(r => r.Id).Select(v => v);
                    if (wypow != null)
                    {
                    foreach (var c in wypow)
                    {
                        string uName = "";
                        if (c.NameOrLogin)
                        {
                            uName = c.userImie + " " + c.userNazwisko;
                        }
                        else
                        {
                            uName = c.userLogin;
                        }

                        string mId = User.Identity.GetUserId();

                        checkWulgar cWulgar = new checkWulgar();

                        wypoModel komentarz = new wypoModel
                        {
                            companyId = c.companyId,
                            content = cWulgar.sprawdzam(c.content),
                            id = c.Id,
                            logOrName = c.NameOrLogin,
                            userId = c.userId,
                            userName = uName,
                            wydzialId = c.wydzialId,
                            mojeId = mId,
                            DateTi = c.dateTime.ToString(),
                            like = c.like,
                            notLike = c.notLike
                        };

                        listWypo.Add(komentarz);
                    }
                    
                }
            }
                if(remZgl != null)
            {
                td.zgloszenieNar.Remove(remZgl);
                td.SaveChanges();
                td.Dispose();
            }
            return Json(listWypo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult edtAns (int idAnswer)
        {
            dataDB dta = new dataDB();
            var edited = dta.wypowiedzUser.Where(x => x.Id == idAnswer).Select(y => y).FirstOrDefault();
            string cont = edited.content;
            return Json(cont, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public JsonResult updAns(  int idWydz, string newVal, int idAn, bool checkd)
        {

            var usr = User.Identity.GetUserId();
            dataDB td = new dataDB();
            List<wypoModel> listWypo = new List<wypoModel>();
            var getAns = td.wypowiedzUser.Where(x => x.Id == idAn).Select(y => y).First();

            if (usr == getAns.userId && User.Identity.IsAuthenticated)
            {

                getAns.NameOrLogin = checkd;
                getAns.content = newVal;
                td.SaveChanges();
                td.Dispose();

                dataDB tdi = new dataDB();

                var wypow = tdi.wypowiedzUser.Where(b => b.wydzialId == idWydz).OrderByDescending(r => r.Id).Select(v => v);
                if (wypow != null)
                {
                    foreach (var c in wypow)
                    {
                        string uName = "";
                        if (c.NameOrLogin)
                        {
                            uName = c.userImie + " " + c.userNazwisko;
                        }
                        else
                        {
                            uName = c.userLogin;
                        }

                        string mId = User.Identity.GetUserId();
                        checkWulgar cWulgar = new checkWulgar();
                        wypoModel komentarz = new wypoModel
                        {
                            companyId = c.companyId,
                            content = cWulgar.sprawdzam(c.content),
                            id = c.Id,
                            logOrName = c.NameOrLogin,
                            userId = c.userId,
                            userName = uName,
                            wydzialId = c.wydzialId,
                            mojeId = mId,
                            DateTi = c.dateTime.ToString(),
                            like = c.like,
                            notLike = c.notLike
                        };

                        listWypo.Add(komentarz);

                    }
                }
            }

                return Json(listWypo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCompanyInfo(int idSelect)
        {
            dataDB dt = new dataDB();
            string imageBase64Data;
            string imageDataURL;

            Company cmp = dt.Company.Where(z => z.Id == idSelect).Select(x => x).FirstOrDefault();

            List<SelectListItem> list = new List<SelectListItem>();

            var wdz = dt.Wydzial.Where(v => v.CompanyID == idSelect).Select(g => g);

            foreach (var ds in wdz)
            {
                var zarzadza = dt.Przelozony.Where(m => m.Id == ds.PrzelozonyId).Select(bv => bv.Nazwisko).First();
                SelectListItem sItem = new SelectListItem()
                {
                    Text = ds.Wydzial1 + " ( " + zarzadza + " )",
                    Value = ds.Id.ToString(),
                    
                };
                list.Add(sItem);

            }

            if (cmp.Logo == null)
            {
                string path = Path.Combine(Server.MapPath("~/images/dlogo.png"));
                Image img = Image.FromFile(path);
                neededClass nClass = new neededClass();
                var nImg = nClass.resizeImage(220, 200, img);
                ImageConverter converter = new ImageConverter();
                byte[] imgArray = (byte[])converter.ConvertTo(nImg, typeof(byte[]));
                imageBase64Data = Convert.ToBase64String(imgArray);
                imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                //ViewBag.ImageData = imageDataURL;
            }
            else
            {
                imageBase64Data = Convert.ToBase64String(cmp.Logo);
                imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                
            }


           



            IndxComp idx = new IndxComp {
                cmpIn = cmp,
                img = imageDataURL,
                wydzial = list,
               

            };
            return Json(idx, JsonRequestBehavior.AllowGet);
        }


        public JsonResult zgloszenie(int idAns)
        {
            bool ok = false;
            try
            {
                dataDB dt = new dataDB();
                var check = dt.zgloszenieNar.Where(t => t.idAnswear == idAns).Select(m => m).Count();
                if (check == 0)
                {
                    var usr = dt.wypowiedzUser.Where(x => x.Id == idAns).Select(y => y).FirstOrDefault();
                    zgloszenieNar zgl = new zgloszenieNar()
                    {
                        idAnswear = idAns,
                        userId = usr.userId
                    };

                    dt.zgloszenieNar.Add(zgl);
                    dt.SaveChanges();
                    ok = true;
                }else
                {
                    ok = false;
                }
            }catch
            {
                ok = false;
            }

            return Json(ok, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]                                                                        dodanie działu poprzednie 
        //public ActionResult Index(IEnumerable<string> selectedCompany)
        //{
        //    string a = "";
        //    if (selectedCompany != null)
        //    {
        //        a = selectedCompany.Select(z => z).First();
        //        return RedirectToAction("AddWydzial", new { comp = a });
        //    }else
        //    {
        //        string id = null;
        //        string message = "Najpierw wybierz firmę!";
        //        return RedirectToAction("Index", new { id, message });
        //    }
            
        //}
        [Authorize]
        public ActionResult AddWydzial(string comp)
        {
            var data = new dataDB();
            comModel cmp = new comModel();
            if (!String.IsNullOrEmpty(comp))
            {
                int compNew = Int32.Parse(comp);
                var companyInfo = data.Company.Where(x => x.Id == compNew).Select(z => z).FirstOrDefault();
                
                cmp.companyMod = companyInfo;
                return View(cmp);
            }

            return View();
        }

        [HttpPost]
        public JsonResult AddWydzial(string[] data, string compId)
        {
            int przelozonyId = 0;
            if (!string.IsNullOrEmpty(compId))
            {
                var wydRec = data[0].ToString();
                var imieRec = data[1].ToString();
                var nazwiskoRec = data[2].ToString();
                var stanowiskoRec = data[3].ToString();

                        dataDB dataDBase = new dataDB();
                        var przelozonyCheck = dataDBase.Przelozony.Where(n => n.Imie.ToUpper() == imieRec.ToUpper() && n.Nazwisko.ToUpper() == nazwiskoRec.ToUpper() && n.Stanowisko.ToUpper() == stanowiskoRec.ToUpper()).Select(u => u.Id).FirstOrDefault();
                        var checkModel = dataDBase.Wydzial.Where(v => v.Wydzial1 == wydRec.ToUpper() && v.PrzelozonyId == przelozonyCheck).Select(y => y).FirstOrDefault();
                        if (checkModel == null)
                        {
                            Przelozony przel = new Przelozony
                            {

                                Imie = imieRec.ToUpper(),
                                Nazwisko = nazwiskoRec.ToUpper(),
                                Stanowisko = stanowiskoRec.ToUpper(),
                                Dodalid = User.Identity.GetUserId().ToString()
                            };
                            dataDBase.Przelozony.Add(przel);
                            dataDBase.SaveChanges();
                            przelozonyId = przel.Id;
                            dataDBase.Dispose();
                            dataDB dt = new dataDB();

                            Wydzial wd = new Wydzial
                            {
                                CompanyID = Int32.Parse(compId),
                                Wydzial1 = wydRec.ToUpper(),
                                PrzelozonyId = przelozonyId,
                                DodalId = User.Identity.GetUserId().ToString()
                            };

                            dt.Wydzial.Add(wd);
                            dt.SaveChanges();
                            dt.Dispose();
                        }
                        else
                        {

                            
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }




     
            return Json(compId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult test_2(int? page)
        {

            using (CsvReader cRead = new CsvReader(new StreamReader("data.csv"), true)
                {
                    

                }
            ) ;
            
            dataDB data = new dataDB();
            string mojeId = User.Identity.GetUserId().ToString();
             
            return View(data.wypowiedzUser.Where(x => x.userId == mojeId).Select(v => v).OrderByDescending(k => k.Id).ToList().ToPagedList(page ?? 1, 20));
        }

        public ActionResult listAns( int? idWyd, int? page)
        {
            dataDB data = new dataDB();
            string mojeId = User.Identity.GetUserId().ToString();
            
            if (idWyd == null) {
                idWyd = 1;
                return PartialView("listAns", data.wypowiedzUser.Where(x => x.userId == 0.ToString() && x.wydzialId == idWyd).Select(v => v).OrderByDescending(k => k.Id).ToList().ToPagedList(page ?? 1, 3));
            }
            else
            {
                var dta = data.wypowiedzUser.Where(x => x.userId == mojeId && x.wydzialId == idWyd).Select(v => v).OrderByDescending(k => k.Id).ToList().ToPagedList(page ?? 1, 3);
                if (dta == null)
                {
                    return PartialView("listAns", data.wypowiedzUser.Where(x => x.userId == 0.ToString()).Select(v => v).OrderByDescending(k => k.Id).ToList().ToPagedList(page ?? 1, 3));
                }
                else
                {
                    return PartialView("listAns", dta);
                }
            }
            
            //var pr = data.wypowiedzUser.Where(x => x.userId == mojeId && x.wydzialId == wydzialId).Select(v => v).OrderByDescending(k => k.Id).ToList().ToPagedList(page ?? 1, 20);
           
        }

        bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }


        [Authorize]
        public ActionResult addEmail (int companyId)
        {
            var data = new dataDB();
            var dane = data.Company.Where(x => x.Id == companyId).Select(z => z).FirstOrDefault();

            if (dane != null)
            {
                var compInfo = new addEmailCompany
                {
                    companyId = dane.Id,
                    companyName = dane.CompanyName
                };
                return View(compInfo);
            }else
            {
                var usr = User.Identity.GetUserId();
                var dt = new userDB();
                var getName = dt.AspNetUsers.Where(v => v.Id == usr).Select(m => m).FirstOrDefault();
                var compInfo = new addEmailCompany
                {
                    companyId = 0,
                    companyName = getName.Imie + " " + getName.Nazwisko + " NIE KOMBINUJ!!! Pierwsze OSTRZEŻENIE! Jeszcze jedno i BAN!!!!!!! "
                };
                return View(compInfo);
            }
        }

        public JsonResult checkEmail(int comp, string email, string stanowisko)
        {
            var cheked = false;
            if (comp != 0)
            {
                var userId = User.Identity.GetUserId();
                var data = new dataDB();
                var userHaveComp = data.UsersCompany.Where(c => c.CompanyID == comp).Select(z => z).Any();
                
                if (userHaveComp == true)
                {
                    if (email.Length > 5)
                    {
                        if (IsValidEmail(email))
                        {
                            cheked = true;
                        }
                    }
                }
            }

            return Json(cheked, JsonRequestBehavior.AllowGet);
        }


        public JsonResult companyList()
        {
            
            string userID_new = User.Identity.GetUserId();
            dataDB dt_new = new dataDB();
            var checkingCompany = dt_new.UsersCompany.Where(op => op.UserID == userID_new).Select(x => x).Any();
            if (checkingCompany)
            {
                List<SelectListItem> listaCpompany = new List<SelectListItem>();
                var cnyList = dt_new.UsersCompany.Where(z => z.UserID == userID_new).Select(x => x);
                if (cnyList != null)
                {
                    foreach (var c in cnyList)
                    {
                        var companyData = dt_new.Company.Where(g => g.Id == c.CompanyID).Select(h => h).First();
                        SelectListItem cxItem = new SelectListItem()
                        {
                            Text = companyData.CompanyName + ", " + companyData.Miejscowosc,
                            Value = companyData.Id.ToString()

                        };
                        listaCpompany.Add(cxItem);

                    }


                    ComLyoutModel clm = new ComLyoutModel()
                    {
                        firma = listaCpompany
                    };


                    return Json(clm, JsonRequestBehavior.AllowGet);

                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {


                return Json(null, JsonRequestBehavior.AllowGet);
            }


        }
        
        public JsonResult AWCompanyName(int? company)
        {
            var companyName = "";

            if (company != null)
            {
                
                var user = User.Identity.GetUserId();
                var dta = new dataDB();
                var check = dta.UsersCompany.Where(x => x.UserID == user && x.CompanyID == company).Select(z => z).Any();
                if(check == true)
                {
                    companyName = dta.Company.Where(b => b.Id == company).Select(y => y.CompanyName).First();
                }
            }
            return Json(companyName, JsonRequestBehavior.AllowGet);
        }
    }
}
        
    
