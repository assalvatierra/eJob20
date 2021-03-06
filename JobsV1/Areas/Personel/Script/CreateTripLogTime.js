
// Using jquery time picker
$(() => {
    $(".jqtimepicker").timepicker({
        timeFormat: 'h:mm p',
        interval: 30,
        minTime: '12:00 AM',
        maxTime: '11:55 PM',
        //defaultTime: '6:00 AM',
        startTime: '6:00 AM',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
});

$(() => {
    $(".jqtimepicker-end").timepicker({
        timeFormat: 'h:mm p',
        interval: 30,
        minTime: '12:00 AM',
        maxTime: '11:55 PM',
        //defaultTime: '6:00 PM',
        startTime: '6:00 PM',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
});