
/******** Form Packages ************/
//global
var selectedTour = 2;

$(document).ready(function () {
    //popup the proceed button
    $('[data-toggle="popover"]').popover({
        placement: 'right',
        html: 'true',
        content: '<button class="btn btn-primary" onclick="FormPackages_NextForm();">Proceed</button>'
    });

    //hide other popovers
    $('[data-toggle="popover"]').on('click', function (e) {
        $('[data-toggle="popover"]').not(this).popover('hide');
    });

});

//update reservation text 
function formPackages_TransText(carId, days, meals, fuel, RentTypeTxt, carDesc) {

    var carId = carId;
    var noDays = days;
    var mealsAcc = meals;
    var fuel = fuel;
    var mealPackage = mealsAcc == '1' ? 'Driver Meals/Accomodation Included ' : 'Driver Meals/Accomodation by renter';
    var fuelPackage = fuel == '1' ? 'Driver Fuel Included ' : 'Driver Fuel by renter';

    //update text
    $('#modal-text-foot').text(RentTypeTxt + ' - ' + carDesc + ' - ' + noDays + ' Days - ' + mealPackage + ' - ' + fuelPackage + '');
}


//public PartialViewResult FormPackages (int? carId, int? days, int? rentType, int? meals, int? fuel)
function FormPackages_Next(controller, carId, noDays, rentalType, meals, fuel, pkg) {
    window.location.href = '/' + controller + '/FormSummary?carId=' + carId + '&days=' + noDays +
        '&rentType=' + rentalType + '&meals=' + meals + '&fuel=' + fuel + '&pkg=' + pkg;
}

//public PartialViewResult FormPackages (int? carId, int? days, int? rentType, int? meals, int? fuel)
function FormPackages_Prev(controller,carId, noDays, rentalType, meals, fuel, pkg) {
    window.location.href = '/' + controller + '/CarReserve?id=' + carId + '&days=' + noDays +
        '&rentType=' + rentalType + '&meals=' + meals + '&fuel=' + fuel + '&pkg=' + pkg;
}

//packages selection on click
$('#pkg-table').on('click', '.clickable-row', function (event) {
    $(this).addClass('active').siblings().removeClass('active');
    selectedTour = $(this).attr('id').substring(4, 6);
});

function LoadOverlay() {
    $("#overlay").css("display", "flex");
}