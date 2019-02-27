using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models
{

    #region Reservation Package Table

    public class PackageTable
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Meals { get; set; }
        public decimal Fuel { get; set; }
        public int Days { get; set; }
        public decimal Rate { get; set; }

        //calculation strings
        public string TextMeals { get; set; }
        public string TextFuel { get; set; }
        public string TextDays { get; set; }
        public string TextRate { get; set; }
    }

    #endregion
    
    public class CarReserve
    {
        JobDBContainer db = new JobDBContainer();
        
        #region PackageList

        /*GET list of packages with the given
         *car id, no of days, rental type, meal,
         *fuel and permission
         */ 
        public List<PackageTable> getPackageList(int CarId, int days, int rentType, int meals, int fuel, int isAuthorize)
        {
            int RentType = rentType;
            int rsvRentType = RentType;
            int rsvDays = days;
            int rsvIncludeMeals = meals;
            int rsvIncludeFuel = fuel;
            decimal TotalRate = 1;
            int adjDays = (rsvDays - 1);    //subtract 1 day for the fuel calculation

            List<PackageTable> packages = new List<PackageTable>();
            List<CarRatePackage> packageList = db.CarRatePackages.Where(c => c.Status == "ACT" || c.Status == "SYS").ToList();

            //list of package
            foreach (var pkg in packageList)
            {
                //package rates of car unit
                var carRatePackage = pkg.CarRateUnitPackages.Where(c => c.CarUnitId == CarId && c.CarRatePackage.Id == pkg.Id).FirstOrDefault();
                
                //handle meals
                decimal MealsRate = getMealsCalc(rsvIncludeMeals, pkg.DailyMeals, pkg.DailyRoom, adjDays, rsvRentType);
                string MealsRateTxt = getMealsCalcTxt(rsvIncludeMeals, pkg.DailyMeals, pkg.DailyRoom, adjDays, rsvRentType, isAuthorize);
                
                //handle fuel
                decimal FuelRate = getFuelCalc(rsvIncludeFuel, carRatePackage.FuelDaily, carRatePackage.FuelLonghaul, rsvDays, rsvRentType, pkg.Id);
                string FuelRateTxt = getFuelCalcTxt(rsvIncludeFuel, carRatePackage.FuelDaily, carRatePackage.FuelLonghaul, rsvDays, rsvRentType, pkg.Id, isAuthorize);

                //handle unit Rate
                decimal carRate = getCarRateCalc(CarId, days, (decimal)carRatePackage.DailyAddon, carRatePackage.DailyRate);
                string carRatetxt = getCarRateCalcTxt(CarId, days, (decimal)carRatePackage.DailyAddon, carRatePackage.DailyRate, isAuthorize);
                
                //handle total Rate
                TotalRate = MealsRate + FuelRate + carRate;

                PackageTable newPkg = new PackageTable();
                newPkg.Id = pkg.Id;
                newPkg.Description = pkg.Description;
                newPkg.Fuel = FuelRate;
                newPkg.Meals = (MealsRate);
                newPkg.Days = (rsvDays);
                newPkg.Rate = TotalRate;
                newPkg.TextMeals = MealsRateTxt;
                newPkg.TextFuel = FuelRateTxt;

                if (isAuthorize == 1) {  //guest user
                    newPkg.TextRate = carRatetxt + " + m: " + MealsRate + " + f: " + FuelRate;
                } else {   //guest user
                    newPkg.TextRate = carRatetxt;
                }

                packages.Add(newPkg);
            }

            return packages;
        }

        //Get the selected package by package ID
        public PackageTable getPackageSummary(int CarId, int days, int rentType, int meals, int fuel, int selectedPkgId, int isAuthorize)
        {
            int RentType = rentType;
            int rsvRentType = RentType;
            int rsvDays = days;
            int rsvIncludeMeals = meals;
            int rsvIncludeFuel = fuel;
            decimal TotalRate = 1;
            int adjDays = (rsvDays - 1);   //subtract 1 day for the fuel calculation

            PackageTable packages = new PackageTable();
            CarRatePackage pkg = db.CarRatePackages.Where(c => c.Id == selectedPkgId).FirstOrDefault();

            //package rates of car unit
            var carRatePackage = pkg.CarRateUnitPackages.Where(c => c.CarUnitId == CarId && c.CarRatePackage.Id == pkg.Id).FirstOrDefault();
            
            //handle meals
            decimal MealsRate = getMealsCalc(rsvIncludeMeals, pkg.DailyMeals, pkg.DailyRoom, adjDays, rsvRentType);
            string MealsRateTxt = getMealsCalcTxt(rsvIncludeMeals, pkg.DailyMeals, pkg.DailyRoom, adjDays, rsvRentType, isAuthorize);
            //handle fuel
            decimal FuelRate = getFuelCalc(rsvIncludeFuel, carRatePackage.FuelDaily, carRatePackage.FuelLonghaul, rsvDays, rsvRentType, pkg.Id);
            string FuelRateTxt = getFuelCalcTxt(rsvIncludeFuel, carRatePackage.FuelDaily, carRatePackage.FuelLonghaul, rsvDays, rsvRentType, pkg.Id, isAuthorize);

            //handle unit Rate
            decimal carRate = getCarRateCalc(CarId, days, (decimal)carRatePackage.DailyAddon, carRatePackage.DailyRate);
            string carRatetxt = getCarRateCalcTxt(CarId, days, (decimal)carRatePackage.DailyAddon, carRatePackage.DailyRate, isAuthorize);
           
            //handle total Rate
            TotalRate = MealsRate + FuelRate + carRate;

            PackageTable newPkg = new PackageTable();
            newPkg.Id = pkg.Id;
            newPkg.Description = pkg.Description;
            newPkg.Fuel = FuelRate;
            newPkg.Meals = (MealsRate);
            newPkg.Days = (rsvDays);
            newPkg.Rate = TotalRate;
            newPkg.TextMeals = MealsRateTxt;
            newPkg.TextFuel = FuelRateTxt;

            if (isAuthorize == 1) { // admin user
                newPkg.TextRate = carRatetxt + " + m: " + MealsRate + " + f: " + FuelRate;
            } else {                // guest user
                newPkg.TextRate = carRatetxt;
            }


            packages = newPkg;

            return packages;
        }

        /**
         * Get total Meal rate 
         */ 
        private decimal getMealsCalc(int rsvIncludeMeals, decimal pkgMeals, decimal pkgRooms, int days, int rentType)
        {
            //MealsAcc
            decimal mealsperDay = (pkgMeals + pkgRooms) * (days);
            decimal mealsRate = ((mealsperDay) + pkgMeals); //include meals on the day
            mealsRate *= rentType;    //self drive does not include meals
            mealsRate *= rsvIncludeMeals;    //check if user wants to include or not include driver meals

            return mealsRate;
        }


        /**
         * Get total Meal rate Text
         */
        private string getMealsCalcTxt(int rsvIncludeMeals, decimal pkgMeals, decimal pkgRooms, int days, int rentType, int isAuthorize)
        {
            // MealsAcc
            decimal mealsperDay = (pkgMeals + pkgRooms) * (days);
            decimal mealsRate = ((mealsperDay) + pkgMeals); //include meals on the day
            string mealsText = "";

            mealsRate *= rentType;    //self drive does not include meals
            mealsRate *= rsvIncludeMeals;    //check if user wants to include or not include driver meals
            
            if (isAuthorize == 1)
            {
                mealsText = rsvIncludeMeals == 1 ? "Included - m : " + mealsRate : "by renter";
            }
            else
            {   //guest user
                mealsText = rsvIncludeMeals == 1 ? "Included in the package " : "by renter";
            }
        

            return mealsText;
        }
        
        /**
         * Get total Fuel rate 
         */
        private decimal getFuelCalc(int rsvIncludeFuel, decimal pkgFuel, decimal pkglonghaul, int days, int rentType, int tourid)
        {
            //MealsAcc
            decimal fuelRate = 0;

            //Fuel
            fuelRate = (pkgFuel * days) + pkglonghaul;
            fuelRate *= rsvIncludeFuel;

            return fuelRate;
        }

        /**
         * Get total Fuel rate  text
         */
        private string getFuelCalcTxt(int rsvIncludeFuel, decimal pkgFuel, decimal pkglonghaul, int days, int rentType, int tourid, int isAuthorize)
        {
            //MealsAcc
            decimal fuelRate = 0;
            string fuelRateTxt = "";

            //Fuel
            fuelRate = (pkgFuel * days) + pkglonghaul;
            fuelRate *= rsvIncludeFuel;
            

            if (isAuthorize == 1)
            {
                fuelRateTxt = rsvIncludeFuel == 1 ? "Included - f: " + pkgFuel * days + "+ lh: " + pkglonghaul +" = " + fuelRate : "by renter";
            }
            else
            {   //guest user
                fuelRateTxt = rsvIncludeFuel == 1 ? "Included in the package" : "by renter";
            }

            return fuelRateTxt;
        }


        /**
         * Get total car rate 
         */
        private decimal getCarRateCalc(int carId, int days, decimal packageDailyAddon, decimal carRateAdditional)
        {
            //MealsAcc
            decimal totalCarRate = 0;
            var carRate = db.CarRates.Where(c => c.Id == carId).FirstOrDefault();
            decimal addedRates = carRateAdditional + (packageDailyAddon * days);

            if (days > 21)
            {
                totalCarRate = (carRate.Monthly * days);
            }

            if (days > 6)
            {
                if (22 > days)
                {
                    totalCarRate = (carRate.Weekly * days);
                }
            }

            if (6 > days)
            {
                totalCarRate = (carRate.Daily * days);
            }

            totalCarRate = (carRate.Daily * days);
            totalCarRate += addedRates;
            return totalCarRate;
        }

        /**
         * Get total Fuel rate text
         */
        private string getCarRateCalcTxt(int carId, int days, decimal packageDailyAddon, decimal carRateAdditional, int isAuthorize)
        {
           
            decimal totalCarRate = 0;
            var carRate = db.CarRates.Where(c => c.Id == carId).FirstOrDefault();
            decimal addedRates = carRateAdditional + (packageDailyAddon * days);
            string totalCarRateTxt = "";

            if (days > 21)
            {
                totalCarRate = (carRate.Monthly * days);
                totalCarRateTxt += "r: (" + carRate.Monthly + " / day ) = "+ totalCarRate + " +" ;
            }

            if (days > 6)
            {
                if (22 > days)
                {
                    totalCarRate = (carRate.Weekly * days);
                    totalCarRateTxt += "r: (" + carRate.Weekly + " / day ) = " + totalCarRate + " +";
                }
            }

            if (6 > days)
            {
                totalCarRate = (carRate.Daily * days);
                totalCarRateTxt += "r: (" + carRate.Daily + " / day ) = " + totalCarRate + " +";
            }
            
            totalCarRate = (carRate.Daily * days);
            totalCarRate += addedRates;

            totalCarRateTxt +=  " a: " + addedRates;
            

            if (isAuthorize == 0)
            {    //guest user
                totalCarRateTxt =  "" ;
            }


            return totalCarRateTxt;
        }


        #endregion
    }
}
