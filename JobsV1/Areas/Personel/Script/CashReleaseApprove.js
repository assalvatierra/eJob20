

/**
 * 
 * 
 */


function CheckAll() {
    $('#cashTrans-Table tr td.item-forApproval input[type="checkbox"]').each(function () {
        $(this).prop('checked', true);
    });

    ShowApproveAllButton();
}

function ShowApproveAllButton() {
    $("#approveAll-Btn").show();
}


function ApproveSelectedItems() {
    //ShowLoading();
    let doneCount = 0;

    $('#cashTrans-Table tr td.item-forApproval input[type="checkbox"]').each(function () {
           
        if ($(this).is(':checked')) {
            doneCount = doneCount + 1;
                console.log("Checked " + $(this).val());

                let tripId = $(this).val();
                if (tripId != null || tripId != undefined) {
                    ApproveItem(this, tripId);
                }
            }
        });

    if (doneCount == 0) {
        alert("No Selected Trips");
    }

    if (GetSelectedTripCount() == doneCount) {
        //HideLoading();
    }
}



function GetSelectedTripCount() {
    let tripCount = 0;
    $('#cashTrans-Table tr td.item-forApproval input[type="checkbox"]:checked').each(function () {
        tripCount = tripCount + 1;
    });

    return tripCount;
}

function ApproveItem(e, id) {
    $.post("/Personel/CarRentalCashRelease/ApproveRequest", { id: parseInt(id) }, (result) => {
        console.log(result)
        if (result == 'True') {
            //window.location.reload();
            $(e).parent().parent().remove();
        } else {
            alert("Unable to approve selected request");
            console.log("Unable to approve selected request" + id);
        }
    }).fail(() => {
        alert("Unable to approve selected request");
        console.log("Unable to approve selected request" + id);
    });
}

$('.item-forApproval input[type="checkbox"]').on('change', () => {
    console.log(GetSelectedTripCount());
    if (GetSelectedTripCount() > 0) {
        ShowApproveAllButton();
    } else {
        $("#approveAll-Btn").hide();
    }
})