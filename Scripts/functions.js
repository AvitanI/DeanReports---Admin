$('#userToggle').click(function (event) {
    event.stopPropagation();
    $('.userProfile').toggleClass('active');
});

$('#messagesParent').click(function (event) {
    event.stopPropagation();
    var ids = "";
    var length = $('.userMessages').length;
    //console.log("list length: " + $('.userMessages').length);
    $('.userMessages').each(function(index){
        ids += $(this).attr('message-id'); // message id
        if(index < length-1){
            ids += ',';
        }
    });
    //$('.label-success').remove();
    $.post("/member/updatemessages", {ids:ids}).done(function (data) {
        console.log(data);
    });
    $('.messagesDropDown').toggleClass('active');
});

$('.toggleMenu').click(function () {
    $('.innerMenu').slideToggle("slow");
});

$(".fancyTrigger").fancybox();
$(".fancyTrigger").trigger('click');

// combobox for departments
$(".departmentCombo").length && (function () {
    $(".departmentCombo").select2({
        dir: "rtl",
        placeholder: "בחר חוג"
    });
}());

$(".serachReports").length && (function () {
    $(".serachReports").select2({
        dir: "rtl"
    });
}());

$('.jungoSelect2').length && (function () {
    $(".jungoSelect2").each(function () {
        $(this).select2({
            dir: "rtl"
        });
    });
}());

$(".searchMessages").length && $(".searchMessages").select2({
    placeholder: "חיפוש",
    ajax: {
        url: "/Member/SerachMessages",
        dataType: 'json',
        delay: 250,
        data: function (params) {
            console.log(params);
            return {
                query: params.term // search term
            };
        },
        processResults: function (data, params) {
            return {
                results: $.map(data, function (item) {
                    console.log(item);
                    if (item) {
                        return {
                            text: item.Subject + " || " + item.From,
                            id: item.ID
                        }
                    }
                })
            };
        },
        cache: true
    },
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: 1
});

$(".programsCombo").length && (function () {
    $(".programsCombo").select2({
        dir: "rtl"
    });
}());

$(".yearsCombo").length && (function () {
    $(".yearsCombo").select2({
        dir: "rtl",
        placeholder: "שנה אקדמית"
    });
}()); 

$(".genderCombo").length && (function () {
    $(".genderCombo").select2({
        dir: "rtl",
        placeholder: "בחר מגדר"
    });
}());

//$(".membersMessages").length && $(".membersMessages").select2({
//    dir: "rtl"
//});

// date for birth
$(".datepicker").length && (function () {
    $(".datepicker").datepicker({
        changeYear: true,
        yearRange: "-100:+0",
        dateFormat: 'dd/mm/yy'
    });
}());

