
// Using jquery time picker

$(() => {
    $(".jqtimepicker").timepicker({
        timeFormat: 'h:mm p',
        interval: 5,
        minTime: '12:00 AM',
        maxTime: '11:55 PM',
        //defaultTime: '8:00 AM',
        startTime: '12:00 AM',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
});