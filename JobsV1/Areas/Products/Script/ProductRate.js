
/**
  * ProductRate.js
  * Perform Add, Edit and Remove functions 
  * using modal. Used with Partial Views in 
  * details.
  */

//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_PostRate(prodId) {
    var rateQty = $("#prod-rateQty").val();
    var rateUom = $("#prod-rateUom").val();
    var ratePubRate = $("#prod-ratePub").val();
    var rateConRate = $("#prod-rateCon").val();

    var query = "";
    //build json object // no use
    var data = {
        prodId: prodId,
        qty: rateQty,
        uomId: rateUom,
        rate: ratePubRate,
        drate: rateConRate
    };

    url = "/Products/SMProducts/AddProdRate";

    //Submit data to server using ajax
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log("SUCCESS");
        },
        error: function (data) {
            //// console.log("ERROR");
            console.log(data);
            //location.reload();
            addRateToDisplay(rateQty, ratePubRate, rateConRate)
        }
    });
}

//Edit - POST
//Submit changes to the product
//using ajax, then reload page
function ajax_EditRate(prodId) {
    var Id = $("#edit-rateId").val();
    var rateQty = $("#edit-rateQty").val();
    var rateUom = $("#edit-rateUom").val();
    var ratePubRate = $("#edit-ratePub").val();
    var rateConRate = $("#edit-rateCon").val();

    var query = "";
    //build json object // no use
    var data = {
        id: Id,
        prodId: prodId,
        qty: rateQty,
        uomId: rateUom,
        rate: ratePubRate,
        drate: rateConRate
    };

    url = "/Products/SMProducts/EditProdRate";

    //Submit data to server using ajax
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log("SUCCESS");
        },
        error: function (data) {
            //// console.log("ERROR");
            console.log(data);
            location.reload();
        }
    });
}

//Edit
//transfer values of selected product
//to the edit form
function EditRate(Id, qty, uomId, prodRate, conRate) {
    $("#edit-rateId").val(Id);
    $("#edit-rateQty").val(qty);
    $("#edit-rateUom").val(uomId);
    $("#edit-ratePub").val(prodRate);
    $("#edit-rateCon").val(conRate);
}

function refreshPage() {
    location.reload();
}

//add rate to view 
function addRateToDisplay(qty, prate,crate) {
    content = "<tr>";
    content += "<td>" + qty + " Pax </td>";
    content += "<td>" + prate + "</td>";
    content += "<td>" + crate + "</td>"; 
    content += "  ";
    content += "</tr>";
    $(content).appendTo("#section-rates");

    //close modal
    $('#prodRateModal').modal('toggle');
}

