

$(document).ready(function () {
    InitDatePicker();
})

function InitDatePicker()
{
    var ddd1 = $('#findLogDate').val();

    $('#findLogDate').daterangepicker(
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
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
         
        }
    );

    $('#findLogDate').val(ddd1);

    //Date 2
    var ddd2 = $('#copiedLogDate').val();
    $('#copiedLogDate').daterangepicker(
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
            var today = moment().format('YYYY-MM-DD');
            var datepicker = start.format('YYYY-MM-DD');

        }
    );

    $('#copiedLogDate').val(ddd2);
}
