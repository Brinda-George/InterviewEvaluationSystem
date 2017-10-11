$(document).ready(function () {
    $('.edit').hide();
    $('.edit-case').on('click', function () {
        var tr = $(this).parents('tr:first');
       var SkillType = tr.find('#skillcategory').text();
       var SkillName = tr.find('#skillname').text();
       var SkillName = tr.find('#skillname').text();
       // tr.find('#SkillCategory').val(SkillType);
        tr.find('#SkillName').val(SkillName);
        tr.find('.edit, .read').toggle();
    });
    $('.update-case').on('click', function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        SkillID = $(this).prop('id');
      //  var SkillCategoryID = tr.find('#SkillName').val();
        var SkillName = tr.find('#SkillName').val();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:58074/HR/SkillEdit",
            data: JSON.stringify({ "SkillID": SkillID, "SkillName": SkillName}),
            dataType: "json",
            success: function (data) {
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#skillname').text(data.SkillName);
                window.location = "http://localhost:58074/HR/Skill";
            },
            error: function (err) {
                alert("Error occured during update.");
            }
        });
    });
    $('.cancel-case').on('click', function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        var id = $(this).prop('id');
        tr.find('.edit, .read').toggle();
        $('.edit').hide();
    });
    $('.delete-case').on('click', function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        id = $(this).prop('id');
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:58074/HR/SkillDelete" + id,
            dataType: "json",
            success: function (data) {
                alert('Delete success');
                window.location.href = "http://localhost:58074/HR/Skill";
            },
            error: function () {
                alert('Error occured during delete.');
            }
        });
    });
});
