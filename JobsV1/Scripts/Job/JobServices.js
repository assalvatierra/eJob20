/*  
 *  JobServices 
 *  Send Quotation, Send reservation, send invoice
 *  load sending overlay animation / gif
 */



function jobActionDisplay(action) {
    alert(action);
}

//SEND Quotation to client in email
function ajaxSendQuotation(jobId) {
    var data = {
        jobId: jobId,
        docType: "QUOTATION"
    }

    console.log(jobId);
    var serviceURL = '/JobOrder/SendEmailBooking';
    console.log("sending email");

    //loading screen
    LoadOverlay();

    //send email using ajax
    $.ajax({
        type: "POST",
        url: serviceURL,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            console.log('success');
            disableLoadOverlay();
            console.log("email sent");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            $("#overlay2-text").text(jqXHR.responseText);
            disableLoadOverlay();
            console.log("Email Sent");
        }
    });
}

//SEND RESERVATION DETAILS IN EMAIL TO CLIENT
function ajaxSendReservation(jobId) {
    var data = {
        jobId: jobId,
        docType: "RESERVATION"
    }

    console.log(jobId);
    var serviceURL = '/JobOrder/SendEmailBooking';
    console.log("sending email");

    //loading screen
    LoadOverlay();

    //send email using ajax
    $.ajax({
        type: "POST",
        url: serviceURL,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            console.log('success');
            disableLoadOverlay();
            console.log("email sent");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            $("#overlay2-text").text(jqXHR.responseText);
            disableLoadOverlay();
            console.log("Email Sent");
        }
    });
}

function LoadOverlay() {
    console.log("load overlay");
    $("#overlay2").css("display", "flex");
}

function disableLoadOverlay() {
    console.log("disable overlay");
    $("#overlay2").css("display", "none");
}


function ajaxSendMail(jobid) {
    var data = {
        jobId: jobid,
        mailType: "CLIENT-INVOICE-SEND"
    }

    var serviceURL = '/JobOrder/SendEmail';

    //loading screen
    LoadOverlay()

    //send email using ajax
    $.ajax({
        type: "POST",
        url: serviceURL,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            console.log('success');
            disableLoadOverlay();
        },
        error: function (jqXHR, textStatus, errorThrown) {

            alert(jqXHR.responseText);
            disableLoadOverlay();
        }
    });
}
