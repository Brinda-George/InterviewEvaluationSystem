
    $(document).ready(function () {
        $('.edit').hide();
        $(document).on('click',".edit-case", function () {
            var tr = $(this).parents('tr:first');
            var SkillName = tr.find('#skillname').text();
            tr.find('#SkillName').val(SkillName);
            tr.find('.edit, .read').toggle();
        });

        $(document).on('click',".update-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        SkillID = $(this).prop('id');
        var SkillName = tr.find('#SkillName').val();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/HR/SkillEdit",
            data: JSON.stringify({ "SkillID": SkillID, "SkillName": SkillName }),
            dataType: "json",
            success: function (data) {
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#skillname').text(data.SkillName);
                alert('Successfully updated');
                window.location = data.Url;
            },
            error: function (data) {
                alert("Error occured during update.");
            }
        });
        });

        $(document).on('click', ".cancel-case",function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        var id = $(this).prop('id');
        tr.find('.edit, .read').toggle();
        $('.edit').hide();
    });

        $(document).on('click', ".delete-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        SkillID = $(this).prop('id');
        $.ajax({
            type: 'POST',
            //contentType: "application/json; charset=utf-8",
            url: '/HR/SkillDelete/',
            data:{ "SkillID": SkillID },
            dataType: "json",
            success: function (data) {
                alert('Successfully deleted');
                window.location.href = data.Url;
            },
            error: function () {
                alert('Error occured during delete.');
            }
        });
    });

});
