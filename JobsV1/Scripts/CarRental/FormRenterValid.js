
/*
 * Validate Reservation Forms
 */ 

var captchaflag = false;
//generate validations on inputs
$(document).ready(function () {
    //initial
    rentDateFilter();
    checkRenterDetails();

    $("#rnt-startDate").change(function () {
        rentDateFilter();
    });

    $("#rnt-endDate").change(function () {
        rentDateFilter();
    });


    $("#rnt-name").change(function () {
        checkRenterDetails();
    });

    $("#rnt-email").change(function () {
        checkRenterDetails();
    });

    $("#rnt-mobile").change(function () {
        checkRenterDetails();
    });


    $("#rsvDate-DropOff").change(function () {
        rentDateFilterInit();
    });

    $("#rsvDate-PickUp").change(function () {
        rentDateFilterInit();
    });


});

/**
 * Ajax sample on sending data to server
 *
 */

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
        RenterEmail : 'jahdielsvillosa@gmail.com',
        RenterMobile : '09123456',
        RenterAddress : '',
        RenterFbAccnt : '',
        RenterLinkedInAccnt : '',
        EstHrPerDay : '',
        EstKmTravel : '',
        host: getUrl()
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


//Check renter name, email and number if not null
//check rent dates if valid
function checkRenterDetails() {
    var name, email, mobile, startdate, enddate;
    name = $('#rnt-name').val();
    email = $('#rnt-email').val();
    mobile = $('#rnt-mobile').val();
    startdate = $('#rnt-startdate').val();
    enddate = $('#rnt-enddate').val();
    var flag = true;


    //check if captcha input is valid
    //if (!captchaflag) {
    //    $('#submit-btn').removeClass('disabled');
    //    $('#dtls-warning').text("Please check the Captcha form.");
    //}


    //validate inputs if null or empty
    if (name == '' || name == null ) {
        flag = false;
        $('#dtls-warning').text("Please enter your Name.");

    }
    //validate inputs if null or empty
    if (name == '' && email == '' && mobile == '') {
    
        flag = false;
        $('#dtls-warning').text("Please enter your Name, Email and Mobile.");

    } else {

        //check if phone input is valid
        if (!validateInputPhone()) {
            flag = false;
            $('#dtls-warning').text("Phone number is not valid.");
        }

        //check if email input is valid
        if (!validateInputEmail()) {
            flag = false;
            $('#dtls-warning').text("Email is not valid.");
        }
    }

    if (flag == true) {
        $('#submit-btn').removeClass('disabled');
        $('#submit-btn').removeClass('btn-default');
        $('#submit-btn').addClass('btn-primary');
        $('#dtls-warning').css('display', 'none');
    } else {
        $('#submit-btn').addClass('disabled');
        $('#dtls-warning').css('display', 'block');
    }

    return flag;

}


//validate inputs if contains the correct format
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

//validate inputs if containes number
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

function LoadOverlay() {
    $("#overlay").css("display", "flex");
}

/**
* Handles Start Date of Reservation
* start date must be 2 days from today
* and end date must be 1 day from start date by default
*
*/

function rentDateFilter() {
    var flag = true;
    var sdate = Date.parse($('input[name="DtStart"]').val());
    var edate = Date.parse($('input[name="DtEnd"]').val());
    var today = new Date();
    var todayPlus4 = new Date();
    today.setDate(today.getDate() + 2);

    //reset time
    var Ssdate = getDateFormat(new Date(sdate));
    var Sedate = getDateFormat(new Date(edate));
    var Stoday = getDateFormat(today);

    //hide warning message

    //start date is greater than or equal to today - must always true
    if (+Date.parse(Ssdate) >= +Date.parse(Stoday)) {
        flag = true;

        //handles invalid date start and date end
        if (+Date.parse(Ssdate) <= +Date.parse(Sedate)) {
            flag = true;
        } else if (+Date.parse(Sedate) < +Date.parse(Stoday)) {
            $('input[name="DtEnd"]').val(moment(Stoday).format("MM/DD/YYYY"));
            flag = false;
        } else {
            //display warning message - invalid date start and date end
            flag = false;
        }

        //start date is less than today
    } else if (+Date.parse(Ssdate) < +Date.parse(Stoday)) {
        $('input[name="DtStart"]').val(moment(Stoday).format("MM/DD/YYYY"));
        flag = false;
    }

    //check renter details;
    checkRenterDetails();

    //update days count
    var daysCount = 0;
    daysCount = Math.round(moment(edate).diff(moment(sdate), 'days', true));

    if (daysCount == 0) {
        daysCount = 1;
    } else if (daysCount > 0) {
        daysCount = daysCount + 1;
    }
    else {
        daysCount = 0;
    }
    $("#rnt-noDays").val(daysCount);

    //generate warning message
    return DateTimeWarning(flag);
}

