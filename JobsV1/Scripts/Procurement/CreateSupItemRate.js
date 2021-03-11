
/**
 *  Create Supplier Item Rate
 */


function Submit_CreateSupItemRate() {
    if (Validate_CreateSupItemRate()) {

        console.log("Validate True");
        Ajax_CreateSupplierItem();
    }
}

function Ajax_CreateSupplierItem() {
    var newSupplierItem = {
        salesLeadItemId: $("#CreateSupItemRate-ItemLeadId").val(),
        supplierId: $("#CreateSupItemRate-Supplier").val(),
        itemId:     $("#CreateSupItemRate-Item").val(),
        particulars:$("#CreateSupItemRate-Particulars").val(),
        materials:  $("#CreateSupItemRate-Materials").val(),
        rate:       $("#CreateSupItemRate-Rate").val(),
        unitTypeId: $("#CreateSupItemRate-UnitType").val(),
        tradeTerm:  $("#CreateSupItemRate-TradeTerm").val(),
        tolerance:  $("#CreateSupItemRate-Tolerance").val(),
        remarks:    $("#CreateSupItemRate-Remarks").val(),
        validFrom:  $("#CreateSupItemRate-ValidFrom").val(),
        validTo:    $("#CreateSupItemRate-ValidTo").val(),
        offeredBy:  $("#CreateSupItemRate-OfferedBy").val(),
        procuredBy: $("#CreateSupItemRate-ProcuredBy").val(),
    }

    console.log(newSupplierItem);

    $.post("/Procurement/CreateSupplierItem", newSupplierItem, (res) => {
        console.log(res);
    }).done((result) => {
        if (result) {
            $("#CreateSupItemRate").modal("hide");
            window.location.reload(false);
        }
        console.log(result);
    }).fail((err) => {
        console.log(err);
    });
}

//Create Supplier Item Validation
function Validate_CreateSupItemRate() {
    var validationMsg = "";
    var isValid = true;

    $("#CreateSupItemRate-Particulars-errorMsg").hide();
    $("#CreateSupItemRate-Particulars").parent().parent().removeClass("has-error");
    $("#CreateSupItemRate-Rate").parent().parent().removeClass("has-error");
    $("#CreateSupItemRate-ValidFrom").parent().parent().removeClass("has-error");
    $("#CreateSupItemRate-ValidTo").parent().parent().removeClass("has-error");

    var salesLead_Id = $("#CreateSupItemRate-ItemLeadId").val();
    if (salesLead_Id == null || salesLead_Id == '' ) {
        //mark input sa red
        $("#CreateSupItemRate-ItemLeadId").parent().parent().addClass("has-error");
        var isValid = false;
    }

    var particulars = $("#CreateSupItemRate-Particulars").val();
    if (particulars == null || particulars.trim() == "" || particulars.length == 0 || isEmpty(particulars)) {
        //mark input sa red
        $("#CreateSupItemRate-Particulars").parent().parent().addClass("has-error");
        $("#CreateSupItemRate-Particulars-errorMsg").show();
        var isValid = false;
    }

    var rate = $("#CreateSupItemRate-Rate").val();
    if (rate == null || rate.trim() == "" || isEmpty(rate)) {
        //mark input sa red
        $("#CreateSupItemRate-Rate").parent().parent().addClass("has-error");
        $("#CreateSupItemRate-Rate-errorMsg").show();
        var isValid = false;
    }

    var validFrom = $("#CreateSupItemRate-ValidFrom").val();
    if (validFrom == null || validFrom.trim() == "" || isEmpty(validFrom)) {
        //mark input sa red
        $("#CreateSupItemRate-ValidFrom").parent().parent().addClass("has-error");
        $("#CreateSupItemRate-Date-errorMsg").show();
        var isValid = false;
    }

    var validTo = $("#CreateSupItemRate-ValidTo").val();
    if (validTo == null || validTo.trim() == "" || isEmpty(validTo)) {
        //mark input sa red
        $("#CreateSupItemRate-ValidTo").parent().parent().addClass("has-error");
        $("#CreateSupItemRate-Date-errorMsg").show();
        var isValid = false;
    }

    console.log(isValid);
    console.log(particulars);
    console.log(rate);
    console.log(validTo);
    console.log(validFrom);
    console.log(isValid);
    return isValid;
}

function isEmpty(str) {
    return (!str || 0 === str.length);
}

//remove validation error on Particulars input
$("#CreateSupItemRate-Particulars").keydown(() => {
    $("#CreateSupItemRate-Particulars-errorMsg").hide();
    $("#CreateSupItemRate-Particulars").parent().parent().removeClass("has-error");
});

//remove validation error on Rate input
$("#CreateSupItemRate-Rate").keydown(() => {
    $("#CreateSupItemRate-Rate-errorMsg").hide();
    $("#CreateSupItemRate-Rate").parent().parent().removeClass("has-error");
});

//remove validation error on Date input
$("#CreateSupItemRate-ValidTo, #CreateSupItemRate-ValidFrom").keydown(() => {
    $("#CreateSupItemRate-Date-errorMsg").hide();
    $("#CreateSupItemRate-ValidTo").parent().parent().removeClass("has-error");
    $("#CreateSupItemRate-ValidFrom").parent().parent().removeClass("has-error");
});