
/**
 * CrLogJobLink
 * Desc: Link Triplogs and Jobs by triplogId & jobmainId
 */

function Show_JobsLinkModal(triplogid, tripDate) {
    $("#LogJobLinkModal").modal("show");

    Set_JobLinkForm_TripLogId(triplogid);
    Set_JobLinkForm_TripLogDate(tripDate);

    GetActiveJobs();

    //get existing triploglink for edit/modify link
    GetTripJobLinkId(triplogid);
}


function GetActiveJobs() {

    $("#Loading-jobs").show();

    //empty list
    $("#LogJobSearchModal-List").empty();

    $.get("/JobOrder/GetActiveJobList", null, (res) => {
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

            //console.log(dtStart);
            //console.log(dtEnd);
            //console.log("----------");

            var item = "<button type='button' class='list-group-item' onclick='SelectJob(" + itemId + ","
                + noItems + ",\"" + dtStart + "\",\"" + dtEnd + "\")'>" +
                itemId + ' - ' + company + ' ' + customer + "<br>" +
                dtStart + " - " + dtEnd +
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

    if (tripdate < jobDtStart || tripdate > jobDtEnd) {

        if (confirm("Trip date is not within the Job date. Do you want to continue?")) {
            return true;
        } else {
            return false;
        }
    }
}

function ConfirmLinkCount(noItems, linkCount) {

    if (linkCount >= noItems) {

        if (confirm("Unit allocation for the job is " + linkCount +
            " of " + noItems + ". Do you want to continue?")) {
            return true;
        } else {
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

function Submit_JobLinkForm() {
    var triplogId = $("#LogJobLink-TripLogId").val();
    var jobmainId = $("#LogJobLink-JobMainId").val();

    $.post("/CarRentalLog/SetLinkTriplogJobs", { "triplogId": triplogId, "jobmainId": jobmainId }, (res) => {
      
    }).done((res) => {

            //on success, hide modal
            $("#LogJobLinkModal").modal("hide");

            //reload
            //window.location = window.location;

        $("#trip-" + triplogId).css("color","black");
        $("#trip-" + triplogId).find(".td-jobid").text(jobmainId);
        
    }).fail((err) => {
       alert("Unable to Link Triplogs and jobs.");
    });
      
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

//function RemoveTripJobLink(tripId, jobmainId) {
//    $.post("/CarRentalLog/DeleteLinkTriplogJobs", { triplogId: tripId, jobmainId, jobmainId }, (res) => {
       
//        if (res == "True") {
//            //on success, hide modal
//            $("#LogJobLinkModal").modal("hide");

//            //reload
//            //window.location = window.location;
//        }
//    })
//}


function RemoveTripJobLink(e, tripId, jobmainId) {
    $.post("/CarRentalLog/DeleteLinkTriplogJobs", { triplogId: tripId, jobmainId, jobmainId }, (res) => {
        console.log(res);
        if (res == "True") {
            //on success, hide modal
            $("#LogJobLinkModal").modal("hide");

            //reload
            //window.location = window.location;
            $(e).parent().parent().parent().parent().parent().css("color", "red");
            //$(e).parent().parent().parent().parent().Remove();
        }
    })
}

 function  CheckJobExist() {
    var jobmainId = $("#LogJobLink-JobMainId").val();
    $.get("/JobMains/CheckJobById", { jobmainId: jobmainId }, (res) => {
       
    }).done(( res) => {
        if ( res == "True") {
            return true;
        }
        return false;
    });
}

function GetTripLinkCount(jobId) {
    return $.get("/CarRentalLog/GetTripIdLinkCountToday", { jobId: jobId }, (res) => {
        var count = res;
        //console.log(count);
        if (count != 0) {
             count;
        } else {
            return 0;
        }
    })
}
