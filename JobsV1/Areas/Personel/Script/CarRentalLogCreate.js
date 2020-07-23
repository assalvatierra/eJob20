

$(document).ready(function () {
    InitDatePicker();
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
            format: 'MM/DD/YYYY h:m A'
        }
    },
    function (start, end, label) {
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
         
        }
    );

}
