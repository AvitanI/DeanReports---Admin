$('#userToggle').click(function (event) {
    event.stopPropagation();
    $('.userProfile').toggleClass('active');
});

$(".fancyTrigger").fancybox();
$(".fancyTrigger").trigger('click');

// combobox for departments
$(".departmentCombo").length && (function () {
    $(".departmentCombo").select2({
        // placeholder: "בחר חוג",
        // allowClear: true,
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
    // placeholder: "בחר חוג",
    // allowClear: true,
    dir: "rtl"
});
$(".courses").select2({
    // placeholder: "בחר חוג",
    // allowClear: true,
    dir: "rtl"
});
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
                                    <label for="" id="" name="" class="lecturerNameLbl">שם המרצה</label> \
                                    <input type="text" id="" name="LecturerName" class="lecturerNameTxt" /> \
                                    <select id="' + comboID + '" name="SelectedCourses" class="courses"></select> \
                                    <a href="#" id="removeCourse">הסר<i class="fa fa-times removeIcon" aria-hidden="true"></i></a> \
                                    <div class="clear"></div> \
                              </div> \
                              <div class="clear">');
            initCourseComboByID(comboID);
            $('#'+comboID).select2({
                dir: "rtl"
            });
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
