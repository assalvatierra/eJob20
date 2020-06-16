
/**
 *  Add Vehicle to the Job
 */
function AddJobVehicle(jobMainId) {
    var data = {
        jobMainId: jobMainId,
        vehicleId: parseInt($('#addVehicle-Options option:selected').val()),
        mileage: parseInt($("#addVehicle-Mileage").val())
    };

    console.log(data);

    var response = $.post("/JobOrder/AddJobVehicle", data, (result) => {
        if (result == 'True') {
            //close modal
            $("#AddVehicleModal").modal('hide');

            //add vehicle to view
            let selectedVehicle = "<br /> Description: " + $('#addVehicle-Options option:selected').text() + " Mileage: " + parseInt($("#addVehicle-Mileage").val());
            $("#jobVehicleText").text("");
            $("#jobVehicleText").append(selectedVehicle);
        } else {
            alert('Error! Unable to add vehicle.');
        }
    });

    console.log(response);
}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function searchVehicle() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("SearchBarCustomer");
    filter = input.value.toUpperCase();
    ul = document.getElementById("SearchListCustomer");
    li = ul.getElementsByTagName("li");

    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("a")[0];
        if (a.innerHTML.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}

function SetVehicle(value) {
    $('#addVehicle-Options').val(value);
    $('#SearchVehicleModal').modal('hide');
   
}
