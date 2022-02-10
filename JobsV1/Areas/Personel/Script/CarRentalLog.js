
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
                $("#OTUpdate-Remarks").val(response.Remarks);
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
    var remarks = $("#OTUpdate-Remarks").val();

    var otData = {
        Id: tripId,
        StartTime: startTime,
        EndTime: endTime,
        TripHours: tripHours,
        OTRate: otRate,
        DriverOTRate: driverOTRate,
        Remarks: remarks
    }

    console.log(otData)

    $.post("/Personel/CarRentalLog/SetTripOT", otData, (response) => {
        console.log(response);
        if (response) {
            window.location.reload(false);
        } else {
            alert("An error occured while updating the trip details.");
            $("#LogOTUpdateModal").modal("hide");
        }
    });
} 


/*
 * Expense Update Modal
 */
function Show_ExpenseUpdateModal(tripId) {
    $("#LogExpenseUpdateModal").modal("show");
    $("#ExpenseUpdate-Id").val(tripId);

    //request odo details
    GetExpenseDetails(tripId);
}

function GetExpenseDetails(tripId) {
    if (tripId != null && tripId != 0) {
        $.get("/Personel/CarRentalLog/GetTripOdo", { id: parseInt(tripId) }, (response) => {
            //console.log(response);
            if (response != null) {
                $("#ExpenseUpdate-Date").text(response.Date);
                $("#ExpenseUpdate-Driver").text(response.Driver);
                $("#ExpenseUpdate-Unit").text(response.Unit);
                $("#ExpenseUpdate-Company").text(response.Company);
            } else {
                alert('Unable to get trip Expense details');
            }
        });
    } else {
        alert('Unable to get trip Expense details');
    }
}

function Submit_ExpenseUpdateForm() {
    var tripId = parseInt($("#ExpenseUpdate-Id").val());
    var expense = parseInt($("#ExpenseUpdate-Expense").val());

    var expenseData = {
        Id: tripId,
        expense: expense
    }
    //console.log(odoData)

    $.post("/Personel/CarRentalLog/SetTripExpense", expenseData, (data, status, headers, config) => {
        console.log(status);
        console.log(status);
        if (status == "success") {
            $("#LogOdoUpdateModal").modal("hide");
            window.location.reload(false);
        } else {
            alert("An error occured while updating the trip Expense details.");
            $("#LogOdoUpdateModal").modal("hide");
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

//Show and Hide Shuttle Trips with cookie named 'shuttle_cookie'
//'shuttle_cookie' = 1, show shuttle trips only, hide other trips
//'shuttle_cookie' = 0, show all trips
function ClickShowShuttle() {
    var shuttle_cookie_prev = getCookie('shuttle_cookie');
    if (shuttle_cookie_prev) {

        if (shuttle_cookie_prev == 1) {
            //set cookie for shuttle 
            //Show All Trups, toggle button/switch OFF
            setCookie('shuttle_cookie', '0', 30);

            var shuttle_cookie = getCookie('shuttle_cookie');

            $("#isShuttle").prop('checked', false);

            window.location.reload();
        } else {
            //Show Shuttle, toggle button/switch ON
            setCookie('shuttle_cookie', '1', 30);

            var shuttle_cookie = getCookie('shuttle_cookie');

            $("#isShuttle").prop('checked', true);

            window.location.reload();
        }
    } else {

        //Show Shuttle, toggle button/switch ON
        setCookie('shuttle_cookie', '1', 30);

        $("#isShuttle").prop('checked', true);

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

//Update OT Rate of the trip
//Will calculate the OT hrs and rate based
//On the input Time and OTDriversRate
//param : id = triplogId
function UpdateOTRate(e, id) {
    $.post('/Personel/CarRentalLog/UpdateTripOTRate', { id: id })
        .done((res) => {
            console.log('Updated OT:' + id);
            //window.location.reload(false);
            $(e).parent().append('<a style="cursor:not-allowed;color:gray;"> OT Calculated </a>');
            $(e).remove();
        })
        .fail((err) => {
            console.log('Error OT update:' + id);
            alert('Unable to Update OT Rate');
        });
}


//Allow Edit trip logs 
//change AllowEdit Flag to True
function AllowEdit(e, id){
    $.post('/Personel/CarRentalLog/AllowEditTripLog', { id: id })
        .done((res) => {
            console.log('Allowed edit on triplog ' + id);
            window.location.reload(false);
            //$(e).parent().append('<a style="cursor:not-allowed;color:gray;"> OT Calculated </a>');
            $(e).remove();
        })
        .fail((err) => {
            console.log('Unable to edit:' + id + "\n " + err);
            alert('Unable to update Triplogs Flag : Allowed to Edit');
        });
}


//Allow Edit trip logs 
//change CloseForEdit Flag to True
function CloseForEdit(e, id) {
    $.post('/Personel/CarRentalLog/CloseForEditTripLog', { id: id })
        .done((res) => {
            console.log('Allowed edit on triplog ' + id);
            //window.location.reload(false);
            //$(e).parent().append('<a style="cursor:not-allowed;color:gray;"> OT Calculated </a>');
            $(e).remove();
        })
        .fail((err) => {
            console.log('Unable to edit:' + id + "\n " + err);
            alert('Unable to update Triplogs Flag : Allowed to Edit');
        });
}



//Handle Site Cookie, check shuttle_cookie to show shuttle services only when true, else show all trips
function CheckShuttleCookies() {
    var shuttle_cookie = getCookie('shuttle_cookie');
    if (shuttle_cookie) {

        if (shuttle_cookie == 1 || shuttle_cookie == '1') {
            $("#isShuttle").prop('checked', true);
        } else {

            $("#isShuttle").prop('checked', false);
        }
    } else {
        console.log('no shuttle cookie found');
    }
}

//Check table for duplicate drivers/Unit for today
function checkDriverUnitDuplicate() {

    $('#TripLogs-Table tr').each(function () {
        var date = $("#filter-edate").val();
        var tdId = $(this).attr('id');

        if (tdId != undefined) {
           
            var id = tdId.substring(5);
            CheckUnitRecordToday(this, id, date);
            CheckDriverRecordToday(this, id, date);
        }

    });

}

//check trip if it have duplciate record for driver/unit
function CheckUnitRecordToday(e, tripid, tripdate) {
    var img = "<img src='/Images/Icons/icons-warning.png' width='15' title='Duplicate Unit for today.' />";
    $.get('/Personel/CarRentalLog/GetTripWarningByUnit', { id: tripid, date: tripdate }, (res) => {
      
        if (res == "True") {
            $(e).children(".td-unit").prepend(img);
        } else {
        }
    });
}


//check trip if it have duplciate record for driver/unit
function CheckDriverRecordToday(e, tripid, tripdate) {
    var img = "<img src='/Images/Icons/icons-warning.png' width='15' title='Duplicate Driver for today.' />";
    $.get('/Personel/CarRentalLog/GetTripWarningByDriver', { id: tripid, date: tripdate }, (res) => {
      
        if (res == "True") {
            $(e).children(".td-driver").prepend(img);
        } else {
        }
    });
}

 
//POST Tripticket flag to true
function SetTripTicket(e,tripid) {
    $.post('/Personel/CarRentalLog/PostTripTicketFlag', { id: tripid }, (data, status) => {
        if (status == "success") {
            $(e).attr('disabled', 'disabled');
            $(e).parent('li').parent('ul').parent('div').parent('td').siblings('.td-date').append('<i class="fa fa-ticket"></i>')
        } else {
        }
    }).fail((status) => {
        alert("Unable to update trip ticket flag.");
    });
}
