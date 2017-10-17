$(document).ready(function () {
    $(function () {
       

        $('.ProceedNext').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateID = tr.find("#lblCandidateID").text();
            var Name = tr.find("#lblCandidateName").text();
            var Email = tr.find("#lblCandidateEmail").text();
            var RoundID = tr.find("#lblCandidateRound").text();
          //  var EmployeeId = tr.find("#lblEmployeeId").html();
            
            $.ajax({
                url: '/HR/ProceedCandidate/',
                data: JSON.stringify({"CandidateID":CandidateID,"Name": Name, "Email": Email, "RoundID": RoundID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $('#ProceedCandidateDiv').html(data);
                    //('Successfully Updated Interviewer');
                    //tr.find('.edit-mode, .display-mode').toggle();
                    //tr.find("#lblName").val(data.Name);
                    //tr.find("#lblDesignation").val(data.Designation);
                    //tr.find("#lblEmail").val(data.Email);
                }
            });

        });

        

    });
});
