using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using EADTearmProject.Models;
using System.Web.Http;
using System.Collections;
namespace EADTearmProject.Controllers
{
    public class HomeController : Controller
    {
        
        
        
        private EADProjectEntities db = new EADProjectEntities();
        public ActionResult SignUp(UserModel model)
        {
            User user = new User();
            user.first_name = model.first_name;
            user.last_name = model.last_name;
            user.password = model.password;
            user.email = model.email;
            user.user_type = model.user_type;
            user.address = model.address;

            Session["user"] = user;
            double total = 0;
            Session["total"] = total;
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            if(Session["user"]==null)
            {
                double total=0;
                Session["total"] = total;
                Session["list"] = null;
            }
            var result2 = db.Products.Where(x=>x.p_type==2).ToList();


            List<ProductModel> model = new List<ProductModel>();
            foreach (var p in result2)
            {
                ProductModel m = new ProductModel();
                m.p_id = p.p_id;
                m.p_description = p.p_description;
                m.p_name = p.p_name;
                m.p_description = p.p_description;
                m.p_picture = p.p_picture;
                m.p_price = p.p_price;
                m.p_type =(int) p.p_type;
                model.Add(m);
            }
            var result = db.Products.Where(x => x.p_type == 3).ToList();


            List<ProductModel> model2 = new List<ProductModel>();
            foreach (var p in result)
            {
                ProductModel m = new ProductModel();
                m.p_id = p.p_id;
                m.p_description = p.p_description;
                m.p_name = p.p_name;
                m.p_description = p.p_description;
                m.p_picture = p.p_picture;
                m.p_price = p.p_price;
                m.p_type =(int) p.p_type;
                model2.Add(m);
            }
            ViewBag.prodList = model2;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Logout()
        {
            Session["user"] = null;

            Session["total"] = 0;
            return RedirectToAction("Index");
        }
        public ActionResult CheckOut()
        {
            if(Session["user"]!=null)
            {
                Cart cart = new Cart();
                cart.total_cost = (Double)Session["total"];
                db.Carts.Add(cart);
                db.SaveChanges();
                List<CartItem> list = (List<CartItem>)Session["list"];

                foreach(var item in list )
                {
                    CartItem refitem = new CartItem();
                    refitem.cart_id = cart.cart_id;
                    refitem.p_id = item.p_id;
                    refitem.quantity = item.quantity;
                    refitem.total_cost = item.total_cost;
                    db.CartItems.Add(refitem);

                    db.SaveChanges();
                }

                Payment payment = new Payment();
                payment.cart_id = cart.cart_id;
                payment.date = DateTime.Now;
                payment.pay_type = "Credit Card";
                payment.amount = cart.total_cost;
                payment.user_id = ((User)Session["user"]).user_id;

                var user = db.Users.Find(payment.user_id);
                payment.User = user;
                db.Payments.Add(payment);

                db.SaveChanges();

                return View(payment);
            }
            else
            {
                return View("register");
            }
        
         
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Delete(int id)
        {

            List<CartItem> list = (List<CartItem>)Session["list"];
            for (int i = 0; i < list.Count; i++ )
            {
                if(list[i].p_id==id)
                {
                    double total = Convert.ToInt64(Session["total"].ToString());
                    total -= list[i].total_cost;
                    Session["total"] = total;
                    list.Remove(list[i]);
                    break;
                }
            }

            return View("cart",list);
        }
        
        public ActionResult products(int id)
        {
            Session["cat"] = id;
              var result =  db.Products.Where(x => x.cat_id == id).ToList();
              List<ProductModel> model = new List<ProductModel>();
            foreach(var p in result)
            {
                ProductModel m = new ProductModel();
                m.p_id = p.p_id;
                m.p_description = p.p_description;
                m.p_name = p.p_name;
                m.p_description = p.p_description;
                m.p_picture = p.p_picture;
                m.p_price = p.p_price;
                m.p_type =(int) p.p_type;
                model.Add(m);
            }
            ViewBag.preCount = 0;
            ViewBag.count = 3;
            return View(model);
        }
        public ActionResult register()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult cart()
        {
            List<CartItem> list = (List<CartItem>)Session["list"];

            return View(list);
        }
        public ActionResult NextFour(int id)
        {
            int cat = Convert.ToInt32(Session["cat"].ToString());
            var result = db.Products.Where(x => x.cat_id == cat).ToList();
            List<ProductModel> model = new List<ProductModel>();
            for (int i = id; i < id+3 &&  i < result.Count;i++ )
            {
                ProductModel m = new ProductModel();
                m.p_id = result[i].p_id;
                m.p_description = result[i].p_description;
                m.p_name = result[i].p_name;
                m.p_description = result[i].p_description;
                m.p_picture = result[i].p_picture;
                m.p_price = result[i].p_price;
                m.p_type =(int) result[i].p_type;
                model.Add(m);
            }
            ViewBag.count = id+3;
            ViewBag.preCount = id - 3;
            return View("products",model);
        }
        public ActionResult product_detail(int id)
        {
            var p = db.Products.Find(id);
            
            ProductModel m = new ProductModel();
            m.p_id = p.p_id;
            m.p_description = p.p_description;
            m.p_name = p.p_name;
            m.p_description = p.p_description;
            m.p_picture = p.p_picture;
            m.p_price = p.p_price;
            m.p_type =(int) p.p_type;

            return View(m);
        }
        public ActionResult AddToCart(CartItemModel model)
        {


            List<CartItem> list = null;
            list = (List<CartItem>)Session["list"];
            if (list == null)
                list = new List<CartItem>();


            double total = Convert.ToInt32(Session["total"].ToString());

            int index = -1;


            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].p_id == model.p_id)
                {
                    index = i;
                }

            }

