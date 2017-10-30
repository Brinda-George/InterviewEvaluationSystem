$(document).ready(function () {
    $(function () {
       
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
                }
            });

        });

        $('.RejectCandidate').on('click', function () {
            var tr = $(this).parents('tr:first');
            var CandidateID = tr.find("#lblCandidateID").text();
            $.ajax({
                url: '/HR/RejectCandidate/',
                data: JSON.stringify({ "CandidateID": CandidateID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    alert('Candidate Rejected');
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
    });
});
