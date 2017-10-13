//$(document).ready(function () {
//    $('.edit').hide();
//    $('.edit-case').on('click', function () {
//        var tr = $(this).parents('tr:first');
//        var SkillName = tr.find('#skillname').text();
//        tr.find('#SkillName').val(SkillName);
//        tr.find('.edit, .read').toggle();
//    });

    $(document).ready(function () {
        $('.edit').hide();
        $(document).on('click',".edit-case", function () {
            var tr = $(this).parents('tr:first');
            var SkillName = tr.find('#skillname').text();
            tr.find('#SkillName').val(SkillName);
            tr.find('.edit, .read').toggle();
        });

    //$('.update-case').on('click', function (e) {
    //    e.preventDefault();
    //    var tr = $(this).parents('tr:first');
    //    SkillID = $(this).prop('id');
    //    var SkillName = tr.find('#SkillName').val();
    //    $.ajax({
    //        type: "POST",
    //        contentType: "application/json; charset=utf-8",
    //        url: "http://localhost:58074/HR/SkillEdit",
    //        data: JSON.stringify({ "SkillID": SkillID, "SkillName": SkillName }),
    //        dataType: "json",
    //        success: function (data) {
    //            tr.find('.edit, .read').toggle();
    //            $('.edit').hide();
    //            tr.find('#skillname').text(data.SkillName);
    //           // alert('Update success');
    //            window.location = "http://localhost:58074/HR/Skill";
    //        },
    //        error: function (data) {
    //            alert("Error occured during update.");
    //        }
    //    });
    //});

        $(document).on('click',".update-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        SkillID = $(this).prop('id');
        var SkillName = tr.find('#SkillName').val();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:58074/HR/SkillEdit",
            data: JSON.stringify({ "SkillID": SkillID, "SkillName": SkillName }),
            dataType: "json",
            success: function (data) {
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#skillname').text(data.SkillName);
                // alert('Update success');
                window.location = "http://localhost:58074/HR/Skill";
            },
            error: function (data) {
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


        $(document).on('click', ".cancel-case",function (e) {
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
    //        url: "http://localhost:58074/HR/SkillDelete/" + id,
    //        dataType: "json",
    //        success: function (data) {
    //            alert('Delete success');
    //            window.location.href = "http://localhost:58074/HR/Skill";
    //        },
    //        error: function () {
    //            alert('Error occured during delete.');
    //        }
    //    });
    //});

        $(document).on('click', ".delete-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        id = $(this).prop('id');
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:58074/HR/SkillDelete/" + id,
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
