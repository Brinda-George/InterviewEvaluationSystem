$(document).ready(function () {
    $(".grid").find("select").attr("disabled", true);
    var roundID = $('#roundId').val();
    $('.class' + roundID).attr("disabled", false);
});
function getValues(element) {
    var roundID = $('#roundId').val();
    var evaluationID = $('#evaluationId').val();
    var buttonclicked = element.id;
    if (buttonclicked == "proceed") {
        var recommended = true;
    }
    else {
        var recommended = false;
    }
    var classname = "class" + roundID;
    var count = $('.' + classname).length;
    var valueArray = [];
    for (var i = 1; i <= count; i++) {
        var itemid = "id" + roundID + i;
        if ($('#' + itemid).find('option:selected').val().length === 0) {
            alert("Please select all rates!!")
            return false;
        }
        else {
            valueArray[i] = $('#' + itemid).find('option:selected').val();
        }
    }
    var evaluationID = evaluationID;
    if ($('#Comments').val().length === 0) {
        alert("Please enter your comments!!")
        return false;
    }
    else if ($('#Comments').val().length > 500) {
        alert("The Comments field cannot contain more than 500 characters!!!")
        return false;
    }
    else {
        var comments = $('#Comments').val();
    }
    $.ajax({
        url: '/Interviewer/InterviewEvaluation',
        type: 'post',
        data: { recommended: recommended, evaluationID: evaluationID, values: valueArray, comments: comments },
        success: function (response) {
            window.location.href = response.Url;
        },
        error: function () {
        }
    });
}