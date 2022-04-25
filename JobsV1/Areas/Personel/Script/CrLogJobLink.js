
/**
 * CrLogJobLink
 * Desc: Link Triplogs and Jobs by triplogId & jobmainId
 *       Show Modal for linking jobs and triplog
 *       Show List of active jobs
 * 
 * Validations:
 *  - Check if job item count is greater than 0
 *  - Check remaining linked job item count is greater than 1
 *  - Check Job date is within the current triplog date (warning)
 *  - Added loading animation
 *  - Added Option to input jobId for past jobs
 * 
 * 4/12/2022
 *  - Remove Page Reload on Submit Link or on select Active Job List
 *  - Remove Page Reload on Remove Link 
 *  - After Submit Link Click, hide modal and add tripId to triplog (no reload)
 *  
 * 4/18/2022
 *  - Added tripDate to GetActiveJobs() ->  GetActiveJobs(tripDate)
 *  - ActiveJobs results are filtered based within date of the job
 *  - Added validation Alerts on no jobs found and invalid triplog date (on input jobId)
 * 
 */

function Show_JobsLinkModal(triplogid, tripDate) {
    $("#LogJobLinkModal").modal("show");

    Set_JobLinkForm_TripLogId(triplogid);
    Set_JobLinkForm_TripLogDate(tripDate);

    GetActiveJobs(tripDate);

    //get existing triploglink for edit/modify link
    GetTripJobLinkId(triplogid);

}


function GetActiveJobs(tripDate) {

    $("#Loading-jobs").show();

    //empty list
    $("#LogJobSearchModal-List").empty();

    $.get("/JobOrder/GetActiveJobList",  { tripDate: tripDate }, (res) => {
        }).done((res) => {
            $("#Loading-jobs").hide();

            AddSelectJobButton(res);
        }).fail((err) => {
            $("#Loading-jobs").hide();
            console.log("Unable to get jobs.");
        });
}

function AddSelectJobButton(jobdetails) {
    if (jobdetails.length != 0) {
        for (var i = 0; i < jobdetails.length; i++) {
            var itemId   = jobdetails[i]["Id"];
            var noItems  = jobdetails[i]["NoItems"];
            var customer = jobdetails[i]["Customer"];
            var company  = jobdetails[i]["Company"] == null ? " " : jobdetails[i]["Company"] + " / ";
            var dtStart  = moment(jobdetails[i]["JobDateStart"]).format("MM/DD/YYYY");
            var dtEnd    = moment(jobdetails[i]["JobDateEnd"]).format("MM/DD/YYYY");

            var item = "<button type='button' class='list-group-item' onclick='SelectJob(" + itemId + ","
                + noItems + ",\"" + dtStart + "\",\"" + dtEnd + "\")'>" +
                itemId + ' - ' + company + ' ' + customer + "<br>" +
                dtStart + " - " + dtEnd + ' /  Assigned:' + noItems +
                '</button>';

            //add to list
            $("#LogJobSearchModal-List").append(item);
        }
    }
   
}

async function SelectJob(jobId,  noItems, dtStart, dtEnd) {
    var PostFlag = true;
    var linkCount = await GetTripLinkCount(jobId);

    Set_JobLinkForm_JobMainId(jobId);

    PostFlag = ConfirmTripDate(dtStart, dtEnd);

    PostFlag = ConfirmLinkCount(noItems, linkCount);

    if (PostFlag) {
        //select and submit
        Submit_JobLinkForm();
    }
}

function ConfirmTripDate(dtStart, dtEnd) {

    var jobDtStart = moment(dtStart);
    var jobDtEnd = moment(dtEnd);
    var tripdate = moment($("#LogJobLink-TripLogDate").val());

    console.log("jobDtStart: " + jobDtStart);
    console.log("jobDtEnd: " + jobDtEnd);
    console.log("tripdate: " + tripdate);

    if (tripdate < jobDtStart || tripdate > jobDtEnd) {

        if (confirm("Trip date is not within the Job date.")) {
            return false;
        } else {
            return false;
        }
    }
    return true;
}

