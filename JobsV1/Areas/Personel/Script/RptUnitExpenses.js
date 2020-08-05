﻿
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
