/* ********************************************************
* By Abel S. Salvatierra
* @2016 - Real Wheels Car Rental Services
* 
*********************************************************** */

$(document).ready(function () {
    $('#aftercalc').hide();
    $('#ReservationDetails').hide();
    
    InitDatePicker();
    InitEditChange();
})


function InitDatePicker()
{
    $('input[name="DtStart"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 30,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY h:mm A'
        }
    },
    function (start, end, label) {
        //alert(start.format('YYYY-MM-DD h:mm A'));
        computeCost();
    }
    );

    $('input[name="DtEnd"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 30,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY h:mm A'
        }
    },
    function (start, end, label) {
        //alert(start.format('YYYY-MM-DD h:mm A'));
        computeCost();
    }
    );

    $('input[name="DtTrx"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 30,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY h:mm A'
        }
    },
    function (start, end, label) {
        //alert(start.format('YYYY-MM-DD h:mm A'));
        computeCost();
    }
    );


    /*
    $('input[name="DtRange"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 30,
        locale: {
            format: 'MM/DD/YYYY h:mm A'
        }
    },
    function (start, end, label) {
        $('input[name="DtStart"]').val(start.format('YYYY-MM-DD h:mm A'));
        $('input[name="DtEnd"]').val(end.format('YYYY-MM-DD h:mm A'));

        computeCost();
        }
    );*/

}

function InitEditChange() {
    $('input[name="EstHrPerDay"]').onchange(function () { computeCost(); });
    $('input[name="EstKmTravel"]').onchange(function () { computeCost(); });
}

function OpenLookup() {
    window.open("/CarRental/LookupDestination?cityid=1");
}

function ParentTest(Km) {
    $('input[name="EstKmTravel"]').val(Km);
    computeCost();
}

var _DAILY_USAGE_HOURS = 10;
var _OTRATE = 250;
var _DAILY_USAGE_KM = 100;
var _KM_CHARGE = 5;
function computeCost()
{
    var estCost = 0;


    var dtstart = new Date($('input[name="DtStart"]').val());
    var dtend = new Date ( $('input[name="DtEnd"]').val() );
    var diffDays = dateDiffInDays(dtstart, dtend) + 1;

    var carrate = $('input[name="Daily"]').val();
    if (diffDays >= 7)
        carrate = $('input[name="Weekly"]').val();
    if (diffDays >= 22)
        carrate = $('input[name="Monthly"]').val();

    var carrental = carrate * diffDays;
    estCost += carrental;

    var HrPerDay = $('input[name="EstHrPerDay"]').val();
    var DailyOTHrs = 0
    var estOvertime = 0;
    if ((HrPerDay - _DAILY_USAGE_HOURS) > 0) {
        DailyOTHrs = (HrPerDay - _DAILY_USAGE_HOURS);
    }
    estOvertime = (DailyOTHrs * diffDays) * _OTRATE;
    estCost += estOvertime;
    //alert(estOvertime);

    var KmTravel = $('input[name="EstKmTravel"]').val();
    var KmAllowance = _DAILY_USAGE_KM * diffDays;
    var KmExcess = 0;
    var estKmCharges = 0;
    if ((KmTravel - KmAllowance) > 0) {
        KmExcess = (KmTravel - KmAllowance);
    }
    estKmCharges = KmExcess * _KM_CHARGE;

    estCost += estKmCharges;
    //alert(estKmCharges);

    $('#estcomputation').html(
        "Number of Days:<span class='userData'>" + diffDays + "</span></br>" +
        "<span class='userRemarks'>" +
        "Daily Rate:" + addCommas(carrate) + " per day</span></br>" +
        "Rental Charges:<span class='userData'>" + addCommas(carrental) + "</span></br></br>" +

        "<span class='userRemarks'>" +
        "Daily Usage:" + HrPerDay + " Hrs</br>" +
        "Daily Allowance:" + _DAILY_USAGE_HOURS + " Hrs</br>" +
        "Daily Overtime:" + DailyOTHrs + " </br></span>" +
        "Overtime Charges:<span class='userData'>" + addCommas(estOvertime) + "</span></br></br>" +
        
        //"<span class='userRemarks'>" +
        //"Estimated Mileage:" + KmTravel + " Kms</br>" +
        //"Mileage Allowance:" + KmAllowance + " Kms</br>" +
        //"Excess Mileage :" + KmExcess + " Kms</span></br>" +
        //"Mileage Charges:<span class='userData'>" + addCommas(estKmCharges) + "</span></br>" +
        "------------------------------------------" + "</br>" +
        "<span class='userLabel-sm1'>Total Estimated Cost:</span><span class='userData'>" + addCommas(estCost) + "</span></br></br>"
        );

    $('#aftercalc').show();
    $('#ReservationDetails').show();
    $('#baseRate').val(estCost);
}

var _MS_PER_DAY = 1000 * 60 * 60 * 24;
// a and b are javascript Date objects
function dateDiffInDays(a, b) {
    // Discard the time and time-zone information.
    var utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
    var utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());
    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}

function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}



function checkRenterDetails(){
    var name, email, mobile, startdate, enddate, dest;
    name = $('#s-rnt-name').val();
    email = $('#s-rnt-email').val();
    mobile = $('#s-rnt-mobile').val();
    startdate = $('#s-rnt-startdate').val();
    enddate = $('#s-rnt-enddate').val();
    dest = $('#s-rnt-dest').val();
    purpose = $('#s-rnt-purpose').val();
    var flag = true;

    if (name != null && email != null && mobile != null) {
        $('#s-dtls-warning').text("Please enter your Name, Email and Mobile.");
    } 

    if (name == '' || name == null || email == '' || email == null || mobile == '' || mobile == null) {
        flag = false;
        $('#s-submit-btn').addClass('disabled');
        $('#s-dtls-warning').css('display', 'block');
    } 

    if (dest == '' || dest == null) {
        flag = false;
        $('#s-dtls-warning').text("destination field is empty.");
    }

    if (purpose == '' || purpose == null) {
        flag = false;
        $('#s-dtls-warning').text("Purpose field is empty.");
    }

    if (validateInputEmail()) {
    } else {
        flag = false;
        $('#s-dtls-warning').text("Email is not valid.");
    }

    if (validateInputPhone()) {
    } else {
        flag = false;
        $('#s-dtls-warning').text("Phone number is not valid.");
    }


    if (flag == true) {
        $('#s-submit-btn').removeClass('disabled');
        $('#s-dtls-warning').css('display', 'none');
    } else {
        $('#s-submit-btn').addClass('disabled');
        $('#s-dtls-warning').css('display', 'block');
    }
}


function validateInputEmail() {
    var fieldValue = document.getElementById("s-rnt-email").value;

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
    var fieldValue = document.getElementById("s-rnt-mobile").value;

    var phoneValidation = /^([\s\(\)\-]*\d[\s\(\)\-]*){11}$/;
    if (fieldValue.match(phoneValidation)) {
        // correct phone structure
        return true;
    } else {
        // incorrect structure
        return false;
    }
}




