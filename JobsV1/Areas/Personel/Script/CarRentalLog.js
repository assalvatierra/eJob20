﻿
/**
 *  Car Rental Logs
 *  /Personel/CarRentalLog
 *  
 **/ 
$(document).ready(function () {
    InitDatePicker();
})

function InitDatePicker()
{
    var ddd1 = $('#filter-sdate').val();

    $('#filter-sdate').daterangepicker(
    {
        timePicker: false,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY'
        }
    },
    function (start, end, label) {
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
         
        }
    );

    $('#filter-sdate').val(ddd1);

    //end date
    var ddd1 = $('#filter-edate').val();

    $('#filter-edate').daterangepicker(
        {
            timePicker: false,
            timePickerIncrement: 1,
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: 'MM/DD/YYYY'
            }
        },
        function (start, end, label) {
            var today = moment().format('YYYY-MM-DD');
            var datepicker = start.format('YYYY-MM-DD');

        }
    );

    $('#filter-edate').val(ddd1);

}

function SetStartDate(days) {
    let sDate = moment().format("MM/DD/YYYY");
    $("#filter-sdate").val(moment(sDate).add(days, 'days').format("MM/DD/YYYY"));
}

/*
 * Odo Update Modal
 */ 
function Show_OdoUpdateModal(tripId) {
    $("#LogOdoUpdateModal").modal("show");
    $("#OdoUpdate-Id").val(tripId);

    //request odo details
    GetOdoDetails(tripId);
}

function GetOdoDetails(tripId) {
    if (tripId != null && tripId != 0) {
        $.get("/Personel/CarRentalLog/GetTripOdo", { id: parseInt(tripId) }, (response) => {
            //console.log(response);
            if (response != null) {
                $("#OdoUpdate-Start").val(response.Start);
                $("#OdoUpdate-End").val(response.End);
                $("#OdoUpdate-Date").text(response.Date);
                $("#OdoUpdate-Driver").text(response.Driver);
                $("#OdoUpdate-Unit").text(response.Unit);
                $("#OdoUpdate-Company").text(response.Company);
            } else {
                alert('Unable to get trip odo details');
            }
        });
    } else {
        alert('Unable to get trip odo details');
    }
}

function Submit_OdoUpdateForm() {
    var tripId = parseInt($("#OdoUpdate-Id").val());
    var odoStart = parseInt($("#OdoUpdate-Start").val());
    var odoEnd = parseInt($("#OdoUpdate-End").val());

    var odoData = {
        Id: tripId,
        OdoStart: odoStart,
        OdoEnd: odoEnd
    }
    //console.log(odoData)

    $.post("/Personel/CarRentalLog/SetTripOdo", odoData, (response) => {
        console.log(response);
        if (response == 'True') {
            var tripLog = $("#trip-" + tripId);
            tripLog.children(".trip-OdoStart").text(odoStart);
            tripLog.children(".trip-OdoEnd").text(odoEnd);
            $("#LogOdoUpdateModal").modal("hide");
        } else {
            alert("An error occured while updating the trip details.");
            $("#LogOdoUpdateModal").modal("hide");
        }
    });
} 


/*
 * OT Update Modal
 */
function Show_OTUpdateModal(tripId) {
    $("#LogOTUpdateModal").modal("show");
    $("#OTUpdate-Id").val(tripId);

    //request OT details
    GetOTDetails(tripId);
}

function GetOTDetails(tripId) {
    if (tripId != null && tripId != 0) {
        $.get("/Personel/CarRentalLog/GetTripOT", { id: parseInt(tripId) }, (response) => {
            console.log(response);
            if (response != null) {
                $("#OTUpdate-Date").text(response.Date);
                $("#OTUpdate-Driver").text(response.Driver);
                $("#OTUpdate-Unit").text(response.Unit);
                $("#OTUpdate-Company").text(response.Company);

                $("#OTUpdate-StartTime").val(response.StartTime);
                $("#OTUpdate-EndTime").val(response.EndTime);
                $("#OTUpdate-TripHours").val(response.TripHours);
                $("#OTUpdate-OTRate").val(response.OTRate);
                $("#OTUpdate-DriverOTRate").val(response.DriverOTRate);
            } else {
                alert('Unable to get trip details');
            }
        });
    } else {
        alert('Unable to get trip details');
    }
}

function Submit_OTUpdateForm() {
    var tripId    = parseInt($("#OTUpdate-Id").val());
    var startTime = $("#OTUpdate-StartTime").val();
    var endTime   = $("#OTUpdate-EndTime").val();
    var tripHours = parseInt($("#OTUpdate-TripHours").val());
    var otRate    = parseInt($("#OTUpdate-OTRate").val());
    var driverOTRate = parseInt($("#OTUpdate-DriverOTRate").val());

    var otData = {
        Id: tripId,
        StartTime: startTime,
        EndTime: endTime,
        TripHours: tripHours,
        OTRate: otRate,
        DriverOTRate: driverOTRate
    }

    console.log(otData)

    $.post("/Personel/CarRentalLog/SetTripOT", otData, (response) => {
        console.log(response);
        if (response) {
            window.location.reload(false);
            //var tripLog = $("#trip-" + tripId);
            //tripLog.children(".trip-OdoStart").text(odoStart);
            //tripLog.children(".trip-OdoEnd").text(odoEnd);
            //$("#LogOdoUpdateModal").modal("hide");
        } else {
            alert("An error occured while updating the trip details.");
            $("#LogOTUpdateModal").modal("hide");
        }
    });
} 



