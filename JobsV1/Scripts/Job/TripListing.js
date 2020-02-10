/****************************/
/***** Supplier Listing *****/
/****************************/


//Add Item Expense
function ajax_AddExpenses() {


    //build json object
    var data = {
        id:         $("#Exp-JobServiceId").val(),
        payment:    $("#Exp-Payment").val(),
        fuel:       $("#Exp-Fuel").val(),
        driver:     $("#Exp-Driver").val(),
        Operator:   $("#Exp-Operator").val(),
        others:     $("#Exp-Others").val(),
        remarks:    $("#Exp-Remarks").val(),
        driverForRelease: $("#driver-readyReleased").is(":disabled"),
        operatorForRelease: $("#operator-readyReleased").is(":disabled"),

    };

    console.log(data);

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
             console.log(data);
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

    var PaymentCash  = temp["PaymentCash"]   != null ? temp["PaymentCash"]   : "0";
    var PaymentBank  = temp["PaymentBank"]   != null ? temp["PaymentBank"]   : "0";
    var ActualAmt    = temp["ActualAmt"]     != null ? temp["ActualAmt"]     : "0";
    var DriverComi   = temp["DriverComi"]    != null ? temp["DriverComi"]    : "0";
    var Fuel         = temp["Fuel"]          != null ? temp["Fuel"]          : "0";
    var OperatorComi = temp["OperatorComi"]  != null ? temp["OperatorComi"]  : "0";
    var Others       = temp["Others"]        != null ? temp["Others"]        : "0";
    var Remarks      = temp["Remarks"]       != null ? temp["Remarks"]       : "";
    var Total        = temp["Total"]         != null ? temp["Total"]         : null;
    var Net          = temp["Net"]           != null ? temp["Net"]           : null;
    var DvrForRelease= temp["DriverForRelease"] != null ? temp["DriverForRelease"] : false;
    var OptForRelease= temp["OperatorForRelease"] != null ? temp["OperatorForRelease"] : false;

    //transfer to fields
    $("#Exp-Payment").val(PaymentCash);
    $("#Exp-PaymentBank").val(PaymentBank);
    $("#Exp-Fuel").val(Fuel);
    $("#Exp-Driver").val(DriverComi);
    $("#Exp-Operator").val(OperatorComi);
    $("#Exp-Others").val(Others);
    $("#Exp-Remarks").val(Remarks);

    //disable fields
    $("#Exp-Driver").attr("disabled", DvrForRelease);
    $("#driver-readyReleased").attr("disabled", DvrForRelease);

    $("#Exp-Operator").attr("disabled", OptForRelease);
    $("#operator-readyReleased").attr("disabled", OptForRelease);

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

