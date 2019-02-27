
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
    var ddd1 = $('input[name="dtControl"]').val();

    $('input[name="dtControl"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY h:mm:ss A '
        }
    },
    function (start, end, label) {
       // alert(start.format('YYYY-MM-DD h:mm A'));
        
    }
    );br


    $('input[name="dtControl"]').val(ddd1.substr( 0, ddd1.indexOf(" ") ));


}
