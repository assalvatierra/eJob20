
/**
 *  Unit Reports Filters
 */

/**  Summary Report Filter */

function Submit_VehicleSummaryFilter() {

    var startDate = $("#VehicleSummaryFilter-StartDate").val();
    var endDate = $("#VehicleSummaryFilter-EndDate").val();
    var unitId = $("#VehicleSummaryFilter-unitId").val();

    var url = $("#VehicleSummaryReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_unitId', unitId);
    //console.log(url)
    window.location.href = url;
}

function VehicleSummaryFilter_AddDays(days) {
    $("#VehicleSummaryFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}


/**  Summary Payment Report Filter */

function Submit_Vehicle_PaymentSummaryFilter() {

    var startDate = $("#Vehicle-PaymentSummaryFilter-StartDate").val();
    var endDate = $("#Vehicle-PaymentSummaryFilter-EndDate").val();
    var unitId = $("#Vehicle-PaymentSummaryFilter-unitId").val();

    var url = $("#VehiclePaymentSummaryReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_unitId', unitId);
    //console.log(url)
    window.location.href = url;
}

function Vehicle_PaymentSummaryFilter_AddDays(days) {
    $("#Vehicle-PaymentSummaryFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}


/**  Trip Report Filter */

function Submit_VehicleTripFilter() {

    var startDate = $("#VehicleTripFilter-StartDate").val();
    var endDate = $("#VehicleTripFilter-EndDate").val();
    var driverId = $("#VehicleTripFilter-driverId").val();
    var unitId = $("#VehicleTripFilter-unitId").val();

    var url = $("#VehicleTripReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_driverId', driverId);
    url = url.replace('_unitId', unitId);

    window.location.href = url;
}

function VehicleTripFilter_AddDays(days) {
    $("#VehicleTripFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}


/**  Vehicle Summary Report Filter */
function Initial_ItemVehicleSummaryFilter(rptId, rptName) {
    $("#ItemVehicleSummaryFilter-rptName").val(rptName);
    $("#ItemVehicleSummaryFilter-rptId").val(rptId);
}

function Submit_ItemVehicleSummaryFilter() {

    var startDate = $("#ItemVehicleSummaryFilter-StartDate").val();
    var endDate = $("#ItemVehicleSummaryFilter-EndDate").val();
    var rptId = $("#ItemVehicleSummaryFilter-rptId").val();

    var url = $("#ItemVehicleSummaryReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_rptId', rptId);
    //console.log(url)
    window.location.href = url;
}

function ItemVehicleSummaryFilter_AddDays(days) {
    $("#ItemVehicleSummaryFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}


/** Item Vehicle Payments Summary Report Filter */
function Initial_ItemVehicle_PaymentSummaryFilter(rptId, rptName) {
    $("#ItemVehicle-PaymentSummaryFilter-rptName").val(rptName);
    $("#ItemVehicle-PaymentSummaryFilter-rptId").val(rptId);
}

function Submit_ItemVehicle_PaymentSummaryFilter() {

    var startDate = $("#ItemVehicle-PaymentSummaryFilter-StartDate").val();
    var endDate = $("#ItemVehicle-PaymentSummaryFilter-EndDate").val();
    var rptId = $("#ItemVehicle-PaymentSummaryFilter-rptId").val();

    var url = $("#ItemVehiclePaymentSummaryReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_rptId', rptId);
    //console.log(url)
    window.location.href = url;
}

function ItemVehicle_PaymentSummaryFilter_AddDays(days) {
    $("#ItemVehicle-PaymentSummaryFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}

/**  Vehicle Trip Report Filter */

function Initial_ItemVehicleTripFilter(rptId, rptName) {
    $("#ItemVehicleTripFilter-rptName").val(rptName);
    $("#ItemVehicleTripFilter-rptId").val(rptId);
}

function Submit_ItemVehicleTripFilter() {

    var startDate = $("#ItemVehicleTripFilter-StartDate").val();
    var endDate = $("#ItemVehicleTripFilter-EndDate").val();
    var rptId = $("#ItemVehicleTripFilter-rptId").val();

    var url = $("#ItemVehicleTripReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_rptId', rptId);

    window.location.href = url;
}

function ItemVehicleTripFilter_AddDays(days) {
    $("#ItemVehicleTripFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}



/**  Driver Trip Report Filter */

function Initial_DriverSummaryFilter(rptId, rptName) {
    $("#DriverSummaryFilter-rptName").val(rptName);
    $("#DriverSummaryFilter-rptId").val(rptId);
}

function Submit_DriverSummaryFilter() {

    var startDate = $("#DriverSummaryFilter-StartDate").val();
    var endDate = $("#DriverSummaryFilter-EndDate").val();
    var driverId = $("#DriverSummaryFilter-driverId").val();

    var url = $("#DriverSummaryReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_driverId', driverId);

    window.location.href = url;
}

function DriverSummaryFilter_AddDays(days) {
    $("#DriverSummaryFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}



/** Item Driver Trip Report Filter */

function Initial_ItemDriverSummaryFilter(rptId, rptName) {
    $("#ItemDriverSummaryFilter-rptName").val(rptName);
    $("#ItemDriverSummaryFilter-rptId").val(rptId);
}

function Submit_ItemDriverSummaryFilter() {

    var startDate = $("#ItemDriverSummaryFilter-StartDate").val();
    var endDate = $("#ItemDriverSummaryFilter-EndDate").val();
    var rptId = $("#ItemDriverSummaryFilter-rptId").val();

    var url = $("#ItemDriverSummaryReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_rptId', rptId);

    window.location.href = url;
}

function ItemDriverSummaryFilter_AddDays(days) {
    $("#ItemDriverSummaryFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}



/** Item Driver Trip Report Filter */

function Initial_ItemVehicleExpenseFilter(rptId, rptName) {
    $("#ItemVehicle-VehicleExpenseFilter-rptName").val(rptName);
    $("#ItemVehicle-VehicleExpenseFilter-rptId").val(rptId);
}

function Submit_ItemVehicle_VehicleExpenseFilter() {

    var startDate = $("#ItemVehicle-VehicleExpenseFilter-StartDate").val();
    var endDate = $("#ItemVehicle-VehicleExpenseFilter-EndDate").val();
    var rptId = $("#ItemVehicle-VehicleExpenseFilter-rptId").val();
    var typeId = $("#ItemVehicle-VehicleExpenseFilter-typeId").val();

    var url = $("#ItemVehicleExpenseReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_rptId', rptId);
    url = url.replace('_typeId', typeId);

    window.location.href = url;
}

function ItemVehicle_VehicleExpenseFilter_AddDays(days) {
    $("#ItemVehicle-VehicleExpenseFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}


/**  PO Report Filter */

function Initial_POSummaryFilter(rptId, rptName) {
    $("#POSummaryFilter-rptName").val(rptName);
    $("#POSummaryFilter-rptId").val(rptId);
}

function Submit_POSummaryFilter() {

    var startDate = $("#POSummaryFilter-StartDate").val();
    var endDate = $("#POSummaryFilter-EndDate").val();
    var driverId = $("#POSummaryFilter-driverId").val();

    var url = $("#POSummaryReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_driverId', driverId);

    window.location.href = url;
}

function POSummaryFilter_AddDays(days) {
    $("#POSummaryFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}


/** Item PO Report Filter */

function Initial_ItemPOSummaryFilter(rptId, rptName) {
    $("#ItemPOSummaryFilter-rptName").val(rptName);
    $("#ItemPOSummaryFilter-rptId").val(rptId);
}

function Submit_ItemPOSummaryFilter() {

    var startDate = $("#ItemPOSummaryFilter-StartDate").val();
    var endDate = $("#ItemPOSummaryFilter-EndDate").val();
    var rptId = $("#ItemPOSummaryFilter-rptId").val();
    var typeId = $("#ItemPOSummaryFilter-typeId").val();

    var url = $("#ItemPOSummaryReport").val();
    url = url.replace('_startDate', startDate);
    url = url.replace('_endDate', endDate);
    url = url.replace('_rptId', rptId);
    url = url.replace('_typeId', typeId);

    window.location.href = url;
}

function ItemVehicle_POSummaryFilter_AddDays(days) {
    $("#ItemPOSummaryFilter-StartDate").val(moment().add(days, 'days').format("MM/DD/YYYY"));
}
