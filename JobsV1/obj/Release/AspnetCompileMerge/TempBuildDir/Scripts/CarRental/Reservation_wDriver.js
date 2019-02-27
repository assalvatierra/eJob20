
//globals
var currentForm = "Modal";
var selectedCar = "";
var RentalType = "With Driver";
var NoDays = 0;
var MealsAcc = 0;
var Fuel = 0;
var selectedTour = 0;
var reservationNum = 0;


//initial-default
$('#1').attr('checked', true);
$('#withdriver').attr('checked', true);
$('#car' + $('input:radio[name=cars]:checked').attr("id")).css('border', '2px solid dodgerblue');
$('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - ' + $('input:radio[name=cars]:checked').val());
$('#pkg-2').attr('checked', true);
$('#pkg-2').addClass('active').siblings().removeClass('active');
selectedTour = $('#pkg-2').attr('title');

function rentalTypeChange() {
    $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - ' + $('input:radio[name=cars]:checked').val());

    if ($('input:radio[name=options]:checked').val() == "Self Drive") {
        if ($('input:radio[name=cars]:checked').attr("id") == 1 || $('input:radio[name=cars]:checked').attr("id") == 2) {
            radiobtn = document.getElementById("3");
            radiobtn.checked = true;
            $('#car1').css('border', '1px solid lightgray');
            $('#car2').css('border', '1px solid lightgray');
            $('#car4').css('border', '1px solid lightgray');
            $('#car5').css('border', '1px solid lightgray');
            $('#car6').css('border', '1px solid lightgray');
        }

        $('#car' + $('input:radio[name=cars]:checked').attr("id")).css('border', '2px solid dodgerblue');

        $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - ' + $('input:radio[name=cars]:checked').val());

        $('.isSelfDrive1').hide();
    } else {

        $('#1').attr('checked', true);
        $('#car' + $('input:radio[name=cars]:checked').attr("id")).css('border', '2px solid dodgerblue');

        $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - ' + $('input:radio[name=cars]:checked').val());

        $('.isSelfDrive1').show();
    }
}

$('#btn-rentalType').click(function () {
    $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - ' + $('input:radio[name=cars]:checked').val());
    rentalReset();
    $('#3').attr('checked', true);

    if ($('input:radio[name=options]:checked').val() == "Self Drive") {
        if ($('input:radio[name=cars]:checked').attr("id") == 1) {
            $('#3').attr('checked', true);
        }

        $('#rental-type-sdrive').addClass('active').siblings().removeClass('active');

        $('#car' + $('input:radio[name=cars]:checked').attr("id")).css('border', '2px solid dodgerblue');

        $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - ' + $('input:radio[name=cars]:checked').val());

        $('.isSelfDrive1').hide();

    } else {

        $('#rental-type-wdriver').addClass('active').siblings().removeClass('active');

        $('#1').attr('checked', true);

        $('#car' + $('input:radio[name=cars]:checked').attr("id")).css('border', '2px solid dodgerblue');

        $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - ' + $('input:radio[name=cars]:checked').val());

        $('.isSelfDrive1').show();
    }
});

//
$('#btn-rentalUnit').click(function () {
    $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - ' + $('input:radio[name=cars]:checked').val());
    $('#car1').css('border', '1px solid lightgray');
    $('#car2').css('border', '1px solid lightgray');
    $('#car3').css('border', '1px solid lightgray');
    $('#car4').css('border', '1px solid lightgray');
    $('#car5').css('border', '1px solid lightgray');
    $('#car6').css('border', '1px solid lightgray');
    $('#car' + $('input:radio[name=cars]:checked').attr("id")).css('border', '2px solid dodgerblue');
});


$('#pkg-table').on('click', '.clickable-row', function (event) {
    $(this).addClass('active').siblings().removeClass('active');
    selectedTour = $(this).attr('title');
});


function rentalReset() {
    $('#modal-text-foot').text($('input:radio[name=options]:checked').val() + ' - Please select a vehicle');

    $('#car1').css('border', '1px solid lightgray');
    $('#car2').css('border', '1px solid lightgray');
    $('#car3').css('border', '1px solid lightgray');
    $('#car4').css('border', '1px solid lightgray');
    $('#car5').css('border', '1px solid lightgray');
    $('#car6').css('border', '1px solid lightgray');

    var selectedCar = "";

}

