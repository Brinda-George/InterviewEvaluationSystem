$(document).ready(function () {
    $(function () {

        $('.edit-modeCandidate').hide();
        $('.edit-userCandidate, .cancel-userCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var UserName = tr.find("#lblInterviewerName").text();
            tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();
            tr.find('#ddlInterviewerName option:contains(' + UserName + ')').attr('selected', 'selected');
        });
        $('.cancel-userCandidate').on('click', function () {

            var tr = $(this).parents('tr:first');
            tr.find('#lblInterviewerNameValidation').html('');
        });

        $('.save-userCandidate').on('click', function () {

            var tr = $(this).parents('tr:first');
            var UserID = tr.find("#ddlInterviewerName").val();
            var CandidateID = $(this).prop('id');
            $.ajax({
                url: '/HR/EditCandidateInterviewer/',
                data: JSON.stringify({ "CandidateID": CandidateID, "UserID": UserID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    location.reload();
                    alert("Successfully Updated");
                    tr.find("#lblInterviewerName").val(data.UserName);
                }
            });
        });

        $('.searchInterviewer').on('click', function () {
            var tr = $(this).parents('tr:first');
            var InterviewerName = tr.find("#InterviewerNameText").val();
            $.ajax({
                type: "post",
                url: "/HR/SearchInterviewerResult",
                data: { UserName: $('#InterviewerNameText').val() },
                datatype: "json",
                success: function (Name) {
                    $('#gridContentInterviewerResult').html(Name);
                    $('.edit-modeCandidateResult').hide();
                }
            });
        });
    });
});