

//******** Add Item Rate Modal ****************/

var SLID = 0;

function addSupItemToView(supId, rateId, item, supplier, ratePerUnit) {
    content = "<span>";
    content += " " + item + " - " + ratePerUnit + " (" + supplier + ")";
    content += "</span><br>";
    $(content).appendTo("#SalesLeadItems-" + SLID);

    //close modal
    $('#selectSupplierItem-' + supId).modal('toggle');

    //console.log("content:" + content);

    //add to db
    ajax_AddSupRate(SLID, rateId);
}

function addSupItem(salesLeadId) {
    SLID = salesLeadId;
    //console.log("SLID:" + SLID);
}


function GetItemSuppliers(itemId, item, itemLeadId, salesLeadId) {
    $('#modal-supitems-title').text(item);

    //set sales lead ID for creating supplier item rate
    $("#CreateSupItemRate-ItemLeadId").val(itemLeadId);
    $("#CreateSupItemRate-Item").val(itemId);
    $("#CreateSupItemRate-Item-Text").text(item);

    //console.log("ItemLeadID: " + itemLeadId);
     
    //build json object
    var data = {
        id: itemId
    };

    //console.log(item);
    //request data from server using ajax call
    $.ajax({
        url: '/Procurement/GetItemSuppliers/' + itemId,
        type: "GET",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            //console.log("ERROR");
            //console.log(data);
            LoadTable(data, itemLeadId);
        }
    });
}


function GetItemSuppliers_SL(itemId, item, itemLeadId, salesLeadId) {
    $('#modal-supitems-title').text(item);

    //set sales lead ID for creating supplier item rate
    $("#CreateSupItemRate-ItemLeadId").val(itemLeadId);
    $("#CreateSupItemRate-Item").val(itemId);
    $("#CreateSupItemRate-Item-Text").text(item);

    //console.log("ItemLeadID: " + itemLeadId);

    //build json object
    var data = {
        id: itemId
    };

    //console.log(item);
    //request data from server using ajax call
    $.ajax({
        url: '~/Procurement/GetItemSuppliers/' + itemId,
        type: "GET",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            //console.log("ERROR");
            //console.log(data);
            LoadTable(data, itemLeadId);
        }
    });
}


//display simple/limited information
//of suppliers
function LoadTable(data, itemLeadId) {

    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);

    //remove existing items
    removeSupItemsModal();

    //populate table content
    for (var x = 0; x < temp.length; x++) {
        var particulars = temp[x]["Particulars"] != undefined ? temp[x]["Particulars"] : " ";
        var materials = temp[x]["Materials"] != undefined ? temp[x]["Materials"] : " ";
        content = '<a class="list-group-item skills" id="supItemDesc" data-toggle="modal" data-target="#selectSupplier'
            +'data - dismiss="modal" onclick = "AddSupItemRate(' + temp[x]["Id"] + ',' + itemLeadId + ')" > '
            + "<b>" + temp[x]["SupplierName"] + ' <br /> '
            + "<span>" + temp[x]["Rate"] + ' per '
            + temp[x]["Unit"] + '</ span> -  '
            + '' + particulars + ' ' + materials + ' </b> '
            + ' <br /> Validity: '
            + moment(temp[x]["ValidStart"]).format("MMM DD YYYY") + ' - '
            + moment(temp[x]["ValidEnd"]).format("MMM DD YYYY") + ' ' +
            ' </a> ';

        if (moment(temp[x]["ValidEnd"]).diff(moment(), 'days') > 0) {
            
            $(content).appendTo("#SupItems-modal-content");
        }
    }
}

//add supplier item quoted rate to the Sales Lead item
function AddSupItemRate(itemId, salesleadId) {
    //build json object
    var data = {
        SalesLeadId: salesleadId,
        ItemRateId: itemId
    };

    var url = '/SalesLeads/AddSupItemRate';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {

        },
        error: function (data) {
            //console.log(data);
            location.reload();
        }
    });
}

function removeSupItemsModal() {
    $("#SupItems-modal-content").empty();
}

function RemoveSupItems(id) {
    //build json object
    var data = {
        id: id
    };

    var url = '/SalesLeads/RemoveSupItemRate';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {

        },
        error: function (data) {
            //console.log(data);
            location.reload(false);
        }
    });
}



function displayNewItem(invId, itemId) {
    var content = $("#supItemDesc-" + itemId);
    $(content).appendTo("#supItem-" + invId);
}

function AddSupplierItemRate() {
    $("#AddSupItemRate").modal('show');
}