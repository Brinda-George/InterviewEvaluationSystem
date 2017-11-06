$(document).ready(function () {
    $('.edit').hide();
    $(document).on('click', ".edit-case", function () {
        var tr = $(this).parents('tr:first');
        var RoundName = tr.find('#roundname').text();
        tr.find('#RoundName').val(RoundName);
        tr.find('.edit, .read').toggle();
    });

    $(document).on('change', ".cat", function (e) {

        var $current = $(this);
        //$(this).attr('class', 'thiss');
        $current.addClass("thiss");

        $('.cat').each(function () {
            if ($(this).val() == $current.val() && $(this).attr('class') != $current.attr('class')) {
                alert('duplicate found!');
                $current.removeClass("thiss");
                //$current.addClass("edit");
                //$current.addClass("cat");
                $('.update-case').prop('disabled', true);
                location.reload(true);
                return false;
            }
            else {
                $('.update-case').prop('disabled', false);
                //$('.edit').hide();

            }
        });
      
        $current.removeClass("thiss");
        //$current.addClass("edit");
        //$current.addClass("cat");
    });




    $(document).on('click', ".update-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        RoundID = $(this).prop('id');
        var RoundName = tr.find('#RoundName').val();
        if (RoundName == "")
        {
            tr.find('#roundLbl').html("Please enter a valid Round Name");
            return false;
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/HR/RoundEdit",
            data: JSON.stringify({ "RoundID": RoundID, "RoundName": RoundName}),
            dataType: "json",
            success: function (data) {
                tr.find('#roundLbl').empty();
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#roundname').text(data.RoundName);
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



    $(document).on('click', ".delete-case", function (e) {
        e.preventDefault();
        if (confirm("Are you sure you want to delete")) {
            var tr = $(this).parents('tr:first');
            RoundID = $(this).prop('id');
            $.ajax({
                type: 'POST',
                url: '/HR/RoundDelete/',
                data: { "RoundID": RoundID },
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
