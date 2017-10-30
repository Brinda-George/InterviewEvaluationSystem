$(document).ready(function () {
   

    $(function () {
        $('.edit-modeCandidate').hide();
        $('.edit-userCandidate, .cancel-userCandidate').on('click', function () {
        //$(document).on("click", ".edit-userCandidate, .cancel-userCandidate", function () {
            
            var tr = $(this).parents('tr:first');
            var UserName = tr.find("#lblInterviewerName").text();
            tr.find('#ddlInterviewerName').val(UserName);
            $('.delete-userCandidate').hide();
            tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();
            
        });
        $('.cancel-userCandidate').on('click', function () {
        //$(document).on("click", ".cancel-userCandidate", function () {
                    $('.delete-userCandidate').show();
        });


        $('.save-userCandidate').on('click', function () {
        //$(document).on("click", ".save-userCandidate", function () {
        
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
        $('.delete-userCandidate').on('click', function () {
        //$(document).on("click", ".delete-userCandidate", function () {
        
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

        $('.searchCandidate').on('click', function () {
       // $(document).on("click", "#searchCandidate", function () {
        
            var tr = $(this).parents('tr:first');
            var CandidateName = tr.find("#CandidateNameText").val();
            
            $.ajax({
                type: "post",
                url: "/HR/SearchCandidateResult",
                data: { Name: $('#CandidateNameText').val() },
                datatype: "json",
                success: function (Name) {
                    $('#gridContentCandidateResult').html(Name);
                    $('.edit-modeCandidateResult').hide();
                }
            });
        });

       
       // $('.edit-userCandidateResult, .cancel-userCandidateResult').on('click', function () {
            $(document).on("click", ".edit-userCandidateResult, .cancel-userCandidateResult", function () {

            var tr = $(this).parents('tr:first');
            var UserName = tr.find("#lblInterviewerName").text();
            tr.find('#ddlInterviewerName').val(UserName);
            $('.delete-userCandidateResult').hide();
            tr.find('.edit-modeCandidateResult, .display-modeCandidateResult').toggle();

            });

        //$('.cancel-userCandidateResult').on('click', function () {
            $(document).on("click", ".cancel-userCandidateResult", function () {
            $('.delete-userCandidateResult').show();
        });


        //$('.save-userCandidateResult').on('click', function () {
            $(document).on("click", ".save-userCandidateResult", function () {

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
                    $('.delete-userCandidateResult').show();

                }
            });

        });
        //$('.delete-userCandidateResult').on('click', function () {
            $(document).on("click", ".delete-userCandidateResult", function () {

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

        
        });
    });


function candidateValidation() {
    //$(document).on("click", "#btnCreate", function () {
    var CandidateName = $('#Name').val();
    if (CandidateName == "") {
        $('#lblCandidateName').html("Candidate Name Required");
        return false;
    }
    var Designation = $('Designation').val();
    if (Designation == "") {
        $('#lblDesignation').html("Designation Required");
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
}