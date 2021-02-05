
// Payables Details 
// Handle Accepting Payment status

function AcceptPayment(id, transId) {
    $.post("/Payables/ApTransactions/AcceptPayment", { paymentId: id, transId: transId }, (result) => {
        console.log(result);
        if (result == "True") {
            window.location.reload(false);
        } else {
            console.log("Unable to accept payment");
        }
    });
}