﻿
/*
 *  Supplier Inventory Items 
 */ 

$(document).ready(new function () {
    var today = moment().format('MM/DD/YYYY');
    var endDate = moment(today).add(1, 'month').format('MM/DD/YYYY');

    $("#InvRate-ValidFrom").val(today);
    $("#InvRate-ValidTo").val(endDate);

});



//ADDRESS EDIT
function ajax_createInvRate() {

    //build json object
    var data = {
        id: $("#InvRate-ItemId").val(),
        Material: $("#InvRate-Material").val(),
        Particulars: $("#InvRate-Particulars").val(),
        Rate: $("#InvRate-Rate").val(),
        Unit: $("#InvRate-UnitType").find(":selected").val(),
        ValidFrom: $("#InvRate-ValidFrom").val(),
        ValidTo: $("#InvRate-ValidTo").val(),
        Remarks: $("#InvRate-Remarks").val(),
        TradeTerm: $("#InvRate-TradeTerm").val(),
        Tolerance: $("#InvRate-Tolerance").val(),
        By: $("#InvRate-By").val(),
        ProcBy: $("#InvRate-ProcBy").val()
    };

    console.log(data);

    var url = '/Suppliers/AddRateInvItems';

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

function conformDelete(Id, desc) {
    var confirmRes = confirm("Do you want to REMOVE " + desc + " ?");
    if (confirmRes) {
        ajax_deleteInv(Id);
    }
}


function editInvRate(Id, Particulars, Material, Rate, UnitType, ValidFrom, ValidTo, Remarks, SupInvId, By, ProcBy, TradeTerm, Tolerance, DtEntered) {
    $("#EditInvRate-ItemId").val(Id);
    $("#EditInvRate-Particulars").val(Particulars);
    $("#EditInvRate-Material").val(Material);
    $("#EditInvRate-Rate").val(Rate);
    $("#EditInvRate-UnitType").val(UnitType);
    $("#EditInvRate-ValidFrom").val(ValidFrom);
    $("#EditInvRate-ValidTo").val(ValidTo);
    $("#EditInvRate-Remarks").val(Remarks);
    $("#EditInvRate-SupInvId").val(SupInvId);
    $("#EditInvRate-TradeTerm").val(TradeTerm);
    $("#EditInvRate-Tolerance").val(Tolerance);
    $("#EditInvRate-By").val(By);
    $("#EditInvRate-ProcBy").val(ProcBy);
    $("#EditInvRate-DtEntered").val(DtEntered);
}

//EDIT PRODUCT
function ajax_editInvRate() {

    //build json object
    var data = {
        id: $("#EditInvRate-ItemId").val(),
        Particulars: $("#EditInvRate-Particulars").val(),
        Material: $("#EditInvRate-Material").val(),
        Rate: $("#EditInvRate-Rate").val(),
        Unit: $("#EditInvRate-UnitType").find(":selected").val(),
        ValidFrom: $("#EditInvRate-ValidFrom").val(),
        ValidTo: $("#EditInvRate-ValidTo").val(),
        Remarks: $("#EditInvRate-Remarks").val(),
        TradeTerm: $("#EditInvRate-TradeTerm").val(),
        Tolerance: $("#EditInvRate-Tolerance").val(),
        SupInvId: $("#EditInvRate-SupInvId").val(),
        By: $("#EditInvRate-By").val(),
        ProcBy: $("#EditInvRate-ProcBy").val(),
        DtEntered: $("#EditInvRate-DtEntered").val()
    };

    console.log(data);

    var url = '/Suppliers/EditRateInvItems';

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

function conformDelete(Id, desc) {
    var confirmRes = confirm("Do you want to REMOVE " + desc + " ?");
    if (confirmRes) {
        ajax_deleteInv(Id);
    }
}

//Item Delete
function ajax_deleteInv(Id) {

    //build json object
    var data = {
        id: Id,
    };

    console.log(data);

    var url = '/Suppliers/RemoveInvItems';

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

function AddRateInitial(id, itemName) {
    $("#InvRate-ItemId").val(id);
    $("#InvRate-ItemName").text(itemName)
}

function confirmRateDelete(id, desc) {
    var res = confirm("Do you want to remove " + desc + "?")
    if (res) {
        ajax_deleteItemRate(id);
    }
}

//Item Delete
function ajax_deleteItemRate(Id) {

    //build json object
    var data = {
        id: Id,
    };

    console.log(data);

    var url = '/Suppliers/RemoveRateInvItem';

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