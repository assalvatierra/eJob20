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
    //------------- Add Start Date ------------------//
    var ddd1 = $('#CreateSupItemRate-ValidFrom').val();

    $('#CreateSupItemRate-ValidFrom').daterangepicker(
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
        //check if date is greater than or equal to today
        
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
        //alert(today > datepicker);

            if (today > datepicker) {
                alert("Job date is past the date today. Do you want to continue?");
            }
        }
    );

    $('#CreateSupItemRate-ValidFrom').val(ddd1);

    if (!$('#CreateSupItemRate-ValidFrom').val()) {
        $('#CreateSupItemRate-ValidFrom').val(moment().format('MM/DD/YYYY'))
    }

    //------------- Add End Date ------------------//

    var ddd2 = $('#CreateSupItemRate-ValidTo').val();

    $('#CreateSupItemRate-ValidTo').daterangepicker(
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
        //check if date is greater than or equal to today

        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');

        if (today > datepicker) {
            alert("Job date is past the date today. Do you want to continue?");
        }
    });

    $('#CreateSupItemRate-ValidTo').val(ddd2);

    if (!$('#CreateSupItemRate-ValidTo').val()) {
        $('#CreateSupItemRate-ValidTo').val(moment().add(6,'M').format('MM/DD/YYYY'))
    }
    //------------- Edit Start Date ------------------//

    //Start Date
    var ddd3 = $('#EditSupItemRate-ValidFrom').val();

    $('#EditSupItemRate-ValidFrom').daterangepicker(
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
        //check if date is greater than or equal to today

        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');

        if (today > datepicker) {
            //alert("Job date is past the date today. Do you want to continue?");
        }
    });

    $('#EditSupItemRate-ValidFrom').val(ddd3);

    //------------- Edit End Date ------------------//

    //End Date
    var ddd4 = $('#EditSupItemRate-ValidTo').val();

    $('#EditSupItemRate-ValidTo').daterangepicker(
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
        //check if date is greater than or equal to today

        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');

        if (today > datepicker) {
            //alert("Job date is past the date today. Do you want to continue?");
        }
    });

    $('#EditSupItemRate-ValidTo').val(ddd4);

}
