function Reset() {
        var email = $("#txtEmail").val();
        $.ajax({
            type: "POST",
            url: "/Home/PasswordReset",
            data: { email: email },
            success: function (data) {
                if (data.result == 1) {
                    $.post(data.Url, function (partial) {
                        $("#partial").html(partial);
                    });
                }
            },
            error: function (data) {
            }
        })
    }