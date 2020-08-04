

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
