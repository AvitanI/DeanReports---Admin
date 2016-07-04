$('#userToggle').click(function (event) {
    event.stopPropagation();
    $('.userProfile').toggleClass('active');
});

$(".fancyTrigger").fancybox();
$(".fancyTrigger").trigger('click');

// combobox for departments
$(".departmentCombo").length && (function () {
    $(".departmentCombo").select2({
        dir: "rtl"
    });
}());

// date for birth
$(".datepicker").length && (function () {
    $(".datepicker").datepicker({
        changeYear: true,
        yearRange: "-100:+0"
    });
}());

$(".programs").select2({
    dir: "rtl"
});
$(".courses").select2({
    dir: "rtl"
});
// add class to courses select2
$(".courses").length && (function () {
    $(".courses").next().addClass("coursesContainer");
}());

// add dynamic course request
$(document).ready(function () {
    var max_fields = 10;
    var wrapper = $(".requestedCourses");
    var add_button = $("#addCourse");

    var x = 1;
    $(add_button).click(function (e) {
        e.preventDefault();
        if (x < max_fields) {
            x++;
            var comboID = 'combo' + x;
            $(wrapper).append('<div class="courseWrapper"> \
                                    <label for="" id="" name="" class="requestLbl">שם הקורס</label> \
                                    <select id="' + comboID + '" name="SelectedCourses" class="courses"></select> \
                                    <label for="" id="" name="" class="requestLbl">שם המרצה</label> \
                                    <input type="text" id="" name="LecturerName" class="requestInputs" /> \
                                    <button type="button" id="removeCourse">הסר<i class="fa fa-times removeIcon" aria-hidden="true"></i></button> \
                                    <div class="clear"></div> \
                              </div> \
                              <div class="clear">');
            initCourseComboByID(comboID);
            $('#'+comboID).select2({
                dir: "rtl"
            });
            $('#' + comboID).next().addClass("coursesContainer");
        }
    });

    $(wrapper).on("click", "#removeCourse", function (e) {
        e.preventDefault();
        $(this).parent('div').next().remove()
        $(this).parent('div').remove();
        x--;
    })
});
// initialize combo of courses
function initCourseComboByID(comboID) {
    $.each(coursesArr, function (i, item) {
        $('#'+comboID).append($('<option>', {
            value: item.id,
            text: item.name
        }));
    });
}

$(document).ready(function () {
    var path = window.location.pathname; // Returns path only
    $('.sideBar li').removeClass();
    var href = "a[href = \'" + path + "\']";
    $(href).parent().addClass('active');
});


// set time picker for sessions
$('#startHour').timepicker(
	$.timepicker.regional['he']
);
$("#startHour").length && (function () {
    $("#startHour").datetimepicker({
        dateFormat: '',
        timeFormat: 'hh:mm tt',
        timeOnly: true
    });
}());

$("#endHour").length && (function () {
    $("#endHour").datetimepicker({
        dateFormat: '',
        timeFormat: 'hh:mm tt',
        timeOnly: true
    });
}());

//$('#endHour').on('input', function () {
//    if ($('#startHour').val()) {
//        console.log($(this).val());
//    }
//});
