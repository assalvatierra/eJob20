
/**
 *  Add Vehicle to the Job
 */
function AddJobVehicle(jobMainId) {
    var data = {
        jobMainId: jobMainId,
        vehicleId: parseInt($('#addVehicle-Options option:selected').val()),
        mileage: parseInt($("#addVehicle-Mileage").val())
    };

    console.log( data);

    var response = $.post("/JobOrder/AddJobVehicle", data, (result) => {
        if (result == 'True') {
            //close modal
            $("#AddVehicleModal").modal('hide');

            //add vehicle to view
            let selectedVehicle = "Vehicle: " + $('#addVehicle-Options option:selected').text();
            $("#jobVehicleText").text(selectedVehicle);
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