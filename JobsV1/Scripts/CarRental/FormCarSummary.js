
/******** Form Summary ************/

//public PartialViewResult FormPackages (int? carId, int? days, int? rentType, int? meals, int? fuel)
function FormSummary_Next(controller,carId, noDays, rentalType, meals, fuel, pkgId) {

    window.location.href = '/' + controller + '/FormRenter?id=' + carId + '&days=' + noDays +
        '&rentType=' + rentalType + '&meals=' + meals + '&fuel=' + fuel + '&pkg=' + pkgId;
}

//public PartialViewResult FormPackages (int? carId, int? days, int? rentType, int? meals, int? fuel)
function FormSummary_Prev(controller,carId, noDays, rentalType, meals, fuel, pkg) {
    window.location.href = '/' + controller + '/FormPackages?carId=' + carId + '&days=' + noDays +
        '&rentType=' + rentalType + '&meals=' + meals + '&fuel=' + fuel;
}

//Display reservation summary at the header of the form
function formPackages_TransText(carId, days, meals, fuel, RentTypeTxt, carDesc) {

    var carId = carId;
    var noDays = days;
    var mealsAcc = meals;
    var fuel = fuel;

    var mealPackage = mealsAcc == '1' ? 'Driver Meals/Accomodation Included ' : 'Driver Meals/Accomodation by renter';
    var fuelPackage = fuel == '1' ? 'Driver Fuel Included ' : 'Driver Fuel by renter';

    $('#modal-text-foot').text(RentTypeTxt + ' - ' + carDesc + ' - ' + noDays + ' Days - ' + mealPackage + ' - ' + fuelPackage + '');
}


function LoadOverlay() {
    $("#overlay").css("display", "flex");
}