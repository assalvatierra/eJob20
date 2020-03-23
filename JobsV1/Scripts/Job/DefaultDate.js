
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
    var ddd1 = $('input[name="DtStart"]').val();

    $('input[name="DtStart"]').daterangepicker(
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
       // alert(start.format('YYYY-MM-DD h:mm A'));
        
    }
    );

    //$('input[name="DtStart"]').val(ddd1.substr(0, ddd1.indexOf(" ")));

    //Date 2
    var ddd2 = $('input[name="DtEnd"]').val();

    $('input[name="DtEnd"]').daterangepicker(
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
        // alert(start.format('YYYY-MM-DD h:mm A'));

    }
    ); 


   // $('input[name="DtEnd"]').val(ddd2.substr(0, ddd2.indexOf(" ")));


}


