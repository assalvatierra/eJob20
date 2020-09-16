/* ********************************************************
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
        timePickerIncrement: 30,
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

    var ddd1 = $('input[name="DtStart"]').val();
    $('input[name="DtStart"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 30,
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

    $('input[name="DtStart"]').val(ddd1);

    var ddd1 = $('input[name="DtEnd"]').val();
    $('input[name="DtEnd"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 30,
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

    $('input[name="DtEnd"]').val(ddd1);
}

