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
    var ddd1 = $('input[name="DtEncoded"]').val();

    $('input[name="DtEncoded"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY hh:mm A'
        }
    },
    function (start, end, label) {
        //check if date is greater than or equal to today
        
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');

            if (today > datepicker) {
                alert("Job date is past the date today. Do you want to continue?");

            }
        }
    );

    $('input[name="DtEncoded"]').val(moment().format("MM/DD/YYYY hh:mm A"));

    //Booking Date
    var ddd2 = $('input[name="DtBooking"]').val();

    $('input[name="DtBooking"]').daterangepicker(
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

            var validDate = moment().add(2, 'days').format('YYYY-MM-DD');
            var datepicker = start.format('YYYY-MM-DD');
            //alert(today > datepicker);

            if (validDate > datepicker) {
                alert("Your Booking Date is not valid. Please select again.");
                $('input[name="DtBooking"]').val(moment().add(2, 'days').format('MM/DD/YYYY'));
            }
            //alert(start.format('YYYY-MM-DD'));
        }
    );

    $('input[name="DtBooking"]').val(moment().add(2, 'days').format("MM/DD/YYYY"));
}