function ConfirmLinkCount(noItems, linkCount) {

    if (linkCount >= noItems) {

        if (confirm("Unit allocation for the job is " + linkCount +
            " of " + noItems + ". Do you want to continue?")) {
             return false;
        }
    }
    return true;
}

function Set_JobLinkForm_JobMainId(jobId) {
    $("#LogJobLink-JobMainId").val(jobId);
}

function Set_JobLinkForm_TripLogId(triplogId) {
    $("#LogJobLink-TripLogId").val(triplogId);
}

function Set_JobLinkForm_TripLogDate(tripDate) {
    $("#LogJobLink-TripLogDate").val(tripDate);
}

async function Submit_JobLinkForm() {
    var triplogId = $("#LogJobLink-TripLogId").val();
    var jobmainId = $("#LogJobLink-JobMainId").val();

    //Validation, Check job exist
    if (await CheckJobExist()) {
        //validation check job dates
        if (await CheckTripLogDate()) {

            await $.post("/CarRentalLog/SetLinkTriplogJobs", { "triplogId": triplogId, "jobmainId": jobmainId }, (res) => {
            }).done((res) => {

                //on success, hide modal
                $("#LogJobLinkModal").modal("hide");

                //reload
                //window.location = window.location;

                $("#trip-" + triplogId).css("color", "black");
                $("#trip-" + triplogId).find(".td-jobid").text(jobmainId);

            }).fail((err) => {
                alert("Unable to Link Triplogs and jobs.");
            });

        } else {
            alert("Job Date is not valid for the triplog date. Please check the jobId to verify.");
        }

    } else {
        alert("Job Does not exist.  Please check the jobId to verify.");
    }

}


function GetTripJobLinkId(tripLogId) {
    $.get("/CarRentalLog/GetLinkTriplogJobs", { tripLogId: tripLogId }, (res) => {
        var jobId = res;

        if (jobId != 0) {
            Set_JobLinkForm_JobMainId(jobId);
        } else {
            //clear text field
            $("#LogJobLink-JobMainId").val("");
        }
    })
}

async function CheckTripLogDate() {
    var jobmainId = $("#LogJobLink-JobMainId").val();

    return $.get("/JobOrder/GetActiveJobById", { jobId: jobmainId })
        .then((res) => {
        console.log(res);

        var jobdetails = res;

        var dtStart = moment(jobdetails["JobDateStart"]).format("MM/DD/YYYY");
        var dtEnd = moment(jobdetails["JobDateEnd"]).format("MM/DD/YYYY");

            console.log("dtStart: " + dtStart);
            console.log("dtEnd: " + dtEnd);

        return ConfirmTripDate(dtStart, dtEnd);
    });
}


function RemoveTripJobLink(e, tripId, jobmainId) {
    $.post("/CarRentalLog/DeleteLinkTriplogJobs", { triplogId: tripId, jobmainId, jobmainId }, (res) => {
        console.log(res);
        if (res == "True") {
            //on success, hide modal
            $("#LogJobLinkModal").modal("hide");

            //reload
            //window.location = window.location;
            $(e).parent().parent().parent().parent().parent().css("color", "red");
        }
    })
}

 async function CheckJobExist() {
    var jobmainId = $("#LogJobLink-JobMainId").val();
     return $.get("/JobMains/CheckJobById", { jobmainId: jobmainId })
         .then((res) => {

            console.log(res);

            if ( res == "True") {
                return true;
            }
            return false;
         });
}

function GetTripLinkCount(jobId) {
    return $.get("/CarRentalLog/GetTripIdLinkCountToday", { jobId: jobId }, (res) => {
        var count = res;

        if (count != 0) {
             count;
        } else {
            return 0;
        }
    })
}
