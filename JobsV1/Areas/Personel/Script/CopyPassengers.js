/* CopyPassengers.js
 * For Copying Passengers in Trip logs
 */ 
var SELECTED_ID = 0;
var TRIP_ID = 0;

$(document).ready(() => {
    //Initialize
});

function ShowCopyLogsModal(id) {
    $("#CopiedTripLogModal").modal("show");

    //set selectedID
    SetSelectedID(id);
}

function ShowTripPassengersModal(id) {
    $("#CopiedTripPassengersModal").modal("show");

    //set Trip Id
    SetSelectedTripID(id);
}

function SetSelectedID(selected_Id) {
    this.SELECTED_ID = selected_Id;
}

function SetSelectedTripID(trip_Id) {
    this.TRIP_ID = trip_Id;
}

//submit
function SelectTripToCopy(source_tripId) {
    //console.log("selected_Id : " + SELECTED_ID);
    //console.log("tripId : " + source_tripId);

    ShowTripPassengersModal(source_tripId);
    GetPassengerList(source_tripId);


    $('#select-all-checkbox').prop('checked', false);
}

//get list if passengers
function GetPassengerList(tripId) {
    $.get("/Personel/crLogPassengers/GetPassengerList", { id: tripId }, (result) => {
        // Unchecks it
        var passengers = JSON.parse(result);
        if (passengers.length > 0) {
            //console.log(passengers);

            //clear passengers
            $("#CopyFrom-TripPassengerList").find("tr:gt(0)").remove();

            //add data to table
            for (var i = 0; i < passengers.length; i++) {
                var psgr = "<tr>" +
                    + "<td><span>" + passengers[i]["Id"] + "</span></td>"
                    + "<td>" + passengers[i]["Name"] + "</td>"
                    + "<td > <input type='checkbox' value='" + passengers[i]["Id"] + "' class='item-checkbox'> </td>"
                    + "</tr>";

                $("#CopyFrom-TripPassengerList").append(psgr);
            }


        } else {
            alert("An Error occured while copying.");
        }
    });
}

function ShowData_CopyTripPassengers() {
    console.log("selected_Id : " + SELECTED_ID);
    console.log("tripId : " + TRIP_ID);
    console.log("Passenger List : " + PASS_LIST);
}

function Cancel_SubmitPassCopyLogs() {
}


function SubmitPassCopy() {
    let cTripLogId = $("#copiedTripLog").val();
    let count = 0;

    $('#CopyFrom-TripPassengerList tr:gt(0)').each(function () {
        var current_row = $(this);
        var passenger_Id = current_row.find("td:eq(1)").children('input:checked').val();

        //submit and copy logs to new logs
        if (passenger_Id != undefined) {
            ajax_CopyPassengers(passenger_Id, SELECTED_ID);
        }

        count++;
    });

}

function ajax_CopyPassengers(PassengerId, Dest_TripId) {

    var res = $.post("/Personel/crLogPassengers/CopyTripPassengers", { destTripId: Dest_TripId, passengerId: PassengerId }, (result) => {
        //console.log("result:" + result);

        if (result == "True") {
            //close modal
            $("#CopiedTripPassengersModal").modal("hide");
            $("#CopiedTripLogModal").modal("hide");

            location.reload();
        } else {
            alert("An Error occured while copying.");
        }
    });
}

function SelectAllCheckBox() {
    if ($("#select-all-checkbox").is(":checked")) {
        $(".item-checkbox").prop("checked", true);
    }
}
