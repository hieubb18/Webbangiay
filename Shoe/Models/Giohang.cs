using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoe.Models
{
    public class Giohang
    {

        dbShoeStoreDataContext data = new dbShoeStoreDataContext();
        //Taouatao.  6 references 
        public int iMagiay { set; get; }
        public string sTengiay { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        //Khoi ta
        public Giohang(int Magiay)
        {
            iMagiay = Magiay;
            GIAY giay = data.GIAYs.Single(n => n.MaGiay == iMagiay);
            sTengiay = giay.TenGiay;
            sAnhbia = giay.Anhbia;
            dDongia = double.Parse(giay.Giaban.ToString());
            iSoluong = 1;
        }
    }
}