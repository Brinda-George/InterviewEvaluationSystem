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
                   
            var tr = $(this).parents('tr:first');
            var UserName = tr.find("#lblInterviewerName").text();
            var date = tr.find('#lblDateOfInterview').text();
            var newdate = date.split("-").reverse().join("-");
            $('.delete-userCandidate').hide();
            tr.find('.edit-modeCandidate, .display-modeCandidate').toggle();
            tr.find('#ddlInterviewerName option:contains(' + UserName + ')').attr('selected', 'selected');
            tr.find('#DateOfInterview').val(newdate);
            
        });
        $('.cancel-userCandidate').on('click', function () {
        
            $('.delete-userCandidate').show();

            var tr = $(this).parents('tr:first');
            tr.find('#lblCandidateNameValidation').html('');
            tr.find('#lblDateOfInterviewValidation').html('');
            tr.find('#lblInterviewerNameValidation').html('');
        });


        $('.save-userCandidate').on('click', function () {
                
            var tr = $(this).parents('tr:first');
            var CandidateName = tr.find("#CandidateName").val();
            var DateOfInterview = tr.find("#DateOfInterview").val();
            //  var UserID = tr.find("#ddlInterviewerName").val();
            
            var flag = 0;
            if (CandidateName == "") {
                tr.find('#lblCandidateNameValidation').html('The Candidate Name Is Required');
                flag = 1;
            }
            else {
                tr.find('#lblCandidateNameValidation').empty();
            }

            if (DateOfInterview == "") {
                tr.find('#lblDateOfInterviewValidation').html('Date Of Interview Is Required');
                flag = 1;
            }
            else {
                tr.find('#lblDateOfInterviewValidation').empty();
            }

            //if (UserID == null) {
            //    tr.find('#lblInterviewerNameValidation').html('Interviewer Is Required');
            //    flag = 1;
            //}
            //else {
            //    tr.find('#lblInterviewerNameValidation').empty();
            //}
            if (flag == 1)
            {
                return false;
            }
            var CandidateID = $(this).prop('id');
            $.ajax({
                url: '/HR/UpdateCandidate/', 
                // data: JSON.stringify(tblNewUser),
                //data: JSON.stringify({ "CandidateID": CandidateID, "CandidateName": CandidateName, "DateOfInterview": DateOfInterview, "UserID": UserID }),
                data: JSON.stringify({ "CandidateID": CandidateID, "CandidateName": CandidateName, "DateOfInterview": DateOfInterview }),
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
            else
            {
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
                    $('#gridContent').html(Name);
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

            var flag = 0;
            if (CandidateName == "") {
                tr.find('#lblCandidateNameValidation').html('The Candidate Name Is Required');
                flag = 1;
            }
            else {
                tr.find('#lblCandidateNameValidation').empty();
            }

            if (DateOfInterview == "") {
                tr.find('#lblDateOfInterviewValidation').html('Date Of Interview Is Required');
                flag = 1;
            }
            else {
                tr.find('#lblDateOfInterviewValidation').empty();
            }

            if (UserID == null) {
                tr.find('#lblInterviewerNameValidation').html('Interviewer Is Required');
                flag = 1;
            }
            else {
                tr.find('#lblInterviewerNameValidation').empty();
            }
            if (flag == 1) {
                return false;
            }
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
            else
            {
                return false;
            }
        });

        
        });
    });


function candidateValidation() {
    //$(document).on("click", "#btnCreate", function () {
    var CandidateName = $('#Name').val();
    if (CandidateName == "") {
        $('#lblCandidateName').html("Candidate Name Required");
        
    }
    else {
        $('#lblCandidateName').empty();
    }
    var Designation = $('#Designation').val();
    if (Designation == "") {
        $('#lblDesignation').html("Designation Required");
        
    }
    else
    {
        $('#lblDesignation').empty();
    }
    var DateOfBirth = $('#DateOfBirth').val();
    if (DateOfBirth == "") {
        $('#lblDOB').html('Date Of Birth Required');
        
    }
    else
    {
        $('#lblDOB').empty();
    }
    var DateOfInterview = $('#DateOfInterview').val();
    if (DateOfInterview == "") {
        $('#lblDOI').html('Date Of Interview Required');
        
    }
    else
    {
        $('#lblDOI').empty();
    }
    var Email = $('#Email').val();
    //var reg = /^(([^<>()[]\.,;:s@"]+(.[^<>()[]\.,;:s@"]+)*)|(".+"))@(([[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}])|(([a-zA-Z-0-9]+.)+[a-zA-Z]{2,}))$/igm;
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(Email))
    {
        $('#lblEmail').empty();
    }
    else 
    {
        $('#lblEmail').html('Enter A Valid Email Address');
        
    }
    var PAN = $('#PAN').val();
    var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
    if (PAN == "" || !regpan.test(PAN))
    {
        $('#lblPAN').html('A Valid PAN Is Required');
        
    }
    else
    {
        $('#lblPAN').empty();
    }

    var noticeperiod = $('#noticeperiod').val();
    if (noticeperiod == "")
    {
        $('#lblNoticePeriod').html('The Notice Period Is Required');
    }
    else
    {
        $('#lblNoticePeriod').empty();
    }
    var Qualifications = $('#Qualifications').val();
    if (Qualifications == "")
    {
        $('#lblQualification').html('Qualification Is Required');
        
    }
    else
    {
        $('#lblQualification').empty();
    }
    var interviewer = $('#ddlUser').val();
    if (interviewer == "")
    {
        $('#lblInterviewer').html('Interviewer Required');
        
    }
    else
    {
        $('#lblInterviewer').empty();
    }
    
    $.ajax({
        url: '/HR/AddCandidate',
        type: 'Post',
        data: $('#frmCreate').serialize(),
        success: function (response) {
            var str = response.roundErrorMessage;
            if (str != "")
            {
                alert(str);
            }
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