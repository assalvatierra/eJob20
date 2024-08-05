
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

/**
 *  Show Vehicle Modal
 */

function OpenVehicleListModal() {
    $("#ShowVehicleListModal").modal("show");
}

function SelectVehicle(id) {
    $("#rsvCarUnit").val(id).change();
    $("#ShowVehicleListModal").modal("hide");

}



/** 
 *  Vehicle Slide Shows Images
 * **/
function ViewVehicleImages() {
    $("#vehicleImagesModal").modal("show");
}

$('#rsvCarUnit').on('change', function () {
    //Get updated images
    console.log("Get Car Unit");
    //GetCarImages();

});

function openModal() {
    document.getElementById("LightboxModal").style.display = "block";

}

function closeModal() {
    document.getElementById("LightboxModal").style.display = "none";
}


function GetCarImages() {
    let carUnitId = $("#rsvCarUnit").val();
    //console.log(carUnitId);

    $.get("/CarRental/GetUnitImages", { unitId: carUnitId }, (response) => {
        //console.log(response);

        for (var i = 0; i < response.length; i++) {
            //console.log(response[i]);

            let carImgUrl = "/Images/CarRental/" + response[i];
            let htmlId = i + 1;
            $("#lightbox-img-" + htmlId).attr("src", carImgUrl);
            $("#lightbox-item-" + htmlId).attr("src", carImgUrl);
        }
    });
}

var slideIndex = 1;
showSlides(slideIndex);

function plusSlides(n) {
    showSlides(slideIndex += n);
}

function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("lightbox-item");
    var captionText = document.getElementById("caption");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
    captionText.innerHTML = dots[slideIndex - 1].alt;
}
