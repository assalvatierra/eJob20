

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
            //window.location.reload();
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
