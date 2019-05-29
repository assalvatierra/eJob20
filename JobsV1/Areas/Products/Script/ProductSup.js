
/**
  * ProductDesc.js
  * Perform Add, Edit and Remove functions 
  * using modal. Used with Partial Views in 
  * details.
  */

function getDatetoday() {
    const now = new Date();
    var today = moment(now).format('YYYY-MM-DD');
    return today;
}

//Add Product Category
function ajax_PostSupplier(prodId) {

    var supplier = $("#prodsup-suppliers").val();
    var validstart = $("#prodsup-validstart").val();
    var validend = $("#prodsup-validend").val();
    var price = $("#prodsup-price").val();
    var contracted = $("#prodsup-contracted").val();

    var query = "";
    //build json object
    var data = {
        prodId: prodId,
        supId: supplier,
        startdate: validstart,
        enddate: validend,
        price: price,
        contracted: contracted
    };

    var url = '/Products/SMProducts/AddProdSup';

    //request data from server using ajax call
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

//Edit Product Category
function ajax_EditSupplier(prodId) {

    var Id = $("#editsup-Id").val();
    var supplier = $("#editsup-suppliers").val();
    var validstart = $("#editsup-validstart").val();
    var validend = $("#editsup-validend").val();
    var price = $("#editsup-price").val();
    var contracted = $("#editsup-contracted").val();

    //alert(Id);

    //build json object
    var data = {
        Id: Id,
        prodId: prodId,
        supId: supplier,
        startdate: validstart,
        enddate: validend,
        price: price,
        contracted: contracted
    };

    var url = '/Products/SMProducts/EditProdSup';

    //request data from server using ajax call
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

//edit on click
function editProdSup(Id, SupplierId, Start, End, Price, Cont) {
    //convert string date to date obj
    var startdate = moment(Start).format('YYYY-MM-DD');
    var enddate = moment(End).format('YYYY-MM-DD');

    $("#editsup-Id").val(Id);
    $("#editsup-suppliers").val(SupplierId);
    $("#editsup-validstart").val(startdate);
    $("#editsup-validend").val(enddate);
    $("#editsup-price").val(Price);
    $("#editsup-contracted").val(Cont);

}

function refreshPage() {
    location.reload();
}


function removeSup(Id, desc) {
    if (confirm('Are you sure you want to remove "' + desc + '"?')) {
        ajax_RemoveSup(Id);
    } else {
        // Do nothing!
    }
}

function ajax_RemoveSup(Id) {
    //build json object

    var data = {
        id: Id
    };

    var url = '/Products/SMProducts/RemoveProdSup';

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


