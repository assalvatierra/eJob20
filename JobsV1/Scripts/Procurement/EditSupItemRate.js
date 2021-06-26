
/**
 *  Edit Supplier Item Rate
 */

function Submit_EditSupItemRate() {
    //console.log("Validating inputs");
    if (Validate_EditSupItemRate()) {
        //console.log("Edit Validate True");
        Ajax_EditSupplierItem();
    }
}

function Init_EditSupItem(itemId) {
    $("#EditSupItemRate").modal("show");
    console.log("Edit Item : " + itemId);
    $.get("/Procurement/GetSupItemDetails", { id: itemId })
        .done((response) => {
            //console.log(response);
            //console.log(response.Description);

                //populate Edit Sup Item Fields
                $("#EditSupItemRate-ItemLeadId").val(response.Id);
                $("#EditSupItemRate-Supplier").val(response.SupplierId);
                $("#EditSupItemRate-Rate").val(response.ItemRate);
                $("#EditSupItemRate-Particulars").val(response.Particulars);
                $("#EditSupItemRate-Materials").val(response.Material);
                $("#EditSupItemRate-UnitType").val(response.SupplierUnitId);
                $("#EditSupItemRate-TradeTerm").val(response.TradeTerm);
                $("#EditSupItemRate-Tolerance").val(response.Tolerance);
                $("#EditSupItemRate-Remarks").val(response.Remarks);
                $("#EditSupItemRate-ValidFrom").val(moment(response.DtValidFrom).format("MM/DD/YYYY"));
                $("#EditSupItemRate-ValidTo").val(moment(response.DtValidTo).format("MM/DD/YYYY"));
                $("#EditSupItemRate-OfferedBy").val(response.By);
                $("#EditSupItemRate-ProcBy").val(response.ProcBy);
                $("#EditSupItemRate-Origin").val(response.Origin);

                $("#EditSupItemRate-Item-Text").text(response.Description);
        })
        .fail((err) => {
            $("#EditSupItemRate").modal("hide");
            alert("Unable to Get Item Details");
        });
}

function Ajax_EditSupplierItem() {

    var editSupplierItem = {
        supItemId:      $("#EditSupItemRate-ItemLeadId").val(),
        supplierId:     $("#EditSupItemRate-Supplier").val(),
        particulars:    $("#EditSupItemRate-Particulars").val(),
        materials:      $("#EditSupItemRate-Materials").val(),
        rate:           $("#EditSupItemRate-Rate").val(),
        unitTypeId:     $("#EditSupItemRate-UnitType").val(),
        tradeTerm:      $("#EditSupItemRate-TradeTerm").val(),
        tolerance:      $("#EditSupItemRate-Tolerance").val(),
        remarks:        $("#EditSupItemRate-Remarks").val(),
        validFrom:      $("#EditSupItemRate-ValidFrom").val(),
        validTo:        $("#EditSupItemRate-ValidTo").val(),
        offeredBy:      $("#EditSupItemRate-OfferedBy").val(),
        procuredBy:     $("#EditSupItemRate-ProcBy").val(),
        origin:         $("#EditSupItemRate-Origin").val(),
    }

    //console.log(editSupplierItem);

    $.post("/Procurement/EditSupplierItem", editSupplierItem, (res) => {
        //console.log(res);
    }).done((result) => {
        //console.log(result)
        if (result == "True") {
            $("#EditSupItemRate").modal("hide");
            window.location.reload(false);
        } else {
            alert("Unable to Create Supplier Item.");
        }
        //console.log(result);
    }).fail((err) => {
        console.log(err);
        alert("Unable to Create Supplier Item.");
    });
}

//Create Supplier Item Validation
function Validate_EditSupItemRate() {
    var validationMsg = "";
    var isValid = true;

    var particularsField = $("#EditSupItemRate-Particulars");
    var rateField = $("#EditSupItemRate-Rate");
    var validFromField = $("#EditSupItemRate-ValidFrom");
    var validToField = $("#EditSupItemRate-ValidTo");
    var itemLeadIdField = $("#EditSupItemRate-ItemLeadId");

    $("#EditSupItemRate-Particulars-errorMsg").hide();
    particularsField.parent().parent().removeClass("has-error");
    rateField.parent().parent().removeClass("has-error");
    validFromField.parent().parent().removeClass("has-error");
    validToField.parent().parent().removeClass("has-error");

    var salesLead_Id = itemLeadIdField.val();
    if (salesLead_Id == null || salesLead_Id == '' ) {
        //mark input sa red
        itemLeadIdField.parent().parent().addClass("has-error");
        var isValid = false;
    }

    var particulars = particularsField.val();
    if (particulars == null || particulars.trim() == "" || particulars.length == 0 || isEmpty(particulars)) {
        //mark input sa red
        particularsField.parent().parent().addClass("has-error");
        $("#EditSupItemRate-Particulars-errorMsg").show();
        var isValid = false;
    }

    var rate = rateField.val();
    if (rate == null || rate.trim() == "" || isEmpty(rate)) {
        //mark input sa red
        rateField.parent().parent().addClass("has-error");
        $("#EditSupItemRate-Rate-errorMsg").show();
        var isValid = false;
    }

    var validFrom = validFromField.val();
    if (validFrom == null || validFrom.trim() == "" || isEmpty(validFrom)) {
        //mark input sa red
        validFromField.parent().parent().addClass("has-error");
        $("#EditSupItemRate-Date-errorMsg").show();
        var isValid = false;
    }

    var validTo = validToField.val();
    if (validTo == null || validTo.trim() == "" || isEmpty(validTo)) {
        //mark input sa red
        validToField.parent().parent().addClass("has-error");
        $("#EditSupItemRate-Date-errorMsg").show();
        var isValid = false;
    }

    //console.log(isValid);
    //console.log(particulars);
    //console.log(rate);
    //console.log(validTo);
    //console.log(validFrom);
    //console.log(isValid);

    return isValid;
}

function isEmpty(str) {
    return (!str || 0 === str.length);
}

//remove validation error on Particulars input
$("#EditSupItemRate-Particulars").keydown(() => {
    $("#EditSupItemRate-Particulars-errorMsg").hide();
    $("#EditSupItemRate-Particulars").parent().parent().removeClass("has-error");
});

//remove validation error on Rate input
$("#EditSupItemRate-Rate").keydown(() => {
    $("#EditSupItemRate-Rate-errorMsg").hide();
    $("#EditSupItemRate-Rate").parent().parent().removeClass("has-error");
});

//remove validation error on Date input
$("#EditSupItemRate-ValidTo, #CreateSupItemRate-ValidFrom").keydown(() => {
    $("#EditSupItemRate-Date-errorMsg").hide();
    $("#EditSupItemRate-ValidTo").parent().parent().removeClass("has-error");
    $("#EditSupItemRate-ValidFrom").parent().parent().removeClass("has-error");
});
