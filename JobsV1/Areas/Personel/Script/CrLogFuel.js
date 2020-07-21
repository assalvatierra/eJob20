

$(document).ready(function () {
    InitDatePicker();
    
})

function InitDatePicker()
{
    var ddd1 = $('input[name="dtRequest"]').val();

    $('input[name="dtRequest"]').daterangepicker(
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
        $('input[name="dtRequest"]').val(moment().format("MM/DD/YYYY hh:mm A"));
    }

    //Date 2
    var ddd2 = $('input[name="dtFillup"]').val();

    $('input[name="dtFillup"]').daterangepicker(
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

    if (ddd2 == "") {
        $('input[name="dtFillup"]').val(moment().format("MM/DD/YYYY hh:mm A"));
    }


}
