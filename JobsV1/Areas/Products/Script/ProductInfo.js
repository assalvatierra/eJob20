
/**
  * ProductInfo.js
  * Perform Add, Edit and Remove functions 
  * using modal. Used with Partial Views in 
  * details.
  */


//load table content on search btn click  
//request data from server using ajax call
//then clear and add contents to the table 
function ajax_PostInfo(prodId) {
    var infolabel = $("#prod-Infolabel").val();
    var infoValue = $("#prod-InfoValue").val();
    var infoRemarks = $("#prod-InfoRemarks").val();

    var query = "";
    //build json object // no use
    var data = {
        prodId: prodId,
        infolabel: infolabel,
        infoValue: infoValue,
        infoRemarks: infoRemarks
    };

    url = "/Products/SMProducts/AddInfo";

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

//Edit - POST
//Submit changes to the product
//using ajax, then reload page
function ajax_EditInfo(prodId) {
    var infoId = $("#edit-InfoId").val();
    var infolabel = $("#edit-Infolabel").val();
    var infoValue = $("#edit-InfoValue").val();
    var infoRemarks = $("#edit-InfoRemarks").val();

    var query = "";
    //build json object // no use
    var data = {
        id: infoId,
        prodId: prodId,
        infolabel: infolabel,
        infoValue: infoValue,
        infoRemarks: infoRemarks
    };

    url = "/Products/SMProducts/EditInfo";

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
function EditInfo(Id, label, value, remarks) {
    $("#edit-InfoId").val(Id);
    $("#edit-Infolabel").val(label);
    $("#edit-InfoValue").val(value);
    $("#edit-InfoRemarks").val(remarks);
}

function refreshPage() {
    location.reload();
}