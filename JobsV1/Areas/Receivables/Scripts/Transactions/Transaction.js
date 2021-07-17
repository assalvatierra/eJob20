

$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('input[name="DtInvoice"]').val();

    $('input[name="DtInvoice"]').daterangepicker(
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

    console.log(ddd1.trim())
    if (ddd1.trim() == "" || ddd1.trim() == '01/01/0001 12:00 AM' || ddd1.trim() == '01/01/2001') {
        //$('input[name="DtInvoice"]').val(moment().format('MM/DD/YYYY'));
    } else {
        //$('input[name="DtInvoice"]').val(moment(ddd1).format('MM/DD/YYYY'));
    }

    //Date 2
    var ddd2 = $('input[name="DtEncoded"]').val();

    $('input[name="DtEncoded"]').daterangepicker(
        {
            timePicker: true,
            timePickerIncrement: 1,
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

    if (ddd2.trim() == "" || ddd2.trim() == '01/01/0001 12:00 AM' || ddd2.trim() == '01/01/2001') {
        //$('input[name="DtEncoded"]').val(moment().format('MM/DD/YYYY'));
    } else {
        //$('input[name="DtEncoded"]').val(moment(ddd2).format('MM/DD/YYYY'));
    }

    //Date 2
    var ddd3 = $('input[name="DtDue"]').val();

    $('input[name="DtDue"]').daterangepicker(
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

    if (ddd3.trim() == "" || ddd3.trim() == '01/01/0001 12:00 AM' || ddd3.trim() == '01/01/2001') {
        //$('input[name="DtDue"]').val(moment().format('MM/DD/YYYY'));
    } else {
        //$('input[name="DtDue"]').val(moment(ddd3).format('MM/DD/YYYY'));
    }

    //Date 4
    var ddd4 = $('input[name="DtService"]').val();

    $('input[name="DtService"]').daterangepicker(
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

    if (ddd4.trim() == "" || ddd4.trim() == '01/01/0001 12:00 AM' || ddd4.trim() == '01/01/2001') {
        //$('input[name="DtService"]').val(moment().format('MM/DD/YYYY'));
    } else {
        //$('input[name="DtService"]').val(moment(ddd4).format('MM/DD/YYYY'));
    }


    //Date 5
    var ddd5 = $('input[name="DtServiceTo"]').val();

    $('input[name="DtServiceTo"]').daterangepicker(
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

    if (ddd5.trim() == "" || ddd5.trim() == '01/01/0001 12:00 AM' || ddd5.trim() == '01/01/2001') {
        //$('input[name="DtServiceTo"]').val(moment().format('MM/DD/YYYY'));
    } else {
        //$('input[name="DtServiceTo"]').val(moment(ddd5).format('MM/DD/YYYY'));
    }

}




function UpdateStatusAndDeposit(id) {
    var result = $.post("/Receivables/ArMgt/UpdatePaymentAsDeposited",
        {
            transId: id
        },
        (response) => {
            console.log("Update Status : " + response);
            if (response == "True") {
                window.location.reload(false);
            } else {
                alert("Unable to Update Deposit.");
            }
        }
    );
}
