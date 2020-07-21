

$(document).ready(function () {
    InitDatePicker();
    
})

function InitDatePicker()
{
    var ddd1 = $('input[name="dtReading"]').val();

    $('input[name="dtReading"]').daterangepicker(
    {
        timePicker: false,
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

    if (ddd1 == "") {
        $('input[name="dtReading"]').val(moment().format("MM/DD/YYYY hh:mm A"));
    }

}
