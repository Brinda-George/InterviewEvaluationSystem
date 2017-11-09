function Forgot() {
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "get",
        url: "/Home/PasswordReset",
        success: function (data) {
            $('#myModalContent').html(data);
            $('#myModal').modal(options);
            $('#myModal').modal('show');

        },
        failure: function () {
            alert("Content load failed.");
        }

    });
}

$(function () {
    $("#PasswordValue").click(function () {
        if ($(this).is(':checked')) {
            $("#inputPassword").prop("type", "text");
            $("label[id*=lblPassword]").text("Hide Password");
        }
        else {
            $("#inputPassword").prop("type", "password");
            $("label[id*=lblPassword]").text("Show Password");
        }
    });
});