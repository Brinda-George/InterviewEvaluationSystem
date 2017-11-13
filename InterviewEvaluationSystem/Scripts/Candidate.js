$(document).ready(function () {
    $(function () {
        $('.edit-modeCandidate').hide();
        $('.edit-userCandidate, .cancel-userCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var dateofbirth = tr.find('#lblDateOfBirth').text();
            var date = tr.find('#lblDateOfInterview').text();
            var newdate = date.split("/").reverse().join("-");
            var newdob = dateofbirth.split("/").reverse().join("-");
            tr.find('#DateOfInterview').val(newdate);
            tr.find('#DateOfBirth').val(newdob);
            $('.delete-userCandidate').hide();
        });

        $('.cancel-userCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            tr.find('#lblCandidateNameValidation').html('');
            tr.find('#lblEmailValidation').html('');
            tr.find('#lblDateOfBirthValidation').html('');
            tr.find('#lblPANValidation').html('');
            tr.find('#lblDesignationValidation').html('');
            tr.find('#lblDateOfInterviewValidation').html('');
            tr.find('#lblTotalExperienceValidation').html('');
            tr.find('#lblQualificationsValidation').html('');
            $('.delete-userCandidate').show();
        });

        $('.save-userCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateName = tr.find("#CandidateName").val();
            var email = tr.find('#Email').val();
            var dateofbirth = tr.find('#DateOfBirth').val();
            var pan = tr.find('#PAN').val();
            var designation = tr.find('#Designation').val();
            var experience = tr.find('#TotalExperience').val();
            var qualifications = tr.find('#Qualifications').val();
            var DateOfInterview = tr.find("#DateOfInterview").val();
            var CandidateID = $(this).prop('id');
            var flag = 0;
            if (CandidateName == "") {
                tr.find('#lblCandidateNameValidation').html('The Candidate Name field is required');
                flag = 1;
            }
            else {
                tr.find('#lblCandidateNameValidation').empty();
            }
            if (email == "") {
                tr.find('#lblEmailValidation').html('The Email field is required');
                flag = 1;
            }
            else {
                tr.find('#lblEmailValidation').empty();
            }
            if (dateofbirth == "") {
                tr.find('#lblDateOfBirthValidation').html('The Date Of Birth field is required');
                flag = 1;
            }
            else {
                tr.find('#lblDateOfBirthValidation').empty();
            }
            if (pan == "") {
                tr.find('#lblPANValidation').html('The PAN field is required');
                flag = 1;
            }
            else {
                tr.find('#lblPANValidation').empty();
            }
            if (designation == "") {
                tr.find('#lblDesignationValidation').html('The Designation field is required');
                flag = 1;
            }
            else {
                tr.find('#lblDesignationValidation').empty();
            }
            if (experience == "") {
                tr.find('#lblTotalExperienceValidation').html('The Total Experience field is required');
                flag = 1;
            }
            else {
                tr.find('#lblTotalExperienceValidation').empty();
            }
            if (qualifications == "") {
                tr.find('#lblQualificationsValidation').html('The Qualifications field is required');
                flag = 1;
            }
            else {
                tr.find('#lblQualificationsValidation').empty();
            }
            if (DateOfInterview == "") {
                tr.find('#lblDateOfInterviewValidation').html('The Date Of Interview field is required');
                flag = 1;
            }
            else {
                tr.find('#lblDateOfInterviewValidation').empty();
            }
            if (flag == 1) {
                return false;
            }
            $.ajax({
                url: '/HR/UpdateCandidate/',
                data: JSON.stringify({ "CandidateID": CandidateID, "CandidateName": CandidateName, "DateOfInterview": DateOfInterview, "email": email, "dateofbirth": dateofbirth, "pan": pan, "designation": designation, "experience": experience, "qualifications": qualifications }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    location.reload();
                    alert("Successfully Updated");
                    $('.delete-userCandidate').show();

                }
            });

        });
        $('.delete-userCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateID = $(this).prop('id');
            var flag = confirm('Are you sure you want to delete?');
            if (flag) {
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
            }
            else {
                return false;
            }
        });

        $('.searchCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateName = tr.find("#CandidateNameText").val();
            $.ajax({
                type: "post",
                url: "/HR/SearchCandidateResult",
                data: { Name: CandidateName },
                datatype: "json",
                success: function (Name) {
                    $('#gridContent').html(Name);
                    $('.edit-modeCandidateResult').hide();
                }
            });
        });

        $(document).on("click", ".edit-userCandidateResult, .cancel-userCandidateResult", function () {
            var tr = $(this).parents('tr:first');
            var dateofbirth = tr.find('#lblDateOfBirth').text();
            var date = tr.find('#lblDateOfInterview').text();
            tr.find('.edit-modeCandidateResult, .display-modeCandidateResult').toggle();
            var newdob = dateofbirth.split("/").reverse().join("-");
            var newdate = date.split("/").reverse().join("-");
            tr.find('#DateOfBirth').val(newdob);
            tr.find('#DateOfInterview').val(newdate);
            $('.delete-userCandidateResult').hide();
        });

        $(document).on("click", ".cancel-userCandidateResult", function () {
            var tr = $(this).parents('tr:first');
            tr.find('#lblCandidateNameValidation').html('');
            tr.find('#lblEmailValidation').html('');
            tr.find('#lblDateOfBirthValidation').html('');
            tr.find('#lblPANValidation').html('');
            tr.find('#lblDesignationValidation').html('');
            tr.find('#lblDateOfInterviewValidation').html('');
            tr.find('#lblTotalExperienceValidation').html('');
            tr.find('#lblQualificationsValidation').html('');
            $('.delete-userCandidateResult').show();
        });

        $(document).on("click", ".save-userCandidateResult", function () {
            var tr = $(this).parents('tr:first');
            var CandidateName = tr.find("#CandidateName").val();
            var email = tr.find('#Email').val();
            var dateofbirth = tr.find('#DateOfBirth').val();
            var pan = tr.find('#PAN').val();
            var designation = tr.find('#Designation').val();
            var experience = tr.find('#TotalExperience').val();
            var qualifications = tr.find('#Qualifications').val();
            var DateOfInterview = tr.find("#DateOfInterview").val();
            var CandidateID = $(this).prop('id');
            var flag = 0;
            if (CandidateName == "") {
                tr.find('#lblCandidateNameValidation').html('The Candidate Name field is required');
                flag = 1;
            }
            else {
                tr.find('#lblCandidateNameValidation').empty();
            }
            if (email == "") {
                tr.find('#lblEmailValidation').html('The Email field is required');
                flag = 1;
            }
            else {
                tr.find('#lblEmailValidation').empty();
            }
            if (dateofbirth == "") {
                tr.find('#lblDateOfBirthValidation').html('The Date Of Birth field is required');
                flag = 1;
            }
            else {
                tr.find('#lblDateOfBirthValidation').empty();
            }
            if (pan == "") {
                tr.find('#lblPANValidation').html('The PAN field is required');
                flag = 1;
            }
            else {
                tr.find('#lblPANValidation').empty();
            }
            if (designation == "") {
                tr.find('#lblDesignationValidation').html('The Designation field is required');
                flag = 1;
            }
            else {
                tr.find('#lblDesignationValidation').empty();
            }
            if (experience == "") {
                tr.find('#lblTotalExperienceValidation').html('The Total Experience field is required');
                flag = 1;
            }
            else {
                tr.find('#lblTotalExperienceValidation').empty();
            }
            if (qualifications == "") {
                tr.find('#lblQualificationsValidation').html('The Qualifications field is required');
                flag = 1;
            }
            else {
                tr.find('#lblQualificationsValidation').empty();
            }
            if (DateOfInterview == "") {
                tr.find('#lblDateOfInterviewValidation').html('The Date Of Interview field is required');
                flag = 1;
            }
            else {
                tr.find('#lblDateOfInterviewValidation').empty();
            }
            if (flag == 1) {
                return false;
            }
            $.ajax({
                url: '/HR/UpdateCandidate/',
                data: JSON.stringify({ "CandidateID": CandidateID, "CandidateName": CandidateName, "DateOfInterview": DateOfInterview, "email": email, "dateofbirth": dateofbirth, "pan": pan, "designation": designation, "experience": experience, "qualifications": qualifications }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    location.reload();
                    alert("Successfully Updated");
                    $('.delete-userCandidateResult').show();
                }
            });
        });

        $(document).on("click", ".delete-userCandidateResult", function () {
            var tr = $(this).parents('tr:first');
            var CandidateID = $(this).prop('id');
            var flag = confirm('Are you sure you want to delete?');
            if (flag) {
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
            }
            else {
                return false;
            }
        });


    });
});


