

// Modal - Change Due Date
function InitChangeDueDate(transId, dueDate) {
    $("#DueDate-transId").val(transId);
    $("#DueDate-Old").val(dueDate);
}

function Submit_DueDateChange() {
    var newDueDate = $("#DueDate-New").val();
    var transId = $("#DueDate-transId").val();

    console.log(newDueDate);
    console.log(transId);

    //show loading
    $("#loading-overlay").show();

    var result = $.post("../Receivables/ArMgt/UpdateDueDate",
        {
            transId: transId,
            dueDate: newDueDate
        },
        (response) => {
            console.log(response);
            if (response == "True") {
                $("#ChangeDueDateModal").modal('hide');
                $("#loading-overlay").hide();
                window.location.reload(false);
            } else {

                alert("Unable to Update Due Date.");
                $("#loading-overlay").hide();
            }
        }
    );
    console.log(result);
}

// Modal - Contact
function InitContactModal(transId, AccountMobile, AccountName) {

    var mobileNumber = AccountMobile.trim().replace(/ +/g, "");
    $("#Contact-Mobile").attr("href", "tel:" + mobileNumber);
    $("#Contact-Mobile").text(AccountMobile);
    $("#Contact-Name").val(AccountName);

    var result = $.get("/Receivables/ArMgt/GetTransactionDetails",
        {
            transId: transId
        },
        (response) => {
            console.log(response);
            if (response != "Error") {

                var today = moment();
                var dueDate = moment(response["DtDue"]);
                var amount = response["Amount"].toLocaleString();
                var invoiceId = parseInt(response["InvoiceId"]);
                var reminderMsg = "";

                //to due
                if (today.diff(dueDate, 'days') <= 0) {
                    var reminderMsg = "Hi " + AccountName + ", \n\n"
                        + "This is a reminder that the payment on Invoice # " + invoiceId
                        + " for P" + amount + " is due on " + dueDate.format("MMM DD YYYY (ddd)") + ". \n\n"
                        + "If the payment has already been sent, please ignore this message.\n\n"
                        + " Thanks,\n RealWheels Car Rental Davao";

                } else {
                    var reminderMsg = "Hi " + AccountName + ", \n\n"
                        + "This is a reminder that the payment on Invoice # " + invoiceId
                        + " for P" + amount + " is over due by " + today.diff(dueDate, 'days') + " days. \n\n"
                        + "If the payment has already been made, please ignore this message.\n\n"
                        + " Thanks,\n RealWheels Car Rental Davao";
                }

                //overdue
                $("#Contact-ReminderMgs").val(reminderMsg);

            } else {
                alert("Unable to retrieve account data.");
                //$("#loading-overlay").hide();
            }
        }
    );
}

function CopyHTMLToClipboard() {
    var htmlContent = $('#Contact-ReminderMgs').select();
    // Use try & catch for unsupported browser

    try {
        // The important part (copy selected text)
        var ok = document.execCommand('copy');

        if (ok) {
            alert('Text Copied');
            console.log('Copied');
        } else {
            alert('Unable to copy');
            console.log('Unable to copy');
        };
    } catch (err) {
        alert('Unsupported Browser');
        console.log('Unsupported Browser');
    }
}

//Email Reminder
function InitEmailReminderModal(transId, AccountEmail, AccountName) {
    console.log(AccountEmail);

    $("#EmailReminder-Email").val(AccountEmail);
    $("#EmailReminder-Name").val(AccountName);
    $("#EmailReminder-transId").val(transId);

    var result = $.get("/Receivables/ArMgt/GetTransactionDetails",
        {
            transId: transId
        },
        (response) => {
            console.log(response);
            if (response != "Error") {

                var today = moment();
                var dueDate = moment(response["DtDue"]);
                var amount = response["Amount"].toLocaleString();
                var invoiceId = parseInt(response["InvoiceId"]);
                var reminderMsg = "";

                //to due
                if (today.diff(dueDate, 'days') <= 0) {
                    var reminderMsg = "Hi " + AccountName + ", \n\n"
                        + "This is a reminder that the payment on Invoice # " + invoiceId
                        + " for P" + amount + " is due on " + dueDate.format("MMM DD YYYY (ddd)") + ". \n\n"
                        + "If the payment has already been sent, please ignore this message.\n\n"
                        + " Thanks,\n RealWheels Car Rental Davao";

                } else {
                    var reminderMsg = "Hi " + AccountName + ", \n\n"
                        + "This is a reminder that the payment on Invoice # " + invoiceId
                        + " for P" + amount + " is over due by " + today.diff(dueDate, 'days') + " days. \n\n"
                        + "If the payment has already been made, please ignore this message.\n\n"
                        + " Thanks,\n RealWheels Car Rental Davao";
                }

                //overdue
                $("#EmailReminder-EmailMsg").val(reminderMsg);

            } else {
                alert("Unable to retrieve account data.");
                //$("#loading-overlay").hide();
            }
        }
    );
}

function SendReminderToEmail() {
    console.log("Sending to Email");

    var transId = $("#EmailReminder-transId").val();

    $("#EmailReminderModal").modal('hide');
    $("#overlay").show();

    var email_recipient = $("#EmailReminder-Email").val();
    var email_content = $("#EmailReminder-EmailMsg").val();

    var result = $.post("/Receivables/ArMgt/SendEmailReminder",
        {
            transId: transId,
            recipient: email_recipient,
            emailMessage: email_content
        },
        (response) => {
            console.log("response :" + response);

            if (response == 'True') {
                alert("Email Sent");
            } else {
                alert("Unable to send email to recipient. Please try again later.");
            }
            $("#overlay").hide();
        }
    );
}
