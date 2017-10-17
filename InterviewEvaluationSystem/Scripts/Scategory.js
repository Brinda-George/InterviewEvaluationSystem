
$(document).ready(function () {
    $('.edit').hide();
    $(document).on('click',".edit-case", function () {
        var tr = $(this).parents('tr:first');
        var SkillCategory = tr.find('#skillcategory').text();
        var Description = tr.find('#description').text();
        tr.find('#SkillCategory').val(SkillCategory);
        tr.find('#Description').val(Description);
        tr.find('.edit, .read').toggle();
        });


    $(document).on('click',".update-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        SkillCategoryID = $(this).prop('id');
        var SkillCategory = tr.find('#SkillCategory').val();
        var Description = tr.find('#Description').val();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/HR/CategoryEdit",
            data: JSON.stringify({ "SkillCategoryID": SkillCategoryID, "SkillCategory": SkillCategory, "Description": Description }),
            dataType: "json",
            success: function (data) {
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#skillcategory').text(data.SkillCategory);
                tr.find('#description').text(data.Description);
                window.location = data.Url;
            },
            error: function (err) {
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

    $(document).on('click',".delete-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        SkillCategoryID = $(this).prop('id');
        $.ajax({
            type: 'POST',
           // contentType: "application/json; charset=utf-8",
            url: '/HR/CategoryDelete/',
            data: { "SkillCategoryID": SkillCategoryID },
            dataType: "json",
            success: function (data) {
                alert('Delete success');
                window.location.href = data.Url;
            },
            error: function () {
                alert('Error occured during delete.');
            }
        });
    });
});
