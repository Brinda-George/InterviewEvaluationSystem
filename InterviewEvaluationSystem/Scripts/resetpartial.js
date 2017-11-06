function Check() {
    var value = $('#otpText').val();
    var otp = $("#otpHidden").val();
    if (value == "") {
        $("#lblOtp").html("Please enter the OTP");
        return false;
    }
    if (otp != value) {
        $("#lblOtp").html("Please enter valid otp");
        return false;
    }
    $.ajax({
        type: "POST",
        url: "/Home/CheckOtp",
        data: { value: value },
        success: function (data) {
            window.location = data.Url;
        },
        error: function (data) {
        }
    });
}