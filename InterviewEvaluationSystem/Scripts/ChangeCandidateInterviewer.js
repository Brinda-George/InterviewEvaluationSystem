$(document).ready(function () {


    $(function () {

        $('.edit-modeCandidate').hide();
        $('.edit-userCandidate, .cancel-userCandidate').on('click', function () {

            var tr = $(this).parents('tr:first');
            var UserName = tr.find("#lblInterviewerName").text();
            var CandidateID = $(this).prop('id');
            $.ajax({
                url: '/HR/GetCandidateInterviewer/',
                // data: JSON.stringify(tblNewUser),
                data: JSON.stringify({ "CandidateID": CandidateID}),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $('#res').html(data.CandidateID);
                    
                    //tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();
                    //tr.find('#ddlInterviewerName option:contains(' + UserName + ')').attr('selected', 'selected');


                }
            });
            
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
                // data: JSON.stringify(tblNewUser),
                data: JSON.stringify({ "CandidateID": CandidateID, "UserID": UserID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    // alert('Successfully Updated Interviewer');   
                    location.reload();
                    alert("Successfully Updated");
                    //tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();                                        
                    //tr.find("#lblCandidateName").text(data.Name);
                    //tr.find("#lblDateOfInterview").text(data.DateOfInterview);
                    tr.find("#lblInterviewerName").val(data.UserName);


                }
            });

        });

        $('.searchInterviewer').on('click', function () {
            // $(document).on("click", "#searchCandidate", function () {
            
            var InterviewerName = $("#InterviewerNameText").val();
            var flag = 0;
            if (InterviewerName == "")
            {
                $('#lblSearchInterviewer').html('The Name Is Required');
                flag = 1;
            }
            else
            {
                $('#lblSearchInterviewer').empty();
            }
            if (flag == 1)
            {
                return false;
            }
            $.ajax({
                type: "post",
                url: "/HR/SearchInterviewerResult",
                data: { UserName: $('#InterviewerNameText').val()},
                datatype: "json",
                success: function (Name) {
                    $('#gridContentInterviewerResult').html(Name);
                    $('.edit-modeCandidateResult').hide();
                }
            });
        });
      });
});




