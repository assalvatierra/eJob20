/* ********************************************************
* By Abel S. Salvatierra
* @2016 
* 
*********************************************************** */

$(document).ready(function () {
    InitDatePicker();
})

function InitDatePicker() {
    $('input[name="TrxDate"]').daterangepicker(
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
//        alert(start.format('YYYY-MM-DD h:mm A'));
    }
    );
}