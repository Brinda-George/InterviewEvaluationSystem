$(document).ready(function () {
    $('.edit').hide();
    $('.edit-case').on('click', function () {
        var tr = $(this).parents('tr:first');
        var RoundName = tr.find('#roundname').text();
        tr.find('#RoundName').val(RoundName);
        tr.find('.edit, .read').toggle();
    });

    $('.update-case').on('click', function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        RoundID = $(this).prop('id');
        var RoundName = tr.find('#RoundName').val();
        if (RoundName == "") {
            tr.find('#roundLbl').html("The Round Name field is required");
            return false;
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/HR/RoundEdit",
            data: JSON.stringify({ "RoundID": RoundID, "RoundName": RoundName }),
            dataType: "json",
            success: function (data) {
                tr.find('#roundLbl').empty();
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#roundname').text(data.RoundName);
                window.location = data.Url;
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
        tr.find('#roundLbl').empty();
    });

    $('.delete-case').on('click', function (e) {
        e.preventDefault();
        if (confirm("Are you sure you want to delete?")) {
            var tr = $(this).parents('tr:first');
            RoundID = $(this).prop('id');
            $.ajax({
                type: 'POST',
                url: '/HR/RoundDelete/',
                data: { "RoundID": RoundID },
                dataType: "json",
                success: function (data) {
                    if (data.res == 1) {
                        alert('Successfully Deleted!!');
                    }
                    else if (data.res == 0) {
                        alert('Cannot Delete!!..Please Delete from bottom');
                    }
                    window.location.href = data.Url;
                },
                error: function () {
                    alert('Error occured during delete.');
                }
            });
        }
    });

});