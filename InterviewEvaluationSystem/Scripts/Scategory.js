
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

    $('.cat').change(function () {

        var $current = $(this);
        $(this).attr('class', 'thiss');

        $('.cat').each(function () {
            if ($(this).val() == $current.val() && $(this).attr('class') != $current.attr('class')) {
                alert('duplicate found!');
                $current.removeClass("thiss");
                //$current.addClass("edit");
                //$current.addClass("cat");
                $('.update-case').prop('disabled', true);
                return false;
            }
            else {
                $('.update-case').prop('disabled', false);
                $current.addClass("edit");
                $current.addClass("cat");
                //$('.edit').hide();

            }
        });
    });


    $(document).on('click',".update-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        SkillCategoryID = $(this).prop('id');
        var SkillCategory = tr.find('#SkillCategory').val();
        var Description = tr.find('#Description').val();
        if (SkillCategory == "")
        {
            tr.find('#catLbl').html("Please enter a valid Skill Category");
            return false;
        }
        if (Description == "")
        {
            tr.find('#desLbl').html("Please enter a valid description");
            return false;
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/HR/CategoryEdit",
            data: JSON.stringify({ "SkillCategoryID": SkillCategoryID, "SkillCategory": SkillCategory, "Description": Description }),
            dataType: "json",
            success: function (data) {
                tr.find('#catLbl').empty();
                tr.find('#desLbl').empty();
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#skillcategory').text(data.SkillCategory);
                tr.find('#description').text(data.Description);
                alert('Successfully updated');
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
        if (confirm("Are you sure you want to delete?")) {
            var tr = $(this).parents('tr:first');
            SkillCategoryID = $(this).prop('id');
            $.ajax({
                type: 'POST',
                // contentType: "application/json; charset=utf-8",
                url: '/HR/CategoryDelete/',
                data: { "SkillCategoryID": SkillCategoryID },
                dataType: "json",
                success: function (data) {
                    alert('Successfully deleted');
                    window.location.href = data.Url;
                },
                error: function () {
                    alert('Error occured during delete.');
                }
            });
        }
    });
});
