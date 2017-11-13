$(document).ready(function () {
    $(function () {
        var length = $('#ddlinterviewers').children('option').length;

        $('.HireCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateID = tr.find("#lblCandidateID").text();
         
            $.ajax({
                url: '/HR/HireCandidate/',
                data: JSON.stringify({ "CandidateID": CandidateID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    alert('Candidate Hired');
                    location.reload();
                }
            });

        });
        $('.RejectCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateID = tr.find("#lblCandidateID").text();
            
            $.ajax({
                url: '/HR/RejectCandidate/',
                data: JSON.stringify({ "CandidateID": CandidateID}),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    alert('Candidate Rejected');
                    location.reload();
                }
            });

        });


        $('.ProceedNext').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateID = tr.find("#lblCandidateID").text();
            var Name = tr.find("#lblCandidateName").text();
            var Email = tr.find("#lblCandidateEmail").text();
            var RoundID = tr.find("#lblCandidateRound").text();
                      
            $.ajax({
                url: '/HR/ProceedCandidate/',
                data: JSON.stringify({"CandidateID":CandidateID,"Name": Name, "Email": Email, "RoundID": RoundID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $('#ProceedCandidateDiv').html(data);
                 }
            });

        });
        $('#ddlinterviewers').change(function () {
            var value = $("#ddlinterviewers").val();
            $.ajax({
                url: '/HR/GetMaxRoundValue',
                type: 'get',
                success: function (result) {
                    if (result.maxRound == value) {
                        
                        $("#DoctorList").append($('<option></option>').val(0).html("--select--"))
                        $.each(result, function (i, doc) {
                            $("#DoctorList").append($('<option></option>').val(doc.Id).html(doc.Name))
                        })
                    }
                }
            });
        });

    });
});


    
    
 
