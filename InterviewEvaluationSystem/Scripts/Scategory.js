//$(document).ready(function () {
//    $('.edit').hide();
//    $('.edit-case').on('click', function () {
//        var tr = $(this).parents('tr:first');
//        var SkillCategory = tr.find('#skillcategory').text();
//        var Description = tr.find('#description').text();
//        tr.find('#SkillCategory').val(SkillCategory);
//        tr.find('#Description').val(Description);
//        tr.find('.edit, .read').toggle();
//    });

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


    //$('.update-case').on('click', function (e) {
    //    e.preventDefault();
    //    var tr = $(this).parents('tr:first');
    //    SkillCategoryID = $(this).prop('id');
    //    var SkillCategory = tr.find('#SkillCategory').val();
    //    var Description = tr.find('#Description').val();
    //    $.ajax({
    //        type: "POST",
    //        contentType: "application/json; charset=utf-8",
    //        url: "http://localhost:58074/HR/CategoryEdit",
    //        data: JSON.stringify({ "SkillCategoryID": SkillCategoryID, "SkillCategory": SkillCategory,"Description": Description }),
    //        dataType: "json",
    //        success: function (data) {
    //            tr.find('.edit, .read').toggle();
    //            $('.edit').hide();
    //            tr.find('#skillcategory').text(data.SkillCategory);
    //            tr.find('#description').text(data.Description);
    //            window.location = "http://localhost:58074/HR/SkillCategory";
    //        },
    //        error: function (err) {
    //            alert("Error occured during update.");
    //        }
    //    });
    //});

    $(document).on('click',".update-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        SkillCategoryID = $(this).prop('id');
        var SkillCategory = tr.find('#SkillCategory').val();
        var Description = tr.find('#Description').val();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:58074/HR/CategoryEdit",
            data: JSON.stringify({ "SkillCategoryID": SkillCategoryID, "SkillCategory": SkillCategory, "Description": Description }),
            dataType: "json",
            success: function (data) {
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#skillcategory').text(data.SkillCategory);
                tr.find('#description').text(data.Description);
                window.location = "http://localhost:58074/HR/SkillCategory";
            },
            error: function (err) {
                alert("Error occured during update.");
            }
        });
    });
  

    //$('.cancel-case').on('click', function (e) {
    //    e.preventDefault();
    //    var tr = $(this).parents('tr:first');
    //    var id = $(this).prop('id');
    //    tr.find('.edit, .read').toggle();
    //    $('.edit').hide();
    //});

    $(document).on('click', ".cancel-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        var id = $(this).prop('id');
        tr.find('.edit, .read').toggle();
        $('.edit').hide();
    });

    //$('.delete-case').on('click', function (e) {
    //    e.preventDefault();
    //    var tr = $(this).parents('tr:first');
    //    id = $(this).prop('id');
    //    $.ajax({
    //        type: 'POST',
    //        contentType: "application/json; charset=utf-8",
    //        url: "http://localhost:58074/HR/CategoryDelete/" + id,
    //        dataType: "json",
    //        success: function (data) {
    //            alert('Delete success');
    //            window.location.href = "http://localhost:58074/HR/SkillCategory";
    //        },
    //        error: function () {
    //            alert('Error occured during delete.');
    //        }
    //    });
    //});

    $(document).on('click',".delete-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        id = $(this).prop('id');
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:58074/HR/CategoryDelete/" + id,
            dataType: "json",
            success: function (data) {
                alert('Delete success');
                window.location.href = "http://localhost:58074/HR/SkillCategory";
            },
            error: function () {
                alert('Error occured during delete.');
            }
        });
    });
});