$(".programs").length && $(".programs").select2({
    dir: "rtl"
});
$(".causes").length && $(".causes").select2({
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

// mark sidebar elements

$(document).ready(function () {
    var path = window.location.pathname; // Returns path only
    $('.sideBar li').removeClass('active');
    var href = "a[href = \'" + path + "\']";
    $(href).parent().addClass('active');
});
//$.datetimepicker.setLocale('he');
$('#startHour').length && $('#endHour').length && jQuery('#startHour, #endHour').datetimepicker({
    datepicker: false,
    format:'H:i',
    allowTimes: ['07:00', '08:00', '09:00', '10:00', '11:00',
                '12:00', '13:00', '14:00', '15:00', '16:00',
                '17:00', '18:00', '19:00', '20:00', '21:00']
    //yearStart: new Date().getFullYear()-1,
    //yearEnd: new Date().getFullYear()
});

$('#birth').length && jQuery('#birth').datetimepicker({
    scrollMonth: false,
    timepicker: false,
    yearStart: 1950,
    yearEnd: 2020,
    format: 'd/m/y',
    placeholder: "בחר משתמש"
});

$('.checkEmail').focusout(function () {
    var username = $(this).val();
    if (username) {
        $.get("/Authentication/IsUserExist?username=" + username).done(function (data) {
            console.log(data);
            if (data.answer == false) {
                $('#markFont').addClass("markRed");
            }
            else {
                $('#markFont').removeClass("markRed");
            }
        });
    }
});

function checkPasswords() {
    var password = $('#password').val();
    var passwordToConfirm = $('#confirmPassword').val();
    if (password != passwordToConfirm) {
        alert("not equal")
        return false;
    }
    return true;
}


//$('#endHour').on('input', function () {
//    if ($('#startHour').val()) {
//        console.log($(this).val());
//    }
//});


//$('.programsCombo').select2("val", "").trigger('change')


$(".requestUsername").hover(function () {
        var username = $(this).text();
        var self = $(this);
        var fName = self.parent().find('.firstName');
        var lName = self.parent().find('.lastName');

        if (fName.text() == "" && lName.text() == "") {
            $.get("/Member/GetMemberDetails?username=" + username).done(function (data) {
                console.log(data);
                self.parent().find('.firstName').text("שם: " + data.firstName);
                self.parent().find('.lastName').text("שם משפחה: " + data.firstName);
                self.next().show();
            });
        }
        else {
            $(this).next().show();
        }
        
    },
    function () {
        $(this).next().hide();
    });

$('.actions').click(function () {
    $(this).toggleClass('active');
    //$(this).next().toggle();
    $(this).next().slideToggle("slow");
});


//$.validator.addMethod("regx", function (value, element, regexpr) {
//    return regexpr.test(value);
//}, "");

//$("#loginForm").length && $("#loginForm").validate({

//    rules: {
//        UserName: {
//            required: true,
//            //change regexp to suit your needs
//            regx: /^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$/i
//        },
//        Password: {
//            required: true,
//            minlength: 4
//        }
//    },
//    messages: {
//        "UserName": {
//            required: "זהו שדה חובה!",
//            regx: "הכנס מייל תקני"
//        },
//        "Password": {
//            required: "זהו שדה חובה!",
//            minlength: "הכנס לפחות 4 תווים!"
//        },
//    },
//    errorPlacement: function (error, element) {
//        error.appendTo(element.next("i").next());
//    },
//    submitHandler: function (form) {
//        return true;
//    }
//})

$('textarea#ckAdmin').length && $('textarea#ckAdmin').ckeditor({
    height: "300px",
    toolbarStartupExpanded: true,
    width: "100%"
});

$(".membersMessages").length && $(".membersMessages").select2({
    ajax: {
        url: "/Member/GetMemberDetails",
        dataType: 'json',
        delay: 250,
        data: function (params) {
            console.log(params);
            return {
                query: params.term// search term
                //page: params.page
            };
        },
        processResults: function (data, params) {
            // parse the results into the format expected by Select2
            // since we are using custom formatting functions we do not need to
            // alter the remote JSON data, except to indicate that infinite
            // scrolling can be used
            //params.page = params.page || 1;
            return {
                results: $.map(data, function (item) {
                    //console.log(item);
                    if (item) {
                        return {
                            text: item.MemberUserName + " || " + item.FirstName + " || " + item.LastName,
                            //slug: item.slug,
                            id: item.MemberUserName
                        }
                    }
                })
            };
        },
        cache: true
    },
    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
    minimumInputLength: 1
    //templateResult: formatRepo, // omitted for brevity, see the source of this page
    //templateSelection: formatRepoSelection // omitted for brevity, see the source of this page
});

$('.remove').click(function () {
    var block = $(this).parent(".blockTitle").parent(".block").parent(".blockWrapper");
    $(block).fadeOut("slow", function () {
        block.remove();
    });
});

$('.plusMinus').click(function () {
    var icon = $(this).children();
    icon.removeClass('fa-minus').addClass('fa-plus').removeClass('fa-plus').addClass('fa-minus')
    $(this).parent(".blockTitle").next(".messageContent").slideToggle();
});

$('.addSession').click(function () {
    var row = " <tr>\
                    <td>-1</td>\
                    <td><input type=\"text\" /></td>\
                    <td><input type=\"text\" value=\"\" name=\"Date\" class=\"birth\" /></td>\
                    <td><input type=\"text\" name=\"StartHour\" id=\"startHour\" class=\"startHour\" /></td>\
                    <td><input type=\"text\" name=\"EndHour\" id=\"endHour\" class=\"endHour\" /></td>\
                    <td><label>0</label></td>\
                    <td><input type=\"text\" name=\"Details\" /></td>\
                    <td><label>false</label></td>\
                    <td><a href=\"/Teacher/DeleteSession?sessionID=-1\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></a></td>\
                <tr>";
    $(this).parent('.refundTableWrapper').find('table').append(row);
    $('.startHour') && $('.endHour') && jQuery('.startHour, .endHour').datetimepicker({
        datepicker: false,
        format: 'H:i',
        allowTimes: ['07:00', '08:00', '09:00', '10:00', '11:00',
                    '12:00', '13:00', '14:00', '15:00', '16:00',
                    '17:00', '18:00', '19:00', '20:00', '21:00']
    });
    $('.birth').length && jQuery('.birth').datetimepicker({
        scrollMonth: false,
        timepicker: false,
        yearStart: 1950,
        yearEnd: 2020,
        format: 'd/m/yy'
    });
});

$('.caruselList').length && $('.caruselList').carouFredSel({
    //direction: "up",
    auto: false,
    responsive: true,
    width: '100%',
    swipe		: {
    	onTouch	: true,
    	onMouse	: true
    },
    prev: $('#prev'),
    next: $('#next'),
    items: {
        height: 'variable',
        visible: 1
    },
    //auto: {
    //    pauseOnHover: true,
    //    duration: 700
    //},
    //pagination: $(".pager",this),
    scroll: 1,
    scroll: {
        onAfter: function (data) {
            var item = data.items.visible[0];
            var src = $(item).find('img').prop('src');
            $('#userImg').val(src);
        }
    	//fx:"crossfade"
    },
    circular: true,
    infinite: true
});

//$('.carouselParagraph').click(function(){
//    console.log("clcickckc");
//});

//var rowCount = $('#myTable tr').length;

function getSumOfHours() {
    // get times from inputs
    var startHour = $('#startHour').val();
    var endHour = $('#endHour').val();
    // calc the diff between hours
    var start = moment(startHour, "HH:mm");
    var end = moment(endHour, "HH:mm");
    var hours = end.diff(start, 'hours');
    // update sum element
    $('#sumHours').val(hours);
}

$("#startHour, #endHour").change(function () {
    var startHour = $('#startHour').val();
    var endHour = $('#endHour').val();

    if (startHour && endHour) {
        getSumOfHours();
    }
});

function setCheckBoxVal() {
    var isChecked = $('#requestSignature:checkbox:checked').length;
    if (isChecked > 0) {
        $('#requestSignature').val("true");
    }
    else {
        $('#requestSignature').val("false");
    }
    return true;
}

$("[tool-tip='true']")
  .mouseenter(function () {
      var element = $(this);
      var elementText = element.attr("tool-tip-text");
      console.log(elementText);
  })
  .mouseleave(function () {
      console.log("leave");
  });

$('#searchReports').click(function () {
    var obj = getReportSearchParams();
    console.log(obj);
    $.get('/Admin/SearchReports', obj).done(function (data) {
        console.log(data);
        $('#table-container').html(data);
    });
});

function getReportSearchParams() {
    var reportObj = {};

    $('.jungoSelect2').each(function () {
        reportObj[$(this).attr('data-type')] = $(this).val();
    });
    return reportObj;
}

$("#refundCourses").select2({
    placeholder: 'בחר קורס',
    ajax: {
        url: '/Teacher/GetCoursesByDepartmentID',
        data: function () {
            //console.log($("#refundDepartment").val());
            return {
                id: $("#refundDepartment").val()
            }
        },
        processResults: function (data) {
            //console.log(data);
            return {
                results: $.map(data, function (item) {
                    if (item) {
                        return {
                            text: item.Name,
                            id: item.ID
                        }
                    }
                })
            };
        }
    }
});