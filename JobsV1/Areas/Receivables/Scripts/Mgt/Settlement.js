
function UpdateStatus(transId, statusId) {
    $("#overlay").show();
    var result = $.post("/Receivables/ArMgt/UpdateTransStatus", {
        transId: transId,
        statusId: statusId
    }, (response) => {
        console.log("Update Status : " + response);
        if (response == "True") {
            $("#overlay").hide();
            window.location.reload(false);
        } else {
            alert("Unable to Update transaction.");
            $("#overlay").hide();
        }
    }
    );
    console.log(result);
    if (result["ResponseCode"] == 500) {
        alert("Unable to Update transaction.");
        $("#overlay").hide();
    }
}
