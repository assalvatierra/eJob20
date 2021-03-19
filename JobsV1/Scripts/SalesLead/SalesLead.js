/**
 *  Sales Leads
 */

// Filter Loading on click
$("#newLead, #acceptedLead, #closedLead, #ongoing, #rejectedLead, #allLead").click(() => {
    $("#overlay").show();
});


function setLeadId(id) {
    $("#supItem-supId").val(id);
}

//add item
function ajax_AddSalesLeadItem() {
    //build json object
    console.log("data: " + $("#supItem-Items option:selected").val());

    var data = {
        SalesLeadId: $("#supItem-supId").val(),
        ItemId: $("#supItem-Items option:selected").val(),
        price: $("#supItem-Rate").val(),
        remarks: $("#supItem-Remarks").val()
    };
    console.log(data);
    var url = '/SalesLeads/addSupItem';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {

        },
        error: function (data) {
            console.log(data);
            location.reload(false);
        }
    });
}

//Remove Item
function ajax_RemoveItem(Id) {
    //build json object

    var data = {
        id: Id
    };

    var url = '/SalesLeads/RemoveItem';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {

        },
        error: function (data) {
            console.log(data);
            location.reload(false);
        }
    });
}

function EditLeadItem(id, invItemId, QuotedRate, Remarks, itemName) {
    $('#edit-supItem-Id').val(id);
    $('#edit-supItem-Items').val(invItemId);
    $('#edit-supItem-Rate').val(QuotedRate);
    $('#edit-supItem-Remarks').val(Remarks);

    $('#editItemName').text(itemName);
}

//Edit Item
function ajax_EditLeadItem() {
    //build json object
    var Id = $('#edit-supItem-Id').val();
    var InvItemId = $('#edit-supItem-Items').val();
    var Rate = $('#edit-supItem-Rate').val();
    var Remarks = $('#edit-supItem-Remarks').val();
    var data = {
        id: Id,
        invItemId: InvItemId,
        rate: Rate,
        remarks: Remarks
    };
    console.log(data);
    var url = '/SalesLeads/EditItem';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log(data);
        },
        error: function (data) {
            console.log(data);
            location.reload(false);
        }
    });
}

//****************************
//*** Handle Invalid Input ***
//****************************

//prevent invalid inputs
function validate(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

//handle numbers only on input
$('#edit-supItem-Rate').on('input', function () {
    //backspace
    if (isNaN($('#edit-supItem-Rate').val())) {
        var txt = $('#edit-supItem-Rate');
        txt.val(txt.val().slice(0, -1));
    }

    if ($('#edit-supItem-Rate').val() < 0) {
        $('#edit-supItem-Rate').val(0);
    }
});

//handle numbers only on input
$('#supItem-Rate').on('input', function () {
    //backspace
    if (isNaN($('#supItem-Rate').val())) {
        var txt = $('#supItem-Rate');
        txt.val(txt.val().slice(0, -1));
    }

    if ($('#supItem-Rate').val() < 0) {
        $('#supItem-Rate').val(0);
    }
});


//Update Activity Status
function UpdateLeadStatus(e, leadId, statusId) {
    $.post("/SalesLeads/PostLeadStatus", { slId: leadId, StatusId: statusId }, (result) => {
        if (result == 'False') {
            alert("Unable to Update Status")
        }
    }).done((result) => {
        //on update success
        $(e).parent().removeClass("btn-primary");
        $(e).parent().addClass("btn-success");
    }).fail(() => {
        //on update failed
        console.log("On Fail");
        console.log(result);

        $(e).parent().removeClass("btn-primary");
        $(e).parent().addClass("btn-primary");

        //display alert message
        alert("Unable to Update Status")
    });

}


//Update Activity Status
function UpdateLeadStatusRemarks(e, leadId, statusId) {
    $.post("/SalesLeads/PostLeadStatus", { slId: leadId, StatusId: statusId }, (result) => {
        if (result == 'False') {
            alert("Unable to Update Status")
        }
    }).done((result) => {
        //on update success
        $(e).parent().removeClass("btn-primary");
        $(e).parent().addClass("btn-success");

        //show add Remarks 
        $("#SalesLeadRemarks-Id").val(leadId);

        $("#SalesLeadRemarksModal").modal('show');
    }).fail(() => {
        //on update failed
        console.log("On Fail");
        console.log(result);

        $(e).parent().removeClass("btn-primary");
        $(e).parent().addClass("btn-primary");

        //display alert message
        alert("Unable to Update Status")
    });

}


function SubmitSalesLeadRemarks() {
    var id = $("#SalesLeadRemarks-Id").val();
    var remarks = $("#SalesLeadRemarks-Remarks").val();

    $.post("/SalesLeads/UpdateSalesLeadRemarks", { Id: id, Remarks: remarks })
        .done((res) => {
            $("#SalesLeadRemarksModal").modal('hide');
            window.location.reload(false);
        })
        .fail((err) => {
            alert("Unable to Update Sales Lead Remarks");
        });
}