
// Using jquery time picker
$(() => {
    $(".jqtimepicker").timepicker({
        timeFormat: 'h:mm p',
        interval: 30,
        minTime: '12:00 AM',
        maxTime: '11:55 PM',
        //defaultTime: '8:00 AM',
        startTime: '7:00 AM',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
});