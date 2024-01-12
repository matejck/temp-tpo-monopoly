using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly_0._9._1._1_Advanced
{
    public class Position
    {
        int index;
        string index_name;
        string owned_by = "/";
        int hotels = 0;
        int houses = 0;
        int rent = 0;
        int price = 0;
        string color_group = "";
        bool buildings_buyable = true;
        int house_price;
        int hotel_price;
        bool buyable = true;
        public Position(int index)
        {
            this.index = index;
            index_name = getIndexName(index);
        }

        public Position() { }

        public string getIndexName()
        {
            return index_name;
        }
        public int getIndex()
        {
            return index;
        }
        public int Price()
        {
            return price;
        }
        public string Color()
        {
            return color_group;
        }
        public string ownedBy
        {
            get { return owned_by; }
            set { owned_by = value; }
        }
        public int Hotels
        {
            get { return hotels; }
            set { hotels = value; }
        }
        public int Houses
        {
            get { return houses; }
            set { houses = value; }
        }

        public int BuyHousePrice()
        {
            return house_price;
        }
        public int BuyHotelPrice()
        {
            return hotel_price;
        }
        public bool BuildingsBuyable()
        {
            return buildings_buyable;
        }

        public bool isBuyable()
        {
            return buyable;
        }

        private string getIndexName(int index)
        {
            string name = "Unavailable";
            if (index == 0)
            {
                name = "GO!";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 1)
            {
                name = "MEDITERRANEAN AVENUE";
                price = 60;
                house_price = 50;
                hotel_price = 50;
            }
            else if (index == 2)
            {
                name = "COMMUNITY CHEST";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 3)
            {
                name = "BALTIC AVENUE";
                price = 60;
                house_price = 50;
                hotel_price = 50;
            }
            else if (index == 4)
            {
                name = "INCOME TAX";
                rent = 200;
                owned_by = "Monopoly Bank";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 5)
            {
                name = "READING RAILROAD";
                price = 200;
                buildings_buyable = false;
            }
            else if (index == 6)
            {
                name = "ORIENTAL AVENUE";
                price = 100;
                house_price = 50;
                hotel_price = 50;
            }
            else if (index == 7)
            {
                name = "CHANCE";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 8)
            {
                name = "VERMONT AVENUE";
                price = 100;
                house_price = 50;
                hotel_price = 50;
            }
            else if (index == 9)
            {
                name = "CONNECTICUT AVENUE";
                price = 120;
                house_price = 50;
                hotel_price = 50;
            }
            else if (index == 10)
            {
                name = "PRISON: JUST VISITING";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 11)
            {
                name = "ST.CHARLES PLACE";
                price = 140;
                house_price = 100;
                hotel_price = 100;
            }
            else if (index == 12)
            {
                name = "ELECTRIC COMPANY";
                price = 150;
                buildings_buyable = false;
            }
            else if (index == 13)
            {
                name = "STATES AVENUE";
                price = 140;
                house_price = 100;
                hotel_price = 100;
            }
            else if (index == 14)
            {
                name = "VIRGINIA AVENUE";
                price = 160;
                house_price = 100;
                hotel_price = 100;
            }
            else if (index == 15)
            {
                name = "PENNSYLVANIA RAILROAD";
                price = 200;
                buildings_buyable = false;
            }
            else if (index == 16)
            {
                name = "ST.JAMES PLACE";
                price = 180;
                house_price = 100;
                hotel_price = 100;
            }
            else if (index == 17)
            {
                name = "COMMUNITY CHEST";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 18)
            {
                name = "TENNESSEE AVENUE";
                price = 180;
                house_price = 100;
                hotel_price = 100;
            }
            else if (index == 19)
            {
                name = "NEW YORK AVENUE";
                price = 200;
                house_price = 100;
                hotel_price = 100;
            }
            else if (index == 20)
            {
                name = "FREE PARKING";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 21)
            {
                name = "KENTUCKY AVENUE";
                price = 220;
                house_price = 150;
                hotel_price = 150;
            }
            else if (index == 22)
            {
                name = "CHANCE";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 23)
            {
                name = "INDIANA AVENUE";
                price = 220;
                house_price = 150;
                hotel_price = 150;
            }
            else if (index == 24)
            {
                name = "ILLINOIS AVENUE";
                price = 240;
                house_price = 150;
                hotel_price = 150;
            }
            else if (index == 25)
            {
                name = "B. AND O. RAILROAD";
                price = 200;
                buildings_buyable = false;
            }
            else if (index == 26)
            {
                name = "ATLANTIC AVENUE";
                price = 260;
                house_price = 150;
                hotel_price = 150;
            }
            else if (index == 27)
            {
                name = "VENTNOR AVENUE";
                price = 260;
                house_price = 150;
                hotel_price = 150;
            }
            else if (index == 28)
            {
                name = "WATER WORKS";
                price = 150;
                buildings_buyable = false;
            }
            else if (index == 29)
            {
                name = "MARVIN GARDENS";
                price = 280;
                house_price = 150;
                hotel_price = 150;
            }
            else if (index == 30)
            {
                name = "GO TO JAIL";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 31)
            {
                name = "PACIFIC AVENUE";
                price = 300;
                house_price = 200;
                hotel_price = 200;
            }
            else if (index == 32)
            {
                name = "NORTH CAROLINA AVENUE";
                price = 300;
                house_price = 200;
                hotel_price = 200;
            }
            else if (index == 33)
            {
                name = "COMMUNITY CHEST";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 34)
            {
                name = "PENNSYLVANIA AVENUE";
                price = 320;
                house_price = 200;
                hotel_price = 200;
            }
            else if (index == 35)
            {
                name = "SHORT LINE";
                price = 200;
                buildings_buyable = false;
            }
            else if (index == 36)
            {
                name = "CHANCE";
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 37)
            {
                name = "PARK PLACE";
                price = 350;
                house_price = 200;
                hotel_price = 200;
            }
            else if (index == 38)
            {
                name = "LUXURY TAX";
                owned_by = "Monopoly Bank";
                rent = 100;
                buyable = false;
                buildings_buyable = false;
            }
            else if (index == 39)
            {
                name = "BOARDWALK";
                price = 400;
                house_price = 200;
                hotel_price = 200;
            }
            return name;
        }

        private int doubleRent(string pos1, string pos2)
        {
            int double_rent = 1;
            foreach (Position p1 in Monopoly.properties)
            {
                if (p1.getIndexName() == pos1)
                {
                    if (p1.ownedBy == owned_by)
                    {
                        foreach (Position p2 in Monopoly.properties)
                        {
                            if (p2.getIndexName() == pos2)
                            {
                                if (p2.ownedBy == owned_by)
                                {
                                    double_rent = 2;
                                    return double_rent;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return double_rent;
        }

        private int doubleRent(string pos)
        {
            int double_rent = 1;
            foreach (Position p in Monopoly.properties)
            {
                if (p.getIndexName() == pos)
                {
                    if (p.ownedBy == owned_by)
                    {
                        double_rent = 2;
                        return double_rent;
                    }
                }
            }
            return double_rent;
        }

        public bool allColorGroup()
        {
            int all_colors_owned = 1;
            switch (index_name)
            {
                case "ATLANTIC AVENUE":
                    all_colors_owned = doubleRent("VENTNOR AVENUE", "MARVIN GARDENS");
                    break;
                case "BALTIC AVENUE":
                    all_colors_owned = doubleRent("MEDITERRANEAN AVENUE");
                    break;
                case "BOARDWALK":
                    all_colors_owned = doubleRent("PARK PLACE");
                    break;
                case "CONNECTICUT AVENUE":
                    all_colors_owned = doubleRent("ORIENTAL AVENUE", "VERMONT AVENUE");
                    break;
                case "ILLINOIS AVENUE":
                    all_colors_owned = doubleRent("INDIANA AVENUE", "KENTUCKY AVENUE");
                    break;
                case "INDIANA AVENUE":
                    all_colors_owned = doubleRent("ILLINOIS AVENUE", "KENTUCKY AVENUE");
                    break;
                case "KENTUCKY AVENUE":
                    all_colors_owned = doubleRent("ILLINOIS AVENUE", "INDIANA AVENUE");
                    break;
                case "MARVIN GARDENS":
                    all_colors_owned = doubleRent("VENTNOR AVENUE", "ATLANTIC AVENUE");
                    break;
                case "MEDITERRANEAN AVENUE":
                    all_colors_owned = doubleRent("BALTIC AVENUE");
                    break;
                case "NEW YORK AVENUE":
                    all_colors_owned = doubleRent("ST.JAMES PLACE", "TENNESSEE AVENUE");
                    break;
                case "NORTH CAROLINA AVENUE":
                    all_colors_owned = doubleRent("PACIFIC AVENUE", "PENNSYLVANIA AVENUE");
                    break;
                case "ORIENTAL AVENUE":
                    all_colors_owned = doubleRent("CONNECTICUT AVENUE", "VERMONT AVENUE");
                    break;
                case "PACIFIC AVENUE":
                    all_colors_owned = doubleRent("PENNSYLVANIA AVENUE", "NORTH CAROLINA AVENUE");
                    break;
                case "PARK PLACE":
                    all_colors_owned = doubleRent("BOARDWALK");
                    break;
                case "PENNSYLVANIA AVENUE":
                    all_colors_owned = doubleRent("PACIFIC AVENUE", "NORTH CAROLINA AVENUE");
                    break;
                case "ST.CHARLES PLACE":
                    all_colors_owned = doubleRent("STATES AVENUE", "VIRGINIA AVENUE");
                    break;
                case "ST.JAMES PLACE":
                    all_colors_owned = doubleRent("TENNESSEE AVENUE", "NEW YORK AVENUE");
                    break;
                case "STATES AVENUE":
                    all_colors_owned = doubleRent("VIRGINIA AVENUE", "ST.CHARLES PLACE");
                    break;
                case "TENNESSEE AVENUE":
                    all_colors_owned = doubleRent("NEW YORK AVENUE", "ST.JAMES PLACE");
                    break;
                case "VENTNOR AVENUE":
                    all_colors_owned = doubleRent("MARVIN GARDENS", "ATLANTIC AVENUE");
                    break;
                case "VERMONT AVENUE":
                    all_colors_owned = doubleRent("CONNECTICUT AVENUE", "ORIENTAL AVENUE");
                    break;
                case "VIRGINIA AVENUE":
                    all_colors_owned = doubleRent("ST.CHARLES PLACE", "STATES AVENUE");
                    break;
            }

            if (all_colors_owned == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CalculateRent()
        {
            int rent = 0;
            int double_rent = 1; 
            double_rent = doubleRent("VENTNOR AVENUE", "MARVIN GARDENS");
            switch (index_name)
            {
                case "ATLANTIC AVENUE":
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 110 + 1150 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 330 + 1150 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 800 + 1150 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 975 + 1150 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1150 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "BALTIC AVENUE":
                    double_rent = doubleRent("MEDITERRANEAN AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 20 + 450 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 60 + 450 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 180 + 450 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 320 + 450 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 450 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = 4;
                    }
                    break;
                case "BOARDWALK":
                    double_rent = doubleRent("PARK PLACE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 200 + 2000 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 600 + 2000 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 1400 + 2000 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 1700 + 2000 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 2000 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = 50;
                    }
                    break;
                case "CONNECTICUT AVENUE":
                    double_rent = doubleRent("ORIENTAL AVENUE", "VERMONT AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 40 + 600 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 100 + 600 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 300 + 600 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 450 + 600 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 600 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "ILLINOIS AVENUE":
                    double_rent = doubleRent("INDIANA AVENUE", "KENTUCKY AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 100 + 1100 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 300 + 1100 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 750 + 1100 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 925 + 1100 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1100 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "INDIANA AVENUE":
                    double_rent = doubleRent("ILLINOIS AVENUE", "KENTUCKY AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 90 + 1050 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 250 + 1050 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 700 + 1050 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 875 + 1050 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1050 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "KENTUCKY AVENUE":
                    double_rent = doubleRent("ILLINOIS AVENUE", "INDIANA AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 90 + 1050 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 250 + 1050 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 700 + 1050 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 875 + 1050 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1050 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "MARVIN GARDENS":
                    double_rent = doubleRent("VENTNOR AVENUE", "ATLANTIC AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 120 + 1200 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 360 + 1200 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 850 + 1200 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 1025 + 1200 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1200 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "MEDITERRANEAN AVENUE":
                    double_rent = doubleRent("BALTIC AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 10 + 250 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 30 + 250 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 90 + 250 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 160 + 250 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 250 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "NEW YORK AVENUE":
                    double_rent = doubleRent("ST.JAMES PLACE", "TENNESSEE AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 80 + 1000 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 220 + 1000 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 600 + 1000 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 800 + 1000 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1000 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "NORTH CAROLINA AVENUE":
                    double_rent = doubleRent("PACIFIC AVENUE", "PENNSYLVANIA AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 130 + 1275 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 390 + 1275 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 900 + 1275 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 1100 + 1275 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1275 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "ORIENTAL AVENUE":
                    double_rent = doubleRent("CONNECTICUT AVENUE", "VERMONT AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 30 + 550 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 90 + 550 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 270 + 550 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 400 + 550 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 550 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "PACIFIC AVENUE":
                    double_rent = doubleRent("PENNSYLVANIA AVENUE", "NORTH CAROLINA AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 130 + 1275 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 390 + 1275 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 900 + 1275 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 1100 + 1275 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1275 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "PARK PLACE":
                    double_rent = doubleRent("BOARDWALK");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 175 + 1500 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 500 + 1500 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 1100 + 1500 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 1300 + 1500 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1500 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = 35;
                    }
                    break;
                case "PENNSYLVANIA AVENUE":
                    double_rent = doubleRent("PACIFIC AVENUE", "NORTH CAROLINA AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 150 + 1400 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 450 + 1400 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 1000 + 1400 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 1200 + 1400 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1400 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "ST.CHARLES PLACE":
                    double_rent = doubleRent("STATES AVENUE", "VIRGINIA AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 50 + 750 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 150 + 750 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 450 + 750 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 625 + 750 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 750 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "ST.JAMES PLACE":
                    double_rent = doubleRent("TENNESSEE AVENUE", "NEW YORK AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 70 + 950 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 200 + 950 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 550 + 950 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 750 + 950 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 950 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "STATES AVENUE":
                    double_rent = doubleRent("VIRGINIA AVENUE", "ST.CHARLES PLACE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 50 + 750 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 150 + 750 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 450 + 750 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 625 + 750 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 750 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "TENNESSEE AVENUE":
                    double_rent = doubleRent("NEW YORK AVENUE", "ST.JAMES PLACE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 70 + 950 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 200 + 950 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 550 + 950 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 750 + 950 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 950 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "VENTNOR AVENUE":
                    double_rent = doubleRent("MARVIN GARDENS", "ATLANTIC AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 110 + 1150 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 330 + 1150 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 800 + 1150 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 975 + 1150 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 1150 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "VERMONT AVENUE":
                    double_rent = doubleRent("CONNECTICUT AVENUE", "ORIENTAL AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 30 + 550 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 90 + 550 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 270 + 550 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 400 + 550 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 550 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "VIRGINIA AVENUE":
                    double_rent = doubleRent("ST.CHARLES PLACE", "STATES AVENUE");
                    if (houses > 0)
                    {
                        switch (houses)
                        {
                            case 1:
                                rent = 60 + 900 * hotels; //če je hotels=0, je 1150*0=0 in je samo 110
                                double_rent = 1;
                                break;
                            case 2:
                                rent = 180 + 900 * hotels;
                                double_rent = 1;
                                break;
                            case 3:
                                rent = 500 + 900 * hotels;
                                double_rent = 1;
                                break;
                            case 4:
                                rent = 700 + 900 * hotels;
                                double_rent = 1;
                                break;
                        }
                    }
                    else if (hotels > 0)
                    {
                        rent = 900 * hotels; //če ima igralec hotel ne prištevam renta
                    }
                    else
                    {
                        rent = (price / 10) - 4; //rezultat je 22 => (260/10)-4 je formula v tem primeru
                    }
                    break;
                case "ELECTRIC COMPANY":
                    double_rent = doubleRent("WATER WORKS");
                    if (double_rent == 2 || Monopoly.double_rent == 2)
                    {
                        rent = 10 * Monopoly.dice_amount;
                    }
                    else
                    {
                        rent = 4 * Monopoly.dice_amount;
                    }
                    double_rent = 1;
                    break;
                case "WATER WORKS":
                    double_rent = doubleRent("ELECTRIC COMPANY");
                    if (double_rent == 2 || Monopoly.double_rent == 2)
                    {
                        rent = 10 * Monopoly.dice_amount;
                    }
                    else
                    {
                        rent = 4 * Monopoly.dice_amount;
                    }
                    double_rent = 1;
                    break;
                case "B. AND O. RAILROAD":
                    rent = 25;
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "PENNSYLVANIA RAILROAD")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "SHORT LINE")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "READING RAILROAD")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    break;

                case "PENNSYLVANIA RAILROAD":
                    rent = 25;
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "B. AND O. RAILROAD")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "SHORT LINE")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "READING RAILROAD")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    break;
                case "READING RAILROAD":
                    rent = 25;
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "B. AND O. RAILROAD")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "SHORT LINE")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "PENNSYLVANIA RAILROAD")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    break;
                case "SHORT LINE":
                    rent = 25;
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "B. AND O. RAILROAD")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "READING RAILROAD")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    foreach (Position p in Monopoly.properties)
                    {
                        if (p.getIndexName() == "PENNSYLVANIA RAILROAD")
                        {
                            if (p.ownedBy == owned_by)
                            {
                                double_rent = double_rent * 2;
                            }
                        }
                    }
                    break;
            }
            if (rent == 0)
            {
                return this.rent;
            }
            return rent * double_rent;
        }

        public bool isBuildingEvenly()
        {
            bool evenly = false;
            switch (index_name)
            {
                case "ATLANTIC AVENUE":
                    evenly = buildingEvenly("VENTNOR AVENUE", "MARVIN GARDENS");
                    break;
                case "BALTIC AVENUE":
                    evenly = buildingEvenly("MEDITERRANEAN AVENUE");
                    break;
                case "BOARDWALK":
                    evenly = buildingEvenly("PARK PLACE");
                    break;
                case "CONNECTICUT AVENUE":
                    evenly = buildingEvenly("ORIENTAL AVENUE", "VERMONT AVENUE");
                    break;
                case "ILLINOIS AVENUE":
                    evenly = buildingEvenly("INDIANA AVENUE", "KENTUCKY AVENUE");
                    break;
                case "INDIANA AVENUE":
                    evenly = buildingEvenly("ILLINOIS AVENUE", "KENTUCKY AVENUE");
                    break;
                case "KENTUCKY AVENUE":
                    evenly = buildingEvenly("ILLINOIS AVENUE", "INDIANA AVENUE");
                    break;
                case "MARVIN GARDENS":
                    evenly = buildingEvenly("VENTNOR AVENUE", "ATLANTIC AVENUE");
                    break;
                case "MEDITERRANEAN AVENUE":
                    evenly = buildingEvenly("BALTIC AVENUE");
                    break;
                case "NEW YORK AVENUE":
                    evenly = buildingEvenly("ST.JAMES PLACE", "TENNESSEE AVENUE");
                    break;
                case "NORTH CAROLINA AVENUE":
                    evenly = buildingEvenly("PACIFIC AVENUE", "PENNSYLVANIA AVENUE");
                    break;
                case "ORIENTAL AVENUE":
                    evenly = buildingEvenly("CONNECTICUT AVENUE", "VERMONT AVENUE");
                    break;
                case "PACIFIC AVENUE":
                    evenly = buildingEvenly("PENNSYLVANIA AVENUE", "NORTH CAROLINA AVENUE");
                    break;
                case "PARK PLACE":
                    evenly = buildingEvenly("BOARDWALK");
                    break;
                case "PENNSYLVANIA AVENUE":
                    evenly = buildingEvenly("PACIFIC AVENUE", "NORTH CAROLINA AVENUE");
                    break;
                case "ST.CHARLES PLACE":
                    evenly = buildingEvenly("STATES AVENUE", "VIRGINIA AVENUE");
                    break;
                case "ST.JAMES PLACE":
                    evenly = buildingEvenly("TENNESSEE AVENUE", "NEW YORK AVENUE");
                    break;
                case "STATES AVENUE":
                    evenly = buildingEvenly("VIRGINIA AVENUE", "ST.CHARLES PLACE");
                    break;
                case "TENNESSEE AVENUE":
                    evenly = buildingEvenly("NEW YORK AVENUE", "ST.JAMES PLACE");
                    break;
                case "VENTNOR AVENUE":
                    evenly = buildingEvenly("MARVIN GARDENS", "ATLANTIC AVENUE");
                    break;
                case "VERMONT AVENUE":
                    evenly = buildingEvenly("CONNECTICUT AVENUE", "ORIENTAL AVENUE");
                    break;
                case "VIRGINIA AVENUE":
                    evenly = buildingEvenly("ST.CHARLES PLACE", "STATES AVENUE");
                    break;
            }

            return evenly;
        }

        private bool buildingEvenly(string pos1, string pos2)
        {
            int pos1_h = 0;
            int pos2_h = 0;

            foreach (Position p1 in Monopoly.properties)
            {
                if (p1.getIndexName() == pos1)
                {
                    pos1_h = p1.Houses;
                }
            }
            foreach (Position p2 in Monopoly.properties)
            {
                if (p2.getIndexName() == pos2)
                {
                    pos2_h = p2.Houses;
                }
            }

            if (houses <= pos1_h && houses <= pos2_h)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool buildingEvenly(string pos)
        {
            int pos_h = 0;

            foreach (Position p1 in Monopoly.properties)
            {
                if (p1.getIndexName() == pos)
                {
                    pos_h = p1.Houses;
                }
            }

            if (houses <= pos_h)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isSellingEvenly()
        {
            bool evenly = false;
            switch (index_name)
            {
                case "ATLANTIC AVENUE":
                    evenly = sellingEvenly("VENTNOR AVENUE", "MARVIN GARDENS");
                    break;
                case "BALTIC AVENUE":
                    evenly = sellingEvenly("MEDITERRANEAN AVENUE");
                    break;
                case "BOARDWALK":
                    evenly = sellingEvenly("PARK PLACE");
                    break;
                case "CONNECTICUT AVENUE":
                    evenly = sellingEvenly("ORIENTAL AVENUE", "VERMONT AVENUE");
                    break;
                case "ILLINOIS AVENUE":
                    evenly = sellingEvenly("INDIANA AVENUE", "KENTUCKY AVENUE");
                    break;
                case "INDIANA AVENUE":
                    evenly = sellingEvenly("ILLINOIS AVENUE", "KENTUCKY AVENUE");
                    break;
                case "KENTUCKY AVENUE":
                    evenly = sellingEvenly("ILLINOIS AVENUE", "INDIANA AVENUE");
                    break;
                case "MARVIN GARDENS":
                    evenly = sellingEvenly("VENTNOR AVENUE", "ATLANTIC AVENUE");
                    break;
                case "MEDITERRANEAN AVENUE":
                    evenly = sellingEvenly("BALTIC AVENUE");
                    break;
                case "NEW YORK AVENUE":
                    evenly = sellingEvenly("ST.JAMES PLACE", "TENNESSEE AVENUE");
                    break;
                case "NORTH CAROLINA AVENUE":
                    evenly = sellingEvenly("PACIFIC AVENUE", "PENNSYLVANIA AVENUE");
                    break;
                case "ORIENTAL AVENUE":
                    evenly = sellingEvenly("CONNECTICUT AVENUE", "VERMONT AVENUE");
                    break;
                case "PACIFIC AVENUE":
                    evenly = sellingEvenly("PENNSYLVANIA AVENUE", "NORTH CAROLINA AVENUE");
                    break;
                case "PARK PLACE":
                    evenly = sellingEvenly("BOARDWALK");
                    break;
                case "PENNSYLVANIA AVENUE":
                    evenly = sellingEvenly("PACIFIC AVENUE", "NORTH CAROLINA AVENUE");
                    break;
                case "ST.CHARLES PLACE":
                    evenly = sellingEvenly("STATES AVENUE", "VIRGINIA AVENUE");
                    break;
                case "ST.JAMES PLACE":
                    evenly = sellingEvenly("TENNESSEE AVENUE", "NEW YORK AVENUE");
                    break;
                case "STATES AVENUE":
                    evenly = sellingEvenly("VIRGINIA AVENUE", "ST.CHARLES PLACE");
                    break;
                case "TENNESSEE AVENUE":
                    evenly = sellingEvenly("NEW YORK AVENUE", "ST.JAMES PLACE");
                    break;
                case "VENTNOR AVENUE":
                    evenly = sellingEvenly("MARVIN GARDENS", "ATLANTIC AVENUE");
                    break;
                case "VERMONT AVENUE":
                    evenly = sellingEvenly("CONNECTICUT AVENUE", "ORIENTAL AVENUE");
                    break;
                case "VIRGINIA AVENUE":
                    evenly = sellingEvenly("ST.CHARLES PLACE", "STATES AVENUE");
                    break;
            }

            return evenly;
        }

        private bool sellingEvenly(string pos1, string pos2)
        {
            int pos1_h = 0;
            int pos2_h = 0;

            foreach (Position p1 in Monopoly.properties)
            {
                if (p1.getIndexName() == pos1)
                {
                    pos1_h = p1.Houses;
                }
            }
            foreach (Position p2 in Monopoly.properties)
            {
                if (p2.getIndexName() == pos2)
                {
                    pos2_h = p2.Houses;
                }
            }

            if (houses <= pos1_h && houses <= pos2_h)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool sellingEvenly(string pos)
        {
            int pos_h = 0;

            foreach (Position p1 in Monopoly.properties)
            {
                if (p1.getIndexName() == pos)
                {
                    pos_h = p1.Houses;
                }
            }

            if (houses <= pos_h)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
