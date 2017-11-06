function Reset() {
    var email = $("#txtEmail").val();
    $.ajax({
        type: "POST",
        url: "/Home/PasswordReset",
        data: { email: email },
        success: function (data) {
            if (data.result == 1) {
                $.post(data.Url, function (partial) {
                    $("#lblEmail").empty();
                    $("#partial").html(partial);
                });

            }
            else if (data.result == 2) {
                $("#lblEmail").html("Email entered is not registered.Please enter a registered email.");
            }
            else {
            }
        },
        error: function (data) {

        }
    })
}