//controls the flow of reservation forms
//display the next form
function showForm() {

    RentalType = $('input:radio[name=options]:checked').val();
    selectedCar = $('input:radio[name=cars]:checked').attr("id");
    NoDays = $('#rsv-days').val();
    MealsAcc = $('#rsv-meal').val();
    Fuel = $('#rsv-fuel').val();

    //self drive
    if ($('input:radio[name=options]:checked').val() == "Self Drive") {
   
        selfDrive($('input:radio[name=cars]:checked').attr("id"));
    }

    switch (currentForm) {
        case "Modal": //next FormPackages
            currentForm = "FormPackages";
            $('#modal2-content').css('display', 'none');
            $('#formReserve').css('display', 'none');
            $('#formPackages').css('display', 'block');
            $('#formSummary').css('display', 'none');
            $('#formRenter').css('display', 'none');
            $('#formFinish').css('display', 'none');
            $('#rsv-footer').css('display','block');
            displayPackages();
            break;
        case "FormPackages": //next formSummary
            currentForm = "formSummary";
            $('#modal2-content').css('display', 'none');
            $('#formPackages').css('display', 'none');
            $('#formReserve').css('display', 'none');
            $('#formSummary').css('display', 'block');
            $('#formRenter').css('display', 'none');
            $('#formFinish').css('display', 'none');
            $('#rsv-footer').css('display','block');
            displaySummary();
            break;
        case "formSummary":  //next formRenter
            currentForm = "formRenter";
            $('#modal2-content').css('display', 'none');
            $('#formPackages').css('display', 'none');
            $('#formReserve').css('display', 'none');
            $('#formSummary').css('display', 'none');
            $('#formRenter').css('display', 'block');
            $('#formFinish').css('display', 'none');
            $('#rsv-footer').css('display', 'none');
            setReferenceIds(selectedTour, MealsAcc, Fuel);
            checkRenterDetails();
            break;
        case "formRenter":  //next formThankyou
            currentForm = "formThankyou";
            $('#modal2-content').css('display', 'none');
            $('#formPackages').css('display', 'none');
            $('#formReserve').css('display', 'none');
            $('#formSummary').css('display', 'none');
            $('#formRenter').css('display', 'none');
            $('#formFinish').css('display', 'block');
            $('#rsv-footer').css('display','none');
            break;
    }

    $('#FormPackages_carid').val($('input:radio[name=cars]:checked').attr("id"));
}

//displays the vehicle selection form
function backtoMain() {
    currentForm = "Modal";
    $('#modal2-content').css('display', 'block');
    $('#formReserve').css('display', 'none');
    $('#formPackages').css('display', 'none');
    $('#formSummary').css('display', 'none');
    $('#formRenter').css('display', 'none');
    $('#formFinish').css('display', 'none');
    $('#rsv-footer').css('display','block');

}

//display the previous form
function backtoPrev() {

    switch (currentForm) {
        case "Modal": //prev none
            currentForm = "Modal";
            $('#modal2-content').css('display', 'block');
            $('#formReserve').css('display', 'none');
            $('#formPackages').css('display', 'none');
            $('#formSummary').css('display', 'none');
            $('#formRenter').css('display', 'none');
            break;
        case "FormPackages": //prev Modal
            currentForm = "Modal";
            $('#modal2-content').css('display', 'block');
            $('#formReserve').css('display', 'none');
            $('#formPackages').css('display', 'none');
            $('#formSummary').css('display', 'none');
            $('#formRenter').css('display', 'none');
            break;
        case "formSummary":  //next back to main
            currentForm = "FormPackages";
            $('#modal2-content').css('display', 'none');
            $('#formReserve').css('display', 'none');
            $('#formPackages').css('display', 'block');
            $('#formSummary').css('display', 'none');
            $('#formRenter').css('display', 'none');
            displayPackages();
            break;
        case "formRenter":  //next summary
            currentForm = "formSummary";
            $('#modal2-content').css('display', 'none');
            $('#formPackages').css('display', 'none');
            $('#formReserve').css('display', 'none');
            $('#formSummary').css('display', 'block');
            $('#formRenter').css('display', 'none');
            break;
    }
}


