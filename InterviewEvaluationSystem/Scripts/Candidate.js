$(document).ready(function () {
   

    $(function () {
        $('.edit-modeCandidate').hide();

        $(document).on("click", ".edit-userCandidate, .cancel-userCandidate", function () {
            
            var tr = $(this).parents('tr:first');
            var UserName = tr.find("#lblInterviewerName").text();
            tr.find('#ddlInterviewerName').val(UserName);
            tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();
            $('.delete-userCandidate').hide();
        });

        $(document).on("click", ".cancel-userCandidate", function () {
                    $('.delete-userCandidate').show();
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
                    alert("Successfully Updated");
                    //tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();                                        
                    //tr.find("#lblCandidateName").text(data.Name);
                    //tr.find("#lblDateOfInterview").text(data.DateOfInterview);
                    //tr.find("#lblInterviewerName").text(data.UserName);
                    $('.delete-userCandidate').show();
                    
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
                    alert("Successfully Deleted");
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
            var CandidateName = $('#Name').val();
            if (CandidateName == "")
            {
                $('#lblCandidateName').html("Candidate Name Required");
            }
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