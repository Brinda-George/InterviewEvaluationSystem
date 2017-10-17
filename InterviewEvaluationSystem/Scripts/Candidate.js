$(document).ready(function () {
    $(function () {
        $('#SearchCandidateResultContent').hide();
        $('.edit-modeCandidate').hide();

        $(document).on("click", ".edit-userCandidate, .cancel-userCandidate", function () {
        
            var tr = $(this).parents('tr:first');
            tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();
        });


        $(document).on("click", ".save-userCandidate", function () {
        
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

        $(document).on("click", ".delete-userCandidate", function () {
        
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
                success: function(response){
                    window.location.href = response.Url;
                },
                error: function(data){
                }
            });
        });
    });
});