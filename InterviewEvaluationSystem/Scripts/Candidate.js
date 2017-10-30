$(document).ready(function () {
    $(function () {
        $('.edit-modeCandidate').hide();

        $(document).on("click", ".edit-userCandidate, .cancel-userCandidate", function () {

            var tr = $(this).parents('tr:first');

            var UserName = tr.find("#lblInterviewerName").text();
            //   $('#ddlInterviewerName').get(0).selectedIndex = UserName;
            //  tr.find('#ddlInterviewerName').text(UserName);
            tr.find('#ddlInterviewerName').val(UserName);
            //   $('#ddlInterviewerName').text("UserName");
            //  $('#ddlInterviewerName').val(UserName).attr("selected", "selected");
            //  $("#ddlInterviewerName").find("option[val='UserName']").attr("selected", "selected");
            // $('#ddlInterviewerName option[value="UserName"]').attr("selected", true);
            // $('#ddlInterviewerName').val($("#ddlInterviewerName option:contains('UserName')").val());
            //  $("#ddlInterviewerName option[text='UserName']").attr("selected", "selected");
            $('#ddlInterviewerName').val(UserName).change();
            $('#ddlInterviewerName option:contains("UserName")').prop('selected', true);
            tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();
        });


        $(document).on("click", ".save-userCandidate", function () {

            var tr = $(this).parents('tr:first');
            var CandidateName = tr.find("#CandidateName").val();
            var DateOfInterview = tr.find("#DateOfInterview").val();
            var UserID = tr.find("#ddlInterviewerName").val();
            var CandidateID = tr.find("#lblCandidateID").html();
            $.ajax({
                url: '/HR/UpdateCandidate/',
                // data: JSON.stringify(tblNewUser),
                data: JSON.stringify({ "CandidateID": CandidateID, "CandidateName": CandidateName, "DateOfInterview": DateOfInterview, "UserID": UserID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    // alert('Successfully Updated Interviewer');   
                    location.reload();
                    //tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();                                        
                    //tr.find("#lblCandidateName").text(data.Name);
                    //tr.find("#lblDateOfInterview").text(data.DateOfInterview);
                    //tr.find("#lblInterviewerName").text(data.UserName);

                }
            });

        });

        $(document).on("click", ".delete-userCandidate", function () {

            var tr = $(this).parents('tr:first');
            var CandidateID = tr.find("#lblCandidateID").html();
            $.ajax({
                url: '/HR/DeleteCandidate/',
                data: JSON.stringify({ "CandidateID": CandidateID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    location.reload();
                }
            })
        });

        $(document).on("click", "#searchCandidate", function () {

            var tr = $(this).parents('tr:first');
            var CandidateName = tr.find("#CandidateName").val();
            debugger;
            $.ajax({
                type: "post",
                url: "/HR/SearchCandidateResult",
                data: { Name: $('#CandidateNameText').val() },
                datatype: "json",
                success: function (Name) {
                    $('#gridContentCandidate').html(Name);
                }
            });
        });

        $(document).on("click", "#btnCreate", function () {

            $.ajax({
                url: '/HR/AddCandidate',
                type: 'Post',
                data: $('#frmCreate').serialize(),
                success: function (response) {
                    window.location.href = response.Url;
                },
                error: function (data) {
                }
            });
        });
    });
});

function GetExistingDynamicTextBoxes(value) {
    var div = $(" <div />");

    var textBox = $(" <input />").attr("type", "textbox").attr("name", "txtBoxes");
    textBox.addClass('form-control');
    textBox.val(value);
    div.append(textBox);

    var button = $(" <input />").attr("type", "button").attr("value", "Remove");
    button.attr("onclick", "DeleteTextBox(this)");
    button.addClass('btn btn-default');
    div.append(button);

    return div;
}
function AppendTextBox() {
    var div = GetExistingDynamicTextBoxes("");
    $("#divTextBoxes").append(div);
}

function DeleteTextBox(button) {
    $(button).parent().remove();
}

$(function () {
    var values = eval('@Html.Raw(ViewBag.DemoMessage)');
    if (values != null) {
        $("#divTextBoxes").html("");
        $(values).each(function () {
            $("#divTextBoxes").append(GetExistingDynamicTextBoxes(this));
        });
    }
});