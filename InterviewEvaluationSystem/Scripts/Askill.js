$(document).ready(function () {
    $('.edit').hide();
    $(document).on('click', ".edit-case", function () {
        var tr = $(this).parents('tr:first');
        var SkillName = tr.find('#skillname').text();
        tr.find('#SkillName').val(SkillName);
        tr.find('.edit, .read').toggle();
    });

    $(document).on('click', ".update-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        SkillID = $(this).prop('id');
        var SkillName = tr.find('#SkillName').val();
        if (SkillName == "") {
            tr.find('#skillLbl').html("Please enter a valid Skill");
            return false;
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/HR/SkillEdit",
            data: JSON.stringify({ "SkillID": SkillID, "SkillName": SkillName }),
            dataType: "json",
            success: function (data) {
                tr.find('#skillLbl').empty();
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#skillname').text(data.SkillName);
                window.location = data.Url;
            },
            error: function (data) {
                alert("Error occured during update.");
            }
        });
    });

    $(document).on('click', ".cancel-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        var id = $(this).prop('id');
        tr.find('.edit, .read').toggle();
        $('.edit').hide();
    });

    $(document).on('click', ".delete-case", function (e) {
        e.preventDefault();
        if (confirm("Are you sure you want to delete")) {
            var tr = $(this).parents('tr:first');
            SkillID = $(this).prop('id');
            $.ajax({
                type: 'POST',
                url: '/HR/SkillDelete/',
                data: { "SkillID": SkillID },
                dataType: "json",
                success: function (data) {
                    alert('Successfully Deleted');
                    window.location.href = data.Url;
                },
                error: function () {
                    alert('Error occured during delete.');
                }
            });
        }
    });
});

function CheckCategory() {
    var cat = $("#categories").val();
    if (cat == 0) {
        $("#lblCategory").html("Please select a category");
        return false;
    }
    else {
        $("#lblCategory").empty();
    }

}