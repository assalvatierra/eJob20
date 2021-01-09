
/**
 *  /CarRental/CarReservation
 *  Handles Reservation Form and Reservation Modal Form
 */ 

$(".rsv-unit-item").click(() => {
    $(this).children('input').prop("checked", true);
});

function selectUnit(e, unitId) {
    $(e).addClass("unit-selected").siblings().removeClass('unit-selected');

    $("#unit-" + unitId).prop("checked", true)
    console.log("Selected Unit: " + unitId);
    $("#rnt-carUnit").val(unitId);
    getUnitRate(unitId);

    //disable type self drive if unitId = 1,2,7
    disableSelfDrive(unitId);
}

function disableSelfDrive(unitId) {
    if (unitId == 1 || unitId == 2 || unitId == 7) {
        //disable self drive
        $("#Rental-SelfDrive").prop("disabled", true);

        //remove checked on self drive
        $("#Rental-SelfDrive").prop("checked", false);

        //check with driver
        $("#Rental-WithDriver").prop("checked", true);
        $("input[name='SelfDrive']").val(0);

        console.log("disable self drive");
    } else {
        //enable self drive
        $("#Rental-SelfDrive").prop("disabled", false);
        console.log("enable self drive");
    }
}

function getUnitRate(unitID) {
    //var unitID = $('#UnitDropdown option:selected').val();
    $.get("/CarRental/GetUnitRate", { unitId: unitID }, (result) => {
        console.log("rate: " + result);
        $("#rnt-baseRate").val(result);
    });
}

function LoadingSubmit() {
    $("#overlay").show();
}

$("#Rental-WithDriver").click(() => {
    console.log("check with driver");
    //update  with driver value
    $("input[name='SelfDrive']").val(0);

    //uncheck other option
    $("#Rental-SelfDrive").prop("checked", false);
});

$("#Rental-SelfDrive").click(() => {
    console.log("check Self Drive");
    //update selfdrive value
    $("input[name='SelfDrive']").val(1);

    //uncheck other option
    $("#Rental-WithDriver").prop("checked", false);
});


function ContinueReservation() {

    console.log("Continue Reservation");
    //show all units again
    $("#btn-rentalUnit").children("div").children("span").show();

    //submit form
    let carUnitId = $("#rsvCarUnit").val();
    let locPickup = $("#rsvLocPickup").val();
    let datePickup = $("#rsvDate-PickUp").val();
    let dateDropOff = $("#rsvDate-DropOff").val();
    console.log("CarUnit: " + carUnitId);
    console.log("datePickup: " + datePickup);

    //transfer to reservation
    $("#rnt-startDate").val(datePickup);
    $("#rnt-endDate").val(dateDropOff);

    $("#rnt-startLoc").val(locPickup);

    SelectedUnit(carUnitId);

    GetVehicleDetails(carUnitId);

    //open modal
    $("#ReservationRequestModal").modal('show');
}

function SelectedUnit(id) {
    if (id != 0) {
        selectUnit(null, id);
        $("#unit-" + id).parent().addClass("unit-selected");
        $("#unit-" + id).parent().siblings().hide();
    }
}

//get vehicle details and rate
function GetVehicleDetails(id) {
    $.get("/CarRental/GetCarDetails", { id: id }, (result) => {
        console.log(result);


        if (result != undefined) {
            $("#rsv-dtls-desc").text(result["Description"]);
            $("#rsv-dtls-class").text(result["Class"]);
            $("#rsv-dtls-fuel").text(result["Fuel"]);
            $("#rsv-dtls-trans").text(result["Transmission"]);
            $("#rsv-dtls-pass").text(result["Passengers"]);
            $("#rsv-dtls-rate").text(" Php " + result["Rate"]);
            $("#rsv-dtls-imgUrl").attr("src", "/Images/CarRental/" + result["ImgUrl"]);

        } else {
            alert("Unable to retrieve Car Details");
        }

        if (result == null) {
            alert("Unable to retrieve Car Details");
        }
    });
}
