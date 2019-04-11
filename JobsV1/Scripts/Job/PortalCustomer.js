/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */


$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker()
{
    var ddd1 = $('input[name="ExpiryDt"]').val();

    $('input[name="ExpiryDt"]').daterangepicker(
    {
        timePicker: false,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY  hh:mm A'
        }
    },
    function (start, end, label) {
        //check if date is greater than or equal to today
        
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
        //alert(today > datepicker);

            if (today > datepicker) {
                alert("Job date is past the date today. Do you want to continue?");

            }

        //alert(start.format('YYYY-MM-DD'));
        
        }
    );

    $('input[name="ExpiryDt"]').val(ddd1);
    //$('input[name="JobDate"]').val(ddd1.substr(0, ddd1.indexOf(" ") ));

}




//get a generated password
//using the customer ID
//and fill the password text area
function generatePassword() {
    var customerId = $('#customerId').val();

    console.log("customer ID: " + customerId);

    //build json object
    var data = {
        custId: customerId
    };

    //request data from server using ajax call
    $.ajax({
        url: '/PortalCustomers/getPassword?custId=' + customerId,
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log("SUCCESS");
            $('input[name="Password"]').val(data["responseText"]);

        },
        error: function (data) {
            console.log("ERROR");
            console.log(data);
            $('input[name="Password"]').val(data["responseText"]);
        }
    });
}

//Get customer number, then display
//the number to the Number Text area
function getCustomerNum() {
    var customerId = $('#customerId').val();

    //build json object
    var data = {
        custId: customerId
    };

    //request data from server using ajax call
    $.ajax({
        url: '/PortalCustomers/getCustomerNum?custId=' + customerId,
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
            $('input[name="ContactNum"]').val(data["responseText"]);

        },
        error: function (data) {
            //console.log("ERROR");
            //console.log(data);
            $('input[name="ContactNum"]').val(data["responseText"]);
        }
    });
}


//Get customer number, then display
//the number to the Number Text area
function validateCustomerSession() {

    //request data from server using ajax call
    $.ajax({
        url: '/PortalCustomers/validateCustomer',
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
            $('input[name="ContactNum"]').val(data["responseText"]);

        },
        error: function (data) {
            //console.log("ERROR");
            //console.log(data);
            $('input[name="ContactNum"]').val(data["responseText"]);
        }
    });
}

$('#customerId').change(function () {
    //generate generic password
    generatePassword();

    //get customer Number
    getCustomerNum();
});

