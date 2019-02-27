/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */

$(document).ready(function () {
    InitDatePicker();
    initFieldEvents();
})


function InitDatePicker()
{
    var ddd1 = $('input[name="DtStart"]').val();
    var ddd2 = $('input[name="DtEnd"]').val();

    $('input[name="DtStart"]').daterangepicker(
    {
        timePicker: false,
        timePickerIncrement: 30,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY '
        }
    },
    function (start, end, label) {
        //alert(start.format('YYYY-MM-DD h:mm A'));
        
    }
    );


    $('input[name="DtEnd"]').daterangepicker(
    {
        timePicker: false,
        timePickerIncrement: 30,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY '
        }
    },
    function (start, end, label) {
        //alert(start.format('YYYY-MM-DD h:mm A'));
        
    }
    );

    $('input[name="DtStart"]').val(ddd1.substr( 0, ddd1.indexOf(" ") ));
    $('input[name="DtEnd"]').val(ddd2.substr(0, ddd1.indexOf(" ") ));




}

function initFieldEvents()
{
    $('#QuotedAmt').on('change', function () {
        var sTmp = $('#QuotedAmt').val();
        $('#SupplierAmt').val(sTmp);
        $('#ActualAmt').val(sTmp);
    });

    $('#SupplierAmt').on('change', function () {
        var sTmp = $('#SupplierAmt').val();
        $('#ActualAmt').val(sTmp);
    });


}
