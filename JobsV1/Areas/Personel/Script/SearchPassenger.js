/**
 *  Job Create / Edit
 *  Job Search Company
 */ 

//Change job desription bsed on the cutomer name

function SearchPassenger() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("SearchBarPassenger");
    filter = input.value.toUpperCase();
    ul = document.getElementById("SearchListPassenger");
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

function setPassengerValue(value) {
    console.log(value);
    $('#PassengerId').val(value);
    $('#Passenger-textfield').val($("#PassengerId option:selected").text());
    $('#PassengerSearchModal').modal('hide');

    GetPassengerMasterData(value);
}

function GetPassengerMasterData(passId) {

   var res =  $.get('/Personel/crLogPassengers/GetPassengerMasterData', { PassId: passId }, (response) => {
       var passData = JSON.parse(response);

       if (passData != undefined) {
           console.log(passData);
           $('#Passenger-textfield').val(passData['Name']);
           $('#Passenger-contact').val(passData['Contact']);
           $('#Passenger-address').val(passData['PassAddress']);
           $('#Passenger-dropPoint').val(passData['DropPoint']);
           $('#Passenger-dropTime').val(passData['DropTime']);
           $('#Passenger-pickupPoint').val(passData['PickupPoint']);
           $('#Passenger-pickupTime').val(passData['PickupTime']);
           $('#Passenger-area').val(passData['Area']);

        } else {
            console.log('Error getting data');
        }

    });
    console.log(res);
}
