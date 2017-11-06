
    $(document).ready(function () {
        $('.edit').hide();
        $(document).on('click',".edit-case", function () {
            var tr = $(this).parents('tr:first');
            var SkillName = tr.find('#skillname').text();
            var SkillCategory = tr.find('#skillcategory').text();
            tr.find('#SkillName').val(SkillName);
            tr.find('#categories').val(SkillCategory);
            tr.find('#categories option:contains(' + SkillCategory + ')').attr('selected', 'selected');
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
        SkillID = $(this).prop('id');
        var SkillName = tr.find('#SkillName').val();
        var CategoryID = tr.find('#categories').val();
        if (SkillName == "")
        {
            tr.find('#skillLbl').html("Please enter a valid Skill");
            return false;
        }
        if (CategoryID == "")
        {
            tr.find('#catLbl').html("Please select a Skill Category");
            return false;
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/HR/SkillEdit",
            data: JSON.stringify({ "SkillID": SkillID, "SkillName": SkillName, "CategoryID": CategoryID }),
            dataType: "json",
            success: function (data) {
                tr.find('#catLbl').empty();
                tr.find('#skillLbl').empty();
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#skillname').text(data.SkillName);
                tr.find('#skillcategory').text(data.SkillCategory);
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
            if (confirm("Are you sure you want to delete?")) {
                var tr = $(this).parents('tr:first');
                SkillID = $(this).prop('id');
                $.ajax({
                    type: 'POST',
                    //contentType: "application/json; charset=utf-8",
                    url: '/HR/SkillDelete/',
                    data: { "SkillID": SkillID },
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
