﻿/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */

$(document).ready(function () {
    InitDatePicker();
   // InitEditChange();
})


function InitDatePicker() {
    var ddd1 = $('input[name="DtTrx"]').val();
    $('input[name="DtTrx"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY h:mm A'
        }
    },
    function (start, end, label) {
        //alert(start.format('YYYY-MM-DD h:mm A'));

    }
    );

    $('input[name="DtTrx"]').val(ddd1);

    //Start Date
    var ddd2 = $('input[name="DtStart"]').val();
    $('input[name="DtStart"]').daterangepicker(
    {
        timePicker: false,
        timePickerIncrement: 30,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY'
        }
    },
    function (start, end, label) {
        //alert(start.format('YYYY-MM-DD h:mm A'));

    }
    );

    $('input[name="DtStart"]').val(moment(ddd2).format("MM/DD/YYYY"));

    //Date End

    var ddd3 = $('input[name="DtEnd"]').val();
    $('input[name="DtEnd"]').daterangepicker(
    {
        timePicker: false,
        timePickerIncrement: 30,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY'
        }
    },
    function (start, end, label) {
        //alert(start.format('YYYY-MM-DD h:mm A'));

        }
    );

    $('input[name="DtEnd"]').val(moment(ddd3).format("MM/DD/YYYY"));
}

