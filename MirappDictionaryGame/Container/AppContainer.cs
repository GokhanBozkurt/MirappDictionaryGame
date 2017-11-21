using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
//using Microsoft.Practices.Unity;

namespace MirappDictionaryGame
{
    class AppContainer 
        //: Application
    {
        /*
        public App(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {

        }

        public override void OnCreate()
        {
            Initialize();

            base.OnCreate();
        }

        public static UnityContainer Container { get; set; }

        private static void Initialize()
        {
            //App.Container = new UnityContainer();

            //App.Container.RegisterType<IProductService, ProductService>();
            //App.Container.RegisterType<BaseAdapter<Product>, ProductListAdapter>();
        }
        */
    }

    /*
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
    
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
    }
    public class ProductService : IProductService
    {
        public IEnumerable<Product> GetAll()
        {
            return new List<Product>
                 {
                 new Product {
                 Id =1,
                 Name = "Google Android 2.2",
                 Description = "Products's description.",
                 },
                 new Product {
                 Id =2,
                 Name = "Apple iPad",
                 Description = "Products's description.",
                 },
                 new Product {
                 Id =3,
                 Name = "Amazon Kindle (third-generation)",
                 Description = "Products's description.",
                 },
                 new Product {
                 Id =4,
                 Name = "Netflix",
                 Description = "Products's description.",
                 },
                 new Product {
                 Id =5,
                 Name = "Samsung Galaxy Tab",
                 Description = "Products's description.",
                 },
                 };
        }
    }
*/
}