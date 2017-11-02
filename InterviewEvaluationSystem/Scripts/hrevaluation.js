$(document).ready(function () {
    $(".grid").find("select").attr("disabled", true);
    var roundID = $('#roundId').val();
    var recommend = $('#recommended').val();
    if (recommend == "null") {
        $('.class' + roundID).attr("disabled", false);
    }
});
function getValues(element) {
    var roundID = $('#roundId').val();
    var evaluationID = $('#evaluationId').val();
    var buttonclicked = element.id;
    if (buttonclicked == "hire") {
        var recommended = true;
    }
    else {
        var recommended = false;
    }
    var count = $('.class' + roundID).length;
    var valueArray = [];
    for (var i = 1; i <= count; i++) {
        var itemid = "id" + roundID + i;
        if ($('#' + itemid).find('option:selected').val().length === 0) {
            alert("Please select a rate!!")
            return false;
        }
        else {
            valueArray[i] = $('#' + itemid).find('option:selected').val();
        }
    }
    if ($('#Comments').val().length === 0) {
        alert("Please enter your comments!!")
        return false;
    }
    else if ($('#Comments').val().length > 250) {
        alert("The Comments field cannot contain more than 500 characters!!!")
        return false;
    }
    else {
        var comments = $('#Comments').val();
    }
    $.ajax({
        url: '/HR/HREvaluation',
        type: 'post',
        data: { recommended: recommended, evaluationID: evaluationID, values: valueArray, comments: comments },
        success: function (response) {
            window.location.href = response.Url;
        },
        error: function () {
        }
    });
}

function printDiv() {
    $(".divContainer").print();
}