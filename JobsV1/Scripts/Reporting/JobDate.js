/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */

$(document).ready(function () {
    InitDatePicker();
})

function InitDatePicker()
{
    //Start Date
    var ddd1 = $('input[name="startDate"]').val();

    $('input[name="startDate"]').daterangepicker(
    {
        timePicker: false,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM / DD / YYYY'
        }

    },
    function (start, end, label) {
        //check if date is greater than or equal to today
        
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
        //alert(today > datepicker);

            if (today > datepicker) {
                //alert("Job date is past the date today. Do you want to continue?");
            }

        //alert(start.format('YYYY-MM-DD'));
        
        }
    );

    $('input[name="startDate"]').val(ddd1);


    //End Date
    var ddd2 = $('input[name="endDate"]').val();

    $('input[name="endDate"]').daterangepicker(
    {
        timePicker: false,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM / DD / YYYY'
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

    $('input[name="endDate"]').val(ddd1);
    //$('input[name="JobDate"]').val(ddd1.substr(0, ddd1.indexOf(" ") ));

}
