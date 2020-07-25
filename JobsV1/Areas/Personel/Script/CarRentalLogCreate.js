﻿

$(document).ready(function () {
    InitDatePicker();
    $('input[name="DtTrip"]').val(moment().format('MM/DD/YYYY hh:mm A'))
})

function InitDatePicker()
{
    var ddd1 = $('input[name="DtTrip"]').val();

    $('input[name="DtTrip"]').daterangepicker(
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
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
         
        }
    );

}