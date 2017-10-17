$(document).ready(function () {
    $(function () {

        $('#SearchCandidateResultContent').hide();
        $('.edit-modeCandidate').hide();
        $('.edit-userCandidate, .cancel-userCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();
        });

        $('.save-userCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateName = tr.find("#CandidateName").val();
            var DateOfInterview = tr.find("#DateOfInterview").val();
            var UserName = tr.find("#InterviewerName").val();
            var CandidateID = tr.find("#lblCandidateID").html();
            $.ajax({
                url: '/HR/UpdateCandidate/',
                // data: JSON.stringify(tblNewUser),
                data: JSON.stringify({ "CandidateID": CandidateID, "CandidateName": CandidateName, "DateOfInterview": DateOfInterview, "UserName": UserName }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    alert('Successfully Updated Interviewer');
                    tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();
                    debugger;
                    tr.find("#lblCandidateName").text(data.Name);
                    tr.find("#lblDateOfInterview").text(data.DateOfInterview);
                    tr.find("#lblInterviewerName").text(data.UserName);
                }
            });

        });

        $('.delete-userCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateID = tr.find("#lblCandidateID").html();
            $.ajax({
                url: '/HR/DeleteCandidate/',
                data: JSON.stringify({ "CandidateID": CandidateID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    alert('Successfully Deleted Candidate');
                }
            })
        });

        $('#searchCandidate').on('click', function () {
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
    });
});
