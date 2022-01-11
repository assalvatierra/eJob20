
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
        //console.log(res.length)

        $("#Loading-jobs").hide();
        if (res.length != 0) {
            for (var i = 0; i < res.length; i++) {
                var itemId = res[i]["Id"];
                var customer = res[i]["Customer"];
                var company = res[i]["Company"] == null ? " " : res[i]["Company"] + " / ";
                var dtStart = moment(res[i]["JobDateStart"]).format("MM/DD/YYYY");
                var dtEnd = moment(res[i]["JobDateEnd"]).format("MM/DD/YYYY");
                var noItems = res[i]["NoItems"];

                //console.log(dtStart);
                //console.log(dtEnd);
                //console.log("----------");

                var item = "<button type='button' class='list-group-item' onclick='SelectJobMainId(" + itemId + "," + noItems + ",\"" + dtStart +"\",\""+ dtEnd +"\")'>" +
                    itemId + ' - ' + company + ' ' + customer + "<br>" +
                    dtStart + " - " + dtEnd +

                '</button>';

                $("#LogJobSearchModal-List").append(item);
            }
        }
    });
}

async function SelectJobMainId(jobId,  noItems, dtStart, dtEnd) {
    var jobDtStart = moment(dtStart);
    var jobDtEnd = moment(dtEnd);
    var tripdate = moment($("#LogJobLink-TripLogDate").val());
    var triplinkedCount = await GetTripLinkCount(jobId);
    var PostFlag = true;

    //to remove
    //console.log("tripdat: " + tripdate);
    //console.log("DtStart: " + jobDtStart);
    //console.log("DtEnd:" + jobDtEnd);

    //console.log("tripdate < jobDtStart: " + (tripdate < jobDtStart));
    //console.log("tripdate > jobDtEnd: " + (tripdate > jobDtEnd));

    //to remove
    //console.log(triplinkedCount);
    //console.log("triplinkedCount >= noItems: " + (triplinkedCount >= noItems));

    Set_JobLinkForm_JobMainId(jobId);

    if ( tripdate < jobDtStart || tripdate > jobDtEnd) {

        if (confirm("Trip date is not within the Job date. Do you want to continue?")) {
            //select and submit
            PostFlag = true;
        } else {
            PostFlag = false;
        }
    }



    if (triplinkedCount >= noItems) {

        if (confirm("TripLog items connected to this Job is now " + triplinkedCount + " of " + noItems + ". Do you want to continue?")) {
            //select and submit
            PostFlag = true;
        } else {
            PostFlag = false; 
        }
    } 

    //console.log("POSTFLAG: " + PostFlag);
    if (PostFlag) {
        //select and submit
        Submit_JobLinkForm();
    }

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
                //console.log(res);
                if (res == "True") {
                    //alert("Linked Triplogs and jobs DONE.");

                    //on success, hide modal
                    $("#LogJobLinkModal").modal("hide");

                    //reload
                    window.location = window.location;
                }
            }).fail((err) => {
                    alert("Unable to Link Triplogs and jobs.");
                });
      
}


function GetTripJobLinkId(tripLogId) {
    $.get("/CarRentalLog/GetLinkTriplogJobs", { tripLogId: tripLogId }, (res) => {
        var jobId = res;
        //console.log("existing jobmainId: " + res);
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
        //console.log(res);
        if (res == "True") {
            //on success, hide modal
            $("#LogJobLinkModal").modal("hide");

            //reload
            window.location = window.location;
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
