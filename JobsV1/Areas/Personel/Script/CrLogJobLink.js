
/**
 * CrLogJobLink
 * Desc: Link Triplogs and Jobs by triplogId & jobmainId
 */

function Show_JobsLinkModal(triplogid) {
    $("#LogJobLinkModal").modal("show");

    Set_JobLinkForm_TripLogId(triplogid);

    GetActiveJobs();

    //get existing triploglink for edit/modify link
    GetTripJobLinkId(triplogid);
}


function GetActiveJobs() {

    $("#Loading-jobs").show();

    //empty list
    $("#LogJobSearchModal-List").empty();

    $.get("/JobOrder/GetActiveJobList", null, (res) => {
        console.log(res.length)

        $("#Loading-jobs").hide();
        if (res.length != 0) {
            for (var i = 0; i < res.length; i++) {
                var itemId = res[i]["Id"];
                //var desc = res[i]["JobDesc"] == null ? " " : res[i]["JobDesc"] + " / ";
                var customer = res[i]["Customer"];
                var company = res[i]["Company"] == null ? " " : res[i]["Company"] + " / ";
                //var dtStart = res[i]["JobDateStart"];
                //var dtEnd = res[i]["JobDateEnd"];

                var item = '<button type="button" class="list-group-item" onclick="SelectJobMainId(' + itemId + ')">' +
                    itemId + ' - ' + company + ' ' + customer + 
                '</button>';

                $("#LogJobSearchModal-List").append(item);
            }
        }
    });
}

function SelectJobMainId(jobId) {
    Set_JobLinkForm_JobMainId(jobId);

    //select and submit
    Submit_JobLinkForm()
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

    $.get("/JobMains/CheckJobById", { jobmainId: jobmainId }, (res) => {

    }).done((res) => {
        if (res == "True") {

            $.post("/CarRentalLog/SetLinkTriplogJobs", { "triplogId": triplogId, "jobmainId": jobmainId }, (res) => {
                console.log(res);
                if (res == "True") {
                    //alert("Linked Triplogs and jobs DONE.");

                    //on success, hide modal
                    $("#LogJobLinkModal").modal("hide");

                    //reload
                    window.location = window.location;
                }
            })
                .fail((err) => {
                    alert("Unable to Link Triplogs and jobs.");
                });
        } else {
            alert("No jobs found with the entered Id.");
        }

    });
    

}


function GetTripJobLinkId(tripLogId) {
    $.get("/CarRentalLog/GetLinkTriplogJobs", { tripLogId: tripLogId }, (res) => {
        var jobId = res;
        console.log("existing jobmainId: " + res);
        if (jobId != 0) {
            Set_JobLinkForm_JobMainId(jobId);

        } else {
            //clear text field
            $("#LogJobLink-JobMainId").val("");

        }

    })
}

function RemoveTripJobLink(tripId, jobmainId) {

    $.post("/CarRentalLog/DeleteLinkTriplogJobs", { triplogId: tripId, jobmainId, jobmainId }, (res) => {
        console.log(res);
        if (res == "True") {
            //on success, hide modal
            $("#LogJobLinkModal").modal("hide");

            //reload
            window.location = window.location;
        }


    })
}

async function  CheckJobExist() {
    var jobmainId = $("#LogJobLink-JobMainId").val();
    await $.get("/JobMains/CheckJobById", { jobmainId: jobmainId }, (res) => {
       
    }).done(( res) => {
        if ( res == "True") {
            return true;
        }

        return false;
    });
}