            if (index != -1)
            {
                list[index].quantity = list[index].quantity + model.quantity;
                list[index].total_cost = list[index].total_cost + list[index].Product.p_price * model.quantity;
                total = total + list[index].Product.p_price * model.quantity;
            }
            else
            {
                var p = db.Products.Find(model.p_id);
                CartItem item = new CartItem();
                item.p_id = model.p_id;
                item.quantity = model.quantity;
                item.Product = p;
                item.total_cost = p.p_price * item.quantity;



                list.Add(item);

                Session["list"] = list;
                total += item.total_cost;
            }


            Session["total"] = total;


            int cat = Convert.ToInt32(Session["cat"].ToString());
            return RedirectToAction("products/"+cat);
        }
        public ActionResult Admin()
        {
            var result2 = db.Products.ToList();


            List<ProductModel> model = new List<ProductModel>();
            foreach (var p in result2)
            {
                ProductModel m = new ProductModel();
                m.p_id = p.p_id;
                m.p_description = p.p_description;
                m.p_name = p.p_name;
                m.p_description = p.p_description;
                m.p_picture = p.p_picture;
                m.p_price = p.p_price;
                m.p_type = (int)p.p_type;
                model.Add(m);
            }
            return View(model);
        }
        public ActionResult Create(int id)
        {
            ProductModel prod = new ProductModel();
            if(id>0)
            {
                var result2 = db.Products.Where(x => x.p_id == id).ToList();
                foreach (var p in result2)
                {
                    prod.p_type = (int)p.p_type;
                    prod.p_picture = p.p_picture;
                    prod.p_name = p.p_name;
                    prod.p_id = p.p_id;
                    prod.p_price = p.p_price;
                    prod.p_description = p.p_description;
                }
            }

            var result = db.Categories.ToList();

            List<CategoryModel> model = new List<CategoryModel>();
            foreach (var p in result)
            {

                CategoryModel cat = new CategoryModel();
                cat.cat_id = p.cat_id;
                cat.cat_name = p.cat_name;
                model.Add(cat);
            }
            ViewBag.list = model;
            return View(prod);

        }
        [System.Web.Mvc.HttpPost]
        public ActionResult AddProduct(Product product)
        {
            var uniqueName = "";
            if (Request.Files["image"] != null)
            {
                var file = Request.Files["image"];
                if(file.FileName!="")
                {
                    var ext = System.IO.Path.GetExtension(file.FileName);
                    uniqueName = Guid.NewGuid().ToString() + ext;
                    var rootpath = Server.MapPath("~/themes/images");
                    var filesavepath = System.IO.Path.Combine(rootpath, uniqueName);
                    file.SaveAs(filesavepath);
                    product.p_picture=uniqueName;
                }
            }
            if(product.p_id>0)
            {
                var result2 = db.Products.Find(product.p_id);

                if(result2 != null)
                {

                    result2.p_name = product.p_name;
                    if(product.p_picture!=null)
                        result2.p_picture = product.p_picture;
                    result2.p_price = product.p_price;
                    result2.cat_id = product.cat_id;
                    result2.p_description = product.p_description;
                    result2.p_type = product.p_type;
                }

            }
            else
            {
                db.Products.Add(product);
            }
            
            db.SaveChanges();
            return RedirectToAction("Admin");
        }
        public ActionResult DeleteProduct(int id)
        {
            var result = db.Products.Where(x => x.p_id == id).ToList();
            db.Products.Remove(result[0]);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }
        
        public ActionResult Login(UserModel user)
        {

            var result = db.Users.Where(x => x.email == user.email && x.password==user.password).ToList();

            if(result.Count>0)
            {
                if(result[0].user_type == 1)
                {  
                    var result2 = db.Products.ToList();

                    List<ProductModel> model = new List<ProductModel>();
                    foreach (var p in result2)
                    {
                        ProductModel m = new ProductModel();
                        m.p_id = p.p_id;
                        m.p_description = p.p_description;
                        m.p_name = p.p_name;
                        m.p_description = p.p_description;
                        m.p_picture = p.p_picture;
                        m.p_price = p.p_price;
                        m.p_type =(int) p.p_type;
                        model.Add(m);
                    }
                 return  View("Admin",model);

                }
                else if( result[0].user_type == 2)
                {
                    Session["user"] = result[0];
                   return RedirectToAction("Index");
                }

            }
            return RedirectToAction("Index");
            
        }

        
    }
}