function candidateValidation() {
    var CandidateName = $('#Name').val();
    var Designation = $('#Designation').val();
    var DateOfBirth = $('#DateOfBirth').val();
    var DateOfInterview = $('#DateOfInterview').val();
    var Email = $('#Email').val();
    var PAN = $('#PAN').val();
    var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
    var noticeperiod = $('#noticeperiod').val();
    var Qualifications = $('#Qualifications').val();
    var interviewer = $('#ddlUser').val();
    var flag = 0;
    if (CandidateName == "") {
        $('#lblCandidateName').html("The Candidate Name field is required.");
        flag = 1;
    }
    else {
        $('#lblCandidateName').empty();
    }
    if (Designation == "") {
        $('#lblDesignation').html("The Designation field is required.");
        flag = 1;
    }
    else {
        $('#lblDesignation').empty();
    }
    if (DateOfBirth == "") {
        $('#lblDOB').html('The Date Of Birth field is required.');
        flag = 1;
    }
    else {
        $('#lblDOB').empty();
    }
    if (DateOfInterview == "") {
        $('#lblDOI').html('The Date Of Interview field is required.');
        flag = 1;
    }
    else {
        $('#lblDOI').empty();
    }
    if (!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(Email))) {
        $('#lblEmail').html('A valid Email is required.');
        flag = 1;
    }
    else {
        $('#lblEmail').empty();
    }
    if (PAN == "" || !regpan.test(PAN)) {
        $('#lblPAN').html('A valid PAN is Required');
        flag = 1;
    }
    else {
        $('#lblPAN').empty();
    }
    if (noticeperiod == "") {
        $('#lblNoticePeriod').html('The Notice Period is required');
        flag = 1;
    }
    else {
        $('#lblNoticePeriod').empty();
    }
    if (Qualifications == "") {
        $('#lblQualification').html('The Qualification field is required.');
        flag = 1;
    }
    else {
        $('#lblQualification').empty();
    }
    if (interviewer == "") {
        $('#lblInterviewer').html('The Interviewer field is required.');
        flag = 1;
    }
    else {
        $('#lblInterviewer').empty();
    }
    if (flag == 1) {
        return false;
    }
    else {
        $.ajax({
            url: '/HR/AddCandidate',
            type: 'Post',
            data: $('#frmCreate').serialize(),
            success: function (response) {
                var str = response.roundErrorMessage;
                if (str != "") {
                    alert(str);
                }
                alert("Candidate Added and Mail sent to interviewer Successfully!!");
                window.location.href = response.Url;
            },
            error: function (data) {
            }
        });
    }
}

function GetExistingDynamicTextBoxes(value) {
    var noticeperiod = $('#noticeperiod').val();
    var totalexp = $('#totalexperience').val();
    if (totalexp == "0" || noticeperiod == "") {
        alert("Give values for total experience and notice period");
        return false;
    }
    else {
        var div = $(" <div />");

        var textBox = $(" <input />").attr("type", "textbox").attr("name", "txtBoxes");
        textBox.addClass('form-control');
        textBox.val(value);
        div.append(textBox);

        var button = $(" <input />").attr("type", "button").attr("value", "Remove");
        button.attr("onclick", "DeleteTextBox(this)");
        button.addClass('removeBtn');
        button.addClass('btn btn-default');
        div.append(button);

        return div;
    }
}
function AppendTextBox() {
    var div = GetExistingDynamicTextBoxes("");
    $("#divTextBoxes").append(div);
}

function DeleteTextBox(button) {
    $(button).parent().remove();
}