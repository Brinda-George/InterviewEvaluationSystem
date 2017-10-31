$(document).ready(function () {


    $(function () {

        //$("#WebGridCandidate th:nth-child(1)").hide();
        //$("#WebGridCandidate td:nth-child(1)").hide();

        //hideColumn = function (column) {
        //    $('tr').each(function () {
        //        $('#WebGridCandidate').find('td,th').eq(column).hide();
        //    });
        //};
        //hideColumn(0);

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
            var CandidateID = $(this).prop('id');
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
            var CandidateID = $(this).prop('id');
            var flag = confirm('Do you want to delete the record');
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
            // $(document).on("click", "#searchCandidate", function () {

            var tr = $(this).parents('tr:first');
            var CandidateName = tr.find("#CandidateNameText").val();

            $.ajax({
                type: "post",
                url: "/HR/SearchCandidateResult",
                data: { Name: $('#CandidateNameText').val() },
                datatype: "json",
                success: function (Name) {
                    $('#gridContentCandidate').html(Name);
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
            var CandidateID = $(this).prop('id');
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
            var CandidateID = $(this).prop('id');
            var flag = confirm('Do you want to delete the record');
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
    if (CandidateName == "") {
        $('#lblCandidateName').html("The Candidate Name field is required.");
    }
    else {
        $('#lblCandidateName').empty();
    }
    var Designation = $('#Designation').val();
    if (Designation == "") {
        $('#lblDesignation').html("The Designation field is required.");
    }
    else {
        $('#lblDesignation').empty();
    }
    var DateOfBirth = $('#DateOfBirth').val();
    if (DateOfBirth == "") {
        $('#lblDOB').html('The Date Of Birth field is required.');
    }
    else {
        $('#lblDOB').empty();
    }
    var DateOfInterview = $('#DateOfInterview').val();
    if (DateOfInterview == "") {
        $('#lblDOI').html('The Date Of Interview field is required.');
    }
    else {
        $('#lblDOI').empty();
    }
    var Email = $('#Email').val();
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(Email)) {
        $('#lblEmail').empty();
    }
    else {
        $('#lblEmail').html('A valid Email is required.');
    }
    var PAN = $('#PAN').val();
    var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
    if (PAN == "" || !regpan.test(PAN)) {
        $('#lblPAN').html('A valid PAN is Required');
    }
    else {
        $('#lblPAN').empty();
    }
    if (noticeperiod == "") {
        $('#lblNoticePeriod').html('The Notice Period Is Required');
    }
    else {
        $('#lblNoticePeriod').empty();
    }
    var Qualifications = $('#Qualifications').val();
    if (Qualifications == "") {
        $('#lblQualification').html('The Qualification field is required.');
    }
    else {
        $('#lblQualification').empty();
    }
    var interviewer = $('#ddlUser').val();
    if (interviewer == "") {
        $('#lblInterviewer').html('The Interviewer field is required.');
    }
    else {
        $('#lblInterviewer').empty();
    }
    return false;
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