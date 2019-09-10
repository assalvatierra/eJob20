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
    var ddd1 = $('#DtStart').val();

    $('#DtStart').daterangepicker(
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
        //check if date is greater than or equal to today
        
        var today = moment().add(2, 'days').format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
        //alert(today > datepicker);

        //if (today > datepicker) {
        //    //Start Date is valid
        //    //var validDate = moment().add(2, 'days').format('MM/DD/YYYY');
        //    console.log("Date OK");
        //    $("#date-warning").hide();
        //} else if (moment().format('YYYY-MM-DD') >= datepicker && today <= datepicker) {
        //    //Start Date is in 48 hours
        //    console.log("Start Date must be at least 2 days in advance");
        //    $("#date-warning").text("Date must be at least 2 days before the start date for Online Payment and Reservation.");
        //    $("#date-warning").show();
        //} else {
        //    //Date is not valid
        //    console.log("Start Date must be at least 2 days in advance");
        //    $("#date-warning").text("Date must be at least 2 days before the start date for Online Payment and Reservation.");
        //    $("#date-warning").show();
        //}
          
    });
    $('#DtStart').val(ddd1);
    //$('input[name="JobDate"]').val(ddd1.substr(0, ddd1.indexOf(" ") ));



}



    //handle numbers only
    $('#Number').on('input', function () {

        //backspace
        if (isNaN($(this).val())) {
            var txt = $(this);
            txt.val(txt.val().slice(0, -1));
        } else {
            //handle negative number
            if ($(this).val() < 0) {
                $(this).val(0);
            }
        }
        //handle negative number
        if ($(this).val() < 0) {
            $(this).val(0);
        }

    });

    //prevent invalid inputs 
    function validate(evt) {
        var theEvent = evt || window.event;
        console.log("validate");

        // Handle paste
        if (theEvent.type === 'paste') {
            key = event.clipboardData.getData('text/plain');
        } else {
            // Handle key press
            var key = theEvent.keyCode || theEvent.which;
            key = String.fromCharCode(key);
        }
        var regex = /[0-9]|\./;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }

function validateEmail(email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    if (!emailReg.test(email)) {
        return false;
    } else {
        return true;
    }
}
