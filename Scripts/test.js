$.validator.addMethod("regx", function (value, element, regexpr) {
    return regexpr.test(value);
}, "");

$("#loginForm").length && $("#loginForm").validate({

    rules: {
        UserName: {
            required: true,
            //change regexp to suit your needs
            regx: /^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$/i
        },
        Password: {
            required: true,
            minlength: 4
        }
    },
    messages: {
        "UserName": {
            required: "זהו שדה חובה!",
            regx: "הכנס מייל תקני"
        },
        "Password": {
            required: "זהו שדה חובה!",
            minlength: "הכנס לפחות 4 תווים!"
        },
    },
    errorPlacement: function (error, element) {
        error.appendTo(element.next("i").next());
    },
    submitHandler: function (form) {
        return true;
    }
})