/**
 *  Reports Filters
 */

/**  Vehicle Summary Report Filter */

function Submit_VehicleSummaryFilter() {

    var startDate = $("#VehicleSummaryFilter-StartDate").val();
    var endDate = $("#VehicleSummaryFilter-EndDate").val();
    var unitId = $("#VehicleSummaryFilter-unitId").val();

    var url = $("#VehicleSummaryReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_unitId', unitId);
    //console.log(url)
    window.location.href = url;
}

function VehicleSummaryFilter_AddDays(days) {
    $("#VehicleSummaryFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}


/**  Vehicle Trip Report Filter */
function Submit_VehicleTripFilter() {

    var startDate = $("#VehicleTripFilter-StartDate").val();
    var endDate = $("#VehicleTripFilter-EndDate").val();
    var driverId = $("#VehicleTripFilter-driverId").val();
    var unitId = $("#VehicleTripFilter-unitId").val();

    var url = $("#VehicleTripReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_driverId', driverId);
    url = url.replace('_unitId', unitId);

    window.location.href = url;
}

function VehicleTripFilter_AddDays(days) {
    $("#VehicleTripFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}

function ClickShowShuttle() {
    //window.location.href = '/Personel/CarRentalLog?isShuttle=1';
    //console.log('set cookie ');

    var shuttle_cookie_prev = getCookie('shuttle_cookie');
    if (shuttle_cookie_prev) {
        //console.log('shuttle cookie : ' + shuttle_cookie);

        if (shuttle_cookie_prev == 1) {
            //set cookie for shuttle
            setCookie('shuttle_cookie', '0', 30);

            var shuttle_cookie = getCookie('shuttle_cookie');

            $("#isShuttle").addClass("btn-default");
            $("#isShuttle").removeClass("btn-primary");

            // console.log('shuttle cookie : ' + shuttle_cookie);

            window.location.reload();
        } else {
            setCookie('shuttle_cookie', '1', 30);

            var shuttle_cookie = getCookie('shuttle_cookie');

            $("#isShuttle").removeClass("btn-default");
            $("#isShuttle").addClass("btn-primary");

            //console.log('shuttle cookie : ' + shuttle_cookie);

            window.location.reload();
        }
    } else {
        setCookie('shuttle_cookie', '1', 30);

        $("#isShuttle").removeClass("btn-default");
        $("#isShuttle").addClass("btn-primary");

        // console.log('shuttle cookie added ');
        window.location.reload();
    }
}

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + ";expires=" + expires + ";path=/";
    console.log(document.cookie);
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

//Request PO
function RequestPOFuel(e, driverId, unitId) {
    $.post("/Personel/CarRentalLog/RequestPOFuel", { driverId: driverId, unitId: unitId }, (response) => {
        if (response) {
            $(e).text("Fuel Requested");
            $(e).prop("Disabled", true);
            $(e).parent().append('<a style="cursor:not-allowed;color:gray;"> Fuel Requested </a>')
            $(e).remove();

            //alert("Fuel Requested")
        } else {
            alert("Unable to request fuel.")
        }
    })
}

//Filter Date to Previous Day
function PrevDay(filteredsDate) {

    if (filteredsDate != "") {
        $("#filter-sdate").val(moment(filteredsDate).add(-1, 'days').format("MM/DD/YYYY"))
    } else {
        $("#filter-sdate").val(moment().add(-1, 'days').format("MM/DD/YYYY"))
    }

    if (filteredsDate != "") {
        $("#filter-edate").val(moment(filteredsDate).add(-1, 'days').format("MM/DD/YYYY"))
    } else {
        $("#filter-edate").val(moment().add(-1, 'days').format("MM/DD/YYYY"))
    }


    $("#SubmitFilterBtn").click();
}

//Filter Date to Next Day
function NextDay(filteredsDate) {

    if (filteredsDate != "") {
        $("#filter-sdate").val(moment(filteredsDate).add(1, 'days').format("MM/DD/YYYY"))
    } else {
        $("#filter-sdate").val(moment().add(1, 'days').format("MM/DD/YYYY"))
    }

    if (filteredsDate != "") {
        $("#filter-edate").val(moment(filteredsDate).add(1, 'days').format("MM/DD/YYYY"))
    } else {
        $("#filter-edate").val(moment().add(1, 'days').format("MM/DD/YYYY"))
    }

    $("#SubmitFilterBtn").click();
}
