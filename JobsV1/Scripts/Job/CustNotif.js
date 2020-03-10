
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
    //Date 1
    var ddd1 = $('input[name="DtScheduled"]').val();

    $('input[name="DtScheduled"]').daterangepicker(
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
       // alert(start.format('YYYY-MM-DD h:mm A'));
    }
    );
    $('input[name="DtScheduled"]').val(ddd1);

    $('input[name="DtScheduled"]').val(moment().add(1, 'days').format("MM/DD/YYYY h:mm A"));
}


