/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */


$(document).ready(function () {
    InitDatePicker();

    var today = moment().format('MM/DD/YYYY');

    $("#Exp-DtDriver").val(today);

    $("#Exp-DtOperator").val(today);
})


function InitDatePicker() {
    var ddd1 = $('input[id="Exp-DtDriver"]').val();

    $('input[id="Exp-DtDriver"]').daterangepicker(
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
    }
    );

    $('input[id="Exp-DtDriver"]').val(ddd1);

    // Operator Date
    var ddd2 = $('input[id="Exp-DtOperator"]').val();

    $('input[id="Exp-DtOperator"]').daterangepicker(
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
        
    }
    );

    $('input[id="Exp-DtOperator"]').val(ddd2);

}



//Add Item Expense
function ajax_AddExpenses() {

    //console.log("Add Expenses");

    //build json object
    var data = {
        id:         $("#Exp-JobServiceId").val(),
        payment:    $("#Exp-Payment").val(),
        fuel:       $("#Exp-Fuel").val(),
        driver:     $("#Exp-Driver").val(),
        Operator:   $("#Exp-Operator").val(),
        others:     $("#Exp-Others").val(),
        remarks:    $("#Exp-Remarks").val(),
        dtDriver:   $("#Exp-DtDriver").val(),
        dtOperator: $("#Exp-DtOperator").val(),
    };

    //console.log(data);

    var url = '/JobMains/AddExpenses';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            // console.log("SUCCESS");
        },
        error: function (data) {
            // console.log("ERROR");
            //console.log(data);
              location.reload();
        }
    });
}

function addExpenses(jsId) {
    $("#Exp-JobServiceId").val(jsId);
    //console.log(jsId);

    //load fields with expenses record
    ajax_getExpenses();
}



//load table content on search btn click
function ajax_getExpenses() {
    var id = $("#Exp-JobServiceId").val();
    //build json object
    var data = {
        jsId: id
    };

    //console.log(query);
    //request data from server using ajax call
    $.ajax({
        url: '/JobMains/getExpenseRecord?jsId=' + id,
        type: "POST",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            // console.log("ERROR");
            console.log(data);
            ExpenseRecord(data);
        }
    });
}


//display products
function ExpenseRecord(data) {
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    console.log("products view");

    var Payment      = temp["Payment"]       != null ? temp["Payment"]       : "0";
    var ActualAmt    = temp["ActualAmt"]     != null ? temp["ActualAmt"]     : "0";
    var DriverComi   = temp["DriverComi"]    != null ? temp["DriverComi"]    : "0";
    var Fuel         = temp["Fuel"]          != null ? temp["Fuel"]          : "0";
    var OperatorComi = temp["OperatorComi"]  != null ? temp["OperatorComi"]  : "0";
    var Others       = temp["Others"]        != null ? temp["Others"]        : "0";
    var Remarks      = temp["Remarks"]       != null ? temp["Remarks"]       : "";
    var DtDriver     = temp["DtDriver"]      != null ? temp["DtDriver"]      : null;
    var DtOperator   = temp["DtOperator"]    != null ? temp["DtOperator"]    : null;
    var Total        = temp["Total"]         != null ? temp["Total"]         : null;
    var Net          = temp["Net"]           != null ? temp["Net"]           : null;

    var test = 2500;
    //console.log("Payment: " + Payment);
    //transfer to fields
    $("#Exp-Payment").val(Payment);
    $("#Exp-Fuel").val(Fuel);
    $("#Exp-Driver").val(DriverComi);
    $("#Exp-Operator").val(OperatorComi);
    $("#Exp-Others").val(Others);
    $("#Exp-Remarks").val(Remarks);
    $("#Exp-DtDriver").val(moment(DtDriver).format('MM/DD/YYYY'));
    $("#Exp-DtOperator").val(moment(DtOperator).format('MM/DD/YYYY'));
    moment().format('YYYY-MM-DD')


    //calculate 
    calculateTotal();
}

function calculateTotal() {

    var payment  = $("#Exp-Payment").val();
    var fuel     = $("#Exp-Fuel").val();
    var driver   = $("#Exp-Driver").val();
    var operator = $("#Exp-Operator").val();
    var others   = $("#Exp-Others").val();

    var total = Number(fuel) + Number(driver) + Number(operator) + Number(others);

    $("#Exp-Total").text(total);

    var net = payment - total;

    $("#Exp-Net").text(net);

}
