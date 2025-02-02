﻿/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */


$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker()
{
    var ddd1 = $('input[name="DtPayment"]').val();

    $('input[name="DtPayment"]').daterangepicker(
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

    $('input[name="DtPayment"]').val(moment().format("MM/DD/YYYY hh:mm A"));
    //$('input[name="JobDate"]').val(ddd1.substr(0, ddd1.indexOf(" ") ));

}
