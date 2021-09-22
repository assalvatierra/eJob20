
/**
 *  CarRentalLogFinalize.js
 *  /Personal/CarRentalLog/Index
 * 
 */

function ShowFinalizeTrip(e) {
    if (!$("#tb-hd-finalize").is(":visible")) {
        $("#tb-hd-finalize").show();
        $(".trip-Finalize").show();
        $("#FinalizeTrip-Btn").show();
        $(e).text("Hide Finalize Trips")
    } else {
        $("#tb-hd-finalize").hide();
        $(".trip-Finalize").hide();
        $("#FinalizeTrip-Btn").hide();
        $(e).text("Show Finalize Trips")
    }
}

function FinalizeAll() {
    $('#TripLogs-Table tr td.trip-Finalize input[type="checkbox"]').each(function () {
        $(this).prop('checked', true);
    });
}


function FinalizeCheckedTrips() {
    ShowLoading();
    let doneCount = 0;
    if ($("#tb-hd-finalize").is(":visible")) {
        $('#TripLogs-Table tr td.trip-Finalize input[type="checkbox"]').each(function () {
            doneCount = doneCount + 1;
            if ($(this).is(':checked')) {
                console.log("Checked " + $(this).val());

                let tripId = $(this).val();
                if (tripId != null || tripId != undefined) {
                    FinalizeTrip(this, tripId);
                }
            }
        });
    } else {
        alert("No Selected Trips");
    }

    if (doneCount == 0) {
        alert("No Selected Trips");
    }

    if (GetSelectedTripCount() == doneCount) {
        HideLoading();
    }
}



function GetSelectedTripCount() {
    let tripCount = 0;
    $('#TripLogs-Table tr td.trip-Finalize input[type="checkbox"]').each(function () {
        tripCount = tripCount + 1;
    });

    return tripCount;
}

function FinalizeTrip(e, tripId) {
    $.post("/Personel/CarRentalLog/SetTripFinalize", { id: tripId })
        .done((data, status, headers) => {
            console.log('done');
            console.log(headers["status"]);

            if (headers["status"] == 200) {
                $(e).parent().parent().addClass('success');
                $(e).parent().siblings('.actions').children().hide();
                $(e).hide();
            } else {
                alert("Unable to Finalize Selected Trips");
            }

        })
        .fail((data, status, headers) => {
            console.log('fail');
            console.log(data);
        });
}


//Allow Edit 

function ShowAllowEdit(e) {
    if (!$("#tb-hd-AllowEdit").is(":visible")) {
        $("#tb-hd-AllowEdit").show();
        $(".trip-AllowEdit").show();
        $("#AllowEditGroup-Btn").show();
        $(e).text("Hide Finalize Trips")
    } else {
        $("#tb-hd-AllowEdit").hide();
        $(".trip-AllowEdit").hide();
        $("#AllowEditGroup-Btn").hide();
        $(e).text("Show Allow Edit Trips")
    }
}

function AllowEditAll() {
    $('#TripLogs-Table tr td.trip-AllowEdit input[type="checkbox"]').each(function () {
        $(this).prop('checked', true);
    });
}
function AllowEditCheckedTrips() {
    ShowLoading();
    let doneCount = 0;
    if ($("#tb-hd-AllowEdit").is(":visible")) {
        $('#TripLogs-Table tr td.trip-AllowEdit input[type="checkbox"]').each(function () {
            doneCount = doneCount + 1;
            if ($(this).is(':checked')) {
                console.log("Checked " + $(this).val());

                let tripId = $(this).val();
                if (tripId != null || tripId != undefined) {
                    AllowEdit(this, tripId);
                }
            }
        });
    } else {
        alert("No Selected Trips");
    }

    if (doneCount == 0) {
        alert("No Selected Trips");
    }

    if (GetSelectedAllowedEditTripCount() == doneCount) {
        HideLoading();
    }
}


function GetSelectedAllowedEditTripCount() {
    let tripCount = 0;
    $('#TripLogs-Table tr td.trip-AllowEdit input[type="checkbox"]').each(function () {
        tripCount = tripCount + 1;
    });

    return tripCount;
}
