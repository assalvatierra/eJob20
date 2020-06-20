
$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker()
{
    var ddd1 = $('input[name="DtEntered"]').val();

    $('input[name="DtEntered"]').daterangepicker(
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
        //alert(today > datepicker);

            if (today > datepicker) {
                alert("Job date is past the date today. Do you want to continue?");
            }

        //alert(start.format('YYYY-MM-DD'));
        
        }
    );

    $('input[name="DtEntered"]').val(moment().format("MM/DD/YYYY hh:mm A"));


    var ddd2 = $('input[name="AppointmentDate"]').val();

    $('input[name="AppointmentDate"]').daterangepicker(
        {
            timePicker: false,
            timePickerIncrement: 1,
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: 'MMM DD YYYY'
            }
        },
        function (start, end, label) {
            //check if date is greater than or equal to today

            var today = moment().format('YYYY-MM-DD');
            var datepicker = start.format('YYYY-MM-DD');
            //alert(today > datepicker);

            if (today >= datepicker) {
                alert("Job date must be past the date today. Do you want to continue?");
            }

        }
    );

    $('input[name="AppointmentDate"]').val(moment().add(1, 'DAYS').format("MMM DD YYYY"));

}
