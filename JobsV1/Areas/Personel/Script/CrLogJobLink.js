
/**
 * CrLogJobLink
 * Desc: Link Triplogs and Jobs by triplogId & jobmainId
 */

function Show_JobsLinkModal(triplogid) {
    $("#LogJobLinkModal").modal("show");
    Set_JobLinkForm_TripLogId(triplogid);

    //get existing triploglink for edit/modify link
    GetTripJobLinkId(triplogid);
}

function Show_JobSearchModal() {
    $("#LogJobSearchModal").modal("show");
    GetActiveJobs();

}

function GetActiveJobs() {

    //empty list
    $("#LogJobSearchModal-List").empty();

    $.get("/JobMains/GetActiveJobList", null, (res) => {
        console.log(res.length)
        if (res.length != 0) {
            for (var i = 0; i < res.length; i++) {
                var itemId = res[i]["Id"];
                var desc = res[i]["JobDesc"] == null ? " " : res[i]["JobDesc"] + " / ";
                var customer = res[i]["Customer"];
                var company = res[i]["Company"] == null ? " " : res[i]["Company"] + " / ";
                var dtStart = res[i]["JobDateStart"];
                var dtEnd = res[i]["JobDateEnd"];

                var item = '<button type="button" class="list-group-item" onclick="SelectJobMainId(' + itemId + ')">' +
                    itemId + ' - ' + desc + ' ' + company + ' ' + customer + " <br> Date: " + dtStart + " - " + dtEnd
                '</button>';

                $("#LogJobSearchModal-List").append(item);
            }
        }
    });
}

function SelectJobMainId(jobId) {
    Set_JobLinkForm_JobMainId(jobId);

    //hide job search modal
    $("#LogJobSearchModal").modal("hide");
}

function Set_JobLinkForm_JobMainId(jobId) {
    $("#LogJobLink-JobMainId").val(jobId);

}

function Set_JobLinkForm_TripLogId(triplogId) {
    $("#LogJobLink-TripLogId").val(triplogId);
}

function Submit_JobLinkForm() {
    var triplogId = $("#LogJobLink-TripLogId").val();
    var jobmainId = $("#LogJobLink-JobMainId").val();
    $.post("/CarRentalLog/SetLinkTriplogJobs", { "triplogId": triplogId, "jobmainId": jobmainId }, (res) => {
        console.log(res);
        if (res == "True") {
            //alert("Linked Triplogs and jobs DONE.");

            //on success, hide modal
            $("#LogJobLinkModal").modal("hide");

            //reload
            window.location.reload();
        }

    })
        .fail((err) => {
            alert("Unable to Link Triplogs and jobs.");
        });
}


function GetTripJobLinkId(tripLogId) {
    $.get("/CarRentalLog/GetLinkTriplogJobs", { tripLogId: tripLogId }, (res) => {
        var jobId = res;
        console.log("existing jobmainId: " + res);
        if (jobId != 0) {
            Set_JobLinkForm_JobMainId(jobId);

            //show remove btn
            $("#LogJobLinkModal_RemoveBtn").show();
        } else {
            //clear text field
            $("#LogJobLink-JobMainId").val("");

            //hide remove btn
            $("#LogJobLinkModal_RemoveBtn").hide();
        }

    })
}

function RemoveTripJobLink() {

    var triplogId = $("#LogJobLink-TripLogId").val();
    var jobmainId = $("#LogJobLink-JobMainId").val();

    $.post("/CarRentalLog/DeleteLinkTriplogJobs", { triplogId: triplogId, jobmainId, jobmainId }, (res) => {
        console.log(res);
        if (res == "True") {
            //on success, hide modal
            $("#LogJobLinkModal").modal("hide");

            //reload
            window.location.reload();
        }


    })
}