//generate warning message, 
//disable submit button on false
//enable submit button on true
function DateTimeWarning(flag) {

    var sdate = Date.parse($('input[name="DtStart"]').val());
    var edate = Date.parse($('input[name="DtEnd"]').val());
    var today = new Date();
    today.setDate(today.getDate() + 2);

    //reset time
    var Ssdate = getDateFormat(new Date(sdate));
    var Sedate = getDateFormat(new Date(edate));
    var Stoday = getDateFormat(today);

    if (flag == true) {
        //enable submit button
        $('#submit-btn').removeClass('disabled');

        //console.log(+Ssdate <= +today);
        //console.log("OK - SDATE: " + Ssdate + " >= TODAY: " + Stoday);
        //console.log(+Ssdate <= +Sedate);
        //console.log("OK - Ssdate: " + Ssdate + " <= Sedate: " + Sedate);

        //hide waring message
        $("#dtls-warning-usage").text("");
        $('#dtls-warning-usage').css('display', 'none');

    } else {
        //disable submit button
        $('#submit-btn').addClass('disabled');

        //console.log(+Ssdate <= +today);
        //console.log("INVALID - Ssdate: " + Ssdate + " <= Sedate: " + Sedate);
        //console.log(+Ssdate <= +Sedate);
        //console.log("INVALID - SDATE: " + Ssdate + " < TODAY: " + Stoday);

        //show waring message
        $("#dtls-warning-usage").text("Entered Date is invalid.");
        $('#dtls-warning-usage').css('display', 'block');
    }


    return flag;
}


function rentDateFilterInit() {
    var flag = true;
    var sdate = Date.parse($('#rsvDate-PickUp').val());
    var edate = Date.parse($('#rsvDate-DropOff').val());
    var today = new Date();
    var todayPlus4 = new Date();
    today.setDate(today.getDate() + 2);

    //reset time
    var Ssdate = getDateFormat(new Date(sdate));
    var Sedate = getDateFormat(new Date(edate));
    var Stoday = getDateFormat(today);

    //hide warning message

    //start date is greater than or equal to today - must always true
    if (+Date.parse(Ssdate) >= +Date.parse(Stoday)) {
        flag = true;

        //handles invalid date start and date end
        if (+Date.parse(Ssdate) <= +Date.parse(Sedate)) {
            flag = true;
        } else if (+Date.parse(Sedate) < +Date.parse(Stoday)) {
            $('#rsvDate-DropOff').val(moment(Stoday).format("MM/DD/YYYY"));
            flag = false;
        } else {
            //display warning message - invalid date start and date end
            flag = false;
        }

        //start date is less than today
    } else if (+Date.parse(Ssdate) < +Date.parse(Stoday)) {
        $('#rsvDate-PickUp').val(moment(Stoday).format("MM/DD/YYYY"));
        flag = false;
    }

    //check renter details;
    checkRenterDetails();

    //update days count
    var daysCount = 0;
    daysCount = Math.round(moment(edate).diff(moment(sdate), 'days', true));

    if (daysCount == 0) {
        daysCount = 1;
    } else if (daysCount > 0) {
        daysCount = daysCount + 1;
    }
    else {
        daysCount = 0;
    }
    $("#rnt-noDays").val(daysCount);

    //generate warning message
    return DateTimeWarningInit(flag);
}

function DateTimeWarningInit(flag) {

    var sdate = Date.parse($('#rsvDate-PickUp').val());
    var edate = Date.parse($('#rsvDate-DropOff').val());
    var today = new Date();
    today.setDate(today.getDate() + 2);

    //reset time
    var Ssdate = getDateFormat(new Date(sdate));
    var Sedate = getDateFormat(new Date(edate));
    var Stoday = getDateFormat(today);

    if (flag == true) {
        //enable submit button
        $('#rsv-submit-btn').removeClass('disabled');

        //console.log(+Ssdate <= +today);
        //console.log("OK - SDATE: " + Ssdate + " >= TODAY: " + Stoday);
        //console.log(+Ssdate <= +Sedate);
        //console.log("OK - Ssdate: " + Ssdate + " <= Sedate: " + Sedate);

        //hide waring message
        $("#rsv-warning-usage").text("");
        $('#rsv-warning-usage').css('display', 'none');

    } else {
        //disable submit button
        $('#submit-btn').addClass('disabled');

        //console.log(+Ssdate <= +today);
        //console.log("INVALID - Ssdate: " + Ssdate + " <= Sedate: " + Sedate);
        //console.log(+Ssdate <= +Sedate);
        //console.log("INVALID - SDATE: " + Ssdate + " < TODAY: " + Stoday);

        //show waring message
        $("#rsv-warning-usage").text("Entered Date is invalid.");
        $('#rsv-warning-usage').css('display', 'block');
    }


    return flag;
}

/**
* Handles Start Date of Reservation
* start date must be 2 days from today
* and end date must be 1 day from start date by default
*
*/

function getDateFormat(currentDt) {

    var mm = currentDt.getMonth() + 1;
    var dd = currentDt.getDate();
    var yyyy = currentDt.getFullYear();
    var fDate = mm + '/' + dd + '/' + yyyy + " 9:00 AM";

    return fDate;
}


function ValidateRequestForm() {

    rentDateFilter();
}