function submitRenter(){

    data = {
        Id : 0,
        DtTrx : '9/22/2018',
        CarUnitId : '1',
        DtStart : '9/22/2018',
        LocStart : 'somewhere',
        DtEnd : '9/22/2018',
        LocEnd : 'somewhere',
        BaseRate : '2000',
        Destinations : 'Samal Tour',
        UseFor : 'somewhere',
        RenterName : 'somewhere',
        RenterCompany : 'somewhere',
        RenterEmail : 'somewhere@gmail.com',
        RenterMobile : '09123456',
        RenterAddress : '',
        RenterFbAccnt : '',
        RenterLinkedInAccnt : '',
        EstHrPerDay : '',
        EstKmTravel : ''
    }


    var serviceURL = '/CarRental/FormRenterPOST';

    $.ajax({
        type: "POST",
        url: serviceURL,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc(),
        error: errorFunc()
    });
}

function ajaxSubmitData() {
    var data = {
        reservationid: reservationNum,
        pacakgeId: selectedTour,
        mealAcc: MealsAcc,
        fuel: Fuel
    }
    var serviceURL = '/SMSWebService.asmx/reservationPkg';

    $.ajax({
        type: "POST",
        url: serviceURL,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(){
            console.log('success');
        },
        error: function(jqXHR, textStatus, errorThrown) {
            console.log('An error occurred... Look at the console (F12 or Ctrl+Shift+I, Console tab) for more information!');

            $('#result').html('<p>status code: '+jqXHR.status+'</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>'+jqXHR.responseText + '</div>');
            console.log('jqXHR:');
            console.log(jqXHR);
            console.log('textStatus:');
            console.log(textStatus);
            console.log('errorThrown:');
            console.log(errorThrown);
        }
    });
}

function selfDrive(id){
    window.location.href = '/CarRental/Reservation?unitid='+id ;
}

function setReferenceIds(RntrSelectedTour, RntrMealsAcc, RntrFuel) {
    $('#dtls-packageId').val(RntrSelectedTour);
    $('#dtls-mealsAcc').val(RntrMealsAcc);
    $('#dtls-fuel').val(RntrFuel);
}

function checkRenterDetails() {
    var name, email, mobile, startdate, enddate;
    name = $('#rnt-name').val();
    email = $('#rnt-email').val();
    mobile = $('#rnt-mobile').val();
    startdate = $('#rnt-startdate').val();
    enddate = $('#rnt-enddate').val();
    var flag = true;


    if (name == '' || name == null || email == '' || email == null || mobile == '' || mobile == null) {
        flag = false;
        $('#dtls-warning').text("Please enter your Name, Email and Mobile.");
      
    }

    if (validateInputPhone()) {
    } else {
        flag = false;
        $('#dtls-warning').text("Phone number is not valid.");
    }

    if (validateInputEmail()) {
    } else {
        flag = false;
        $('#dtls-warning').text("Email is not valid.");
    }


    if (name != null && email != null && mobile != null) {
    } else {
        flag = false;
        $('#dtls-warning').text("Please enter your Name, Email and Mobile.");

    }

    if (flag == true) {
        $('#submit-btn').removeClass('disabled');
        $('#dtls-warning').css('display', 'none');
    } else {
        $('#submit-btn').addClass('disabled');
        $('#dtls-warning').css('display', 'block');
    }
}


function  validateInputEmail() {
    var fieldValue = document.getElementById("rnt-email").value;

    var mailValidation = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (fieldValue.match(mailValidation)) {
        // correct mail format
        return true;
    } else {
        // incorrect structure
        return false;
    }
}

function validateInputPhone() {
    var fieldValue = document.getElementById("rnt-mobile").value;

    var phoneValidation = /^([\s\(\)\-]*\d[\s\(\)\-]*){11}$/;
    if (fieldValue.match(phoneValidation)) {
        // correct phone structure
        return true;
    } else {
        // incorrect structure
        return false;
    }
}

//handle the count of number of days 
$('#days-add').click(function () {
    $('#rsv-days').val(+$('#rsv-days').val() + 1);
});
$('#days-sub').click(function () {
    if ($('#rsv-days').val() > 1) {
        $('#rsv-days').val(+$('#rsv-days').val() - 1);
    }
});

function dayschange() {
    if ($('#rsv-days').val() < 1) {
        $('#rsv-days').val(1);
    }
}


//long term rental
function longtermRental() {
    currentForm = "FormPackages";
    selectedTour = $('#pkg-1').attr('title');
    showForm();
}