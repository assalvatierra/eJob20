

/**
 *  Cash Tranasctions Release / Approve Cash Transactions
 *  For Salary, CA, Payments and other transactions
 */

function FilterStatus(statusId) {
    switch (statusId) {
        case '1':
            $("#status-request").css('color', 'black');
            break;
        case '2':
            $("#status-approved").css('color', 'black');
            break;
        case '3':
            $("#status-released").css('color', 'black');
            break;
        case '4':
            $("#status-returned").css('color', 'black');
            break;
        default:
            $("#status-request").css('color', 'black');
            break;
    }
}

function ApproveRequest(e, id) {
    $.post("/Personel/CarRentalCashRelease/ApproveRequest", { id: parseInt(id) }, (result) => {
        console.log(result)
        if (result == 'True') {
            //window.location.reload();
            $(e).parent().parent().parent().remove();
        } else {
            alert("An Error occured while process your request");
        }
    });
}

function ApproveRelease(e, id) {
    $.post("/Personel/CarRentalCashRelease/ApproveRelease", { id: parseInt(id) }, (result) => {
        console.log(result)
        if (result == 'True') {
            //window.location.reload();
            $(e).parent().parent().parent().remove();
        } else {
            alert("An Error occured while process your request");
        }
    });
}

/**
 *  On Cash Transactions REQUEST FOR APPROVAL
 * */

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

/**
 *  On Cash Transactions RELEASE
 * */

function CheckAllRelease() {
    $('#cashTrans-Table tr td.item-forRelease input[type="checkbox"]').each(function () {
        $(this).prop('checked', true);
    });

    ShowReleaseAllButton();
}

function ShowReleaseAllButton() {
    $("#releaseAllBtn").show();
}

function ReleaseSelectedItems() {
    //ShowLoading();
    let doneCount = 0;

    $('#cashTrans-Table tr td.item-forRelease input[type="checkbox"]').each(function () {

        if ($(this).is(':checked')) {
            doneCount = doneCount + 1;

            let tripId = $(this).val();
            if (tripId != null || tripId != undefined) {
                ReleaseItem(this, tripId);
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

function ReleaseItem(e, id) {
    $.post("/Personel/CarRentalCashRelease/ApproveRelease", { id: parseInt(id) }, (result) => {
        console.log(result)
        if (result == 'True') {
            $(e).parent().parent().remove();
        } else {
            alert("Unable to release selected request");
            console.log("Unable to approve selected request" + id);
        }
    }).fail(() => {
        alert("Unable to release selected request");
        console.log("Unable to approve selected request" + id);
    });
}
