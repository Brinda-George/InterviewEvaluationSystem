$(document).ready(function () {

    // Disable all drop downs
    $(".grid").find("select").attr("disabled", true);

    // Get current round id
    var roundID = $('#roundId').val();
    var recommend = $('#recommended').val();

    // Enable drop downs for current round
    if (recommend == "null") {
        $('.class' + roundID).attr("disabled", false);
    }
});

function getValues(element) {
    var roundID = $('#roundId').val();
    var evaluationID = $('#evaluationId').val();

    // Get id of button clicked
    var buttonclicked = element.id;
    if (buttonclicked == "proceed") {
        var recommended = true;
    }
    else {
        var recommended = false;
    }

    var valueArray = [];
    var idArray = [];

    // Get skills of current round
    var skills = $('select[id^="id' + roundID + '"]');
    for (var i = 0; i < skills.length; i++) {
        var itemid = $(skills[i]).attr('id');
        var commonid = "id" + roundID;

        // Check if all drop downs are selected
        if ($('#' + itemid).find('option:selected').val().length === 0) {
            alert("Please select all rates!!")
            return false;
        }
        else {
            // Get id of particular skill
            idArray[i] = itemid.replace(commonid, '');

            // Get rate scale value selected for particular skill
            valueArray[i] = $('#' + itemid).find('option:selected').val();
        }
    }

    // Check if comment is given
    if ($('#Comments').val().length === 0) {
        alert("Please enter your comments!!")
        return false;
    }
    // Check if comment length is greater than 1000 characters
    else if ($('#Comments').val().length > 1000) {
        alert("The Comments field cannot contain more than 1000 characters!!!")
        return false;
    }
    else {
        // Get comments
        var comments = $('#Comments').val();
    }

    // Submit values to post method to enter to db
    $.ajax({
        url: '/HR/HREvaluation',
        type: 'post',
        data: { recommended: recommended, evaluationID: evaluationID, ids: idArray, values: valueArray, comments: comments },
        success: function (response) {

            // Redirect to response url
            window.location.href = response.Url;
        },
        error: function () {
        }
    });
}


// Print contents of div
function printDiv() {
    $(".divContainer").print();
}