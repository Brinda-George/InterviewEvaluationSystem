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