
/* Cashier Payments
 * 
 */

$(() => {
    $("#AddPayment-Amount").focus();
});

function AddPaymentSubmit(id) {
    var bank = $("#AddPayment-Bank").val();
    var type = $("#AddPayment-Type").val();
    var remarks = $("#AddPayment-Remarks").val();
    var amount = $("#AddPayment-Amount").val();

    var data = {
        id: id,
        bankId: bank,
        typeId: type,
        remarks: remarks,
        amount: amount
    }

    $.post("/Cashier/Ajax_AddPayment", data, (result) => {
        console.log(result);
        if (result == 'True') {
            window.location.reload();
        } else {
            alert("Unable to add Payment.");
        }
    });

}

//Accept only positive decimal for amount
$(function () {
    $("input#AddPayment-Amount").bind("change keyup input", function () {
        var position = this.selectionStart - 1;
        //remove all but number and .
        var fixed = this.value.replace(/[^0-9\.]/g, '');
        if (fixed.charAt(0) === '.')                  //can't start with .
            fixed = fixed.slice(1);

        var pos = fixed.indexOf(".") + 1;
        if (pos >= 0)               //avoid more than one .
            fixed = fixed.substr(0, pos) + fixed.slice(pos).replace('.', '');

        if (this.value !== fixed) {
            this.value = fixed;
            this.selectionStart = position;
            this.selectionEnd = position;
        }
    });

    $("#AddPayment-Amount").bind("focus blur", function () {
        $("#AddPayment-Amount").select();
    });
});



//On Enter submit
$('#AddPayment-Amount').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        $('#AddPayment-Submit').click();
        return false;
    }
});   


//Set Payment Id for edit
function SetEditPermissionId(id) {
    $("#Edit-Pass-Id").val(id);
    $("#Edit-Pass-Input").val("");
    $("#Edit-Pass-warning").hide();
    $("#Edit-Pass-Input").focus();
}

//Check admin pass to edit
function CheckEditPermission() {
    //reset
    $("#Edit-Pass-warning").hide();

    //check permission
    $.get("/Cashier/CheckAdminPermission", { pass: $("#Edit-Pass-Input").val() }, (result) => {
        console.log(result);
        if (result == 'True') {

            var paymentId = $("#Edit-Pass-Id").val();
            //redirect to Edit
            window.location.href = "/Cashier/EditPayment/" + paymentId;
        } else {
            $("#Edit-Pass-warning").show();
        }

    })
}

//Set Payment Id for delete
function SetDeletePermissionId(id) {
    $("#Delete-Pass-Id").val(id);
    $("#Delete-Pass-Input").val("");
    $("#Delete-Pass-warning").hide();
    $("#Delete-Pass-Input").focus();
}

//Check admin pass to delete
function CheckDeletePermission() {
    //reset
    $("#Delete-Pass-warning").hide();

    //check permission
    $.get("/Cashier/CheckAdminPermission", { pass: $("#Delete-Pass-Input").val() }, (result) => {
        console.log(result);
        if (result == 'True') {

            var paymentId = $("#Delete-Pass-Id").val();
            //redirect to Edit
            window.location.href = "/Cashier/Delete/" + paymentId;
        } else {
            $("#Delete-Pass-warning").show();
        }

    })
}

function ResetWarning() {
    $("#Edit-Pass-warning").hide();
    $("#Delete-Pass-warning").hide();

}

//On Enter submit
$('#Edit-Pass-Input').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        $('#Edit-Proceed').click();
        return false;
    }
});



//On Enter submit
$('#Delete-Pass-Input').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        $('#Delete-Proceed').click();
        return false;
    }
});   


